// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReportedDateEngine.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.DataParser.Engine
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;

    #endregion

    /// <summary>
    /// The ReportedDateEngine interface.
    /// </summary>
    public interface IReportedDateEngine
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets all reported dates.
        /// </summary>
        /// <param name="dataReaderEngine">The data reader engine</param>
        /// <returns>
        /// The reported dates
        /// </returns>
        IDictionary<TimeFormat, IList<string>> GetAllReportedDates(IDataReaderEngine dataReaderEngine);

        #endregion
    }
}
