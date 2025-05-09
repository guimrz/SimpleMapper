namespace SimpleMapper.Abstractions
{
    /// <summary>
    /// Defines a contract for mapping objects from a source type to a destination type.
    /// </summary>
    /// <typeparam name="TSource">The source type to map from.</typeparam>
    /// <typeparam name="TDestination">The destination type to map to.</typeparam>
    public interface ITypeMapper<TSource, TDestination>
    {
        /// <summary>
        /// Maps an instance of <typeparamref name="TSource"/> to an instance of <typeparamref name="TDestination"/>.
        /// </summary>
        /// <param name="source">The source object to map.</param>
        /// <returns>A new instance of <typeparamref name="TDestination"/> populated with values from the source.</returns>
        TDestination Map(TSource source);
    }
}
