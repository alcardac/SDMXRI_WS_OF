// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataReaderPositionManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.DataParser.Manager
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;

    #endregion

    /// <summary>
    /// Moves the data reader position backwards or forwards
    /// </summary>
    public interface IDataReaderPositionManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Moves the data reader engine to the position at the given index
        /// </summary>
        /// <param name="dataReaderEngine">
        /// The data reader engine
        /// </param>
        /// <param name="datasetIndex">
        /// The dataset to read the key for
        /// </param>
        /// <param name="keyPosition">
        /// The key index in the dataset
        /// </param>
        /// <returns>
        /// The position
        /// </returns>
        IKeyable MoveToPosition(IDataReaderEngine dataReaderEngine, int datasetIndex, int keyPosition);

        /// <summary>
        /// Moves the data reader engine to the observation at the given index, from the key obtained by the given index
        /// </summary>
        /// <param name="dataReaderEngine">
        /// The data reader engine
        /// </param>
        /// <param name="datasetIndex">
        /// The dataset to read the key for
        /// </param>
        /// <param name="keyPosition">
        /// The key index that the observation belongs to
        /// </param>
        /// <param name="obsNum">
        /// The observation index in the key
        /// </param>
        /// <returns>
        /// The observation
        /// </returns>
        IObservation MoveToObs(IDataReaderEngine dataReaderEngine, int datasetIndex, int keyPosition, int obsNum);

        #endregion
    }
}
