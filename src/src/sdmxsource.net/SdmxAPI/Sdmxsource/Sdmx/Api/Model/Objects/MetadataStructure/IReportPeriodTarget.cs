// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReportPeriodTarget.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     The ReportPeriodTarget interface.
    /// </summary>
    public interface IReportPeriodTarget : IIdentifiableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the end time of the report period target, null if undefined
        /// </summary>
        /// <value> </value>
        ISdmxDate EndTime { get; }

        /// <summary>
        ///     Gets the start time of the report period target, null if undefined
        /// </summary>
        /// <value> </value>
        ISdmxDate StartTime { get; }

        /// <summary>
        ///     Gets the text type of this report period target, defaults to TEXT_TYPE.OBSERVATIONAL_TIME_PERIOD
        /// </summary>
        /// <value> </value>
        TextType TextType { get; }

        #endregion
    }
}