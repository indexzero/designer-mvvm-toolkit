//-----------------------------------------------------------------------
// <copyright file="NotifyVisualStateChangedEventArgs.cs" company="Charlie Robbins">
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
// <summary>Contains the NotifyVisualStateChangedEventArgs class.</summary>
//-----------------------------------------------------------------------

namespace MicroMvvmToolkit.Behaviors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class represents the event argument for visual state.
    /// </summary>
    public class NotifyVisualStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Backstore for StateName property.
        /// </summary>
        private string stateName;

        /// <summary>
        /// The state asserted before the state change.
        /// </summary>
        private string previousState;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyVisualStateChangedEventArgs" /> class.
        /// </summary>
        /// <param name="stateName">Name of the visual state.</param>
        public NotifyVisualStateChangedEventArgs(string stateName)
        {
            this.stateName = stateName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyVisualStateChangedEventArgs" /> class.
        /// </summary>
        /// <param name="stateName">Name of the visual state.</param>
        /// <param name="previousState">State of the previous.</param>
        public NotifyVisualStateChangedEventArgs(string stateName, string previousState)
        {
            this.stateName = stateName;
            this.previousState = previousState;
        }

        /// <summary>
        /// Gets the StateName.
        /// </summary>
        public string StateName
        {
            get
            {
                return this.stateName;
            }
        }

        /// <summary>
        /// Gets the state of the previous.
        /// </summary>
        /// <value>The state of the previous.</value>
        public string PreviousState
        {
            get
            {
                return this.previousState;
            }
        }
    }
}
