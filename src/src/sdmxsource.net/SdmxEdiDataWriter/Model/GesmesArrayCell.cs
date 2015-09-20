// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GesmesArrayCell.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   This class holds a series GESMES/TS array
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxEdiDataWriter.Model
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;

    using Estat.Sri.SdmxEdiDataWriter.Helper;

    using Org.Sdmxsource.Sdmx.EdiParser.Constants;
    using Org.Sdmxsource.Sdmx.Util.Date;

    /// <summary>
    ///     This class holds a series GESMES/TS array
    /// </summary>
    internal class GesmesArrayCell
    {
        #region Static Fields

        /// <summary>
        ///     Colon separator
        /// </summary>
        private static readonly string _separatorColon =
            EdiConstants.Colon.ToString(CultureInfo.InvariantCulture);

        #endregion

        #region Fields

        /// <summary>
        ///     Holds the ARR++ with dimensions
        /// </summary>
        private readonly StringBuilder _arrKeys = new StringBuilder();

        /// <summary>
        ///     The dimension values
        /// </summary>
        private readonly string[] _dimensionValues;

        /// <summary>
        ///     The dimension position inside the ARR segment
        /// </summary>
        private readonly GesmesKeyMap _map;

        /// <summary>
        ///     The observations {time period, value, status, confidentiality, pre-break} list
        /// </summary>
        private readonly List<GesmesObservation> _observations = new List<GesmesObservation>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GesmesArrayCell"/> class.
        /// </summary>
        /// <param name="map">
        /// The dimension position inside the ARR segment
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="map"/> is null
        /// </exception>
        public GesmesArrayCell(GesmesKeyMap map)
        {
            if (map == null)
            {
                throw new ArgumentNullException("map");
            }

            this._map = map;

            this._dimensionValues = new string[map.Count];
            this.Observation = new GesmesObservation();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the dimension values
        /// </summary>
        public string[] DimensionValues
        {
            get
            {
                return this._dimensionValues;
            }
        }

        /// <summary>
        ///     Gets the frequency.
        /// </summary>
        public string Frequency
        {
            get
            {
                return this._dimensionValues.Length > 0 ? this._dimensionValues[0] : null;
            }
        }

        /// <summary>
        ///     Gets the GESMES Observation
        /// </summary>
        public GesmesObservation Observation { get; private set; }

        /// <summary>
        ///     Gets or sets the time format for single observation.
        /// </summary>
        public string TimeFormat { get; set; }

        /// <summary>
        ///     Gets or sets the time format for time range.
        /// </summary>
        public string TimeFormatRange { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Add dimension value
        /// </summary>
        /// <param name="dimension">
        /// The dimension id/conceptRef
        /// </param>
        /// <param name="value">
        /// The dimension value
        /// </param>
        public void AddDimensionValue(string dimension, string value)
        {
            int pos;
            if (this._map.TryGetValue(dimension, out pos))
            {
                this._dimensionValues[pos] = value;
            }
        }

        /// <summary>
        ///     Clear keys and observation
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < this._dimensionValues.Length; i++)
            {
                this._dimensionValues[i] = null;
            }

            this.TimeFormat = null;
            this.TimeFormatRange = null;
            this._arrKeys.Length = 0;
            this.ClearObservation();
            this._observations.Clear();
        }

        /// <summary>
        ///     Clear observation
        /// </summary>
        public void ClearObservation()
        {
            this.Observation = new GesmesObservation();
        }

        /// <summary>
        ///     Add new observation. The current is added to an internal list and then cleared.
        /// </summary>
        public void PushObservation()
        {
            this._observations.Add(this.Observation);
            this.ClearObservation();
        }

        /// <summary>
        /// Streams the dimensions and OBS to ARR series segment
        /// </summary>
        /// <param name="gesmesWriter">
        /// The GESMES Writer.
        /// </param>
        public void StreamToGesmes(TextWriter gesmesWriter)
        {
            this.WriteArrKeys(gesmesWriter);
            gesmesWriter.Write(this.Observation.TimePeriod);
            gesmesWriter.Write(_separatorColon);
            gesmesWriter.Write(this.TimeFormat);
            this.Observation.StreamToGesmes(gesmesWriter, _separatorColon);
            gesmesWriter.WriteLine(EdiConstants.EndTag);
        }

        /// <summary>
        /// Streams the dimensions and observation to ARR series segment
        /// </summary>
        /// <param name="gesmesWriter">
        /// The GESMES Writer.
        /// </param>
        /// <param name="gesmes">
        /// The GESMES period 
        /// </param>
        public void StreamToGesmesTimeRange(TextWriter gesmesWriter, GesmesPeriod gesmes)
        {
            if (this._observations.Count == 0)
            {
                throw new InvalidOperationException(
                    " PushObservation must be used to add observations to ToTimeRangeArray");
            }

            this.WriteArrKeys(gesmesWriter);
            this._observations.Sort(
                (observation, observation1) => observation.TimePeriodValue.CompareTo(observation1.TimePeriodValue));

            string minTimePeriod = this._observations[0].TimePeriod;
            string maxTimePeriod = this._observations[this._observations.Count - 1].TimePeriod;

            gesmesWriter.Write(minTimePeriod);
            gesmesWriter.Write(maxTimePeriod);

            gesmesWriter.Write(_separatorColon);

            gesmesWriter.Write(this.TimeFormatRange);

            string plusSeparator = EdiConstants.Plus.ToString(CultureInfo.InvariantCulture);
            string prefixSeparator = EdiConstants.Colon.ToString(CultureInfo.InvariantCulture);
            int lastperiod = this._observations[0].TimePeriodValue;

            for (int i = 0; i < this._observations.Count; i++)
            {
                GesmesObservation observation = this._observations[i];
                int diff = gesmes.Diff(observation.TimePeriodValue, lastperiod);
                if (diff > 1)
                {
                    gesmesWriter.Write(new string(EdiConstants.Plus[0], diff - 1));
                }

                lastperiod = observation.TimePeriodValue;
                observation.StreamToGesmes(gesmesWriter, prefixSeparator);
                prefixSeparator = plusSeparator;
            }

            gesmesWriter.WriteLine(EdiConstants.EndTag);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Optionally build and write the <see cref="_arrKeys"/> to <paramref name="gesmesWriter"/>
        /// </summary>
        /// <param name="gesmesWriter">
        /// The GESMES writer
        /// </param>
        private void WriteArrKeys(TextWriter gesmesWriter)
        {
            if (this._arrKeys.Length == 0)
            {
                GesmesHelper.StartSegment(this._arrKeys, EdiConstants.ArrTag)
                            .Append(EdiConstants.Plus)
                            .Append(string.Join(_separatorColon, this._dimensionValues));
                this._arrKeys.Append(_separatorColon);
            }

            gesmesWriter.Write(this._arrKeys);
        }

        #endregion
    }
}