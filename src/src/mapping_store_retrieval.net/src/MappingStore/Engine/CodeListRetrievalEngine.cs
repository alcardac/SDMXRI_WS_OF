// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeListRetrievalEngine.cs" company="Eurostat">
//   Date Created : 2013-02-12
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The code list retrieval engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Collections.Generic;

    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;

    /// <summary>
    ///     The code list retrieval engine.
    /// </summary>
    internal class CodeListRetrievalEngine : HierarchicalItemSchemeRetrievalEngine<ICodelistMutableObject, ICodeMutableObject>
    {
        #region Fields

        /// <summary>
        ///     The _item <see cref="SqlQueryInfo" /> builder.
        /// </summary>
        private readonly ItemSqlQueryBuilder _itemSqlQueryBuilder;

        /// <summary>
        ///     The _item SQL query info.
        /// </summary>
        private readonly SqlQueryInfo _itemSqlQueryInfo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeListRetrievalEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null
        /// </exception>
        public CodeListRetrievalEngine(Database mappingStoreDb)
            : base(mappingStoreDb, CodeListConstant.TableInfo)
        {
            this._itemSqlQueryBuilder = new ItemSqlQueryBuilder(mappingStoreDb, CodeListConstant.ItemOrderBy);
            this._itemSqlQueryInfo = this._itemSqlQueryBuilder.Build(CodeListConstant.ItemTableInfo);
        }

        /// <summary>
        ///    Gets the Code SQL query info.
        /// </summary>
        protected SqlQueryInfo ItemSqlQueryInfo
        {
            get
            {
                return this._itemSqlQueryInfo;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Create a new instance of <see cref="ICodelistMutableObject" />.
        /// </summary>
        /// <returns>
        ///     The <see cref="ICodelistMutableObject" />.
        /// </returns>
        protected override ICodelistMutableObject CreateArtefact()
        {
            return new CodelistMutableCore();
        }

        /// <summary>
        ///     Create an item.
        /// </summary>
        /// <returns>
        ///     The <see cref="ICodeMutableObject" />.
        /// </returns>
        protected override ICodeMutableObject CreateItem()
        {
            return new CodeMutableCore();
        }

        /// <summary>
        /// When this method is overridden it is used to retrieve Items of a ItemScheme and populate the output List
        /// </summary>
        /// <param name="itemScheme">
        /// The <see cref="IItemSchemeMutableObject{T}"/> to fill with <see cref="IItemMutableObject"/>
        /// </param>
        /// <param name="parentSysId">
        /// The primary key of the Item Scheme from Mapping Store table ARTEFACT.ART_ID field
        /// </param>
        protected override void FillItems(ICodelistMutableObject itemScheme, long parentSysId)
        {
            var itemQuery = new ItemSqlQuery(this.ItemSqlQueryInfo, parentSysId);
            this.FillItemWithParent(itemScheme, itemQuery);
        }

        /// <summary>
        /// Handle item child method. Override to handle item relationships
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
        protected override void HandleItemChild(
            ICodelistMutableObject itemSchemeBean, IDictionary<long, ICodeMutableObject> allItems, IDictionary<long, long> childItems, long childSysId, ICodeMutableObject child)
        {
            long parentItemId;
            if (childItems.TryGetValue(childSysId, out parentItemId))
            {
                ICodeMutableObject parent;
                if (allItems.TryGetValue(parentItemId, out parent))
                {
                    child.ParentCode = parent.Id;
                }
            }

            // HACK Move descriptions to names. 
            // In SDMX  v2.0 codes had only descriptions while in SDMX v2.1 they need to have names.
            // In mapping store (at least up to 2.7) only SDMX v2.0 was supported and code description were saved
            // as descriptions. So in case there are no names we move descriptions to names.
            if (child.Names.Count == 0)
            {
                var description = child.Descriptions;
                while (description.Count > 0)
                {
                    var index = description.Count - 1;
                    var last = description[index];
                    description.RemoveAt(index);
                    child.Names.Add(last);
                }
            }

            itemSchemeBean.AddItem(child);
        }

        #endregion
    }
}