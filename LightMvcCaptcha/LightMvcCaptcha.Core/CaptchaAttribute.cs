using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LightMvcCaptcha.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CaptchaAttribute : ValidationAttribute
    {
        public CaptchaAttribute()
        {
            
        }

        public override bool IsValid(object value)
        {
            string key = value as string;
            Captcha captcha = HttpContext.Current.Session["CAPTCHA"] as Captcha;

            bool result = key != null && captcha != null && string.Equals(captcha.Key, key, Captcha.Comparison);

            HttpContext.Current.Session["CAPTCHA"] = null;

            return result;
        }
    }
}
