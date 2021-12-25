namespace ReflectionPractice.Infrastructure
{
    using Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services,params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(a => a.GetExportedTypes()).ToList();
            var servicesToRegister = GetTransientServicesToRegisters(types);

            if (servicesToRegister is null)
            {
                throw new ArgumentNullException();
            }

            foreach (var serviceToRegister in servicesToRegister)
            {
                services.AddTransient(serviceToRegister.ServiceInterface, serviceToRegister.ServiceClass);
            }

            return services;
        }

        public static IEnumerable<ServicesToRegister> GetTransientServicesToRegisters(IEnumerable<Type> types)
        {
            var serviceType = typeof(ITransientService);
            var serviceName = nameof(ITransientService);

            return types
                .Where(x => serviceType.IsAssignableFrom(x) && x.IsInterface && x.GetTypeInfo().Name != serviceName)
                .Select(interfaceName => types.Where(x => interfaceName.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .Select(x => new ServicesToRegister { ServiceClass = x, ServiceInterface = interfaceName, })
                    .FirstOrDefault())
                .ToList();
        }
    }
}
