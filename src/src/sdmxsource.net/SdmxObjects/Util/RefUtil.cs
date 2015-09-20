// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RefUtil.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Utility static methods for building SDMXObjects from v2.1+ Schemas, where common features are reused.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.SdmxObjects.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using V20 = Org.Sdmx.Resources.SdmxMl.Schemas.V20;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    using AttachmentConstraintRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.AttachmentConstraintRefType;
    using CategoryRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.CategoryRefType;
    using CategorySchemeRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.CategorySchemeRefType;
    using CodeRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.CodeRefType;
    using CodelistRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.CodelistRefType;
    using ConceptSchemeRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.ConceptSchemeRefType;
    using DataProviderRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.DataProviderRefType;
    using DataflowRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.DataflowRefType;
    using HierarchicalCodelistRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.HierarchicalCodelistRefType;
    using MetadataStructureRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.MetadataStructureRefType;
    using MetadataflowRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.MetadataflowRefType;
    using OrganisationSchemeRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.OrganisationSchemeRefType;
    using ProvisionAgreementRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.ProvisionAgreementRefType;

    /// <summary>
    ///     Utility static methods for building SDMXObjects from v2.1+ Schemas, where common features are reused.
    /// </summary>
    public static class RefUtil
    {

        #region MappingDict

        private static readonly Dictionary<Type, Type> _refTypeMap = new Dictionary<Type, Type>
                                                                {
                                                                    {
                                                                        typeof(ObjectReferenceType)
                                                                        , typeof(ObjectRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        MaintainableReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        MaintainableRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        StructureReferenceType), 
                                                                        typeof(StructureRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        StructureUsageReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        StructureUsageRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        ItemSchemeReferenceType), 
                                                                        typeof(ItemSchemeRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalComponentListComponentReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalComponentListComponentRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalComponentReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalComponentRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        StructureOrUsageReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        StructureOrUsageRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        CategorisationReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        CategorisationRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        CategorySchemeReferenceType
                                                                        ), 
                                                                        typeof(CategorySchemeRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        CategoryReferenceType), 
                                                                        typeof(CategoryRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalCategoryReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalCategoryRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        CodelistReferenceType), 
                                                                        typeof(CodelistRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(CodeReferenceType), 
                                                                        typeof(CodeRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalCodeReferenceType), 
                                                                        typeof(LocalCodeRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        AnyCodelistReferenceType)
                                                                        , 
                                                                        typeof(AnyCodelistRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        AnyLocalCodeReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        AnyLocalCodeRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        ConceptSchemeReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        ConceptSchemeRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        ConceptReferenceType), 
                                                                        typeof(ConceptRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalConceptReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalConceptRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        OrganisationSchemeReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        OrganisationSchemeRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        OrganisationReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        OrganisationRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalOrganisationReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalOrganisationRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        OrganisationUnitSchemeReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        OrganisationUnitSchemeRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        OrganisationUnitReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        OrganisationUnitRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalOrganisationUnitReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalOrganisationUnitRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        AgencySchemeReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        AgencySchemeRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(AgencyReferenceType)
                                                                        , typeof(AgencyRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalAgencyReferenceType)
                                                                        , 
                                                                        typeof(LocalAgencyRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        DataConsumerSchemeReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        DataConsumerSchemeRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        DataConsumerReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        DataConsumerRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalDataConsumerReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalDataConsumerRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        DataProviderSchemeReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        DataProviderSchemeRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        DataProviderReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        DataProviderRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalDataProviderReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalDataProviderRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        ReportingTaxonomyReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        ReportingTaxonomyRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        ReportingCategoryReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        ReportCategoryRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalReportingCategoryReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalReportingCategoryRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        HierarchicalCodelistReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        HierarchicalCodelistRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        HierarchyReferenceType), 
                                                                        typeof(HierarchyRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(LevelReferenceType), 
                                                                        typeof(LevelRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalLevelReferenceType), 
                                                                        typeof(LocalLevelRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        HierarchicalCodeReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        HierarchicalCodeRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        ConstraintReferenceType), 
                                                                        typeof(ConstraintRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        AttachmentConstraintReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        AttachmentConstraintRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        ContentConstraintReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        ContentConstraintRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        DataflowReferenceType), 
                                                                        typeof(DataflowRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        MetadataflowReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        MetadataflowRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        DataStructureReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        DataStructureRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        KeyDescriptorReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        KeyDescriptorRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        AttributeDescriptorReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        AttributeDescriptorRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        MeasureDescriptorReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        MeasureDescriptorRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        GroupKeyDescriptorReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        GroupKeyDescriptorRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalGroupKeyDescriptorReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalGroupKeyDescriptorRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        DimensionReferenceType), 
                                                                        typeof(DimensionRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        MeasureDimensionReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        MeasureDimensionRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        TimeDimensionReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        TimeDimensionRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalDimensionReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalDimensionRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        AttributeReferenceType), 
                                                                        typeof(AttributeRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        PrimaryMeasureReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        PrimaryMeasureRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalPrimaryMeasureReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalPrimaryMeasureRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalDataStructureComponentReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalDataStructureComponentRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        DataStructureEnumerationSchemeReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        DataStructureEnumerationSchemeRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        MetadataStructureReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        MetadataStructureRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        MetadataTargetReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        MetadataTargetRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalMetadataTargetReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalMetadataTargetRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        ConstraintTargetReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        ConstraintTargetRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        DataSetTargetReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        DataSetTargetRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        KeyDescriptorValuesTargetReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        KeyDescriptorValuesTargetRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        ReportPeriodTargetReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        ReportPeriodTargetRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        IdentifiableObjectTargetReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        IdentifiableObjectTargetRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalTargetObjectReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalTargetObjectRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        ReportStructureReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        ReportStructureRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalReportStructureReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalReportStructureRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        MetadataAttributeReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        MetadataAttributeRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalMetadataStructureComponentReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalMetadataStructureComponentRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        ProvisionAgreementReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        ProvisionAgreementRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        ProcessReferenceType), 
                                                                        typeof(ProcessRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        ProcessStepReferenceType)
                                                                        , 
                                                                        typeof(ProcessStepRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalProcessStepReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalProcessStepRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        TransitionReferenceType), 
                                                                        typeof(TransitionRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        StructureSetReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        StructureSetRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        StructureMapReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        StructureMapRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        CategorySchemeMapReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        CategorySchemeMapRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        CodelistMapReferenceType)
                                                                        , 
                                                                        typeof(CodelistMapRefType
                                                                        )
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        LocalCodelistMapReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        LocalCodelistMapRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        ConceptSchemeMapReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        ConceptSchemeMapRefType)
                                                                    }, 
                                                                    {
                                                                        typeof(
                                                                        OrganisationSchemeMapReferenceType
                                                                        ), 
                                                                        typeof(
                                                                        OrganisationSchemeMapRefType
                                                                        )
                                                                    }
                                                                    
                                                                };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The create category ref.
        /// </summary>
        /// <param name="xref">
        /// The structureReference.
        /// </param>
        /// <returns>
        /// The <see cref="IStructureReference"/> .
        /// </returns>
        public static IStructureReference CreateCategoryRef(V20.structure.CategoryRefType xref)
        {
            if (xref.URN != null)
            {
                return new StructureReferenceImpl(xref.URN);
            }

            IList<string> catId = new List<string>();
            string catSchemeAgencyId = xref.CategorySchemeAgencyID;
            string catSchemeId = xref.CategorySchemeID;
            string catSchemeVersion = xref.CategorySchemeVersion;

            if (xref.CategoryID != null)
            {
                GetCateogryIds(catId, xref.CategoryID);
            }

            string[] catIds = catId.ToArray();

            return new StructureReferenceImpl(
                catSchemeAgencyId, catSchemeId, catSchemeVersion, SdmxStructureEnumType.Category, catIds);
        }

        /// <summary>
        /// The create category ref.
        /// </summary>
        /// <param name="referencedFrom">
        /// The referenced from.
        /// </param>
        /// <param name="xref">
        /// The structureReference.
        /// </param>
        /// <returns>
        /// The <see cref="ICrossReference"/> .
        /// </returns>
        public static ICrossReference CreateCategoryRef(ISdmxObject referencedFrom, V20.structure.CategoryRefType xref)
        {
            if (xref.URN != null)
            {
                return new CrossReferenceImpl(referencedFrom, xref.URN);
            }

            IList<string> catId = new List<string>();
            string catSchemeAgencyId = xref.CategorySchemeAgencyID;
            string catSchemeId = xref.CategorySchemeID;
            string catSchemeVersion = xref.CategorySchemeVersion;

            if (xref.CategoryID != null)
            {
                GetCateogryIds(catId, xref.CategoryID);
            }

            string[] catIds = catId.ToArray();

            return new CrossReferenceImpl(
                referencedFrom, catSchemeAgencyId, catSchemeId, catSchemeVersion, SdmxStructureEnumType.Category, catIds);
        }

        /// <summary>
        /// Create local reference id from <paramref name="localReference"/>.
        /// </summary>
        /// <param name="localReference">
        /// The local reference.
        /// </param>
        /// <typeparam name="T">
        /// The <paramref name="localReference"/> type a <see cref="ReferenceType"/> subtype
        /// </typeparam>
        /// <returns>
        /// The local reference id from <paramref name="localReference"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="localReference"/> is null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="localReference"/> <see cref="ReferenceType.Ref"/> is null.
        /// </exception>
        public static string CreateLocalIdReference<T>(T localReference) where T : ReferenceType
        {
            if (localReference == null)
            {
                throw new ArgumentNullException("localReference");
            }

            var xref = localReference.GetTypedRef<RefBaseType>();

            if (xref != null)
            {
                return xref.id;
            }

            throw new ArgumentException("localReference.Ref is null");
        }

        /// <summary>
        /// The create local id reference.
        /// </summary>
        /// <param name="localReference">
        /// The local reference.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        public static string CreateLocalIdReference(LocalMetadataTargetReferenceType localReference)
        {
            if (localReference == null)
            {
                throw new ArgumentNullException("localReference");
            }

            var xref = localReference.GetTypedRef<LocalMetadataTargetRefType>();

            if (xref != null)
            {
                return xref.id;
            }

            throw new ArgumentException("localReference.Ref is null");
        }

        /// <summary>
        /// The create local id reference.
        /// </summary>
        /// <param name="localReference">
        /// The local reference.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        public static string CreateLocalIdReference(LocalLevelReferenceType localReference)
        {
            if (localReference == null)
            {
                throw new ArgumentNullException("localReference");
            }

            var xref = localReference.GetTypedRef<LocalLevelRefType>();

            if (xref != null)
            {
                return xref.id;
            }

            throw new ArgumentException("localReference.Ref is null");
        }

        /// <summary>
        /// The create local id reference.
        /// </summary>
        /// <param name="localReference">
        /// The local reference.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        public static string CreateLocalIdReference(LocalProcessStepReferenceType localReference)
        {
            if (localReference == null)
            {
                throw new ArgumentNullException("localReference");
            }

            var xref = localReference.GetTypedRef<LocalProcessStepRefType>();

            if (xref != null)
            {
                return xref.id;
            }

            throw new ArgumentException("localReference.Ref is null");
        }

        /// <summary>
        /// The create local id reference.
        /// </summary>
        /// <param name="localReference">
        /// The local reference.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        public static string CreateLocalIdReference(LocalGroupKeyDescriptorReferenceType localReference)
        {
            if (localReference == null)
            {
                throw new ArgumentNullException("localReference");
            }

            var xref = localReference.GetTypedRef<LocalGroupKeyDescriptorRefType>();

            if (xref != null)
            {
                return xref.id;
            }

            throw new ArgumentException("localReference.Ref is null");
        }

        /// <summary>
        /// The create local id reference.
        /// </summary>
        /// <param name="localReference">
        /// The local reference.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        public static string CreateLocalIdReference(LocalDimensionReferenceType localReference)
        {
            if (localReference == null)
            {
                throw new ArgumentNullException("localReference");
            }

            var xref = localReference.GetTypedRef<LocalDimensionRefType>();

            if (xref != null)
            {
                return xref.id;
            }

            throw new ArgumentException("localReference.Ref is null");
        }

        /// <summary>
        /// The create local id reference.
        /// </summary>
        /// <param name="localReference">
        /// The local reference.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        public static string CreateLocalIdReference(LocalPrimaryMeasureReferenceType localReference)
        {
            if (localReference == null)
            {
                throw new ArgumentNullException("localReference");
            }

            var xref = localReference.GetTypedRef<LocalPrimaryMeasureRefType>();

            if (xref != null)
            {
                return xref.id;
            }

            throw new ArgumentException("localReference.Ref is null");
        }

        /// <summary>
        /// The create reference.
        /// </summary>
        /// <param name="objectReference">
        /// The object reference.
        /// </param>
        /// <returns>
        /// The <see cref="IStructureReference"/> .
        /// </returns>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public static IStructureReference CreateReference(ReferenceType objectReference)
        {
            if (objectReference.URN != null && objectReference.URN.Count > 0)
            {
                return new StructureReferenceImpl(objectReference.URN[0]);
            }
            RefBaseType xref = null;
            Type refType;

            MethodInfo methodInfo = objectReference.GetType().GetMethod("GetTypedRef");

            if (_refTypeMap.TryGetValue(objectReference.GetType(), out refType))
            {
                MethodInfo genericMethod = methodInfo.MakeGenericMethod(refType);
                xref = (RefBaseType)genericMethod.Invoke(objectReference, null);
            }

            if (xref == null)
            {
                throw new SdmxSemmanticException("Illegal Reference : ObjectReference does not contain URN or Ref");
            }

            SdmxStructureType referencedStructure = SdmxStructureType.ParseClass(xref.@class);
            if (!referencedStructure.UrnPackage.Equals(xref.package))
            {
                throw new SdmxSemmanticException(referencedStructure.StructureType + " is not in package " + xref.package);
            }

            RefBaseType refBase = xref;
            bool hasContainer = !string.IsNullOrWhiteSpace(refBase.containerID);

            string version;
            string maintainableId;
            string[] identifiableId = null;

            bool hasIdentifiable = false;
            if (refBase.maintainableParentID == null)
            {
                maintainableId = refBase.id;
                version = refBase.version;
            }
            else
            {
                maintainableId = refBase.maintainableParentID;
                hasIdentifiable = !string.IsNullOrWhiteSpace(refBase.id);
                version = refBase.maintainableParentVersion;
            }

            if (hasIdentifiable)
            {
                if (hasContainer)
                {
                    string containerId = refBase.containerID;
                    string[] id = Regex.Split(refBase.id, "\\.");

                    identifiableId = new string[id.Length + 1];
                    identifiableId[0] = containerId;
                    for (int i = 0; i < id.Length; i++)
                    {
                        identifiableId[i + 1] = id[i];
                    }
                }
                else
                {
                    identifiableId = Regex.Split(refBase.id, "\\.");
                }
            }

            SdmxStructureType referencedStructure2 = SdmxStructureType.ParsePackageAndClass(
                refBase.package, refBase.@class);

            // TODO check the array works as var args, not a single arg
            IStructureReference structureReference = new StructureReferenceImpl(
                refBase.agencyID, maintainableId, version, referencedStructure2, identifiableId);
            return structureReference;
        }

        /// <summary>
        /// The create reference.
        /// </summary>
        /// <param name="referencedFrom">
        /// The referenced from.
        /// </param>
        /// <param name="objectReference">
        /// The object reference.
        /// </param>
        /// <returns>
        /// The <see cref="ICrossReference"/> .
        /// </returns>
        public static ICrossReference CreateReference(ISdmxObject referencedFrom, ReferenceType objectReference)
        {
            return new CrossReferenceImpl(referencedFrom, CreateReference(objectReference));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the cateogry ids.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <param name="idType">
        /// The id type.
        /// </param>
        private static void GetCateogryIds(ICollection<string> list, V20.structure.CategoryIDType idType)
        {
            V20.structure.CategoryIDType currentCategoryId = idType;

            while (currentCategoryId != null)
            {
                list.Add(currentCategoryId.ID);
                currentCategoryId = currentCategoryId.CategoryID;
            }
        }

        #endregion



    }
}