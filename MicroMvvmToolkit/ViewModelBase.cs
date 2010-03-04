//-----------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="Charlie Robbins">
//     Copyright (c) Charlie Robbins.  All rights reserved.
// </copyright>
// <license>
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// </license>
// <summary>Contains the ViewModelBase class.</summary>
//-----------------------------------------------------------------------
        
namespace MicroMvvmToolkit
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using MicroMvvmToolkit.Behaviors;
    using Microsoft.Practices.Composite.Presentation.Commands;

    /// <summary>
    /// A base class for all ViewModels
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged, INotifyVisualStateChanged
    {
        #region Fields 

        /// <summary>
        /// Backstore for storing all the commands in the viewmodel.
        /// </summary>
        private readonly List<ICommand> commands = new List<ICommand>();

        /// <summary>
        /// The current visual state the ViewModel is in.
        /// </summary>
        private string currentState;

        #endregion Fields 

        #region Events 

        /// <summary>
        /// Event provided by the Interface implementation of 
        /// INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event provided by the interface implementation of 
        /// IVisualStateManager interface.
        /// </summary>
        public event EventHandler<NotifyVisualStateChangedEventArgs> VisualStateChanged;

        #endregion Events 

        #region Properties 

        /// <summary>
        /// Gets or sets the current visual state the ViewModel is in.
        /// </summary>
        /// <value>The current visual state the ViewModel is in..</value>
        public string CurrentState
        {
            get 
            { 
                return this.currentState; 
            }

            set
            {
                if (this.currentState != value)
                {
                    this.currentState = value;
                    this.RaisePropertyChanged(() => this.CurrentState);
                    this.RaiseVisualStateChanged(this.currentState);
                }
            }
        }

        #endregion Properties 

        #region Methods 

        /// <summary>
        /// The create command.
        /// </summary>
        /// <param name="execute">
        /// The execute.
        /// </param>
        /// <typeparam name="TPayLoad">
        /// Type of payload which will be used by command.
        /// </typeparam>
        /// <returns>
        /// The delegate command for the given action and predicate.
        /// </returns>
        public DelegateCommand<TPayLoad> CreateCommand<TPayLoad>(Action<TPayLoad> execute)
        {
            return this.CreateCommand<TPayLoad>(execute, null);
        }

        /// <summary>
        /// The create command.
        /// </summary>
        /// <param name="execute">
        /// The execute.
        /// </param>
        /// <param name="canExecute">
        /// The can execute.
        /// </param>
        /// <typeparam name="TPayLoad">
        /// Type of payload which will be used by command.
        /// </typeparam>
        /// <returns>
        /// The delegate command for the given action and predicate.
        /// </returns>
        public DelegateCommand<TPayLoad> CreateCommand<TPayLoad>(Action<TPayLoad> execute, Func<TPayLoad, bool> canExecute)
        {
            var command = new DelegateCommand<TPayLoad>(execute, canExecute);
            this.OnCommandChanged(command);
            return command;
        }

        /// <summary>
        /// The raise property changed.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        protected virtual void RaisePropertyChanged(Expression<Func<object>> expression)
        {
            if (this.PropertyChanged == null)
            {
                return;
            }

            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;
            this.RaisePropertyChanged(propertyInfo.Name);
        }

        /// <summary>
        /// Virtual method which calls the PropertyChanged event hides the 
        /// logic for checking if there is any subscriber available for 
        /// the event.
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property which is changed.
        /// </param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            Debug.Assert(!String.IsNullOrEmpty(propertyName), "The property named provided must be a non-empty string.");

            if (String.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            if (this.PropertyChanged == null)
            {
                return;
            }

            PropertyChangedEventHandler handler = this.PropertyChanged;
            var e = new PropertyChangedEventArgs(propertyName);
            if (handler != null)
            {
                Dispatcher dispatcher = Application.Current.Dispatcher;
                bool eventDispatched = false;
                if (dispatcher != null)
                {
                    if (!dispatcher.CheckAccess())
                    {
                        dispatcher.Invoke(new Action(() => handler.Invoke(this, e)));
                        dispatcher.Invoke(new Action(() => this.commands
                                                               .OfType<DelegateCommand<object>>()
                                                               .ToList()
                                                               .ForEach(c => c.RaiseCanExecuteChanged())));
                        eventDispatched = true;
                    }
                }

                if (!eventDispatched)
                {
                    handler(this, e);
                    this.commands
                        .OfType<DelegateCommand<object>>()
                        .ToList()
                        .ForEach(c => c.RaiseCanExecuteChanged());
                }
            }
        }

        /// <summary>
        /// Virtual method calls the ChangedVisualState
        /// </summary>
        /// <param name="newStateName">
        /// Name of the new state
        /// </param>
        protected void RaiseVisualStateChanged(string newStateName)
        {
            Debug.Assert(!String.IsNullOrEmpty(newStateName), "The state name provided must be a non-empty string.");

            if (String.IsNullOrEmpty(newStateName))
            {
                throw new ArgumentNullException("newStateName");
            }

            if (this.VisualStateChanged != null)
            {
                Dispatcher dispatcher = Application.Current.Dispatcher;
                bool eventDispatched = false;
                var args = new NotifyVisualStateChangedEventArgs(newStateName);
                if (dispatcher != null)
                {
                    if (!dispatcher.CheckAccess())
                    {
                        dispatcher.Invoke(new Action(() => this.VisualStateChanged(this, args)));
                        eventDispatched = true;
                    }
                }

                if (!eventDispatched)
                {
                    this.VisualStateChanged(this, args);
                }
            }

            this.CurrentState = newStateName;
        }

        /// <summary>
        /// Method is used to add command in the commands collection. 
        /// </summary>
        /// <param name="command">
        /// ICommand added to the collection.
        /// </param>
        private void OnCommandChanged(ICommand command)
        {
            Debug.Assert(command != null, "The command must be non-null.");

            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            this.commands.Add(command);
        }

        #endregion Methods 
    }
}
