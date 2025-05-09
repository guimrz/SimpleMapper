using SimpleMapper.Exceptions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SimpleMapper
{
    internal sealed class CachedMapper
    {
        private readonly Type mapperType;
        private readonly MethodInfo method;

        public CachedMapper(Type mapperType, MethodInfo method)
        {
            this.mapperType = mapperType;
            this.method = method;
        }

        public object Invoke(object source, IServiceProvider serviceProvider)
        {
            var mapper = serviceProvider.GetService(mapperType);

            if (mapper is null)
            {
                ThrowMapperNotRegistered(source.GetType(), mapperType.GenericTypeArguments[1]);
            }

            return method.Invoke(mapper, [source])!;
        }

        [StackTraceHidden]
        [DoesNotReturn]
        private static void ThrowMapperNotRegistered(Type sourceType, Type destinationType)
        {
            throw new MappingException($"""
                Could not resolve a mapper from '{sourceType.FullName}' to '{destinationType}'.
                Ensure that 'ITypeMapper<TSource, TDestination>' is registered.
                """);
        }
    }
}
