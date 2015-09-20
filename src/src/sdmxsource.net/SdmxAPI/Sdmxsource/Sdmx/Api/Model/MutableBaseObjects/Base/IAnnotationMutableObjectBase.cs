// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnnotationMutableObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.MutableBaseObjects.Base
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///   An Annotation is a piece of information that can be held against an Annotable Object.
    ///   <p />
    ///   It can be thought of as a placeholder for any extraneous information that needs to be stored, where there is no
    ///   other appropriate place to store this.
    /// </summary>
    public interface IAnnotationMutableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///   Gets the texts.
        /// </summary>
        IList<ITextTypeWrapperMutableObject> Texts { get; }

        /// <summary>
        ///   Gets the title of the annotation
        /// </summary>
        /// <value> </value>
        string Title { get; }

        /// <summary>
        ///   Gets the type of the annotation
        /// </summary>
        /// <value> </value>
        string Type { get; }

        /// <summary>
        ///   Gets the Uri of the annotation
        /// </summary>
        /// <value> </value>
        Uri Url { get; }

        #endregion
    }
}