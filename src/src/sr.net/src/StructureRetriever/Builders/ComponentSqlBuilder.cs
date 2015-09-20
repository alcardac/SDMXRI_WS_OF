// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentSqlBuilder.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   SQL Builder for Components
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Builders
{
    using System.Globalization;
    using System.IO.IsolatedStorage;
    using System.Text;

    using Estat.Nsi.StructureRetriever.Model;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    /// <summary>
    /// SQL Builder for Components
    /// </summary>
    internal class ComponentSqlBuilder : SqlBuilderBase
    {
        #region Public Methods and Operators

        /// <summary>
        /// Generate the SQL for executing on the DDB
        /// </summary>
        /// <param name="info">
        /// The current structure retrieval information 
        /// </param>
        /// <returns>
        /// The generated sql. 
        /// </returns>
        public override string GenerateSql(StructureRetrievalInfo info)
        {
            MappingEntity[] mapping;

            if (info.RequestedComponentInfo != null)
            {
                mapping = new[] { info.RequestedComponentInfo.Mapping };
            }
            else if (info.RequestedComponent.Equals(info.TimeDimension))
            {
                mapping = info.FrequencyInfo != null ? new[] { info.TimeMapping, info.FrequencyInfo.Mapping } : new[] { info.TimeMapping };
            }
            else
            {
                return null;
            }

            var columnList = ToColumnNameString(mapping);

            return string.Format(
                CultureInfo.InvariantCulture,
                "SELECT DISTINCT {2} \n FROM ({0}) virtualDataset {1} \n ORDER BY {2} ",
                info.InnerSqlQuery,
                GenerateWhere(info),
                columnList);
        }

        #endregion
    }
}