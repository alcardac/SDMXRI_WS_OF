// -----------------------------------------------------------------------
// <copyright file="IQueryStructureRequestBuilder.cs" company="Eurostat">
//   Date Created : 2013-03-27
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.CustomRequests.Builder
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// Responsible for building a Query Structure Request that can be used to query services external to the 
    /// <c>SdmxSource</c> framework
    /// </summary>
    /// <typeparam name="T">The output query type</typeparam>
    public interface IQueryStructureRequestBuilder<out T>
    {
        /// <summary>
        /// Builds a <c>QueryStructureRequest</c> that matches the passed in format
        /// </summary>
        /// <param name="queries">
        ///     The queries.
        /// </param>
        /// <param name="resolveReferences">
        /// Set to <c>True</c> to resolve references.
        /// </param>
        /// <returns>
        /// The <typeparamref name="T"/> from <paramref name="queries"/>.
        /// </returns>
        T BuildStructureQuery(IEnumerable<IStructureReference> queries, bool resolveReferences);
    }
}