//-----------------------------------------------------------------------
// <copyright file="TriggerTestViewModel.cs" company="Charlie Robbins">
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
// <summary>Contains the TriggerTestViewModel class.</summary>
//-----------------------------------------------------------------------
        
namespace MicroMvvmSample
{
    using System.Windows.Input;
    using DesignerMvvmToolkit;

    public class TriggerTestViewModel : ViewModelBase
    {
        #region Fields 

        /// <summary>
        /// The command that tests if a trigger is firing.
        /// </summary>
        private ICommand testCommand;

        #endregion Fields 

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerTestViewModel"/> class.
        /// </summary>
        public TriggerTestViewModel()
        {
        }

        #region Properties 

        /// <summary>
        /// Gets the command that tests if a trigger is firing.
        /// </summary>
        public ICommand TestCommand
        {
            get
            {
                return this.testCommand ?? 
                    (this.testCommand = this.CreateCommand<object>(
                        obj => this.ExecuteTest()));
            }
        }

        #endregion Properties 

        #region Methods 

        private void ExecuteTest()
        {
        }

        #endregion Methods 
     }
}
