// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using StackExchange.Redis;
using System;

namespace Plato.Redis.Interfaces
{
    public interface IRedisSerializer
    {
        RedisValue Serialize(object data);
        T Deserialize<T>(RedisValue value);
        T Deserialize<T>(object data);
        object Deserialize(object data, Type type);
    }
}
