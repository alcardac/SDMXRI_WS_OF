// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDictionaryOfSets.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Util
{
    using System.Collections.Generic;

    /// <summary>
    /// A <see cref="IDictionary{TKey,TValue}"/> between <typeparamref name="TKey"/> and a <see cref="ISet{T}"/> of <typeparamref name="TSetValue"/>
    /// </summary>
    /// <typeparam name="TKey"> The key type</typeparam>
    /// <typeparam name="TSetValue">
    /// The type of the <see cref="ISet{T}"/>
    /// </typeparam>
    public interface IDictionaryOfSets<TKey, TSetValue> : IDictionary<TKey, ISet<TSetValue>>
    {
        /// <summary>
        /// Adds the specified value to the <paramref name="value"/> to the set corresponding to the specified <paramref name="key"/>
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        void AddToSet(TKey key, TSetValue value);
    }
}