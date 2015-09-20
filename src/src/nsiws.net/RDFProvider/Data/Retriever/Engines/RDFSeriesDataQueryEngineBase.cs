// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeriesDataQueryEngineBase.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The base class for DataQueryEngine used for Time series output
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RDFProvider.Retriever.Engines
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Globalization;
   
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using Estat.Nsi.DataRetriever.Properties;

    using log4net;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using RDFProvider.Constants;
    using RDFProvider.Retriever.Model; 

    internal abstract class RDFSeriesDataQueryEngineBase : RDFDataQueryEngineBase
    {
        #region Methods

        private static readonly ILog Logger = LogManager.GetLogger(typeof(RDFSeriesDataQueryEngineBase));

        protected override IMappedValues CreateMappedValues(DataRetrievalInfo info, IDataReader reader)
        {
            var seriesInfo = info as DataRetrievalInfoSeries;
            if (seriesInfo == null)
            {
                throw new ArgumentException(Resources.ErrorTypeNotDataRetrievalInfoSeries, "info");
            }

            return new MappedValues(info) { StartedDataSet = seriesInfo.DataSetAttributes.Count > 0 };
        }

        private static bool ProcessedKeySet(
            GroupInformation targetGroup, DataRetrievalInfoSeries info, MappedValues componentValues)
        {
            var current = new ReadOnlyKey(componentValues, info.GroupNameTable);

            if (!targetGroup.KeySet.ContainsKey(current))
            {
                targetGroup.KeySet.Add(current, null);
                return false;
            }

            return true;
        }

        protected static int RDFWriteObservation(DataRetrievalInfoSeries info, MappedValues row, string value)
        {
            // write time and obs value            
            info.SeriesWriter.RDFWriteObservation(row.TimeValue, value);
            return 1;
        }

        protected static void RDFWriteSeries(DataRetrievalInfoSeries info, MappedValues row)
        {
            string values;
            values = RDFConstants.RdfDataset + info.MappingSet.DataSet.Description.ToString();
            foreach (var dimensionValue in row.DimensionValues)
            {
                if (!dimensionValue.Key.Id.Equals(info.DimensionAtObservation))
                {
                    values += "/" + dimensionValue.Value;
                }
            }

            values += "/" + row.TimeValue;

            // start series
            info.SeriesWriter.StartSeries(values);

            values = "";

            // write dimensions
            foreach (var dimensionValue in row.DimensionValues)
            {
                if (!dimensionValue.Key.Id.Equals(info.DimensionAtObservation))
                {                    
                    info.SeriesWriter.WriteSeriesKeyValue(dimensionValue.Key.Id, dimensionValue.Value, dimensionValue.Key.CodeList.Version, dimensionValue.Key.CodeList.Id);
                }
            }
        }

        protected static void RDFWriterStrucInfo(DataRetrievalInfoSeries info, string dataset, string struc)
        {
            info.SeriesWriter.RDFWriterStrucInfo(dataset, struc);
        }

        #endregion
    }
}