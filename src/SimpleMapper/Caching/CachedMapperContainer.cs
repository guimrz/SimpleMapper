using SimpleMapper.Abstractions;
using SimpleMapper.Exceptions;
using System.Collections.Concurrent;
using System.Reflection;

namespace SimpleMapper.Caching
{
    public class CachedMapperContainer : ICachedMapperContainer
    {
        protected readonly ConcurrentDictionary<CachedMapperKey, CachedMapper> cachedMappers = new();

        public CachedMapper ResolveMapper(Type sourceType, Type destinationType)
        {
            return cachedMappers.GetOrAdd(new(sourceType, destinationType), BuildCachedMapper);
        }

        private static CachedMapper BuildCachedMapper(CachedMapperKey key)
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
