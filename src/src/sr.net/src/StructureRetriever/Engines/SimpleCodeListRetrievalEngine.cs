// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleCodeListRetrievalEngine.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   A simple mapping store codelists retrieval engine
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Engines
{
    using System.Collections.ObjectModel;

    using Estat.Nsi.StructureRetriever.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using System.Collections.Generic;

    /// <summary>
    /// A simple mapping store codelists retrieval engine
    /// </summary>
    internal class SimpleCodeListRetrievalEngine : ICodeListRetrievalEngine
    {
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
            ISet<ICodelistMutableObject> codeLists =
                info.MastoreAccess.GetMutableCodelistObjects(new MaintainableRefObjectImpl(info.CodelistRef.AgencyId, 
                                                                                            info.CodelistRef.MaintainableId,
                                                                                            info.CodelistRef.Version));
            return CodeListHelper.GetFirstCodeList(codeLists);
        }

        #endregion
    }
}