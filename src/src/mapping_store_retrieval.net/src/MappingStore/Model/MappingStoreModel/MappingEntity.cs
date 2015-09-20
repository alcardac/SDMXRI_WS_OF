// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MappingEntity.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class refers to a Mapping Set. It is in fact a value object to
//   contain the values retrieved by the MA store.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// This class refers to a Mapping Set. It is in fact a value object to 
    /// contain the values retrieved by the MA store.
    /// </summary>
    public class MappingEntity : PersistentEntityBase
    {
        #region Constants and Fields

        /// <summary>
        /// The list of local columns
        /// </summary>
        private readonly Collection<DataSetColumnEntity> _columns;

        /// <summary>
        /// The list of DSD components
        /// </summary>
        private readonly Collection<ComponentEntity> _components;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingEntity"/> class. 
        /// Default constructor used to initialize 
        /// <see cref="Components"/>
        /// and <see cref="Columns"/>
        /// </summary>
        /// <param name="sysId">
        /// The sys Id.
        /// </param>
        public MappingEntity(long sysId)
            : base(sysId)
        {
            this._components = new Collection<ComponentEntity>();
            this._columns = new Collection<DataSetColumnEntity>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the mapping columns
        /// </summary>
        public Collection<DataSetColumnEntity> Columns
        {
            get
            {
                return this._columns;
            }
        }

        /// <summary>
        /// Gets the mapping components
        /// </summary>
        public Collection<ComponentEntity> Components
        {
            get
            {
                return this._components;
            }
        }

        /// <summary>
        /// Gets or sets the mapping constant
        /// </summary>
        public string Constant { get; set; }

        /// <summary>
        /// Gets or sets the mapping type
        /// </summary>
        public string MappingType { get; set; }

        /// <summary>
        /// Gets or sets the mapping transcoding
        /// </summary>
        public TranscodingEntity Transcoding { get; set; }

        #endregion
    }
}