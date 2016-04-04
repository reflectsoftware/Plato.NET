// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Plato.Configuration.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISectionContentContainer
    {
        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <param name="sectionId">The section identifier.</param>
        /// <param name="contentId">The content identifier.</param>
        /// <param name="bFilter">if set to <c>true</c> [b filter].</param>
        /// <param name="bThrowOnMissing">if set to <c>true</c> [b throw on missing].</param>
        /// <returns></returns>
        string GetContent(string sectionId, string contentId, bool bFilter, bool bThrowOnMissing);

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <param name="sectionId">The section identifier.</param>
        /// <param name="contentId">The content identifier.</param>
        /// <param name="bFilter">if set to <c>true</c> [b filter].</param>
        /// <returns></returns>
        string GetContent(string sectionId, string contentId, bool bFilter);

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <param name="sectionId">The section identifier.</param>
        /// <param name="contentId">The content identifier.</param>
        /// <returns></returns>
        string GetContent(string sectionId, string contentId);
    }
}
