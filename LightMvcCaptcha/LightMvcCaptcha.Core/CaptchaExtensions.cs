using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text.Encodings.Web;

namespace LightMvcCaptcha.Core
{
    public static class CaptchaExtensions
    {
        /// <summary>
        /// Creates captcha img tag
        /// </summary>
        /// <param name="expression">Property with CaptchaAttribute</param>
        /// <param name="htmlAttributes">HTML attributes that will be applied to generated html code</param>
        /// <returns></returns>
        public static IHtmlContent CaptchaFor<TModel, TValue>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression
                ?? throw new InvalidOperationException("Expression must be a member expression");
            TagBuilder tag = new TagBuilder("img");
            if (htmlAttributes != null) tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tag.Attributes.Add("src", CaptchaMiddleware.CaptchaPath);
            tag.Attributes.Add("onclick",  "this.src+=''");
            return tag.RenderSelfClosingTag();
        }
    }
}
