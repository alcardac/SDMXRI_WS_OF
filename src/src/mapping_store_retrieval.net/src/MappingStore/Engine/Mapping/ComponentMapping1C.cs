// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentMapping1C.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Handles constant mapping
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Engine.Mapping
{
    using System.Data;
    using System.Globalization;

    /// <summary>
    /// Handles constant mapping
    /// </summary>
    internal class ComponentMapping1C : ComponentMapping, IComponentMapping
    {
        #region Constants and Fields

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates the SQL Where clause for the constant value used in this mapping
        /// and the condition value from SDMX Query which is transcoded
        /// </summary>
        /// <param name="conditionValue">
        /// string with the conditional value from the SDMX query
        /// </param>
        /// <param name="operatorValue">
        /// string with the operator value from the SDMX query, "=" by default
        /// </param>
        /// <returns>
        /// A SQL where clause for the column of the mapping
        /// </returns>
        public string GenerateComponentWhere(string conditionValue, string operatorValue = "=")
        {
            var quotedConstantValue = string.Format(CultureInfo.InvariantCulture, "'{0}'", this.Mapping.Constant);
            var escapeString = EscapeString(conditionValue);
            if (operatorValue.Contains("value"))
            {
                return string.Format(CultureInfo.InvariantCulture, " ( {0} ) ", SqlOperatorComponent(quotedConstantValue, escapeString, operatorValue));
            }

            return string.Format(CultureInfo.InvariantCulture, " ( {0} {2} '{1}' ) ", quotedConstantValue, escapeString, operatorValue);
        }

        /// <summary>
        /// Maps the constant value of the mapping to the component of this ComponentMapping1C object 
        /// </summary>
        /// <param name="reader">
        /// The DataReader for retrieving the values of the column.
        /// </param>
        /// <returns>
        /// The constant value
        /// </returns>
        public string MapComponent(IDataReader reader)
        {
            return this.Mapping.Constant;
        }
        #endregion
    }
}