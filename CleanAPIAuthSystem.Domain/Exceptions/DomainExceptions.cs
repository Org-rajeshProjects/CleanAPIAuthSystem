using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Domain.Exceptions
{
    /// <summary>
    /// DomainException - Base exception for domain-level errors
    /// Theory: Custom exceptions provide semantic meaning
    /// Different from technical exceptions (NullReferenceException, etc.)
    /// Represents business rule violations
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// Error code for programmatic handling
        /// Example: "USER_NOT_FOUND", "INVALID_EMAIL"
        /// </summary>
        public string Code { get; }

        public DomainException(string message, string code = "DOMAIN_ERROR")
            : base(message)
        {
            Code = code;
        }

        public DomainException(string message, string code, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }
    }
}
