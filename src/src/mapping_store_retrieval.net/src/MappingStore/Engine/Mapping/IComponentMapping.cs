// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComponentMapping.cs" company="Eurostat">
//   Date Created : 2011-10-21
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   A common interface for all component mapping classes
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Engine.Mapping
{
    /// <summary>
    /// A common interface for all component mapping classes
    /// </summary>
    public interface IComponentMapping : IMapping
    {
        #region Public Methods

        /// <summary>
        /// Generates the SQL Where clause for the component used in this mapping
        /// and the condition value from SDMX Query
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
        string GenerateComponentWhere(string conditionValue, string operatorValue = "=");

        #endregion
    }
}