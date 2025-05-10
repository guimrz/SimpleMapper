using SimpleMapper.Abstractions;
using SimpleMapper.Caching;
using SimpleMapper.Exceptions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SimpleMapper
{
    public class Mapper : IMapper
    {

        protected readonly IServiceProvider serviceProvider;
        protected readonly ICachedMapperContainer cacheMapperContainer;

        public Mapper(IServiceProvider serviceProvider, ICachedMapperContainer cacheMapperContainer)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(cacheMapperContainer);

            this.serviceProvider = serviceProvider;
            this.cacheMapperContainer = cacheMapperContainer;
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

            var cachedMap = cacheMapperContainer.ResolveMapper(sourceType, destinationType);

            return cachedMap.Invoke(source, serviceProvider); ;
        }

        private static bool IsNullable(Type type)
        {
            return Nullable.GetUnderlyingType(type) is not null || !type.IsValueType;
        }

        [StackTraceHidden]
        [DoesNotReturn]
        private static void ThrowNullSourceMappingException<TDestination>()
        {
            throw new MappingException($"Cannot map 'null' to non-nullable destination type '{typeof(TDestination).FullName}'. If null mapping is expected, consider using a nullable destination type.");
        }
    }
}
