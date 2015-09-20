// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReportedAttributeBase.cs" company="Eurostat">
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
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;

    #endregion

    /// <summary>
    ///     The ReportedAttributeBase interface.
    /// </summary>
    public interface IReportedAttributeBase : IObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the IReferenceValue that was used to build this Base object - Override from parent
        /// </summary>
        new IReportedAttributeObject BuiltFrom { get; }

        /// <summary>
        ///     Gets the codelist.
        /// </summary>
        ICodelistObject Codelist { get; }

        /// <summary>
        ///     Gets the concept.
        /// </summary>
        IConceptObject Concept { get; }

        /// <summary>
        ///    Gets a value indicating whether this is the coded representation.
        /// </summary>
        bool HasCodedRepresentation { get; }

        /// <summary>
        ///     Gets a value indicating whether the IReportedAttributeObject has a simple value, in which case getSimpleValue will return a not null value
        /// </summary>
        /// <value> The &lt; see cref= &quot; bool &quot; / &gt; . </value>
        bool HasSimpleValue { get; }

        /// <summary>
        ///     Gets the id.
        /// </summary>
        string Id { get; }

        /// <summary>
        ///     Gets the metadata text.
        /// </summary>
        /// <returns>a list of structured texts for this component - will return an empty list if no Texts exist.</returns>
        IList<ITextTypeWrapper> MetadataText { get; }

        /// <summary>
        ///     Gets a value indicating whether the getMetadataText returns an empty list
        /// </summary>
        /// <value> </value>
        bool Presentational { get; }

        /// <summary>
        ///     Gets the reported attributes.
        /// </summary>
        /// <returns>child attributes</returns>
        IList<IReportedAttributeBase> ReportedAttributes { get; }

        /// <summary>
        ///     Gets a simple value for this attribute, returns null if there is no simple value
        /// </summary>
        /// <value> </value>
        string SimpleValue { get; }

        #endregion
    }
}