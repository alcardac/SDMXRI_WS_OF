// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnnotableMutableObject.cs" company="Eurostat">
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

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     The AnnotableMutableObject interface.
    /// </summary>
    public interface IAnnotableMutableObject : IMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the annotations.
        /// </summary>
        IList<IAnnotationMutableObject> Annotations { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add annotation.
        /// </summary>
        /// <param name="annotation">
        /// The annotation.
        /// </param>
        void AddAnnotation(IAnnotationMutableObject annotation);


        /// <summary>
        /// Adds an annotation and returns it 
        /// </summary>
        /// <param name="title">The title</param>
        /// <param name="type">The type</param>
        /// <param name="url">The URL</param>
        /// <returns></returns>
        IAnnotationMutableObject AddAnnotation(string title, string type, string url);
        #endregion
    }
}