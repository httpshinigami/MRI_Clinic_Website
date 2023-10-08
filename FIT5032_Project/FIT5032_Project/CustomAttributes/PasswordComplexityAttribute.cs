using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FIT5032_Project.CustomAttributes
{
    public class PasswordComplexityAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || !(value is string))
            {
                return false;
            }

            string password = (string)value;

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            // Check if the password contains at least one uppercase letter.
            if (!password.Any(char.IsUpper))
            {
                ErrorMessage = "The password must contain at least one uppercase letter.";
                return false;
            }

            // Check if the password contains at least one lowercase letter.
            if (!password.Any(char.IsLower))
            {
                ErrorMessage = "The password must contain at least one lowercase letter.";
                return false;
            }

            // Check if the password contains at least one digit.
            if (!password.Any(char.IsDigit))
            {
                ErrorMessage = "The password must contain at least one digit.";
                return false;
            }

            // Check if the password contains at least one special character.
            if (!password.Any(c => !char.IsLetterOrDigit(c)))
            {
                ErrorMessage = "The password must contain at least one special character.";
                return false;
            }

            return true;
        }
    }
}