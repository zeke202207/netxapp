namespace NetX.AppCore.Contract
{
    public class PageNotFoundException : Exception
    {
        public PageNotFoundException(string message)
            : base(message)
        {
        }
    }
}
