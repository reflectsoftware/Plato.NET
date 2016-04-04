// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Security.Interfaces;

namespace Plato.Configuration.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConfigValueIntercept
    {
        /// <summary>
        /// Gets or sets the encryption provider.
        /// </summary>
        /// <value>
        /// The encryption provider.
        /// </value>
        IEncryptionProvider EncryptionProvider { get; set; }

        /// <summary>
        /// Adds the variables.
        /// </summary>
        /// <param name="configContainer">The configuration container.</param>
        /// <param name="bForceRefresh">if set to <c>true</c> [b force refresh].</param>
        void AddVariables(IConfigContainer configContainer, bool bForceRefresh = true);

        /// <summary>
        /// Removes the variables.
        /// </summary>
        /// <param name="configContainer">The configuration container.</param>
        /// <param name="bForceRefresh">if set to <c>true</c> [b force refresh].</param>
        void RemoveVariables(IConfigContainer configContainer, bool bForceRefresh = true);

        /// <summary>
        /// Values the intercept.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        string ValueIntercept(string value);
    }
}
