using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Domain.Interfaces
{
    /// <summary>
    /// IEntity interface - Marker interface for all entities
    /// Theory: Allows generic repository to work with any entity type
    /// Ensures all entities have an Id property for CRUD operations
    /// </summary>
    /// <typeparam name="T">Type of the primary key</typeparam>
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
