// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISpecialRequestManager.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The special request manager interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Manager
{
    using System;

    using Estat.Nsi.StructureRetriever.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The special request manager interface.
    /// </summary>
    internal interface ISpecialRequestManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Retrieve the codelist that is referenced by the given <paramref name="codelistRef"/> . 
        /// </summary>
        /// <param name="codelistRef">
        /// The codelist reference containing the id, agency and version of the requested dimension. Can be empty for time dimension 
        /// </param>
        /// <param name="info">
        /// The current structure retrieval state. 
        /// </param>
        /// <returns>
        /// The partial codelist 
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="codelistRef"/>
        ///   is null
        /// </exception>
        ICodelistMutableObject RetrieveAvailableData(IMaintainableRefObject codelistRef, StructureRetrievalInfo info);

        #endregion
    }
}