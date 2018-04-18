using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
     

namespace LightMvcCaptcha.Core
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CaptchaAttribute : Attribute, IActionFilter
    {
        public bool IsReusable => true;

        string propName;
        string message;
        public CaptchaAttribute(string propName,string message)
        {
            this.propName = propName;
            this.message = message;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string actual   = context.HttpContext.Request.Form[propName];
            string expected = context.HttpContext.Session.GetString("CAPTCHA");
            context.HttpContext.Session.Remove("CAPTCHA");

            if (actual == null || expected == null || !string.Equals(actual, expected, Captcha.Comparison))
                context.ModelState.AddModelError(propName, message);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }


    }
}
