using System;
using System.Collections.Generic;
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
            using (var captcha = Captcha.Generate())
            {
                Session["CAPTCHA"] = captcha;

                return new FileStreamResult(captcha.ImageStream, "image/jpeg");
            }
        }
    }
}
