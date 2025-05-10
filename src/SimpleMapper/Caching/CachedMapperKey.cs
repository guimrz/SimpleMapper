namespace SimpleMapper.Caching
{
    public class CachedMapperKey
    {
        public Type SourceType { get; init; }

        public Type DestinationType { get; init; }

        public CachedMapperKey(Type sourceType, Type destinationType)
        {
            ArgumentNullException.ThrowIfNull(sourceType);
            ArgumentNullException.ThrowIfNull(destinationType);

            SourceType = sourceType;
            DestinationType = destinationType;
        }
    }
}
