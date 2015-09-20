// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentMapping1N.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Handles the mapping between 1 component and N columns where N &gt; 1
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#define MAT200

namespace Estat.Sri.MappingStoreRetrieval.Engine.Mapping
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    /// <summary>
    /// Handles the mapping between 1 component and N columns where N &gt; 1
    /// </summary>
    internal class ComponentMapping1N : ComponentMapping, IComponentMapping
    {
        #region Constants and Fields
        #endregion

        #region Public Methods

        /// <summary>
        /// Generates the SQL Where clause for the component used in this mapping
        /// and the condition value from SDMX Query which is transcoded
        /// </summary>
        /// <param name="conditionValue">
        /// string with the conditional value from the SDMX query
        /// </param>
        /// <param name="operatorValue">
        /// string with the operator value from the SDMX query, "=" by default
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
            if (localCodesSet.Count > 0)
            {
                for (int i = 0; i < localCodesSet.Count; i++)
                {
                    if (i != 0)
                    {
                        ret.Append(" OR ");
                    }

                    ret.Append(" (");
                    Collection<string> localCodes = localCodesSet[i];

                    // component to columns
                    var mappedClause = new List<string>();
                    foreach (DataSetColumnEntity column in this.Mapping.Columns)
                    {
                        string mappedId = column.Name;
                        string mappedValue = EscapeString(conditionValue);
                        int columnPosition = this.Mapping.Transcoding.TranscodingRules.ColumnAsKeyPosition[column.SysId];
                        if (localCodes != null && columnPosition < localCodes.Count)
                        {
                            mappedValue = localCodes[columnPosition];
                        }

                        mappedClause.Add(SqlOperatorComponent(mappedId, mappedValue, operatorValue));
                        //mappedClause.Add(
                        //    string.Format(CultureInfo.InvariantCulture, "{0} " + operatorValue + " '{1}' ", mappedId, mappedValue));
                    }

                    ret.Append(string.Join(" AND ", mappedClause.ToArray()));
                    ret.Append(" ) ");
                }
            }
            else
            {
                var mappedClause = new List<string>();
                foreach (DataSetColumnEntity column in this.Mapping.Columns)
                {
                    string mappedId = column.Name;
                    string mappedValue = EscapeString(conditionValue);
                    mappedClause.Add(SqlOperatorComponent(mappedId, mappedValue, operatorValue));
                    //mappedClause.Add(string.Format(CultureInfo.InvariantCulture, "{0} " + operatorValue + " '{1}' ", mappedId, mappedValue));
                }

                ret.Append(string.Join(" AND ", mappedClause.ToArray()));
            }

            ret.Append(" )");
            return ret.ToString();
        }

        /// <summary>
        /// Maps the columns of the mapping to the component of this ComponentMapping1N object
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
            var resultCodes = new string[this.Mapping.Columns.Count];
            this.BuildOrdinals(reader);

            foreach (var column in this.ColumnOrdinals)
            {
                resultCodes[column.ColumnPosition] =
                    DataReaderHelper.GetString(reader, column.Value);
            }

            Collection<string> transcodedCodes =
                this.Mapping.Transcoding.TranscodingRules.GetDsdCodes(new CodeCollection(resultCodes));
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