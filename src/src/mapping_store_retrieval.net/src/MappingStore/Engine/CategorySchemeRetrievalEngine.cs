// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorySchemeRetrievalEngine.cs" company="Eurostat">
//   Date Created : 2013-02-12
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The concept scheme retrieval engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;

    using Estat.Sdmxsource.Extension.Constant;
    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.CategoryScheme;

    /// <summary>
    ///     The concept scheme retrieval engine.
    /// </summary>
    internal class CategorySchemeRetrievalEngine : HierarchicalItemSchemeRetrievalEngine<ICategorySchemeMutableObject, ICategoryMutableObject>
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
        /// Initializes a new instance of the <see cref="CategorySchemeRetrievalEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null
        /// </exception>
        public CategorySchemeRetrievalEngine(Database mappingStoreDb)
            : base(mappingStoreDb, CategorySchemeConstant.TableInfo, CategorySchemeConstant.OrderBy)
        {
            this._itemSqlQueryBuilder = new ItemSqlQueryBuilder(mappingStoreDb, CategorySchemeConstant.ItemOrderBy);
            this._itemSqlQueryInfo = this._itemSqlQueryBuilder.Build(CategorySchemeConstant.ItemTableInfo);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Create a new instance of <see cref="ICategorySchemeMutableObject" />.
        /// </summary>
        /// <returns>
        ///     The <see cref="ICategorySchemeMutableObject" />.
        /// </returns>
        protected override ICategorySchemeMutableObject CreateArtefact()
        {
            return new CategorySchemeMutableCore();
        }

        /// <summary>
        ///     Create an item.
        /// </summary>
        /// <returns>
        ///     The <see cref="ICategoryMutableObject" />.
        /// </returns>
        protected override ICategoryMutableObject CreateItem()
        {
            return new CategoryMutableCore();
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
        protected override void FillItems(ICategorySchemeMutableObject itemScheme, long parentSysId)
        {
            var itemQuery = new ItemSqlQuery(this._itemSqlQueryInfo, parentSysId);
            this.FillItemWithParent(itemScheme, itemQuery);
        }

        /// <summary>
        /// When overridden handles the artefact extra fields. Does nothing by default.
        /// </summary>
        /// <param name="artefact">
        /// The artefact.
        /// </param>
        /// <param name="reader">
        /// The reader.
        /// </param>
        protected override void HandleArtefactExtraFields(ICategorySchemeMutableObject artefact, IDataReader reader)
        {
            base.HandleArtefactExtraFields(artefact, reader);
            var categoryOrder = DataReaderHelper.GetInt64(reader, "CS_ORDER");
            if (categoryOrder == long.MinValue)
            {
                categoryOrder = 0;
            }

            artefact.AddAnnotation(CustomAnnotationType.CategorySchemeNodeOrder.ToAnnotation<AnnotationMutableCore>(categoryOrder.ToString(CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// When overridden it will handle extra fields. By default it does nothing
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="reader">
        /// The reader.
        /// </param>
        protected override void HandleItemExtraFields(ICategoryMutableObject item, IDataReader reader)
        {
            base.HandleItemExtraFields(item, reader);
            var categoryOrder = DataReaderHelper.GetInt64(reader, "CORDER");
            if (categoryOrder == long.MinValue)
            {
                item.AddAnnotation(CustomAnnotationType.CategorySchemeNodeOrder.ToAnnotation<AnnotationMutableCore>(categoryOrder.ToString(CultureInfo.InvariantCulture)));
            }
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
        /// <param name="category">
        /// The child.
        /// </param>
        protected override void HandleItemChild(
            ICategorySchemeMutableObject itemSchemeBean, IDictionary<long, ICategoryMutableObject> allItems, IDictionary<long, long> childItems, long childSysId, ICategoryMutableObject category)
        {
            long parentItemId;
            if (category.Names.Count == 0)
            {
                category.AddName("en", category.Id);
            }

            if (childItems.TryGetValue(childSysId, out parentItemId))
            {
                // has parent
                ICategoryMutableObject parent = allItems[parentItemId];

                parent.AddItem(category);

                //// TODO Common API has no ParentId
                //// category.ParentId = parent.Id;
            }
            else
            {
                // add only root elements
                itemSchemeBean.AddItem(category);
            }

            //// TODO handle this at CategorisationRetrievalEngine
            //// this.PopulateDataflowRef(sysId, category.DataflowRef);
        }

        #endregion
    }
}