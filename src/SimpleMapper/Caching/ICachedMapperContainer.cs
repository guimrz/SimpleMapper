namespace SimpleMapper.Caching
{
    /// <summary>
    /// Defines a contract for resolving and caching object mappers used for converting instances
    /// from a source type to a destination type.
    /// </summary>
    public interface ICachedMapperContainer
    {
        /// <summary>
        /// Resolves a cached mapper capable of mapping objects from the specified source type
        /// to the specified destination type.
        /// </summary>
        /// <param name="sourceType">The runtime type of the source object.</param>
        /// <param name="destinationType">The desired destination type to map to.</param>
        /// <returns>
        /// A <see cref="CachedMapper"/> instance that can perform the mapping between the specified types.
        /// </returns>
        CachedMapper ResolveMapper(Type sourceType, Type destinationType);
    }
}
