using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Application.Common
{
    /// <summary>
    /// ServiceResolver Implementation
    /// Theory: Wraps IServiceProvider from Microsoft.Extensions.DependencyInjection
    /// Acts as a facade - simpler interface than IServiceProvider
    /// </summary>
    public class ServiceResolver : IServiceResolver
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor injection of IServiceProvider
        /// Theory: IServiceProvider is registered automatically by ASP.NET Core
        /// It's the root of the dependency injection container
        /// </summary>
        public ServiceResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider
                ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Resolve optional service
        /// Theory: GetService returns null if not registered
        /// Safe for optional dependencies
        /// </summary>
        public T? Resolve<T>() where T : class
        {
            return _serviceProvider.GetService<T>();
        }

        /// <summary>
        /// Resolve required service
        /// Theory: GetRequiredService throws if not registered
        /// Better error message than null reference exception
        /// Fails fast - detect configuration errors immediately
        /// </summary>
        public T ResolveRequired<T>() where T : class
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// Resolve all implementations
        /// Theory: GetServices returns IEnumerable of all registrations
        /// Example: If you register 3 IPaymentProcessor implementations
        /// This returns all 3, not just the last one registered
        /// </summary>
        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return _serviceProvider.GetServices<T>();
        }

        /// <summary>
        /// Check registration
        /// Theory: Try to resolve, check if result is not null
        /// Doesn't throw exception, just returns bool
        /// </summary>
        public bool IsRegistered<T>() where T : class
        {
            return _serviceProvider.GetService<T>() != null;
        }

        /// <summary>
        /// Create scope
        /// Theory: IServiceScopeFactory creates new scopes
        /// Each scope has its own instances of scoped services
        /// </summary>
        public IServiceScope CreateScope()
        {
            var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
            var scope = scopeFactory.CreateScope();
            return new ServiceScopeWrapper(scope);
        }

        /// <summary>
        /// Wrapper for Microsoft's IServiceScope
        /// Theory: Adapter pattern - make Microsoft's interface match ours
        /// </summary>
        private class ServiceScopeWrapper : IServiceScope
        {
            private readonly Microsoft.Extensions.DependencyInjection.IServiceScope _scope;

            public ServiceScopeWrapper(Microsoft.Extensions.DependencyInjection.IServiceScope scope)
            {
                _scope = scope;
            }

            public IServiceProvider ServiceProvider => _scope.ServiceProvider;

            public void Dispose()
            {
                _scope.Dispose();
            }
        }
    }
}
