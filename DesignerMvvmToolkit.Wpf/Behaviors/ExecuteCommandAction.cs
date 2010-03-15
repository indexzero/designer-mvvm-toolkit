//-----------------------------------------------------------------------
// <copyright file="ExecuteCommandAction.cs" company="Charlie Robbins">
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
// <summary>Contains the ExecuteCommandAction class.</summary>
//-----------------------------------------------------------------------
        
namespace DesignerMvvmToolkit.Behaviors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using EventTrigger = System.Windows.Interactivity.EventTrigger;

    /// <summary> 
    /// Behaviour helps to bind any RoutedEvent of UIElement to Command. 
    /// </summary> 
    [DefaultTrigger(typeof(UIElement), typeof(EventTrigger), "MouseLeftButtonDown")]
    public class ExecuteCommandAction : TargetedTriggerAction<UIElement>
    {
        #region Dependency Properties 

        /// <summary> 
        /// Dependency property represents the Command of the behaviour. 
        /// </summary> 
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
            "CommandParameter",
            typeof(object),
            typeof(ExecuteCommandAction),
            new FrameworkPropertyMetadata(null));

        /// <summary> 
        /// Dependency property represents the Command parameter of the behaviour. 
        /// </summary> 
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(ExecuteCommandAction),
            new FrameworkPropertyMetadata(null));

        #endregion Dependency Properties 

        #region Properties 

        /// <summary> 
        /// Gets or sets the Commmand. 
        /// </summary> 
        public ICommand Command
        {
            get
            {
                return (ICommand)this.GetValue(CommandProperty);
            }

            set
            {
                this.SetValue(CommandProperty, value);
            }
        }

        /// <summary> 
        /// Gets or sets the CommandParameter. 
        /// </summary> 
        public object CommandParameter
        {
            get
            {
                return this.GetValue(CommandParameterProperty);
            }

            set
            {
                this.SetValue(CommandParameterProperty, value);
            }
        }

        #endregion Properties 

        #region Methods 

        /// <summary> 
        /// Invoke method is called when the given routed event is fired. 
        /// </summary> 
        /// <param name="parameter"> 
        /// Parameter is the sender of the event. 
        /// </param> 
        protected override void Invoke(object parameter)
        {
            if (this.Command != null)
            {
                if (this.Command.CanExecute(this.CommandParameter))
                {
                    this.Command.Execute(this.CommandParameter);
                }
            }
        }

        #endregion Methods 
    }
}
