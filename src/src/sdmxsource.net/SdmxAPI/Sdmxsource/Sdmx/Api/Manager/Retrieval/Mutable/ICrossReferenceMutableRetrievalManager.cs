// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICrossReferenceMutableRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The CrossReferenceRetrievalMutableManager is used to retrieve mutable structures that cross reference or are cross referenced by the
    ///     given structures.
    /// </summary>
    public interface ICrossReferenceMutableRetrievalManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns a tree structure representing the <paramref name="maintainableObject"/> and all the structures that cross reference it, and the structures that reference them, and so on.
        /// </summary>
        /// <param name="maintainableObject">
        /// The maintainable object to build the tree for.
        /// </param>
        /// <returns>
        /// The <see cref="ICrossReferencingTree"/>.
        /// </returns>
        IMutableCrossReferencingTree GetCrossReferenceTree(IMaintainableMutableObject maintainableObject);

        /// <summary>
        /// Returns a list of MaintainableObject that cross reference the structure(s) that match the reference parameter
        /// </summary>
        /// <param name="structureReference">
        /// - What Do I Reference?
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <param name="structures">
        /// an optional parameter to further filter the list by structure type
        /// </param>
        /// <returns>
        /// The <see cref="IList{IMaintainableMutableObject}"/>.
        /// </returns>
        IList<IMaintainableMutableObject> GetCrossReferencedStructures(IStructureReference structureReference, bool returnStub, params SdmxStructureType[] structures);

        /// <summary>
        /// Returns a list of MaintainableObject that are cross referenced by the given identifiable structure
        /// </summary>
        /// <param name="identifiable">
        /// the identifiable bean to retrieve the references for - What Do I Reference?
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <param name="structures">
        /// an optional parameter to further filter the list by structure type
        /// </param>
        /// <returns>
        /// The <see cref="IList{IMaintainableMutableObject}"/>.
        /// </returns>
        IList<IMaintainableMutableObject> GetCrossReferencedStructures(IIdentifiableMutableObject identifiable, bool returnStub, params SdmxStructureType[] structures);

        /// <summary>
        /// Returns a list of MaintainableObject that cross reference the structure(s) that match the reference parameter
        /// </summary>
        /// <param name="structureReference">
        /// Who References Me?
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <param name="structures">
        /// an optional parameter to further filter the list by structure type
        /// </param>
        /// <returns>
        /// The <see cref="IList{IMaintainableMutableObject}"/>.
        /// </returns>
        IList<IMaintainableMutableObject> GetCrossReferencingStructures(IStructureReference structureReference, bool returnStub, params SdmxStructureType[] structures);

        /// <summary>
        /// Returns a list of MaintainableObject that cross reference the given identifiable structure
        /// </summary>
        /// <param name="identifiable">
        /// the identifiable bean to retrieve the references for - Who References Me?
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <param name="structures">
        /// an optional parameter to further filter the list by structure type
        /// </param>
        /// <returns>
        /// The <see cref="IList{IMaintainableMutableObject}"/>.
        /// </returns>
        IList<IMaintainableMutableObject> GetCrossReferencingStructures(IIdentifiableMutableObject identifiable, bool returnStub, params SdmxStructureType[] structures);

        #endregion
    }
}