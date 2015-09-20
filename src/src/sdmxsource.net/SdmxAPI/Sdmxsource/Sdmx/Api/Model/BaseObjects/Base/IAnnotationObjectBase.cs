// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnnotationObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base
{
    #region Using directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;

    #endregion

    /// <summary>
    ///     An Annotation is a piece of information that can be held against an Annotable Object.
    ///     <p />
    ///     It can be thought of as a placeholder for any extraneous information that needs to be stored, where there is no
    ///     other appropriate place to store this.
    /// </summary>
    public interface IAnnotationObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the text of the annotation in the default locale
        /// </summary>
        /// <value> </value>
        string Text { get; }

        /// <summary>
        ///     Gets the text of the annotation in the default locale
        /// </summary>
        /// <value> </value>
        IDictionary<CultureInfo, string> Texts { get; }

        /// <summary>
        ///     Gets the title of the annotation
        /// </summary>
        /// <value> </value>
        string Title { get; }

        /// <summary>
        ///     Gets the type of the annotation
        /// </summary>
        /// <value> </value>
        string Type { get; }

        /// <summary>
        ///     Gets the Uri of the annotation
        /// </summary>
        /// <value> </value>
        Uri Url { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the text of the annotation in the given locale
        /// </summary>
        /// <param name="locale">The locale
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        string GetText(CultureInfo locale);

        #endregion
    }
}