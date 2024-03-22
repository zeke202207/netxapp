namespace NetX.AppCore.Contract
{
    public class WindowNotFoundException : Exception
    {
        public WindowNotFoundException(string message)
            : base(message)
        {
        }
    }
}
