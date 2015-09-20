// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureRetrievalInfoBuilder.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The <see cref="StructureRetrievalInfo" /> Builder interface
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Estat.Nsi.StructureRetriever.Model;
    using Estat.Sri.CustomRequests.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The <see cref="StructureRetrievalInfo"/> Builder interface
    /// </summary>
    internal interface IStructureRetrievalInfoBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build a <see cref="StructureRetrievalInfo"/> from the specified parameters
        /// </summary>
        /// <param name="dataflow">
        /// The datflow to get the available data for 
        /// </param>
        /// <param name="connectionStringSettings">
        /// The Mapping Store connection string settings 
        /// </param>
        /// <param name="logger">
        /// The logger to log information, warnings and errors. It can be null 
        /// </param>
        /// <param name="allowedDataflows">
        /// The collection of allowed dataflows 
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// connectionStringSettings is null
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// dataflow is null
        /// </exception>
        /// <exception cref="StructureRetrieverException">
        /// Parsing error or mapping store exception error
        /// </exception>
        /// <returns>
        /// a <see cref="StructureRetrievalInfo"/> from the specified parameters 
        /// </returns>
        StructureRetrievalInfo Build(
            IConstrainableStructureReference dataflow,
            ConnectionStringSettings connectionStringSettings,
            IList<IMaintainableRefObject> allowedDataflows);

        #endregion
    }
}