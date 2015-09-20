// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossSectionalMeasuresMappedDataQueryEngine.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The cross sectional measures mapped data query engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSVZip.Retriever.Engines
{
    using System;

    using CSVZip.Retriever.Model;

    /// <summary>
    /// The cross sectional measures mapped data query engine.
    /// </summary>
    internal class CrossSectionalMeasuresMappedDataQueryEngine : CrossSectionalDataQueryEngineBase, IDataQueryEngine
    {
        #region Constants and Fields

        /// <summary>
        ///   The singleton instance
        /// </summary>
        private static readonly CrossSectionalMeasuresMappedDataQueryEngine _instance =
            new CrossSectionalMeasuresMappedDataQueryEngine();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Prevents a default instance of the <see cref="CrossSectionalMeasuresMappedDataQueryEngine" /> class from being created.
        /// </summary>
        private CrossSectionalMeasuresMappedDataQueryEngine()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the singleton instance
        /// </summary>
        public static CrossSectionalMeasuresMappedDataQueryEngine Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

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
        public override void ExecuteSqlQuery(DataRetrievalInfo info)
        {
            info.BuildXSMeasures();
            base.ExecuteSqlQuery(info);
        }

        #endregion

        #region Methods

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
        protected override int StoreResults(IMappedValues componentValues, int limit, DataRetrievalInfo info)
        {
            var row = componentValues as MappedXsValues;
            if (row == null)
            {
                throw new ArgumentException("mappedValues not of MappedXsValues type");
            }

            var xsInfo = info as DataRetrievalInfoXS;
            if (xsInfo == null)
            {
                throw new ArgumentException("info not of DataRetrievalInfoXS type");
            }

            WriteDataSet(row, xsInfo);

            if (row.IsNewGroupKey())
            {
                WriteGroup(row, xsInfo);
            }

            if (row.IsNewSectionKey())
            {
                WriteSection(row, xsInfo);
            }

            return WriteObservation(row, xsInfo, limit);
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
        protected override int WriteObservation(MappedXsValues row, DataRetrievalInfoXS info)
        {
            int count = 0;
            for (var i = 0; i < info.CrossSectionalMeasureMappings.Count; i++)
            {
                var crossSectionalMeasureMapping = info.CrossSectionalMeasureMappings[i];
                var xsComponent = crossSectionalMeasureMapping.Components[0];
                count += WriteObservation(row, xsComponent.Id, row.GetXSMeasureValue(xsComponent), info);
            }

            return count;
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
        /// <param name="limit">
        /// The limit. 
        /// </param>
        /// <returns>
        /// The number of observations stored. 
        /// </returns>
        private static int WriteObservation(MappedXsValues row, DataRetrievalInfoXS info, int limit)
        {
            int maxMeasures = Math.Min(info.CrossSectionalMeasureMappings.Count, limit);

            int count = 0;
            for (var i = 0; i < maxMeasures; i++)
            {
                var crossSectionalMeasureMapping = info.CrossSectionalMeasureMappings[i];
                var xsComponent = crossSectionalMeasureMapping.Components[0];
                count += WriteObservation(row, xsComponent.Id, row.GetXSMeasureValue(xsComponent), info);
            }

            return count;
        }

        #endregion
    }
}