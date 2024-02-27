using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetX.AppContainer.Contract
{
    public interface IControlCreator
    {
        Control? CreateControl(Type controlType);
    }

    public class ControlCreator : IControlCreator
    {
        public Control? CreateControl(Type controlType)
        {
            return (Control)Activator.CreateInstance(controlType);
        }
    }
}
