using Avalonia.Controls;

namespace NetX.AppCore.Contract
{
    public abstract class StartupWindowViewModel : BaseViewModel, IStartupWindowViewModel, ICloseWindowViewModel
    {
        public Guid Id { get; private set; }
        public Guid GotoStep { get; private set; } = Guid.Empty;
        public Window? Window => base.View as Window;

        private AutoResetEvent AutoResetEvent { get; set; }

        public StartupWindowViewModel(Guid id ,IServiceProvider serviceProvider, Type pageView)
            : base(serviceProvider, pageView)
        {
            Id = id;
        }

        public void SetResetEvent(AutoResetEvent resetEvent) => AutoResetEvent = resetEvent;

        public void GotoNextWindow() => AutoResetEvent.Set();

        public virtual void GotoWindow(Guid stepid)
        {
            GotoStep = stepid;
            base.CloseApplication();
        }

        protected override void ControlLoaded()
        {
            base.ControlLoaded();
        }
    }
}
