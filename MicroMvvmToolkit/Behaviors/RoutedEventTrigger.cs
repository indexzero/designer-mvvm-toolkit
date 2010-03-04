//-----------------------------------------------------------------------
// <copyright file="RoutedEventTrigger.cs" company="Charlie Robbins">
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
// <summary>Contains the RoutedEventTrigger class.</summary>
//-----------------------------------------------------------------------
        
namespace MicroMvvmToolkit.Behaviors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    /// An event trigger that fires on any RoutedEvent
    /// </summary>
    public class RoutedEventTrigger : EventTriggerBase<DependencyObject> 
    {
        #region Fields 

        /// <summary>
        /// The underlying RoutedEvent
        /// </summary>
        private RoutedEvent routedEvent;

        #endregion Fields 

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEventTrigger"/> class.
        /// </summary>
        public RoutedEventTrigger() 
        { 
        }

        #region Properties 

        /// <summary>
        /// Gets or sets the underlying RoutedEvent.
        /// </summary>
        /// <value>The RoutedEvent.</value>
        public RoutedEvent RoutedEvent 
        { 
            get 
            { 
                return this.routedEvent; 
            } 

            set 
            { 
                this.routedEvent = value; 
            } 
        }

        #endregion Properties 

        #region Methods 

        /// <summary>
        /// Specifies the name of the Event this EventTriggerBase is listening for.
        /// </summary>
        /// <returns>The name of the event associated with this instance</returns>
        protected override string GetEventName() 
        { 
            return RoutedEvent.Name; 
        }

        /// <summary>
        /// Called after the trigger is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached() 
        { 
            Behavior behavior = this.AssociatedObject as Behavior; 
            FrameworkElement associatedElement = this.AssociatedObject as FrameworkElement; 
            if (behavior != null) 
            { 
                associatedElement = ((IAttachedObject)behavior).AssociatedObject as FrameworkElement; 
            } 
            
            if (associatedElement == null) 
            { 
                throw new ArgumentException("Routed Event trigger can only be associated to framework elements"); 
            } 
            
            if (RoutedEvent != null) 
            { 
                associatedElement.AddHandler(RoutedEvent, new RoutedEventHandler(this.OnRoutedEvent)); 
            }
        }

        /// <summary>
        /// Called when the RoutedEvent fires
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnRoutedEvent(object sender, RoutedEventArgs args) 
        { 
            this.OnEvent(args); 
        }

        #endregion Methods 
     }
}
