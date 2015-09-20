// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorySchemeEntity.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class represents a Mapping Store CategoryScheme.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// This class represents a Mapping Store CategoryScheme.
    /// </summary>
    public class CategorySchemeEntity : ArtefactEntity
    {
        #region Constants and Fields

        /// <summary>
        /// List of categories that belong to this category scheme
        /// </summary>
        private readonly Collection<CategoryEntity> _categories;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySchemeEntity"/> class.
        /// </summary>
        /// <param name="sysId">
        /// The unique entity identifier
        /// </param>
        public CategorySchemeEntity(long sysId)
            : base(sysId)
        {
            this._categories = new Collection<CategoryEntity>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the list of categories that belong to this category scheme
        /// </summary>
        public Collection<CategoryEntity> Categories
        {
            get
            {
                return this._categories;
            }
        }

        #endregion
    }
}