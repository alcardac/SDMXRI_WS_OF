// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataFlowEntity.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class represents a dataflow
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    /// <summary>
    /// This class represents a dataflow
    /// </summary>
    public class DataflowEntity : ArtefactEntity
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataflowEntity"/> class.
        /// </summary>
        /// <param name="sysId">
        /// The unique entity identifier
        /// </param>
        public DataflowEntity(long sysId)
            : base(sysId)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the data structure definition
        /// </summary>
        public DsdEntity Dsd { get; set; }

        /// <summary>
        /// Gets or sets the mapping set entity
        /// </summary>
        public MappingSetEntity MappingSet { get; set; }

        #endregion
    }
}