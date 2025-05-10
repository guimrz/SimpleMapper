namespace SimpleMapper.Caching
{
    public interface ICachedMapperContainer
    {
        CachedMapper ResolveMapper(Type sourceType, Type destinationType);
    }
}
