// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureQueryFactory.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Factory
{
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;

    /// <summary>
    /// A structure query factory is a factory which is responsible fore creating a builder that can build
    /// a StructureQuery message in the format defined by the StructureQueryFormat 
    /// </summary>
    public interface IStructureQueryFactory
    {
     /// <summary>
        /// Returns a StructureQueryBuilder only if this factory understands the StructureQueryFormat.  If the format is unknown, null will be returned
     /// </summary>
     /// <typeparam name="T">generic type parameter</typeparam>
     /// <param name="format">Format</param>
        /// <returns>StructureQueryBuilder is this factory knows how to build this query format, or null if it doesn't</returns>
        IStructureQueryBuilder<T> GetStructureQueryBuilder<T>(IStructureQueryFormat<T> format);
    }
}
