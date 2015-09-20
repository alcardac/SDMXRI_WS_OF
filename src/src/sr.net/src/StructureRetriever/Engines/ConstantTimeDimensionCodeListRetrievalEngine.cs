// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantTimeDimensionCodeListRetrievalEngine.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Constant mapping time dimension code retrieval
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Engines
{
    using Estat.Nsi.StructureRetriever.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    /// <summary>
    /// Constant mapping time dimension code retrieval
    /// </summary>
    internal class ConstantTimeDimensionCodeListRetrievalEngine : ICodeListRetrievalEngine
    {
        #region Public Methods and Operators

        /// <summary>
        /// Retrieve Codelist
        /// </summary>
        /// <param name="info">
        /// The current StructureRetrieval state 
        /// </param>
        /// <returns>
        /// A <see cref="ICodelistMutableObject"/> 
        /// </returns>
        public ICodelistMutableObject GetCodeList(StructureRetrievalInfo info)
        {
            ISdmxDate constantDate = new SdmxDateCore(info.TimeMapping.Constant);
            return TimeDimensionCodeListHelper.BuildTimeCodelist(constantDate, constantDate);
        }

        #endregion
    }
}