// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;

    #endregion

    /// <summary>
    ///     Manages the retrieval of structures and returns the responses as SDMX Objects.
    ///     <p />
    ///     The SdmxObjectRetrievalManager contains the means to return sdmxObjects by simple search criteria, based on a reference structure,
    /// </summary>
    /// <example>
    ///     A sample implementation in C# of <see cref="ISdmxObjectRetrievalManager" />
    ///     <code source="..\ReUsingExamples\DataQuery\ReUsingDataQueryParsingManager.cs" lang="cs" />
    /// </example>
    public interface ISdmxObjectRetrievalManager : IIdentifiableRetrievalManager
    {
        // IMPORTANT PUT REGISTARION AND SUBSCRIPTION ONTO THIS INTERFACE, THIS MAKES THE REGISTRATION RETRIEVAL OBSOLETEE 

        #region Public Methods and Operators

        /// <summary>
        /// Get all the maintainable that match the <paramref name="restquery"/>
        /// </summary>
        /// <param name="restquery">The REST structure query.</param>
        /// <returns>the maintainable that match the <paramref name="restquery"/></returns>
        ISdmxObjects GetMaintainables(IRestStructureQuery restquery);

        /// <summary>
        /// Gets the SDMX objects.
        /// </summary>
        /// <param name="structureReference">The <see cref="IStructureReference"/> which must not be null.</param>
        /// <param name="resolveCrossReferences">either 'do not resolve', 'resolve all' or 'resolve all excluding agencies'. If not set to 'do not resolve' then all the structures that are referenced by the resulting structures are also returned (and also their children).  This will be equivalent to descendants for a <c>RESTful</c> query..</param>
        /// <returns>Returns a <see cref="ISdmxObjects"/> container containing all the Maintainable Objects that match the query parameters as defined by the <paramref name="structureReference"/>.</returns>
        ISdmxObjects GetSdmxObjects(IStructureReference structureReference, ResolveCrossReferences resolveCrossReferences);

        /// <summary>
        /// Gets a maintainable defined by the StructureQueryObject parameter.
        ///     <p/>
        ///     Expects only ONE maintainable to be returned from this query
        /// </summary>
        /// <param name="structureReference">
        /// The reference object defining the search parameters, this is expected to uniquely identify one MaintainableObject
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableObject"/> .
        /// </returns>
        IMaintainableObject GetMaintainableObject(IStructureReference structureReference);

        /// <summary>
        /// Gets a maintainable defined by the StructureQueryObject parameter.
        ///     <p/>
        ///     Expects only ONE maintainable to be returned from this query
        /// </summary>
        /// <param name="structureReference">
        /// The reference object defining the search parameters, this is expected to uniquely identify one MaintainableObject
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableObject"/> .
        /// </returns>
        IMaintainableObject GetMaintainableObject(IStructureReference structureReference, bool returnStub, bool returnLatest);

        /// <summary>
        /// Gets a maintainable that is of the given type, determined by T, and matches the reference parameters in the IMaintainableRefObject.
        ///     <p/>
        ///     Expects only ONE maintainable to be returned from this query
        /// </summary>
        /// <param name="maintainableReference">
        /// The reference object that must match on the returned structure. If version information is missing, then latest version is assumed
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableObject"/> .
        /// </returns>
        T GetMaintainableObject<T>(IMaintainableRefObject maintainableReference) where T : IMaintainableObject;

        /// <summary>
        /// Gets a maintainable that is of the given type, determined by T, and matches the reference parameters in the IMaintainableRefObject.
        ///     <p/>
        ///     Expects only ONE maintainable to be returned from this query
        /// </summary>
        /// <param name="maintainableReference">
        /// The reference object that must match on the returned structure. If version information is missing, then latest version is assumed
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// /// <param name="returnLatest">
        /// If true then the latest version is returned, regardless of whether version information is supplied
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableObject"/> .
        /// </returns>
        T GetMaintainableObject<T>(IMaintainableRefObject maintainableReference, bool returnStub, bool returnLatest) where T : IMaintainableObject;

        /// <summary>
        /// Gets a set of all MaintainableObjects of type T
        ///     <p/>
        ///     An empty Set will be returned if there are no matches to the query
        /// </summary>
        /// <returns>
        /// The set of <see cref="IMaintainableObject"/> .
        /// </returns>
        ISet<T> GetMaintainableObjects<T>() where T : IMaintainableObject;

        /// <summary>
        /// Gets a set of all MaintainableObjects of type T that match the reference parameters in the IMaintainableRefObject argument.
        //     <p/>
        ///     An empty Set will be returned if there are no matches to the query
        /// </summary>
        /// <param name="maintainableReference">
        /// Contains the identifiers of the structures to returns, can include widcarded values (null indicates a wildcard). 
        /// </param>
        /// <returns>
        /// The set of <see cref="IMaintainableObject"/> .
        /// </returns>
        ISet<T> GetMaintainableObjects<T>(IMaintainableRefObject maintainableReference) where T : IMaintainableObject;

        /// <summary>
        /// Gets a set of all MaintainableObjects of type T that match the reference parameters in the IMaintainableRefObject argument.
        //     <p/>
        ///     An empty Set will be returned if there are no matches to the query
        /// </summary>
        /// <param name="maintainableReference">
        /// Contains the identifiers of the structures to returns, can include widcarded values (null indicates a wildcard). 
        /// </param>
        /// <param name="returnStub">
        /// If true then a stub object will be returned
        /// </param>
        /// /// <param name="returnLatest">
        /// If true then the latest version is returned, regardless of whether version information is supplied
        /// </param>
        /// <returns>
        /// The set of <see cref="IMaintainableObject"/> .
        /// </returns>
        ISet<T> GetMaintainableObjects<T>(IMaintainableRefObject maintainableReference, bool returnStub, bool returnLatest) where T : IMaintainableObject;

        /// <summary>
        /// Gets a set of all MaintainableObjects of type T that match the reference parameters in the IMaintainableRefObject argument.
        /// An empty Set will be returned if there are no matches to the query
        /// </summary>
        /// <typeparam name="T">The type of the maintainable. It is constraint  </typeparam>
        /// <param name="maintainableInterface">The maintainable interface.</param>
        /// <param name="maintainableReference">Contains the identifiers of the structures to returns, can include wild-carded values (null indicates a wild-card).</param>
        /// <returns>
        /// The set of <see cref="IMaintainableObject" /> .
        /// </returns>
        /// <remarks>This method exists only for compatibility reasons with Java implementation of this interface which uses raw types and unchecked generics.</remarks>
        ISet<T> GetMaintainableObjects<T>(Type maintainableInterface, IMaintainableRefObject maintainableReference) where T : ISdmxObject;

        #endregion
    }
}