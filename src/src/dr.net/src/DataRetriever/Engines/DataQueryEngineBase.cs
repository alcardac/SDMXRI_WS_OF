// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataQueryEngineBase.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The base data query engine
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.DataRetriever.Engines
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    using Estat.Nsi.DataRetriever.Builders;
    using Estat.Nsi.DataRetriever.Model;
    using Estat.Sri.MappingStoreRetrieval.Engine.Mapping;

    using org.estat.PcAxis.PcAxisProvider;

    /// <summary>
    /// The base data query engine
    /// </summary>
    /// <typeparam name="TDataRetrievalInfo">The type of the data retrieval information.</typeparam>
    /// <typeparam name="TMappedValues">The type of the mapped values.</typeparam>
    internal abstract class DataQueryEngineBase<TDataRetrievalInfo, TMappedValues> where TDataRetrievalInfo : DataRetrievalInfo where TMappedValues : IMappedValues
    {
        #region Constants and Fields

        /// <summary>
        ///   The Dissemination database connection builder instance
        /// </summary>
        private static readonly DDbConnectionBuilder _dbConnectionBuilder = DDbConnectionBuilder.Instance;

        #endregion

        #region Methods

        /// <summary>
        /// Handles the component mapping except for the TimeDimension when TRANSCODING is used
        /// </summary>
        /// <param name="reader">The IDataReader to read data from DDB</param>
        /// <param name="componentValues">The collection to store the component values</param>
        /// <param name="componentMappings">Component mappings list</param>
        /// <param name="info">The data retrieval information.</param>
        /// <returns>
        /// True all components were mapped - false when an unmapped code was found
        /// </returns>
        protected static bool HandleComponentMapping(IDataReader reader, TMappedValues componentValues, IList<IComponentMapping> componentMappings, TDataRetrievalInfo info)
        {
            var effectiveDimensionAtObservation = info.EffectiveDimensionAtObservation;
            var dimensionAtObservationMapping = info.DimensionAtObservationMapping;
            for (int index = 0; index < componentMappings.Count; index++)
            {
                var componentMapping = componentMappings[index];

                var val = componentMapping.MapComponent(reader);
                if (val != null)
                {
                    componentValues.Add(index, val);
                    if (componentMapping.Component.FrequencyDimension)
                    {
                        componentValues.FrequencyValue = val;
                    }

                    if (componentMapping.Equals(dimensionAtObservationMapping) || componentMapping.Component.Id.Equals(effectiveDimensionAtObservation))
                    {
                        componentValues.DimensionAtObservationValue = val;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

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
        protected abstract TMappedValues CreateMappedValues(TDataRetrievalInfo info, IDataReader reader);

        /// <summary>
        /// Execute the <see cref="DataRetrievalInfo.SqlString"/>
        /// </summary>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        protected void ExecuteDbCommand(TDataRetrievalInfo info)
        {
            // for PC-axis it doesn't support DbFactory.CreateCommand() and the enteprise libs seem to call it
            using (DbConnection connection = _dbConnectionBuilder.Build(info))
            {
                SetPCAxisLimit(info, connection);

                // for pc-axis
                connection.Open();

                this.RunAdditionalQueries(info, connection);

                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = info.SqlString;
                    command.CommandTimeout = 0;

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        var componentValues = this.CreateMappedValues(info, reader);
                        if (info.HasMaxObsPerSeries)
                        {
                            this.ReadDataMaxObsPerSeries(info, reader, componentValues, info.AllComponentMappings);
                        }
                        else if (info.Limit <= 0)
                        {
                            this.ReadData(info, reader, componentValues, info.AllComponentMappings);
                        }
                        else
                        {
                            this.ReadData(info, reader, componentValues, info.Limit, info.AllComponentMappings);
                        }

                        // Cancel any pending work of the IDataReader to avoid delays when closing it. 
                        // this requires an updated MySQL driver, 6.3.7 or later. See http://bugs.mysql.com/bug.php?id=60541
                        command.Cancel();
                    }
                }
            }
        }

        /// <summary>
        /// Reads the data with max observation per series.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="componentValues">The component values.</param>
        /// <param name="mappings">The mappings.</param>
        protected virtual void ReadDataMaxObsPerSeries(TDataRetrievalInfo info, IDataReader reader, TMappedValues componentValues, IList<IComponentMapping> mappings)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Read data from DDB, perform mapping and transcoding and store it in the writer specified <see cref="DataRetrievalInfo"/>
        /// </summary>
        /// <param name="info">
        ///   The current Data Retrieval state 
        /// </param>
        /// <param name="reader">
        ///   The <see cref="IDataReader"/> to read data from DDB 
        /// </param>
        /// <param name="componentValues">
        ///   The component Values. 
        /// </param>
        /// <param name="mappings">
        ///   The collection of component mappings 
        /// </param>
        protected virtual void ReadData(TDataRetrievalInfo info, IDataReader reader, TMappedValues componentValues, IList<IComponentMapping> mappings)
        {
            int count = 0;

            while (reader.Read())
            {
                if (HandleMappings(reader, info, componentValues, mappings))
                {
                    count += this.StoreResults(componentValues, info);
                }
            }

            info.RecordsRead = count;
        }

        /// <summary>
        /// Read data from DDB up the specified number of observations, perform mapping and transcoding and store it in the writer specified <see cref="DataRetrievalInfo"/>
        /// </summary>
        /// <param name="info">
        ///   The current Data Retrieval state 
        /// </param>
        /// <param name="reader">
        ///   The <see cref="IDataReader"/> to read data from DDB 
        /// </param>
        /// <param name="componentValues">
        ///   The component Values. 
        /// </param>
        /// <param name="limit">
        ///   The maximum number of observations, should be greater than 0 
        /// </param>
        /// <param name="mappings">
        ///   The collection of component mappings 
        /// </param>
        protected virtual void ReadData(TDataRetrievalInfo info, IDataReader reader, TMappedValues componentValues, int limit, IList<IComponentMapping> mappings)
        {
            int count = 0;

            // note that setting the limit to SQL query doesn't help because a row might be ignored (see below) or a row might have multiple observations
            while (!info.IsTruncated && reader.Read())
            {
                if (HandleMappings(reader, info, componentValues, mappings))
                {
                    // we need to do the check in here to be sure that we reached the limit with accepted records. 
                    // This was not important for SDMX v2.0 but it is for SDMX v2.1. In SDMX v2.1 we generate an error.
                    if (count >= limit)
                    {
                        // set the is truncated flag so we can determine 
                        info.IsTruncated = true;
                    }
                    else
                    {
                        count += this.StoreResults(componentValues, limit, info);
                    }
                }
            }

            info.RecordsRead = count;
        }

        /// <summary>
        /// Execute any additional queries if overridden in a subclass. The base method does nothing
        /// </summary>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        /// <param name="connection">
        /// The <see cref="DbConnection"/> to the DDB 
        /// </param>
        protected virtual void RunAdditionalQueries(TDataRetrievalInfo info, DbConnection connection)
        {
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
        protected abstract int StoreResults(TMappedValues componentValues, TDataRetrievalInfo info);

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
        protected abstract int StoreResults(TMappedValues componentValues, int limit, TDataRetrievalInfo info);

        /// <summary>
        /// Handles the component mappings
        /// </summary>
        /// <param name="reader">
        /// The IDataReader to read data from DDB 
        /// </param>
        /// <param name="info">
        /// The info. 
        /// </param>
        /// <param name="componentValues">
        /// The collection to store the component values 
        /// </param>
        /// <param name="mappings">
        /// The collection of component mappings 
        /// </param>
        /// <returns>
        /// True all components were mapped - false when an unmapped code was found 
        /// </returns>
        protected static bool HandleMappings(
            IDataReader reader,
            TDataRetrievalInfo info,
            TMappedValues componentValues,
            IList<IComponentMapping> mappings)
        {
            return HandleComponentMapping(reader, componentValues, mappings, info)
                   && HandleTimeDimensionMapping(reader, info, componentValues); // MAT-262
        }

        /// <summary>
        /// Handles the time dimension mapping when transcoding is used
        /// </summary>
        /// <param name="reader">
        /// The IDataReader to read data from DDB 
        /// </param>
        /// <param name="info">
        /// The info. 
        /// </param>
        /// <param name="mappedValues">
        /// The dictionary to store the time dimension value 
        /// </param>
        /// <returns>
        /// True all components were mapped - false when an unmapped code was found 
        /// </returns>
        private static bool HandleTimeDimensionMapping(
            IDataReader reader, TDataRetrievalInfo info, TMappedValues mappedValues)
        {
            if (info.TimeMapping != null)
            {
                string val = info.TimeTranscoder.MapComponent(reader, mappedValues.FrequencyValue);
                if (val != null)
                {
                    mappedValues.TimeValue = val;
                    if (info.IsTimePeriodAtObservation)
                    {
                        mappedValues.DimensionAtObservationValue = val;
                    }
                }
                else
                {
                    return false; // null value found at time dimension
                }
            }

            return true;
        }

        /// <summary>
        /// Conditionally set the PCAxis data limit if the <paramref name="connection"/> is a <see cref="PcAxisConnection"/>
        /// </summary>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        /// <param name="connection">
        /// The connection to DDB. 
        /// </param>
        private static void SetPCAxisLimit(TDataRetrievalInfo info, IDbConnection connection)
        {
            var pcAxisConnection = connection as PcAxisConnection;
            if (pcAxisConnection != null)
            {
                // to avoid loading all observations from px file
                pcAxisConnection.DataPreviewRows = info.Limit;
            }
        }

        #endregion
    }
}