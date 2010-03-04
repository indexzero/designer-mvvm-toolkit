using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Windows.Interactivity;
using MicroMvvmToolkit.Behaviors;
using System.Windows;

namespace MicroMvvmToolkit.Tests
{
    /// <summary>
    /// Summary description for PropertyTriggerTests
    /// </summary>
    [TestClass]
    public class PropertyTriggerTests
    {
        [TestMethod]
        public void ShouldFireTriggersOnPropertyChanged()
        {
            PropertyTrigger trigger = new PropertyTrigger()
            {
                PropertyName = "HorizontalAlignment"
            };

            MockAction action = new MockAction();
            trigger.Actions.Add(action);
            FrameworkElement element = new FrameworkElement();

            System.Windows.Interactivity.TriggerCollection triggers = Interaction.GetTriggers(element);
            triggers.Add(trigger);

            element.HorizontalAlignment = HorizontalAlignment.Left;

            Assert.IsTrue(action.hasExecuted);
            Assert.IsTrue(action.IsAttached);
        }
    }
}
