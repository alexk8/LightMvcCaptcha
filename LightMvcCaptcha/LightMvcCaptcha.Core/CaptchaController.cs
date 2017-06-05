using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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

            ms.Seek(0, SeekOrigin.Begin);

            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            Response.Cache.SetValidUntilExpires(false);
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            return new FileStreamResult(ms, "image/jpeg");
        }
    }
}
