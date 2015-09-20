// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataReaderWriterTransform.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.DataParser.Transform
{
    #region Using directives

    using System;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;

    #endregion

    /// <summary>
    /// The DataReaderWriterTransform interface.
    /// </summary>
    public interface IDataReaderWriterTransform
    {
        #region Public Methods and Operators

        /// <summary>
        /// Copies to writer.
        /// </summary>
        /// <param name="dataReaderEngine">The data reader engine.</param>
        /// <param name="dataWriterEngine">The data writer engine.</param>
        /// <param name="includeObs">if set to <c>true</c> include the OBS.</param>
        /// <param name="maxObs">The maximum OBS.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="copyHeader">if set to <c>true</c> copy the header.</param>
        /// <param name="closeWriter">if set to <c>true</c> close the writer.</param>
        void CopyToWriter(IDataReaderEngine dataReaderEngine, IDataWriterEngine dataWriterEngine, bool includeObs, int maxObs, DateTime dateFrom, DateTime dateTo, bool copyHeader, bool closeWriter);

        /// <summary>
        /// Copies the dataset to writer.
        /// </summary>
        /// <param name="dataReaderEngine">The data reader engine</param>
        /// <param name="dataWriterEngine">The data writer engine</param>
        /// <param name="pivotDimension">The pivot dimension</param>
        /// <param name="includeObs">The include observation</param>
        /// <param name="maxObs">The max observation</param>
        /// <param name="dateFrom">The date from</param>
        /// <param name="dateTo">The date to</param>
        /// <param name="includeHeader">The include header</param>
        /// <param name="closeOnCompletion">The close on completion</param>
        void CopyDatasetToWriter(
            IDataReaderEngine dataReaderEngine,
            IDataWriterEngine dataWriterEngine,
            string pivotDimension,
            bool includeObs,
            int maxObs,
            DateTime dateFrom,
            DateTime dateTo,
            bool includeHeader,
            bool closeOnCompletion);

        /// <summary>
        /// Copies the data held in the data reader engine to the data writer engine verbatim.
        /// <p/>
        /// This will make an initial call on the reader engine to reset the position back to the start to ensure a full copy
        /// <p/>
        /// The writer engine will be closed on completion, the reader engine will NOT be closed
        /// </summary>
        /// <param name="dataReaderEngine">
        /// The reader engine to read the data from
        /// </param>
        /// <param name="dataWriterEngine">
        /// The writer engine to write the data to
        /// </param>
        /// <param name="copyHeader">
        /// The copy header
        /// </param>
        /// <param name="closeWriter">
        /// The close writer
        /// </param>
        void CopyToWriter(IDataReaderEngine dataReaderEngine, IDataWriterEngine dataWriterEngine, bool copyHeader, bool closeWriter);

        /// <summary>
        /// Copies the data held in the data reader engine to the data writer engine - pivoting the data on the dimension supplied.
        /// <p/>
        /// This will make an initial call on the reader engine to reset the position back to the start to ensure a full copy
        /// <p/>
        /// The writer engine will be closed on completion, the reader engine will NOT be closed
        /// </summary>
        /// <param name="dataReaderEngine">
        /// The reader engine to read the data from
        /// </param>
        /// <param name="dataWriterEngine">
        /// The writer engine to write the data to
        /// </param>
        /// <param name="pivotDimension">
        /// The pivot dimension
        /// </param>
        /// <param name="closeWriter">
        /// The close writer
        /// </param>
        void CopyToWriter(IDataReaderEngine dataReaderEngine, IDataWriterEngine dataWriterEngine, string pivotDimension, bool closeWriter);

        /// <summary>
        /// Writes the key able to the writer engine, if the key is a series then it will also write any observations under the series
        /// </summary>
        /// <param name="dataReaderEngine">The data reader engine</param>
        /// <param name="dataWriterEngine">The data writer engine</param>
        /// <param name="keyable">The key able.</param>
        /// <param name="maxObs">If 0 or null then do not output observations, if less then 0 then there is no limit, if greater then 0 then limit the max OBS to this number</param>
        void WriteKeyableToWriter(IDataReaderEngine dataReaderEngine, IDataWriterEngine dataWriterEngine, IKeyable keyable, int maxObs);

        /// <summary>
        /// Writes the keyable to the writer engine, if the key is a series then it will also write any observations under the series.
        /// <p/>
        /// Only writes the observations that fall between the two date parameters
        /// </summary>
        /// <param name="dataReaderEngine">
        /// The data reader engine
        /// </param>
        /// <param name="dataWriterEngine">
        /// The data writer engine
        /// </param>
        /// <param name="keyable">
        /// The keyable
        /// </param>
        /// <param name="maxObs">
        /// If 0 then do not output observations, if null or less then 0 then there is no limit, if greater then 0 then limit the max OBS to this number
        /// </param>
        /// <param name="dateFrom">
        /// The date from
        /// </param>
        /// <param name="dateTo">
        /// The date to
        /// </param>
        void WriteKeyableToWriter(IDataReaderEngine dataReaderEngine, IDataWriterEngine dataWriterEngine, IKeyable keyable, int maxObs, DateTime dateFrom, DateTime dateTo);

        /// <summary>
        /// Writes the observation to the writer engine
        /// </summary>
        /// <param name="dataWriterEngine">
        /// The data writer engine
        /// </param>
        /// <param name="keyable">
        /// The key able
        /// </param>
        /// <param name="obs">
        /// The observation
        /// </param>
        void WriteObsToWriter(IDataWriterEngine dataWriterEngine, IKeyable keyable, IObservation obs);

        #endregion
    }
}
