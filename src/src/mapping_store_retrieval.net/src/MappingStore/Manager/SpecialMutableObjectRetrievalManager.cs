// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpecialMutableObjectRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-04-15
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The special mutable object retrieval manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Manager
{
    using System.Collections.Generic;
    using System.Configuration;

    using Estat.Sdmxsource.Extension.Manager;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Engine;
    using Estat.Sri.MappingStoreRetrieval.Helper;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The special mutable object retrieval manager.
    /// </summary>
    public class SpecialMutableObjectRetrievalManager : ISpecialMutableObjectRetrievalManager
    {
        #region Fields

        /// <summary>
        ///     The partial code list retrieval engine.
        /// </summary>
        private readonly PartialCodeListRetrievalEngine _codeListRetrievalEngine;

        /// <summary>
        ///     The _subset codelist retrieval.
        /// </summary>
        private readonly SubsetCodelistRetrievalEngine _subsetCodelistRetrieval;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialMutableObjectRetrievalManager"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection string settings.
        /// </param>
        public SpecialMutableObjectRetrievalManager(ConnectionStringSettings connectionStringSettings)
        {
            var mappingStoreDb = new Database(connectionStringSettings);
            this._codeListRetrievalEngine = new PartialCodeListRetrievalEngine(mappingStoreDb);
            this._subsetCodelistRetrieval = new SubsetCodelistRetrievalEngine(mappingStoreDb);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Get a list of CodeList matching the given id, version and agencyID and dataflow/component information.
        /// </summary>
        /// <param name="codelistReference">
        /// The codelist reference
        /// </param>
        /// <param name="dataflowReference">
        /// The dataflow reference
        /// </param>
        /// <param name="componentConceptRef">
        /// The component in the dataflow that this codelist is used and for which partial codes are requests.
        /// </param>
        /// <param name="isTranscoded">
        /// if true will get the codes from the transcoding rules. Otherwise from locally stored codes.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed dataflows.
        /// </param>
        /// <returns>
        /// The list of matched CodeListBean objects
        /// </returns>
        /// <remarks>
        /// I.e. codes used for the given dataflow.
        ///     It can look codes actually used from transcoding rule or from locally stored codes.
        ///     If any of the Codelist identification parameters is null or empty it will act as wildcard.
        /// </remarks>
        public ISet<ICodelistMutableObject> GetMutableCodelistObjects(
            IMaintainableRefObject codelistReference, IMaintainableRefObject dataflowReference, string componentConceptRef, bool isTranscoded, IList<IMaintainableRefObject> allowedDataflows)
        {
            if (SecurityHelper.Contains(allowedDataflows, dataflowReference))
            {
                return this._codeListRetrievalEngine.Retrieve(codelistReference, ComplexStructureQueryDetailEnumType.Full, dataflowReference, componentConceptRef, isTranscoded, allowedDataflows);
            }

            return new HashSet<ICodelistMutableObject>();
        }

        /// <summary>
        /// Get a list of CodeList matching the given id, version and agencyID. If any of the parameter is null or empty it will act as wildcard
        /// </summary>
        /// <param name="codelistReference">
        /// The codelist reference
        /// </param>
        /// <param name="subset">
        /// The list of items to retrieve
        /// </param>
        /// <returns>
        /// The list of matched CodeListBean objects
        /// </returns>
        public ISet<ICodelistMutableObject> GetMutableCodelistObjects(IMaintainableRefObject codelistReference, IList<string> subset)
        {
            return this._subsetCodelistRetrieval.Retrieve(codelistReference, ComplexStructureQueryDetailEnumType.Full, subset);
        }

        /// <summary>
        /// Gets CodelistObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="codelistReference">
        /// The codelist reference
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<ICodelistMutableObject> GetMutableCodelistObjects(IMaintainableRefObject codelistReference)
        {
            return this._subsetCodelistRetrieval.Retrieve(codelistReference, ComplexStructureQueryDetailEnumType.Full, VersionQueryType.All);
        }

        #endregion
    }
}