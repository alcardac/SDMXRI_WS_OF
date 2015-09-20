// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MappedValuesBase.cs" company="Eurostat">
//   Date Created : 2012-01-19
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The mapped values base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.DataRetriever.Model
{
    using System.Collections.Generic;

    using Estat.Sri.MappingStoreRetrieval.Engine.Mapping;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    /// <summary>
    /// The mapped values base.
    /// </summary>
    internal abstract class MappedValuesBase
    {
        #region Constants and Fields

        /// <summary>
        ///   An array of collections used to store all components. The index used is <see cref="CrossSectionalLevels" />
        /// </summary>
        private readonly List<ComponentValue> _componentValues = new List<ComponentValue>();

        /// <summary>
        ///   The measure dimension value
        /// </summary>
        private ComponentValue _measureDimensionValue;

        /// <summary>
        ///   The primary measure value
        /// </summary>
        private ComponentValue _primaryMeasureValue;

        /// <summary>
        ///   The time dimension <see cref="ComponentValue" />
        /// </summary>
        private ComponentValue _timeDimensionValue;

        /// <summary>
        ///   The frequency dimension <see cref="ComponentValue" />
        /// </summary>
        private string _frequencyDimensionValue;

        /// <summary>
        /// The dimension at observation value
        /// </summary>
        private string _dimensionAtObservationValue;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the measure dimension value
        /// </summary>
        public ComponentValue MeasureDimensionValue
        {
            get
            {
                return this._measureDimensionValue;
            }

            protected set
            {
                this._measureDimensionValue = value;
            }
        }

        /// <summary>
        ///   Gets or sets the primary measure value
        /// </summary>
        public ComponentValue PrimaryMeasureValue
        {
            get
            {
                return this._primaryMeasureValue;
            }

            protected set
            {
                this._primaryMeasureValue = value;
            }
        }

        /// <summary>
        ///   Gets or sets the primary measure value
        /// </summary>
        public ComponentValue TimeDimensionValue
        {
            get
            {
                return this._timeDimensionValue;
            }

            protected set
            {
                this._timeDimensionValue = value;
            }
        }

        /// <summary>
        ///   Gets or sets the Time dimension value
        /// </summary>
        public string TimeValue
        {
            get
            {
                return this._timeDimensionValue.Value;
            }

            set
            {
                this._timeDimensionValue.Value = value;
            }
        }

        /// <summary>
        ///   Gets or sets the Time dimension value
        /// </summary>
        public string FrequencyValue
        {
            get
            {
                return this._frequencyDimensionValue;
            }

            set
            {
                this._frequencyDimensionValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the dimension at observation value
        /// </summary>
        public string DimensionAtObservationValue
        {
            get
            {
                return this._dimensionAtObservationValue;
            }

            set
            {
                this._dimensionAtObservationValue = value;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets an array of collections used to store all components.
        /// </summary>
        protected List<ComponentValue> ComponentValues
        {
            get
            {
                return this._componentValues;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add the <paramref name="value"/> to component at <paramref name="index"/> .
        /// </summary>
        /// <param name="index">
        /// The index 
        /// </param>
        /// <param name="value">
        /// The value 
        /// </param>
        public void Add(int index, string value)
        {
            this._componentValues[index].Value = value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Copy the <see cref="ComponentValue.Value"/> of <paramref name="currentKeyValues"/> to <paramref name="previousKeyValues"/>
        /// </summary>
        /// <param name="currentKeyValues">
        /// The list of <see cref="ComponentValue"/> 
        /// </param>
        /// <param name="previousKeyValues">
        /// The list of string 
        /// </param>
        protected static void CopyValues(List<ComponentValue> currentKeyValues, IList<string> previousKeyValues)
        {
            for (int i = 0; i < currentKeyValues.Count; i++)
            {
                previousKeyValues[i] = currentKeyValues[i].Value;
            }
        }

        /// <summary>
        /// Returns a value indicating whether the values in <paramref name="currentKeyValues"/> are equal to the specified <paramref name="previousKeyValues"/> .
        /// </summary>
        /// <param name="currentKeyValues">
        /// The component values. 
        /// </param>
        /// <param name="previousKeyValues">
        /// The previous key values. 
        /// </param>
        /// <returns>
        /// a value indicating whether the values in <paramref name="currentKeyValues"/> are equal to the specified <paramref name="previousKeyValues"/> . 
        /// </returns>
        protected static bool EqualKeyValues(IList<ComponentValue> currentKeyValues, IList<string> previousKeyValues)
        {
            for (int i = 0; i < currentKeyValues.Count; i++)
            {
                if (!string.Equals(currentKeyValues[i].Value, previousKeyValues[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Set the time dimension
        /// </summary>
        /// <param name="timeDimension">
        /// The Time Dimension mapping 
        /// </param>
        /// <returns>
        /// The time dimension <see cref="ComponentValue"/> 
        /// </returns>
        protected ComponentValue SetTimeDimensionComponent(ITimeDimension timeDimension)
        {
            if (timeDimension == null)
            {
                return null;
            }

            this.TimeDimensionValue = new ComponentValue(timeDimension.Component);
            return this.TimeDimensionValue;
        }

        #endregion
    }
}