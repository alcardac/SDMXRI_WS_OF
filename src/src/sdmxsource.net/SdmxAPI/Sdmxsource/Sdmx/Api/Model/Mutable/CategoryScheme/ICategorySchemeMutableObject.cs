// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICategorySchemeMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;

    #endregion

    /// <summary>
    ///     The CategorySchemeMutableObject interface.
    /// </summary>
    public interface ICategorySchemeMutableObject : IItemSchemeMutableObject<ICategoryMutableObject>
    {
        #region Public Properties

        /// <summary>
        ///     Gets a representation of itself in a Object which can not be modified, modifications to the mutable Object
        ///     are not reflected in the returned instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new ICategorySchemeObject ImmutableInstance { get; }

        #endregion
    }
}