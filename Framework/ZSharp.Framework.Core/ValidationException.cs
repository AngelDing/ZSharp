using System.Collections.Generic;

namespace ZSharp.Framework
{
    public class ValidationException  : FrameworkException
    {
        private IEnumerable<string> validationErrors;

        public IEnumerable<string> ValidationErrors
        {
            get
            {
                return validationErrors;
            }
        }

        public ValidationException(IEnumerable<string> validationErrors)
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
