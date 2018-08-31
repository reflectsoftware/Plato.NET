// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Threading.Tasks;

namespace Plato.Core.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    public interface IMapper<T1, T2>
    {
        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        void Map(T1 source, T2 target);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    public interface IMapperAsync<T1, T2>
    {
        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        Task MapAsync(T1 source, T2 target);
    }
}