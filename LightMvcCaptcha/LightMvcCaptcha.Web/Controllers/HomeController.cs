using Microsoft.AspNetCore.Mvc;
using AspNetCoreWebTest.Models;
using LightMvcCaptcha.Core;

namespace AspNetCoreWebTest.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Captcha("captchacode" , "sorry, try again")]
        public IActionResult Index(TestCaptchaModel model)
        {
            if (!ModelState.IsValid) return View(model);
            ViewData["message"] = "success";
            return View();
        }

    }
}
