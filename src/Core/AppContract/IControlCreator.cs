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
}
