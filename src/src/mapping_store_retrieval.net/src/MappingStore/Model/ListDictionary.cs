// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListDictionary.cs" company="Eurostat">
//   Date Created : 2013-04-30
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   A generic ordered dictionary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// A generic ordered dictionary.
    /// </summary>
    /// <typeparam name="TKey">
    /// Key type
    /// </typeparam>
    /// <typeparam name="TValue">
    /// Value type
    /// </typeparam>
    public class ListDictionary<TKey, TValue> : KeyedCollection<TKey, KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ListDictionary{TKey,TValue}" /> class.
        /// </summary>
        public ListDictionary()
            : base(null, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListDictionary{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="comparer">
        /// The implementation of the <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> generic interface to use when comparing keys, or null to use the default equality comparer for the type of the key, obtained from
        ///     <see cref="P:System.Collections.Generic.EqualityComparer`1.Default"/>
        ///     .
        /// </param>
        public ListDictionary(IEqualityComparer<TKey> comparer)
            : base(comparer, 0)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2" />
        ///     .
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the object that implements
        ///     <see cref="T:System.Collections.Generic.IDictionary`2" />
        ///     .
        /// </returns>
        public ICollection<TKey> Keys
        {
            get
            {
                if (this.Dictionary != null)
                {
                    return this.Dictionary.Keys;
                }

                return this.Items.Select(pair => pair.Key).ToArray();
            }
        }

        /// <summary>
        ///     Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2" />
        ///     .
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the object that implements
        ///     <see cref="T:System.Collections.Generic.IDictionary`2" />
        ///     .
        /// </returns>
        public ICollection<TValue> Values
        {
            get
            {
                return this.Items.Select(pair => pair.Value).ToArray();
            }
        }

        #endregion

        #region Explicit Interface Indexers

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <returns>
        /// The element with the specified key.
        /// </returns>
        /// <param name="key">
        /// The key of the element to get or set.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="key"/> is null.
        /// </exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">
        /// The property is retrieved and <paramref name="key"/> is not found.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The property is set and the <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.
        /// </exception>
        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get
            {
                return this[key].Value;
            }

            set
            {
                this.Remove(key);
                this.Add(key, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">
        /// The object to use as the key of the element to add.
        /// </param>
        /// <param name="value">
        /// The object to use as the value of the element to add.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="key"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.
        /// </exception>
        public void Add(TKey key, TValue value)
        {
            this.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the key; otherwise, false.
        /// </returns>
        /// <param name="key">
        /// The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="key"/> is null.
        /// </exception>
        public bool ContainsKey(TKey key)
        {
            return this.Dictionary != null ? this.Dictionary.ContainsKey(key) : this.Items.Any(pair => pair.Key.Equals(key));
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key; otherwise, false.
        /// </returns>
        /// <param name="key">
        /// The key whose value to get.
        /// </param>
        /// <param name="value">
        /// When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the
        ///     <paramref name="value"/>
        ///     parameter. This parameter is passed uninitialized.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="key"/> is null.
        /// </exception>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (this.Dictionary != null)
            {
                KeyValuePair<TKey, TValue> output;
                if (this.Dictionary.TryGetValue(key, out output))
                {
                    value = output.Value;
                    return true;
                }
            }
            else
            {
                foreach (var source in this.Items.Where(pair => pair.Key.Equals(key)))
                {
                    value = source.Value;
                    return true;
                }
            }

            value = default(TValue);
            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// When implemented in a derived class, extracts the key from the specified element.
        /// </summary>
        /// <returns>
        /// The key for the specified element.
        /// </returns>
        /// <param name="item">
        /// The element from which to extract the key.
        /// </param>
        protected override TKey GetKeyForItem(KeyValuePair<TKey, TValue> item)
        {
            return item.Key;
        }

        #endregion
    }
}