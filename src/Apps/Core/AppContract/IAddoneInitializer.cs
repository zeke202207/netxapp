using Microsoft.Extensions.DependencyInjection;

namespace NetX.AppCore.Contract
{
    public interface IAddoneInitializer
    {
        void ConfigureServices(IServiceCollection services);
    }
}
