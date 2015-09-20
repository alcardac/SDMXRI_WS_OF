// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataRetrievalInfoTabular.cs" company="Eurostat">
//   Date Created : 2011-12-19
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The current data retrieval state for tabular output
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace IstatExtension.Retriever.Model
{
    using System.Configuration;

    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Estat.Sri.TabularWriters.Engine;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;

    /// <summary>
    /// The current data retrieval state for tabular output
    /// </summary>
    public class DataRetrievalInfoTabular : DataRetrievalInfo
    {
        #region Constants and Fields

        /// <summary>
        ///   Writer provided for tabular to write the retrieved data.
        /// </summary>
        private readonly ITabularWriter _tabularWriter;

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
        /// <param name="tabularWriter">
        /// The tabular Writer. 
        /// </param>
        public DataRetrievalInfoTabular(
            MappingSetEntity mappingSet,
            IDataQuery query,
            ConnectionStringSettings connectionStringSettings,
            ITabularWriter tabularWriter)
            : base(mappingSet, query, connectionStringSettings)
        {
            this._tabularWriter = tabularWriter;
        }

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
        /// <param name="tabularWriter">
        /// The tabular Writer. 
        /// </param>
        public DataRetrievalInfoTabular(
            MappingSetEntity mappingSet,
            IComplexDataQuery query,
            ConnectionStringSettings connectionStringSettings,
            ITabularWriter tabularWriter)
            : base(mappingSet, query, connectionStringSettings)
        {
            this._tabularWriter = tabularWriter;
        }
        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the writer provided for tabular to write the retrieved data. If null the <see
        ///    cref="DataRetrievalInfoSeries.SeriesWriter" /> or <see cref="DataRetrievalInfoXS.XSWriter" /> should be set instead.
        /// </summary>
        public ITabularWriter TabularWriter
        {
            get
            {
                return this._tabularWriter;
            }
        }

        #endregion
    }
}