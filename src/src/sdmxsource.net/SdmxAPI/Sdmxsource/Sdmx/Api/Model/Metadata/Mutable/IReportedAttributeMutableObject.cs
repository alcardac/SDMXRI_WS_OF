// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReportedAttributeMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Metadata.Mutable
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     The ReportedAttributeMutableObject interface.
    /// </summary>
    public interface IReportedAttributeMutableObject : IIdentifiableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the reported attributes.
        /// </summary>
        /// <value> child attributes </value>
        IList<IReportedAttributeObject> ReportedAttributes { get; }

        /// <summary>
        ///    Gets the structured Text is XHTML
        /// </summary>
        /// <value> the structured text in the default locale </value>
        string StructuredText { get; }

        /// <summary>
        ///     Gets the structured Text is XHTML
        /// </summary>
        /// <value> a list of structured texts for this component - will return an empty list if no Texts exist. </value>
        IList<ITextTypeWrapper> StructuredTexts { get; }

        /// <summary>
        ///     Gets the text.
        /// </summary>
        /// <value> the text in the default locale </value>
        string Text { get; }

        /// <summary>
        ///    Gets the texts.
        /// </summary>
        /// <value> a list of texts for this component - will return an empty list if no Texts exist. </value>
        IList<ITextTypeWrapper> Texts { get; }

        #endregion

        // /**
        // * @return a list of texts for this component - will return an empty list if no Texts exist.
        // */
        // void setTexts(List<ITextTypeWrapper> texts);
        // /**
        // * @return the text in the default locale
        // */
        // string getText();
        // /**
        // * Structured Text is XHTML
        // * @return a list of structured texts for this component - will return an empty list if no Texts exist.
        // */
        // List<ITextTypeWrapper> getStructuredTexts();
        // /**
        // * Structured Text is XHTML
        // * @return the structured text in the default locale
        // */
        // string getStructuredText();
        // /**
        // * @return child attributes
        // */
        // List<IReportedAttributeObject> getReportedAttributes();
    }
}