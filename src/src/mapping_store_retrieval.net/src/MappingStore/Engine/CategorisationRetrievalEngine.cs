// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorisationRetrievalEngine.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The categorisation retrieval engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;

    using Estat.Sdmxsource.Extension.Constant;
    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Util.Objects;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     The categorisation retrieval engine.
    /// </summary>
    internal class CategorisationRetrievalEngine : ArtefactRetrieverEngine<ICategorisationMutableObject>
    {
        #region Static Fields

        /// <summary>
        ///     The log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(CategorisationRetrievalEngine));

        #endregion

        #region Fields

        /// <summary>
        ///     The authorization reference command builder.
        /// </summary>
        private readonly AuthReferenceCommandBuilder _authReferenceCommandBuilder;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorisationRetrievalEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <param name="filter">
        /// The dataflow PRODUCTION filter.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null.
        /// </exception>
        public CategorisationRetrievalEngine(Database mappingStoreDb, DataflowFilter filter)
            : base(mappingStoreDb, CategorisationConstant.TableInfo)
        {
            this._authReferenceCommandBuilder = new AuthReferenceCommandBuilder(mappingStoreDb, filter);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Retrieve the <see cref="IDataStructureMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        /// The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="versionConstraints">
        /// The version query type.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IDataStructureMutableObject}"/>.
        /// </returns>
        public override ISet<ICategorisationMutableObject> Retrieve(
            IMaintainableRefObject maintainableRef,
            ComplexStructureQueryDetailEnumType detail,
            VersionQueryType versionConstraints,
            IList<IMaintainableRefObject> allowedDataflows)
        {
            var sqlInfo = versionConstraints == VersionQueryType.Latest ? this.SqlQueryInfoForLatest : this.SqlQueryInfoForAll;
            var categorisations = this.GetCategorisations(maintainableRef, ComplexStructureQueryDetailEnumType.Full, sqlInfo, allowedDataflows);
            NormalizeDetailLevel(detail, categorisations);

            return categorisations;
        }

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
        /// The version query type.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        public override ISet<ICategorisationMutableObject> Retrieve(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, VersionQueryType versionConstraints)
        {
            return this.Retrieve(maintainableRef, detail, versionConstraints, null);
        }

        /// <summary>
        /// Retrieve the set of <see cref="IMaintainableMutableObject"/> from Mapping Store that references
        ///     <paramref name="referencedStructure"/>
        ///     .
        /// </summary>
        /// <param name="referencedStructure">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION. This is the referenced structure.
        /// </param>
        /// <param name="detail">
        /// The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="referencedStructure"/> is null.
        /// </exception>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        public override ISet<ICategorisationMutableObject> RetrieveFromReferenced(
            IStructureReference referencedStructure, ComplexStructureQueryDetailEnumType detail, IList<IMaintainableRefObject> allowedDataflows)
        {
            if (referencedStructure == null)
            {
                throw new ArgumentNullException("referencedStructure");
            }

            var dataflowCache = new Dictionary<long, IStructureReference>();
            var categoryCache = new Dictionary<long, IStructureReference>();

            this.RetrieveArtefactReference(dataflowCache, allowedDataflows);
            this.RetrieveCategoryReference(categoryCache);

            var retrieveFromReferencedInternal = this.RetrieveFromReferencedInternal(referencedStructure, ComplexStructureQueryDetailEnumType.Full, null, (o, l) => RetrieveReferences(o, l, l1 => GetReference(l1, dataflowCache), l1 => GetReference(l1, categoryCache)));
            NormalizeDetailLevel(detail, retrieveFromReferencedInternal);
            return retrieveFromReferencedInternal;
        }

        /// <summary>
        /// Retrieve the set of <see cref="IMaintainableMutableObject"/> from Mapping Store that references
        ///     <paramref name="referencedStructure"/>
        ///     .
        /// </summary>
        /// <param name="referencedStructure">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION. This is the referenced structure.
        /// </param>
        /// <param name="detail">
        /// The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="referencedStructure"/> is null.
        /// </exception>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        public override ISet<ICategorisationMutableObject> RetrieveFromReferenced(IStructureReference referencedStructure, ComplexStructureQueryDetailEnumType detail)
        {
            return this.RetrieveFromReferenced(referencedStructure, detail, null);
        }

        /// <summary>
        /// Retrieve the <see cref="ICategorisationMutableObject"/> with the latest version group by ID and AGENCY from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        /// The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{ICategorisationMutableObject}"/>.
        /// </returns>
        public override ICategorisationMutableObject RetrieveLatest(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail)
        {
            return this.RetrieveLatest(maintainableRef, detail, null);
        }

        /// <summary>
        /// Retrieve the <see cref="ICategorisationMutableObject"/> with the latest version group by ID and AGENCY from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        /// The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{ICategorisationMutableObject}"/>.
        /// </returns>
        public override ICategorisationMutableObject RetrieveLatest(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, IList<IMaintainableRefObject> allowedDataflows)
        {
            ISet<ICategorisationMutableObject> mutableObjects = this.Retrieve(maintainableRef, detail, VersionQueryType.Latest, allowedDataflows);

            ////return mutableObjects.GetOneOrNothing();
            //// HACK This needs to be fixed after including proper categorisation support
            return mutableObjects.FirstOrDefault();
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
        protected override void HandleArtefactExtraFields(ICategorisationMutableObject artefact, IDataReader reader)
        {
            base.HandleArtefactExtraFields(artefact, reader);
            var categoryOrder = DataReaderHelper.GetInt64(reader, "DC_ORDER");
            if (categoryOrder == long.MinValue)
            {
                categoryOrder = 0;
            }

            artefact.AddAnnotation(CustomAnnotationType.CategorySchemeNodeOrder.ToAnnotation<AnnotationMutableCore>(categoryOrder.ToString(CultureInfo.InvariantCulture)));
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Create a new instance of <see cref="ICategorisationMutableObject" />.
        /// </summary>
        /// <returns>
        ///     The <see cref="ICategorisationMutableObject" />.
        /// </returns>
        protected override ICategorisationMutableObject CreateArtefact()
        {
            return new CategorisationMutableCore();
        }

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
                case SdmxStructureEnumType.Category:
                case SdmxStructureEnumType.CategoryScheme:
                    innerJoin = CategorySchemeConstant.ReferencedByCategorisation;
                    break;
                default:
                    if (SdmxStructureType.GetFromEnum(structureEnumType).IsMaintainable)
                    {
                        innerJoin = CategorisationConstant.ReferencedByCategorisation;
                    }

                    break;
            }

            return innerJoin;
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
        /// The <see cref="ICategorisationMutableObject"/>.
        /// </returns>
        protected override ICategorisationMutableObject RetrieveDetails(ICategorisationMutableObject artefact, long sysId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Normalizes the detail level.
        /// </summary>
        /// <param name="detail">The detail.</param>
        /// <param name="categorisations">The categorisations.</param>
        private static void NormalizeDetailLevel(ComplexStructureQueryDetailEnumType detail, IEnumerable<ICategorisationMutableObject> categorisations)
        {
            foreach (var categorisation in categorisations)
            {
                switch (detail)
                {
                    case ComplexStructureQueryDetailEnumType.CompleteStub:
                        categorisation.ExternalReference = SdmxObjectUtil.CreateTertiary(true);
                        categorisation.Stub = true;
                        categorisation.StructureURL = DefaultUri;
                        categorisation.CategoryReference = null;
                        categorisation.StructureReference = null;
                        break;
                    case ComplexStructureQueryDetailEnumType.Stub:
                        categorisation.Descriptions.Clear();
                        goto case ComplexStructureQueryDetailEnumType.CompleteStub;
                }
            }
        }

        /// <summary>
        /// The get reference.
        /// </summary>
        /// <param name="sysId">
        /// The sys id.
        /// </param>
        /// <param name="cache">
        /// The cache.
        /// </param>
        /// <returns>
        /// The <see cref="IStructureReference"/>.
        /// </returns>
        private static IStructureReference GetReference(long sysId, IDictionary<long, IStructureReference> cache)
        {
            IStructureReference structureReference;
            return !cache.TryGetValue(sysId, out structureReference) ? null : structureReference;
        }

        /// <summary>
        /// The retrieve details.
        /// </summary>
        /// <param name="artefact">
        /// The artefact.
        /// </param>
        /// <param name="sysId">
        /// The sys id.
        /// </param>
        /// <param name="getStructureRef">
        /// The get dataflow reference method.
        /// </param>
        /// <param name="getCategory">
        /// The get category reference method.
        /// </param>
        /// <returns>
        /// The <see cref="ICategorisationMutableObject"/>.
        /// </returns>
        private static ICategorisationMutableObject RetrieveReferences(
            ICategorisationMutableObject artefact, long sysId, Func<long, IStructureReference> getStructureRef, Func<long, IStructureReference> getCategory)
        {
            var structureReference = getStructureRef(sysId);
            if (structureReference != null)
            {
                artefact.StructureReference = structureReference;
                artefact.CategoryReference = getCategory(sysId);
                return artefact;
            }

            return null;
        }

        /// <summary>
        /// Returns the categorisations.
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable ref.
        /// </param>
        /// <param name="detail">
        /// The detail.
        /// </param>
        /// <param name="sqlInfo">
        /// The SQL info.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{ICategorisationMutableObject}"/>.
        /// </returns>
        private ISet<ICategorisationMutableObject> GetCategorisations(
            IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, SqlQueryInfo sqlInfo, IList<IMaintainableRefObject> allowedDataflows)
        {
            var artefactSqlQuery = new ArtefactSqlQuery(sqlInfo, maintainableRef);

            var dataflowCache = new Dictionary<long, IStructureReference>();
            var categoryCache = new Dictionary<long, IStructureReference>();

            this.RetrieveArtefactReference(dataflowCache, allowedDataflows);
            this.RetrieveCategoryReference(categoryCache);

            return this.RetrieveArtefacts(artefactSqlQuery, detail, null, (o, l) => RetrieveReferences(o, l, l1 => GetReference(l1, dataflowCache), l1 => GetReference(l1, categoryCache)));
        }

        /// <summary>
        /// The retrieve artefact reference.
        /// </summary>
        /// <param name="artefactCache">
        /// The artefact cache.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed dataflows.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// There is an error in the SQL Query in <see cref="CategorisationConstant.ArtefactRefQueryFormat"/>
        /// </exception>
        private void RetrieveArtefactReference(IDictionary<long, IStructureReference> artefactCache, IList<IMaintainableRefObject> allowedDataflows)
        {
            using (var command = this._authReferenceCommandBuilder.Build(new ReferenceSqlQuery(CategorisationConstant.ArtefactReference), allowedDataflows))
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, "Executing Query: '{0}'", command.CommandText);
                using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
                {
                    //// A.ID, A.VERSION, A.AGENCY, T.STYPE
                    int catnIdx = dataReader.GetOrdinal("CATN_ID");
                    int idIdx = dataReader.GetOrdinal("ID");
                    int versionIdx = dataReader.GetOrdinal("VERSION");
                    int agencyIdx = dataReader.GetOrdinal("AGENCY");
                    int stypeIdx = dataReader.GetOrdinal("STYPE");

                    while (dataReader.Read())
                    {
                        long catn = DataReaderHelper.GetInt64(dataReader, catnIdx);
                        string id = DataReaderHelper.GetString(dataReader, idIdx);
                        string agency = DataReaderHelper.GetString(dataReader, agencyIdx);
                        string version = DataReaderHelper.GetString(dataReader, versionIdx);
                        string stype = DataReaderHelper.GetString(dataReader, stypeIdx);
                        SdmxStructureEnumType structureType;
                        if (Enum.TryParse(stype, true, out structureType))
                        {
                            var structureReference = new StructureReferenceImpl(agency, id, version, structureType);
                            artefactCache.Add(catn, structureReference);
                        }
                        else
                        {
                            var message = string.Format(CultureInfo.InvariantCulture, "Error could not convert {0} to SdmxStructureEnumType", stype);
                            _log.Error(message);
                            throw new InvalidOperationException(message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The retrieve category reference.
        /// </summary>
        /// <param name="categoryCache">
        /// The category cache.
        /// </param>
        private void RetrieveCategoryReference(IDictionary<long, IStructureReference> categoryCache)
        {
            using (var command = this.MappingStoreDb.GetSqlStringCommand(CategorisationConstant.CategoryRefQueryFormat))
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, "Executing Query: '{0}'", command.CommandText);
                using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
                {
                    ////  C.CATN_ID, A.ID, A.VERSION, A.AGENCY, I.ID as CATID
                    int catnIdx = dataReader.GetOrdinal("CATN_ID");
                    int idIdx = dataReader.GetOrdinal("ID");
                    int versionIdx = dataReader.GetOrdinal("VERSION");
                    int agencyIdx = dataReader.GetOrdinal("AGENCY");
                    int catidIdx = dataReader.GetOrdinal("CATID");

                    while (dataReader.Read())
                    {
                        long catn = DataReaderHelper.GetInt64(dataReader, catnIdx);
                        string id = DataReaderHelper.GetString(dataReader, idIdx);
                        string agency = DataReaderHelper.GetString(dataReader, agencyIdx);
                        string version = DataReaderHelper.GetString(dataReader, versionIdx);
                        string catid = DataReaderHelper.GetString(dataReader, catidIdx);

                        var structureReference = new StructureReferenceImpl(agency, id, version, SdmxStructureEnumType.Category, catid);
                        categoryCache.Add(catn, structureReference);
                    }
                }
            }
        }

        #endregion
    }
}