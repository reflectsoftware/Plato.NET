// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Plato.Messaging.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageSenderFactory<TData>
    {
        /// <summary>
        /// Creates the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        IMessageSender<TData> Create(IMessageSettings settings);
    }
}
