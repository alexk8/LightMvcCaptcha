using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            if (ModelState.IsValid)
                return RedirectToAction("Success");

            return View(model);
        }

        public string Success()
        {
            return "Correct!";
        }
    }
}