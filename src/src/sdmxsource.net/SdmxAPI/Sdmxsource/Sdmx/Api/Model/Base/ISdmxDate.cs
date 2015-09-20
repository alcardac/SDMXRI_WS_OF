// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxDate.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Base
{
    #region Using directives

    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    #endregion

    /// <summary>
    ///     An SDMX date contains a Java Date, and gives access to the string representation and TIME_FORMAT which is used to define the date
    /// </summary>
    public interface ISdmxDate : ISerializable
    {
        #region Public Properties

        /// <summary>
        ///     Gets a copy of the date  - this can never be null
        /// </summary>
        /// <value> </value>
        DateTime? Date { get; }

        /// <summary>
        ///     Gets the Date in SDMX format
        /// </summary>
        string DateInSdmxFormat { get; }

        /// <summary>
        ///     Gets the time format for the date - returns null if this information is not present
        /// </summary>
        /// <value> </value>
        TimeFormat TimeFormatOfDate { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a value indicating whether the date is later then the date provided
        /// </summary>
        /// <param name="date">Sdmx date
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        bool IsLater(ISdmxDate date);

        #endregion
    }
}