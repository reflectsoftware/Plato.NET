// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections.Generic;

namespace Plato.Redis.Interfaces
{
    public interface IRedisList<T> : IRedisControl, IList<T>
    {
    }
}
