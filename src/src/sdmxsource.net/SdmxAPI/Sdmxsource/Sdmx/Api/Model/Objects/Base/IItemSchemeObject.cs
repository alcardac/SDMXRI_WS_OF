// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IItemSchemeObject.cs" company="Eurostat">
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
    /// An ItemScheme contains a list of items
    /// </summary>
    /// <typeparam name="T">
    /// The item type. Which is a <see cref="IItemObject"/> based class
    /// </typeparam>
    public interface IItemSchemeObject<T> : IMaintainableObject
        where T : IItemObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the list of items contained within this scheme.
        ///     <p />
        ///     <b>NOTE</b>The list is a copy so modify the returned set will not
        ///     be reflected in the IItemSchemeObject instance
        /// </summary>
        //// TODO Use ReadOnlyCollection<T> to avoid copying the list.
        IList<T> Items { get; } 

        /// <summary>
        ///     Gets a value indicating whether this is a partial item scheme.
        ///     Gets a value indicating whether the is a partial item scheme.
        /// </summary>
        bool Partial { get; }

        /// <summary>
        /// Filters the items.
        /// </summary>
        /// <param name="filterIds">The filter ids.</param>
        /// <param name="isKeepSet">if set to <c>true</c> [is keep set].</param>
        /// <returns>A new <see cref="IItemSchemeObject{T}"/> with filtered items</returns>
        IItemSchemeObject<T> FilterItems(ICollection<string> filterIds, bool isKeepSet);

        #endregion
    }
}