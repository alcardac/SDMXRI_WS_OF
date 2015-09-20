// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnnotableMutableObjectBase.cs" company="Eurostat">
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

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     An Annotable Object is one which can hold annotations against it.
    /// </summary>
    public interface IAnnotableMutableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets a list of annotations that exist for this Annotable Object
        /// </summary>
        ISet<IAnnotationMutableObjectBase> Annotations { get; }

        #endregion
    }
}