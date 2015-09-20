// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataRetrievalInfoComplex.cs" company="Eurostat">
//   Date Created : 2011-12-19
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The current data retrieval state for Time Series output
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Configuration;
using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;
using Org.Sdmxsource.Sdmx.Api.Engine;
using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;

namespace IstatExtension.Retriever.Model
{
    public class DataRetrievalInfoComplex : DataRetrievalInfoSeries
    {
        #region Constants and Fields

        /// <summary>
        ///   Writer provided that is based on the XS model to write the retrieved data.
        /// </summary>
        private readonly IDataWriterEngine _complexWriter;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRetrievalInfoTabular"/> class.
        /// </summary>
        /// <param name="mappingSet">
        /// The mapping set of the dataflow found in the sdmx query 
        /// </param>
        /// <param name="query">
        /// The current SDMX Query object 
        /// </param>
        /// <param name="connectionStringSettings">
        /// The Mapping Store connection string settings 
        /// </param>
        /// <param name="complexWriter">
        /// The tabular Writer. 
        /// </param>
        public DataRetrievalInfoComplex(
            MappingSetEntity mappingSet,
            IComplexDataQuery query,
            ConnectionStringSettings connectionStringSettings,
            IDataWriterEngine complexWriter)
            : base(mappingSet, query, connectionStringSettings, complexWriter)
        {
            this.Limit = query.DefaultLimit.HasValue ? query.DefaultLimit.Value : 0;
        }

        #endregion
    }
}
