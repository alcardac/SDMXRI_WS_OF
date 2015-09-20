// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuthCrossReferenceMutableRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-06-12
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The authorization aware CrossReference mutable object retrieval manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sdmxsource.Extension.Manager
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The authorization aware CrossReference mutable object retrieval manager.
    /// </summary>
    public interface IAuthCrossReferenceMutableRetrievalManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns a tree structure representing the <paramref name="maintainableObject"/> and all the structures that cross reference it, and the structures that reference them, and so on.
        /// </summary>
        /// <param name="maintainableObject">
        /// The maintainable object to build the tree for.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="IMutableCrossReferencingTree"/>.
        /// </returns>
        IMutableCrossReferencingTree GetCrossReferenceTree(IMaintainableMutableObject maintainableObject, IList<IMaintainableRefObject> allowedDataflows);

        /// <summary>
        /// Returns a list of MaintainableObject that cross reference the structure(s) that match the reference parameter
        /// </summary>
        /// <param name="structureReference">
        /// - What Do I Reference?
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <param name="structures">
        /// an optional parameter to further filter the list by structure type
        /// </param>
        /// <returns>
        /// The <see cref="IList{IMaintainableMutableObject}"/>.
        /// </returns>
        IList<IMaintainableMutableObject> GetCrossReferencedStructures(
            IStructureReference structureReference, bool returnStub, IList<IMaintainableRefObject> allowedDataflows, params SdmxStructureType[] structures);

        /// <summary>
        /// Returns a list of MaintainableObject that are cross referenced by the given identifiable structure
        /// </summary>
        /// <param name="identifiable">
        /// the identifiable bean to retrieve the references for - What Do I Reference?
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <param name="structures">
        /// an optional parameter to further filter the list by structure type
        /// </param>
        /// <returns>
        /// The <see cref="IList{IMaintainableMutableObject}"/>.
        /// </returns>
        IList<IMaintainableMutableObject> GetCrossReferencedStructures(
            IIdentifiableMutableObject identifiable, bool returnStub, IList<IMaintainableRefObject> allowedDataflows, params SdmxStructureType[] structures);

        /// <summary>
        /// Returns a list of MaintainableObject that cross reference the structure(s) that match the reference parameter
        /// </summary>
        /// <param name="structureReference">
        /// Who References Me?
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <param name="structures">
        /// an optional parameter to further filter the list by structure type
        /// </param>
        /// <returns>
        /// The <see cref="IList{IMaintainableMutableObject}"/>.
        /// </returns>
        IList<IMaintainableMutableObject> GetCrossReferencingStructures(
            IStructureReference structureReference, bool returnStub, IList<IMaintainableRefObject> allowedDataflows, params SdmxStructureType[] structures);

        /// <summary>
        /// Returns a list of MaintainableObject that cross reference the given identifiable structure
        /// </summary>
        /// <param name="identifiable">
        /// the identifiable bean to retrieve the references for - Who References Me?
        /// </param>
        /// <param name="returnStub">
        /// The return Stub.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <param name="structures">
        /// an optional parameter to further filter the list by structure type
        /// </param>
        /// <returns>
        /// The <see cref="IList{IMaintainableMutableObject}"/>.
        /// </returns>
        IList<IMaintainableMutableObject> GetCrossReferencingStructures(
            IIdentifiableMutableObject identifiable, bool returnStub, IList<IMaintainableRefObject> allowedDataflows, params SdmxStructureType[] structures);

        #endregion
    }
}