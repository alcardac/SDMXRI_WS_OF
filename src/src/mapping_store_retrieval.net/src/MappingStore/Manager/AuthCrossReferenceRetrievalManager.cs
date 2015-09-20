// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthCrossReferenceRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The cross reference mutable retrieval manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Estat.Sdmxsource.Extension.Extension;
    using Estat.Sdmxsource.Extension.Manager;
    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Engine;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Helper;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The cross reference mutable retrieval manager.
    /// </summary>
    public class AuthCrossReferenceRetrievalManager : IAuthCrossReferenceMutableRetrievalManager
    {
        /// <summary>
        /// The _retrieval advanced manager
        /// </summary>
        private readonly IAuthAdvancedSdmxMutableObjectRetrievalManager _retrievalAdvancedManager;

        #region Fields

        /// <summary>
        ///     The _from mutable.
        /// </summary>
        private readonly StructureReferenceFromMutableBuilder _fromMutable;

        /// <summary>
        ///     The retrieval engine container.
        /// </summary>
        private readonly RetrievalEngineContainer _retrievalEngineContainer;

        /// <summary>
        ///     The _cross reference.
        /// </summary>
        private readonly IAuthSdmxMutableObjectRetrievalManager _retrievalManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthCrossReferenceRetrievalManager"/> class.
        /// </summary>
        /// <param name="retrievalManager">
        /// The retrieval manager
        /// </param>
        /// <param name="connectionStringSettings">
        /// The connection String Settings.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="retrievalManager"/> is null
        /// -or-
        /// <paramref name="connectionStringSettings"/> is null
        /// </exception>
        public AuthCrossReferenceRetrievalManager(
            IAuthSdmxMutableObjectRetrievalManager retrievalManager, ConnectionStringSettings connectionStringSettings)
        {
            if (retrievalManager == null)
            {
                throw new ArgumentNullException("retrievalManager");
            }

            if (connectionStringSettings == null)
            {
                throw new ArgumentNullException("connectionStringSettings");
            }

            this._retrievalManager = retrievalManager;
            this._retrievalEngineContainer = new RetrievalEngineContainer(new Database(connectionStringSettings));
            this._fromMutable = new StructureReferenceFromMutableBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthCrossReferenceRetrievalManager"/> class.
        /// </summary>
        /// <param name="retrievalManager">
        /// The retrieval manager
        /// </param>
        /// <param name="mappingStoreDatabase">
        /// The mapping Store Database.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="retrievalManager"/> is null
        /// -or-
        /// <paramref name="mappingStoreDatabase"/> is null
        /// </exception>
        public AuthCrossReferenceRetrievalManager(IAuthSdmxMutableObjectRetrievalManager retrievalManager, Database mappingStoreDatabase)
        {
            if (retrievalManager == null)
            {
                throw new ArgumentNullException("retrievalManager");
            }

            if (mappingStoreDatabase == null)
            {
                throw new ArgumentNullException("mappingStoreDatabase");
            }

            this._retrievalManager = retrievalManager;
            this._retrievalEngineContainer = new RetrievalEngineContainer(mappingStoreDatabase);
            this._fromMutable = new StructureReferenceFromMutableBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthCrossReferenceRetrievalManager"/> class.
        /// </summary>
        /// <param name="retrievalAdvancedManager">The retrieval advanced manager.</param>
        /// <param name="mappingStoreDatabase">The mapping store database.</param>
        /// <exception cref="System.ArgumentNullException">
        /// retrievalAdvancedManager
        /// or
        /// mappingStoreDatabase
        /// </exception>
        public AuthCrossReferenceRetrievalManager(IAuthAdvancedSdmxMutableObjectRetrievalManager retrievalAdvancedManager, Database mappingStoreDatabase)
        {
            if (retrievalAdvancedManager == null)
            {
                throw new ArgumentNullException("retrievalAdvancedManager");
            }

            if (mappingStoreDatabase == null)
            {
                throw new ArgumentNullException("mappingStoreDatabase");
            }

            this._retrievalAdvancedManager = retrievalAdvancedManager;
            this._retrievalEngineContainer = new RetrievalEngineContainer(mappingStoreDatabase);
            this._fromMutable = new StructureReferenceFromMutableBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthCrossReferenceRetrievalManager"/> class.
        /// </summary>
        /// <param name="retrievalAdvancedManager">The retrieval advanced manager.</param>
        /// <param name="connectionStringSettings">The connection string settings.</param>
        /// <exception cref="System.ArgumentNullException">
        /// retrievalAdvancedManager
        /// or
        /// connectionStringSettings
        /// </exception>
        public AuthCrossReferenceRetrievalManager(IAuthAdvancedSdmxMutableObjectRetrievalManager retrievalAdvancedManager, ConnectionStringSettings connectionStringSettings)
        {
            if (retrievalAdvancedManager == null)
            {
                throw new ArgumentNullException("retrievalAdvancedManager");
            }

            if (connectionStringSettings == null)
            {
                throw new ArgumentNullException("connectionStringSettings");
            }

            this._retrievalAdvancedManager = retrievalAdvancedManager;
            this._retrievalEngineContainer = new RetrievalEngineContainer(new Database(connectionStringSettings));
            this._fromMutable = new StructureReferenceFromMutableBuilder();
        }

        #endregion

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
        public IMutableCrossReferencingTree GetCrossReferenceTree(IMaintainableMutableObject maintainableObject, IList<IMaintainableRefObject> allowedDataflows)
        {
            throw new NotImplementedException();
        }

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
        /// The <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </returns>
        public IList<IMaintainableMutableObject> GetCrossReferencedStructures(
            IStructureReference structureReference, bool returnStub, IList<IMaintainableRefObject> allowedDataflows, params SdmxStructureType[] structures)
        {
            if (structureReference == null)
            {
                throw new ArgumentNullException("structureReference");
            }

            IMaintainableMutableObject maintainableObject = this.GetMutableMaintainable(structureReference, allowedDataflows);

            return this.GetCrossReferencedStructures(maintainableObject, returnStub, allowedDataflows, structures);
        }

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
        public IList<IMaintainableMutableObject> GetCrossReferencedStructures(
            IIdentifiableMutableObject identifiable, bool returnStub, IList<IMaintainableRefObject> allowedDataflows, params SdmxStructureType[] structures)
        {
            if (identifiable == null)
            {
                throw new ArgumentNullException("identifiable");
            }

            var sdmxStructureType = identifiable.StructureType;
            var maintainableMutableObject = identifiable as IMaintainableMutableObject;
            if (!sdmxStructureType.IsMaintainable || maintainableMutableObject == null)
            {
                throw new NotImplementedException(ErrorMessages.CrossReferenceIdentifiable + sdmxStructureType);
            }

            if (maintainableMutableObject.ExternalReference != null && maintainableMutableObject.ExternalReference.IsTrue)
            {
                var structureReference = this._fromMutable.Build(maintainableMutableObject);
                maintainableMutableObject = this.GetMutableMaintainable(structureReference, allowedDataflows);
            }

            Func<IStructureReference, IMaintainableMutableObject> retrievalManager = reference => this.GetMutableMaintainable(reference, allowedDataflows, returnStub);

            var crossReferenceEngine = new CrossReferenceResolverMutableEngine(structures);
            return new List<IMaintainableMutableObject>(crossReferenceEngine.ResolveReferences(maintainableMutableObject, 1, retrievalManager));
        }

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
        public IList<IMaintainableMutableObject> GetCrossReferencingStructures(
            IStructureReference structureReference, bool returnStub, IList<IMaintainableRefObject> allowedDataflows, params SdmxStructureType[] structures)
        {
            if (structureReference == null)
            {
                throw new ArgumentNullException("structureReference");
            }

            IMaintainableMutableObject maintainableObject = this.GetMutableMaintainable(structureReference, allowedDataflows, returnStub);

            return this.GetCrossReferencingStructures(maintainableObject, returnStub, allowedDataflows, structures);
        }

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
        public IList<IMaintainableMutableObject> GetCrossReferencingStructures(IIdentifiableMutableObject identifiable, bool returnStub, params SdmxStructureType[] structures)
        {
            return this.GetCrossReferencingStructures(identifiable, returnStub, null, structures);
        }

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
        public IList<IMaintainableMutableObject> GetCrossReferencingStructures(
            IIdentifiableMutableObject identifiable, bool returnStub, IList<IMaintainableRefObject> allowedDataflows, params SdmxStructureType[] structures)
        {
            var maintainable = identifiable as IMaintainableMutableObject;
            if (maintainable == null)
            {
                throw new NotImplementedException("Only maintainable are supported by this implementation.");
            }

            var structureSet = new HashSet<SdmxStructureType>(structures ?? new SdmxStructureType[0]);
            IStructureReference structureReference = this._fromMutable.Build(maintainable);
            var mutableObjects = new List<IMaintainableMutableObject>();
            switch (identifiable.StructureType.EnumType)
            {
                case SdmxStructureEnumType.Dataflow:
                    {
                        if (structureSet.HasStructure(SdmxStructureEnumType.Categorisation))
                        {
                            ISet<ICategorisationMutableObject> referenced = this._retrievalEngineContainer.CategorisationRetrievalEngine.RetrieveFromReferenced(
                                structureReference, returnStub.GetComplexQueryDetail(), allowedDataflows);
                            mutableObjects.AddRange(referenced);
                        }
                    }

                    break;
                case SdmxStructureEnumType.CodeList:
                    {
                        if (structureSet.HasStructure(SdmxStructureEnumType.Dsd))
                        {
                            ISet<IDataStructureMutableObject> referenced = this._retrievalEngineContainer.DSDRetrievalEngine.RetrieveFromReferenced(
                                structureReference, returnStub.GetComplexQueryDetail());
                            mutableObjects.AddRange(referenced);
                        }

                        if (structureSet.HasStructure(SdmxStructureEnumType.HierarchicalCodelist))
                        {
                            ISet<IHierarchicalCodelistMutableObject> referenced2 = this._retrievalEngineContainer.HclRetrievalEngine.RetrieveFromReferenced(
                                structureReference, returnStub.GetComplexQueryDetail());
                            mutableObjects.AddRange(referenced2);
                        }
                    }

                    break;
                case SdmxStructureEnumType.ConceptScheme:
                    {
                        if (structureSet.HasStructure(SdmxStructureEnumType.Dsd))
                        {
                            ISet<IDataStructureMutableObject> referenced = this._retrievalEngineContainer.DSDRetrievalEngine.RetrieveFromReferenced(
                                structureReference, returnStub.GetComplexQueryDetail());
                            mutableObjects.AddRange(referenced);
                        }
                    }

                    break;
                case SdmxStructureEnumType.CategoryScheme:
                    {
                        if (structureSet.HasStructure(SdmxStructureEnumType.Categorisation))
                        {
                            ISet<ICategorisationMutableObject> referenced = this._retrievalEngineContainer.CategorisationRetrievalEngine.RetrieveFromReferenced(
                                structureReference, returnStub.GetComplexQueryDetail(), allowedDataflows);
                            mutableObjects.AddRange(referenced);
                        }
                    }

                    break;

                case SdmxStructureEnumType.Dsd:
                    {
                        if (structureSet.HasStructure(SdmxStructureEnumType.Dataflow))
                        {
                            ISet<IDataflowMutableObject> referenced = this._retrievalEngineContainer.DataflowRetrievalEngine.RetrieveFromReferenced(
                                structureReference, returnStub.GetComplexQueryDetail(), allowedDataflows);
                            mutableObjects.AddRange(referenced);
                        }
                    }

                    break;
            }

            return mutableObjects;
        }

        #endregion

        /// <summary>
        /// Gets the mutable maintainable.
        /// </summary>
        /// <param name="structureReference">The structure reference.</param>
        /// <param name="allowedDataflows">The allowed dataflows.</param>
        /// <param name="returnStub">if set to <c>true</c> [return stub].</param>
        /// <returns>The mutable maintainable or null</returns>
        private IMaintainableMutableObject GetMutableMaintainable(IStructureReference structureReference, IList<IMaintainableRefObject> allowedDataflows, bool returnStub = false)
        {
            if (this._retrievalManager != null)
            {
                return this._retrievalManager.GetMutableMaintainable(structureReference, false, returnStub, allowedDataflows);
            }

            var complexStructureQueryDetail = ComplexStructureQueryDetail.GetFromEnum(returnStub.GetComplexQueryDetail());
            return this._retrievalAdvancedManager.GetMutableMaintainable(structureReference.ToComplex(), complexStructureQueryDetail, allowedDataflows);
        }
    }
}