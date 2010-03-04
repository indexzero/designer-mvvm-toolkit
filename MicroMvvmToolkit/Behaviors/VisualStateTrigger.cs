//-----------------------------------------------------------------------
// <copyright file="VisualStateTrigger.cs" company="Charlie Robbins">
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
// <summary>Contains the VisualStateTrigger class.</summary>
//-----------------------------------------------------------------------
        
namespace MicroMvvmToolkit.Behaviors
{
    using System;
    using System.Windows;
    using System.Windows.Interactivity;
    using EventTrigger = System.Windows.Interactivity.EventTrigger;

    /// <summary>
    /// EventTrigger that fires on the VisualStateChanged event.
    /// </summary>
    public class VisualStateTrigger : EventTriggerBase<FrameworkElement>
    {
        #region Dependency Properties 

        /// <summary>
        /// Backing store for the StateName property.
        /// </summary>
        public static readonly DependencyProperty StateNameProperty = DependencyProperty.Register(
            "StateName",
            typeof(string),
            typeof(VisualStateTrigger),
            new FrameworkPropertyMetadata(null));

        #endregion Dependency Properties 

        #region Fields 

        /// <summary>
        /// The INotifyVisualStateChanged this trigger is listening to.
        /// </summary>
        private INotifyVisualStateChanged visualStateChanged;

        #endregion Fields 

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualStateTrigger"/> class.
        /// </summary>
        public VisualStateTrigger()
        {
        }

        #region Properties 

        /// <summary>
        /// Gets or sets the name of the state.
        /// </summary>
        /// <value>The name of the state.</value>
        public string StateName
        {
            get { return (string)GetValue(StateNameProperty); }
            set { SetValue(StateNameProperty, value); }
        }

        #endregion Properties 

        #region Methods 

        /// <summary>
        /// Specifies the name of the Event this EventTriggerBase is listening for.
        /// </summary>
        /// <returns>The event this instance tracks.</returns>
        protected override string GetEventName()
        {
            return "VisualStateChanged";
        }

        /// <summary>
        /// Called after the trigger is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            this.visualStateChanged = this.Source.DataContext as INotifyVisualStateChanged;

            // TODO: Walk up the visual tree to potentially find a parent INotifyVisualStateChanged
            if (this.visualStateChanged == null)
            {
                throw new ArgumentException("Cannot attach to FramworkElement without VisualStateChanged as DataContext.");
            }

            this.visualStateChanged.VisualStateChanged += this.OnVisualStateChanged;
        }

        /// <summary>
        /// Called when the trigger is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this.visualStateChanged != null)
            {
                this.visualStateChanged.VisualStateChanged -= this.OnVisualStateChanged;
            }
        }

        /// <summary>
        /// Called when the VisualStateChanged event is raised; fires the event triggers.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="MicroMvvmToolkit.Behaviors.NotifyVisualStateChangedEventArgs"/> instance containing the event data.</param>
        private void OnVisualStateChanged(object sender, NotifyVisualStateChangedEventArgs args)
        {
            if (this.visualStateChanged != null)
            {
                this.OnEvent(args);
            }
        }

        #endregion Methods 
}
}
