// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RetrievalSettings.cs" company="Eurostat">
//   Date Created : 2013-02-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The retrieval settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Model
{
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The retrieval settings.
    /// </summary>
    public class RetrievalSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RetrievalSettings"/> class. 
        /// </summary>
        public RetrievalSettings()
            : this(StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.Full))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RetrievalSettings"/> class. 
        /// </summary>
        /// <param name="queryDetail">
        /// The query Detail. 
        /// </param>
        public RetrievalSettings(StructureQueryDetail queryDetail)
        {
            this.QueryDetail = queryDetail;
        }

        #region Public Properties

        /// <summary>
        /// Gets the <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </summary>
        public StructureQueryDetail QueryDetail { get; private set; }

        #endregion
    }
}