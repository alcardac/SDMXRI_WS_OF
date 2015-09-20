// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupEntity.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The class models a group
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// The class models a group
    /// </summary>
    public class GroupEntity : PersistentEntityBase
    {
        #region Constants and Fields

        /// <summary>
        /// The list of group dimensions
        /// </summary>
        private readonly Collection<ComponentEntity> _dimensions;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupEntity"/> class. 
        /// Standard constructor used to initialize the dimensions internal list
        /// </summary>
        /// <param name="sysId">
        /// The sys Id.
        /// </param>
        public GroupEntity(long sysId)
            : base(sysId)
        {
            this._dimensions = new Collection<ComponentEntity>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the list of Dimensions
        /// </summary>
        public Collection<ComponentEntity> Dimensions
        {
            get
            {
                return this._dimensions;
            }
        }

        /// <summary>
        /// Gets or sets the Id of the Group
        /// </summary>
        public string Id { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a dimension to the Dimensions list
        /// </summary>
        /// <param name="dimension">
        /// &gt;The <see cref="ComponentEntity"/> 
        /// that needs to be added
        /// </param>
        public void AddDimensions(ComponentEntity dimension)
        {
            this._dimensions.Add(dimension);
        }

        #endregion
    }
}