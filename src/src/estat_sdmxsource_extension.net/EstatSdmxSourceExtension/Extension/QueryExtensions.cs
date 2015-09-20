// -----------------------------------------------------------------------
// <copyright file="QueryExtensions.cs" company="Eurostat">
//   Date Created : 2013-10-03
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sdmxsource.Extension.Extension
{
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference.Complex;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// Various Structure query related extensions
    /// </summary>
    public static class QueryExtensions
    {
        /// <summary>
        /// Gets the complex query detail from the specified <paramref name="returnStub"/>
        /// </summary>
        /// <param name="returnStub">
        /// The return stub.
        /// </param>
        /// <returns>
        /// The <see cref="ComplexStructureQueryDetailEnumType"/>.
        /// </returns>
        public static ComplexStructureQueryDetailEnumType GetComplexQueryDetail(this bool returnStub)
        {
            return returnStub ? ComplexStructureQueryDetailEnumType.Stub : ComplexStructureQueryDetailEnumType.Full;
        }

        /// <summary>
        /// Gets the complex query detail from the specified <paramref name="returnStub"/>
        /// </summary>
        /// <param name="returnStub">
        /// The return stub.
        /// </param>
        /// <returns>
        /// The <see cref="ComplexStructureQueryDetailEnumType"/>.
        /// </returns>
        public static StructureQueryDetail GetStructureQueryDetail(this bool returnStub)
        {
            return StructureQueryDetail.GetFromEnum(returnStub ? StructureQueryDetailEnumType.AllStubs : StructureQueryDetailEnumType.Full);
        }

        /// <summary>
        /// Gets the return stub value.
        /// </summary>
        /// <param name="detail">
        /// The detail.
        /// </param>
        /// <returns>
        /// The return stub value
        /// </returns>
        public static bool GetReturnStub(this StructureQueryDetail detail)
        {
            return detail == StructureQueryDetailEnumType.AllStubs;
        }

        /// <summary>
        /// Convert to <see cref="IComplexStructureReferenceObject"/>.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <returns>The <see cref="IComplexStructureReferenceObject"/>.</returns>
        public static IComplexStructureReferenceObject ToComplex(this IStructureReference reference)
        {
            var maintainableRef = reference.MaintainableReference;
            IComplexTextReference id = null;
            if (maintainableRef.HasMaintainableId())
            {
                id = new ComplexTextReferenceCore(null, TextSearch.GetFromEnum(TextSearchEnumType.Equal), maintainableRef.MaintainableId);
            }

            IComplexTextReference agency = null;
            if (maintainableRef.HasAgencyId())
            {
                agency = new ComplexTextReferenceCore(null, TextSearch.GetFromEnum(TextSearchEnumType.Equal), maintainableRef.AgencyId);
            }

            IComplexVersionReference version = maintainableRef.HasVersion()
                                                   ? new ComplexVersionReferenceCore(TertiaryBool.ParseBoolean(false), maintainableRef.Version, null, null)
                                                   : new ComplexVersionReferenceCore(TertiaryBool.ParseBoolean(true), null, null, null);

            return new ComplexStructureReferenceCore(agency, id, version, reference.MaintainableStructureEnumType, null, null, null, null);
        }

        /// <summary>
        /// Returns a <see cref="IMaintainableRefObject"/> from the specified <paramref name="complexStructureQuery"/>
        /// </summary>
        /// <param name="complexStructureQuery">
        /// The complex structure query.
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableRefObject"/>.
        /// </returns>
        /// <remarks>This method currently assumes all operators are <c>EQUAL</c></remarks>
        public static IMaintainableRefObject GetMaintainableRefObject(this IComplexStructureReferenceObject complexStructureQuery)
        {
            string version = complexStructureQuery.VersionReference.Version;

            string id = complexStructureQuery.Id != null ? complexStructureQuery.Id.SearchParameter : null;

            string agencyId = complexStructureQuery.AgencyId != null ? complexStructureQuery.AgencyId.SearchParameter : null;

            return new MaintainableRefObjectImpl(agencyId, id, version);
        }

        /// <summary>
        /// Convert to <see cref="ComplexStructureQueryDetail"/>
        /// </summary>
        /// <param name="detail">The detail.</param>
        /// <returns>The <see cref="ComplexStructureQueryDetail"/></returns>
        public static ComplexStructureQueryDetail ToComplex(this StructureQueryDetail detail)
        {
            ComplexStructureQueryDetailEnumType complexStructureQueryDetail;
            switch (detail.EnumType)
            {
                case StructureQueryDetailEnumType.AllStubs:
                    complexStructureQueryDetail = ComplexStructureQueryDetailEnumType.Stub;
                    break;
                default:
                    complexStructureQueryDetail = ComplexStructureQueryDetailEnumType.Full;
                    break;
            }

            return ComplexStructureQueryDetail.GetFromEnum(complexStructureQueryDetail);
        }

        /// <summary>
        /// Convert to <see cref="ComplexMaintainableQueryDetail"/>
        /// </summary>
        /// <param name="detail">
        /// The detail.
        /// </param>
        /// <returns>
        /// The <see cref="ComplexMaintainableQueryDetail"/>.
        /// </returns>
        public static ComplexMaintainableQueryDetail ToComplexReference(this StructureQueryDetail detail)
        {
            ComplexMaintainableQueryDetailEnumType complexStructureQueryDetail;
            switch (detail.EnumType)
            {
                case StructureQueryDetailEnumType.AllStubs:
                    complexStructureQueryDetail = ComplexMaintainableQueryDetailEnumType.Stub;
                    break;
                default:
                    complexStructureQueryDetail = ComplexMaintainableQueryDetailEnumType.Full;
                    break;
            }

            return ComplexMaintainableQueryDetail.GetFromEnum(complexStructureQueryDetail);
        }
    }
}