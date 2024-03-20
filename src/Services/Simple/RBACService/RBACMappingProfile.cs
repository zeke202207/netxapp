using AutoMapper;
using Google.Protobuf;
using Netx.Rbac.V1;
using Netx.Response.V1;
using NetX.RBAC.Service.Models;
using NetX.ServiceCore;

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
            cfg.CreateMap<LoginResultModel, Response>()
                .ForMember(Response => Response.Status, opt=> opt.MapFrom(LoginResultModel => Status.Ok))
                .ForMember(Response => Response.Data, opt => opt.MapFrom(LoginResultModel => ByteString.CopyFromUtf8(Newtonsoft.Json.JsonConvert.SerializeObject(LoginResultModel))));
            cfg.CreateMap<CaptchaResultModel, Response>()
                .ForMember(Response => Response.Status, opt => opt.MapFrom(CaptchaResultModel => Status.Ok))
                .ForMember(Response => Response.Data, opt => opt.MapFrom(CaptchaResultModel => ByteString.CopyFromUtf8(Newtonsoft.Json.JsonConvert.SerializeObject(CaptchaResultModel))));
        }
    }
}
