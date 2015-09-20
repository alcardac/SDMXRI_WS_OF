// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReportedAttribute.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Metadata
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     The ReportedAttribute interface.
    /// </summary>
    public interface IReportedAttributeObject : ISdmxObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the id.
        /// </summary>
        string Id { get; }

        /// <summary>
        ///    Gets the metadata text.
        /// </summary>
        /// <value> a list of structured texts for this component - will return an empty list if no Texts exist. </value>
        IList<ITextTypeWrapper> MetadataText { get; }

        /// <summary>
        ///     Gets a value indicating whether the getMetadataText returns an empty list
        /// </summary>
        /// <value> </value>
        bool Presentational { get; }

        /// <summary>
        ///     Gets the reported attributes.
        /// </summary>
        /// <value> child attributes </value>
        IList<IReportedAttributeObject> ReportedAttributes { get; }

        /// <summary>
        ///     Gets a simple value for this attribute, returns null if there is no simple value
        /// </summary>
        /// <value> </value>
        string SimpleValue { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets a value indicating whether the IReportedAttributeObject has a simple value, in which case getSimpleValue will return a not null value
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasSimpleValue();

        #endregion
    }
}