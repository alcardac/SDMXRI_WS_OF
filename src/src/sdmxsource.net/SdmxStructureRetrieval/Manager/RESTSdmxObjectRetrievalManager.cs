// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RESTSdmxBeanRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-01-29
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The local retrieval manager provides interfaces to retrieve structures off an in memory storage of the ISdmxObjects.
//   <p />
//   This class is able to updated its cache as if it were a local storage with the interface methods provided by the StructurePersistenceManager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Org.Sdmxsource.Sdmx.StructureRetrieval.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    /// The rest SDMX object retrieval manager.
    /// </summary>
    public class RESTSdmxObjectRetrievalManager : BaseSdmxObjectRetrievalManager
    {
        #region Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private string _restURL;

        /// <summary>
        /// The structure query builder
        /// </summary>
        private IStructureQueryBuilder<string> _restQueryBuilder;

        /// <summary>
        /// The structure parsing manager
        /// </summary>
        private IStructureParsingManager spm;

        /// <summary>
        /// The data location factory
        /// </summary>
        private IReadableDataLocationFactory rdlFactory;

        #endregion


        #region Public properties

        /// <summary>
        /// gets the rest url
        /// </summary>
        public string RestURL
        {
            get { return _restURL; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RESTSdmxObjectRetrievalManager" /> class.
        /// </summary>
        /// <param name="restUrl">The REST URL string.</param>
        /// <param name="restQueryBuilder">The rest query builder.</param>
        /// <param name="spm">The structure parsing manager.</param>
        /// <param name="rdlFactory">The readable-data location factory.</param>
        /// <exception cref="System.ArgumentNullException">
        /// restQueryBuilder
        /// or
        /// <paramref name="spm"/>
        /// </exception>
        public RESTSdmxObjectRetrievalManager(string restUrl, IStructureQueryBuilder<string> restQueryBuilder, IStructureParsingManager spm, IReadableDataLocationFactory rdlFactory)
        {
            if (restQueryBuilder == null)
            {
                throw new ArgumentNullException("restQueryBuilder");
            }

            if (spm == null)
            {
                throw new ArgumentNullException("spm");
            }

            this._restURL = restUrl;
            this._restQueryBuilder = restQueryBuilder;
            this.spm = spm;
            this.rdlFactory = rdlFactory ?? new ReadableDataLocationFactory();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all the maintainable that match the <paramref name="restquery"/>
        /// </summary>
        /// <param name="restquery">The REST structure query.</param>
        /// <returns>the maintainable that match the <paramref name="restquery"/></returns>
        public override ISdmxObjects GetMaintainables(IRestStructureQuery _sQuery)
        {
            string restQuery = _restURL + "/" + _restQueryBuilder.BuildStructureQuery(_sQuery);
            WebRequest restURL;
            try
            {
                restURL = WebRequest.Create(restQuery);
            }
            catch (WebException e)
            {
                throw new SdmxException("Could not open a conneciton to URL: " + restQuery, e);
            }
            IReadableDataLocation rdl = rdlFactory.GetReadableDataLocation(_restURL);
            return spm.ParseStructures(rdl).GetStructureObjects(false);
        }

        /// <summary>
        /// Gets the SDMX objects.
        /// </summary>
        /// <param name="structureReference">The <see cref="IStructureReference"/> which must not be null.</param>
        /// <param name="resolveCrossReferences">either 'do not resolve', 'resolve all' or 'resolve all excluding agencies'. If not set to 'do not resolve' then all the structures that are referenced by the resulting structures are also returned (and also their children).  This will be equivalent to descendants for a <c>RESTful</c> query..</param>
        /// <returns>Returns a <see cref="ISdmxObjects"/> container containing all the Maintainable Objects that match the query parameters as defined by the <paramref name="structureReference"/>.</returns>
        public override ISdmxObjects GetSdmxObjects(IStructureReference sRef, ResolveCrossReferences resolveCrossReferences)
        {
            StructureReferenceDetail refDetail;
            switch (resolveCrossReferences)
            {
                case ResolveCrossReferences.DoNotResolve:
                    refDetail = StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.None);
                    break;
                default:
                    refDetail = StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.Descendants);
                    break;
            }

            StructureQueryDetail queryDetail = StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.Full);
            IRestStructureQuery query = new RESTStructureQueryCore(queryDetail, refDetail, null, sRef, false);
            return GetMaintainables(query);
        }

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
        public override ISet<T> GetMaintainableObjects<T>(IMaintainableRefObject maintainableReference, bool returnStub, bool returnLatest)
        {
            SdmxStructureType type = SdmxStructureType.ParseClass(typeof(T));

            IStructureReference sRef = new StructureReferenceImpl(maintainableReference, type);
            StructureReferenceDetail refDetail = StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.None);
            StructureQueryDetail queryDetail = returnStub ? StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.AllStubs) : StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.Full);
            IRestStructureQuery query = new RESTStructureQueryCore(queryDetail, refDetail, null, sRef, returnLatest);
            return (ISet<T>)GetMaintainables(query).GetMaintainables(sRef.MaintainableStructureEnumType);
        }

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
        public override ISet<T> GetMaintainableObjects<T>(Type maintainableInterface, IMaintainableRefObject maintainableReference)
        {
            // TODO implement.
            SdmxStructureType type = SdmxStructureType.ParseClass(maintainableInterface);

            IStructureReference sRef = new StructureReferenceImpl(maintainableReference, type);
            StructureReferenceDetail refDetail = StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.None);
            StructureQueryDetail queryDetail = StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.Full);
            IRestStructureQuery query = new RESTStructureQueryCore(queryDetail, refDetail, null, sRef, false);
            return (ISet<T>)GetMaintainables(query).GetMaintainables(sRef.MaintainableStructureEnumType);
        }

        #endregion
    }
}
