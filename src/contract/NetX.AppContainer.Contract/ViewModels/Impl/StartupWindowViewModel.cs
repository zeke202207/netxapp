using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NetX.AppContainer.Contract
{
    public abstract class StartupWindowViewModel : BaseViewModel, IStartupWindowViewModel
    {
        public AutoResetEvent AutoResetEvent { get; set; }
        public Window Window { get; private set; }
        public int Order { get; private set; }

        public StartupWindowViewModel(IControlCreator controlCreator, Type pageView, int order)
            : base(controlCreator, pageView)
        {
            Order = order;
        }
    }
}
