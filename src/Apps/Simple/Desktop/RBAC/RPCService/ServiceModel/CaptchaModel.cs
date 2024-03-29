﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.RPCService
{
    public class CaptchaModel
    {
        public string CaptchaId { get; set; }
        public string CaptchaBase64 { get; set; }

        public CaptchaModel(string captchaId, string captchaBase64)
        {
            CaptchaId = captchaId;
            CaptchaBase64 = captchaBase64;
        }
    }
}
