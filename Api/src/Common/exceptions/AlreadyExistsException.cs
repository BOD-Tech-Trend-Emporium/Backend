using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Api.src.Common.exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException() : base("Conflict")
        {

        }

        public AlreadyExistsException(string message) : base(message)
        {

        }

        public AlreadyExistsException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
            
        }
    }
}