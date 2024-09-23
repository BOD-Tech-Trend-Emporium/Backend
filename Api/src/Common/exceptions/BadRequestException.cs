using System.Globalization;

namespace Api.src.Common.exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() : base("Check the fields")
        {

        }
        public BadRequestException(string message) : base(message)
        {

        }

        public BadRequestException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
