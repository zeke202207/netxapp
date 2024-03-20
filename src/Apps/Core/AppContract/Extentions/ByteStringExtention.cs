using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppCore.Contract.Extentions
{
    public static class ByteStringExtention
    {
        public static T ToModel<T>(this Google.Protobuf.ByteString byteString)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(byteString.ToStringUtf8());
        }

        public static T ToModel<T>(this string byteString)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(byteString);
        }
    }
}
