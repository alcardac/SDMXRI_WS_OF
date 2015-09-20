// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReportStructure.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     The ReportStructure interface.
    /// </summary>
    public interface IReportStructure : IIdentifiableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the metadata attributes.
        /// </summary>
        IList<IMetadataAttributeObject> MetadataAttributes { get; }

        /// <summary>
        ///     Gets the target metadata list.
        /// </summary>
        IList<string> TargetMetadatas { get; }

        #endregion
    }
}