// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArtefactEntity.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class represents an identifiable artefact. Such artefact have a triplet of id,
//   version, agency that uniquely identifies them
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    /// <summary>
    /// This class represents an identifiable artefact. Such artefact have a triplet of id,
    /// version, agency that uniquely identifies them
    /// </summary>
    public abstract class ArtefactEntity : PersistentEntityBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtefactEntity"/> class.
        /// </summary>
        /// <param name="sysId">
        /// The unique entity identifier
        /// </param>
        protected ArtefactEntity(long sysId)
            : base(sysId)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the artefact agency
        /// </summary>
        public string Agency { get; set; }

        /// <summary>
        /// Gets or sets the artefact identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the artefact version
        /// </summary>
        public string Version { get; set; }

        #endregion
    }
}