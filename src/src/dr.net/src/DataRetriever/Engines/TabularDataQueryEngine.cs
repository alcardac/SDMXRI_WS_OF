// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TabularDataQueryEngine.cs" company="Eurostat">
//   Date Created : 2011-12-16
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Dissemination Data Query engine for tabular output.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.DataRetriever.Engines
{
    using System;
    using System.Data;

    using Estat.Nsi.DataRetriever.Model;
    using Estat.Nsi.DataRetriever.Properties;

    /// <summary>
    /// Dissemination Data Query engine for tabular output.
    /// </summary>
    internal class TabularDataQueryEngine : TabularDataQueryEngineBase, IDataQueryEngine<DataRetrievalInfoTabular>
    {
        #region Constants and Fields

        /// <summary>
        ///   The singleton instance
        /// </summary>
        private static readonly TabularDataQueryEngine _instance = new TabularDataQueryEngine();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Prevents a default instance of the <see cref="TabularDataQueryEngine" /> class from being created.
        /// </summary>
        private TabularDataQueryEngine()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the singleton instance
        /// </summary>
        public static TabularDataQueryEngine Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method executes an SQL query on the dissemination database and writes it to the writer in <see cref="DataRetrievalInfo"/> . The SQL query is located inside <paramref name="info"/> at <see cref="DataRetrievalInfo.SqlString"/>
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="info"/>
        ///   is null
        /// </exception>
        /// <exception cref="DataRetrieverException">
        /// <see cref="ErrorTypes"/>
        /// </exception>
        /// <param name="info">
        /// The current DataRetrieval state 
        /// </param>
        public void ExecuteSqlQuery(DataRetrievalInfoTabular info)
        {
            this.ExecuteDbCommand(info);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create and return a <see cref="IMappedValues"/> implementation
        /// </summary>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        /// <param name="reader">
        /// The <see cref="IDataReader"/> to read data from DDB 
        /// </param>
        /// <returns>
        /// a <see cref="IMappedValues"/> implementation 
        /// </returns>
        protected override MappedValuesFlat CreateMappedValues(DataRetrievalInfoTabular info, IDataReader reader)
        {
            return new MappedValuesFlat(info);
        }

        /// <summary>
        /// Store the SDMX compliant data for each component entity in the store
        /// </summary>
        /// <param name="componentValues">
        /// The map between components and their values 
        /// </param>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        /// <returns>
        /// The number of observations stored 
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="info"/>
        ///   not
        ///   <see cref="DataRetrievalInfoTabular"/>
        ///   type
        ///   -or-
        ///   <paramref name="componentValues"/>
        ///   not
        ///   <see cref="MappedValuesFlat"/>
        ///   type
        /// </exception>
        protected override int StoreResults(MappedValuesFlat componentValues, DataRetrievalInfoTabular info)
        {
            var mappedValues = componentValues;
            if (mappedValues == null)
            {
                throw new ArgumentException(Resources.ErrorComponentValuesNotMappedValuesFlaType);
            }

            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            this.WriteData(mappedValues, info);

            return 1;
        }

        /// <summary>
        /// Store the SDMX compliant data for each component entity in the store
        /// </summary>
        /// <param name="componentValues">
        /// The map between components and their values 
        /// </param>
        /// <param name="limit">
        /// The limit. 
        /// </param>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        /// <returns>
        /// The number of observations stored 
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="info"/>
        ///   not
        ///   <see cref="DataRetrievalInfoTabular"/>
        ///   type
        ///   -or-
        ///   <paramref name="componentValues"/>
        ///   not
        ///   <see cref="MappedValuesFlat"/>
        ///   type
        /// </exception>
        protected override int StoreResults(MappedValuesFlat componentValues, int limit, DataRetrievalInfoTabular info)
        {
            return this.StoreResults(componentValues, info);
        }

        #endregion
    }
}