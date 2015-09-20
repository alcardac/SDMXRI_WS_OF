// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRelatedStructures.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     Represents an SDMX Related Structures Map
    /// </summary>
    public interface IRelatedStructures : ISdmxStructure
    {
        #region Public Properties

        /// <summary>
        ///     Gets the category scheme ref.
        /// </summary>
        IList<ICrossReference> CategorySchemeRef { get; }

        /// <summary>
        ///     Gets the concept scheme ref.
        /// </summary>
        IList<ICrossReference> ConceptSchemeRef { get; }

        /// <summary>
        ///     Gets the data structure ref.
        /// </summary>
        IList<ICrossReference> DataStructureRef { get; }

        /// <summary>
        ///     Gets the hier codelist ref.
        /// </summary>
        IList<ICrossReference> HierCodelistRef { get; }

        /// <summary>
        ///     Gets the metadata structure ref.
        /// </summary>
        IList<ICrossReference> MetadataStructureRef { get; }

        /// <summary>
        ///     Gets the org scheme ref.
        /// </summary>
        IList<ICrossReference> OrgSchemeRef { get; }

        #endregion
    }
}