using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Application.Common
{
    /// <summary>
    /// Service Resolver Interface - Dynamic service and repository resolution
    /// Theory: Service Locator Pattern (use sparingly!)
    /// 
    /// Why we need it:
    /// - Sometimes we don't know which service we need until runtime
    /// - Useful for plugin systems or dynamic feature loading
    /// - Can resolve services by interface type
    /// 
    /// Caution: Overuse is an anti-pattern
    /// - Makes dependencies implicit (hard to see what a class needs)
    /// - Can hide the fact that class has too many dependencies
    /// - Prefer constructor injection when possible
    /// 
    /// Good use cases:
    /// - Factory patterns that create different types
    /// - Middleware that needs to resolve services per request
    /// - Generic handlers that work with many service types
    /// </summary>
    public interface IServiceResolver
    {
        /// <summary>
        /// Resolve service by type
        /// Theory: Generic method allows type-safe resolution
        /// Example: var authService = resolver.Resolve<IAuthService>();
        /// Returns null if service not registered (graceful failure)
        /// </summary>
        T? Resolve<T>() where T : class;

        /// <summary>
        /// Resolve service by type with fallback
        /// Theory: If service not found, throws exception with clear message
        /// Use when service is required (not optional)
        /// </summary>
        T ResolveRequired<T>() where T : class;

        /// <summary>
        /// Resolve all implementations of an interface
        /// Theory: Strategy pattern support
        /// Example: All payment processors, all notification providers
        /// Returns empty collection if none registered
        /// </summary>
        IEnumerable<T> ResolveAll<T>() where T : class;

        /// <summary>
        /// Check if service is registered
        /// Theory: Defensive programming - check before resolving
        /// Avoids exceptions in optional scenarios
        /// </summary>
        bool IsRegistered<T>() where T : class;

        /// <summary>
        /// Create a new scope for scoped services
        /// Theory: Scoped services (like DbContext) live for one request
        /// Sometimes need to create artificial scope outside HTTP request
        /// Example: Background jobs, console apps
        /// Returns IDisposable - must dispose to free resources
        /// </summary>
        IServiceScope CreateScope();
    }

    /// <summary>
    /// Service Scope wrapper
    /// Theory: Represents a lifetime scope for scoped services
    /// When disposed, all scoped services in that scope are disposed
    /// </summary>
    public interface IServiceScope : IDisposable
    {
        /// <summary>
        /// Service provider for this scope
        /// Theory: Can resolve services within this scope
        /// Services resolved here have their own lifetime
        /// </summary>
        IServiceProvider ServiceProvider { get; }
    }
}
