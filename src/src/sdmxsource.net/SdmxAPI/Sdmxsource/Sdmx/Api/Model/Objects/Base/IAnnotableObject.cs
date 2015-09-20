// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnnotableObject.cs" company="Eurostat">
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

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     An AnnotableObject Object is one which can contain annotations
    ///     <p />
    ///     This is an immutable @object - this @object can not be modified
    /// </summary>
    public interface IAnnotableObject : ISdmxStructure
    {
        #region Public Properties

        /// <summary>
        ///     Gets the list of annotations
        ///     <p />
        ///     <b>NOTE</b>The list is a copy so modify the returned list will not
        ///     be reflected in the AnnotableObject instance
        /// </summary>
        /// <value> </value>
        IList<IAnnotation> Annotations { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a value indicating whether the @object has an annotation with the given type
        /// </summary>
        /// <param name="annoationType">Anotation type
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        bool HasAnnotationType(string annoationType);

        /// <summary>
        /// Returns the annotations with the given type, returns an empty Set is no annotations exist that have a type which
        /// matches the given string
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ISet<IAnnotation> GetAnnotationsByType(string type);

        /// <summary>
        /// Returns the annotations with the given title, returns an empty Set is no annotations exist that have a type which
        /// matches the given string
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        ISet<IAnnotation> GetAnnotationsByTitle(string title);

        #endregion
    }
}