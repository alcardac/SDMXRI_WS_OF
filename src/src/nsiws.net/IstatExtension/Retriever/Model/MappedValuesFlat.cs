// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MappedValuesFlat.cs" company="Eurostat">
//   Date Created : 2011-12-19
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Holds the mapped and transcoded values without any attachment level information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace IstatExtension.Retriever.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Engine.Mapping;
    using Estat.Sri.MappingStoreRetrieval.Constants;

    /// <summary>
    /// Holds the mapped and transcoded values without any attachment level information.
    /// </summary>
    public class MappedValuesFlat : MappedValuesBase, IMappedValues
    {
        #region Constants and Fields

        /// <summary>
        ///   Attribute values
        /// </summary>
        private readonly List<ComponentValue> _attributeValues = new List<ComponentValue>();

        /// <summary>
        ///   Dimension values
        /// </summary>
        private readonly List<ComponentValue> _dimensionValues = new List<ComponentValue>();

        /// <summary>
        ///   Primary /XS values values
        /// </summary>
        private readonly List<ComponentValue> _measureValues = new List<ComponentValue>();

        /// <summary>
        ///   The current data record reference.
        /// </summary>
        private readonly IDataRecord _record;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MappedValuesFlat"/> class.
        /// </summary>
        /// <param name="info">
        /// The current data retrieval state 
        /// </param>
        public MappedValuesFlat(DataRetrievalInfo info)
        {
            this.SetComponentOrder(info.AllComponentMappings);
            this.SetTimeDimensionComponent(info.TimeTranscoder);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappedValuesFlat"/> class.
        /// </summary>
        /// <param name="record">
        ///   The current data record reference. used to retrieve the local columns 
        /// </param>
        /// <param name="info">
        /// The current data retrieval state 
        /// </param>
        public MappedValuesFlat(IDataRecord record, DataRetrievalInfo info) : this(info)
        {
            this._record = record;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the attribute values
        /// </summary>
        public IEnumerable<ComponentValue> AttributeValues
        {
            get
            {
                return this._attributeValues;
            }
        }

        /// <summary>
        ///   Gets the Dimension values
        /// </summary>
        public IEnumerable<ComponentValue> DimensionValues
        {
            get
            {
                return this._dimensionValues;
            }
        }

        /// <summary>
        ///   Gets the Primary /XS values values
        /// </summary>
        public IEnumerable<ComponentValue> MeasureValues
        {
            get
            {
                return this._measureValues;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns an enumeration of the local column names
        /// </summary>
        /// <returns>
        /// an enumeration of the local column names 
        /// </returns>
        public IEnumerable<string> GetLocalColumns()
        {
            if (this._record == null)
            {
                yield break;
            }

            for (int i = 0; i < this._record.FieldCount; i++)
            {
                yield return this._record.GetName(i);
            }
        }

        /// <summary>
        /// Returns an enumeration of a key value pairs of local column name and value
        /// </summary>
        /// <returns>
        /// an enumeration of a key value pairs of local column name and value 
        /// </returns>
        public IEnumerable<KeyValuePair<string, string>> GetLocalValues()
        {
            if (this._record == null)
            {
                yield break;
            }

            for (int i = 0; i < this._record.FieldCount; i++)
            {
                string key = this._record.GetName(i);
                string value = this._record.IsDBNull(i)
                                   ? string.Empty
                                   : Convert.ToString(this._record.GetValue(i), CultureInfo.InvariantCulture);

                yield return new KeyValuePair<string, string>(key, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize the public order of the components based on the specified <paramref name="componentMappings"/>
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
                switch (componentMapping.Component.ComponentType)
                {
                    case SdmxComponentType.Dimension:
                        this._dimensionValues.Add(componentValue);
                        break;
                    case SdmxComponentType.Attribute:
                        this._attributeValues.Add(componentValue);
                        break;
                    case SdmxComponentType.PrimaryMeasure:
                    case SdmxComponentType.CrossSectionalMeasure:
                        this._measureValues.Add(componentValue);
                        break;
                }
            }
        }

        #endregion
    }
}