﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ZSharp.Framework.Validator
{
    /// <summary>
    /// Validator based on Data Annotations. 
    /// This validator use IValidatableObject interface and
    /// ValidationAttribute ( hierachy of this) for
    /// perform validation
    /// </summary>
    public class EntityValidator : IValidator
    {
        private List<string> validationErrors;
        
        public bool IsValid<T>(T item) where T : class
        {
            if (item == null)
            {
                return false;
            }

            validationErrors = new List<string>();

            SetValidatableObjectErrors(item);
            SetValidationAttributeErrors(item);

            return !validationErrors.Any();
        }

        public IEnumerable<string> GetInvalidMessages()
        {
            return validationErrors;
        }

        #region Private Methods

        /// <summary>
        /// Get erros if object implement IValidatableObject
        /// </summary>
        /// <typeparam name="T">The typeof entity</typeparam>
        /// <param name="item">The item to validate</param>
        private void SetValidatableObjectErrors<T>(T item) where T : class
        {
            if (typeof(IValidatableObject).IsAssignableFrom(typeof(T)))
            {
                var validationContext = new ValidationContext(item, null, null);

                var validationResults = ((IValidatableObject)item).Validate(validationContext);
                if (validationResults != null)
                {
                    foreach (var vr in validationResults)
                    {
                        validationErrors.Add(string.Join(",", vr.MemberNames) + ":" + vr.ErrorMessage);
                    }
                }
            }
        }

        /// <summary>
        /// Get errors on ValidationAttribute
        /// </summary>
        /// <typeparam name="T">The type of entity</typeparam>
        /// <param name="item">The entity to validate</param>
        private void SetValidationAttributeErrors<T>(T item) where T : class
        {
            var result = from property in TypeDescriptor.GetProperties(item).Cast<PropertyDescriptor>()
                         from attribute in property.Attributes.OfType<ValidationAttribute>()
                         where !attribute.IsValid(property.GetValue(item))
                         select attribute.FormatErrorMessage(string.Empty);

            if (result != null && result.Any())
            {
                validationErrors.AddRange(result);
            }
        }

        #endregion
    }
}
