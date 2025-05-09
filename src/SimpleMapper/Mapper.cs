using SimpleMapper.Abstractions;
using System.Collections.Concurrent;
using System.Reflection;

namespace SimpleMapper
{
    public partial class Mapper : IMapper
    {
        private readonly ConcurrentDictionary<(Type Source, Type Destination), CachedMapper> _cache = new();

        protected readonly IServiceProvider serviceProvider;

        public Mapper(IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);

            this.serviceProvider = serviceProvider;
        }

        public TDestination Map<TDestination>(object? source)
        {
            Type destinationType = typeof(TDestination);

            if (source is null)
            {
                if (IsNullable(destinationType))
                {
                    return default!;
                }

                ThrowNullSourceMappingException<TDestination>();
            }

            return (TDestination)InvokeMap(source!, destinationType)!;
        }

        private object? InvokeMap(object source, Type destinationType)
        {
            Type sourceType = source.GetType();

            var cachedMap = ResolveCachedMapper(sourceType, destinationType);

            return cachedMap.Invoke(source, serviceProvider); ;
        }

        private CachedMapper ResolveCachedMapper(Type sourceType, Type destinationType)
        {
            return _cache.GetOrAdd((sourceType, destinationType), static key =>
            {
                var (source, destination) = key;
                Type mapperType = typeof(ITypeMapper<,>).MakeGenericType(source, destination);

                MethodInfo? mapMethod = mapperType.GetMethod(nameof(ITypeMapper<object, object>.Map), [source]);

                if (mapMethod is null)
                {
                    ThrowMissingMapMethod(source, mapperType);
                }

                return new(mapperType, mapMethod);
            });
        }

        private static bool IsNullable(Type type)
        {
            return Nullable.GetUnderlyingType(type) is not null || !type.IsValueType;
        }
    }
}
