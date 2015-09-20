// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHierarchicalItemObject.cs" company="Eurostat">
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
    /// IHierarchicalItemObject are ItemObjects that can contain child ItemObjects
    /// </summary>
    /// <typeparam name="T">
    /// The item type. Which is a <see cref="IItemObject"/> based class
    /// </typeparam>
    public interface IHierarchicalItemObject<T> : IItemObject
        where T : IItemObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets any child items, if no children exist then an empty list is returned
        ///     <p />
        ///     <b>NOTE</b>The list is a copy so modify the returned set will not
        ///     be reflected in this instance
        /// </summary>
        //// TODO Use ReadOnlyCollection<T> to avoid copying the list.
        IList<T> Items { get; }

        #endregion
    }
}