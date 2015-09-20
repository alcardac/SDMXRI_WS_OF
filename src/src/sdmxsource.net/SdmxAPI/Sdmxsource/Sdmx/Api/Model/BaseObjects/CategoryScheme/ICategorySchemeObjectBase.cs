// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICategorySchemeObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.CategoryScheme
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;

    #endregion

    /// <summary>
    ///     A CategoryScheme is a container for categories.  Typically a category would be a statistical subject matter domain.
    /// </summary>
    public interface ICategorySchemeObjectBase : IMaintainableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the built from.
        /// </summary>
        new ICategorySchemeObject BuiltFrom { get; }

        /// <summary>
        ///     Gets a list of all the categories contained within this scheme
        /// </summary>
        /// <value> list of categories </value>
        IList<ICategoryObjectBase> Categories { get; }

        #endregion
    }
}