using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using IJustWatched.Data;
using Microsoft.Extensions.DependencyInjection;

namespace IJustWatched.Models.CustomAttributes
{
    public class FilmInDbValidation: ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (IJustWatchedContext)validationContext
                .GetService(typeof(IJustWatchedContext));
            var filmTitle = value as string;
            if (context?.Films.FirstOrDefault(film => film.Title.Equals(filmTitle,
                StringComparison.OrdinalIgnoreCase)) != null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("There's no such film in database.");
        }
    }
}