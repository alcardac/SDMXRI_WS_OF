// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossSectionalPrimaryDataQueryEngine.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Dissemination Data Query engine for XS DSD without XS Measures.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace IstatExtension.Retriever.Engines
{
    using IstatExtension.Retriever.Model;

    /// <summary>
    /// Dissemination Data Query engine for XS DSD without XS Measures.
    /// </summary>
    public class CrossSectionalPrimaryDataQueryEngine : CrossSectionalDataQueryEngineBase, IDataQueryEngine
    {
        #region Constants and Fields

        /// <summary>
        ///   The singleton instance
        /// </summary>
        private static readonly CrossSectionalPrimaryDataQueryEngine _instance =
            new CrossSectionalPrimaryDataQueryEngine();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Prevents a default instance of the <see cref="CrossSectionalPrimaryDataQueryEngine" /> class from being created.
        /// </summary>
        private CrossSectionalPrimaryDataQueryEngine()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the singleton instance
        /// </summary>
        public static CrossSectionalPrimaryDataQueryEngine Instance
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
            return WriteObservation(row, row.PrimaryMeasureValue.Key.Id, row.PrimaryMeasureValue.Value, info);
        }

        #endregion
    }
}