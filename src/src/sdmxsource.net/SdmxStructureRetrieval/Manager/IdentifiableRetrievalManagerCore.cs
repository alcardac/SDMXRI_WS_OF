// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentifiableRetrievalManagerCore.cs" company="Eurostat">
//   Date Created : 2014-04-17
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The identifiable retrieval manager core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.StructureRetrieval.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     The identifiable retrieval manager core.
    /// </summary>
    public class IdentifiableRetrievalManagerCore : IIdentifiableRetrievalManager
    {
        #region Fields

        /// <summary>
        ///     The _external reference retrieval manager. Warning can be null.
        /// </summary>
        private readonly IExternalReferenceRetrievalManager _externalReferenceRetrievalManager;

        /// <summary>
        ///     The _retrieval manager
        /// </summary>
        private ISdmxObjectRetrievalManager _retrievalManager;

        #endregion

        #region Constructors and Destructors

        public IdentifiableRetrievalManagerCore()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableRetrievalManagerCore"/> class.
        /// </summary>
        /// <param name="externalReferenceRetrievalManager">
        /// The external reference retrieval manager.
        /// </param>
        /// <param name="retrievalManager">
        /// The retrieval manager.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="retrievalManager"/> is null.
        /// </exception>
        public IdentifiableRetrievalManagerCore(IExternalReferenceRetrievalManager externalReferenceRetrievalManager, ISdmxObjectRetrievalManager retrievalManager)
        {
            if (retrievalManager == null)
            {
                throw new ArgumentNullException("retrievalManager");
            }

            this._externalReferenceRetrievalManager = externalReferenceRetrievalManager;
            this.RetrievalManager = retrievalManager;
        }

        /// <summary>
        ///     The _retrieval manager
        /// </summary>
        protected ISdmxObjectRetrievalManager RetrievalManager
        {
            get
            {
                return this._retrievalManager;
            }
            set
            {
                this._retrievalManager = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the agency.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IAgency"/>.
        /// </returns>
        public virtual IAgency GetAgency(string id)
        {
            string agencyId = id;
            string agencyParentId = AgencyScheme.DefaultScheme;
            int lastDotIdx = id.LastIndexOf('.');
            if (lastDotIdx > -1)
            {
                // Sub Agency, get parent Scheme
                agencyParentId = id.Substring(0, lastDotIdx);
                agencyId = id.Substring(lastDotIdx + 1);
            }

            var structureReference = new StructureReferenceImpl(agencyParentId, AgencyScheme.FixedId, AgencyScheme.FixedVersion, SdmxStructureEnumType.AgencyScheme);

            var agencyScheme = this.GetIdentifiableObject<IAgencyScheme>(structureReference);
            return agencyScheme != null ? agencyScheme.Items.FirstOrDefault(agency => agency.Id.Equals(agencyId)) : null;
        }

        /// <summary>
        /// Resolves an identifiable reference
        /// </summary>
        /// <param name="crossReference">
        /// Cross-reference object
        /// </param>
        /// <returns>
        /// The <see cref="IIdentifiableObject"/> .
        /// </returns>
        /// <exception cref="CrossReferenceException">
        /// if the ICrossReference could not resolve to an IIdentifiableObject
        /// </exception>
        public IIdentifiableObject GetIdentifiableObject(ICrossReference crossReference)
        {
            var identifiableObject = this.GetIdentifiableObject((IStructureReference)crossReference);
            if (identifiableObject != null)
            {
                return identifiableObject;
            }

            throw new CrossReferenceException(crossReference);
        }

        /// <summary>
        /// Resolves an reference to a Object of type T, this will return the Object of the given type, throwing an exception
        ///     if either the
        ///     Object can not be resolved or if it is not of type T
        /// </summary>
        /// <typeparam name="T">
        /// Generic type parameter. 
        /// </typeparam>
        /// <param name="crossReference">
        /// Cross-reference object
        /// </param>
        /// <returns>
        /// The <see cref="T"/> .
        /// </returns>
        /// <exception cref="CrossReferenceException">
        /// if the ICrossReference could not resolve to an IIdentifiableObject
        /// </exception>
        public T GetIdentifiableObject<T>(ICrossReference crossReference)
        {
            var identifiable = this.GetIdentifiableObject(crossReference);
            if (identifiable is T)
            {
                return (T)identifiable;
            }

            throw new CrossReferenceException(crossReference);
        }

        /// <summary>
        /// Resolves an reference to a Object of type T, this will return the Object of the given type, throwing an exception
        ///     if e
        ///     Object is not of type T
        /// </summary>
        /// <typeparam name="T">
        /// Generic type parameter.
        /// </typeparam>
        /// <param name="crossReference">
        /// Structure-reference object
        /// </param>
        /// <returns>
        /// The <typeparamref name="T"/>
        /// </returns>
        /// <exception cref="SdmxReferenceException">
        /// If the object is not of type <typeparamref name="T"/>
        /// </exception>
        public T GetIdentifiableObject<T>(IStructureReference crossReference)
        {
            var identifiable = this.GetIdentifiableObject(crossReference);
            if (identifiable == null)
            {
                return default(T);
            }

            if (identifiable is T)
            {
                return (T)identifiable;
            }

            throw new SdmxReferenceException(crossReference);
        }

        /// <summary>
        /// Resolves an reference to a Object of type T, this will return the Object of the given type, throwing an exception
        ///     if e
        ///     Object is not of type T
        /// </summary>
        /// <param name="crossReference">
        /// Structure-reference object
        /// </param>
        /// <returns>
        /// The <see cref="IIdentifiableObject"/> .
        /// </returns>
        public IIdentifiableObject GetIdentifiableObject(IStructureReference crossReference)
        {
            IMaintainableObject maintainable = this.RetrievalManager.GetMaintainableObject(crossReference);
            if (maintainable == null)
            {
                return null;
            }

            if (maintainable.IsExternalReference.IsTrue)
            {
                if (this._externalReferenceRetrievalManager != null)
                {
                    maintainable = this._externalReferenceRetrievalManager.ResolveFullStructure(maintainable);
                }
            }

            var targetUrn = crossReference.TargetUrn;
            if (maintainable.Urn.Equals(targetUrn))
            {
                return maintainable;
            }

            var identifiableComposites = maintainable.IdentifiableComposites;
            foreach (var identifiableComposite in identifiableComposites)
            {
                if (identifiableComposite.Urn.Equals(targetUrn))
                {
                    return identifiableComposite;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the identifiable objects.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the identifiable objects to return.
        /// </typeparam>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <returns>
        /// Returns a set of identifiable objects that match the structure reference, which may be a full or partial
        ///     reference to a maintainable or identifiable
        /// </returns>
        public ISet<T> GetIdentifiableObjects<T>(IStructureReference structureReference) where T : IIdentifiableObject
        {
            var maintainableObjects = this.RetrievalManager.GetMaintainableObjects<T>(structureReference.MaintainableStructureEnumType.MaintainableInterface, structureReference.MaintainableReference);
            if (structureReference.TargetReference.IsMaintainable)
            {
                return maintainableObjects;
            }

            return new HashSet<T>(maintainableObjects.Cast<IMaintainableObject>().Select(structureReference.GetMatch).Where(match => match != null).Cast<T>());
        }

        #endregion
    }
}