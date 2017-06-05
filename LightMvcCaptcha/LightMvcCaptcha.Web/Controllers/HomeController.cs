using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Mvc;
using LightMvcCaptcha.Core;
using LightMvcCaptcha.Web.ViewModels;

namespace LightMvcCaptcha.Web.Controllers
{
    public class HomeController : CaptchaController
    {
        private CaptchaViewModel captchaViewModel
        {
            get
            {
                var cpt = Session["captchaViewModel"] as CaptchaViewModel;
                if (cpt == null)
                    Session["captchaViewModel"] = cpt = new CaptchaViewModel();
                return cpt;
            }
            set { Session["captchaViewModel"] = value; }
        }


        public ActionResult Index()
        {
            return View(captchaViewModel);
        }

        [HttpPost]
        public ActionResult Index(CaptchaViewModel model)
        {
            Font font = new Font(model.FontFamily, model.FontSize); // forcing creating default font if FontFamily is not correct
            model.FontFamily = font.FontFamily.Name;
            captchaViewModel = model;

            if (ModelState.IsValid)
            {
                ViewBag.Answer = "Correct! " + model.Captcha;
            }

            return RedirectToAction("Index");
        }

        public ActionResult Reset()
        {
            captchaViewModel = new CaptchaViewModel();

            return RedirectToAction("Index");
        }

        //Replacing CaptchaController's GetCaptcha because we dont want to use static settings for captcha
        public override FileStreamResult GetCaptcha()
        {
            var c = captchaViewModel;
            var captcha = Captcha.Generate(new Font(c.FontFamily, c.FontSize), c.Chars, c.Length,
                c.CharsSpacing, c.MaxRotationAngle, c.WaveDistortionEnabled, c.WaveDistortionAmplitude,
                c.WaveDistortionPeriod, c.LineNoiseEnabled, c.LineNoiseCount);

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