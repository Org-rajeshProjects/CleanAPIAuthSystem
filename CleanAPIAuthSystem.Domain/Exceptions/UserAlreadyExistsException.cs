using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Domain.Exceptions
{
    public class UserAlreadyExistsException : DomainException
    {
        public UserAlreadyExistsException(string email)
            : base($"User with email '{email}' already exists.", "USER_ALREADY_EXISTS")
        {
        }
    }
}
