using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.Service.Models
{
    public class CaptchaResultModel
    {
        public string CaptchaId { get; set; }

        public string CaptchaBase64 { get; set; }
    }
}
