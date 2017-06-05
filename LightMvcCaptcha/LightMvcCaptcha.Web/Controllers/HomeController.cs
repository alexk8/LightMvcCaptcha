using System;
using System.Drawing;
using System.Web.Mvc;
using LightMvcCaptcha.Core;
using LightMvcCaptcha.Web.ViewModels;

namespace LightMvcCaptcha.Web.Controllers
{
    public class HomeController : CaptchaController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View(new CaptchaViewModel());
        }

        [HttpPost]
        public ActionResult Index(CaptchaViewModel model)
        {
            try
            {
                Captcha.Font = new Font(Captcha.Font.FontFamily, model.FontSize);
                Captcha.Chars = model.Chars;
                Captcha.Length = model.Length;
                Captcha.CharsSpacing = model.CharsSpacing;
                Captcha.MaxRotationAngle = model.MaxRotationAngle;
                Captcha.WaveDistortionAmplitude = model.WaveDistortionAmplitude;
                Captcha.WaveDistortionPeriod = model.WaveDistortionPeriod;
                Captcha.LineNoiseCount = model.LineNoiseCount;
                Captcha.WaveDistortionEnabled = model.WaveDistortionEnabled;
                Captcha.LineNoiseEnabled = model.LineNoiseEnabled;

                if (ModelState.IsValid)
                {
                    ViewBag.Answer = "Correct! " + model.Captcha;
                    return View(new CaptchaViewModel());
                }
            }
            catch(Exception e)
            {
                ViewBag.Answer = "Error: " + e.Message;
            }
            return View(model);
        }

        public ActionResult Reset()
        {
            Captcha.Font = new Font("Lucida Console", 60);
            Captcha.Chars = "WERTUPASDFGHKLZXCVBNM123456789";
            Captcha.Length = 6;
            Captcha.CharsSpacing = 48;
            Captcha.MaxRotationAngle = 60;
            Captcha.WaveDistortionAmplitude = 100;
            Captcha.WaveDistortionPeriod = 100;
            Captcha.LineNoiseCount = 10;
            Captcha.WaveDistortionEnabled = true;
            Captcha.LineNoiseEnabled = true;

            return RedirectToAction("Index");
        }
    }
}