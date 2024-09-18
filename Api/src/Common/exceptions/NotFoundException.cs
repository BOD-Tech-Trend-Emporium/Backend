using System.Globalization;

namespace Api.src.Common.exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Not found.")
        {
        }
        public NotFoundException(string message) : base(message)
        {

        }

        public NotFoundException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
