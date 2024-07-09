using Ganss.Xss;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace FIT5032_Project.CustomAttributes
{
    public class SanitiseAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string input = (string)value;
            // Initialize an instance of the HtmlSanitizer class (library used for cleaning & sanitizing HTML content).
            //var sanitizer = new HtmlSanitizer();
            // Use HtmlSanitizer to sanitize the HTML content.
            var sanitized = HttpUtility.HtmlEncode(input);

            HttpUtility.HtmlEncode(input);

            // Check if the sanitized string is equal to the original input.
            if (string.Equals(input, sanitized, StringComparison.Ordinal))
            {
                return ValidationResult.Success; // Input is properly sanitized.
            }
            else
            {
                return new ValidationResult("Input contains potentially unsafe HTML content.");
            }

        }
    }
}
