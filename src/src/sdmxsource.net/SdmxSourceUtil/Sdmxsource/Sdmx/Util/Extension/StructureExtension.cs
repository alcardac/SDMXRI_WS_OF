// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureExtension.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Extension
{
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// The structure extensions methods.
    /// </summary>
    public static class StructureExtension
    {
        /// <summary>
        /// Changes the stars <c>*</c>  to null.
        /// </summary>
        /// <param name="reference">The reference.</param>
        public static IStructureReference ChangeStarsToNull(this IStructureReference reference)
        {
            if (reference == null)
            {
                return null;
            }

            if (!reference.HasAgencyId() && !reference.HasMaintainableId() && !reference.HasVersion())
            {
                return reference;
            }

            if (SdmxConstants.RestWildcard.Equals(reference.AgencyId) || SdmxConstants.RestWildcard.Equals(reference.MaintainableId) || SdmxConstants.RestWildcard.Equals(reference.Version))
            {
                string agencyId = SdmxConstants.RestWildcard.Equals(reference.AgencyId) ? null : reference.AgencyId;
                string version = SdmxConstants.RestWildcard.Equals(reference.Version) ? null : reference.Version;
                string id = SdmxConstants.RestWildcard.Equals(reference.MaintainableId) ? null : reference.MaintainableId;
                if (reference.HasChildReference())
                {
                    return new StructureReferenceImpl(agencyId, id, version, reference.TargetReference, reference.IdentifiableIds);
                }

                return new StructureReferenceImpl(agencyId, id, version, reference.TargetReference);
            }

            return reference;
        }

        /// <summary>
        /// Returns the id from the leaf <see cref="IIdentifiableRefObject"/>
        /// </summary>
        /// <param name="identifiableRefObject">
        /// The <see cref="IIdentifiableRefObject"/>.
        /// </param>
        /// <returns>
        /// The the id from the leaf <see cref="IIdentifiableRefObject"/> from the specified <paramref name="identifiableRefObject"/>; otherwise null.
        /// </returns>
        public static string GetLastId(this IIdentifiableRefObject identifiableRefObject)
         {
             if (identifiableRefObject == null)
             {
                 return null;
             }

             while (identifiableRefObject.ChildReference != null)
             {
                 identifiableRefObject = identifiableRefObject.ChildReference;
             }

             return identifiableRefObject.Id;
         }
    }
}