using System.Globalization;

namespace Api.src.Common.exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base("Check the fields")
        {

        }
        public ValidationException(string message) : base(message)
        {

        }

        public ValidationException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
