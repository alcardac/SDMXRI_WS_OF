// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantCodeListRetrievalEngine.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Codelist with a single constant code retrieval engine
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Engines
{
    using System.Collections.ObjectModel;

    using Estat.Nsi.StructureRetriever.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using System.Collections.Generic;

    /// <summary>
    /// Codelist with a single constant code retrieval engine
    /// </summary>
    internal class ConstantCodeListRetrievalEngine : ICodeListRetrievalEngine
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
            if (info.RequestedComponentInfo == null)
            {
                return null;
            }


            ISet<ICodelistMutableObject> codeLists = info.MastoreAccess.GetMutableCodelistObjects(
            info.CodelistRef,
            new[] { info.RequestedComponentInfo.Mapping.Constant });

            return CodeListHelper.GetFirstCodeList(codeLists);
        }

        #endregion
    }
}