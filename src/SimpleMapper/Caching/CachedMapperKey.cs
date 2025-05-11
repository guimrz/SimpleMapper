namespace SimpleMapper.Caching
{
    public readonly struct CachedMapperKey
    {
        public Type SourceType { get; }

        public Type DestinationType { get; }

        public CachedMapperKey(Type sourceType, Type destinationType)
        {
            ArgumentNullException.ThrowIfNull(sourceType);
            ArgumentNullException.ThrowIfNull(destinationType);

            SourceType = sourceType;
            DestinationType = destinationType;
        }

        public override bool Equals(object? obj)
        {
            return obj is CachedMapperKey v && Equals(v);
        }

        private bool Equals(CachedMapperKey obj)
        {
            return SourceType == obj.SourceType && DestinationType == obj.DestinationType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SourceType, DestinationType);
        }
    }
}
