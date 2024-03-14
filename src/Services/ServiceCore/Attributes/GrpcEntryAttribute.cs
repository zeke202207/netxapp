using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.ServiceCore
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GrpcEntryAttribute : Attribute
    {
        public GrpcEntryAttribute()
        {
        }
    }
}
