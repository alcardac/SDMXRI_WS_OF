// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossSectionalMeasuresDataQueryEngine.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Dissemination Data Query engine for XS DSD with XS Measures and Measure Dimension/Primary Measure mapped.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.DataRetriever.Engines
{
    using System;
    using System.Globalization;

    using Estat.Nsi.DataRetriever.Model;

    /// <summary>
    /// Dissemination Data Query engine for XS DSD with XS Measures and Measure Dimension/Primary Measure mapped.
    /// </summary>
    internal class CrossSectionalMeasuresDataQueryEngine : CrossSectionalDataQueryEngineBase, IDataQueryEngine<DataRetrievalInfoXS>
    {
        #region Constants and Fields

        /// <summary>
        ///   The singleton instance
        /// </summary>
        private static readonly CrossSectionalMeasuresDataQueryEngine _instance =
            new CrossSectionalMeasuresDataQueryEngine();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Prevents a default instance of the <see cref="CrossSectionalMeasuresDataQueryEngine" /> class from being created.
        /// </summary>
        private CrossSectionalMeasuresDataQueryEngine()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the singleton instance
        /// </summary>
        public static CrossSectionalMeasuresDataQueryEngine Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Methods

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
            if (row.MeasureDimensionValue == null)
            {
                throw new InvalidOperationException("MappedXsValues.MeasureDimension is null");
            }

            string tag;
            if (info.XSMeasureCodeToConcept.TryGetValue(row.MeasureDimensionValue.Value, out tag))
            {
                return WriteObservation(row, tag, row.PrimaryMeasureValue.Value, info);
            }

            throw new InvalidOperationException(
                string.Format(CultureInfo.InvariantCulture, "Unknown measure code {0}", row.MeasureDimensionValue.Value));
        }

        #endregion
    }
}