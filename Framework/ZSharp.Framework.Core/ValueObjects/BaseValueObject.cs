using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZSharp.Framework.Validator;

namespace ZSharp.Framework.ValueObjects
{
    /// <summary>
    /// DDD中值對象的基類
    /// </summary>
    [Serializable]
    public abstract class BaseValueObject : IValidatableObject
    {
        public BaseValueObject()
        {
        }

        public void ThrowExceptionIfInvalid()
        {
            var validator = new EntityValidator();
            if (!validator.IsValid(this))
            {
                throw new ValidationException(validator.GetInvalidMessages());
            }
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}
