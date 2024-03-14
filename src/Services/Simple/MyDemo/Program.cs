using NetX.ServiceCore;

namespace MyDemo
{
    public class Program
    {
        public static async Task Main(string[] args) => await ServiceBootstrap.Start(args);
    }
}
