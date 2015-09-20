// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartialCodeListRetrievalEngine.cs" company="Eurostat">
//   Date Created : 2013-04-15
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The partial code list retrieval engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;

    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The partial code list retrieval engine.
    /// </summary>
    internal class PartialCodeListRetrievalEngine : CodeListRetrievalEngine
    {
        #region Fields

        /// <summary>
        ///     The _partial codes command builder.
        /// </summary>
        private readonly PartialCodesCommandBuilder _partialCodesCommandBuilder;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PartialCodeListRetrievalEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null
        /// </exception>
        public PartialCodeListRetrievalEngine(Database mappingStoreDb)
            : base(mappingStoreDb)
        {
            this._partialCodesCommandBuilder = new PartialCodesCommandBuilder(mappingStoreDb);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Retrieve the <see cref="ICodelistMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="dataflowRef">
        ///     The dataflow Ref.
        /// </param>
        /// <param name="conceptId">
        ///     The concept Id.
        /// </param>
        /// <param name="isTranscoded">
        ///     The is Transcoded.
        /// </param>
        /// <param name="allowedDataflows">The allowed dataflows.</param>
        /// <returns>
        /// The <see cref="ISet{ICodelistMutableObject}"/>.
        /// </returns>
        public ISet<ICodelistMutableObject> Retrieve(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, IMaintainableRefObject dataflowRef, string conceptId, bool isTranscoded, IList<IMaintainableRefObject> allowedDataflows)
        {
            var sqlQuery = new ArtefactSqlQuery(this.SqlQueryInfoForAll, maintainableRef);
            return this.RetrieveArtefacts(sqlQuery, detail, retrieveDetails: (o, l) => this.FillCodes(o, l, dataflowRef, conceptId, isTranscoded, allowedDataflows));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the Codes
        /// </summary>
        /// <param name="itemScheme">
        ///     The parent <see cref="ICodelistMutableObject"/>
        /// </param>
        /// <param name="parentSysId">
        ///     The parent ItemScheme primary key in Mapping Store
        /// </param>
        /// <param name="dataflowRef">
        ///     The dataflow Ref.
        /// </param>
        /// <param name="conceptId">
        ///     The concept Id.
        /// </param>
        /// <param name="isTranscoded">
        ///     Set to true if component is transcoded; otherwise false
        /// </param>
        /// <param name="allowedDataflows">The allowed dataflows.</param>
        /// <returns>
        /// The <see cref="ICodelistMutableObject"/>.
        /// </returns>
        private ICodelistMutableObject FillCodes(ICodelistMutableObject itemScheme, long parentSysId, IMaintainableRefObject dataflowRef, string conceptId, bool isTranscoded, IList<IMaintainableRefObject> allowedDataflows)
        {
            var allItems = new Dictionary<long, ICodeMutableObject>();
            var orderedItems = new List<KeyValuePair<long, ICodeMutableObject>>();
            var childItems = new Dictionary<long, long>();
            var sqlQuery = new PartialCodesSqlQuery(isTranscoded ? CodeListConstant.TranscodedSqlQueryInfo : CodeListConstant.LocalCodeSqlQueryInfo, parentSysId)
                               {
                                   ConceptId = conceptId, 
                                   DataflowReference = dataflowRef
                               };

            using (DbCommand command = this._partialCodesCommandBuilder.Build(sqlQuery, allowedDataflows))
            {
                this.ReadItems(allItems, orderedItems, command, childItems);
            }

            this.FillParentItems(itemScheme, childItems, allItems, orderedItems);

            return itemScheme;
        }

        #endregion
    }
}