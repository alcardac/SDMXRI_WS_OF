// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatasetEntity.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This is a class that represents a dataset
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    /// <summary>
    /// This is a class that represents a dataset
    /// </summary>
    public class DataSetEntity : PersistentEntityBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSetEntity"/> class.
        /// </summary>
        /// <param name="sysId">
        /// The unique entity identifier
        /// </param>
        public DataSetEntity(long sysId)
            : base(sysId)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the dataset connection
        /// </summary>
        public ConnectionEntity Connection { get; set; }

        /// <summary>
        /// Gets or sets the dataset description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the dataset name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the dataset query
        /// </summary>
        public string Query { get; set; }

        #endregion
    }
}