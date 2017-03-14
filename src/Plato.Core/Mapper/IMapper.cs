namespace Plato.Core.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    public interface IMapper<T1, T2>
    {
        T2 Map(T1 source, T2 target);
    }
}