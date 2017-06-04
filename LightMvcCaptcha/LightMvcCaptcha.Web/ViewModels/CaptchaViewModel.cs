using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LightMvcCaptcha.Core;

namespace LightMvcCaptcha.Web.ViewModels
{
    public class CaptchaViewModel
    {
        [Captcha(ErrorMessage = "Wrong captcha")]
        public string Captcha { get; set; }
    }
}