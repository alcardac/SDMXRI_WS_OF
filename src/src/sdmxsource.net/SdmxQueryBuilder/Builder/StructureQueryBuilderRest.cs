// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureQueryBuilderRest.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The Reststructure query builder implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Builder
{
    #region Using Directives

    using System;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using Org.Sdmxsource.Sdmx.Util.Extension;

    #endregion

    /// <summary>
    /// TODO
    /// </summary>
    public class StructureQueryBuilderRest : IStructureQueryBuilder<string>
    {

        #region Public Methods and Operators

        /// <summary>
        /// Builds a StructureQuery that matches the passed in format
        /// </summary>
        /// <param name="structureQuery">
        /// The query
        /// </param>
        /// <returns>
        /// The string
        /// </returns>
        public string BuildStructureQuery(IRestStructureQuery structureQuery)
        {
            if (structureQuery == null)
            {
                throw new ArgumentNullException("structureQuery");
            }

            string returnUrl = "";

            // the ChangeStarsToNull is needed because of a bug in SdmxSource 1.1.4 Java (and possibly in 1.2.3). 
            // Test TestQueryBuilder.TestRestStructureQueryRoundTrip
            IStructureReference structureReference = structureQuery.StructureReference.ChangeStarsToNull();

            if (structureReference.MaintainableStructureEnumType == SdmxStructureEnumType.Any)
            {
                returnUrl += "structure/";
            }
            else if (structureReference.MaintainableStructureEnumType == SdmxStructureEnumType.OrganisationScheme)
            {
                returnUrl += "organisationscheme/";
            }
            else
            {
                returnUrl += structureReference.MaintainableStructureEnumType.UrnClass.ToLower() + "/";
            }

            IMaintainableRefObject maintainableRefObject = structureReference.MaintainableReference;

            if (!string.IsNullOrWhiteSpace(maintainableRefObject.AgencyId))
            {
                returnUrl += maintainableRefObject.AgencyId + "/";
            }
            else
            {
                returnUrl += "all/";
            }

            if (!string.IsNullOrWhiteSpace(maintainableRefObject.MaintainableId))
            {
                returnUrl += maintainableRefObject.MaintainableId + "/";
            }
            else
            {
                returnUrl += "all/";
            }

            IStructureQueryMetadata structureQueryMetadata = structureQuery.StructureQueryMetadata;
            if (structureQueryMetadata.IsReturnLatest)
            {
                returnUrl += "latest/";
            }
            else if (!string.IsNullOrWhiteSpace(maintainableRefObject.Version))
            {
                returnUrl += maintainableRefObject.Version + "/";
            }
            else
            {
                returnUrl += "all/";
            }

            string concat = "?";
            if (structureQueryMetadata.SpecificStructureReference != null)
            {
                returnUrl += concat + "references=" + structureQueryMetadata.SpecificStructureReference.UrnClass.ToLower();
                concat = "&";
            }
            else if (structureQueryMetadata.StructureReferenceDetail != null)
            {
                returnUrl += concat + "references=" + structureQueryMetadata.StructureReferenceDetail.ToString();
                concat = "&";
            }

            if (structureQueryMetadata.StructureQueryDetail != null)
            {
                returnUrl += concat + "detail=" + structureQueryMetadata.StructureQueryDetail.ToString();
                concat = "&";
            }

            return returnUrl;
        }

        #endregion
    }
}
