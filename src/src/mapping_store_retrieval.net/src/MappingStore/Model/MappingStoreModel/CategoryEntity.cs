// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryEntity.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class represents a Mapping Store Category.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// This class represents a Mapping Store Category.
    /// </summary>
    public class CategoryEntity : ItemEntity
    {
        #region Constants and Fields

        /// <summary>
        /// The _categories.
        /// </summary>
        private readonly Collection<CategoryEntity> _categories;

        /// <summary>
        /// The _dataflows.
        /// </summary>
        private readonly Collection<DataflowEntity> _dataflows;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryEntity"/> class. 
        /// Mapping Store SDMX CategoryEntity constructor.
        /// This constructor initialises the child categories and Dataflows collection
        /// </summary>
        /// <param name="sysId">
        /// The sys Id.
        /// </param>
        public CategoryEntity(long sysId)
            : base(sysId)
        {
            this._categories = new Collection<CategoryEntity>();
            this._dataflows = new Collection<DataflowEntity>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the list of child categories
        /// </summary>
        public Collection<CategoryEntity> Categories
        {
            get
            {
                return this._categories;
            }
        }

        /// <summary>
        /// Gets the list of linked dataflows
        /// </summary>
        public Collection<DataflowEntity> Dataflows
        {
            get
            {
                return this._dataflows;
            }
        }

        #endregion
    }
}