// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuthMutableStructureSearchManager.cs" company="Eurostat">
//   Date Created : 2013-04-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The MutableStructureAuthSearchManager interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sdmxsource.Extension.Manager
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;

    /// <summary>
    /// The interface for searching structures with a list of allowed dataflows. 
    /// </summary>
    public interface IAuthMutableStructureSearchManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns the latest version of the maintainable for the given maintainable input
        /// </summary>
        /// <param name="maintainableObject">
        /// The maintainable Object.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows. Optional. Set to null to disable checking against allowed dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableMutableObject"/>.
        /// </returns>
        IMaintainableMutableObject GetLatest(IMaintainableMutableObject maintainableObject, IList<IMaintainableRefObject> allowedDataflows);

        /// <summary>
        /// Returns a set of maintainable that match the given query parameters
        /// </summary>
        /// <param name="structureQuery">
        /// The structure Query.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows. Optional. Set to null to disable checking against allowed dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="IMutableObjects"/>.
        /// </returns>
        IMutableObjects GetMaintainables(IRestStructureQuery structureQuery, IList<IMaintainableRefObject> allowedDataflows);

        /// <summary>
        /// Retrieves all structures that match the given query parameters in the list of query objects.  The list
        ///     must contain at least one StructureQueryObject.
        /// </summary>
        /// <param name="queries">
        /// The queries.
        /// </param>
        /// <param name="resolveReferences">
        /// - if set to true then any cross referenced structures will also be available in the SdmxObjects container
        /// </param>
        /// <param name="returnStub">
        /// - if set to true then only stubs of the returned objects will be returned.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows. Optional. Set to null to disable checking against allowed dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="IMutableObjects"/>.
        /// </returns>
        IMutableObjects RetrieveStructures(IList<IStructureReference> queries, bool resolveReferences, bool returnStub, IList<IMaintainableRefObject> allowedDataflows);

        #endregion
    }
}