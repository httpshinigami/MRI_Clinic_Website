using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FIT5032_Project.CustomAttributes
{
    using System.Web.Mvc;

    public class SecurityHeaderAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;

            // Set security-related headers
            response.Headers.Add("Content-Security-Policy", "...");
            response.Headers.Add("X-Content-Type-Options", "nosniff");
            response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            response.Headers.Add("Strict-Transport-Security", "max-age=31536000");

            base.OnActionExecuted(filterContext);
        }
    }

}