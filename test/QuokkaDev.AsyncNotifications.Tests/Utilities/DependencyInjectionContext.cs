using QuokkaDev.AsyncNotifications.Abstractions;
using QuokkaDev.AsyncNotifications.Abstractions.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuokkaDev.AsyncNotifications.Tests.Utilities
{
    /// <summary>
    /// Help class for setup dependency injection and track mock objects
    /// </summary>
    internal class DependencyInjectionContext
    {
        private readonly IServiceCollection services;
        private readonly IList<Mock> mocks;
        private ServiceProvider? serviceProvider;

        public DependencyInjectionContext()
        {
            services = new ServiceCollection();
            services.AddAsyncNotifications(typeof(DependencyInjectionContext).Assembly);
            mocks = new List<Mock>();
        }

        /// <summary>
        /// Register a Mock ILogger<typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type of the logger</typeparam>
        /// <param name="services">The service collection where register the log</param>
        /// <returns>The original service collection. Used for chaining multiple calls</returns>
        public Mock<ILogger<T>> RegisterMockLogger<T>()
        {
            var loggerMock = new Mock<ILogger<T>>();
            mocks.Add(loggerMock);
            services.AddSingleton(loggerMock.Object);
            return loggerMock;
        }

        /// <summary>
        /// Build the service provider after the dependency injection configuration
        /// </summary>
        public void BuildServiceProvider()
        {
            serviceProvider = services.BuildServiceProvider();
            services.AddSingleton(serviceProvider);
        }

        /// <summary>
        /// Retrieve a service from the dependency injection container
        /// </summary>
        /// <typeparam name="T">Type of the requested service</typeparam>
        /// <returns>The service</returns>
        /// <exception cref="InvalidOperationException">Raised if the service provider is not initialized</exception>
        public T GetService<T>() where T : class
        {
            if(serviceProvider != null) {
                return serviceProvider.GetRequiredService<T>();
            }
            else {
                throw new InvalidOperationException("Service provider is not initialized. Ensure to call BuildServiceProvider()");
            }
        }

        public Mock<IMethodCall> RegisterMethodCall()
        {
            var methodCallMock = new Mock<IMethodCall>();
            methodCallMock.Setup(caller => caller.Call());

            mocks.Add(methodCallMock);
            services.AddSingleton(methodCallMock.Object);

            return methodCallMock;
        }
    }
}
