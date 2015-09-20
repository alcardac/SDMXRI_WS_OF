// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureWorkspace.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure workspace implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Workspace
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Output;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager;
    using Org.Sdmxsource.Sdmx.StructureRetrieval.Engine;
    using Org.Sdmxsource.Sdmx.Util.Objects;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Util.Collections;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    ///     The structure workspace implementation.
    /// </summary>
    public class StructureWorkspace : IStructureWorkspace
    {
        #region Fields

        /// <summary>
        ///     The header.TODO
        /// </summary>
        private readonly IHeader _header = new HeaderImpl("UNASSIGNED", "UNKNOWN");

        /// <summary>
        ///     The resolution depth.
        /// </summary>
        private readonly int _resolutionDepth;

        /// <summary>
        ///     The retrieval manager.
        /// </summary>
        private readonly IIdentifiableRetrievalManager _retrievalManager;

        /// <summary>
        ///     The retrieve agencies.
        /// </summary>
        private readonly bool _retrieveAgencies;

        /// <summary>
        ///     The retrieve cross references.
        /// </summary>
        private readonly bool _retrieveCrossRefefences;

        /// <summary>
        ///     The all sdmxObjects.
        /// </summary>
        private ISdmxObjects _allSdmxObjects;

        /// <summary>
        ///     The sdmxObjects.
        /// </summary>
        private ISdmxObjects _sdmxObjects;

        /// <summary>
        ///     The cross referenced sdmxObjects.
        /// </summary>
        private IDictionaryOfSets<IIdentifiableObject, IIdentifiableObject> _crossReferencedObjects;

        /// <summary>
        ///     The structure writing manager.
        /// </summary>
        private readonly IStructureWriterManager _structureWritingManager = new StructureWriterManager();


        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWorkspace"/> class.
        /// </summary>
        /// <param name="sdmxObjects">
        /// The SDMX Objects
        /// </param>
        public StructureWorkspace(ISdmxObjects sdmxObjects)
            : this(sdmxObjects, null, false, false, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWorkspace"/> class.
        ///     Creates a workspace, gets all cross referenced objects using the retrieval manager (if it is not null)
        /// </summary>
        /// <param name="sdmxObjects">
        /// - the sdmxObjects to populate the workspace with
        /// </param>
        /// <param name="retrievalManager">
        /// - this will be used to retrieve all the cross references if it is not null
        /// </param>
        /// <param name="retrieveCrossReferences">
        /// The retrieve Cross References.
        /// </param>
        /// <param name="retrieveAgencies">
        /// - this will also retrieve all the agencies if true
        /// </param>
        /// <param name="resolutionDepth4">
        /// The resolution Depth.
        /// </param>
        public StructureWorkspace(
            ISdmxObjects sdmxObjects,
            IIdentifiableRetrievalManager retrievalManager,
            bool retrieveCrossReferences,
            bool retrieveAgencies,
            int resolutionDepth4)
        {
            if (sdmxObjects == null)
                throw new ArgumentException("Cannot instantiate a StructureWorkspace with beans as a null reference.");

            this._sdmxObjects = sdmxObjects;
            this._retrievalManager = retrievalManager;
            this._retrieveCrossRefefences = retrieveCrossReferences;
            this._retrieveAgencies = retrieveAgencies;
            this._resolutionDepth = resolutionDepth4;
            this.Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWorkspace"/> class.
        ///     Creates a workspace
        /// </summary>
        /// <param name="sdmxObjects">
        /// The SDMX Objects
        /// </param>
        /// <param name="crossReferencedBeans1">
        /// The cross Referenced Objects .
        /// </param>
        private StructureWorkspace(
            ISdmxObjects sdmxObjects, IDictionaryOfSets<IIdentifiableObject, IIdentifiableObject> crossReferencedBeans1)
        {
            if (sdmxObjects == null)
                throw new ArgumentException("Cannot instantiate a StructureWorkspace with beans as a null reference.");

            this._sdmxObjects = sdmxObjects;
            this._crossReferencedObjects = crossReferencedBeans1;
            this.Init();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the cross references, if it is null then the references were never resolved as there was no retrieval
        ///     manager provided on the construction of this class
        /// </summary>
        public virtual IDictionaryOfSets<IIdentifiableObject, IIdentifiableObject> CrossReferences
        {
            get
            {
                return this._crossReferencedObjects;
            }
        }

        /// <summary>
        ///     Gets the header that was present with the file
        /// </summary>
        /// <value></value>
        public virtual IHeader Header
        {
            get
            {
                return this._header;
            }
        }

        /// <summary>
        ///     Gets the super sdmxObjects.
        /// </summary>
        /// <exception cref="NotImplementedException">
        ///     BaseObjects/BaseObjects are not supported in this implementation
        /// </exception>
        public virtual IObjectsBase BaseObjects
        {
            get
            {
                throw new NotImplementedException("BaseObjects/BaseObjects are not supported in this implementation");
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets structure sdmxObjects.
        /// </summary>
        /// <param name="includeCrossReferences">
        /// The include cross references.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxObjects"/>.
        /// </returns>
        public virtual ISdmxObjects GetStructureObjects(bool includeCrossReferences)
        {
            if (includeCrossReferences && this._crossReferencedObjects != null)
            {
                return this._allSdmxObjects;
            }

            return this._sdmxObjects;
        }

        /// <summary>
        /// Get subset workspace from <paramref name="query"/>
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <returns>
        /// The <see cref="IStructureWorkspace"/>.
        /// </returns>
        public virtual IStructureWorkspace GetSubsetWorkspace(params IStructureReference[] query)
        {
            ISet<IMaintainableObject> maintainablesSubset = new HashSet<IMaintainableObject>();
            IDictionaryOfSets<IIdentifiableObject, IIdentifiableObject> crossReferencedSubset = new DictionaryOfSets<IIdentifiableObject, IIdentifiableObject>();

            for (int i = 0; i < query.Length; i++)
            {
                IStructureReference currentQuery = query[i];
                ISet<IMaintainableObject> maintainableForStructure =
                    this._sdmxObjects.GetMaintainables(currentQuery.MaintainableStructureEnumType.EnumType);
                ISet<IMaintainableObject> maintainableMatches =
                    MaintainableUtil<IMaintainableObject>.FindMatches(maintainableForStructure, currentQuery);
                maintainablesSubset.AddAll(maintainableMatches);

                /* foreach */
                foreach (IMaintainableObject currentMatch in maintainableMatches)
                {
                    ISet<IIdentifiableObject> identifiables = (this._crossReferencedObjects == null)
                                                                  ? (new HashSet<IIdentifiableObject>())
                                                                  : this._crossReferencedObjects[currentMatch];
                    if (identifiables != null)
                    {
                        crossReferencedSubset.Add(currentMatch, identifiables);
                    }
                }
            }

            ISdmxObjects beansSubset = new SdmxObjectsImpl(this._sdmxObjects.Header, maintainablesSubset);
            return new StructureWorkspace(beansSubset, crossReferencedSubset);
        }

        /// <summary>
        /// The merge workspace.
        /// </summary>
        /// <param name="workspace">
        /// The workspace.
        /// </param>
        public virtual void MergeWorkspace(IStructureWorkspace workspace)
        {
            this._allSdmxObjects = new SdmxObjectsImpl(this._allSdmxObjects, workspace.GetStructureObjects(true));
            this._sdmxObjects = new SdmxObjectsImpl(this._sdmxObjects, workspace.GetStructureObjects(false));

            IDictionary<IIdentifiableObject, ISet<IIdentifiableObject>> localCrossReferencedBeans =
                workspace.CrossReferences;
            if (localCrossReferencedBeans != null)
            {
                /* foreach */
                foreach (KeyValuePair<IIdentifiableObject, ISet<IIdentifiableObject>> currentKey in localCrossReferencedBeans)
                {
                    ISet<IIdentifiableObject> currentSet;
                    if (!this._crossReferencedObjects.TryGetValue(currentKey.Key, out currentSet))
                    {
                        currentSet = new HashSet<IIdentifiableObject>();
                        this._crossReferencedObjects.Add(currentKey.Key, currentSet);
                    }

                    currentSet.AddAll(currentKey.Value);
                }
            }
        }

        /// <summary>
        /// The write structures.
        /// </summary>
        /// <param name="structureType">
        /// The structure type.
        /// </param>
        /// <param name="outputStream">
        /// The output Stream.
        /// </param>
        /// <param name="includeCrossReferences">
        /// The include cross references.
        /// </param>
        public virtual void WriteStructures(SdmxSchema structureType, Stream outputStream, bool includeCrossReferences)
        {
            var outputFormat = StructureOutputFormatEnumType.Null /* was: null */;
            switch (structureType.EnumType)
            {
                case SdmxSchemaEnumType.Csv:
                    outputFormat = StructureOutputFormatEnumType.Csv;
                    break;
                case SdmxSchemaEnumType.Edi:
                    outputFormat = StructureOutputFormatEnumType.Edi;
                    break;
                case SdmxSchemaEnumType.VersionOne:
                    outputFormat = StructureOutputFormatEnumType.SdmxV1StructureDocument;
                    break;
                case SdmxSchemaEnumType.VersionTwo:
                    outputFormat = StructureOutputFormatEnumType.SdmxV2StructureDocument;
                    break;
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    outputFormat = StructureOutputFormatEnumType.SdmxV21StructureDocument;
                    break;
            }

            StructureOutputFormat output = StructureOutputFormat.GetFromEnum(outputFormat);
            if (includeCrossReferences)
            {
                this._structureWritingManager.WriteStructures(this._allSdmxObjects, new SdmxStructureFormat(output), outputStream);
            }
            else
            {
                this._structureWritingManager.WriteStructures(this._sdmxObjects, new SdmxStructureFormat(output), outputStream);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Initialize the instance.
        /// </summary>
        private void Init()
        {
            if (this._retrieveCrossRefefences)
            {
                ICrossReferenceResolverEngine crossRefResolver = new CrossReferenceResolverEngineCore();
                this._crossReferencedObjects = crossRefResolver.ResolveReferences(
                    this._sdmxObjects, this._retrieveAgencies, this._resolutionDepth, this._retrievalManager);
            }

            this._allSdmxObjects = new SdmxObjectsImpl(this._sdmxObjects);
            if (this._crossReferencedObjects != null)
            {
                /* foreach */
                foreach (ISet<IIdentifiableObject> currentBeanSet in this._crossReferencedObjects.Values)
                {
                    /* foreach */
                    foreach (IIdentifiableObject currentBean in currentBeanSet)
                    {
                        this._allSdmxObjects.AddIdentifiable(currentBean);
                    }
                }
            }
        }

        #endregion
    }
}