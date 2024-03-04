using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NetX.AppContainer.Contract
{
    public interface IStartupWindowViewModel
    {
        public int Order { get; }

        void SetResetEvent(AutoResetEvent resetEvent);

        void GotoNextWindow();
    }
}
