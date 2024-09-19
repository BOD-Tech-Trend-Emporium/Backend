using System.Globalization;

namespace Api.src.Common.exceptions
{
    public class DuplicateException : Exception
    {
        public DuplicateException() : base("Duplicate")
        {
        }
        public DuplicateException(string message) : base(message)
        {

        }

        public DuplicateException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
