using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FIT5032_Project.CustomAttributes
{
    using System.Web.Mvc;

    public class SecurityHeaderAttribute : ActionFilterAttribute
    {
        // Set security-related headers
        // These lines of code are essential for enhancing the security of a web application
        // by implementing various security policies to protect against common web vulnerabilities
        // and improve the overall security posture of the website.
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;

            //response.Headers.Add("Content-Security-Policy", "...");

            // Instruct browser not to override the response's "Content-Type" header,
            // preventing browsers from interpreting files as something other than what the server intended.
            // This can help mitigate certain types of attacks like MIME type sniffing.
            response.Headers.Add("X-Content-Type-Options", "nosniff");

            // Prevent the web page from being displayed in a frame or iframe on a different origin (domain).
            // It helps protect against clickjacking attacks by ensuring the page can only be embedded in frames from the same origin.
            response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

            //  Ensure all communication with the site occurs over a secure HTTPS connection, reducing the risk of man-in-the-middle attacks.
            response.Headers.Add("Strict-Transport-Security", "max-age=31536000");

            base.OnActionExecuted(filterContext);
        }
    }

}