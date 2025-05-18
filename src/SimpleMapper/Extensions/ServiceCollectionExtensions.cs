using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimpleMapper.Abstractions;
using SimpleMapper.Caching;
using System.Reflection;

namespace SimpleMapper.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the SimpleMapper services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection to add the mapper to.</param>
        /// <returns>The <paramref name="services"/> instance for fluent chainning.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is null.</exception>
        public static IServiceCollection AddSimpleMapper(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.TryAddScoped<IMapper, Mapper>();
            services.TryAddSingleton<ICachedMapperContainer, CachedMapperContainer>();

            return services;
        }

        /// <summary>
        /// Registers a type mapper of the specified generic type in the service collection.
        /// </summary>
        /// <typeparam name="TTypeMapper">The mapper type implementing <c>ITypeMapper&lt;TSource, TDestination&gt;</c>.</typeparam>
        /// <param name="services">The service collection to add the mapper to.</param>
        /// <returns>The <paramref name="services"/> instance for fluent chainning.</returns>
        public static IServiceCollection RegisterTypeMapper<TTypeMapper>(this IServiceCollection services)
            where TTypeMapper : class
        {
            return services.RegisterTypeMapper(typeof(TTypeMapper));
        }

        /// <summary>
        /// Registers a specific mapper type in the service collection if it implements one or more <c>ITypeMapper&lt;TSource, TDestination&gt;</c> interfaces.
        /// </summary>
        /// <param name="services">The service collection to add the mapper to.</param>
        /// <param name="mapperType">The mapper type to register.</param>
        /// <returns>The <paramref name="services"/> instance for fluent chainning.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="mapperType"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when <paramref name="mapperType"/> does not implement any ITypeMapper interface.</exception>
        public static IServiceCollection RegisterTypeMapper(this IServiceCollection services, Type mapperType)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(mapperType);

            if (!IsValidMapperType(mapperType))
            {
                throw new InvalidOperationException($"The type '{mapperType.FullName}' does not implement any ITypeMapper<TSource, TDestination> interface.");
            }

            foreach (var mapperTypeDefinition in GetImplementedTypeMapperInterfaces(mapperType))
            {
                services.AddScoped(mapperTypeDefinition, mapperType);
            }

            return services;
        }

        /// <summary>
        /// Scans the specified assembly and registers all valid mappers that implement <c>ITypeMapper&lt;TSource, TDestination&gt;</c>.
        /// </summary>
        /// <param name="services">The service collection to add the mappers to.</param>
        /// <param name="assembly">The assembly to scan for mapper types.</param>
        /// <returns>The <paramref name="services"/> instance for fluent chainning.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="assembly"/> is null.</exception>
        public static IServiceCollection RegisterTypeMappersFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(assembly);

            IEnumerable<Type> mapperTypes = assembly
                .GetTypes()
                .Where(IsValidMapperType)
                .ToArray();

            foreach (var mapperType in mapperTypes)
            {
                services.RegisterTypeMapper(mapperType);
            }

            return services;
        }

        private static bool IsValidMapperType(Type mapperType)
        {
            return !mapperType.IsAbstract
                && !mapperType.IsInterface
                && mapperType.IsClass
                && mapperType.GetInterfaces()
                    .Any(i => i.IsGenericType
                        && i.GetGenericTypeDefinition() == typeof(ITypeMapper<,>));
        }

        private static Type[] GetImplementedTypeMapperInterfaces(Type mapperType)
        {
            return mapperType.GetInterfaces()
                .Where(i => i.IsGenericType
                    && i.GetGenericTypeDefinition() == typeof(ITypeMapper<,>))
                .ToArray();
        }
    }

}
