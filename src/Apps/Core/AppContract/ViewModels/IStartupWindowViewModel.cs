namespace NetX.AppCore.Contract
{
    public interface IStartupWindowViewModel
    {
        public int Order { get; }

        void SetResetEvent(AutoResetEvent resetEvent);

        void GotoNextWindow();
    }
}
