// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyKey.cs" company="Eurostat">
//   Date Created : 2011-12-19
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class is used to created key to be used in <see cref="IDictionary{TKey,TValue}" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSVZip.Retriever.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Xml;

    /// <summary>
    /// This class is used to created key to be used in <see cref="IDictionary{TKey,TValue}"/>
    /// </summary>
    internal class ReadOnlyKey : IEquatable<ReadOnlyKey>
    {
        #region Constants and Fields

        /// <summary>
        ///   The hash code
        /// </summary>
        private readonly int _hashCode;

        /// <summary>
        ///   The key values collection
        /// </summary>
        private readonly ReadOnlyCollection<object> _keys;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyKey"/> class.
        /// </summary>
        /// <param name="keys">
        /// The <see cref="MappedValues"/> to retrieve the keys from. 
        /// </param>
        /// <param name="nameTable">
        /// The name Table. 
        /// </param>
        public ReadOnlyKey(MappedValues keys, XmlNameTable nameTable)
        {
            var values = new List<object>(keys.DimensionValues.Count);
            foreach (var key in keys.DimensionValues)
            {
                object item = nameTable.Add(key.Value);
                this._hashCode ^= item.GetHashCode();
                values.Add(item);
            }

            this._keys = new ReadOnlyCollection<object>(values);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/> .
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/> ; otherwise, false. 
        /// </returns>
        /// <param name="obj">
        /// The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/> . 
        /// </param>
        /// <exception cref="T:System.NullReferenceException">
        /// The
        ///   <paramref name="obj"/>
        ///   parameter is null.
        /// </exception>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as ReadOnlyKey);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false. 
        /// </returns>
        /// <param name="other">
        /// An object to compare with this object. 
        /// </param>
        public bool Equals(ReadOnlyKey other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            bool areEqual = other._keys.Count == this._keys.Count;
            if (areEqual)
            {
                for (int i = 0, j = this._keys.Count; i < j && areEqual; i++)
                {
                    areEqual = ReferenceEquals(this._keys[i], other._keys[i]);
                }
            }

            return areEqual;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/> . 
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return this._hashCode;
        }

        #endregion
    }
}