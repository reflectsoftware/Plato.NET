// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

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
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static TTarget Map<TSource, TTarget>(this IMapper<TSource, TTarget> mapper, TSource source, TTarget target, Action<TSource, TTarget> action = null)
            where TSource : class
            where TTarget : class            
        {
            mapper.Map(source, target);
            action?.Invoke(source, target);
            return target;
        }

        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static TTarget Map<TSource, TTarget>(this IMapper<TSource, TTarget> mapper, TSource source, Action<TSource, TTarget> action = null)
            where TSource : class
            where TTarget : class            
        {
            var target = Activator.CreateInstance<TTarget>();
            return mapper.Map(source, target, action);
        }

        /// <summary>
        /// Maps the specified mapper.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="target">The target.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static TTarget Map<TSource, TTarget>(this TSource source, IMapper<TSource, TTarget> mapper, TTarget target, Action<TSource, TTarget> action = null)
            where TSource : class
            where TTarget : class            
        {
            return mapper.Map(source, target, action);
        }

        /// <summary>
        /// Maps the specified mapper.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static TTarget Map<TSource, TTarget>(this TSource source, IMapper<TSource, TTarget> mapper, Action<TSource, TTarget> action = null)
            where TSource : class
            where TTarget : class            
        {
            var target = Activator.CreateInstance<TTarget>();
            return mapper.Map(source, target, action);
        }
    }
}
