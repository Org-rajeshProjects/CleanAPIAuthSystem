using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Domain.Exceptions
{
    /// <summary>
    /// Specific domain exceptions for authentication
    /// Theory: Fail-fast principle - detect errors early and throw meaningful exceptions
    /// </summary>
    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException(string email)
            : base($"User with email '{email}' was not found.", "USER_NOT_FOUND")
        {
        }
    }
}
