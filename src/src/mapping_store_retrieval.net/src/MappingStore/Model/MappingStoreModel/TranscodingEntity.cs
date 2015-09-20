// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TranscodingEntity.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class represents a Transcoding rule
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    /// <summary>
    /// This class represents a Transcoding rule
    /// </summary>
    public class TranscodingEntity : PersistentEntityBase
    {
        #region Constants and Fields

        /// <summary>
        /// Gets the time transcoding collection.
        /// </summary>
        private readonly TimeTranscodingCollection _timeTranscodingCollection;

        /// <summary>
        /// The transcoding dictionaries
        /// </summary>
        private TranscodingRulesEntity _transcodingRules = new TranscodingRulesEntity();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TranscodingEntity"/> class.
        /// </summary>
        /// <param name="sysId">
        /// The sys Id.
        /// </param>
        public TranscodingEntity(long sysId)
            : base(sysId)
        {
            this._timeTranscodingCollection = new TimeTranscodingCollection(sysId);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the expression to evaluate in mapping time for transcoding
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets the transcoding dictionaries
        /// </summary>
        public TranscodingRulesEntity TranscodingRules
        {
            get
            {
                return this._transcodingRules;
            }

            set
            {
                this._transcodingRules = value;
            }
        }

        /// <summary>
        /// Gets the time transcoding collection.
        /// </summary>
        public TimeTranscodingCollection TimeTranscodingCollection
        {
            get
            {
                return this._timeTranscodingCollection;
            }
        }

        #endregion
    }
}