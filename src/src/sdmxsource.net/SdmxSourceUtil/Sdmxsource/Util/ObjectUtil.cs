// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectUtil.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Util
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    /// Utility class providing helper methods for generic Objects.
    /// </summary>
    public class ObjectUtil
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns true if:
        /// <ul>
        ///   <li>Both Strings are null</li>
        ///   <li>Both Strings are equal</li>
        /// </ul>
        /// </summary>
        /// <param name="o1">First object
        /// </param>
        /// <param name="o2">Second object
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public static bool Equivalent(object o1, object o2)
        {
            if (o1 == null && o2 == null)
            {
                return true;
            }

            if (o1 == null)
            {
                return false;
            }

            return o1.Equals(o2);
        }

        // <summary>
        /// Returns true if:
        /// <ul>
        ///   <li>Both objects are null</li>
        ///   <li>The first collection contains all elements of the 2nd and the 2nd contains all elements of the 1st</li>
        /// </ul>
        /// </summary>
        /// <param name="c1">First collection
        /// </param>
        /// <param name="c2">Second collection
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public static bool ContainsAll<T>(ICollection<T> c1, ICollection<T> c2)
        {
            if (c1 == null && c2 == null)
            {
                return true;
            }

            if (c2 == null)
            {
                return false;
            }

            if (c1 == null)
            {
                return false;
            }

            if (c1.Count != c2.Count)
            {
                return false;
            }

            if (!c1.ContainsAll(c2))
            {
                return false;
            }

            if (!c2.ContainsAll(c1))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The equivalent collection.
        /// </summary>
        /// <param name="c1">
        /// The c 1. 
        /// </param>
        /// <param name="c2">
        /// The c 2. 
        /// </param>
        /// <typeparam name="T">Generic type param
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public static bool EquivalentCollection<T>(ICollection<T> c1, ICollection<T> c2)
        {
            if (ReferenceEquals(c1, c2))
            {
                return true;
            }

            if (!ContainsAll(c1, c2))
            {
                return false;
            }

            IEnumerator<T> it1 = c1.GetEnumerator();
            IEnumerator<T> it2 = c2.GetEnumerator();
            while (it1.MoveNext() && it2.MoveNext())
            {
                object obj1 = it1.Current;
                object obj2 = it2.Current;
                if (!Equivalent(obj1, obj2))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if:
        /// <ul>
        ///   <li>Both Strings are null</li>
        ///   <li>Both Strings are equal</li>
        /// </ul>
        /// </summary>
        /// <param name="s1">The first string
        /// </param>
        /// <param name="s2">The second string
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public static bool EquivalentString(string s1, string s2)
        {
            return string.Equals(s1, s2);
            ////return Equivalent(s1, s2);
        }

        /// <summary>
        /// Returns true if the array is not null and has a size greater than zero.
        /// </summary>
        /// <param name="array">Input array
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public static bool ValidArray(object[] array)
        {
            return array != null && array.Length > 0;
        }

        /// <summary>
        /// Returns true if the array is not null and has a size greater than zero.
        /// </summary>
        /// <param name="array">
        /// The array 
        /// </param>
        /// <typeparam name="T">
        /// The Type of <paramref name="array"/> 
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public static bool ValidArray<T>(T[] array)
        {
            return array != null && array.Length > 0;
        }

        /// <summary>
        /// Returns true if the collection is not null and has a size greater than zero.
        /// </summary>
        /// <param name="collection">
        /// The collection to check 
        /// </param>
        /// <typeparam name="T">
        /// The type of the <paramref name="collection"/> 
        /// </typeparam>
        /// <returns>
        /// true if the collection is not null and has a size greater than zero. 
        /// </returns>
        public static bool ValidCollection<T>(ICollection<T> collection)
        {
            return collection != null && collection.Count > 0;
        }

        /// <summary>
        /// Returns true if the collection is not null and has a size greater than zero.
        /// </summary>
        /// <param name="collection">
        /// The collection to check 
        /// </param>
        /// <returns>
        /// true if the collection is not null and has a size greater than zero. 
        /// </returns>
        public static bool ValidCollection(ICollection collection)
        {
            return collection != null && collection.Count > 0;
        }

        /// <summary>
        /// Returns true if the Map is not null and has a size greater than zero.
        /// </summary>
        /// <param name="map">The dictionary
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public static bool ValidMap(IDictionary map)
        {
            return map != null && map.Count > 0;
        }

        /// <summary>
        /// Returns whether all of the objects are not null
        /// </summary>
        /// <param name="objects">Objects params
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public static bool ValidObject(params object[] objects)
        {
            if (objects == null || objects.Length == 0)
            {
                return false;
            }

            foreach (object obj in objects)
            {
                if (obj == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns whether at least one of the strings is not null and has a length of greater than zero
        /// after trimming the leading and trailing whitespace
        /// </summary>
        /// <param name="strings">String array
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public static bool ValidOneString(params string[] strings)
        {
            if (strings != null)
            {
                foreach (string str in strings)
                {
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns whether all of the strings are not null and have a length of greater than zero
        /// after trimming the leading and trailing whitespace.
        /// </summary>
        /// <param name="strings">Strings param
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public static bool ValidString(params string[] strings)
        {
            if (strings == null || strings.Length == 0)
            {
                return false;
            }

            foreach (string str in strings)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// The valid string.
        /// </summary>
        /// <param name="uri">
        /// The uri. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public static bool ValidString(Uri uri)
        {
            return uri != null && !string.IsNullOrEmpty(uri.ToString());
        }

        #endregion
    }
}