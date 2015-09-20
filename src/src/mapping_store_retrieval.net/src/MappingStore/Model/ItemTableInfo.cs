// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemTableInfo.cs" company="Eurostat">
//   Date Created : 2013-02-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The item table info.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Model
{
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The item table info.
    /// </summary>
    public class ItemTableInfo : TableInfo
    {
        #region Public Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemTableInfo"/> class.
        /// </summary>
        /// <param name="structureType">Type of the structure.</param>
        public ItemTableInfo(SdmxStructureEnumType structureType)
            : base(structureType)
        {
        }

        /// <summary>
        ///     Gets or sets the foreign key.
        /// </summary>
        public string ForeignKey { get; set; }

        /// <summary>
        ///     Gets or sets the parent item. If any.
        /// </summary>
        public string ParentItem { get; set; }

        #endregion
    }
}