using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace LightMvcCaptcha.Core
{
    public static class AppBuilderExtension
    {
        public static void UseCaptcha(this IApplicationBuilder app, string path= "/captcha-image")
        {
            CaptchaMiddleware.CaptchaPath = path;
            app.UseWhen(
                ctx => ctx.Request.Path == CaptchaMiddleware.CaptchaPath,
                bld => { bld.UseMiddleware<CaptchaMiddleware>(); }
                );
        }
    }

    public class CaptchaMiddleware
    {
        //private readonly RequestDelegate _next;
        public CaptchaMiddleware(RequestDelegate _next)
        {
            //this._next = _next;
        }

        public static string CaptchaPath;
        public async Task Invoke(HttpContext context)
        {
            Captcha captcha = Captcha.Generate();
            context.Session.SetString("CAPTCHA", captcha.Key);
            context.Response.Headers.Add("Content-Type", "image/jpeg");
            context.Response.ContentLength = captcha.Image.Length;
            await context.Response.Body.WriteAsync(captcha.Image, 0, captcha.Image.Length);
            captcha.DisposeImage();
        }


    }

}
