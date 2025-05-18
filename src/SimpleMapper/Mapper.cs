using SimpleMapper.Abstractions;
using SimpleMapper.Caching;
using SimpleMapper.Exceptions;

namespace SimpleMapper
{
    /// <summary>
    /// Default implementation of the <see cref="IMapper"/> interface that performs type mappings using registered <c>ITypeMapper&lt;TSource, TDestination&gt;</c> implementations.
    /// </summary>
    public class Mapper : IMapper
    {
        /// <summary>
        /// Gets the service provider used to resolve type mappers.
        /// </summary>
        protected readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Gets the cached mapper container used to cache and retrieve mapper delegates.
        /// </summary>
        protected readonly ICachedMapperContainer cacheMapperContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mapper"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve mapper services.</param>
        /// <param name="cacheMapperContainer">The container responsible for caching mapping strategies.</param>
        /// <exception cref="ArgumentNullException">Thrown if any parameter is null.</exception>
        public Mapper(IServiceProvider serviceProvider, ICachedMapperContainer cacheMapperContainer)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(cacheMapperContainer);

            this.serviceProvider = serviceProvider;
            this.cacheMapperContainer = cacheMapperContainer;
        }

        /// <summary>
        /// Maps the specified <paramref name="source"/> object to an instance of <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TDestination">The type to map the source object to.</typeparam>
        /// <param name="source">The object to map.</param>
        /// <returns>An instance of <typeparamref name="TDestination"/> representing the mapped result.</returns>
        /// <exception cref="MappingException">
        /// Thrown if the source is <c>null</c> and the destination type is not nullable, or if the mapping operation fails.
        /// </exception>
        public TDestination Map<TDestination>(object? source)
        {
            Type destinationType = typeof(TDestination);

            if (source is null)
            {
                if (IsNullable(destinationType))
                {
                    return default!;
                }

                throw new MappingException($"Cannot map 'null' to non-nullable destination type '{typeof(TDestination).FullName}'. If null mapping is expected, consider using a nullable destination type.");
            }

            try
            {
                return (TDestination)InvokeMap(source!, destinationType)!;
            }
            catch (Exception ex)
            {
                throw new MappingException($"An error occurred while mapping from '{source.GetType().FullName}' to '{destinationType.FullName}'.", ex);
            }
        }

        private object? InvokeMap(object source, Type destinationType)
        {
            Type sourceType = source.GetType();

            var cachedMap = cacheMapperContainer.ResolveMapper(sourceType, destinationType);

            return cachedMap.Invoke(source, serviceProvider);
        }

        private static bool IsNullable(Type type)
        {
            return Nullable.GetUnderlyingType(type) is not null || !type.IsValueType;
        }
    }
}
