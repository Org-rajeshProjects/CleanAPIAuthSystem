using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Domain.Interfaces
{
    /// <summary>
    /// ISoftDeletable - Contract for entities that support soft delete
    /// Theory: Soft delete pattern - mark as deleted instead of removing from database
    /// Benefits:
    /// 1. Data recovery - can undelete records
    /// 2. Audit trail - know what was deleted and when
    /// 3. Referential integrity - related records still reference valid IDs
    /// 4. Compliance - some regulations require data retention
    /// </summary>
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
        string? DeletedBy { get; set; }
    }

}
