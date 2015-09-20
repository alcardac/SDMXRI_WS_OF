// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchicalCodeListRetrievealEngine.cs" company="Eurostat">
//   Date Created : 2013-02-13
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The hierarchical code list retrieval engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     The hierarchical code list retrieval engine.
    /// </summary>
    internal class HierarchicalCodeListRetrievealEngine : ArtefactRetrieverEngine<IHierarchicalCodelistMutableObject>
    {
        #region Static Fields

        /// <summary>
        ///     The _codelist type.
        /// </summary>
        private static readonly SdmxStructureType _codelistType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList);

        #endregion

        #region Fields

        /// <summary>
        ///     The Code Reference query.
        /// </summary>
        private readonly SqlQueryInfo _codeRefQueryInfo;

        /// <summary>
        ///     The <c>CodelistRefQuery</c> query.
        /// </summary>
        private readonly SqlQueryInfo _codelistRefQueryInfo;

        /// <summary>
        ///     The hierarchy query.
        /// </summary>
        private readonly SqlQueryInfo _hierarchyQueryInfo;

        /// <summary>
        ///     The reference/item command builder.
        /// </summary>
        private readonly ItemCommandBuilder _itemCommandBuilder;

        /// <summary>
        ///     The Level query.
        /// </summary>
        private readonly SqlQueryInfo _levelQueryInfo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalCodeListRetrievealEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null.
        /// </exception>
        public HierarchicalCodeListRetrievealEngine(Database mappingStoreDb)
            : base(mappingStoreDb, HclConstant.TableInfo)
        {
            var sqlQueryBuilder = new ReferencedSqlQueryBuilder(this.MappingStoreDb, null);
            this._hierarchyQueryInfo = sqlQueryBuilder.Build(HclConstant.HierarchyQueryFormat);
            this._codelistRefQueryInfo = sqlQueryBuilder.Build(HclConstant.CodelistRefQueryFormat);
            this._codeRefQueryInfo = sqlQueryBuilder.Build(HclConstant.CodeRefQueryFormat);
            this._levelQueryInfo = sqlQueryBuilder.Build(HclConstant.LevelQueryFormat);
            this._itemCommandBuilder = new ItemCommandBuilder(this.MappingStoreDb);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Retrieve the <see cref="IHierarchicalCodelistMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="versionConstraints">The version constraints</param>
        /// <returns>
        /// The <see cref="ISet{IHierarchicalCodelistMutableObject}"/>.
        /// </returns>
        public override ISet<IHierarchicalCodelistMutableObject> Retrieve(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, VersionQueryType versionConstraints)
        {
            var sqlInfo = versionConstraints == VersionQueryType.Latest ? this.SqlQueryInfoForLatest : this.SqlQueryInfoForAll;
            return this.HierarchicalCodelistMutableObjects(maintainableRef, detail, sqlInfo);
        }

        /// <summary>
        /// Retrieve the <see cref="IHierarchicalCodelistMutableObject"/> with the latest version group by ID and AGENCY from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <returns>
        /// The <see cref="IHierarchicalCodelistMutableObject"/>.
        /// </returns>
        public override IHierarchicalCodelistMutableObject RetrieveLatest(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail)
        {
            SqlQueryInfo queryInfo = this.SqlQueryInfoForLatest;
            ISet<IHierarchicalCodelistMutableObject> mutableObjects = this.HierarchicalCodelistMutableObjects(maintainableRef, detail, queryInfo);
            return mutableObjects.GetOneOrNothing();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the referenced from inner joins to use with <see cref="ArtefactParentsSqlBuilder"/>
        /// </summary>
        /// <param name="structureEnumType">
        ///     The structure  type.
        /// </param>
        /// <returns>
        /// The referenced from inner joins
        /// </returns>
        protected override string GetReferencedFromInnerJoins(SdmxStructureEnumType structureEnumType)
        {
            string innerJoin = null;
            switch (structureEnumType)
            {
                case SdmxStructureEnumType.CodeList:
                    innerJoin = CodeListConstant.ReferencingFromHcl;
                    break;
            }

            return innerJoin;
        }

        /// <summary>
        ///     Create a new instance of <see cref="IDataflowMutableObject" />.
        /// </summary>
        /// <returns>
        ///     The <see cref="IDataflowMutableObject" />.
        /// </returns>
        protected override IHierarchicalCodelistMutableObject CreateArtefact()
        {
            return new HierarchicalCodelistMutableCore();
        }

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
        /// The <see cref="IHierarchicalCodelistMutableObject"/>.
        /// </returns>
        protected override IHierarchicalCodelistMutableObject RetrieveDetails(IHierarchicalCodelistMutableObject artefact, long sysId)
        {
            this.GetCodelistRefs(sysId, artefact.CodelistRef);
            this.FillHierarchy(artefact, sysId);
            return artefact;
        }

        /// <summary>
        /// Build a string suitable for <see cref="ICodelistRefMutableObject.Alias"/> and
        ///     <see cref="ICodeRefMutableObject.CodelistAliasRef"/>
        /// </summary>
        /// <param name="id">
        /// The Codelist ID
        /// </param>
        /// <param name="agency">
        /// The Codelist Agency
        /// </param>
        /// <param name="version">
        /// The Codelist version
        /// </param>
        /// <returns>
        /// The build codelist ref alias.
        /// </returns>
        private static string BuildCodelistRefAlias(string id, string agency, string version)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}@{1}@{2}", id, agency, version).Replace(".", string.Empty);
        }

        /// <summary>
        /// Retrieve and populate the <see cref="IHierarchyMutableObject.HierarchicalCodeObjects"/> for the HIERARCHY with specified Primary KEY
        /// </summary>
        /// <param name="artefact">
        /// The <see cref="IHierarchyMutableObject"/> instance to populate
        /// </param>
        /// <param name="hid">
        /// The Hierarchy Mapping store Primary KEY
        /// </param>
        private void FillCodeRef(IHierarchyMutableObject artefact, long hid)
        {
            IDictionary<long, ICodeRefMutableObject> allItems = new ListDictionary<long, ICodeRefMutableObject>();
            var childItems = new Dictionary<long, long>();
            using (DbCommand command = this._itemCommandBuilder.Build(new ItemSqlQuery(this._codeRefQueryInfo, hid)))
            {
                using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
                {
                    int sysIdIdx = dataReader.GetOrdinal("SYSID");
                    int idIdx = dataReader.GetOrdinal("CODE_ID");
                    int parentIdx = dataReader.GetOrdinal("PARENT");
                    int nodeAliasIdIdx = dataReader.GetOrdinal("NodeAliasID");

                    //// TODO check how to set the version (it seems supported in v2.1)
                    int levelRefIdx = dataReader.GetOrdinal("LEVEL_REF");
                    int clidIdx = dataReader.GetOrdinal("CLID");
                    int clversionIdx = dataReader.GetOrdinal("CLVERSION");
                    int clagencyIdx = dataReader.GetOrdinal("CLAGENCY");
                    int validFromIdx = dataReader.GetOrdinal("VALID_FROM");
                    int validToIdx = dataReader.GetOrdinal("VALID_TO");
                    while (dataReader.Read())
                    {
                        var codeRef = new CodeRefMutableCore
                                          {
                                              CodelistAliasRef =
                                                  BuildCodelistRefAlias(
                                                      DataReaderHelper.GetString(dataReader, clidIdx), 
                                                      DataReaderHelper.GetString(dataReader, clagencyIdx), 
                                                      DataReaderHelper.GetString(dataReader, clversionIdx)), 
                                              Id = DataReaderHelper.GetString(dataReader, nodeAliasIdIdx), 
                                              CodeId = DataReaderHelper.GetString(dataReader, idIdx), 
                                              LevelReference = DataReaderHelper.GetString(dataReader, levelRefIdx), 
                                          };

                        if (string.IsNullOrWhiteSpace(codeRef.Id))
                        {
                            codeRef.Id = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", codeRef.CodelistAliasRef.Replace('@', '_'), '_', codeRef.CodeId);
                        }

                        DateTime? validFrom = DataReaderHelper.GetStringDate(dataReader, validFromIdx);
                        if (validFrom != null)
                        {
                            codeRef.ValidFrom = validFrom.Value;
                        }

                        DateTime? validTo = DataReaderHelper.GetStringDate(dataReader, validToIdx);
                        if (validTo != null)
                        {
                            codeRef.ValidTo = validTo.Value;
                        }

                        long sysId = DataReaderHelper.GetInt64(dataReader, sysIdIdx);
                        long parentItemId = DataReaderHelper.GetInt64(dataReader, parentIdx);
                        if (parentItemId > long.MinValue)
                        {
                            childItems.Add(sysId, parentItemId);
                        }

                        allItems.Add(sysId, codeRef);
                    }
                }
            }

            ICollection<KeyValuePair<long, ICodeRefMutableObject>> collection = allItems;
            foreach (KeyValuePair<long, ICodeRefMutableObject> item in collection)
            {
                long sysId = item.Key;
                ICodeRefMutableObject codeRef = item.Value;
                long parentItemId;
                if (childItems.TryGetValue(sysId, out parentItemId))
                {
                    // has parent
                    ICodeRefMutableObject parent = allItems[parentItemId];
                    parent.AddCodeRef(codeRef);
                }
                else
                {
                    // add only root elements
                    artefact.AddHierarchicalCode(codeRef);
                }
            }
        }

        /// <summary>
        /// Retrieve and populate the <see cref="IHierarchicalCodelistMutableObject.Hierarchies"/> for the HCL with specified Primary KEY
        /// </summary>
        /// <param name="hierarchicalCodelistBean">
        /// The <see cref="IHierarchicalCodelistMutableObject"/> to populate
        /// </param>
        /// <param name="sysId">
        /// The HCL Mapping store Primary KEY
        /// </param>
        private void FillHierarchy(IHierarchicalCodelistMutableObject hierarchicalCodelistBean, long sysId)
        {
            // ReSharper restore SuggestBaseTypeForParameter
            var sysIdArtefacts = new Dictionary<long, IHierarchyMutableObject>();
            var itemSqlQuery = new ItemSqlQuery(this._hierarchyQueryInfo, sysId);
            using (DbCommand command = this._itemCommandBuilder.Build(itemSqlQuery))
            {
                using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
                {
                    int sysIdIdx = dataReader.GetOrdinal("SYSID");
                    int idIdx = dataReader.GetOrdinal("ID");

                    int txtIdx = dataReader.GetOrdinal("TEXT");
                    int langIdx = dataReader.GetOrdinal("LANGUAGE");
                    int typeIdx = dataReader.GetOrdinal("TYPE");
                    while (dataReader.Read())
                    {
                        long hierarchySysId = DataReaderHelper.GetInt64(dataReader, sysIdIdx);
                        IHierarchyMutableObject artefact;
                        if (!sysIdArtefacts.TryGetValue(hierarchySysId, out artefact))
                        {
                            artefact = new HierarchyMutableCore { Id = DataReaderHelper.GetString(dataReader, idIdx) };

                            sysIdArtefacts.Add(hierarchySysId, artefact);
                        }

                        ReadLocalisedString(artefact, typeIdx, txtIdx, langIdx, dataReader);
                    }
                }
            }

            foreach (KeyValuePair<long, IHierarchyMutableObject> sysIdArtefact in sysIdArtefacts)
            {
                long hid = sysIdArtefact.Key;
                IHierarchyMutableObject artefact = sysIdArtefact.Value;

                this.FillLevel(artefact, hid);
                this.FillCodeRef(artefact, hid);
                hierarchicalCodelistBean.AddHierarchies(artefact);
            }
        }

        /// <summary>
        /// Retrieve and populate the <see cref="IHierarchyMutableObject.ChildLevel"/> for the HIERARCHY with specified Primary KEY
        /// </summary>
        /// <param name="hierarchy">
        /// the <see cref="IHierarchyMutableObject"/> to populate
        /// </param>
        /// <param name="sysId">
        /// The HIERARCHY Mapping store Primary KEY
        /// </param>
        private void FillLevel(IHierarchyMutableObject hierarchy, long sysId)
        {
            var sysIdArtefacts = new Dictionary<long, ILevelMutableObject>();

            // holds the next level id to level id map. It contains all but the first level (with order == 1)
            var orderChain = new Dictionary<long, long>();

            using (DbCommand command = this._itemCommandBuilder.Build(new ItemSqlQuery(this._levelQueryInfo, sysId)))
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
                        long levelId = DataReaderHelper.GetInt64(dataReader, sysIdIdx);
                        ILevelMutableObject artefact;
                        if (!sysIdArtefacts.TryGetValue(levelId, out artefact))
                        {
                            artefact = new LevelMutableCore { Id = DataReaderHelper.GetString(dataReader, idIdx) };

                            long nextLevelId = DataReaderHelper.GetInt64(dataReader, parentIdx);

                            // TODO CodingType when MSDB supports it
                            if (nextLevelId > long.MinValue)
                            {
                                orderChain[levelId] = nextLevelId;
                            }

                            sysIdArtefacts.Add(levelId, artefact);
                        }

                        ReadLocalisedString(artefact, typeIdx, txtIdx, langIdx, dataReader);
                    }
                }
            }

            foreach (KeyValuePair<long, ILevelMutableObject> level in sysIdArtefacts)
            {
                long nextLevelId;
                if (orderChain.TryGetValue(level.Key, out nextLevelId))
                {
                    ILevelMutableObject nextLevel;
                    if (sysIdArtefacts.TryGetValue(nextLevelId, out nextLevel))
                    {
                        nextLevel.ChildLevel = level.Value;
                    }
                }
                else
                {
                    hierarchy.ChildLevel = level.Value;

                    //// TODO FIXME common api sets this to true for SDMX v2.0 input if levels exist.
                    hierarchy.FormalLevels = true;
                }
            }
        }

        /// <summary>
        /// Get the CodelistRef(s) for the given HCL sys id.
        /// </summary>
        /// <param name="sysId">
        /// The HCL primary key value in Mapping Store
        /// </param>
        /// <param name="codelistRefs">
        /// The collection of <see cref="ICodelistRefMutableObject"/> to add the codelist references
        /// </param>
        private void GetCodelistRefs(long sysId, ICollection<ICodelistRefMutableObject> codelistRefs)
        {
            using (DbCommand command = this._itemCommandBuilder.Build(new ItemSqlQuery(this._codelistRefQueryInfo, sysId)))
            using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
            {
                int idField = dataReader.GetOrdinal("ID");
                int agencyField = dataReader.GetOrdinal("AGENCY");
                int versionField = dataReader.GetOrdinal("VERSION");
                while (dataReader.Read())
                {
                    var item = new StructureReferenceImpl(_codelistType)
                                   {
                                       MaintainableId = DataReaderHelper.GetString(dataReader, idField), 
                                       Version = DataReaderHelper.GetString(dataReader, versionField), 
                                       AgencyId = DataReaderHelper.GetString(dataReader, agencyField)
                                   };
                    ICodelistRefMutableObject codelistRef = new CodelistRefMutableCore();
                    codelistRef.CodelistReference = item;
                    codelistRef.Alias = BuildCodelistRefAlias(item.MaintainableId, item.AgencyId, item.Version);

                    codelistRefs.Add(codelistRef);
                }
            }
        }

        /// <summary>
        /// Retrieve the <see cref="IHierarchicalCodelistMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        /// The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="queryInfo">
        /// The query Info.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IHierarchicalCodelistMutableObject}"/>.
        /// </returns>
        private ISet<IHierarchicalCodelistMutableObject> HierarchicalCodelistMutableObjects(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, SqlQueryInfo queryInfo)
        {
            var artefactSqlQuery = new ArtefactSqlQuery(queryInfo, maintainableRef);

            return this.RetrieveArtefacts(artefactSqlQuery, detail);
        }

        #endregion
    }
}