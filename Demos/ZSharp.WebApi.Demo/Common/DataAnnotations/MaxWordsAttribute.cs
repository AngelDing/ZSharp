using System.ComponentModel.DataAnnotations;

namespace ZSharp.WebApi.Demo.Common.DataAnnotations
{
    /// <summary>
    /// This code comes from the book "Professional ASP.NET MVC 5".
    /// </summary>
    public class MaxWordsAttribute : ValidationAttribute
    {
        private readonly int _maxWords;

        public MaxWordsAttribute(int maxWords)
        {
            _maxWords = maxWords;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var valueAsString = value.ToString();

                if (valueAsString.Split(' ').Length > _maxWords)
                {
                    return new ValidationResult("Too many words!");
                }
            }

            return ValidationResult.Success;
        }
    }
}