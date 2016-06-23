using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZSharp.Framework.Validator;
using ZSharp.Framework.Utils;
using ZSharp.Framework.Exceptions;

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

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            if (object.ReferenceEquals(null, obj))
            {
                return false;
            }
            if (this.GetType() != obj.GetType())
            {
                return false;
            }
            BaseValueObject valueObject = obj as BaseValueObject;
            return this.GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return HashCodeHelper.CombineHashCodes(this.GetEqualityComponents());
        }

        public void ThrowExceptionIfInvalid()
        {
            var validator = new EntityValidator();
            if (!validator.IsValid(this))
            {
                throw new CustomValidationException(validator.GetInvalidMessages());
            }
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}
