// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MappingSetEntity.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This value object represents a mappingset
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// This value object represents a mappingset
    /// </summary>
    public class MappingSetEntity : PersistentEntityBase
    {
        #region Constants and Fields

        /// <summary>
        /// The _mappings.
        /// </summary>
        private readonly Collection<MappingEntity> _mappings;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingSetEntity"/> class. 
        /// Default constructor used to initialize the 
        /// <see cref="Mappings"/>
        /// </summary>
        /// <param name="sysId">
        /// The sys Id.
        /// </param>
        public MappingSetEntity(long sysId)
            : base(sysId)
        {
            this._mappings = new Collection<MappingEntity>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the mappingset dataset
        /// </summary>
        public DataSetEntity DataSet { get; set; }

        /// <summary>
        /// Gets or sets the mappingset dataflow
        /// </summary>
        public DataflowEntity Dataflow { get; set; }

        /// <summary>
        /// Gets or sets the mappingset description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the mappingset identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets the mappingset list of mappings
        /// </summary>
        public Collection<MappingEntity> Mappings
        {
            get
            {
                return this._mappings;
            }
        }

        #endregion
    }
}