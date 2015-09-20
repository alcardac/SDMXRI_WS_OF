// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryOfSets.cs" company="Eurostat">
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
    /// The dictionary of sets.
    /// </summary>
    /// <typeparam name="TKey">
    /// The key type
    /// </typeparam>
    /// <typeparam name="TValue">
    /// The <see cref="ISet{T}"/> type
    /// </typeparam>
    public class DictionaryOfSets<TKey, TValue> : Dictionary<TKey, ISet<TValue>>, IDictionaryOfSets<TKey, TValue>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryOfSets{TKey,TValue}"/> class.
        /// </summary>
        public DictionaryOfSets()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryOfSets{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="capacity">
        /// The capacity.
        /// </param>
        public DictionaryOfSets(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryOfSets{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="comparer">
        /// The comparer.
        /// </param>
        public DictionaryOfSets(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryOfSets{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="capacity">
        /// The capacity.
        /// </param>
        /// <param name="comparer">
        /// The comparer.
        /// </param>
        public DictionaryOfSets(int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryOfSets{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="dictionary">
        /// The dictionary.
        /// </param>
        /// <param name="comparer">
        /// The comparer.
        /// </param>
        public DictionaryOfSets(IDictionary<TKey, ISet<TValue>> dictionary, IEqualityComparer<TKey> comparer)
            : base(dictionary, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryOfSets{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="dictionary">
        /// The dictionary.
        /// </param>
        public DictionaryOfSets(IDictionary<TKey, ISet<TValue>> dictionary)
            : base(dictionary)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryOfSets{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        protected DictionaryOfSets(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        /// <summary>
        /// Adds the specified value to the <paramref name="value"/> to the set corresponding to the specified <paramref name="key"/>
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public void AddToSet(TKey key, TValue value)
        {
            ISet<TValue> set;
            if (!this.TryGetValue(key, out set))
            {
                set = new HashSet<TValue>();
                this.Add(key, set);
            }

            set.Add(value);
        }
    }
}