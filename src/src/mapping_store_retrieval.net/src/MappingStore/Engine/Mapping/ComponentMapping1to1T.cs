// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentMapping1to1T.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Handles 1-1 mappings with transconding
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#define MAT200

namespace Estat.Sri.MappingStoreRetrieval.Engine.Mapping
{
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    /// <summary>
    /// Handles 1-1 mappings with transconding
    /// </summary>
    internal class ComponentMapping1To1T : ComponentMapping, IComponentMapping
    {
        #region Constants and Fields
        #endregion

        #region Public Methods

        /// <summary>
        /// Generates the SQL Where clause for the component used in this mapping
        /// and the condition value from SDMX Query which is transcoded 
        /// </summary>
        /// <param name="conditionValue">
        /// string with the conditional value from the sdmx query
        /// </param>
        /// <param name="operatorValue">
        /// string with the operator value from the sdmx query, "=" by default
        /// </param>
        /// <returns>
        /// A SQL where clause for the columns of the mapping
        /// </returns>
        public string GenerateComponentWhere(string conditionValue, string operatorValue = "=")
        {
            var ret = new StringBuilder();
            ret.Append(" (");

            CodeSetCollection localCodesSet =
                this.Mapping.Transcoding.TranscodingRules.GetLocalCodes(new CodeCollection(new[] { conditionValue }));
            DataSetColumnEntity column = this.Mapping.Columns[0];
            string mappedValue = EscapeString(conditionValue);

            // TODO check if columnIndex == 0 always
            int columnIndex = 0; // was this.Mapping.Transcoding.TranscodingRules.ColumnAsKeyPosition[column.Name];
            if (localCodesSet.Count > 0)
            {
                for (int i = 0; i < localCodesSet.Count; i++)
                {
                    Collection<string> localCodes = localCodesSet[i];
                    if (localCodes != null && localCodes.Count > 0)
                    {
                        mappedValue = localCodes[columnIndex];
                    }

                    if (i != 0)
                    {
                        ret.Append(" or ");
                    }

                    ret.Append("( " + SqlOperatorComponent(column.Name, mappedValue, operatorValue) + ")");
                    //ret.AppendFormat("( {0} " + operatorValue + " '{1}' )", column.Name, mappedValue);
                }
            }
            else
            {
                ret.Append(" " + SqlOperatorComponent(column.Name, mappedValue, operatorValue));
                //ret.AppendFormat(" {0} " + operatorValue + " '{1}' ", column.Name, mappedValue);
            }

            ret.Append(") ");
            return ret.ToString();
        }

        /// <summary>
        /// Maps the column of the mapping to the component of this ComponentMapping1to1T object
        /// and transcodes it. 
        /// </summary>
        /// <param name="reader">
        /// The DataReader for retrieving the values of the column.
        /// </param>
        /// <returns>
        /// The value of the component or null if no transcoding rule for the column values is found
        /// </returns>
        public string MapComponent(IDataReader reader)
        {
            var resultCodes = new CodeCollection();
            this.BuildOrdinals(reader);
            var column = this.ColumnOrdinals[0];
            string columnValue = DataReaderHelper.GetString(reader, column.Value);
            resultCodes.Add(columnValue);
            Collection<string> transcodedCodes = this.Mapping.Transcoding.TranscodingRules.GetDsdCodes(resultCodes);
            string ret = null;
            if (transcodedCodes != null && transcodedCodes.Count > 0)
            {
                ret = transcodedCodes[0];
            }

            return ret;
        }

        #endregion
    }
}
