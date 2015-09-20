// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IItemSchemeMutableObject.cs" company="Eurostat">
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
    /// The ItemSchemeMutableObject interface.
    /// </summary>
    /// <typeparam name="T">
    /// Generic type parameter.
    /// </typeparam>
    public interface IItemSchemeMutableObject<T> : IMaintainableMutableObject
        where T : IItemMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the list of items, this can be null.
        /// </summary>
        IList<T> Items { get;  }

        /// <summary>
        ///     Gets or sets a value indicating whether it is partial.
        /// </summary>
        bool IsPartial { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the item to the list of items, creates a list if it does not already exist.
        /// </summary>
        /// <param name="item">The item.</param>
        void AddItem(T item);

        /// <summary>
        /// Removes the item with the given id, if it exists.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>true if the item was successfully removed, false otherwise</returns>
        bool RemoveItem(string id);

        /// <summary>
        /// Creates an item and adds it to the scheme
        /// </summary>
        /// <param name="id">The id to set</param>
        /// <param name="name">The name to set in the 'en' lang</param>
        /// <returns>The created item</returns>
        T CreateItem(string id, string name);

        #endregion
    }
}