// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaintainableUtil.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Objects
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;

    #endregion


    /// <summary>
    ///   This class provides utility methods that can be performed on a IMaintainableObject
    /// </summary>
    /// <typeparam name="T"> Generic type param </typeparam>
    public class MaintainableUtil<T>
        where T : IMaintainableObject
    {
        #region Public Methods and Operators

        /// <summary>
        ///   For a set of maintainables, this method will return the maintainable that has the same urn as the ref bean.
        ///   <p />
        ///   If the ref bean does not have a urn, then it will return any matches for the agency id, id and version
        ///   as the ref bean. 
        ///   <p />
        ///   If the ref bean does not have a urn OR agency id, id and version set then it will return all the beans.
        ///   <p />
        ///   <p />
        /// </summary>
        /// <typeparam name="TMaint"> Type param - IMaintainableObject </typeparam>
        /// <param name="populateCollection"> The populate Collection. </param>
        /// <param name="maintianables"> Collection of maintainable objects </param>
        /// <param name="structureReference"> The structure Reference. </param>
        public static void FindMatches<TMaint>(
            ICollection<IMaintainableObject> populateCollection,
            ICollection<TMaint> maintianables,
            IStructureReference structureReference) where TMaint : IMaintainableObject
        {
            if (structureReference == null)
            {
                throw new ArgumentNullException("structureReference");
            }

            if (maintianables != null)
            {
                foreach (TMaint currentMaintainable in maintianables)
                {
                    if (Match(currentMaintainable, structureReference))
                    {
                        populateCollection.Add(currentMaintainable);
                    }
                }
            }
        }

        /// <summary>
        ///   For a set of maintainables, this method will return the maintainable that has the same urn as the ref bean.
        ///   <p />
        ///   If the ref bean does not have a urn, then it will return any matches for the agency id, id and version
        ///   as the ref bean. 
        ///   <p />
        ///   If the ref bean does not have a urn OR agency id, id and version set then it will return all the beans.
        ///   <p />
        ///   <p />
        /// </summary>
        /// <typeparam name="TMaint"> Generic type parameter - IMaintainableObject </typeparam>
        /// <param name="maintianables"> Collection of maintainables </param>
        /// <param name="structureReference"> Structure reference </param>
        /// <returns> The sub set of <paramref name="maintianables" /> that match the <paramref name="structureReference" /> </returns>
        public static ISet<IMaintainableObject> FindMatches<TMaint>(
            ICollection<TMaint> maintianables, IStructureReference structureReference)
            where TMaint : IMaintainableObject
        {
            if (structureReference == null)
            {
                throw new ArgumentNullException("structureReference");
            }

            ISet<IMaintainableObject> returnSet = new HashSet<IMaintainableObject>();

            FindMatches(returnSet, maintianables, structureReference);

            return returnSet;
        }

        /// <summary>
        ///   The match.
        /// </summary>
        /// <param name="maint"> The maint. </param>
        /// <param name="structureReference"> The structureReference. </param>
        /// <returns> The <see cref="bool" /> . </returns>
        public static bool Match(IMaintainableObject maint, IStructureReference structureReference)
        {
            if (structureReference == null)
            {
                return true;
            }

            return structureReference.IsMatch(maint);
        }

        /// <summary>
        ///   For a set of maintainables, this method will return the maintainable that matches the parameters of the ref bean.
        ///   <p />
        ///   If the ref bean does not have a urn, then it will return the maintainable that has the same agency id, id and version
        ///   as the ref bean. 
        ///   <p />
        ///   If the ref bean does not have a urn OR agency id, id and version set then it will result in an error.
        ///   <p />
        ///   This method will stop at the first match and return it, no checks are performed on the type of maintainables in the set.
        ///   <p />
        ///   If no match is found null is returned.
        /// </summary>
        /// <typeparam name="TMaint"> Genric type param - IMaintainableObject </typeparam>
        /// <param name="maintianables"> Collection of maintianables objects </param>
        /// <param name="structureReference"> The structure reference object. </param>
        /// <returns> The <see cref="IMaintainableObject" /> . </returns>
        public static IMaintainableObject ResolveReference(
            ICollection<T> maintianables, IStructureReference structureReference)
        {
            if (structureReference == null)
            {
                throw new ArgumentNullException("structureReference");
            }

            IMaintainableRefObject maintainableReference = structureReference.MaintainableReference;
            if (structureReference.TargetUrn == null
                &&
                (string.IsNullOrWhiteSpace(maintainableReference.AgencyId)
                 || string.IsNullOrWhiteSpace(maintainableReference.MaintainableId)
                 || string.IsNullOrWhiteSpace(maintainableReference.Version)))
            {
                throw new ArgumentException("Ref requires a URN or AgencyId, Maintainable Id and Version");
            }

            if (maintianables != null)
            {
                foreach (T currentMaintainable in maintianables)
                {
                    if (Match(currentMaintainable, structureReference))
                    {
                        return currentMaintainable;
                    }

                }
            }

            return null;
        }

        /// <summary>
        ///   Resolve reference.
        /// </summary>
        /// <param name="maintainables"> The maintainables. </param>
        /// <param name="maintainableRef"> The structure Reference. </param>
        /// <typeparam name="TMaint"> Generic type param </typeparam>
        /// <returns> The <see cref="IMaintainableObject" /> . </returns>
        /// <exception cref="ArgumentException">Throws ArgumentException</exception>
        public static IMaintainableObject ResolveReference<TMaint>(
            ICollection<TMaint> maintainables, IMaintainableRefObject maintainableRef)
            where TMaint : IMaintainableObject
        {
            if (!ObjectUtil.ValidCollection(maintainables))
            {
                return null;
            }

            if (maintainableRef == null)
            {
                if (maintainables.Count == 1)
                {
                    return maintainables.GetEnumerator().Current;
                }
                throw new ArgumentNullException("Can not resolve reference, more then one bean supplied and no reference parameters passed in");
            }

            if (maintainables != null)
            {
                foreach (TMaint currentMaintainable in maintainables)
                {
                    if (maintainableRef.AgencyId == null || currentMaintainable.AgencyId.Equals(maintainableRef.AgencyId))
                    {
                        if (maintainableRef.MaintainableId == null || currentMaintainable.Id.Equals(maintainableRef.MaintainableId))
                        {
                            if (maintainableRef.Version == null || currentMaintainable.Version.Equals(maintainableRef.Version))
                            {
                                return currentMaintainable;
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        ///   Returns true if the <paramref name="superSet" /> is a subset of <paramref name="subSet" />
        /// </summary>
        /// <param name="superSet"> The super set reference </param>
        /// <param name="subSet"> The sub set reference </param>
        /// <returns> The true if the <paramref name="superSet" /> is a subset of <paramref name="subSet" /> ; otherwise false </returns>
        public static bool SubsetOf(IMaintainableRefObject superSet, IMaintainableRefObject subSet)
        {
            bool match = true;

            if (string.IsNullOrWhiteSpace(superSet.AgencyId))
            {
                match = superSet.AgencyId.Equals(subSet.AgencyId);
            }

            if (match)
            {
                if (string.IsNullOrWhiteSpace(superSet.MaintainableId))
                {
                    match = superSet.MaintainableId.Equals(subSet.MaintainableId);
                }
            }

            if (match)
            {
                if (string.IsNullOrWhiteSpace(superSet.Version))
                {
                    match = superSet.Version.Equals(subSet.Version);
                }
            }

            return match;
        }

        /// <summary>
        ///   Returns a subset of <paramref name="maintianables" /> that match <paramref name="maintainableRef" /> <see
        ///    cref="IMaintainableRefObject.AgencyId" />, <see cref="IMaintainableRefObject.MaintainableId" /> and <see
        ///    cref="IMaintainableRefObject.Version" />
        /// </summary>
        /// <param name="maintianables"> The maintianables. </param>
        /// <param name="maintainableRef"> The maintainable reference. Null is consindered to be a wild card </param>
        /// <returns> The a subset of <paramref name="maintianables" /> that match <paramref name="maintainableRef" /> </returns>
        public ISet<T> FilterCollection(ICollection<T> maintianables, IMaintainableRefObject maintainableRef)
        {
            ISet<T> returnSet = new HashSet<T>();

            string agencyId = null;
            string id = null;
            string version = null;

            if (maintainableRef != null)
            {
                agencyId = maintainableRef.AgencyId;
                id = maintainableRef.MaintainableId;
                version = maintainableRef.Version;
            }

            foreach (T currentMaintainable in maintianables)
            {
                if (agencyId == null || currentMaintainable.AgencyId.Equals(agencyId))
                {
                    if (id == null || currentMaintainable.Id.Equals(id))
                    {
                        if (version == null || currentMaintainable.Version.Equals(version))
                        {
                            returnSet.Add(currentMaintainable);
                        }
                    }
                }
            }

            return returnSet;
        }

        /// <summary>
        /// Returns a collection keeping only the latest versions of each maintainable passed in.
        /// Note, the returned Set is a new Set, no changes are made to the passed in collection.
        /// </summary>
        /// <param name="maintianables"></param>
        /// <returns></returns>
        public static ISet<IMaintainableObject> FilterCollectionGetLatest(
            ICollection<IMaintainableObject> maintianables)
        {
            IDictionary<string, IMaintainableObject> resultMap = new Dictionary<string, IMaintainableObject>();
            bool filteredResponse = false;
            foreach (IMaintainableObject currentMaint in maintianables)
            {
                string key = currentMaint.StructureType.StructureType + "_" + currentMaint.AgencyId + "_" + currentMaint.Id;
                if (resultMap.ContainsKey(key))
                {
                    filteredResponse = true;
                    IMaintainableObject storedAgainstKey = resultMap[key];
                    if (VersionableUtil.IsHigherVersion(currentMaint.Version, storedAgainstKey.Version))
                    {
                        resultMap.Add(key, currentMaint);
                    }
                }
                else
                {
                    resultMap.Add(key, currentMaint);
                }
            }
            if (filteredResponse)
            {
                return new HashSet<IMaintainableObject>(resultMap.Values);
            }
            return new HashSet<IMaintainableObject>(maintianables);
        }

        /// <summary>
        /// Returns a collection keeping only the latest versions of each maintainable passed in.
        /// <p/>
        /// Note, the returned Set is a new Set, no changes are made to the passed in collection.
        /// </summary>
        /// <param name="maintainables">
        /// The maintainables.
        /// </param>
        /// <returns>
        /// The a subset of <paramref name="maintainables" /> that match <paramref name="maintainableRef" />
        /// </returns>
        public ISet<T> FilterCollectionGetLatestOfType(ICollection<T> maintainables)
        {
            IDictionary<string, T> resultMap = new Dictionary<string, T>();

            bool filteredResponse = false;

            foreach (T currentMaint in maintainables)
            {
                string key = currentMaint.AgencyId + "_" + currentMaint.Id;

                if (resultMap.ContainsKey(key))
                {
                    filteredResponse = true;
                    T storedAgainstKey = resultMap[key];

                    if (VersionableUtil.IsHigherVersion(currentMaint.Version, storedAgainstKey.Version))
                    {
                        resultMap.Add(key, currentMaint);
                    }

                }
                else
                {
                    resultMap.Add(key, currentMaint);
                }
            }

            if (filteredResponse)
            {
                return new HashSet<T>(resultMap.Values);
            }

            return new HashSet<T>(maintainables);
        }

        #endregion
    }
}