// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnnotableObjectBase.cs" company="Eurostat">
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

    #endregion

    /// <summary>
    ///     An Annotable Object is one which can hold annotations against it.
    /// </summary>
    public interface IAnnotableObjectBase : IObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets a list of annotations that exist for this Annotable Object
        /// </summary>
        /// <value> </value>
        ISet<IAnnotationObjectBase> Annotations { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets an annotations with the given title, this returns null if no annotation exists with
        ///     the given type
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <returns>
        /// The annotations with the given title, this returns null if no annotation exists with
        ///     the given type
        /// </returns>
        ISet<IAnnotationObjectBase> GetAnnotationByTitle(string title);

        /// <summary>
        /// Gets an annotation with the given type, this returns null if no annotation exists with
        ///     the given type
        /// </summary>
        /// <param name="type">The type. </param>
        /// <returns>
        /// The <see cref="IAnnotationObjectBase"/> .
        /// </returns>
        IAnnotationObjectBase GetAnnotationByType(string type);

        /// <summary>
        /// Gets an annotations with the given url, this returns null if no annotation exists with
        ///     the given type
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The annotations with the given url, this returns null if no annotation exists with
        ///     the given type
        /// </returns>
        ISet<IAnnotationObjectBase> GetAnnotationByUrl(Uri url);

        /// <summary>
        ///     Gets a value indicating whether the annotations exist for this Annotable Object
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasAnnotations();

        #endregion
    }
}