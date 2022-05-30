using QuokkaDev.AsyncNotifications.Abstractions;
using QuokkaDev.AsyncNotifications.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace QuokkaDev.AsyncNotifications
{
    /// <summary>
    /// Extensions method for dependency injection registration
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the AsyncNotifications infrastructure.
        /// Register all the notification handlers in a given set of assemblies
        /// </summary>
        /// <param name="services">The service collection where register the AsyncNotifications</param>
        /// <param name="assemblies">An array of assemblies to scan for notification handlers registration</param>
        /// <returns>The service collection, so you can chain multiple methods</returns>
        public static IServiceCollection AddAsyncNotifications(this IServiceCollection services, params Assembly[] assemblies)
        {
            if(assemblies is null || assemblies.Length == 0) {
                assemblies = new Assembly[] { Assembly.GetCallingAssembly() };
            }

            services.AddScoped<IAsyncNotificationDispatcher, AsyncNotificationDispatcher>();

            services.Scan(selector => {
                selector.FromAssemblies(assemblies)
                        .AddClasses(filter => {
                            filter.AssignableTo(typeof(INotificationHandler<>));
                        })
                        .AsImplementedInterfaces()
                        .WithScopedLifetime();
            });

            return services;
        }
    }
}
