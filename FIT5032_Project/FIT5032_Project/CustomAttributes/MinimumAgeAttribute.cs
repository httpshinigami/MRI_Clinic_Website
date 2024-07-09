using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FIT5032_Project.CustomAttributes
{
    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public MinimumAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateOfBirth)
            {
                // Calculate the age based on the entered date of birth.
                int age = DateTime.Now.Year - dateOfBirth.Year;

                // Adjust age if the birthday hasn't occurred yet this year.
                if (DateTime.Now < dateOfBirth.AddYears(age))
                {
                    age--;
                }

                // Check if the age meets the minimum requirement.
                if (age < _minimumAge)
                {
                    return new ValidationResult($"You must be at least {_minimumAge} years old.");
                }
            }

            // If the date of birth is not valid or the age requirement is met, return success.
            return ValidationResult.Success;
        }
    }
}