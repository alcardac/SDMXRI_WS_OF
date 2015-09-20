// -----------------------------------------------------------------------
// <copyright file="QueryExtensions.cs" company="Eurostat">
//   Date Created : 2013-06-13
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Extensions
{
    using Estat.Sri.MappingStoreRetrieval.Constants;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;

    /// <summary>
    /// Various Structure query related extensions
    /// </summary>
    public static class QueryExtensions
    {
        /// <summary>
        /// Returns the version constraints.
        /// </summary>
        /// <param name="complexStructureReferenceObject">
        /// The complex structure reference object.
        /// </param>
        /// <returns>
        /// The <see cref="VersionQueryType"/>.
        /// </returns>
        public static VersionQueryType GetVersionConstraints(this IComplexStructureReferenceObject complexStructureReferenceObject)
        {
            return complexStructureReferenceObject.VersionReference != null && complexStructureReferenceObject.VersionReference.IsReturnLatest.IsTrue
                       ? VersionQueryType.Latest
                       : VersionQueryType.All;
        }

        /// <summary>
        /// Returns the version constraints.
        /// </summary>
        /// <param name="returnLatest">
        /// The return Latest.
        /// </param>
        /// <returns>
        /// The <see cref="VersionQueryType"/>.
        /// </returns>
        public static VersionQueryType GetVersionConstraints(this bool returnLatest)
        {
            return returnLatest
                       ? VersionQueryType.Latest
                       : VersionQueryType.All;
        }
    }
}