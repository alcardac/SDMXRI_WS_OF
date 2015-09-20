// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossReferenceResolverMutableEngine.cs" company="Eurostat">
//   Date Created : 2013-03-04
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The cross reference resolver mutable engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Util.Collections;

    /// <summary>
    ///     The cross reference resolver mutable engine.
    /// </summary>
    public class CrossReferenceResolverMutableEngine : ICrossReferenceResolverMutableEngine
    {
        #region Static Fields

        /// <summary>
        /// The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(CrossReferenceResolverMutableEngine));

        /// <summary>
        ///     The builder that builds a <see cref="ISet{T}" /> from a <see cref="IIdentifiableMutableObject" />
        /// </summary>
        private static readonly CrossReferenceChildBuilder _childBuilder = new CrossReferenceChildBuilder();

        /// <summary>
        /// The _from mutable.
        /// </summary>
        private static readonly StructureReferenceFromMutableBuilder _fromMutable = new StructureReferenceFromMutableBuilder();

        #endregion

        #region Fields

        /// <summary>
        /// The _structure type set.
        /// </summary>
        private readonly HashSet<SdmxStructureType> _structureTypeSet;

        /// <summary>
        /// The cache.
        /// </summary>
        private readonly Dictionary<IStructureReference, IMaintainableMutableObject> _cache = new Dictionary<IStructureReference, IMaintainableMutableObject>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceResolverMutableEngine"/> class.
        /// </summary>
        public CrossReferenceResolverMutableEngine()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceResolverMutableEngine"/> class.
        /// </summary>
        /// <param name="objects">
        /// The objects.
        /// </param>
        /// <param name="sdmxStructureTypes">
        /// The SDMX structure types.
        /// </param>
        public CrossReferenceResolverMutableEngine(IEnumerable<IMaintainableMutableObject> objects, params SdmxStructureType[] sdmxStructureTypes)
            : this(sdmxStructureTypes)
        {
            if (objects != null)
            {
                foreach (var mutableObject in objects)
                {
                    IStructureReference reference = _fromMutable.Build(mutableObject);
                    this._cache.Add(reference, mutableObject);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceResolverMutableEngine"/> class.
        /// </summary>
        /// <param name="structures">
        /// The structures.
        /// </param>
        public CrossReferenceResolverMutableEngine(IEnumerable<SdmxStructureType> structures)
        {
            this._structureTypeSet = new HashSet<SdmxStructureType>(structures ?? new SdmxStructureType[0]);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a Dictionary of <see cref="IIdentifiableMutableObject"/> alongside any cross references they declare that could not be found in the set of
        ///     <paramref name="beans"/>
        ///     provided, and the <paramref name="retrievalManager"/> (if given).
        ///     <p/>
        ///     <b>NOTE :</b>An empty Map is returned if all cross references are present.
        /// </summary>
        /// <param name="beans">
        /// - the objects to return the Map of missing references for
        /// </param>
        /// <param name="numberLevelsDeep">
        /// references, an argument of 0 (zero) implies there is no limit, and the resolver engine will continue re-cursing  until it has found every directly and indirectly referenced
        /// </param>
        /// <param name="retrievalManager">
        /// - Used to resolve the structure references. Can be null, if supplied this is used to resolve any references that do not exist in the supplied beans
        /// </param>
        /// <returns>
        /// Map of IIdentifiableMutableObject with a Set of CrossReferences that could not be resolved for the IIdentifiableMutableObject - an empty Map is returned if all cross references are present
        /// </returns>
        public MaintainableReferenceDictionary GetMissingCrossReferences(
            IMutableObjects beans, int numberLevelsDeep, Func<IStructureReference, IMaintainableMutableObject> retrievalManager)
        {
            var missingReferences = new MaintainableReferenceDictionary();
            this.ResolveReferencesInternal(beans, numberLevelsDeep, retrievalManager, missingReferences);
            return missingReferences;
        }

        /// <summary>
        /// Resolves all references and returns a Map containing all the input beans and the objects that are cross referenced,
        ///     the Map's key set contains the Identifiable that is the referencing object and the Map's value collection contains the referenced artifacts.
        /// </summary>
        /// <param name="beans">
        /// - the <see cref="IMutableObjects"/> container, containing all the beans to check references for
        /// </param>
        /// <param name="numberLevelsDeep">
        /// references, an argument of 0 (zero) implies there is no limit, and the resolver engine will continue re-cursing until it has found every directly and indirectly referenced artifact. Note that there is no risk of infinite recursion in calling this.
        /// </param>
        /// <param name="retrievalManager">
        /// - Used to resolve the structure references. Can be null, if supplied this is used to resolve any references that do not exist in the supplied beans
        /// </param>
        /// <returns>
        /// Map of referencing versus  references
        /// </returns>
        /// <exception cref="SdmxReferenceException">
        /// - if any of the references could not be resolved
        /// </exception>
        public MaintainableDictionary<IMaintainableMutableObject> ResolveReferences(
            IMutableObjects beans, int numberLevelsDeep, Func<IStructureReference, IMaintainableMutableObject> retrievalManager)
        {
            var missingReferences = new DictionaryOfSets<IMaintainableMutableObject, IStructureReference>(MaintainableMutableComparer.Instance);
            var returnMap = this.ResolveReferencesInternal(beans, numberLevelsDeep, retrievalManager, missingReferences);
            if (missingReferences.Count > 0)
            {
                var keyValuePair = missingReferences.First();
                throw new SdmxReferenceException(keyValuePair.Key.ImmutableInstance, keyValuePair.Value.FirstOrDefault());
            }

            return returnMap;
        }

        /// <summary>
        /// Returns a set of <see cref="IMaintainableMutableObject"/> that the IMaintainableMutableObject cross references
        /// </summary>
        /// <param name="artefact">
        /// The bean.
        /// </param>
        /// <param name="numberLevelsDeep">
        /// references, an argument of 0 (zero) implies there is no limit, and the resolver engine will continue re-cursing until it has found every directly and indirectly referenced artifact. Note that there is no risk of infinite recursion in calling this.
        /// </param>
        /// <param name="retrievalManager">
        /// - Used to resolve the structure references. Can be null, if supplied this is used to resolve any references that do not exist in the supplied beans
        /// </param>
        /// <exception cref="SdmxReferenceException">
        /// - if any of the references could not be resolved
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="artefact"/> is null.</exception>
        /// <returns>
        /// a set of <see cref="IMaintainableMutableObject"/> that the IMaintainableMutableObject cross references
        /// </returns>
        public ISet<IMaintainableMutableObject> ResolveReferences(IMaintainableMutableObject artefact, int numberLevelsDeep, Func<IStructureReference, IMaintainableMutableObject> retrievalManager)
        {
            if (artefact == null)
            {
                throw new ArgumentNullException("artefact");
            }

            IMutableObjects objects = new MutableObjectsImpl();
            objects.AddIdentifiable(artefact);
            IDictionaryOfSets<IMaintainableMutableObject, IMaintainableMutableObject> dictionaryOfSets = this.ResolveReferences(objects, numberLevelsDeep, retrievalManager);
            ISet<IMaintainableMutableObject> set;
            if (!dictionaryOfSets.TryGetValue(artefact, out set))
            {
                set = new HashSet<IMaintainableMutableObject>();
            }

            return set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resolve references.
        /// </summary>
        /// <param name="beans">
        /// The beans.
        /// </param>
        /// <param name="numberLevelsDeep">
        /// The number levels deep.
        /// </param>
        /// <param name="retrievalManager">
        /// The retrieval manager.
        /// </param>
        /// <param name="missingReferences">
        /// The missing references.
        /// </param>
        /// <returns>
        /// The <see cref="MaintainableDictionary{IMaintainableMutableObject}"/>.
        /// </returns>
        private MaintainableDictionary<IMaintainableMutableObject> ResolveReferencesInternal(
            IMutableObjects beans, int numberLevelsDeep, Func<IStructureReference, IMaintainableMutableObject> retrievalManager, IDictionaryOfSets<IMaintainableMutableObject, IStructureReference> missingReferences)
        {
            var returnMap = new MaintainableDictionary<IMaintainableMutableObject>();
            var outerLevel = new Queue<IMaintainableMutableObject>();
            foreach (IMaintainableMutableObject artefact in beans.AllMaintainables)
            {
                outerLevel.Enqueue(artefact);
            }

            int count = 0;
            do
            {
                var innerLevel = new Queue<IMaintainableMutableObject>();
                while (outerLevel.Count > 0)
                {
                    IMaintainableMutableObject parent = outerLevel.Dequeue();
                    var structureReferences = GetChildStructureReferences(retrievalManager, parent);

                    foreach (IStructureReference structureReference in structureReferences)
                    {
                        if (structureReference == null)
                        {
                            string message = string.Format(CultureInfo.InvariantCulture, "Null reference for parent artefact : {0}+{1}+{2}", parent.AgencyId, parent.Id, parent.Version);
                            _log.Error(message);
                            throw new MappingStoreException(message);
                        }

                        if (this._structureTypeSet.HasStructure(structureReference.MaintainableStructureEnumType))
                        {
                            IMaintainableMutableObject resolved = this.GetMutableObject(retrievalManager, structureReference);

                            if (resolved != null)
                            {
                                returnMap.AddToSet(parent, resolved);
                                if (numberLevelsDeep == 0 || numberLevelsDeep > count)
                                {
                                    innerLevel.Enqueue(resolved);
                                }
                            }
                            else if (missingReferences != null)
                            {
                                missingReferences.AddToSet(parent, structureReference);
                            }
                        }
                    }
                }

                count++;
                outerLevel = innerLevel;
            }
            while (count < numberLevelsDeep);

            return returnMap;
        }

        /// <summary>
        /// Gets the child structure references.
        /// </summary>
        /// <param name="retrievalManager">
        /// The retrieval manager.
        /// </param>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{IStructureReference}"/>.
        /// </returns>
        private static IEnumerable<IStructureReference> GetChildStructureReferences(Func<IStructureReference, IMaintainableMutableObject> retrievalManager, IMaintainableMutableObject parent)
        {
            ISet<IStructureReference> structureReferences;
            if (parent.ExternalReference != null && parent.ExternalReference.IsTrue)
            {
                switch (parent.StructureType.EnumType)
                {
                    case SdmxStructureEnumType.CodeList:
                    case SdmxStructureEnumType.ConceptScheme:
                        structureReferences = new HashSet<IStructureReference>();
                        break;
                    default:
                        {
                            var parentReference = _fromMutable.Build(parent);
                            var parentNoStub = retrievalManager(parentReference);
                            structureReferences = _childBuilder.Build(parentNoStub);
                        }

                        break;
                }
            }
            else
            {
                structureReferences = _childBuilder.Build(parent);
            }
            return structureReferences;
        }

        /// <summary>
        /// Returns a mutable object that matches <paramref name="structureReference"/> from either <see cref="_cache"/> or <paramref name="retrievalManager"/>
        /// </summary>
        /// <param name="retrievalManager">
        /// The retrieval manager.
        /// </param>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableMutableObject"/>.
        /// </returns>
        private IMaintainableMutableObject GetMutableObject(Func<IStructureReference, IMaintainableMutableObject> retrievalManager, IStructureReference structureReference)
        {
            IMaintainableMutableObject resolved;
            if (!this._cache.TryGetValue(structureReference, out resolved))
            {
                resolved = retrievalManager(structureReference);

                if (resolved != null)
                {
                    this._cache.Add(structureReference, resolved);
                }
            }

            return resolved;
        }

        #endregion
    }
}