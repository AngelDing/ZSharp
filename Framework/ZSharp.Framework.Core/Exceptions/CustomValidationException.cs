using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZSharp.Framework.Exceptions
{
    public class CustomValidationException : ValidationException
    {
        private IEnumerable<string> validationErrors;

        public IEnumerable<string> ValidationErrors
        {
            get
            {
                return validationErrors;
            }
        }

        public CustomValidationException(IEnumerable<string> validationErrors)
            : base(GetErrorMessage(validationErrors))
        {
            this.validationErrors = validationErrors;
        }

        private static string GetErrorMessage(IEnumerable<string> validationErrors)
        {
            var msg = string.Empty;
            foreach (var error in validationErrors)
            {
                msg = msg + error + ";";
            }
            return msg;
        }
    }
}
