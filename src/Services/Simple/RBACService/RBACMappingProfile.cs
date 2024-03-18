using AutoMapper;
using NetX.RBAC.Service.Models;
using NetX.ServiceCore;
using RBAC;

namespace NetX.RBAC.Service
{
    public class RBACMappingProfile : IMapperConfig
    {
        public void Bind(IMapperConfigurationExpression cfg)
        {
            AccountMapping(cfg);
        }

        private void AccountMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<LoginResultModel, LoginResponse>();
        }
    }
}
