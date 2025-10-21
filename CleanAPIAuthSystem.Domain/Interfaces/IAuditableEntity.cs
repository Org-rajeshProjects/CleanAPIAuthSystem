using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Domain.Interfaces
{
    /// <summary>
    /// IAuditableEntity - Contract for entities that track changes
    /// Theory: Interface Segregation Principle (ISP)
    /// Only entities that need auditing implement this
    /// Allows infrastructure layer to automatically set audit fields
    /// </summary>
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Optional: Track who created/modified the record
        /// Useful in multi-user systems
        /// </summary>
        string? CreatedBy { get; set; }
        string? UpdatedBy { get; set; }
    }
}
