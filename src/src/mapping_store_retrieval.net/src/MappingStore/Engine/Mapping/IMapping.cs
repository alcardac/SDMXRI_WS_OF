// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMapping.cs" company="Eurostat">
//   Date Created : 2011-11-28
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   General mapping interface
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Engine.Mapping
{
    using System.Data;

    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    /// <summary>
    /// General mapping interface
    /// </summary>
    public interface IMapping
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the DSD Component.
        /// </summary>
        ComponentEntity Component { get; set; }

        /// <summary>
        /// Gets or sets the mapping
        /// </summary>
        MappingEntity Mapping { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Maps the column(s) of the mapping to the component(s) of this IComponentMapping
        /// </summary>
        /// <param name="reader">
        /// The DataReader for retrieving the values of the column.
        /// </param>
        /// <returns>
        /// The value of the component
        /// </returns>
        string MapComponent(IDataReader reader);

        #endregion
    }
}