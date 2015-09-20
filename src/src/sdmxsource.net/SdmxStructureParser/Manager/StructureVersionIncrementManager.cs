// -----------------------------------------------------------------------
// <copyright file="StructureVersionIncrementManager.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Manager
{
    using System;
    using System.Collections.Generic;
    using log4net;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.CrossReference;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Structureparser.Engine;
    using Org.Sdmxsource.Sdmx.Structureparser.Engine.Reversion;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class StructureVersionIncrementManager : IStructureVersionIncrementManager
    {
        #region Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(StructureVersionIncrementManager));

        /// <summary>
        ///     The structure version retrieval manager.
        /// </summary>
        private readonly IStructureVersionRetrievalManager _structureVersionRetrievalManager;

        /// <summary>
        ///     The cross referencing retrieval manager.
        /// </summary>
        private ICrossReferencingRetrievalManager _crossReferencingRetrievalManager;

        /// <summary>
        ///     The cross reference reversion engine.
        /// </summary>
        private readonly ICrossReferenceReversionEngine _crossReferenceReversionEngine;

        /// <summary>
        ///     The object retrieval manager.
        /// </summary>
        private ISdmxObjectRetrievalManager _beanRetrievalManager;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureVersionIncrementManager" /> class.
        /// </summary>
        /// <param name="beanRetrievalManager">The bean retrieval manager.</param>
        /// <param name="crossReferenceReversionEngine">The cross reference reversion engine.</param>
        /// <param name="crossReferencingRetrievalManager">The cross referencing retrieval manager.</param>
        /// <param name="structureVersionRetrievalManager">The structure version retrieval manager.</param>
        public StructureVersionIncrementManager(ISdmxObjectRetrievalManager beanRetrievalManager, ICrossReferenceReversionEngine crossReferenceReversionEngine, ICrossReferencingRetrievalManager crossReferencingRetrievalManager, IStructureVersionRetrievalManager structureVersionRetrievalManager)
        {
            this._beanRetrievalManager = beanRetrievalManager;
            this._crossReferenceReversionEngine = crossReferenceReversionEngine ?? new CrossReferenceReversionEngine();
            this._crossReferencingRetrievalManager = crossReferencingRetrievalManager;
            this._structureVersionRetrievalManager = structureVersionRetrievalManager;
        }

        #region Public Properties

        /// <summary>
        /// Sets the cross referencing retrieval manager.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetCrossReferencingRetrievalManager(ICrossReferencingRetrievalManager value)
        {
            this._crossReferencingRetrievalManager = value;
        }

        /// <summary>
        /// Sets the object retrieval manager.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetObjectRetrievalManager(ISdmxObjectRetrievalManager value)
        {
            this._beanRetrievalManager = value;
        }

        #endregion

        /// <summary>
        /// Increments the versions of sdmx objects
        /// </summary>
        /// <param name="sdmxObjects">
        /// The sdmx objects.
        /// </param>
        public void IncrementVersions(ISdmxObjects sdmxObjects)
        {
            _log.Info("Update Versions of Structures if existing structures found");
            //Store a map of old versions vs the new version
            IDictionary<IStructureReference, IStructureReference> oldVsNew =
                new Dictionary<IStructureReference, IStructureReference>();
            IDictionary<IMaintainableObject, IMaintainableObject> oldMaintVsNew =
                new Dictionary<IMaintainableObject, IMaintainableObject>();
            ISet<IMaintainableObject> updatedMaintainables = new HashSet<IMaintainableObject>();
            ISet<IMaintainableObject> oldMaintainables = new HashSet<IMaintainableObject>();

            foreach (IMaintainableObject currentMaint in sdmxObjects.GetAllMaintainables())
            {
                _log.Debug("Auto Version - check latest version for maintainable: " + currentMaint);

                IMaintainableObject persistedMaintainable = this._structureVersionRetrievalManager.GetLatest(currentMaint);
                if (persistedMaintainable == null)
                {
                    persistedMaintainable = this._beanRetrievalManager.GetMaintainableObject(currentMaint.AsReference);
                }
                if (persistedMaintainable != null)
                {
                    if (VersionableUtil.IsHigherVersion(persistedMaintainable.Version, currentMaint.Version))
                    {
                        //Modify version of maintainable to be the same as persisted maintainable
                        IMaintainableMutableObject mutableInstance = currentMaint.MutableInstance;
                        mutableInstance.Version = persistedMaintainable.Version;

                        //Remove the Maintainable from the submission - as we've changed the versions
                        sdmxObjects.RemoveMaintainable(currentMaint);

                        //currentMaint = mutableInstance.ImmutableInstance;
                    }
                    if (persistedMaintainable.Version.Equals(currentMaint.Version))
                    {
                        _log.Debug("Latest version is '" + persistedMaintainable.Version + "' perform update checks");
                        if (!currentMaint.DeepEquals(persistedMaintainable, true))
                        {
                            ISet<IIdentifiableObject> allIdentifiables1 = currentMaint.IdentifiableComposites;
                            ISet<IIdentifiableObject> allIdentifiables2 = persistedMaintainable.IdentifiableComposites;

                            bool containsAll = allIdentifiables1.ContainsAll(allIdentifiables2)
                                               && allIdentifiables2.ContainsAll(allIdentifiables1);
                            if (_log.IsInfoEnabled)
                            {
                                string increment = containsAll ? "Minor" : "Major";
                                _log.Info("Perform " + increment + " Version Increment for structure:" + currentMaint.Urn);
                            }

                            //Increment the version number
                            IMaintainableObject newVersion = this.IncrmentVersion(
                                currentMaint, persistedMaintainable.Version, !containsAll);

                            //Remove the Maintainable from the submission
                            sdmxObjects.RemoveMaintainable(currentMaint);

                            //Store the newly updated maintainable in a container for further processing
                            updatedMaintainables.Add(newVersion);
                            oldMaintainables.Add(currentMaint);
                            //Store the old version number mappings to the new version number
                            oldMaintVsNew.Add(currentMaint, newVersion);
                            oldVsNew.Add(currentMaint.AsReference, newVersion.AsReference);

                            string oldVersionNumber = currentMaint.Version;
                            AddOldVsNewReferences(oldVersionNumber, newVersion, oldVsNew);
                        }
                    }
                }
            }

            //Create a set of parent sdmxObjects to not update (regardless of version)
            ISet<IMaintainableObject> filterSet = new HashSet<IMaintainableObject>(updatedMaintainables);
            filterSet.AddAll(sdmxObjects.GetAllMaintainables());

            //Get all the referencing structures to reversion them
            IEnumerable<IMaintainableObject> referencingStructures = this.RecurseUpTree(oldMaintainables, new HashSet<IMaintainableObject>(), filterSet);

            foreach (IMaintainableObject currentReferencingStructure in referencingStructures)
            {
                _log.Info("Perform Minor Version Increment on referencing structure:" + currentReferencingStructure);
                String newVersionNumber;
                if (oldMaintVsNew.ContainsKey(currentReferencingStructure))
                {
                    //The old maintainable is also in the submission and has had it's version number incremented, use this version
                    var tmp = oldMaintVsNew[currentReferencingStructure];
                    //currentReferencingStructure = oldMaintVsNew[currentReferencingStructure];
                    updatedMaintainables.Remove(tmp);
                    newVersionNumber = currentReferencingStructure.Version;
                }
                else
                {
                    newVersionNumber = VersionableUtil.IncrementVersion(currentReferencingStructure.Version, false);
                }
                IMaintainableObject updatedMaintainable =
                    this._crossReferenceReversionEngine.UdpateReferences(
                        currentReferencingStructure, oldVsNew, newVersionNumber);
                AddOldVsNewReferences(currentReferencingStructure.Version, updatedMaintainable, oldVsNew);

                updatedMaintainables.Add(updatedMaintainable);
            }

            foreach (IMaintainableObject currentReferencingStructure in updatedMaintainables)
            {
                IMaintainableObject updatedMaintainable =
                    this._crossReferenceReversionEngine.UdpateReferences(
                        currentReferencingStructure, oldVsNew, currentReferencingStructure.Version);
                sdmxObjects.AddIdentifiable(updatedMaintainable);
            }

            //Update the references of any structures that existed in the submission
            foreach (IMaintainableObject currentReferencingStructure in sdmxObjects.GetAllMaintainables())
            {
                IMaintainableObject updatedMaintainable =
                    this._crossReferenceReversionEngine.UdpateReferences(
                        currentReferencingStructure, oldVsNew, currentReferencingStructure.Version);
                sdmxObjects.AddIdentifiable(updatedMaintainable);
            }
        }

        /// <summary>
        /// Recurse all the way up the tree until all referenced structures are found - in the order in which they are found
        /// </summary>
        /// <param name="getParentsFor">
        /// The list of structures to get the parents for.
        /// </param>
        /// <param name="ignoreParents">
        /// The list of structures to ignore the parents.
        /// </param>
        /// <param name="filterSet">
        /// The filter set.
        /// </param>
        /// /// <returns>
        /// The referenced structures.
        /// </returns>
        private IEnumerable<IMaintainableObject> RecurseUpTree(
            IEnumerable<IMaintainableObject> getParentsFor,
            ISet<IMaintainableObject> ignoreParents,
            ISet<IMaintainableObject> filterSet)
        {
            var crossReferencingStructures = new List<IMaintainableObject>();

            foreach (IMaintainableObject oldBean in getParentsFor)
            {
                crossReferencingStructures.AddAll(this._crossReferencingRetrievalManager.GetCrossReferencingStructures(oldBean.AsReference, false));
            }
            //Filter out the parents we do not want to reversion
            crossReferencingStructures.RemoveItemList(ignoreParents);
            this.FilterReferencingStructures(crossReferencingStructures, filterSet);

            ignoreParents.AddAll(crossReferencingStructures);

            if (crossReferencingStructures.Count > 0)
            {
                IEnumerable<IMaintainableObject> ancestors = this.RecurseUpTree(
                    crossReferencingStructures, ignoreParents, filterSet);
                foreach (IMaintainableObject currentAncestor in ancestors)
                {
                    if (!crossReferencingStructures.Contains(currentAncestor))
                    {
                        crossReferencingStructures.AddAll(ancestors);
                    }
                }
            }
            return crossReferencingStructures;
        }

        /// <summary>
        /// Filters referencing structures
        /// </summary>
        /// <param name="refereningStructures">
        /// The collection of referencing structures.
        /// </param>
        /// <param name="alreadyReversionedMaintainables">
        /// The set of already reversioned maintainables.
        /// </param>
        private void FilterReferencingStructures(
            ICollection<IMaintainableObject> refereningStructures,
            ISet<IMaintainableObject> alreadyReversionedMaintainables)
        {
            ISet<IMaintainableObject> removeSet = new HashSet<IMaintainableObject>();
            foreach (IMaintainableObject currentReference in refereningStructures)
            {
                foreach (IMaintainableObject alreadyReversionedReference in alreadyReversionedMaintainables)
                {
                    if (currentReference.StructureType == alreadyReversionedReference.StructureType)
                    {
                        if (currentReference.AgencyId.Equals(alreadyReversionedReference.AgencyId))
                        {
                            if (currentReference.Id.Equals(alreadyReversionedReference.Id))
                            {
                                removeSet.Add(currentReference);
                            }
                        }
                    }
                }
            }
            refereningStructures.RemoveItemList(removeSet);
        }

        /// <summary>
        /// Maps old versus new references.
        /// </summary>
        /// <param name="oldVersionNumber">
        /// The old version number.
        /// </param>
        /// <param name="newVersion">
        /// The new version object.
        /// </param>
        /// <param name="oldVsNew">
        /// The old vs new map.
        /// </param>
        private void AddOldVsNewReferences(
            String oldVersionNumber,
            IMaintainableObject newVersion,
            IDictionary<IStructureReference, IStructureReference> oldVsNew)
        {
            this.AddOldVsNewReferencesToMap(oldVersionNumber, newVersion, oldVsNew);
            foreach (IIdentifiableObject composite in newVersion.IdentifiableComposites)
            {
                this.AddOldVsNewReferencesToMap(oldVersionNumber, composite, oldVsNew);
            }
        }

        /// <summary>
        /// Adds new elements to the old versus new references.
        /// </summary>
        /// <param name="oldVersionNumber">
        /// The old version number.
        /// </param>
        /// <param name="newVersion">
        /// The new version object.
        /// </param>
        /// <param name="oldVsNew">
        /// The old vs new map.
        /// </param>
        private void AddOldVsNewReferencesToMap(
            String oldVersionNumber,
            IIdentifiableObject newVersion,
            IDictionary<IStructureReference, IStructureReference> oldVsNew)
        {
            IStructureReference asReference = newVersion.AsReference;
            IMaintainableRefObject mRef = asReference.MaintainableReference;
            IStructureReference oldReference = new StructureReferenceImpl(
                mRef.AgencyId,
                mRef.MaintainableId,
                oldVersionNumber,
                asReference.TargetReference,
                asReference.IdentifiableIds);
            oldVsNew.Add(oldReference, asReference);
        }

        /// <summary>
        /// Increments the version of the old maintainable
        /// </summary>
        /// <param name="currentMaint">
        /// current maintainable
        /// </param>
        /// <param name="incrementFromVersion">
        /// the increment from version
        /// </param>
        /// <param name="majorIncrement">
        /// the major increment
        /// </param>
        /// <returns>
        /// The mantainable object with incremented version
        /// </returns>
        private IMaintainableObject IncrmentVersion(
            IMaintainableObject currentMaint, String incrementFromVersion, bool majorIncrement)
        {
            IMaintainableMutableObject mutable = currentMaint.MutableInstance;
            String newVersion = VersionableUtil.IncrementVersion(incrementFromVersion, majorIncrement);
            mutable.Version = newVersion;

            IMaintainableObject newMaint = mutable.ImmutableInstance;
            return newMaint;
        }
    }
}
