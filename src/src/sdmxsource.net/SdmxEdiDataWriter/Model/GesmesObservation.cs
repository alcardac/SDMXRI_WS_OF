// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GesmesObservation.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The GESMES observation value and flags
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxEdiDataWriter.Model
{
    using System.Globalization;
    using System.IO;

    using Estat.Sri.SdmxEdiDataWriter.Constants;

    using Org.Sdmxsource.Sdmx.EdiParser.Constants;

    /// <summary>
    ///     The GESMES observation value and flags
    /// </summary>
    internal class GesmesObservation
    {
        #region Fields

        /// <summary>
        ///     Gets or sets the time period.
        /// </summary>
        private string _timePeriod;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the observation confidentiality.
        /// </summary>
        public string ObservationConf { get; set; }

        /// <summary>
        ///     Gets or sets the observation pre break.
        /// </summary>
        public string ObservationPreBreak { get; set; }

        /// <summary>
        ///     Gets or sets the observation status.
        /// </summary>
        public string ObservationStatus { get; set; }

        /// <summary>
        ///     Gets or sets the observation value.
        /// </summary>
        public string ObservationValue { get; set; }

        /// <summary>
        ///     Gets or sets the time period.
        /// </summary>
        public string TimePeriod
        {
            get
            {
                return this._timePeriod;
            }

            set
            {
                this._timePeriod = value;
                int num;
                this.TimePeriodValue = int.TryParse(this._timePeriod, out num) ? num : int.MinValue;
            }
        }

        /// <summary>
        ///     Gets the time period as <c>int</c>
        /// </summary>
        public int TimePeriodValue { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Stream this instance to <paramref name="gesmesWriter"/>
        /// </summary>
        /// <param name="gesmesWriter">
        /// The GESMES writer
        /// </param>
        /// <param name="prefixSeparator">
        /// The separator that should be added in front.
        /// </param>
        public void StreamToGesmes(TextWriter gesmesWriter, string prefixSeparator)
        {
            string colon = EdiConstants.Colon.ToString(CultureInfo.InvariantCulture);
            var array = new[]
                            {
                                this.ObservationValue, this.ObservationStatus, this.ObservationConf, 
                                this.ObservationPreBreak
                            };
            int i = array.Length - 1;
            while (i >= 0 && string.IsNullOrEmpty(array[i]))
            {
                i--;
            }

            if (i == -1)
            {
                return;
            }

            string separator = prefixSeparator;

            for (int j = 0; j <= i; j++)
            {
                gesmesWriter.Write(separator);
                gesmesWriter.Write(array[j]);
                separator = colon;
            }
        }

        /// <summary>
        ///     Returns a <see cref="T:System.String" /> that contains the <see cref="ObservationValue" />,
        ///     <see cref="ObservationStatus" />
        ///     , <see cref="ObservationConf" /> and <see cref="ObservationPreBreak" /> separated by
        ///     <see cref="EdiConstants.Colon" />
        /// </summary>
        /// <returns>
        ///     a <see cref="T:System.String" /> that contains the <see cref="ObservationValue" />, <see cref="ObservationStatus" />,
        ///     <see cref="ObservationConf" />
        ///     and <see cref="ObservationPreBreak" /> separated by
        ///     <see cref="EdiConstants.Colon" />
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            var array = new[]
                            {
                                this.ObservationValue, this.ObservationStatus, this.ObservationConf, 
                                this.ObservationPreBreak
                            };
            int i = array.Length - 1;
            while (i >= 0 && string.IsNullOrEmpty(array[i]))
            {
                i--;
            }

            if (i == -1)
            {
                return string.Empty;
            }

            return string.Join(EdiConstants.Colon.ToString(CultureInfo.InvariantCulture), array, 0, i + 1);
        }

        #endregion
    }
}