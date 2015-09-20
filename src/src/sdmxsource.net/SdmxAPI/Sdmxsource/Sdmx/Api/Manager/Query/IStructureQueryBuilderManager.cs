// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureQueryBuilderManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Query
{
    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IStructureQueryBuilderManager
    {
        /// <summary>
        /// Builds a structure query in the requested format
        /// </summary>
        /// <param name="structureQuery">The query to build a representation of</param>
        /// <param name="structureQueryFormat">The required format</param>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <returns>Representation of query in the desired format.</returns>
        T BuildStructureQuery<T>(IRestStructureQuery structureQuery, IStructureQueryFormat<T> structureQueryFormat);
    }
}
