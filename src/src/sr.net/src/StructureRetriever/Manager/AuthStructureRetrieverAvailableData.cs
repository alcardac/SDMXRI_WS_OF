// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthStructureRetrieverAvailableData.cs" company="Eurostat">
//   Date Created : 2013-07-26
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This is an implementation of "IStructureRetriever" interface that can retrieve available data from DDB and dataflows with complete mapping set.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Nsi.StructureRetriever.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Estat.Nsi.StructureRetriever.Factory;
    using Estat.Sdmxsource.Extension.Manager;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;

    /// <summary>
    ///     This is an implementation of "IStructureRetriever" interface that can retrieve available data from DDB and dataflows with complete mapping set.
    /// </summary>
    /// <example>
    ///     Code example for C#
    ///     <code source="ReUsingExamples\StructureRetriever\ReUsingStructureRetriever.cs" lang="cs">
    /// </code>
    /// </example>
    /// <remarks>
    ///     It's main job is to retrieve structural metadata from Mapping Store. It can be used with any v2.6 or higher "Mapping Store" complying with the database design specified there. This implementation supports the following special case in order to retrieve a subset of codes for a dimension that can be constrained by the values of other dimensions: If the <c>QueryStructureRequest</c> contains a <c>CodelistRef</c> and <c>DataflowRef</c> with constrains with one <c>Member</c> without <c>MemberValue</c> and optionally any number <c>Member</c> with <c>MemberValue</c> then the this implementation will retrieve the subset of the requested codelist that is used by the dimension referenced in the member without member value, used by the specified dataflow and constrained by the dimension values defined with Member and MemberValues.
    /// </remarks>
    public class AuthStructureRetrieverAvailableData : IAuthMutableStructureSearchManager
    {
        #region Fields

        /// <summary>
        ///     The authorization structure retriever.
        /// </summary>
        private readonly IAuthMutableStructureSearchManager _authStructureRetriever;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthStructureRetrieverAvailableData"/> class. 
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// defaultHeader is null
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// connectionStringSettings is null
        /// </exception>
        /// <exception cref="StructureRetrieverException">
        /// Could not establish a connection to the mapping store DB
        ///     <see cref="StructureRetrieverException.ErrorType"/>
        ///     is set to
        ///     <see cref="StructureRetrieverErrorTypes.MappingStoreConnectionError"/>
        /// </exception>
        /// <param name="connectionStringSettings">
        /// The connection to the "Mapping Store", from which the SDMX Structural metadata will be retrieved
        /// </param>
        /// <param name="sdmxSchema">
        /// The SDMX Schema.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// defaultHeader or connectionStringSettings is null
        /// </exception>
        public AuthStructureRetrieverAvailableData(ConnectionStringSettings connectionStringSettings, SdmxSchema sdmxSchema)
        {
            if (connectionStringSettings == null)
            {
                throw new ArgumentNullException("connectionStringSettings");
            }

            IStructureSearchManagerFactory<IAuthMutableStructureSearchManager> factory = new AuthMutableStructureSearchManagerFactory();

            this._authStructureRetriever = factory.GetStructureSearchManager(connectionStringSettings, sdmxSchema);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns the latest version of the maintainable for the given maintainable input
        /// </summary>
        /// <param name="maintainableObject">
        /// The maintainable Object.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IMaintainableMutableObject"/>.
        /// </returns>
        public IMaintainableMutableObject GetLatest(IMaintainableMutableObject maintainableObject, IList<IMaintainableRefObject> allowedDataflows)
        {
            return this._authStructureRetriever.GetLatest(maintainableObject, allowedDataflows);
        }

        /// <summary>
        /// Returns a set of maintainable that match the given query parameters
        /// </summary>
        /// <param name="structureQuery">
        /// The structure Query.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.IMutableObjects"/>.
        /// </returns>
        public IMutableObjects GetMaintainables(IRestStructureQuery structureQuery, IList<IMaintainableRefObject> allowedDataflows)
        {
            return this._authStructureRetriever.GetMaintainables(structureQuery, allowedDataflows);
        }

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
        /// The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.IMutableObjects"/>.
        /// </returns>
        public IMutableObjects RetrieveStructures(IList<IStructureReference> queries, bool resolveReferences, bool returnStub, IList<IMaintainableRefObject> allowedDataflows)
        {
            return this._authStructureRetriever.RetrieveStructures(queries, resolveReferences, returnStub, allowedDataflows);
        }

        #endregion
    }
}