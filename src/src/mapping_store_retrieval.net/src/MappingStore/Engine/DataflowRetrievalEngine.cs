// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataflowRetrievalEngine.cs" company="Eurostat">
//   Date Created : 2013-02-12
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The dataflow retrieval engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Globalization;

    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.MetadataStructure;
    using Estat.Sdmxsource.Extension.Constant;

    /// <summary>
    ///     The dataflow retrieval engine.
    /// </summary>
    internal class DataflowRetrievalEngine : ArtefactRetrieverEngine<IDataflowMutableObject>
    {
        #region Fields

        /// <summary>
        /// The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(DataflowRetrievalEngine));

        /// <summary>
        ///     The artefact command builder
        /// </summary>
        private readonly DataflowCommandBuilder _artefactCommandBuilder;

        /// <summary>
        ///     The _key family ref command builder.
        /// </summary>
        private readonly ItemCommandBuilder _keyFamilyRefCommandBuilder;

        /// <summary>
        ///     The _key family ref query.
        /// </summary>
        private readonly SqlQueryInfo _keyFamilyRefQuery;

        /// <summary>
        ///     The SQL query info for getting the latest version only.
        /// </summary>
        private readonly SqlQueryInfo _sqlQueryInfoLatest;

        /// <summary>
        ///     The SQL query info for getting the latest version only.
        /// </summary>
        private readonly SqlQueryInfo _sqlQueryInfo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataflowRetrievalEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <param name="filter">
        /// The filter. (Optional defaults to <see cref="DataflowFilter.Production"/>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null.
        /// </exception>
        public DataflowRetrievalEngine(Database mappingStoreDb, DataflowFilter filter = DataflowFilter.Production)
            : base(mappingStoreDb, DataflowConstant.TableInfo)
        {
            this._artefactCommandBuilder = new DataflowCommandBuilder(mappingStoreDb, filter);
            var sqlQueryBuilder = new ReferencedSqlQueryBuilder(this.MappingStoreDb, null);
            this._keyFamilyRefQuery = sqlQueryBuilder.Build(DataflowConstant.KeyFamilyRefQueryFormat);
            this._keyFamilyRefCommandBuilder = new ItemCommandBuilder(this.MappingStoreDb);
            ArtefactSqlBuilder dataflowBuilder;
            ArtefactSqlBuilder latestBuilder;
            switch (filter)
            {
                case DataflowFilter.Any:
                    dataflowBuilder = new ArtefactSqlBuilder();
                    latestBuilder = new ArtefactSqlBuilder(null, VersionQueryType.Latest);
                    break;
                case DataflowFilter.Production:
                    dataflowBuilder = new ArtefactSqlBuilder(null, VersionQueryType.All, DataflowConstant.ProductionWhereLatestClause);
                    latestBuilder = new ArtefactSqlBuilder(null, VersionQueryType.Latest, DataflowConstant.ProductionWhereLatestClause);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("filter");
            }

            this._sqlQueryInfo = dataflowBuilder.Build(DataflowConstant.TableInfo);
            this._sqlQueryInfoLatest = latestBuilder.Build(DataflowConstant.TableInfo);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the command builder.
        /// </summary>
        protected override ArtefactCommandBuilder CommandBuilder
        {
            get
            {
                return this._artefactCommandBuilder;
            }
        }

        /// <summary>
        ///     Gets the SQL query info for latest versions
        /// </summary>
        protected override SqlQueryInfo SqlQueryInfoForLatest
        {
            get
            {
                return this._sqlQueryInfoLatest;
            }
        }

        /// <summary>
        /// Retrieve the <see cref="IMaintainableMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="versionConstraints">
        /// The version criteria
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        public override ISet<IDataflowMutableObject> Retrieve(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, VersionQueryType versionConstraints)
        {
            return this.Retrieve(maintainableRef, detail, versionConstraints, null);
        }

        /// <summary>
        /// Retrieve the <see cref="IMaintainableMutableObject"/> with the latest version group by ID and AGENCY from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableMutableObject"/>.
        /// </returns>
        public override IDataflowMutableObject RetrieveLatest(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail)
        {
            return this.RetrieveLatest(maintainableRef, detail, null);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Retrieve the <see cref="IDataflowMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="versionConstraints">
        /// The version query type
        /// </param>
        /// <param name="allowedDataflows">
        ///     The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IDataflowMutableObject}"/>.
        /// </returns>
        public override ISet<IDataflowMutableObject> Retrieve(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, VersionQueryType versionConstraints, IList<IMaintainableRefObject> allowedDataflows)
        {
            var dataflowMutableObjects = new HashSet<IDataflowMutableObject>();
            var sqlQueryInfo = versionConstraints == VersionQueryType.Latest ? this._sqlQueryInfoLatest : this._sqlQueryInfo;
            var artefactSqlQuery = new ArtefactSqlQuery(sqlQueryInfo, maintainableRef);

            // do a security check. 
            return !SecurityHelper.Contains(allowedDataflows, maintainableRef) ? dataflowMutableObjects : this.RetrieveArtefacts(artefactSqlQuery, detail, query => this._artefactCommandBuilder.Build(query, allowedDataflows));
        }

        /// <summary>
        /// Retrieve the <see cref="IDataflowMutableObject"/> with the latest version group by ID and AGENCY from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="allowedDataflows">
        ///     The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IDataflowMutableObject}"/>.
        /// </returns>
        public override IDataflowMutableObject RetrieveLatest(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, IList<IMaintainableRefObject> allowedDataflows)
        {
            var dataflowMutableObjects = new HashSet<IDataflowMutableObject>();

            var artefactSqlQuery = new ArtefactSqlQuery(this._sqlQueryInfoLatest, maintainableRef);

            // do a security check. 
            var dataflows = !SecurityHelper.Contains(allowedDataflows, maintainableRef) ? dataflowMutableObjects : this.RetrieveArtefacts(artefactSqlQuery, detail, query => this._artefactCommandBuilder.Build(query, allowedDataflows));
            return dataflows.GetOneOrNothing();
        }

        /// <summary>
        /// Retrieve the set of <see cref="IMaintainableMutableObject"/> from Mapping Store that references
        ///     <paramref name="referencedStructure"/>
        ///     .
        /// </summary>
        /// <param name="referencedStructure">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION. This is the referenced structure.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="allowedDataflows">
        ///     The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        public override ISet<IDataflowMutableObject> RetrieveFromReferenced(IStructureReference referencedStructure, ComplexStructureQueryDetailEnumType detail, IList<IMaintainableRefObject> allowedDataflows)
        {
            return this.RetrieveFromReferencedInternal(referencedStructure, detail, query => this._artefactCommandBuilder.Build(query, allowedDataflows), this.RetrieveDetails);
        }

        /// <summary>
        /// Retrieve the set of <see cref="IMaintainableMutableObject"/> from Mapping Store that references
        ///     <paramref name="referencedStructure"/>
        ///     .
        /// </summary>
        /// <param name="referencedStructure">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION. This is the referenced structure.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="referencedStructure"/> is null.
        /// </exception>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        public override ISet<IDataflowMutableObject> RetrieveFromReferenced(IStructureReference referencedStructure, ComplexStructureQueryDetailEnumType detail)
        {
            return this.RetrieveFromReferenced(referencedStructure, detail, null);
        }

        #endregion

        /// <summary>
        /// Handles the extra fields of Dataflows
        /// </summary>
        /// <param name="artefact">
        ///     The maintainable reference to add the annotations to
        /// </param>
        /// <param name="reader">
        ///  reader
        /// </param>
        protected override void HandleArtefactExtraFields(IDataflowMutableObject artefact, System.Data.IDataReader reader)
        {
            base.HandleArtefactExtraFields(artefact, reader);

            // The referenced from queries do not include PRODUCTION field. So we need to check. 
            // TODO pass instead the extraFields method to RetrieveArtefacts 
            if (!reader.HasFieldName("PRODUCTION"))
            {
                return;
            }

            var isProduction = DataReaderHelper.GetInt32(reader, "PRODUCTION");

            if (isProduction == 0)
                artefact.SetNonProduction();
        }

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
                    case SdmxStructureEnumType.Dsd:
                    innerJoin = DsdConstant.ReferencedByDataflow;
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
        /// The <see cref="IDataflowMutableObject"/>.
        /// </returns>
        protected override IDataflowMutableObject RetrieveDetails(IDataflowMutableObject artefact, long sysId)
        {
            artefact.DataStructureRef = this.GetKeyFamilyRef(sysId);
            if (artefact.DataStructureRef == null)
            {
                var message = string.Format(CultureInfo.InvariantCulture, "Cannot find DataStructureRef for dataflow {0}", artefact.Id);
                _log.ErrorFormat(message);
                throw new MappingStoreException(message);
            }

            return artefact;
        }

        /// <summary>
        ///     Create a new instance of <see cref="IDataflowMutableObject" />.
        /// </summary>
        /// <returns>
        ///     The <see cref="IDataflowMutableObject" />.
        /// </returns>
        protected override IDataflowMutableObject CreateArtefact()
        {
            return new DataflowMutableCore();
        }

        /// <summary>
        /// Get KeyFamilyRef for the given dataflow
        /// </summary>
        /// <param name="dataflowId">
        /// The dataflow primary key value in Mapping Store
        /// </param>
        /// <returns>
        /// A KeyFamilyRef object
        /// </returns>
        private IStructureReference GetKeyFamilyRef(long dataflowId)
        {
            var list = new List<IStructureReference>();

            using (DbCommand command = this._keyFamilyRefCommandBuilder.Build(new ItemSqlQuery(this._keyFamilyRefQuery, dataflowId)))
            {
                this.RetrieveRef(list, command, SdmxStructureEnumType.Dsd);
            }

            return list.Count > 0 ? list[0] : null;
        }

        #endregion
    }
}