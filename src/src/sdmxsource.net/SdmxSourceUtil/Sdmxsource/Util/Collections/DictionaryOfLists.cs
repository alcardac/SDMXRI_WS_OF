// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryOfLists.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Util.Collections
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using Org.Sdmxsource.Sdmx.Api.Util;

    /// <summary>
    /// The mapped lists.
    /// </summary>
    /// <typeparam name="TKey">
    /// The key type
    /// </typeparam>
    /// <typeparam name="TValue">
    /// The value type
    /// </typeparam>
    public class DictionaryOfLists<TKey, TValue> : Dictionary<TKey, IList<TValue>>, IDictionaryOfLists<TKey, TValue>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryOfLists{TKey,TValue}"/> class.
        /// </summary>
        public DictionaryOfLists()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryOfLists{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="capacity">
        /// The capacity.
        /// </param>
        public DictionaryOfLists(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryOfLists{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="comparer">
        /// The comparer.
        /// </param>
        public DictionaryOfLists(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryOfLists{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="capacity">
        /// The capacity.
        /// </param>
        /// <param name="comparer">
        /// The comparer.
        /// </param>
        public DictionaryOfLists(int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryOfLists{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="dictionary">
        /// The dictionary.
        /// </param>
        public DictionaryOfLists(IDictionary<TKey, IList<TValue>> dictionary)
            : base(dictionary)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryOfLists{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="dictionary">
        /// The dictionary.
        /// </param>
        /// <param name="comparer">
        /// The comparer.
        /// </param>
        public DictionaryOfLists(IDictionary<TKey, IList<TValue>> dictionary, IEqualityComparer<TKey> comparer)
            : base(dictionary, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryOfLists{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        protected DictionaryOfLists(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}