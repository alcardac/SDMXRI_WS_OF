// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArtefactRetrieverEngine.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The base artefact retriever engine.
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

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// The base artefact retriever engine.
    /// </summary>
    /// <typeparam name="T">
    /// The <see cref="IMaintainableMutableObject"/> based type
    /// </typeparam>
    internal abstract class ArtefactRetrieverEngine<T> : BaseRetrievalEngine, IRetrievalEngine<T>
        where T : IMaintainableMutableObject
    {
        #region Static Fields

        /// <summary>
        ///     The _log.
        /// </summary>
        // ReSharper disable StaticFieldInGenericType
        private static readonly ILog _log = LogManager.GetLogger(typeof(ArtefactRetrieverEngine<T>));

        #endregion

        // ReSharper restore StaticFieldInGenericType
        #region Fields

        /// <summary>
        ///     The artefact command builder
        /// </summary>
        private readonly ArtefactCommandBuilder _artefactCommandBuilder;

        /// <summary>
        ///     The artefact parents SQL builder.
        /// </summary>
        private readonly ArtefactParentsSqlBuilder _artefactParentsSqlBuilder;

        /// <summary>
        ///     The artefact SQL builder.
        /// </summary>
        private readonly ArtefactSqlBuilder _artefactSqlBuilder;

        /// <summary>
        ///     The artefact SQL builder for latest dataflows.
        /// </summary>
        private readonly ArtefactSqlBuilder _latestArtefactSqlBuilder;

        /// <summary>
        ///     This field holds the Mapping Store Database object.
        /// </summary>
        private readonly Database _mappingStoreDb;

        /// <summary>
        ///     The SQL query info.
        /// </summary>
        private readonly SqlQueryInfo _sqlQueryInfo;

        /// <summary>
        ///     The SQL query info.
        /// </summary>
        private readonly SqlQueryInfo _sqlQueryInfoLatest;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtefactRetrieverEngine{T}"/> class.
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
        /// <paramref name="mappingStoreDb"/> is null.
        /// </exception>
        protected ArtefactRetrieverEngine(Database mappingStoreDb, TableInfo tableInfo, string orderBy = null)
        {
            if (mappingStoreDb == null)
            {
                throw new ArgumentNullException("mappingStoreDb");
            }

            this._mappingStoreDb = mappingStoreDb;
            this._artefactCommandBuilder = new ArtefactCommandBuilder(this._mappingStoreDb);
            if (tableInfo != null)
            {
                this._latestArtefactSqlBuilder = new ArtefactSqlBuilder(orderBy, VersionQueryType.Latest);
                this._artefactSqlBuilder = new ArtefactSqlBuilder(orderBy);
                this._sqlQueryInfo = this._artefactSqlBuilder.Build(tableInfo);
                this._sqlQueryInfoLatest = this._latestArtefactSqlBuilder.Build(tableInfo);
                this._artefactParentsSqlBuilder = new ArtefactParentsSqlBuilder(tableInfo);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the artefact <see cref="SqlQueryInfo" /> builder.
        /// </summary>
        protected ArtefactSqlBuilder ArtefactSqlBuilder
        {
            get
            {
                return this._artefactSqlBuilder;
            }
        }

        /// <summary>
        ///     Gets the command builder.
        /// </summary>
        protected virtual ArtefactCommandBuilder CommandBuilder
        {
            get
            {
                return this._artefactCommandBuilder;
            }
        }

        /// <summary>
        ///     Gets the artefact <see cref="SqlQueryInfo" /> builder for latest artefacts.
        /// </summary>
        protected ArtefactSqlBuilder LatestArtefactSqlBuilder
        {
            get
            {
                return this._latestArtefactSqlBuilder;
            }
        }

        /// <summary>
        ///     Gets the Mapping Store Database object.
        /// </summary>
        protected Database MappingStoreDb
        {
            get
            {
                return this._mappingStoreDb;
            }
        }

        /// <summary>
        ///     Gets the SQL query info for all versions
        /// </summary>
        protected SqlQueryInfo SqlQueryInfoForAll
        {
            get
            {
                return this._sqlQueryInfo;
            }
        }

        /// <summary>
        ///     Gets the SQL query info for latest versions
        /// </summary>
        protected virtual SqlQueryInfo SqlQueryInfoForLatest
        {
            get
            {
                return this._sqlQueryInfoLatest;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Retrieve the <see cref="IMaintainableMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="versionConstraints">A value indicating the version criteria.</param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        public abstract ISet<T> Retrieve(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, VersionQueryType versionConstraints);

        /// <summary>
        /// Retrieve the set of <see cref="IMaintainableMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        /// The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="versionConstraints">
        /// A value indicating the version criteria.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        public virtual ISet<T> Retrieve(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, VersionQueryType versionConstraints, IList<IMaintainableRefObject> allowedDataflows)
        {
            return this.Retrieve(maintainableRef, detail, versionConstraints);
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
        public virtual ISet<T> RetrieveFromReferenced(IStructureReference referencedStructure, ComplexStructureQueryDetailEnumType detail)
        {
            return this.RetrieveFromReferencedInternal(referencedStructure, detail, query => this.CommandBuilder.Build(query), this.RetrieveDetails);
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
        public virtual ISet<T> RetrieveFromReferenced(IStructureReference referencedStructure, ComplexStructureQueryDetailEnumType detail, IList<IMaintainableRefObject> allowedDataflows)
        {
            return this.RetrieveFromReferenced(referencedStructure, detail);
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
        /// <param name="allowedDataflows">
        ///     The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableMutableObject"/>.
        /// </returns>
        public virtual T RetrieveLatest(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, IList<IMaintainableRefObject> allowedDataflows)
        {
            return this.RetrieveLatest(maintainableRef, detail);
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
        public abstract T RetrieveLatest(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail);

        #endregion

        #region Methods

        /// <summary>
        ///     Create a new instance of <typeparamref name="T" />.
        /// </summary>
        /// <returns>
        ///     The <typeparamref name="T" />.
        /// </returns>
        protected abstract T CreateArtefact();

        /// <summary>
        /// Returns the referenced from inner joins to use with <see cref="ArtefactParentsSqlBuilder"/>
        /// </summary>
        /// <param name="structureEnumType">
        /// The structure  type.
        /// </param>
        /// <returns>
        /// The referenced from inner joins
        /// </returns>
        protected virtual string GetReferencedFromInnerJoins(SdmxStructureEnumType structureEnumType)
        {
            return null;
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
        protected virtual void HandleArtefactExtraFields(T artefact, IDataReader reader)
        {
        }

        /// <summary>
        /// Retrieve the common <see cref="IMaintainableMutableObject"/> information from Mapping Store ARTEFACT table. This method does not retrieve the Names and Description
        /// </summary>
        /// <param name="sqlQuery">
        /// The SQL Query for artefacts
        /// </param>
        /// <param name="detail">
        /// The structure query detail
        /// </param>
        /// <param name="commandBuilder">
        /// The command Builder.
        /// </param>
        /// <param name="retrieveDetails">
        /// The method to retrieve details of the artefacts
        /// </param>
        /// <param name="extraFields">
        /// The extra Fields.
        /// </param>
        /// <returns>
        /// A dictionary with key the primary key in Mapping Store
        /// </returns>
        protected ISet<T> RetrieveArtefacts(ArtefactSqlQuery sqlQuery, ComplexStructureQueryDetailEnumType detail, Func<ArtefactSqlQuery, DbCommand> commandBuilder = null, Func<T, long, T> retrieveDetails = null, Action<T, IDataReader> extraFields = null)
        {
            commandBuilder = commandBuilder ?? this.CommandBuilder.Build;
            retrieveDetails = retrieveDetails ?? this.RetrieveDetails;
            var artefactPkPairs = new List<KeyValuePair<T, long>>();
            using (DbCommand command = commandBuilder(sqlQuery))
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, "Executing Query: '{0}' with '{1}'", command.CommandText, sqlQuery.MaintainableRef);
                using (IDataReader dataReader = this._mappingStoreDb.ExecuteReader(command))
                {
                    int sysIdIdx = dataReader.GetOrdinal("SYSID");
                    int idIdx = dataReader.GetOrdinal("ID");
                    int version1Idx = dataReader.GetOrdinal("VERSION");
                    int agencyIdx = dataReader.GetOrdinal("AGENCY");
                    int validFromIdx = dataReader.GetOrdinal("VALID_FROM");
                    int validToIdx = dataReader.GetOrdinal("VALID_TO");
                    int isFinalIdx = dataReader.GetOrdinal("IS_FINAL");
                    int txtIdx = dataReader.GetOrdinal("TEXT");
                    int langIdx = dataReader.GetOrdinal("LANGUAGE");
                    int typeIdx = dataReader.GetOrdinal("TYPE");
                    var artefactMap = new Dictionary<long, T>();
                    while (dataReader.Read())
                    {
                        long sysId = DataReaderHelper.GetInt64(dataReader, sysIdIdx);
                        T artefact;
                        if (!artefactMap.TryGetValue(sysId, out artefact))
                        {
                            artefact = this.CreateArtefact();
                            artefact.FinalStructure = DataReaderHelper.GetTristate(dataReader, isFinalIdx);
                            artefact.EndDate = DataReaderHelper.GetStringDate(dataReader, validToIdx);
                            artefact.StartDate = DataReaderHelper.GetStringDate(dataReader, validFromIdx);
                            artefact.Version = DataReaderHelper.GetString(dataReader, version1Idx);
                            artefact.AgencyId = DataReaderHelper.GetString(dataReader, agencyIdx);
                            artefact.Id = DataReaderHelper.GetString(dataReader, idIdx);
                            this.HandleArtefactExtraFields(artefact, dataReader);

                            if (extraFields != null)
                            {
                                extraFields(artefact, dataReader);
                            }

                            artefactPkPairs.Add(new KeyValuePair<T, long>(artefact, sysId));
                            artefactMap.Add(sysId, artefact);
                        }

                        if (!artefact.IsDefault())
                        {
                            ReadLocalisedString(artefact, typeIdx, txtIdx, langIdx, dataReader, detail);
                        }
                    }
                }
            }

            if (artefactPkPairs.Count < 1)
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, "No artefacts retrieved for : '{0}'", sqlQuery.MaintainableRef);
                return new HashSet<T>();
            }

            return HandleDetailLevel(detail, retrieveDetails, artefactPkPairs);
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
        /// The <typeparamref name="T"/>
        /// </returns>
        protected abstract T RetrieveDetails(T artefact, long sysId);

        /// <summary>
        /// Generic method for retrieving <see cref="IStructureReference"/> based objects
        /// </summary>
        /// <param name="outList">
        /// The output list
        /// </param>
        /// <param name="command">
        /// The current <see cref="DbCommand"/>
        /// </param>
        /// <param name="structureType">
        /// The structure Type.
        /// </param>
        protected void RetrieveRef(ICollection<IStructureReference> outList, DbCommand command, SdmxStructureEnumType structureType)
        {
            _log.DebugFormat("RetrieveRef of {0} SQL : {1}", structureType, command.CommandText);
            using (IDataReader dataReader = this._mappingStoreDb.ExecuteReader(command))
            {
                int idField = dataReader.GetOrdinal("ID");
                int agencyField = dataReader.GetOrdinal("AGENCY");
                int versionField = dataReader.GetOrdinal("VERSION");
                while (dataReader.Read())
                {
                    var item = new StructureReferenceImpl(SdmxStructureType.GetFromEnum(structureType))
                                   {
                                       MaintainableId = DataReaderHelper.GetString(dataReader, idField), 
                                       Version = DataReaderHelper.GetString(dataReader, versionField), 
                                       AgencyId = DataReaderHelper.GetString(dataReader, agencyField)
                                   };
                    outList.Add(item);
                }
            }
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
        /// <param name="commandBuilder">
        ///     The command Builder.
        /// </param>
        /// <param name="retrieveDetails">The method that retrieves the details of the artefact.</param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        protected ISet<T> RetrieveFromReferencedInternal(IStructureReference referencedStructure, ComplexStructureQueryDetailEnumType detail, Func<ArtefactSqlQuery, DbCommand> commandBuilder, Func<T, long, T> retrieveDetails)
        {
            if (referencedStructure == null)
            {
                throw new ArgumentNullException("referencedStructure");
            }

            string innerJoin = this.GetReferencedFromInnerJoins(referencedStructure.MaintainableStructureEnumType.EnumType);

            ISet<T> mutableObjects;
            if (innerJoin != null)
            {
                SqlQueryInfo sqlQueryInfo = this._artefactParentsSqlBuilder.Build(innerJoin);
                var artefactSqlQuery = new ArtefactSqlQuery(sqlQueryInfo, referencedStructure.MaintainableReference);
                mutableObjects = this.RetrieveArtefacts(artefactSqlQuery, detail, commandBuilder, retrieveDetails);
            }
            else
            {
                mutableObjects = new HashSet<T>();
            }

            return mutableObjects;
        }

        /// <summary>
        /// Handles the detail level.
        /// </summary>
        /// <param name="detail">The detail.</param>
        /// <param name="retrieveDetails">The retrieve details.</param>
        /// <param name="artefactPkPairs">The artefact - primary key pairs.</param>
        /// <returns>The artefacts.</returns>
        private static ISet<T> HandleDetailLevel(ComplexStructureQueryDetailEnumType detail, Func<T, long, T> retrieveDetails, IEnumerable<KeyValuePair<T, long>> artefactPkPairs)
        {
            ISet<T> artefacts = new HashSet<T>();
            foreach (var artefactPkPair in artefactPkPairs)
            {
                var artefact = artefactPkPair.Key;
                var sysId = artefactPkPair.Value;
                switch (detail)
                {
                    case ComplexStructureQueryDetailEnumType.MatchedItems:
                        break;
                    case ComplexStructureQueryDetailEnumType.CascadedMatchedItems:
                        break;
                    case ComplexStructureQueryDetailEnumType.Null:
                    case ComplexStructureQueryDetailEnumType.Full:
                        artefact = retrieveDetails(artefact, sysId);
                        break;
                    case ComplexStructureQueryDetailEnumType.Stub:
                        artefact.ExternalReference = SdmxObjectUtil.CreateTertiary(true);
                        artefact.Stub = true;
                        artefact.StructureURL = DefaultUri;
                        break;
                }

                if (!artefact.IsDefault())
                {
                    artefacts.Add(artefact);
                }
            }

            return artefacts;
        }
        #endregion
    }
}