// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnnotationMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     The AnnotationMutableObject interface.
    /// </summary>
    public interface IAnnotationMutableObject : IMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        ///     Gets the text.
        /// </summary>
        IList<ITextTypeWrapperMutableObject> Text { get; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        string Type { get; set; }

        /// <summary>
        ///     Gets or sets the url.
        /// </summary>
         Uri Uri { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add text.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        void AddText(ITextTypeWrapperMutableObject text);

        /// <summary>
        /// The add text.
        /// </summary>
        /// <param name="locale">
        /// The locale.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        void AddText(string locale, string text);

        #endregion
    }
}