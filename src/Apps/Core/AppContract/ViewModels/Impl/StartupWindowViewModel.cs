using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NetX.AppCore.Contract
{
    public abstract class StartupWindowViewModel : BaseViewModel, IStartupWindowViewModel, ICloseWindowViewModel
    {
        public int Order { get; private set; }
        public int GotoStep { get; private set; } = -1;
        public Window? Window => base.View as Window;

        private AutoResetEvent AutoResetEvent { get; set; }

        public StartupWindowViewModel(IControlCreator controlCreator, Type pageView, int order)
            : base(controlCreator, pageView)
        {
            Order = order;
        }

        public void SetResetEvent(AutoResetEvent resetEvent)=> AutoResetEvent = resetEvent;

        public void GotoNextWindow() => AutoResetEvent.Set();

        protected virtual void GotoWindow(int step) => GotoStep = step;

        protected override void ControlLoaded()
        {
            base.ControlLoaded();
        }
    }
}
