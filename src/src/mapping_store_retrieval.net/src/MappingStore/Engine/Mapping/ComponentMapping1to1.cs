// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentMapping1to1.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Handles 1-1 mappings without transconding
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Engine.Mapping
{
    using System;
    using System.Data;
    using System.Globalization;

    /// <summary>
    /// Handles 1-1 mappings without transconding
    /// </summary>
    internal class ComponentMapping1To1 : ComponentMapping, IComponentMapping
    {
        #region Constants and Fields

        /// <summary>
        /// The position of the column of this mapping inside the row
        /// in the reader
        /// </summary>
        private int _columnOrdinal = -1;

        /// <summary>
        /// The type of the column of this mapping E.g. string, float, int e.t.c.
        /// </summary>
        private Type _fieldType;

        /// <summary>
        /// The last data reader
        /// </summary>
        private IDataReader _lastReader;

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates the SQL Where clause for the component used in this mapping
        /// and the condition value from SDMX Query as it is
        /// </summary>
        /// <param name="conditionValue">
        /// string with the conditional value from the SDMX query
        /// </param>
        /// <param name="operatorValue">
        /// string with the operator value from the sdmx query, "=" by default
        /// </param>
        /// <returns>
        /// A SQL where clause for the column of the mapping
        /// </returns>
        public string GenerateComponentWhere(string conditionValue, string operatorValue = "=")
        {
            return " ( " + SqlOperatorComponent(this.Mapping.Columns[0].Name, EscapeString(conditionValue), operatorValue) + ") ";
            //return string.Format(CultureInfo.InvariantCulture, " ( {0} " + operatorValue + " '{1}' ) ", this.Mapping.Columns[0].Name, EscapeString(conditionValue));
        }

        /// <summary>
        /// Maps the column of the mapping to the component of this ComponentMapping1to1 object 
        /// </summary>
        /// <param name="reader">
        /// The DataReader for retrieving the values of the column.
        /// </param>
        /// <returns>
        /// The value of the component or String.Empty in case the column value is null
        /// </returns>
        public string MapComponent(IDataReader reader)
        {
            if (!ReferenceEquals(reader, this._lastReader))
            {
                this._columnOrdinal = -1;
                this._lastReader = reader;
            }

            if (this._columnOrdinal == -1)
            {
                this._columnOrdinal = reader.GetOrdinal(this.Mapping.Columns[0].Name);
                this._fieldType = reader.GetFieldType(this._columnOrdinal);
            }

            string val = string.Empty;
            if (!reader.IsDBNull(this._columnOrdinal))
            {
                if (this._fieldType == typeof(double))
                {
                    val = reader.GetDouble(this._columnOrdinal).ToString(CultureInfo.InvariantCulture);
                }
                else if (this._fieldType == typeof(float))
                {
                    val = reader.GetFloat(this._columnOrdinal).ToString(CultureInfo.InvariantCulture);
                }
                else if (this._fieldType == typeof(string))
                {
                    val = reader.GetString(this._columnOrdinal);
                }
                else
                {
                    val = Convert.ToString(reader.GetValue(this._columnOrdinal), CultureInfo.InvariantCulture);
                }
            }

            return val;
        }

        #endregion
    }
}