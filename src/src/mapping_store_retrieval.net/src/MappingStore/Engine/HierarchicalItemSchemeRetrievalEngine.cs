// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchicalItemSchemeRetrievalEngine.cs" company="Eurostat">
//   Date Created : 2013-02-12
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The hierarchical item scheme retrieval engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    /// <summary>
    /// The hierarchical item scheme retrieval engine.
    /// </summary>
    /// <typeparam name="TMaintaible">
    /// The <see cref="IItemSchemeMutableObject{T}"/> type
    /// </typeparam>
    /// <typeparam name="TItem">
    /// The <typeparamref name="TMaintaible"/> Item type
    /// </typeparam>
    internal abstract class HierarchicalItemSchemeRetrievalEngine<TMaintaible, TItem> : ItemSchemeRetrieverEngine<TMaintaible, TItem>
        where TMaintaible : IItemSchemeMutableObject<TItem> where TItem : IItemMutableObject
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalItemSchemeRetrievalEngine{TMaintaible,TItem}"/> class. 
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <param name="tableInfo">
        /// The table Info.
        /// </param>
        /// <param name="orderBy">
        /// The order By.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null
        /// </exception>
        protected HierarchicalItemSchemeRetrievalEngine(Database mappingStoreDb, TableInfo tableInfo, string orderBy = null)
            : base(mappingStoreDb, tableInfo, orderBy)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Create an item.
        /// </summary>
        /// <returns>
        ///     The <typeparamref name="TItem"/>
        /// </returns>
        protected abstract TItem CreateItem();

        /// <summary>
        /// Fill the specified <paramref name="itemSchemeBean"/> with parent items.
        /// </summary>
        /// <param name="itemSchemeBean">
        ///     The parent <see cref="IItemSchemeMutableObject{T}"/>
        /// </param>
        /// <param name="itemQuery">
        ///     The item Query.
        /// </param>
        protected void FillItemWithParent(TMaintaible itemSchemeBean, ItemSqlQuery itemQuery)
        {
            var allItems = new Dictionary<long, TItem>();
            var orderedItems = new List<KeyValuePair<long, TItem>>();
            var childItems = new Dictionary<long, long>();
            using (DbCommand command = this.ItemCommandBuilder.Build(itemQuery))
            {
                this.ReadItems(allItems, orderedItems, command, childItems);
            }

            this.FillParentItems(itemSchemeBean, childItems, allItems, orderedItems);
        }

        /// <summary>
        /// Handle item child method. Override to handle parent relationships 
        /// </summary>
        /// <param name="itemSchemeBean">
        /// The item scheme bean.
        /// </param>
        /// <param name="allItems">
        /// The all items.
        /// </param>
        /// <param name="childItems">
        /// The child items.
        /// </param>
        /// <param name="childSysId">
        /// The child sys id.
        /// </param>
        /// <param name="child">
        /// The child.
        /// </param>
        protected abstract void HandleItemChild(TMaintaible itemSchemeBean, IDictionary<long, TItem> allItems, IDictionary<long, long> childItems, long childSysId, TItem child);

        /// <summary>
        /// When overridden  it will handle extra fields. By default it does nothing
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="reader">
        /// The reader.
        /// </param>
        protected virtual void HandleItemExtraFields(TItem item, IDataReader reader)
        {
        }

        /// <summary>
        /// Fill parent items
        /// </summary>
        /// <param name="itemSchemeBean">
        /// The item scheme bean.
        /// </param>
        /// <param name="childItems">
        /// The child items.
        /// </param>
        /// <param name="allItems">
        /// All items.
        /// </param>
        /// <param name="orderedItems">
        /// The ordered items.
        /// </param>
        protected void FillParentItems(TMaintaible itemSchemeBean, IDictionary<long, long> childItems, IDictionary<long, TItem> allItems, IEnumerable<KeyValuePair<long, TItem>> orderedItems)
        {
            foreach (KeyValuePair<long, TItem> item in orderedItems)
            {
                long sysId = item.Key;
                TItem itemBean = item.Value;
                this.HandleItemChild(itemSchemeBean, allItems, childItems, sysId, itemBean);
            }
        }

        /// <summary>
        /// Read all items with parent item
        /// </summary>
        /// <param name="allItems">
        /// The all items.
        /// </param>
        /// <param name="orderedItems">
        /// The ordered items.
        /// </param>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="childItems">
        /// The child items.
        /// </param>
        protected void ReadItems(IDictionary<long, TItem> allItems, ICollection<KeyValuePair<long, TItem>> orderedItems, DbCommand command, IDictionary<long, long> childItems)
        {
            using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
            {
                int sysIdIdx = dataReader.GetOrdinal("SYSID");
                int idIdx = dataReader.GetOrdinal("ID");
                int parentIdx = dataReader.GetOrdinal("PARENT");
                int txtIdx = dataReader.GetOrdinal("TEXT");
                int langIdx = dataReader.GetOrdinal("LANGUAGE");
                int typeIdx = dataReader.GetOrdinal("TYPE");
                while (dataReader.Read())
                {
                    long sysId = DataReaderHelper.GetInt64(dataReader, sysIdIdx);
                    TItem item;
                    if (!allItems.TryGetValue(sysId, out item))
                    {
                        item = this.CreateItem();
                        PopulateItem(item, dataReader, idIdx);
                        this.HandleItemExtraFields(item, dataReader);
                        orderedItems.Add(new KeyValuePair<long, TItem>(sysId, item));
                        
                        allItems.Add(sysId, item);
                        long parentItemId = DataReaderHelper.GetInt64(dataReader, parentIdx);
                        if (parentItemId > long.MinValue)
                        {
                            childItems.Add(sysId, parentItemId);
                        }
                    }

                    ReadLocalisedString(item, typeIdx, txtIdx, langIdx, dataReader);
                }
            }
        }

        #endregion
    }
}