// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRestStructureQueryManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Rest
{
    #region Using directives

    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;

    #endregion

    /// <summary>
    /// TODO
    /// </summary>
    public interface IRestStructureQueryManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="query">
        /// The query
        /// </param>
        /// <param name="outPutStream">
        /// The output stream
        /// </param>
        /// <param name="outputFormat">
        /// The output format
        /// </param>
        void GetStructures(IRestStructureQuery query, Stream outPutStream, IStructureFormat outputFormat);

        #endregion
    }
}
