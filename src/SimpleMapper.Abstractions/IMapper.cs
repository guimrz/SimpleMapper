namespace SimpleMapper.Abstractions
{
    /// <summary>
    /// Defines a contract for mapping objects from any source type to a destination type.
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// Maps an object of any type to a specified destination type.
        /// </summary>
        /// <typeparam name="TDestination">The type to map to.</typeparam>
        /// <param name="source">The source object to map from. It can be of any type.</param>
        /// <returns>An instance of <typeparamref name="TDestination"/> populated with values from the source.</returns>
        TDestination Map<TDestination>(object? source);
    }
}
