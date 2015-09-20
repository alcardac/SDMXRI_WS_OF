// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableInfo.cs" company="Eurostat">
//   Date Created : 2013-02-09
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The table info.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Model
{
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The table info.
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        /// The _structure type
        /// </summary>
        private readonly SdmxStructureEnumType _structureType;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableInfo"/> class.
        /// </summary>
        /// <param name="structureType">Type of the structure.</param>
        public TableInfo(SdmxStructureEnumType structureType)
        {
            this._structureType = structureType;
        }

        #region Public Properties

        /// <summary>
        /// Gets or sets the primary key.
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// Gets or sets the extra fields to include in the SELECT statement (should start with a comma)
        /// </summary>
        public string ExtraFields { get; set; }

        /// <summary>
        /// Gets the _structure type
        /// </summary>
        public SdmxStructureEnumType StructureType
        {
            get
            {
                return this._structureType;
            }
        }

        #endregion
    }
}