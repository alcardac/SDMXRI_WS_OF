// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TabularDataQueryEngineBase.cs" company="Eurostat">
//   Date Created : 2011-12-19
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The base class for Tabular output <see cref="IDataQueryEngine" /> implementations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace IstatExtension.Retriever.Engines
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using IstatExtension.Retriever.Model;
    using Estat.Nsi.DataRetriever.Properties;
    using Estat.Sri.MappingStoreRetrieval.Engine.Mapping;

    /// <summary>
    /// The base class for Tabular output <see cref="IDataQueryEngine"/> implementations.
    /// </summary>
    public abstract class TabularDataQueryEngineBase : DataQueryEngineBase
    {
        #region Methods

        /// <summary>
        /// Read data from DDB, perfom mapping and transcoding and store it in the writer specified <see cref="DataRetrievalInfo"/>
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
        protected override void ReadData(DataRetrievalInfo info, IDataReader reader, IMappedValues componentValues, IList<IComponentMapping> mappings)
        {
            this.WriteColumns(info, componentValues, mappings);
            base.ReadData(info, reader, componentValues, mappings);
        }

        /// <summary>
        /// Read data from DDB up the specified number of observations, perfom mapping and transcoding and store it in the writer specified <see cref="DataRetrievalInfo"/>
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
        protected override void ReadData(DataRetrievalInfo info, IDataReader reader, IMappedValues componentValues, int limit, IList<IComponentMapping> mappings)
        {
            this.WriteColumns(info, componentValues, mappings);
            base.ReadData(info, reader, componentValues, limit, mappings);
        }

        /// <summary>
        /// Write the columns
        /// </summary>
        /// <param name="mappedValues">
        /// The mapped components and their values 
        /// </param>
        /// <param name="tabularInfo">
        /// The current data retrieval state 
        /// </param>
        protected virtual void WriteColumns(MappedValuesFlat mappedValues, DataRetrievalInfoTabular tabularInfo)
        {
            tabularInfo.TabularWriter.StartColumns();
            foreach (var dimensionValue in mappedValues.DimensionValues)
            {
                tabularInfo.TabularWriter.WriteColumnKey(dimensionValue.Key.Id);
            }

            if (tabularInfo.MappingSet.Dataflow.Dsd.TimeDimension != null)
            {
                tabularInfo.TabularWriter.WriteColumnKey(tabularInfo.MappingSet.Dataflow.Dsd.TimeDimension.Id);
            }

            foreach (var measureValue in mappedValues.MeasureValues)
            {
                tabularInfo.TabularWriter.WriteColumnMeasure(measureValue.Key.Id);
            }

            foreach (var attributeValue in mappedValues.AttributeValues)
            {
                tabularInfo.TabularWriter.WriteColumnAttribute(attributeValue.Key.Id);
            }
        }

        /// <summary>
        /// Write data to <see cref="DataRetrievalInfoTabular.TabularWriter"/> from the specified <paramref name="tabularInfo"/>
        /// </summary>
        /// <param name="mappedValues">
        /// The map between components and their values 
        /// </param>
        /// <param name="tabularInfo">
        /// The current Data Retrieval state 
        /// </param>
        protected virtual void WriteData(MappedValuesFlat mappedValues, DataRetrievalInfoTabular tabularInfo)
        {
            tabularInfo.TabularWriter.StartRecord();
            foreach (var dimensionValue in mappedValues.DimensionValues)
            {
                tabularInfo.TabularWriter.WriteCellKeyValue(dimensionValue.Value);
            }

            if (tabularInfo.MappingSet.Dataflow.Dsd.TimeDimension != null)
            {
                tabularInfo.TabularWriter.WriteCellKeyValue(mappedValues.TimeValue);
            }

            foreach (var measureValue in mappedValues.MeasureValues)
            {
                tabularInfo.TabularWriter.WriteCellMeasureValue(measureValue.Value);
            }

            foreach (var attributeValue in mappedValues.AttributeValues)
            {
                tabularInfo.TabularWriter.WriteCellAttributeValue(attributeValue.Value);
            }
        }

        /// <summary>
        /// Write the columns
        /// </summary>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        /// <param name="componentValues">
        /// The component Values. 
        /// </param>
        /// <param name="mappings">
        /// The collection of component mappings 
        /// </param>
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
        private void WriteColumns(
            DataRetrievalInfo info, IMappedValues componentValues, ICollection<IComponentMapping> mappings)
        {
            var mappedValues = componentValues as MappedValuesFlat;
            if (mappedValues == null)
            {
                throw new ArgumentException(Resources.ErrorComponentValuesNotMappedValuesFlaType, "componentValues");
            }

            var tabularInfo = info as DataRetrievalInfoTabular;
            if (tabularInfo == null)
            {
                throw new ArgumentException(Resources.ErrorInfoNotDataRetrievalInfoTabularType, "info");
            }

            for (int i = 0; i < mappings.Count; i++)
            {
                mappedValues.Add(i, null);
            }

            this.WriteColumns(mappedValues, tabularInfo);
        }

        #endregion
    }
}