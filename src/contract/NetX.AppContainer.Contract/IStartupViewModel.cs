using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NetX.AppContainer.Contract
{
    public interface IStartupViewModel
    {
        public int Order { get; }

        public AutoResetEvent AutoResetEvent { get; set; }
    }
}
