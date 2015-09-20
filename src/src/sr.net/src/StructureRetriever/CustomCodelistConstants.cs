// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomCodelistConstants.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class holds constants for custom codelists used in special requests
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// This class holds constants for custom codelists used in special requests
    /// </summary>
    public static class CustomCodelistConstants
    {
        #region Constants and Fields

        /// <summary>
        ///   The Agency for custom codelists
        /// </summary>
        public const string Agency = "MA";

        /// <summary>
        ///   The custom dataflow data count code default value
        /// </summary>
        public const string CountCodeDefault = "0";

        /// <summary>
        ///   The custom dataflow data count code description
        /// </summary>
        public const string CountCodeDescription = "Data count";

        /// <summary>
        ///   The custom dataflow data count codelist
        /// </summary>
        public const string CountCodeList = "CL_COUNT";

        /// <summary>
        ///   The custom dataflow data count codelist name
        /// </summary>
        public const string CountCodeListName = "Special dataflow count codelist";

        /// <summary>
        ///   The language for custom codelist names
        /// </summary>
        public const string Lang = "en";

        /// <summary>
        ///   The custom time dimension start/end codelist
        /// </summary>
        public const string TimePeriodCodeList = "CL_TIME_PERIOD";

        /// <summary>
        ///   The custom time dimension start/end codelist name
        /// </summary>
        public const string TimePeriodCodeListName = "Time Dimension Start and End periods";

        /// <summary>
        ///   The custom time dimension end code description
        /// </summary>
        public const string TimePeriodEndDescription = "End Time period";

        /// <summary>
        ///   The custom time dimension max value
        /// </summary>
        public const string TimePeriodMax = "9999";

        /// <summary>
        ///   The custom time dimension min value
        /// </summary>
        public const string TimePeriodMin = "0001";

        /// <summary>
        ///   The custom time dimension start code description
        /// </summary>
        public const string TimePeriodStartDescription = "Start Time period";

        /// <summary>
        ///   The version used for custom codelists
        /// </summary>
        public const string Version = "1.0";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Check if the specified <c>CodelistRefBean</c> is for the special COUNT request
        /// </summary>
        /// <param name="codeListRef">
        /// The <c>CodelistRefBean</c> object. It should have Id and Agency set. Version is ignored. 
        /// </param>
        /// <returns>
        /// True if the <c>CodelistRefBean</c> ID and Agency matches the custom <see cref="CountCodeList"/> and <see cref="Agency"/> . Else false 
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "We want to accept only CodelistRefBean")]
        public static bool IsCountRequest(IMaintainableRefObject codeListRef)
        {
            return CountCodeList.Equals(codeListRef.MaintainableId, StringComparison.OrdinalIgnoreCase)
                   && Agency.Equals(codeListRef.AgencyId, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Check if the specified <c>CodelistRefBean</c> is for the special Time Dimension request
        /// </summary>
        /// <param name="codeListRef">
        /// The <c>CodelistRefBean</c> object. It should have Id and Agency set. Version is ignored. 
        /// </param>
        /// <returns>
        /// True if the <c>CodelistRefBean</c> ID and Agency matches the custom <see cref="TimePeriodCodeList"/> and <see cref="Agency"/> . Else false 
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "We want to accept only CodelistRefBean")]
        public static bool IsTimeDimensionRequest(IMaintainableRefObject codeListRef)
        {
            return TimePeriodCodeList.Equals(codeListRef.MaintainableId, StringComparison.OrdinalIgnoreCase)
                   && Agency.Equals(codeListRef.AgencyId, StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}