// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentifiableComparator.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Sort
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// The Identifiable Comparator.
    /// </summary>
    public class IdentifiableComparator : IComparer<IIdentifiableObject>
    {
        /// <summary>
        /// Compares two identifiables by name or by Urn
        /// </summary>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.Value Meaning Less than zero<paramref name="x"/> is less than <paramref name="y"/>.Zero<paramref name="x"/> equals <paramref name="y"/>.Greater than zero<paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        /// <param name="x">The first IIdentifiableObject to compare.</param>
        /// <param name="y">The second IIdentifiableObject to compare.</param>
        public int Compare(IIdentifiableObject x, IIdentifiableObject y)
        {
            string id1 = x.Id;
            string id2 = y.Id;

            int compare = String.Compare(id1, id2, System.StringComparison.Ordinal);

            if (compare == 0)
                return Uri.Compare(x.Urn, y.Urn, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, System.StringComparison.Ordinal);

            return compare;
        }
    }
}
