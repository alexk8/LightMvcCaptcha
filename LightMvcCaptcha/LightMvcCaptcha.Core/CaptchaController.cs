using System;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace LightMvcCaptcha.Core
{
    public abstract class CaptchaController : Controller
    {
        public virtual FileStreamResult GetCaptcha()
        {
            var captcha = Captcha.Generate();

            Session["CAPTCHA"] = captcha;

            var ms = new MemoryStream(captcha.Image);
            captcha.DisposeImage();

            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            Response.Cache.SetValidUntilExpires(false);
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            return new FileStreamResult(ms, "image/jpeg");
        }
    }
}
