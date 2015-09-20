// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GesmesAttributeGroup.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Represents a GESMES attribute group ARR+... and it's IDE - FTX/CDV
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxEdiDataWriter.Model
{
    using System;
    using System.Collections.Generic;

    using Estat.Sri.SdmxEdiDataWriter.Constants;

    using Org.Sdmxsource.Sdmx.EdiParser.Properties;

    /// <summary>
    ///     Represents a GESMES attribute group ARR+... and it's IDE - FTX/CDV
    /// </summary>
    internal class GesmesAttributeGroup
    {
        #region Fields

        /// <summary>
        ///     the attribute name and values list
        /// </summary>
        private readonly IList<KeyValuePair<string, string>> _attributeValues = new List<KeyValuePair<string, string>>();

        /// <summary>
        ///     The dimension values
        /// </summary>
        private readonly string[] _dimensionValues;

        /// <summary>
        ///     The first non-wildcard dimension
        /// </summary>
        private readonly int _firstDimension;

        /// <summary>
        ///     The attachment level code
        /// </summary>
        private readonly AttachmentLevel _level;

        /// <summary>
        ///     The dimension position inside the ARR segment
        /// </summary>
        private readonly GesmesKeyMap _map;

        /// <summary>
        ///     The time format position in <see cref="_dimensionValues" />
        /// </summary>
        private readonly int _timeFormatPosition = -1;

        /// <summary>
        ///     The time period position in <see cref="_dimensionValues" />
        /// </summary>
        private readonly int _timePeriodPosition = -1;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GesmesAttributeGroup"/> class.
        /// </summary>
        /// <param name="map">
        /// The map.
        /// </param>
        /// <param name="level">
        /// The attachment level 
        /// </param>
        public GesmesAttributeGroup(GesmesKeyMap map, RelStatus level)
        {
            this._map = map;
            switch (level)
            {
                case RelStatus.None:
                    throw new ArgumentException(Resources.ErrorInvalidAttachmentLevel, "level");
                case RelStatus.DataSet:

                    // no dimension
                    this._dimensionValues = new string[0];
                    this._level = AttachmentLevel.DataSet;
                    break;
                case RelStatus.Sibling:

                    // all dimensions. FREQ is wildcarded
                    this._dimensionValues = new string[map.Count];
                    this._dimensionValues[0] = string.Empty;
                    this._firstDimension = 1;
                    this._level = AttachmentLevel.Group;
                    break;
                case RelStatus.Series:

                    // all dimensions
                    this._dimensionValues = new string[map.Count];
                    this._level = AttachmentLevel.Series;
                    break;
                case RelStatus.Observation:

                    // all dimensions plus time period and time format
                    this._dimensionValues = new string[map.Count + 2];
                    this._timePeriodPosition = map.Count;
                    this._timeFormatPosition = this._timePeriodPosition + 1;
                    this._level = AttachmentLevel.Observation;
                    break;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the attribute name and values list
        /// </summary>
        public IList<KeyValuePair<string, string>> AttributeValues
        {
            get
            {
                return this._attributeValues;
            }
        }

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
        ///     Gets the attachment level code
        /// </summary>
        public AttachmentLevel Level
        {
            get
            {
                return this._level;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Add the attribute with the specified <paramref name="name"/> and <paramref name="value"/>
        /// </summary>
        /// <param name="name">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        public void AddAttributeValue(string name, string value)
        {
            this._attributeValues.Add(new KeyValuePair<string, string>(name, value));
        }

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
        /// Add <paramref name="timePeriod"/> and <paramref name="timeFormat"/>
        /// </summary>
        /// <param name="timePeriod">
        /// The TIME PERIOD value
        /// </param>
        /// <param name="timeFormat">
        /// The TIME FORMAT value
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// AddTimeValue can only be used with Observation level
        /// </exception>
        public void AddTimeValue(string timePeriod, string timeFormat)
        {
            if (this._timePeriodPosition == -1)
            {
                throw new InvalidOperationException("AddTimeValue can only be used with Observation level");
            }

            this._dimensionValues[this._timePeriodPosition] = timePeriod;
            this._dimensionValues[this._timeFormatPosition] = timeFormat;
        }

        /// <summary>
        ///     Set to null the <see cref="DimensionValues" /> (except wildcarded) and clear all <see cref="AttributeValues" />
        /// </summary>
        public void Clear()
        {
            for (int i = this._firstDimension; i < this._dimensionValues.Length; i++)
            {
                this._dimensionValues[i] = null;
            }

            this._attributeValues.Clear();
        }

        #endregion
    }
}