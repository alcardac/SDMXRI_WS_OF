// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataInformationManager.cs" company="Eurostat">
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

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.DataParser.Model;

    #endregion

    /// <summary>
    /// The purpose of the data information manager is to get metadata off the dataset.
    /// </summary>
    public interface IDataInformationManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns the data type for the sourceData
        /// </summary>
        /// <param name="sourceData">
        /// The readable data location
        /// </param>
        /// <returns>
        /// The data type for the sourceData
        /// </returns>
        IDataFormat GetDataType(IReadableDataLocation sourceData);

        /// <summary>
        /// Returns the target namespace of the dataset
        /// </summary>
        /// <param name="sourceData">
        /// The readable data location
        /// </param>
        /// <returns>
        /// The target namespace of the dataset
        /// </returns>
        string GetTargetNamepace(IReadableDataLocation sourceData);

        /// <summary>
        /// Returns DataInformation about the data, this processes the entire dataset to give an overview of what is in the dataset.
        /// </summary>
        /// <param name="dre">
        /// The data reader engine
        /// </param>
        /// <returns>
        /// The DataInformation
        /// </returns>
        DataInformation GetDataInformation(IDataReaderEngine dre);

        /// <summary>
        /// Returns an ordered list of all the unique dates for each time format in the dataset.
        /// <p/>
        /// This list is ordered with the earliest date first.
        /// <p/>
        /// This method will call <code>reset()</code> on the dataReaderEngine before and after the 
        /// information has been gathered
        /// </summary>
        /// <param name="dataReaderEngine">
        /// The data reader engine
        /// </param>
        /// <returns>
        /// The dictionary of time format to the list of dates that are contained for the time format
        /// </returns>
        IDictionary<TimeFormat, IList<string>> GetAllReportedDates(IDataReaderEngine dataReaderEngine);

        /// <summary>
        /// Returns a list of dimension - value pairs where there is only a single value in the data for the dimension.  For example if the entire
        /// dataset had FREQ=A then one of the returned KeyValue pairs would be FREQ,A.  If FREQ=A and Q this would not be returned.
        /// <p/>
        /// <b>Note : an initial call to DataReaderEngine.reset will be made</b>
        /// </summary>
        /// <param name="dre">
        /// The data reader engine
        /// </param>
        /// <param name="includeObs">
        /// The include observation
        /// </param>
        /// <param name="includeAttributes">
        /// If true will also report the attributes that have only one value in the entire dataset
        /// </param>
        /// <returns>
        /// The list of dimension
        /// </returns>
        IList<IKeyValue> GetFixedConcepts(IDataReaderEngine dre, bool includeObs, bool includeAttributes);

        #endregion
    }
}
