using System;

namespace Plato.Core.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    public static class MapperExtensions
    {
        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <typeparam name="TMapper">The type of the mapper.</typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static TTarget Map<TMapper, TSource, TTarget>(this TMapper mapper, TSource source, TTarget target)
            where TSource : class
            where TTarget : class
            where TMapper : IMapper<TSource, TTarget>
        {
            mapper.Map(source, target);
            return target;
        }

        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <typeparam name="TMapper">The type of the mapper.</typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static TTarget Map<TMapper, TSource, TTarget>(this TMapper mapper, TSource source)
            where TSource : class
            where TTarget : class
            where TMapper : IMapper<TSource, TTarget>
        {
            var target = Activator.CreateInstance<TTarget>();
            return mapper.Map<TMapper, TSource, TTarget>(source, target);
        }

        /// <summary>
        /// Maps the specified mapper.
        /// </summary>
        /// <typeparam name="TMapper">The type of the mapper.</typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static TTarget Map<TMapper, TSource, TTarget>(this TSource source, TMapper mapper, TTarget target)
            where TSource : class
            where TTarget : class
            where TMapper : IMapper<TSource, TTarget>
        {
            return mapper.Map<TMapper, TSource, TTarget>(source, target);
        }

        /// <summary>
        /// Maps the specified mapper.
        /// </summary>
        /// <typeparam name="TMapper">The type of the mapper.</typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns></returns>
        public static TTarget Map<TMapper, TSource, TTarget>(this TSource source, TMapper mapper)
            where TSource : class
            where TTarget : class
            where TMapper : IMapper<TSource, TTarget>
        {
            var target = Activator.CreateInstance<TTarget>();
            return mapper.Map<TMapper, TSource, TTarget>(source, target);
        }

        /// <summary>
        /// Maps the specified target.
        /// </summary>
        /// <typeparam name="TMapper">The type of the mapper.</typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static TTarget Map<TMapper, TSource, TTarget>(this TSource source, TTarget target)
            where TSource : class
            where TTarget : class
            where TMapper : IMapper<TSource, TTarget>
        {
            var mappper = (TMapper)Activator.CreateInstance(typeof(TMapper));
            return mappper.Map<TMapper, TSource, TTarget>(source, target);
        }

        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <typeparam name="TMapper">The type of the mapper.</typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static TTarget Map<TMapper, TSource, TTarget>(this TSource source)
            where TSource : class
            where TTarget : class
            where TMapper : IMapper<TSource, TTarget>
        {
            var target = Activator.CreateInstance<TTarget>();
            return source.Map<TMapper, TSource, TTarget>(target);
        }

    }
}
