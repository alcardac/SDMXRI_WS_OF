// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataflowCompare.cs" company="Eurostat">
//   Date Created : 2011-05-20
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Comparer for <see cref="DataflowRefBean" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// Comparer for <see cref="IMaintainableRefObject"/>
    /// </summary>
    internal class DataflowCompare : EqualityComparer<IMaintainableRefObject>
    {
        #region Public Methods

        /// <summary>
        /// Determines whether two objects of type DataflowRefBean are equal.
        /// </summary>
        /// <returns>
        /// true if the specified DataflowRefBean are equal; otherwise, false.
        /// </returns>
        /// <param name="x">
        /// The first object to compare.
        ///                 </param>
        /// <param name="y">
        /// The second object to compare.
        /// </param>
        public override bool Equals(IMaintainableRefObject x, IMaintainableRefObject y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null)
            {
                return false;
            }

            if (y == null)
            {
                return false;
            }

            return string.Equals(x.MaintainableId, y.MaintainableId) && string.Equals(x.AgencyId, y.AgencyId)
                   && string.Equals(x.Version, y.Version);
        }

        /// <summary>
        /// A hash function for the specified DataflowRefBean for hashing algorithms and data structures, such as a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the specified DataflowRefBean object.
        /// </returns>
        /// <param name="obj">
        /// The DataflowRefBean object for which to get a hash code. if null it returns 0
        /// </param>
        public override int GetHashCode(IMaintainableRefObject obj)
        {
            if (obj == null)
            {
                return 0;
            }

            int hash = (obj.MaintainableId ?? string.Empty).GetHashCode() ^ (obj.AgencyId ?? string.Empty).GetHashCode()
                       ^ (obj.Version ?? string.Empty).GetHashCode();
            return hash;
        }

        #endregion
    }
}