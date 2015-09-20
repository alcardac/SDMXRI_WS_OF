// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TranscodedCodeListRetrievalEngine.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   //   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Codelist retrieval engine that gets the codelist from transcoding configuration. It implies that no criteria
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Engines
{
    using System.Collections.ObjectModel;
    using System.Globalization;

    using Estat.Nsi.StructureRetriever.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using System.Collections.Generic;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// Codelist retrieval engine that gets the codelist from transcoding configuration. It implies that no criteria
    /// </summary>
    internal class TranscodedCodeListRetrievalEngine : ICodeListRetrievalEngine
    {
        #region Constants and Fields

        /// <summary>
        /// The _is transcoded.
        /// </summary>
        private readonly bool _isTranscoded;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TranscodedCodeListRetrievalEngine"/> class.
        /// </summary>
        /// <param name="isTranscoded">
        /// The is transcoded.
        /// </param>
        public TranscodedCodeListRetrievalEngine(bool isTranscoded)
        {
            this._isTranscoded = isTranscoded;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Retrieve Codelist
        /// </summary>
        /// <param name="info">
        /// The current StructureRetrieval state 
        /// </param>
        /// <returns>
        /// A <see cref="CodeListBean"/> 
        /// </returns>
        public ICodelistMutableObject GetCodeList(StructureRetrievalInfo info)
        {
            var dataflowrRef = new MaintainableRefObjectImpl(info.MappingSet.Dataflow.Agency, info.MappingSet.Dataflow.Id, info.MappingSet.Dataflow.Version);
            ISet<ICodelistMutableObject> codeLists = info.MastoreAccess.GetMutableCodelistObjects(info.CodelistRef,dataflowrRef, info.RequestedComponent, this._isTranscoded, info.AllowedDataflows);

            return CodeListHelper.GetFirstCodeList(codeLists);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="TranscodedCodeListRetrievalEngine"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="TranscodedCodeListRetrievalEngine"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}(isTranscoded={1})", base.ToString(), this._isTranscoded);
        }

        #endregion
    }
}