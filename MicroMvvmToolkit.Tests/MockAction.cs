using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Windows.Interactivity;
using System.Windows;

namespace MicroMvvmToolkit.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class MockAction : TargetedTriggerAction<DependencyObject>
    {
        public bool IsAttached
        {
            get;
            private set;
        }

        public bool hasExecuted
        {
            get;
            private set;
        }

        public MockAction()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            this.IsAttached = true;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.IsAttached = false;
        }

        protected override void Invoke(object parameter)
        {
            this.hasExecuted = true;
        }
    }
}
