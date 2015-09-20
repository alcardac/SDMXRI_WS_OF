// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionsExtensionMethods.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Util.Extensions
{
    using System;
    using System.Collections.Generic;

    /// <summary> Extension methods
    /// </summary>
    public static class CollectionsExtensionMethods
    {
        #region Public Methods and Operators

        /// <summary>
        /// The add all.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <typeparam name="T">Generic type param
        /// </typeparam>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException
        /// </exception>
        public static void AddAll<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items", "New collection cannot be null");
            }

            if (collection == null)
            {
                throw new ArgumentNullException("collection", "Parameter cannot be null");
            }

            foreach (T item in items)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// The remove item list.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <typeparam name="T">Generic type param
        /// </typeparam>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException
        /// </exception>
        public static void RemoveItemList<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items", "New collection cannot be null");
            }

            if (collection == null)
            {
                throw new ArgumentNullException("collection", "Parameter cannot be null");
            }

            foreach (T item in items)
            {
                collection.Remove(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="items"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool ContainsAll<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items", "New collection cannot be null");
            }

            if (collection == null)
            {
                throw new ArgumentNullException("collection", "Parameter cannot be null");
            }

            foreach (T item in items)
            {
                if (!collection.Contains(item))
                    return false;
            }

            return true;
        }



        #endregion
    }
}