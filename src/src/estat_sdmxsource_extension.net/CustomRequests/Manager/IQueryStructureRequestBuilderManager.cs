// -----------------------------------------------------------------------
// <copyright file="IQueryStructureRequestBuilderManager.cs" company="Eurostat">
//   Date Created : 2013-03-28
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.CustomRequests.Manager
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The Query Structure Request Builder Manager interface. 
    /// </summary>
    public interface IQueryStructureRequestBuilderManager
    {
        /// <summary>
        /// Builds a query structure request in the requested format
        /// </summary>
        /// <param name="structureReferences">The list of <see cref="IStructureReference"/> to build a representation of <c>QueryStructureRequest</c></param>
        /// <param name="structureQueryFormat">The required format.</param>
        /// <param name="resolveReferences">
        /// Set to <c>True</c> to resolve references.
        /// </param>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <returns>Representation of query in the desired format.</returns>
        T BuildStructureQuery<T>(IEnumerable<IStructureReference> structureReferences, IStructureQueryFormat<T> structureQueryFormat, bool resolveReferences); 
    }
}