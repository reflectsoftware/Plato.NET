using System;

namespace Plato.Core.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    public static class MapperExtensions
    {
        /// <summary>
        /// Maps the specified arguments.
        /// </summary>
        /// <typeparam name="TMapper">The type of the mapper.</typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static TTarget Map<TMapper, TSource, TTarget>(this TSource source, params Object[] args) where TMapper : IMapper<TSource, TTarget>
        {
            var map = (TMapper)Activator.CreateInstance(typeof(TMapper), args);
            var target = Activator.CreateInstance<TTarget>();

            map.Map(source, target);

            return target;
        }
    }
}
