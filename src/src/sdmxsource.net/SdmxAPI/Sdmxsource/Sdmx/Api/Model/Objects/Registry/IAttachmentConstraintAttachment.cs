// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAttachmentConstraintAttachment.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     The AttachmentConstraintAttachment interface.
    /// </summary>
    public interface IAttachmentConstraintAttachment : ISdmxStructure
    {
        #region Public Properties

        /// <summary>
        ///   Gets the id of that set if the target structure is a dataset or metadata set
        /// </summary>
        /// <value> </value>
        IList<IDataAndMetadataSetReference> DataOrMetadataSetReference { get; }

        /// <summary>
        ///     Gets the datasources.
        /// </summary>
        IList<IDataSource> Datasources { get; }

        /// <summary>
        ///     Gets the structure references that this contstraint is referencing
        /// </summary>
        /// <value> </value>
        IList<ICrossReference> StructureReferences { get; }

        /// <summary>
        ///     Gets the target structure for this constraint attachment
        /// </summary>
        /// <value> </value>
        SdmxStructureType TargetStructureType { get; }

        #endregion
    }
}