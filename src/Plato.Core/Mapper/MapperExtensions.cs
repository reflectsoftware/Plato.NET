// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Threading.Tasks;

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


    /// <summary>
    /// 
    /// </summary>
    public static class MapperAsyncExtensions
    {
        /// <summary>
        /// Maps the asynchronous.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static async Task<TTarget> MapAsync<TSource, TTarget>(this IMapperAsync<TSource, TTarget> mapper, TSource source, TTarget target, Func<TSource, TTarget, Task> action = null)
            where TSource : class
            where TTarget : class
        {
            await mapper.MapAsync(source, target);

            if (action != null)
            {
                await action.Invoke(source, target);
            }

            return target;
        }

        /// <summary>
        /// Maps the asynchronous.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static async Task<TTarget> MapAsync<TSource, TTarget>(this IMapperAsync<TSource, TTarget> mapper, TSource source, Func<TSource, TTarget, Task> action = null)
            where TSource : class
            where TTarget : class
        {
            var target = Activator.CreateInstance<TTarget>();
            return await mapper.MapAsync(source, target, action);
        }

        /// <summary>
        /// Maps the asynchronous.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="target">The target.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static async Task<TTarget> MapAsync<TSource, TTarget>(this TSource source, IMapperAsync<TSource, TTarget> mapper, TTarget target, Func<TSource, TTarget, Task> action = null)
            where TSource : class
            where TTarget : class
        {
            return await mapper.MapAsync(source, target, action);
        }

        /// <summary>
        /// Maps the asynchronous.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static async Task<TTarget> MapAsync<TSource, TTarget>(this TSource source, IMapperAsync<TSource, TTarget> mapper, Func<TSource, TTarget, Task> action = null)
            where TSource : class
            where TTarget : class
        {
            var target = Activator.CreateInstance<TTarget>();
            return await mapper.MapAsync(source, target, action);
        }
    }
}
