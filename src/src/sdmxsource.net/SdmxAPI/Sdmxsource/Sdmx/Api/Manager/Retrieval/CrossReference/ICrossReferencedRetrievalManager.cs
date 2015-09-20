// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICrossReferencedRetrievalManager.cs" company="Eurostat">
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

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    /// The CrossReferenceRetrievalManager is used to retrieve structures that cross reference or are cross referenced by the 
    /// given structures.
    /// </summary>
    public interface ICrossReferencedRetrievalManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns a set of MaintainableObjects that are cross referenced by the structure(s) that match the reference parameter
        /// </summary>
        /// <param name="structureReference">
        /// The structureReference  - What Do I Reference?
        /// </param>
        /// <param name="returnStub">
        /// returnStub if true, then will return the stubs that reference the structure
        /// </param>
        /// <param name="structures">
        /// structures an optional parameter to further filter the list by structure type
        /// </param>
        /// <returns>
        /// The MaintainableObjects
        /// </returns>
        ISet<IMaintainableObject> GetCrossReferencedStructures(IStructureReference structureReference, bool returnStub, params SdmxStructureType[] structures);

        /// <summary>
        /// Returns a set of MaintainableObject that are cross referenced by the given identifiable structure
        /// </summary>
        /// <param name="identifiable">
        /// The identifiable object to retrieve the references for - What Do I Reference?
        /// </param>
        /// <param name="returnStub">
        /// ReturnStub if true, then will return the stubs that reference the structure
        /// </param>
        /// <param name="structures">
        /// Structures an optional parameter to further filter the list by structure type
        /// </param>
        /// <returns>
        /// The MaintainableObjects
        /// </returns>
        ISet<IMaintainableObject> GetCrossReferencedStructures(IIdentifiableObject identifiable, bool returnStub, params SdmxStructureType [] structures);

        #endregion
    }
}
