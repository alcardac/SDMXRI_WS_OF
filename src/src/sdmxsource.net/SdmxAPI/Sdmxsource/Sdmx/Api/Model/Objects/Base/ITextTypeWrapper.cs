// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextTypeWrapper.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Base
{
    /// <summary>
    ///     Contains a locale alongside the string value, the value may be in HTML
    /// </summary>
    public interface ITextTypeWrapper : ISdmxObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether the HTML is text.
        /// </summary>
        /// <value> </value>
        bool Html { get; }

        /// <summary>
        ///     Gets the locale of the text string, retrievable by getValue()
        /// </summary>
        /// <value> </value>
        string Locale { get; }

        /// <summary>
        ///     Gets the text, in the locale given by getLocale()
        /// </summary>
        /// <value> </value>
        string Value { get; }

        #endregion
    }
}