using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

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
        public static MvcHtmlString CaptchaFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
                throw new InvalidOperationException("Expression must be a member expression");

            bool usesCaptchaAttribute = memberExpression.Member.GetCustomAttributes(typeof(CaptchaAttribute), true).Any();

            if (!usesCaptchaAttribute)
                throw new InvalidOperationException("Expression member must be with CaptchaAttribute");

            var controllerName = typeof(TModel).Assembly.GetTypes()
                .Where(type => typeof(CaptchaController).IsAssignableFrom(type))
                .Select(type => type.Name.Replace("Controller", ""))
                .FirstOrDefault();

            if (controllerName == null)
                throw new InvalidOperationException("No controller that inherits from CaptchaController was found");

            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var url = urlHelper.RouteUrl(new {controller = controllerName, action = "GetCaptcha"});

            TagBuilder tag = new TagBuilder("img");

            if (htmlAttributes != null) tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tag.Attributes.Add("src", url);

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.SelfClosing));
        }
    }
}
