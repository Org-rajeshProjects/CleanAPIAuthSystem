using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Domain.Exceptions
{
    public class InvalidTokenException : DomainException
    {
        public InvalidTokenException()
            : base("Invalid or expired token.", "INVALID_TOKEN")
        {
        }
    }
}
