// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataRetrievalInfoXS.cs" company="Eurostat">
//   Date Created : 2011-12-19
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The current data retrieval state for Cross Sectional output
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSVZip.Retriever.Model
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;

    /// <summary>
    /// The current data retrieval state for Cross Sectional output
    /// </summary>
    internal class DataRetrievalInfoXS : DataRetrievalInfo
    {
        #region Constants and Fields

        /// <summary>
        ///   Gets the XSMeasure attached code to Concept ref map
        /// </summary>
        private readonly Dictionary<string, string> _xsMeasureCodeToConcept =
            new Dictionary<string, string>(StringComparer.Ordinal);

        /// <summary>
        ///   Writer provided that is based on the XS model to write the retrieved data.
        /// </summary>
        private readonly ICrossSectionalWriterEngine _xsWriter;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRetrievalInfoXS"/> class.
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
        /// <param name="xsWriter">
        /// The ICrossSectionalWriter Writer. 
        /// </param>
        public DataRetrievalInfoXS(
            MappingSetEntity mappingSet,
            IDataQuery query,
            ConnectionStringSettings connectionStringSettings,
            ICrossSectionalWriterEngine xsWriter)
            : base(mappingSet, query, connectionStringSettings)
        {
            this._xsWriter = xsWriter;
            if (this.MeasureComponent == null)
            {
                foreach (var crossSectionalMeasure in this.MappingSet.Dataflow.Dsd.CrossSectionalMeasures)
                {
                    this._xsMeasureCodeToConcept.Add(
                        crossSectionalMeasure.CrossSectionalMeasureCode, crossSectionalMeasure.Id);
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the XSMeasure attached code to Concept ref map
        /// </summary>
        public IDictionary<string, string> XSMeasureCodeToConcept
        {
            get
            {
                return this._xsMeasureCodeToConcept;
            }
        }

        /// <summary>
        ///   Gets the writer provided that is based on the XS model to write the retrieved data. If null the seriesWriter should be set instead.
        /// </summary>
        public ICrossSectionalWriterEngine XSWriter
        {
            get
            {
                return this._xsWriter;
            }
        }

        #endregion
    }
}