using System;

namespace Plato.Core.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    public static class MapperExtensions
    {
        /// <summary>
        /// Maps the specified target.
        /// </summary>
        /// <typeparam name="TMapper">The type of the mapper.</typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static TTarget Map<TMapper, TSource, TTarget>(this TSource source, TTarget target, params object[] args)
            where TSource : class
            where TTarget : class
            where TMapper : IMapper<TSource, TTarget>
        {
            var map = (TMapper)Activator.CreateInstance(typeof(TMapper), args);

            map.Map(source, target);

            return target;
        }

        /// <summary>
        /// Maps the specified arguments.
        /// </summary>
        /// <typeparam name="TMapper">The type of the mapper.</typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static TTarget Map<TMapper, TSource, TTarget>(this TSource source, params object[] args)
            where TSource : class
            where TTarget : class
            where TMapper : IMapper<TSource, TTarget>
        {
            var target = Activator.CreateInstance<TTarget>();
            return source.Map<TMapper, TSource, TTarget>(target, args);
        }
    }
}
