// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatasetColumnEntity.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This is a value object representing a dataset column
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    /// <summary>
    /// This is a value object representing a dataset column
    /// </summary>
    public class DataSetColumnEntity : PersistentEntityBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSetColumnEntity"/> class.
        /// </summary>
        /// <param name="sysId">
        /// The unique entity identifier
        /// </param>
        public DataSetColumnEntity(long sysId)
            : base(sysId)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the dataset column description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the dataset column name
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}