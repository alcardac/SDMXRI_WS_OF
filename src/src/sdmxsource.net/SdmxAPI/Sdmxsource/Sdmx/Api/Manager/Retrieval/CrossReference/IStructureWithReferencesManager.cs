// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureWithReferencesManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.CrossReference
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    /// Provides a mechanism to query for a structure, and include the full structure tree in the response
    /// </summary>
    public interface IStructureWithReferencesManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns the given structure, along with all the structures that it references
        /// </summary>
        /// <param name="structureReference">
        /// The structure reference
        /// </param>
        /// <returns>
        /// The structure
        /// </returns>
        ISdmxObjects GetStructureWithReferences(IStructureReference structureReference);

        #endregion
    }
}
