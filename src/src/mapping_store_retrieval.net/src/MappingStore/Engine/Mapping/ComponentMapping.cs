// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentMapping.cs" company="Eurostat">
//   Date Created : 2013-05-30
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class contains the static method to create IComponentMapping object
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine.Mapping
{
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;

    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    /// <summary>
    ///     This class contains the static method to create IComponentMapping object
    /// </summary>
    public abstract class ComponentMapping
    {
        #region Fields

        /// <summary>
        ///     The column ordinal of the local columns
        /// </summary>
        private readonly List<ColumnOrdinal> _columnOrdinals = new List<ColumnOrdinal>();

        /// <summary>
        ///     The component of this mapping
        /// </summary>
        private ComponentEntity _component;

        /// <summary>
        ///     The mapping associated with this object
        /// </summary>
        private MappingEntity _mapping;

        /// <summary>
        ///     The last <see cref="IDataReader" /> that was used. This field is used to cache the ordinal of the column(s).
        /// </summary>
        private IDataReader _prevReader;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the component
        /// </summary>
        /// <value> A component entity </value>
        public ComponentEntity Component
        {
            get
            {
                return this._component;
            }

            set
            {
                this._component = value;
            }
        }

        /// <summary>
        ///     Gets or sets mapping associated with this object
        /// </summary>
        public MappingEntity Mapping
        {
            get
            {
                return this._mapping;
            }

            set
            {
                this._mapping = value;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the column ordinal of the local columns
        /// </summary>
        protected IList<ColumnOrdinal> ColumnOrdinals
        {
            get
            {
                return this._columnOrdinals;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Create the IComponentMapping object for a specific component in a mapping depending on the type of mapping e.g. 1
        ///     DSD component - N local columns e.t.c.
        /// </summary>
        /// <param name="component">
        /// The component entity
        /// </param>
        /// <param name="mapping">
        /// The mapping entity
        /// </param>
        /// <returns>
        /// An IComponentMapping object based on the mapping or null if the mapping is not supported
        /// </returns>
        /// <exception cref="TranscodingException">
        /// Invalid or unsupported mapping
        /// </exception>
        public static IComponentMapping CreateComponentMapping(ComponentEntity component, MappingEntity mapping)
        {
            IComponentMapping componentMapping;
            if (mapping.Components.Count == 1)
            {
                // one component
                switch (mapping.Columns.Count)
                {
                    case 0: // no columns => constant mapping
                        if (mapping.Constant != null)
                        {
                            componentMapping = new ComponentMapping1C();
                        }
                        else
                        {
                            throw new TranscodingException(string.Format(CultureInfo.CurrentCulture, ErrorMessages.ComponentMappingNoMappingFormat1, mapping.Components[0]));
                        }

                        break;
                    case 1: // 1 to 1 mapping
                        if (mapping.Transcoding != null)
                        {
                            // with transcoding
                            if (mapping.Transcoding.TranscodingRules.ColumnAsKeyPosition.Count > 0 && mapping.Transcoding.TranscodingRules.ComponentAsKeyPosition.Count > 0)
                            {
                                componentMapping = new ComponentMapping1To1T();
                            }
                            else
                            {
                                // transcoding enabled but no transcoding rules
                                // TODO log a warning
                                componentMapping = new ComponentMapping1To1(); // disable transcoding
                            }
                        }
                        else
                        {
                            // without transcoding
                            componentMapping = new ComponentMapping1To1();
                        }

                        break;
                    default: // N columns where N > 1
                        if (mapping.Transcoding != null)
                        {
                            // transcoding is mandatory
                            if (mapping.Transcoding.TranscodingRules.ColumnAsKeyPosition.Count > 0 && mapping.Transcoding.TranscodingRules.ComponentAsKeyPosition.Count > 0)
                            {
                                // there are transcoding rules
                                componentMapping = new ComponentMapping1N();
                            }
                            else
                            {
                                throw new TranscodingException(string.Format(CultureInfo.CurrentCulture, ErrorMessages.ComponentMappingNoTranscodingRulesFormat1, component.Id));
                            }
                        }
                        else
                        {
                            throw new TranscodingException(string.Format(CultureInfo.CurrentCulture, ErrorMessages.ComponentMappingNoTranscodingFormat1, component.Id));
                        }

                        break;
                }
            }
            else if (mapping.Columns.Count == 1 && mapping.Components.Count > 1)
            {
                // N components to 1 column mapping 
                if (mapping.Transcoding != null)
                {
                    // transcoding is mandatory
                    if (mapping.Transcoding.TranscodingRules.ColumnAsKeyPosition.Count > 0 && mapping.Transcoding.TranscodingRules.ComponentAsKeyPosition.Count > 0)
                    {
                        // there are transcoding rules
                        componentMapping = new ComponentMappingNto1(mapping.Transcoding.TranscodingRules.ComponentAsKeyPosition[component.SysId]);
                    }
                    else
                    {
                        throw new TranscodingException(string.Format(CultureInfo.CurrentCulture, ErrorMessages.ComponentMappingNoTranscodingRulesFormat1, component.Id));
                    }
                }
                else
                {
                    throw new TranscodingException(string.Format(CultureInfo.CurrentCulture, ErrorMessages.ComponentMappingNoTranscodingFormat1, component.Id));
                }
            }
            else
            {
                throw new TranscodingException(string.Format(CultureInfo.CurrentCulture, ErrorMessages.ComponentMappingNNMapping, component.Id));
            }

            componentMapping.Mapping = mapping;
            componentMapping.Component = component;
            return componentMapping;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Escape <paramref name="input"/> to make it safer for SQL. It replaces a single quote with two single quotes
        ///     characters.
        /// </summary>
        /// <param name="input">
        /// The literal to escape
        /// </param>
        /// <returns>
        /// The escaped <paramref name="input"/>
        /// </returns>
        protected static string EscapeString(string input)
        {
            return input.Replace("'", "''");
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <param name="dateColumnSysId">The date column sys id.</param>
        /// <returns>The column name</returns>
        protected static string GetColumnName(MappingEntity mapping, long dateColumnSysId)
        {
            return mapping.Columns.First(entity => entity.SysId == dateColumnSysId).Name;
        }

        /// <summary>
        /// Build the <see cref="ColumnOrdinals"/>
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        protected void BuildOrdinals(IDataReader reader)
        {
            if (!ReferenceEquals(reader, this._prevReader))
            {
                this._columnOrdinals.Clear();
                this._prevReader = reader;
            }

            if (this._columnOrdinals.Count == 0)
            {
                bool getPosition = this._mapping.Columns.Count > 1;
                int position = 0;
                foreach (DataSetColumnEntity column in this._mapping.Columns)
                {
                    if (getPosition)
                    {
                        position = this._mapping.Transcoding.TranscodingRules.ColumnAsKeyPosition[column.SysId];
                    }

                    this._columnOrdinals.Add(new ColumnOrdinal { Key = column, Value = reader.GetOrdinal(column.Name), ColumnPosition = position });
                }
            }
        }

        /// <summary>
        /// Build the SQL condition part (based on the provided operator)
        /// </summary>
        /// <param name="mappedId">
        /// The mapped identifier
        /// </param>
        /// <param name="mappedValue">
        /// The mapped value
        /// </param>
        /// <param name="operatorValue">
        /// The value of the ordinal or text search operator
        /// </param>
        /// <returns>
        /// The SQL condition part
        /// </returns>
        protected static string SqlOperatorComponent(string mappedId, string mappedValue, string operatorValue)
        {
            switch (operatorValue)
            {
                case "LIKE %value%":
                    return string.Format(CultureInfo.InvariantCulture, "{0} LIKE '%{1}%' ", mappedId, mappedValue);
                case "NOT LIKE %value%":
                    return string.Format(CultureInfo.InvariantCulture, "{0} NOT LIKE '%{1}%' ", mappedId, mappedValue);
                case "NOT LIKE %value":
                    return string.Format(CultureInfo.InvariantCulture, "{0} NOT LIKE '%{1}' ", mappedId, mappedValue);
                case "NOT LIKE value%":
                    return string.Format(CultureInfo.InvariantCulture, "{0} NOT LIKE '{1}%' ", mappedId, mappedValue);
                case "LIKE %value":
                    return string.Format(CultureInfo.InvariantCulture, "{0} LIKE '%{1}' ", mappedId, mappedValue);
                case "LIKE value%":
                    return string.Format(CultureInfo.InvariantCulture, "{0} LIKE '{1}%' ", mappedId, mappedValue);

            }
            return string.Format(CultureInfo.InvariantCulture, "{0} " + operatorValue + " '{1}' ", mappedId, mappedValue);
        }

        #endregion

        /// <summary>
        ///     This class stores the <see cref="DataSetColumnEntity" /> ordinal
        /// </summary>
        protected class ColumnOrdinal
        {
            #region Public Properties

            /// <summary>
            ///     Gets or sets the Column position in <see cref="TranscodingRulesEntity.ColumnAsKeyPosition" />
            /// </summary>
            public int ColumnPosition { get; set; }

            /// <summary>
            ///     Gets or sets the <see cref="DataSetColumnEntity" />
            /// </summary>
            public DataSetColumnEntity Key { get; set; }

            /// <summary>
            ///     Gets or sets the ordinal of <see cref="Key" />
            /// </summary>
            public int Value { get; set; }

            #endregion
        }
    }
}