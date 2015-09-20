// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaintainableSortByIdentifiers.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Sort
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    /// The maintainable sort by identifiers.
    /// </summary>
    /// <typeparam name="T">Generic type param
    /// </typeparam>
    public class MaintainableSortByIdentifiers<T> : IComparer<T>
        where T : IMaintainableObject
    {
        #region Public Methods and Operators

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.Value Meaning Less than zero<paramref name="x"/> is less than <paramref name="y"/>.Zero<paramref name="x"/> equals <paramref name="y"/>.Greater than zero<paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        /// <param name="x">The first object to compare.</param><param name="y">The second object to compare.</param>
        public int Compare(T x, T y)
        {
            if (EqualityComparer<T>.Default.Equals(x, y))
            {
                return 0;
            }

            if (EqualityComparer<T>.Default.Equals(x, default(T)))
            {
                return -1;
            }

            if (EqualityComparer<T>.Default.Equals(y, default(T)))
            {
                return 1;
            }

            int comp = string.CompareOrdinal(x.StructureType.StructureType, y.StructureType.StructureType);
            if (comp != 0)
            {
                return comp;
            }

            string agencyId1 = x.AgencyId;
            string agencyId2 = y.AgencyId;
            comp = string.CompareOrdinal(agencyId1, agencyId2);
            if (comp != 0)
            {
                return comp;
            }

            string id1 = x.Id;
            string id2 = y.Id;
            comp = string.CompareOrdinal(id1, id2);
            if (comp != 0)
            {
                return comp;
            }

            string v1 = x.Version;
            string v2 = y.Version;
            if (v1.Equals(v2))
            {
                // SHOULD NEVER HAPPEN
                // TODO why?
                return -1;
            }

            return VersionableUtil.IsHigherVersion(v2, v1) ? -1 : 1;
        }

        #endregion
    }
}