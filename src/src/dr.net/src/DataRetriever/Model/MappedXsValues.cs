// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MappedXsValues.cs" company="Eurostat">
//   Date Created : 2011-12-19
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The mapped xs values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.DataRetriever.Model
{
    using System;
    using System.Collections.Generic;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Engine.Mapping;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    /// <summary>
    /// The mapped xs values.
    /// </summary>
    internal class MappedXsValues : MappedValuesBase, IMappedValues
    {
        #region Constants and Fields

        /// <summary>
        ///   An array of collections used to store the attributes of different cross sectional levels. The index used is <see
        ///    cref="CrossSectionalLevels" />
        /// </summary>
        private readonly List<List<ComponentValue>> _attributeValues = new List<List<ComponentValue>>(4);

        /// <summary>
        ///   The data set level index
        /// </summary>
        private readonly int _dataSetIndex;

        /// <summary>
        ///   An array of collections used to store the dimensions of different cross sectional levels. The index used is <see
        ///    cref="CrossSectionalLevels" />
        /// </summary>
        private readonly List<List<ComponentValue>> _dimensionValues = new List<List<ComponentValue>>(4);

        /// <summary>
        ///   The group level index
        /// </summary>
        private readonly int _groupIndex;

        /// <summary>
        ///   An array of collections used to store the dimensions of different cross sectional levels. The index used is <see
        ///    cref="CrossSectionalLevels" />
        /// </summary>
        private readonly List<List<ComponentValue>> _keyValues = new List<List<ComponentValue>>(4);

        /// <summary>
        ///   The observation level index
        /// </summary>
        private readonly int _observationIndex;

        /// <summary>
        ///   The previous key values
        /// </summary>
        private readonly string[][] _previousKeyValues = new string[5][];

        /// <summary>
        ///   The section level index
        /// </summary>
        private readonly int _sectionIndex;

        /// <summary>
        ///   A value indicating whether a specific level has started at all.
        /// </summary>
        private readonly bool[] _startedLevel = new bool[4];

        /// <summary>
        ///   CrossSectional measure values
        /// </summary>
        private readonly List<ComponentValue> _xsMeasureValues = new List<ComponentValue>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MappedXsValues"/> class.
        /// </summary>
        /// <param name="info">
        /// The info. 
        /// </param>
        public MappedXsValues(DataRetrievalInfo info)
        {
            var values = (CrossSectionalLevels[])Enum.GetValues(typeof(CrossSectionalLevels));

            for (int i = 0; i < values.Length; i++)
            {
                this._dimensionValues.Add(new List<ComponentValue>());
                this._attributeValues.Add(new List<ComponentValue>());
                this._keyValues.Add(new List<ComponentValue>());
            }

            this._dataSetIndex = GetIndex(CrossSectionalLevels.DataSet);
            this._groupIndex = GetIndex(CrossSectionalLevels.Group);
            this._sectionIndex = GetIndex(CrossSectionalLevels.Section);
            this._observationIndex = GetIndex(CrossSectionalLevels.Observation);

            this.SetComponentOrder(info.AllComponentMappings);
            if (info.TimeTranscoder != null)
            {
                var timeDimensionComponent = this.SetTimeDimensionComponent(info.TimeTranscoder);
                this.HandleTimeDimension(timeDimensionComponent);
            }

            for (int i = 0; i < this._keyValues.Count; i++)
            {
                List<ComponentValue> keyValue = this._keyValues[i];
                this._previousKeyValues[i] = new string[keyValue.Count];
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the Attributes attached to data set level
        /// </summary>
        public IEnumerable<ComponentValue> DataSetLevelAttributeValues
        {
            get
            {
                return this._attributeValues[this._dataSetIndex];
            }
        }

        /// <summary>
        ///   Gets the Dimensions attached to data set level
        /// </summary>
        public IEnumerable<ComponentValue> DataSetLevelDimensionValues
        {
            get
            {
                return this._dimensionValues[this._dataSetIndex];
            }
        }

        /// <summary>
        ///   Gets the Attributes attached to group level
        /// </summary>
        public IEnumerable<ComponentValue> GroupLevelAttributeValues
        {
            get
            {
                return this._attributeValues[this._groupIndex];
            }
        }

        /// <summary>
        ///   Gets the Dimensions attached to group level
        /// </summary>
        public IEnumerable<ComponentValue> GroupLevelDimensionValues
        {
            get
            {
                return this._dimensionValues[this._groupIndex];
            }
        }

        /// <summary>
        ///   Gets the Attributes attached to group level
        /// </summary>
        public IEnumerable<ComponentValue> ObservationLevelAttributeValues
        {
            get
            {
                return this._attributeValues[this._observationIndex];
            }
        }

        /// <summary>
        ///   Gets the Dimensions attached to group level
        /// </summary>
        public IEnumerable<ComponentValue> ObservationLevelDimensionValues
        {
            get
            {
                return this._dimensionValues[this._observationIndex];
            }
        }

        /// <summary>
        ///   Gets the Attributes attached to section level
        /// </summary>
        public IEnumerable<ComponentValue> SectionAttributeValues
        {
            get
            {
                return this._attributeValues[this._sectionIndex];
            }
        }

        /// <summary>
        ///   Gets the Dimensions attached to section level
        /// </summary>
        public IEnumerable<ComponentValue> SectionLevelDimensionValues
        {
            get
            {
                return this._dimensionValues[this._sectionIndex];
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether dataset has been parsed.
        /// </summary>
        public bool StartedDataSet
        {
            get
            {
                return this._startedLevel[this._dataSetIndex];
            }

            set
            {
                this._startedLevel[this._dataSetIndex] = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the value for the specified <paramref name="component"/>
        /// </summary>
        /// <param name="component">
        /// The CrossSectional measure 
        /// </param>
        /// <returns>
        /// the value for the specified <paramref name="component"/> 
        /// </returns>
        public string GetXSMeasureValue(ComponentEntity component)
        {
            return this._xsMeasureValues.Find(x => x.Key.Equals(component)).Value;
        }

        /// <summary>
        /// Gets a value indicating whether this is a new group key or it is the same as the last one.
        /// </summary>
        /// <returns>
        /// a value indicating whether this is a new group key or it is the same as the last one. 
        /// </returns>
        public bool IsNewGroupKey()
        {
            bool newKeyAtLevel = this.IsNewKeyAtLevel(this._groupIndex);
            if (newKeyAtLevel)
            {
                this._startedLevel[this._sectionIndex] = false;
            }

            return newKeyAtLevel;
        }

        /// <summary>
        /// Gets a value indicating whether this is a new group key or it is the same as the last one.
        /// </summary>
        /// <returns>
        /// a value indicating whether this is a new group key or it is the same as the last one. 
        /// </returns>
        public bool IsNewSectionKey()
        {
            return this.IsNewKeyAtLevel(this._sectionIndex);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the index for cross sectional level from the specified <paramref name="level"/>
        /// </summary>
        /// <param name="level">
        /// The <see cref="CrossSectionalLevels"/> 
        /// </param>
        /// <returns>
        /// the index for cross sectional level from the specified <paramref name="level"/> 
        /// </returns>
        private static int GetIndex(CrossSectionalLevels level)
        {
            var value = (int)level;
            return (value >> 1) - (value >> 0x3);
        }

        /// <summary>
        /// Handle Time Dimension components
        /// </summary>
        /// <param name="keyedValue">
        /// The time dimension <see cref="ComponentValue"/> 
        /// </param>
        private void HandleTimeDimension(ComponentValue keyedValue)
        {
            CrossSectionalLevels level = keyedValue.Key.EffectiveCrossSectionalAttachmentLevel
                                         == CrossSectionalLevels.None
                                             ? CrossSectionalLevels.Group
                                             : keyedValue.Key.EffectiveCrossSectionalAttachmentLevel;
            int index = GetIndex(level);
            this._keyValues[index].Add(keyedValue);
            this._dimensionValues[index].Add(keyedValue);
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ComponentValue.Value"/> in <see cref="_keyValues"/> equal to <see cref="_previousKeyValues"/> at the specified <paramref name="index"/>
        /// </summary>
        /// <param name="index">
        /// The attachment level index, <see cref="GetIndex"/> . 
        /// </param>
        /// <returns>
        /// a value indicating whether the <see cref="ComponentValue.Value"/> in <see cref="_keyValues"/> equal to <see cref="_previousKeyValues"/> at the specified <paramref name="index"/> 
        /// </returns>
        private bool IsNewKeyAtLevel(int index)
        {
            List<ComponentValue> currentKeyValues = this._keyValues[index];
            string[] previousKeyValues = this._previousKeyValues[index];

            if (!this._startedLevel[index])
            {
                CopyValues(currentKeyValues, previousKeyValues);
                this._startedLevel[index] = true;
                return true;
            }

            bool ret = !EqualKeyValues(currentKeyValues, previousKeyValues);
            if (ret)
            {
                CopyValues(currentKeyValues, previousKeyValues);
            }

            return ret;
        }

        /// <summary>
        /// Initialize the internal order of the components based on the specified <paramref name="componentMappings"/>
        /// </summary>
        /// <param name="componentMappings">
        /// The component mappings 
        /// </param>
        private void SetComponentOrder(IEnumerable<IComponentMapping> componentMappings)
        {
            foreach (IComponentMapping componentMapping in componentMappings)
            {
                var componentValue = new ComponentValue(componentMapping.Component);
                this.ComponentValues.Add(componentValue);
                CrossSectionalLevels attachmentLevel = componentMapping.Component.EffectiveCrossSectionalAttachmentLevel;
                attachmentLevel = attachmentLevel == CrossSectionalLevels.None
                                  && componentMapping.Component.FrequencyDimension
                                      ? CrossSectionalLevels.Group
                                      : attachmentLevel;
                int index = GetIndex(attachmentLevel);

                switch (componentMapping.Component.ComponentType)
                {
                    case SdmxComponentType.Dimension:
                        if (!componentMapping.Component.MeasureDimension)
                        {
                            this._keyValues[index].Add(componentValue);
                            this._dimensionValues[index].Add(componentValue);
                        }
                        else
                        {
                            this.MeasureDimensionValue = componentValue;
                        }

                        break;
                    case SdmxComponentType.Attribute:
                        this._attributeValues[index].Add(componentValue);
                        break;
                    case SdmxComponentType.PrimaryMeasure:
                        this.PrimaryMeasureValue = componentValue;
                        break;
                    case SdmxComponentType.CrossSectionalMeasure:
                        this._xsMeasureValues.Add(componentValue);
                        break;
                }
            }
        }

        #endregion
    }
}