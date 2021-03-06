﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeListEntity.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class represents an SDMX codelist
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// This class represents an SDMX codelist
    /// </summary>
    public class CodeListEntity : ArtefactEntity
    {
        #region Constants and Fields

        /// <summary>
        /// The _code list.
        /// </summary>
        private readonly Collection<string> _codeList;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeListEntity"/> class.
        /// </summary>
        /// <param name="sysId">
        /// The unique entity identifier
        /// </param>
        public CodeListEntity(long sysId)
            : base(sysId)
        {
            this._codeList = new Collection<string>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the list of codes
        /// </summary>
        public Collection<string> CodeList
        {
            get
            {
                return this._codeList;
            }
        }

        #endregion
    }
}