using NetX.ServiceCore;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NetX.RBAC.Service
{
    [Transient]
    public class Captcha : ICaptcha
    {
        //private const string Letters = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
        private const string Letters = "1,2,3,4,5,6,7,8,9";
        private readonly SixLabors.ImageSharp.Color[] Colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Brown, Color.Purple };

        public Task<byte[]> GenerateCaptchaImageAsync(string captchaCode, int width = 100, int height = 30)
        {
            using var image = new Image<Rgba32>(width, height);
            var font = SystemFonts.CreateFont(SystemFonts.Families.First().Name, 25, FontStyle.Bold);
            var r = new Random();
            image.Mutate(ctx =>
            {
                ctx.Fill(Color.White);
                for (int i = 0; i < captchaCode.Length; i++)
                {
                    ctx.DrawText(captchaCode[i].ToString(), font, Colors[r.Next(Colors.Length)], new PointF(20 * i + 10, r.Next(2, 12)));
                }
                for (int i = 0; i < 10; i++)
                {
                    var pen = new SolidPen(Colors[r.Next(Colors.Length)], 1);
                    var p1 = new PointF(r.Next(width), r.Next(height));
                    var p2 = new PointF(r.Next(width), r.Next(height));
                    ctx.DrawLine(pen, p1, p2);
                }
                for (int i = 0; i < 80; i++)
                {
                    var pen = new SolidPen(Colors[r.Next(Colors.Length)], 1);
                    var p1 = new PointF(r.Next(width), r.Next(height));
                    var p2 = new PointF(p1.X + 1f, p1.Y + 1f);
                    ctx.DrawLine(pen, p1, p2);
                }
            });

            using var ms = new System.IO.MemoryStream();
            image.SaveAsGif(ms);
            //image.SaveAsGif("1.gif");
            return Task.FromResult(ms.ToArray());
        }

        public Task<string> GenerateRandomCaptchaAsync(int codeLength = 4)
        {
            var array = Letters.Split(new[] { ',' });
            var random = new Random();
            var temp = -1;
            var captcheCode = string.Empty;
            for (int i = 0; i < codeLength; i++)
            {
                if (temp != -1)
                    random = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                var index = random.Next(array.Length);
                if (temp != -1 && temp == index)
                    return GenerateRandomCaptchaAsync(codeLength);
                temp = index;
                captcheCode += array[index];
            }
            return Task.FromResult(captcheCode);
        }

    }
}
