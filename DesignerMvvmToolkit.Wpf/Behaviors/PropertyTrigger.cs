//-----------------------------------------------------------------------
// <copyright file="PropertyTrigger.cs" company="Charlie Robbins">
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
// <summary>Contains the PropertyTrigger class.</summary>
//-----------------------------------------------------------------------
        
namespace DesignerMvvmToolkit.Behaviors
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    /// A trigger that executes when a specified DependencyProperty changes.
    /// </summary>
    public class PropertyTrigger : TriggerBase<DependencyObject>
    {
        #region Dependency Properties 

        /// <summary>
        /// Backing store for the PropertyName property.
        /// </summary>
        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(
            "PropertyName",
            typeof(string),
            typeof(PropertyTrigger),
            new FrameworkPropertyMetadata(null, OnPropertyNamePropertyChanged));

        /// <summary>
        /// Backing store for the Value property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(object),
            typeof(PropertyTrigger),
            new FrameworkPropertyMetadata(null, OnValuePropertyChanged));

        #endregion Dependency Properties 

        /// <summary>
        /// The property descriptor for the current DependencyProperty being listened to.
        /// </summary>
        private DependencyPropertyDescriptor propertyDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyTrigger"/> class.
        /// </summary>
        public PropertyTrigger()
        {
        }

        #region Properties 

        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        #endregion Properties 

        #region Methods 

        /// <summary>
        /// Called after the trigger is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.ListenToPropertyChanged(this.PropertyName);
        }

        /// <summary>
        /// Called when PropertyName has changed; starts or stops listening to property changes. 
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnPropertyNameChanged(string oldValue, string newValue)
        {
            if (!string.IsNullOrEmpty(oldValue))
            {
                this.propertyDescriptor.RemoveValueChanged(this.AssociatedObject, this.OnPropertyChanged);
            }

            if (!string.IsNullOrEmpty(newValue) && this.AssociatedObject != null)
            {
                this.ListenToPropertyChanged(newValue);
            }
        }

        /// <summary>
        /// Called when Value changes; updates the value being listened for
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnValueChanged(object oldValue, object newValue)
        {
        }

        #region Private Static Methods 

        /// <summary>
        /// Called when the PropertyName property changes; calls the instance OnPropertyNameChanged method.
        /// </summary>
        /// <param name="obj">The DependencyObject on whom PropertyNameProperty changed.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnPropertyNamePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((PropertyTrigger)obj).OnPropertyNameChanged((string)args.OldValue, (string)args.NewValue);
        }

        /// <summary>
        /// Called when the Value property changes; calls the instance OnValueChanged method.
        /// </summary>
        /// <param name="obj">The DependencyObject on whom ValueProperty changed..</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnValuePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
        }

        #endregion Private Static Methods 

        /// <summary>
        /// Starts listening to property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void ListenToPropertyChanged(string propertyName)
        {
            Type targetType = this.AssociatedObject.GetType();

            PropertyInfo propertyInfo = targetType.GetProperty(propertyName);
            if (!propertyInfo.CanRead)
            {
                throw new ArgumentException("Cannot listen to changes for a write only property");
            }

            DependencyPropertyDescriptor newDescriptor = DependencyPropertyDescriptor.FromName(
                propertyName,
                targetType,
                targetType);

            if (newDescriptor == null)
            {
                // TODO: Check to see if the property is setup using DependencyPropertyKey
                throw new ArgumentException("Cannot listen to changes for a property that is not implemented using a DependencyProperty");
            }

            newDescriptor.AddValueChanged(this.AssociatedObject, this.OnPropertyChanged);
            this.propertyDescriptor = newDescriptor;
        }

        /// <summary>
        /// Called when the property being tracked by this instance changes; invokes the associated actions.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnPropertyChanged(object sender, EventArgs args)
        {
            // TODO: Do something if the value is not null...
            if (this.Value == null)
            {
                this.InvokeActions(this.Value);
            }
        }

        #endregion Methods 
    }
}
