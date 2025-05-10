using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimpleMapper.Abstractions;
using SimpleMapper.Caching;
using System.Reflection;

namespace SimpleMapper.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSimpleMapper(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.TryAddScoped<IMapper, Mapper>();
            services.TryAddSingleton<ICachedMapperContainer, CachedMapperContainer>();

            return services;
        }

        public static IServiceCollection RegisterTypeMapper<TTypeMapper>(this IServiceCollection services)
            where TTypeMapper : class
        {
            return services.RegisterTypeMapper(typeof(TTypeMapper));
        }

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
