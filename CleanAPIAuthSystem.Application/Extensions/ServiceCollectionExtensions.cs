using CleanAPIAuthSystem.Application.Common;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace CleanAPIAuthSystem.Application.Extensions
{
    /// <summary>
    /// Extension methods for IServiceCollection
    /// Theory: Extension methods add functionality to existing types
    /// Allows fluent API: services.AddApplication().AddInfrastructure()
    /// Keeps Startup.cs/Program.cs clean
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register all Application layer services
        /// Theory: Single method to register entire layer
        /// Encapsulates registration logic, easier to maintain
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register Service Resolver
            // Theory: Singleton lifetime - one instance for entire app
            // ServiceResolver has no state, safe to share across requests
            services.AddSingleton<IServiceResolver, ServiceResolver>();

            // Register AutoMapper for DTO mapping
            // Theory: AutoMapper scans assemblies for mapping profiles
            // Automatically maps Entity -> DTO based on property names
            // Reduces boilerplate code
            // FIXED: Correct way for AutoMapper 12.0+ which expects Action<IMapperConfigurationExpression>
            services.AddAutoMapper(config =>
            {
                // Add all profiles from this assembly
                // Theory: AutoMapper will scan for classes inheriting from Profile
                // and automatically register them
                config.AddMaps(Assembly.GetExecutingAssembly());

                // Alternative: You can explicitly add profiles
                // config.AddProfile<MappingProfile>();
                // config.AddProfile<AnotherProfile>();
            });
            // Register all services automatically
            // Theory: Scan assembly for interfaces/implementations
            // Convention: I{Name}Service -> {Name}Service
            RegisterServicesAutomatically(services);

            return services;
        }

        /// <summary>
        /// Auto-register services by convention
        /// Theory: Convention over Configuration
        /// If interface is IAuthService, implementation should be AuthService
        /// Saves manual registration for every service
        /// </summary>
        private static void RegisterServicesAutomatically(IServiceCollection services)
        {
            // Get all types in Application assembly
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();

            // Find all interfaces ending with "Service"
            var serviceInterfaces = types
                .Where(t => t.IsInterface && t.Name.EndsWith("Service"))
                .ToList();

            foreach (var serviceInterface in serviceInterfaces)
            {
                // Find implementation (remove 'I' prefix)
                // Example: IAuthService -> AuthService
                var implementationName = serviceInterface.Name.Substring(1);
                var implementation = types
                    .FirstOrDefault(t =>
                        t.IsClass &&
                        !t.IsAbstract &&
                        t.Name == implementationName);

                if (implementation != null)
                {
                    // Register as Scoped - new instance per request
                    // Theory: Services often need DbContext (scoped)
                    // Scoped ensures service and DbContext have same lifetime
                    services.AddScoped(serviceInterface, implementation);
                }
            }
        }

        /// <summary>
        /// Extension method to register custom validator
        /// Theory: FluentValidation for complex validation rules
        /// More powerful than Data Annotations
        /// </summary>
        public static IServiceCollection AddCustomValidators(this IServiceCollection services)
        {
            // Register validators from assembly
            // Theory: Validators are singletons (no state)
            // One instance can validate many requests
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
