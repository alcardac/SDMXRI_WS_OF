﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossSectionalDataQueryEngineBase.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The base class for DataQueryEngine used for CrossSectional output
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.DataRetriever.Engines
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Estat.Nsi.DataRetriever.Model;
    using Estat.Nsi.DataRetriever.Properties;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    /// <summary>
    /// The base class for DataQueryEngine used for CrossSectional output
    /// </summary>
    internal abstract class CrossSectionalDataQueryEngineBase : DataQueryEngineBase<DataRetrievalInfoXS, MappedXsValues>
    {
        #region Public Methods

        /// <summary>
        /// This method executes an SQL query on the dissemination database and writes it to <see cref="DataRetrievalInfoXS.XSWriter"/> . The SQL query is located inside <paramref name="info"/> at <see cref="DataRetrievalInfo.SqlString"/>
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
        public virtual void ExecuteSqlQuery(DataRetrievalInfoXS info)
        {
            this.ExecuteDbCommand(info);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Writes the attributes.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <param name="info">The info.</param>
        protected static void WriteAttributes(IEnumerable<ComponentValue> attributes, DataRetrievalInfoXS info)
        {
            // write  attributes
            foreach (var keyValuePair in attributes)
            {
                var componentEntity = keyValuePair.Key;
                var value = keyValuePair.Value;

                // SODIHD-1272 write optional attribute only if it is not empty.
                if (componentEntity.ComponentType != SdmxComponentType.Attribute || componentEntity.AttStatus != AssignmentStatus.Conditional || !string.IsNullOrEmpty(value))
                {
                    info.XSWriter.WriteAttributeValue(componentEntity.Id, value);
                }
            }
        }

        /// <summary>
        /// Conditionally write the dataset values.
        /// </summary>
        /// <param name="row">
        /// The map between components and their values 
        /// </param>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        protected static void WriteDataSet(MappedXsValues row, DataRetrievalInfoXS info)
        {
            if (row.StartedDataSet)
            {
                return;
            }

            foreach (var dataSetLevelDimension in row.DataSetLevelDimensionValues)
            {
                info.XSWriter.WriteDataSetKeyValue(dataSetLevelDimension.Key.Id, dataSetLevelDimension.Value);
            }

            WriteAttributes(row.DataSetLevelAttributeValues, info);

            row.StartedDataSet = true;
        }

        /// <summary>
        /// Write the CrossSectional Group with the dimensions and attributes found in <paramref name="row"/>
        /// </summary>
        /// <param name="row">
        /// The map between components and their values 
        /// </param>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        protected static void WriteGroup(MappedXsValues row, DataRetrievalInfoXS info)
        {
            info.XSWriter.StartXSGroup();

            foreach (var groupLevelDimension in row.GroupLevelDimensionValues)
            {
                info.XSWriter.WriteXSGroupKeyValue(groupLevelDimension.Key.Id, groupLevelDimension.Value);
            }

            WriteAttributes(row.GroupLevelAttributeValues, info);
        }

        /// <summary>
        /// Write observation element. The time, observation and attribute values are in <paramref name="row"/>
        /// </summary>
        /// <param name="row">
        /// The map between components and their values 
        /// </param>
        /// <param name="tag">
        /// The element tag. The Primary or XS measure concept ref. 
        /// </param>
        /// <param name="value">
        /// The observation value 
        /// </param>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        /// <returns>
        /// The number of observations stored. Is is always 1 
        /// </returns>
        protected static int WriteObservation(MappedXsValues row, string tag, string value, DataRetrievalInfoXS info)
        {
            // write time and obs value
            info.XSWriter.StartXSObservation(tag, value);

            // write observation attributes
            foreach (var keyValuePair in row.ObservationLevelDimensionValues)
            {
                info.XSWriter.WriteXSObservationKeyValue(keyValuePair.Key.Id, keyValuePair.Value);
            }

            // write observation attributes
            WriteAttributes(row.ObservationLevelAttributeValues, info);
            return 1;
        }

        /// <summary>
        /// Write the CrossSectional Group with the dimensions and attributes found in <paramref name="row"/>
        /// </summary>
        /// <param name="row">
        /// The map between components and their values 
        /// </param>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        protected static void WriteSection(MappedXsValues row, DataRetrievalInfoXS info)
        {
            info.XSWriter.StartSection();

            foreach (var dimension in row.SectionLevelDimensionValues)
            {
                info.XSWriter.WriteSectionKeyValue(dimension.Key.Id, dimension.Value);
            }

            // write observation attributes
            WriteAttributes(row.SectionAttributeValues, info);
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
        protected override MappedXsValues CreateMappedValues(DataRetrievalInfoXS info, IDataReader reader)
        {
            return new MappedXsValues(info);
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
        protected override int StoreResults(MappedXsValues componentValues, DataRetrievalInfoXS info)
        {
            if (componentValues == null)
            {
                throw new ArgumentException(Resources.ErrorTypeNotMappedXsValues, "componentValues");
            }

            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            WriteDataSet(componentValues, info);

            if (componentValues.IsNewGroupKey())
            {
                WriteGroup(componentValues, info);
            }

            if (componentValues.IsNewSectionKey())
            {
                WriteSection(componentValues, info);
            }

            return this.WriteObservation(componentValues, info);
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
        protected override int StoreResults(MappedXsValues componentValues, int limit, DataRetrievalInfoXS info)
        {
            return this.StoreResults(componentValues, info);
        }

        /// <summary>
        /// Write a primary measure observation
        /// </summary>
        /// <param name="row">
        /// The map between components and their values 
        /// </param>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        /// <returns>
        /// The number of observations stored. 
        /// </returns>
        protected abstract int WriteObservation(MappedXsValues row, DataRetrievalInfoXS info);

        #endregion
    }
}