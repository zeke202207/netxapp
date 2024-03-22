namespace NetX.AppCore.Contract
{
    public interface IStartupWindowViewModel
    {
        public Guid Id { get; }

        void SetResetEvent(AutoResetEvent resetEvent);

        void GotoNextWindow();
    }
}
