// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlBuilderBase.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The base class for SQL Builders
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Data.Odbc;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Estat.Nsi.StructureRetriever.Model;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The base class for SQL Builders
    /// </summary>
    internal abstract class SqlBuilderBase : ISqlBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Generate the SQL for executing on the DDB
        /// </summary>
        /// <param name="info">
        /// The current structure retrieval information 
        /// </param>
        /// <returns>
        /// The generated SQL. 
        /// </returns>
        public abstract string GenerateSql(StructureRetrievalInfo info);

        #endregion

        #region Methods

        /// <summary>
        /// Returns string containing all column names used in the specified <paramref name="mappings"/> separated by comma.
        /// </summary>
        /// <param name="mappings">The mappings.</param>
        /// <returns>A string containing all column names used in the specified <paramref name="mappings"/> separated by comma.</returns>
        /// <exception cref="System.ArgumentException">No mapping provided.</exception>
        protected static string ToColumnNameString(params MappingEntity[] mappings)
        {
            if (mappings == null || mappings.Length == 0)
            {
                throw new ArgumentException("No mapping provided.");
            }

            ISet<string> columnNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            columnNames.UnionWith(from mapping in mappings from column in mapping.Columns select column.Name);
            return string.Join(", ", columnNames);
        }

        /// <summary>
        /// This method generates the WHERE part of the SQL query that will be used against the DDB for retrieving the available codes
        /// </summary>
        /// <param name="info">
        /// The current structure retrieval information 
        /// </param>
        /// <returns>
        /// A string containing the WHERE part of the SQL Query or an Empty string 
        /// </returns>
        protected static string GenerateWhere(StructureRetrievalInfo info)
        {
            var sb = new StringBuilder();
            int lastClause = 0;

            foreach (IKeyValues member in info.Criteria)
            {
                if (!string.IsNullOrEmpty(member.Id))
                {
                    if (member.Id.Equals(info.TimeDimension))
                    {
                        if (member.Values.Count > 0)
                        {
                            ISdmxDate startDate = new SdmxDateCore(member.Values[0]);
                            ISdmxDate endDate = null;
                            if (member.Values.Count > 1)
                            {
                                endDate = new SdmxDateCore(member.Values[1]);
                            }

                            sb.Append("(");
                            sb.Append(
                                info.TimeTranscoder.GenerateWhere(startDate, endDate, null));
                            sb.Append(")");
                            lastClause = sb.Length;
                            sb.Append(" AND ");
                        }
                    }
                    else
                    {
                        ComponentInfo compInfo;
                        if (info.ComponentMapping.TryGetValue(member.Id, out compInfo))
                        {
                            sb.Append("(");
                            foreach (string value in member.Values)
                            {
                                sb.Append(compInfo.ComponentMapping.GenerateComponentWhere(value));
                                lastClause = sb.Length;
                                sb.Append(" OR ");
                            }

                            sb.Length = lastClause;
                            if (lastClause > 0)
                            {
                                sb.Append(")");
                                lastClause = sb.Length;
                                sb.Append(" AND ");
                            }
                        }
                    }
                }
            }

            if (info.ReferencePeriod != null)
            {
                // TODO DEPRECIATED. We should not use it. We never did. But leaving it in case a 3rd party client uses it.
                IReferencePeriodMutableObject time = info.ReferencePeriod;

                sb.Append("(");
                sb.Append(info.TimeTranscoder.GenerateWhere(new SdmxDateCore(time.StartTime, TimeFormatEnumType.DateTime),
                                                            new SdmxDateCore(time.StartTime, TimeFormatEnumType.DateTime), null));
                sb.Append(")");
                lastClause = sb.Length;
            }

            sb.Length = lastClause;
            if (sb.Length > 0)
            {
                return " where " + sb;
            }

            return string.Empty;
        }

        #endregion
    }
}