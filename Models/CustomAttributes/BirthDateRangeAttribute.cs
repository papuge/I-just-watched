using System;
using System.ComponentModel.DataAnnotations;

namespace IJustWatched.Models.CustomAttributes
{
    public class BirthDateRangeAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            value = (DateTime)value;
            bool lessThan100Ago = DateTime.Now.AddYears(-120).CompareTo(value) <= 0;
            bool lessThanNow = DateTime.Now.CompareTo(value) >= 0;
            if (lessThan100Ago && lessThanNow)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Impossible birth date");
        }
    }
}