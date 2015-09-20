// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeTranscodingCollection.cs" company="Eurostat">
//   Date Created : 2013-07-10
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class holds a collection of <see cref="TimeTranscodingEntity" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    ///     This class holds a collection of <see cref="TimeTranscodingEntity" />
    /// </summary>
    public class TimeTranscodingCollection : KeyedCollection<string, TimeTranscodingEntity>
    {
        #region Fields

        /// <summary>
        /// The _transcoding id.
        /// </summary>
        private readonly long _transcodingId;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeTranscodingCollection"/> class.
        /// </summary>
        /// <param name="transcodingId">
        /// The transcoding id.
        /// </param>
        public TimeTranscodingCollection(long transcodingId)
            : base(StringComparer.Ordinal, 0)
        {
            this._transcodingId = transcodingId;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Create add and return a new <see cref="TimeTranscodingEntity"/> with the specified <paramref name="frequencyValue"/>
        /// </summary>
        /// <param name="frequencyValue">
        /// The frequency value.
        /// </param>
        /// <returns>
        /// The <see cref="TimeTranscodingEntity"/>.
        /// </returns>
        public TimeTranscodingEntity Add(string frequencyValue)
        {
            var item = new TimeTranscodingEntity(frequencyValue, this._transcodingId);
            this.Add(item);
            return item;
        }

        /// <summary>
        /// Deep copy the current collection.
        /// </summary>
        /// <param name="transcodingId">
        /// The transcoding id.
        /// </param>
        /// <returns>
        /// The <see cref="TimeTranscodingCollection"/>.
        /// </returns>
        public TimeTranscodingCollection DeepCopy(long transcodingId)
        {
            var collection = new TimeTranscodingCollection(transcodingId);
            for (int i = 0; i < this.Items.Count; i++)
            {
                collection.Add(this.Items[i].DeepCopy(transcodingId));
            }

            return collection;
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
        protected override string GetKeyForItem(TimeTranscodingEntity item)
        {
            return item.FrequencyValue;
        }

        /// <summary>
        /// Inserts an element into the <see cref="T:System.Collections.ObjectModel.KeyedCollection`2"/> at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="item"/> should be inserted.
        /// </param>
        /// <param name="item">
        /// The object to insert.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.-or-<paramref name="index"/> is greater than
        ///     <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>
        ///     .
        /// </exception>
        protected override void InsertItem(int index, TimeTranscodingEntity item)
        {
            this.ValidateItem(item);

            base.InsertItem(index, item);
        }

        /// <summary>
        /// Replaces the item at the specified index with the specified item.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the item to be replaced.
        /// </param>
        /// <param name="item">
        /// The new item.
        /// </param>
        protected override void SetItem(int index, TimeTranscodingEntity item)
        {
            this.ValidateItem(item);
            base.SetItem(index, item);
        }

        /// <summary>
        /// Validate the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">
        /// The <see cref="TimeTranscodingEntity"/> to validate
        /// </param>
        /// <exception cref="ArgumentException">
        /// The <paramref name="item"/> <see cref="TimeTranscodingEntity.TranscodingId"/> does not match the
        ///     <see cref="_transcodingId"/>
        /// </exception>
        private void ValidateItem(TimeTranscodingEntity item)
        {
            if (item.TranscodingId != this._transcodingId)
            {
                throw new ArgumentException(ErrorMessages.InvalidTimeTranscoding, "item");
            }
        }

        #endregion
    }
}