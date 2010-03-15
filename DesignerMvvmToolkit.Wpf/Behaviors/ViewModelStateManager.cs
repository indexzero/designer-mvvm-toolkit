//-----------------------------------------------------------------------
// <copyright file="ViewModelStateManager.cs" company="Charlie Robbins">
//     Copyright (c) Charlie Robbins. All rights reserved.
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
// <summary>Contains the ViewModelStateManager class.</summary>
//-----------------------------------------------------------------------

namespace DesignerMvvmToolkit.Behaviors
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Class represents the State of a ViewModel.
    /// </summary>
    public static class ViewModelStateManager
    {
        #region Dependency Properties 

        public static readonly DependencyProperty InitialStateProperty =
            DependencyProperty.RegisterAttached(
            "InitialState",
            typeof(string),
            typeof(ViewModelStateManager),
            new FrameworkPropertyMetadata(null, OnInitialStatePropertyChanged));

        /// <summary>
        /// Dependency property for implementing attached behaviour.
        /// </summary>
        ////public static readonly DependencyProperty AttachViewModelStatesProperty =
        ////    DependencyProperty.RegisterAttached(
        ////        "AttachViewModelStates",
        ////        typeof(bool),
        ////        typeof(ViewModelStateManager),
        ////        new FrameworkPropertyMetadata(false, OnAttachViewModelStatesChanged));

        /// <summary>
        /// Method is called when the dependency property is changed.
        /// </summary>
        /// <param name="depObj">
        /// DependencyObject on which property is set.
        /// </param>
        /// <param name="e">
        /// EventArgs having old and new value.
        /// </param>
        ////private static void OnAttachViewModelStatesChanged(
        ////    DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        ////{
        ////    bool wasAttached = (bool)args.OldValue;
        ////    bool isAttached = (bool)args.NewValue;

        ////    FrameworkElement element = depObj as FrameworkElement;
        ////    if (!wasAttached && isAttached && element != null)
        ////    {
        ////        element.DataContextChanged += OnDataContextChanged;
        ////        element.Loaded += OnLoaded;
        ////        element.Unloaded += OnUnloaded;
        ////    }
        ////    else if (wasAttached && !isAttached && element != null)
        ////    {
        ////        element.DataContextChanged -= OnDataContextChanged;
        ////        element.Loaded -= OnLoaded;
        ////        element.Unloaded -= OnUnloaded;
        ////    }
        ////}

        #endregion Dependency Properties 

        #region Fields 

        /// <summary>
        /// Static dictionary storing the reference of the views and there relevent view models.
        /// </summary>
        private static readonly Dictionary<INotifyVisualStateChanged, FrameworkElement> stateManagers =
            new Dictionary<INotifyVisualStateChanged, FrameworkElement>();

        #endregion Fields 

        #region Methods 

        public static string GetInitialState(DependencyObject obj)
        {
            return (string)obj.GetValue(InitialStateProperty);
        }

        public static void SetInitialState(DependencyObject obj, string value)
        {
            obj.SetValue(InitialStateProperty, value);
        }

        /// <summary>
        /// Gets the value of dependency property.
        /// </summary>
        /// <param name="control">
        /// Control on which the property is set.
        /// </param>
        /// <returns>
        /// Value from the dependency property
        /// </returns>
        ////public static bool GetAttachViewModelStates(FrameworkElement control)
        ////{
        ////    return (bool)control.GetValue(AttachViewModelStatesProperty);
        ////}

        /// <summary>
        /// Sets the value of dependency property.
        /// </summary>
        /// <param name="control">
        /// Control on which the property is set.
        /// </param>
        /// <param name="value">
        /// Value for dependency property
        /// </param>
        ////public static void SetAttachViewModelStates(FrameworkElement control, bool value)
        ////{
        ////    control.SetValue(AttachViewModelStatesProperty, value);
        ////}

        #region Private Static Methods 

        /// <summary>
        /// Method is called when the DataContext is changed.
        /// </summary>
        /// <param name="sender">
        /// Control whose data context is changed.
        /// </param>
        /// <param name="e">
        /// Event arguments have old and new datacontext.
        /// </param>
        private static void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var viewModelStateManager = e.OldValue as INotifyVisualStateChanged;
            if (viewModelStateManager != null && stateManagers.ContainsKey(viewModelStateManager))
            {
                stateManagers.Remove(viewModelStateManager);
            }

            if (e.NewValue is INotifyVisualStateChanged)
            {
                var viewModel = e.NewValue as INotifyVisualStateChanged;
                viewModel.VisualStateChanged += OnViewModelStateChanged;
                if (!stateManagers.ContainsKey(viewModel))
                {
                    stateManagers.Add(viewModel, sender as FrameworkElement);
                }
            }
        }

        private static void OnInitialStatePropertyChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            bool wasAttached = !string.IsNullOrEmpty((string)args.OldValue);
            bool isAttached = !string.IsNullOrEmpty((string)args.NewValue);

            FrameworkElement element = depObj as FrameworkElement;
            if (!wasAttached && isAttached && element != null)
            {
                element.DataContextChanged += OnDataContextChanged;
                element.Loaded += OnLoaded;
                element.Unloaded += OnUnloaded;
            }
            else if (wasAttached && !isAttached && element != null)
            {
                element.DataContextChanged -= OnDataContextChanged;
                element.Loaded -= OnLoaded;
                element.Unloaded -= OnUnloaded;
            }
        }

        /// <summary>
        /// Method prevents the Memory leak and removes the element 
        /// reference form the static dictionary.
        /// </summary>
        /// <param name="sender">
        /// Framework element whose Loaded event is fired.
        /// </param>
        /// <param name="e">
        /// Event arguments for the fired event.
        /// </param>
        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement != null)
            {
                var notifyVisualStateChanged = frameworkElement.DataContext as INotifyVisualStateChanged;
                if (notifyVisualStateChanged != null)
                {
                    if (!stateManagers.ContainsKey(notifyVisualStateChanged))
                    {
                        stateManagers.Add(notifyVisualStateChanged, frameworkElement as FrameworkElement);
                    }
                }

                OnViewModelStateChanged(
                    notifyVisualStateChanged, 
                    new NotifyVisualStateChangedEventArgs(GetInitialState(frameworkElement)));
            }
        }

        /// <summary>
        /// Method prevents the Memory leak and removes the element 
        /// reference form the static dictionary.
        /// </summary>
        /// <param name="sender">
        /// Framework element whose Unloaded event is fired.
        /// </param>
        /// <param name="e">
        /// Event arguments for the fired event.
        /// </param>
        private static void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement != null)
            {
                if (stateManagers.ContainsValue(frameworkElement))
                {
                    var viewModelStateManager = frameworkElement.DataContext as INotifyVisualStateChanged;
                    if (viewModelStateManager != null)
                    {
                        stateManagers.Remove(viewModelStateManager);
                    }
                }
            }
        }

        /// <summary>
        /// Method is called when the ViewModel state is changed.
        /// </summary>
        /// <param name="sender">
        /// ViewModel whose visual state is changed.
        /// </param>
        /// <param name="e">
        /// Event argument having the name of the state.
        /// </param>
        private static void OnViewModelStateChanged(object sender, NotifyVisualStateChangedEventArgs e)
        {
            var viewModelStateManager = sender as INotifyVisualStateChanged;
            if (viewModelStateManager != null && stateManagers.ContainsKey(viewModelStateManager))
            {
                var control = stateManagers[viewModelStateManager] as Control;
                if (control != null)
                {
                    VisualStateManager.GoToState(control, e.StateName, true);
                }
            }
        }

        #endregion Private Static Methods 

        #endregion Methods 
    }
}
