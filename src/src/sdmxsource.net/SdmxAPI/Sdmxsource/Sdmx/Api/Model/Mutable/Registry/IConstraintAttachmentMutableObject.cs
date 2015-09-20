// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConstraintAttachmentMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     The ConstraintAttachmentMutableObject interface.
    /// </summary>
    public interface IConstraintAttachmentMutableObject : IMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the data or metadata set reference.
        /// </summary>
        IDataAndMetadataSetMutableReference DataOrMetadataSetReference { get; set; }

        /// <summary>
        ///     Gets the data sources.
        /// </summary>
        IList<IDataSourceMutableObject> DataSources { get; }

        /// <summary>
        ///     Gets the structure reference.
        /// </summary>
        ISet<IStructureReference> StructureReference { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add data sources.
        /// </summary>
        /// <param name="dataSourceMutableObject">
        /// The dataSourceMutableObject.
        /// </param>
        void AddDataSources(IDataSourceMutableObject dataSourceMutableObject);

        /// <summary>
        /// The add structure reference.
        /// </summary>
        /// <param name="structureReference">
        /// The dataSourceMutableObject.
        /// </param>
        void AddStructureReference(IStructureReference structureReference);

        #endregion
    }
}