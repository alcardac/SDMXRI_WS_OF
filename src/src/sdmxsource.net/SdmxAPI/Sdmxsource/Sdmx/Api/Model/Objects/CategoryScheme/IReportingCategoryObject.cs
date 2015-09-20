// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReportingCategoryObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     A Reporting Category is used to group structures (dataflows or metadataflows) into useful sub-packages.
    /// </summary>
    public interface IReportingCategoryObject : IHierarchicalItemObject<IReportingCategoryObject>
    {
        #region Public Properties

        /// <summary>
        ///     Gets the provisioning metadata.
        /// </summary>
        IList<ICrossReference> ProvisioningMetadata { get; }

        /// <summary>
        ///     Gets the structural metadata.
        /// </summary>
        IList<ICrossReference> StructuralMetadata { get; }

        #endregion
    }
}