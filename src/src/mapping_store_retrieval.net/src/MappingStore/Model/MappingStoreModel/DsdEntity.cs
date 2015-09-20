// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DsdEntity.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This is the entity class representing a DSD
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// This is the entity class representing a DSD
    /// </summary>
    public class DsdEntity : ArtefactEntity
    {
        #region Constants and Fields

        /// <summary>
        /// The list of data structure definition attributes
        /// </summary>
        private readonly Collection<ComponentEntity> _attributes = new Collection<ComponentEntity>();

        /// <summary>
        /// The list of data structure definition crossSectionalMeasures
        /// </summary>
        private readonly Collection<ComponentEntity> _crossSectionalMeasures = new Collection<ComponentEntity>();

        /// <summary>
        /// The list of data structure definition dimensions
        /// </summary>
        private readonly Collection<ComponentEntity> _dimensions = new Collection<ComponentEntity>();

        /// <summary>
        /// The list of data structure definition groups
        /// </summary>
        private readonly Collection<GroupEntity> _groups = new Collection<GroupEntity>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DsdEntity"/> class.
        /// </summary>
        /// <param name="sysId">
        /// The unique entity identifier
        /// </param>
        public DsdEntity(long sysId)
            : base(sysId)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the list of data structure definition attributes
        /// </summary>
        public Collection<ComponentEntity> Attributes
        {
            get
            {
                return this._attributes;
            }
        }

        /// <summary>
        /// Gets the list of data structure definition cross sectional measures
        /// </summary>
        public Collection<ComponentEntity> CrossSectionalMeasures
        {
            // MAT-130
            get
            {
                return this._crossSectionalMeasures;
            }
        }

        /// <summary>
        /// Gets the list of data structure definition dimensions
        /// </summary>
        public Collection<ComponentEntity> Dimensions
        {
            get
            {
                return this._dimensions;
            }
        }

        /// <summary>
        /// Gets the list of data structure definition groups
        /// </summary>
        public Collection<GroupEntity> Groups
        {
            get
            {
                return this._groups;
            }
        }

        /// <summary>
        /// Gets or sets the data structure definition primary measure
        /// </summary>
        public ComponentEntity PrimaryMeasure { get; set; }

        /// <summary>
        /// Gets or sets the data structure definition time dimension
        /// </summary>
        public ComponentEntity TimeDimension { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The method is used to add a <see cref="GroupEntity"/> 
        /// to the data structure definition list of groups
        /// </summary>
        /// <param name="group">
        /// The <see cref="GroupEntity"/> 
        /// that needs to be added
        /// </param>
        public void AddGroup(GroupEntity group)
        {
            this._groups.Add(group);
        }

        #endregion
    }
}