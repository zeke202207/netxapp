using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.Service
{
    public interface ICaptcha
    {
        Task<string> GenerateRandomCaptchaAsync(int codeLength = 4);
        Task<byte[]> GenerateCaptchaImageAsync(string captchaCode, int width = 100, int height = 30);
    }
}
