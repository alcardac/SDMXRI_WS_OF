// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeDimensionCodeListHelper.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   A collection of helper methods for Time Dimension Codelist
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Engines
{
    using Estat.Sri.MappingStoreRetrieval.Extensions;

    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;

    /// <summary>
    /// A collection of helper methods for Time Dimension Codelist
    /// </summary>
    internal static class TimeDimensionCodeListHelper
    {
        #region Public Methods and Operators

        /// <summary>
        /// Setup a CodelistBean object as a TimeDimension codelist containing two codes, startTime and endTime and using as codelist id the <see cref="CustomCodelistConstants.TimePeriodCodeList"/> .
        /// </summary>
        /// <param name="startCode">
        /// The code that will contain the start Time period 
        /// </param>
        /// <param name="endCode">
        /// The code that will contain the end Time period 
        /// </param>
        /// <param name="timeCodeList">
        /// The codelist to setup 
        /// </param>
        public static void SetupTimeCodelist(ICodeMutableObject startCode, ICodeMutableObject endCode, ICodelistMutableObject timeCodeList)
        {
            timeCodeList.Id = CustomCodelistConstants.TimePeriodCodeList;
            timeCodeList.AgencyId = CustomCodelistConstants.Agency;
            timeCodeList.Version = CustomCodelistConstants.Version;
            timeCodeList.Names.Add(
                new TextTypeWrapperMutableCore { Locale = CustomCodelistConstants.Lang, Value = CustomCodelistConstants.TimePeriodCodeListName });
            startCode.Names.Add(
                new TextTypeWrapperMutableCore { Locale = CustomCodelistConstants.Lang, Value = CustomCodelistConstants.TimePeriodStartDescription });
            timeCodeList.AddItem(startCode);

            if (endCode != null)
            {
                endCode.Names.Add(
                    new TextTypeWrapperMutableCore { Locale = CustomCodelistConstants.Lang, Value = CustomCodelistConstants.TimePeriodEndDescription });

                timeCodeList.AddItem(endCode);
            }
        }

        #endregion

        /// <summary>
        /// Builds the time codelist.
        /// </summary>
        /// <param name="minPeriod">The minimum period.</param>
        /// <param name="maxPeriod">The maximum period.</param>
        /// <returns>the time codelist.</returns>
        public static ICodelistMutableObject BuildTimeCodelist(ISdmxDate minPeriod, ISdmxDate maxPeriod)
        {
            ICodelistMutableObject timeCodeList = new CodelistMutableCore();
            var startDate = minPeriod.FormatAsDateString(true);
            ICodeMutableObject startCode = new CodeMutableCore { Id = startDate };
            var endDate = maxPeriod.FormatAsDateString(false);
            ICodeMutableObject endCode = !startDate.Equals(endDate) ? new CodeMutableCore { Id = endDate } : null;
            SetupTimeCodelist(startCode, endCode, timeCodeList);
            return timeCodeList;
        }
    }
}