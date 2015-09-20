// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeRangeXmlAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The time range xml assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///     The time range xml assembler.
    /// </summary>
    public class TimeRangeXmlAssembler : IAssembler<TimeRangeValueType, ITimeRange>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The assemble.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <param name="assembleFrom">
        /// The assemble from.
        /// </param>
        public virtual void Assemble(TimeRangeValueType assembleInto, ITimeRange assembleFrom)
        {
            if (assembleFrom.Range)
            {
                SetDate(
                    assembleInto.StartPeriod = new TimePeriodRangeType(), 
                    assembleFrom.StartDate, 
                    assembleFrom.StartInclusive);
                SetDate(
                    assembleInto.EndPeriod = new TimePeriodRangeType(), assembleFrom.EndDate, assembleFrom.EndInclusive);
            }
            else
            {
                if (assembleFrom.StartDate != null)
                {
                    SetDate(
                        assembleInto.StartPeriod = new TimePeriodRangeType(), 
                        assembleFrom.StartDate, 
                        assembleFrom.StartInclusive);
                }
                else
                {
                    SetDate(
                        assembleInto.EndPeriod = new TimePeriodRangeType(), 
                        assembleFrom.EndDate, 
                        assembleFrom.EndInclusive);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the date.
        /// </summary>
        /// <param name="timePeriodRangeType">
        /// The time Period Range Type.
        /// </param>
        /// <param name="sdmxDate">
        /// The sdmxDate.
        /// </param>
        /// <param name="inclusive">
        /// The inclusive.
        /// </param>
        private static void SetDate(TimePeriodRangeType timePeriodRangeType, ISdmxDate sdmxDate, bool inclusive)
        {
            timePeriodRangeType.isInclusive = inclusive;
            timePeriodRangeType.TypedValue = sdmxDate.DateInSdmxFormat;
        }

        #endregion
    }
}