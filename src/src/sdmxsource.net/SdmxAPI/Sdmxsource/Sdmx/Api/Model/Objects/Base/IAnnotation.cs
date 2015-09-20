// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnnotation.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Base
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     Contains annotation information
    ///     <p />
    ///     Provides for non-documentation notes and annotations to be embedded in data and structure messages.
    ///     It provides optional fields for providing a title, a type description, a URI, and the text of the annotation.
    ///     <p />
    ///     This is an immutable Object - this Object can not be modified
    /// </summary>
    public interface IAnnotation : ISdmxObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the title of the annotation, this is a free text field
        /// </summary>
        /// <value> </value>
        string Id { get; }

        /// <summary>
        ///     Gets a language-specific string which holds the text of the annotation
        ///     <p />
        ///     <b>NOTE</b>The list is a copy so modify the returned list will not
        ///     be reflected in the AnnotableObject instance
        /// </summary>
        /// <value> </value>
        IList<ITextTypeWrapper> Text { get; }

        /// <summary>
        ///     Gets the title of the annotation, this is a free text field
        /// </summary>
        /// <value> </value>
        string Title { get; }

        /// <summary>
        ///     Gets the type of annotation, this is a free text field
        ///     <p />
        ///     Is used to distinguish between annotations designed to support various uses.
        ///     The types are not enumerated, as these can be specified by the user or creator of the annotations.
        ///     The definitions and use of annotation types should be documented by their creator
        /// </summary>
        /// <value> </value>
        string Type { get; }

        /// <summary>
        ///     Gets the URI - typically a Uri - which points to an external resource which may contain or supplement the annotation.
        ///     <p />
        ///     If a specific behaviour is desired, an annotation type should be defined which specifies the use of this field more exactly.
        /// </summary>
        /// <value> </value>
        Uri Uri { get; }

        #endregion
    }
}