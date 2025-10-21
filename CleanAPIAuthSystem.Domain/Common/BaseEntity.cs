using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Domain.Common
{
    /// <summary>
    /// Base Entity - All entities inherit from this
    /// Theory: DRY (Don't Repeat Yourself) principle
    /// Common properties like Id, audit fields are defined once
    /// Generic type T allows derived classes to specify their ID type
    /// </summary>
    /// <typeparam name="T">Type of the primary key (usually Guid or int)</typeparam>
    public abstract class BaseEntity<T>
    {
        /// <summary>
        /// Primary key - virtual allows derived classes to override behavior
        /// </summary>
        public virtual T Id { get; set; } = default!;

        /// <summary>
        /// Audit timestamps
        /// Theory: ISO 8601 standard for date/time representation
        /// UTC prevents timezone confusion in distributed systems
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
