using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetX.AppContainer.Contract
{
    public class ActivatorControlCreator : IControlCreator
    {
        public Control? CreateControl(Type controlType)
        {
            return (Control)Activator.CreateInstance(controlType);
        }
    }
}
