using AutoMapper;
using NetX.ServiceCore;

namespace NetX.RBAC.Service
{
    public class DemoMappingProfile : IMapperConfig
    {
        public void Bind(IMapperConfigurationExpression cfg)
        {
            AccountMapping(cfg);
        }

        private void AccountMapping(IMapperConfigurationExpression cfg)
        {
            
        }
    }
}
