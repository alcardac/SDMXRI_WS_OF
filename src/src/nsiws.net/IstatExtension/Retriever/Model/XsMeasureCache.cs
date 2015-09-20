// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XsMeasureCache.cs" company="Eurostat">
//   Date Created : 2011-12-19
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class is used to use buffering in order prevent big TS files when XS measures are mapped.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace IstatExtension.Retriever.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// This class is used to use buffering in order prevent big TS files when XS measures are mapped.
    /// </summary>
    public class XsMeasureCache
    {
        #region Constants and Fields

        /// <summary>
        ///   The list of time and observation value per XS Measure. It is used only when XS measures are mapped
        /// </summary>
        private readonly List<List<ComponentValue>> _attributes =
            new List<List<ComponentValue>>();

        /// <summary>
        ///   The list of all series dimension values minus the measure dimension. It is used only when XS measures are mapped.
        /// </summary>
        private readonly List<ComponentValue> _cachedSeriesAttributes;

        /// <summary>
        ///   The list of all series dimension values minus the measure dimension. It is used only when XS measures are mapped.
        /// </summary>
        private readonly List<ComponentValue> _cachedSeriesKey;

        /// <summary>
        ///   The list of time and observation value per XS Measure. It is used only when XS measures are mapped
        /// </summary>
        private readonly List<KeyValuePair<string, string>> _xsMeasureCachedObservations =
            new List<KeyValuePair<string, string>>();

        /// <summary>
        ///   The measure code
        /// </summary>
        private readonly string _xsMeasureCode;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XsMeasureCache"/> class.
        /// </summary>
        /// <param name="cachedSeriesKey">
        /// The last Cached Series Key. 
        /// </param>
        /// <param name="cachedSeriesAttributes">
        /// The xs Measure Cached Series Attributes. 
        /// </param>
        /// <param name="xsMeasureCode">
        /// The xs Measure Code. 
        /// </param>
        public XsMeasureCache(
            List<ComponentValue> cachedSeriesKey, 
            List<ComponentValue> cachedSeriesAttributes, 
            string xsMeasureCode)
        {
            this._cachedSeriesKey = cachedSeriesKey;
            this._cachedSeriesAttributes = cachedSeriesAttributes;
            this._xsMeasureCode = xsMeasureCode;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the list of time and observation value per XS Measure. It is used only when XS measures are mapped
        /// </summary>
        public List<List<ComponentValue>> Attributes
        {
            get
            {
                return this._attributes;
            }
        }

        /// <summary>
        ///   Gets the list of all series dimension values minus the measure dimension. It is used only when XS measures are mapped.
        /// </summary>
        public IEnumerable<ComponentValue> CachedSeriesAttributes
        {
            get
            {
                return this._cachedSeriesAttributes;
            }
        }

        /// <summary>
        ///   Gets the list of all series dimension values minus the measure dimension. It is used only when XS measures are mapped.
        /// </summary>
        public IEnumerable<ComponentValue> CachedSeriesKey
        {
            get
            {
                return this._cachedSeriesKey;
            }
        }

        /// <summary>
        ///   Gets the list of time and observation value per XS Measure. It is used only when XS measures are mapped
        /// </summary>
        public List<KeyValuePair<string, string>> XSMeasureCachedObservations
        {
            get
            {
                return this._xsMeasureCachedObservations;
            }
        }

        /// <summary>
        ///   Gets the measure code
        /// </summary>
        public string XSMeasureCode
        {
            get
            {
                return this._xsMeasureCode;
            }
        }

        #endregion
    }
}