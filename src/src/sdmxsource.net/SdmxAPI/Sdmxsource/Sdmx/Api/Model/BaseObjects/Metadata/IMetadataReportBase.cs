// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMetadataReportBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Metadata
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Metadata;

    #endregion

    /// <summary>
    ///     The MetadataReportBase interface.
    /// </summary>
    public interface IMetadataReportBase : IObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the IMetadataReport that was used to build this base object - verride from parent
        /// </summary>
        new IMetadataReport BuiltFrom { get; }

        /// <summary>
        ///     Gets the id.
        /// </summary>
        string Id { get; }

        /// <summary>
        ///     Gets the reported attributes.
        /// </summary>
        IList<IReportedAttributeBase> ReportedAttributes { get; }

        /// <summary>
        ///     Gets the target.
        /// </summary>
        ITargetBase Target { get; }

        #endregion
    }
}