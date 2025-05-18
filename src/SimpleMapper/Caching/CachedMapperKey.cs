namespace SimpleMapper.Caching
{
    /// <summary>
    /// Represents a unique key used for identifying a cached mapper based on a source and destination type.
    /// </summary>
    public readonly struct CachedMapperKey
    {
        /// <summary>
        /// Gets the source type used in the mapping.
        /// </summary>
        public Type SourceType { get; }

        /// <summary>
        /// Gets the destination type used in the mapping.
        /// </summary>
        public Type DestinationType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedMapperKey"/> struct with the specified source and destination types.
        /// </summary>
        /// <param name="sourceType">The type of the source object.</param>
        /// <param name="destinationType">The type of the destination object.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either <paramref name="sourceType"/> or <paramref name="destinationType"/> is <c>null</c>.
        /// </exception>
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
