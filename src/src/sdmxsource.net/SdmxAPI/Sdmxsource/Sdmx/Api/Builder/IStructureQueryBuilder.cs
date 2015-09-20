// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureQueryBuilder.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Builder
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Query;

    #endregion

    /// <summary>
    /// Responsible for building a Structure Query that can be used to query services external to the 
    /// SdmxSource framework
    /// </summary>
    public interface IStructureQueryBuilder<out T>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds a StructureQuery that matches the passed in format
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        T BuildStructureQuery(IRestStructureQuery query);

        #endregion
    }
}
