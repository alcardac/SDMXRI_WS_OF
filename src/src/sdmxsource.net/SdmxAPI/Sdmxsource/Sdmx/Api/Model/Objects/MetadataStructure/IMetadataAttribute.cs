// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMetadataAttribute.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     A IMetadataAttributeObject is used in a <c>IReportStructure</c> and is similar to a <c>IDimension</c>
    ///     in that it contains reference to a semantic (<c>IConceptObject</c>) and representation (codelists/free text)
    /// </summary>
    public interface IMetadataAttributeObject : IComponent
    {
        #region Public Properties

        /// <summary>
        ///     Gets the max occurs.
        /// </summary>
        int? MaxOccurs { get; }

        /// <summary>
        ///     Gets any child metadata attributes
        ///     <p />
        ///     <b>NOTE</b>The list is a copy so modifying the returned list will not
        ///     be reflected in the IMetadataAttributeObject instance
        /// </summary>
        IList<IMetadataAttributeObject> MetadataAttributes { get; }

        /// <summary>
        ///     Gets the min occurs.
        /// </summary>
        int? MinOccurs { get; }

        /// <summary>
        ///     Gets the presentational.
        /// </summary>
        TertiaryBool Presentational { get; }

        #endregion
    }
}