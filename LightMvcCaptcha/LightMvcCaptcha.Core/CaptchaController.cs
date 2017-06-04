using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LightMvcCaptcha.Core
{
    public abstract class CaptchaController : Controller
    {
        public FileStreamResult GetCaptcha()
        {
            var captcha = Captcha.Generate();

            Session["CAPTCHA"] = captcha;

            MemoryStream ms = new MemoryStream();
            captcha.Image.Save(ms, ImageFormat.Jpeg);

            captcha.Image.Dispose();

            ms.Position = 0;

            return new FileStreamResult(ms, "image/jpeg");
        }
    }
}
