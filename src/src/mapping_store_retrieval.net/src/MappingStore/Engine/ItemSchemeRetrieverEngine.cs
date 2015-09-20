// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemSchemeRetrieverEngine.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The item scheme retriever engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The item scheme retriever engine.
    /// </summary>
    /// <typeparam name="TMaintaible">
    /// The <see cref="IItemSchemeMutableObject{T}"/> type
    /// </typeparam>
    /// <typeparam name="TItem">
    /// The <typeparamref name="TMaintaible"/> Item type
    /// </typeparam>
    internal abstract class ItemSchemeRetrieverEngine<TMaintaible, TItem> : ArtefactRetrieverEngine<TMaintaible>
        where TMaintaible : IItemSchemeMutableObject<TItem> where TItem : IItemMutableObject
    {
        #region Fields

        /// <summary>
        ///     The item command builder.
        /// </summary>
        private readonly ItemCommandBuilder _itemCommandBuilder;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSchemeRetrieverEngine{TMaintaible,TItem}"/> class.
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
        protected ItemSchemeRetrieverEngine(Database mappingStoreDb, TableInfo tableInfo, string orderBy = null)
            : base(mappingStoreDb, tableInfo, orderBy)
        {
            this._itemCommandBuilder = new ItemCommandBuilder(mappingStoreDb);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the item command builder.
        /// </summary>
        protected ItemCommandBuilder ItemCommandBuilder
        {
            get
            {
                return this._itemCommandBuilder;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Retrieve the <see cref="IMaintainableMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        /// The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="versionConstraints">
        /// The version types.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        public override ISet<TMaintaible> Retrieve(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, VersionQueryType versionConstraints)
        {
            var sqlInfo = versionConstraints == VersionQueryType.Latest ? this.SqlQueryInfoForLatest : this.SqlQueryInfoForAll;
            var sqlQuery = new ArtefactSqlQuery(sqlInfo, maintainableRef);
            return this.RetrieveItemScheme(detail, sqlQuery);
        }

        /// <summary>
        /// Retrieve the <see cref="IMaintainableMutableObject"/> with the latest version group by ID and AGENCY from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        /// The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        public override TMaintaible RetrieveLatest(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail)
        {
            var sqlQuery = new ArtefactSqlQuery(this.SqlQueryInfoForLatest, maintainableRef);
            var mutableObjects = this.RetrieveItemScheme(detail, sqlQuery);
            switch (mutableObjects.Count)
            {
                case 0:
                    return default(TMaintaible);
                case 1:
                    return mutableObjects.First();
                default:
                    throw new ArgumentException(ErrorMessages.MoreThanOneArtefact, "maintainableRef");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The parse extra fields.
        /// </summary>
        /// <param name="artefact">
        /// The artefact.
        /// </param>
        /// <param name="dataReader">
        /// The reader.
        /// </param>
        protected override void HandleArtefactExtraFields(TMaintaible artefact, IDataReader dataReader)
        {
            artefact.IsPartial = DataReaderHelper.GetBoolean(dataReader, "IS_PARTIAL");
        }

        /// <summary>
        /// Populate the common properties of an Item
        /// </summary>
        /// <param name="item">
        /// The item to populate
        /// </param>
        /// <param name="dataReader">
        /// The IDataReader to read the values from
        /// </param>
        /// <param name="idIndex">
        /// The field index of the ITEM.ID field
        /// </param>
        protected static void PopulateItem(IItemMutableObject item, IDataRecord dataReader, int idIndex)
        {
            // ReSharper restore SuggestBaseTypeForParameter
            item.Id = DataReaderHelper.GetString(dataReader, idIndex); // "ID"
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
        protected abstract void FillItems(TMaintaible itemScheme, long parentSysId);

        /// <summary>
        /// Retrieve details for the specified <paramref name="artefact"/> with MAPPING STORE ARTEFACT.ART_ID equal to
        ///     <paramref name="sysId"/>
        /// </summary>
        /// <param name="artefact">
        /// The artefact.
        /// </param>
        /// <param name="sysId">
        /// The MAPPING STORE ARTEFACT.ART_ID value
        /// </param>
        /// <returns>
        /// The <typeparamref name="TMaintaible"/>.
        /// </returns>
        protected override TMaintaible RetrieveDetails(TMaintaible artefact, long sysId)
        {
            this.FillItems(artefact, sysId);
            return artefact;
        }

        /// <summary>
        /// The retrieve item scheme.
        /// </summary>
        /// <param name="detail">
        /// The detail.
        /// </param>
        /// <param name="sqlQuery">
        /// The SQL query.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IItemSchemeObject}"/>.
        /// </returns>
        private ISet<TMaintaible> RetrieveItemScheme(ComplexStructureQueryDetailEnumType detail, ArtefactSqlQuery sqlQuery)
        {
            return this.RetrieveArtefacts(sqlQuery, detail);
        }

        #endregion
    }
}