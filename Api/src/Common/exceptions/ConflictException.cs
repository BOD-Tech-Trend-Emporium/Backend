using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Api.src.Common.exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException() : base("Conflict")
        {

        }

        public ConflictException(string message) : base(message)
        {

        }

        public ConflictException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
            
        }
    }
}