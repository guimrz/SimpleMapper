using SimpleMapper.Exceptions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SimpleMapper
{
    public partial class Mapper
    {
        [StackTraceHidden]
        [DoesNotReturn]
        private static void ThrowNullSourceMappingException<TDestination>()
        {
            throw new MappingException($"""
                Cannot map 'null' to non-nullable destination type '{typeof(TDestination).FullName}'.
                If null mapping is expected, consider using a nullable destination type.
                """);
        }

        [StackTraceHidden]
        [DoesNotReturn]
        private static void ThrowMissingMapMethod(Type sourceType, Type mapperType)
        {
            throw new MappingException($"Could not find a matching 'Map({sourceType.Name})' method on '{mapperType.Name}'.");
        }
    }
}
