using SimpleMapper.Exceptions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SimpleMapper.Caching
{
    /// <summary>
    /// Encapsulates the logic to invoke a strongly-typed mapper method resolved at runtime, caching the necessary reflection metadata for performance.
    /// </summary>
    public sealed class CachedMapper
    {
        private readonly Type mapperType;
        private readonly MethodInfo method;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedMapper"/> class with the specified mapper type and method information.
        /// </summary>
        /// <param name="mapperType">The type of the mapper implementing <c>ITypeMapper&lt;TSource, TDestination&gt;</c>.</param>
        /// <param name="method">The mapping method to be invoked.</param>
        public CachedMapper(Type mapperType, MethodInfo method)
        {
            this.mapperType = mapperType;
            this.method = method;
        }

        /// <summary>
        /// Invokes the cached map method on the registered mapper instance.
        /// </summary>
        /// <param name="source">The source object to map from.</param>
        /// <param name="serviceProvider">The service provider used to resolve the mapper instance.</param>
        /// <returns>The result of the mapping operation.</returns>
        /// <exception cref="MappingException">
        /// Thrown if the mapper instance could not be resolved from the service provider.
        /// </exception>
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
            throw new MappingException($"Could not resolve a mapper from '{sourceType.FullName}' to '{destinationType}'. Ensure that 'ITypeMapper<TSource, TDestination>' is registered.");
        }
    }
}
