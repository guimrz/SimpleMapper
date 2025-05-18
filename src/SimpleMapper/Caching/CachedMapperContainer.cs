using SimpleMapper.Abstractions;
using SimpleMapper.Exceptions;
using System.Collections.Concurrent;
using System.Reflection;

namespace SimpleMapper.Caching
{
    /// <summary>
    /// Provides caching for mapper instances based on source and destination types, improving performance by avoiding repetitive reflection-based resolution.
    /// </summary>
    public class CachedMapperContainer : ICachedMapperContainer
    {
        /// <summary>
        /// The internal dictionary used to cache <see cref="CachedMapper"/> instances based on a combination of source and destination types.
        /// </summary>
        protected readonly ConcurrentDictionary<CachedMapperKey, CachedMapper> cachedMappers = new();

        /// <summary>
        /// Resolves a cached <see cref="CachedMapper"/> instance for the given source and destination types.
        /// If not present in the cache, it builds and stores it.
        /// </summary>
        /// <param name="sourceType">The source type of the mapping.</param>
        /// <param name="destinationType">The destination type of the mapping.</param>
        /// <returns>A <see cref="CachedMapper"/> instance capable of mapping between the given types.</returns>
        public CachedMapper ResolveMapper(Type sourceType, Type destinationType)
        {
            return cachedMappers.GetOrAdd(new(sourceType, destinationType), BuildCachedMapper);
        }

        /// <summary>
        /// Builds a new <see cref="CachedMapper"/> using reflection for the given type pair.
        /// </summary>
        /// <param name="key">A key representing the source and destination types.</param>
        /// <returns>A <see cref="CachedMapper"/> instance for the given key.</returns>
        /// <exception cref="MappingException">
        /// Thrown if the mapping method cannot be found on the target mapper type.
        /// </exception>
        protected static CachedMapper BuildCachedMapper(CachedMapperKey key)
        {
            Type mapperType = typeof(ITypeMapper<,>).MakeGenericType(key.SourceType, key.DestinationType);

            MethodInfo? mapMethod = mapperType.GetMethod(nameof(ITypeMapper<object, object>.Map), [key.SourceType]);

            if (mapMethod is null)
            {
                throw new MappingException($"Could not find a matching 'Map({key.SourceType.Name})' method on '{mapperType.Name}'.");
            }

            return new(mapperType, mapMethod);
        }
    }
}
