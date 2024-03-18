using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.ServiceCore
{
    public interface IMapperConfig
    {
        void Bind(IMapperConfigurationExpression cfg);
    }
}
