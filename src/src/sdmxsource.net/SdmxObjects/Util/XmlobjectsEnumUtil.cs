// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlobjectsEnumUtil.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The xml objects enum util.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Util
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;

    /// <summary>
    ///   The xml objects enum util.
    /// </summary>
    public class XmlobjectsEnumUtil
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="XmlobjectsEnumUtil"/> class from being created. 
        /// </summary>
        private XmlobjectsEnumUtil()
        {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////CONVERT FROM V2.1 SCHEMA            ///////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods and Operators

        /// <summary>
        /// The build v 21.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from. 
        /// </param>
        /// <returns>
        /// The <see cref="string"/> . 
        /// </returns>
        /// ///
        /// <exception cref="SdmxNotImplementedException">
        /// Throws Unsupported Exception.
        /// </exception>
        public static string BuildV21(SdmxStructureType buildFrom)
        {
            if (buildFrom == null)
            {
                return ObjectTypeCodelistTypeConstants.Any;
            }

            switch (buildFrom.EnumType)
            {
                case SdmxStructureEnumType.Agency:
                    return ObjectTypeCodelistTypeConstants.Agency;
                case SdmxStructureEnumType.AgencyScheme:
                    return ObjectTypeCodelistTypeConstants.AgencyScheme;
                case SdmxStructureEnumType.AttachmentConstraint:
                    return ObjectTypeCodelistTypeConstants.AttachmentConstraint;
                case SdmxStructureEnumType.DataAttribute:
                    return ObjectTypeCodelistTypeConstants.Attribute;
                case SdmxStructureEnumType.AttributeDescriptor:
                    return ObjectTypeCodelistTypeConstants.AttributeDescriptor;
                case SdmxStructureEnumType.Categorisation:
                    return ObjectTypeCodelistTypeConstants.Categorisation;
                case SdmxStructureEnumType.Category:
                    return ObjectTypeCodelistTypeConstants.Category;
                case SdmxStructureEnumType.CategoryScheme:
                    return ObjectTypeCodelistTypeConstants.CategoryScheme;
                case SdmxStructureEnumType.CategorySchemeMap:
                    return ObjectTypeCodelistTypeConstants.CategorySchemeMap;
                case SdmxStructureEnumType.Code:
                    return ObjectTypeCodelistTypeConstants.Code;
                case SdmxStructureEnumType.CodeMap:
                    return ObjectTypeCodelistTypeConstants.CodeMap;
                case SdmxStructureEnumType.CodeList:
                    return ObjectTypeCodelistTypeConstants.Codelist;
                case SdmxStructureEnumType.CodeListMap:
                    return ObjectTypeCodelistTypeConstants.CodelistMap;
                case SdmxStructureEnumType.ComponentMap:
                    return ObjectTypeCodelistTypeConstants.ComponentMap;
                case SdmxStructureEnumType.Concept:
                    return ObjectTypeCodelistTypeConstants.Concept;
                case SdmxStructureEnumType.ConceptMap:
                    return ObjectTypeCodelistTypeConstants.ConceptMap;
                case SdmxStructureEnumType.ConceptScheme:
                    return ObjectTypeCodelistTypeConstants.ConceptScheme;
                case SdmxStructureEnumType.ConceptSchemeMap:
                    return ObjectTypeCodelistTypeConstants.ConceptSchemeMap;

                // case C: return ObjectTypeCodelistType.CONSTRAINT;  //IMPORTANT CONSTRAINT NOT SUPPORTED
                case SdmxStructureEnumType.ConstraintContentTarget:
                    return ObjectTypeCodelistTypeConstants.ConstraintTarget;
                case SdmxStructureEnumType.ContentConstraint:
                    return ObjectTypeCodelistTypeConstants.ContentConstraint;
                case SdmxStructureEnumType.DataConsumer:
                    return ObjectTypeCodelistTypeConstants.DataConsumer;
                case SdmxStructureEnumType.DataConsumerScheme:
                    return ObjectTypeCodelistTypeConstants.DataConsumerScheme;
                case SdmxStructureEnumType.DataProvider:
                    return ObjectTypeCodelistTypeConstants.DataProvider;
                case SdmxStructureEnumType.DataProviderScheme:
                    return ObjectTypeCodelistTypeConstants.DataProviderScheme;
                case SdmxStructureEnumType.DatasetTarget:
                    return ObjectTypeCodelistTypeConstants.DataSetTarget;
                case SdmxStructureEnumType.Dsd:
                    return ObjectTypeCodelistTypeConstants.DataStructure;
                case SdmxStructureEnumType.Dataflow:
                    return ObjectTypeCodelistTypeConstants.Dataflow;
                case SdmxStructureEnumType.Dimension:
                    return ObjectTypeCodelistTypeConstants.Dimension;
                case SdmxStructureEnumType.DimensionDescriptor:
                    return ObjectTypeCodelistTypeConstants.DimensionDescriptor;
                case SdmxStructureEnumType.DimensionDescriptorValuesTarget:
                    return ObjectTypeCodelistTypeConstants.DimensionDescriptorValuesTarget;
                case SdmxStructureEnumType.Group:
                    return ObjectTypeCodelistTypeConstants.GroupDimensionDescriptor;
                case SdmxStructureEnumType.HierarchicalCode:
                    return ObjectTypeCodelistTypeConstants.HierarchicalCode;
                case SdmxStructureEnumType.HierarchicalCodelist:
                    return ObjectTypeCodelistTypeConstants.HierarchicalCodelist;
                case SdmxStructureEnumType.Hierarchy:
                    return ObjectTypeCodelistTypeConstants.Hierarchy;
                case SdmxStructureEnumType.HybridCode:
                    return ObjectTypeCodelistTypeConstants.HybridCodeMap;
                case SdmxStructureEnumType.HybridCodelistMap:
                    return ObjectTypeCodelistTypeConstants.HybridCodelistMap;
                case SdmxStructureEnumType.Level:
                    return ObjectTypeCodelistTypeConstants.Level;
                case SdmxStructureEnumType.MeasureDescriptor:
                    return ObjectTypeCodelistTypeConstants.MeasureDescriptor;
                case SdmxStructureEnumType.MeasureDimension:
                    return ObjectTypeCodelistTypeConstants.MeasureDimension;
                case SdmxStructureEnumType.MetadataAttribute:
                    return ObjectTypeCodelistTypeConstants.MetadataAttribute;
                case SdmxStructureEnumType.MetadataSet:
                    return ObjectTypeCodelistTypeConstants.MetadataSet;
                case SdmxStructureEnumType.Msd:
                    return ObjectTypeCodelistTypeConstants.MetadataStructure;
                case SdmxStructureEnumType.MetadataTarget:
                    return ObjectTypeCodelistTypeConstants.MetadataTarget;
                case SdmxStructureEnumType.MetadataFlow:
                    return ObjectTypeCodelistTypeConstants.Metadataflow;

                // case ORG: return ObjectTypeCodelistType.ORGANISATION;  //IMPORTANT ORGANISATION NOT SUPPORTED
                // case ORG: return ObjectTypeCodelistType.OrganisationScheme;  //IMPORTANT OrganisationScheme NOT SUPPORTED
                case SdmxStructureEnumType.OrganisationMap:
                    return ObjectTypeCodelistTypeConstants.OrganisationMap;
                case SdmxStructureEnumType.OrganisationSchemeMap:
                    return ObjectTypeCodelistTypeConstants.OrganisationSchemeMap;
                case SdmxStructureEnumType.OrganisationUnit:
                    return ObjectTypeCodelistTypeConstants.OrganisationUnit;
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    return ObjectTypeCodelistTypeConstants.OrganisationUnitScheme;
                case SdmxStructureEnumType.PrimaryMeasure:
                    return ObjectTypeCodelistTypeConstants.PrimaryMeasure;
                case SdmxStructureEnumType.Process:
                    return ObjectTypeCodelistTypeConstants.Process;
                case SdmxStructureEnumType.ProcessStep:
                    return ObjectTypeCodelistTypeConstants.ProcessStep;
                case SdmxStructureEnumType.ProvisionAgreement:
                    return ObjectTypeCodelistTypeConstants.ProvisionAgreement;
                case SdmxStructureEnumType.ReportPeriodTarget:
                    return ObjectTypeCodelistTypeConstants.ReportPeriodTarget;
                case SdmxStructureEnumType.ReportStructure:
                    return ObjectTypeCodelistTypeConstants.ReportStructure;
                case SdmxStructureEnumType.ReportingCategory:
                    return ObjectTypeCodelistTypeConstants.ReportingCategory;
                case SdmxStructureEnumType.CategoryMap:
                    return ObjectTypeCodelistTypeConstants.ReportingCategoryMap;
                case SdmxStructureEnumType.ReportingTaxonomy:
                    return ObjectTypeCodelistTypeConstants.ReportingTaxonomy;
                case SdmxStructureEnumType.ReportingTaxonomyMap:
                    return ObjectTypeCodelistTypeConstants.ReportingTaxonomyMap;
                case SdmxStructureEnumType.StructureMap:
                    return ObjectTypeCodelistTypeConstants.StructureMap;
                case SdmxStructureEnumType.StructureSet:
                    return ObjectTypeCodelistTypeConstants.StructureSet;
                case SdmxStructureEnumType.TimeDimension:
                    return ObjectTypeCodelistTypeConstants.TimeDimension;
                case SdmxStructureEnumType.Transition:
                    return ObjectTypeCodelistTypeConstants.Transition;
            }

            throw new SdmxNotImplementedException("Object Type : " + buildFrom);
        }

        /// <summary>
        /// The get sdmx object id type.
        /// </summary>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        /// <returns>
        /// The <see cref="string"/> . 
        /// </returns>
        /// ///
        /// <exception cref="SdmxNotImplementedException">
        /// Throws Unsupported Exception.
        /// </exception>
        public static string GetSdmxObjectIdType(SdmxStructureType structureType)
        {
            switch (structureType.EnumType)
            {
                case SdmxStructureEnumType.Agency:
                    return ObjectIDTypeConstants.Agency;
                case SdmxStructureEnumType.AttachmentConstraint:
                    return ObjectIDTypeConstants.AttachmentConstraint;
                case SdmxStructureEnumType.DataAttribute:
                    return ObjectIDTypeConstants.Attribute;
                case SdmxStructureEnumType.AttributeDescriptor:
                    return ObjectIDTypeConstants.AttributeDescriptor;
                case SdmxStructureEnumType.Category:
                    return ObjectIDTypeConstants.Category;
                case SdmxStructureEnumType.CategoryMap:
                    return ObjectIDTypeConstants.CategoryMap;
                case SdmxStructureEnumType.CategoryScheme:
                    return ObjectIDTypeConstants.CategoryScheme;
                case SdmxStructureEnumType.CategorySchemeMap:
                    return ObjectIDTypeConstants.CategorySchemeMap;
                case SdmxStructureEnumType.Code:
                    return ObjectIDTypeConstants.Code;
                case SdmxStructureEnumType.CodeMap:
                    return ObjectIDTypeConstants.CodeMap;
                case SdmxStructureEnumType.CodeList:
                    return ObjectIDTypeConstants.Codelist;
                case SdmxStructureEnumType.CodeListMap:
                    return ObjectIDTypeConstants.CodelistMap;
                case SdmxStructureEnumType.Component:
                    return ObjectIDTypeConstants.Component;
                case SdmxStructureEnumType.ComponentMap:
                    return ObjectIDTypeConstants.ComponentMap;
                case SdmxStructureEnumType.Concept:
                    return ObjectIDTypeConstants.Concept;
                case SdmxStructureEnumType.ConceptMap:
                    return ObjectIDTypeConstants.ConceptMap;
                case SdmxStructureEnumType.ConceptScheme:
                    return ObjectIDTypeConstants.ConceptScheme;
                case SdmxStructureEnumType.ConceptSchemeMap:
                    return ObjectIDTypeConstants.ConceptSchemeMap;
                case SdmxStructureEnumType.ContentConstraint:
                    return ObjectIDTypeConstants.ContentConstraint;
                case SdmxStructureEnumType.Dataflow:
                    return ObjectIDTypeConstants.DataFlow;
                case SdmxStructureEnumType.DataProvider:
                    return ObjectIDTypeConstants.DataProvider;
                case SdmxStructureEnumType.Dataset:
                    return ObjectIDTypeConstants.DataSet;
                case SdmxStructureEnumType.Dimension:
                    return ObjectIDTypeConstants.Dimension;
                case SdmxStructureEnumType.Group:
                    return ObjectIDTypeConstants.GroupKeyDescriptor;
                case SdmxStructureEnumType.HierarchicalCodelist:
                    return ObjectIDTypeConstants.HierarchicalCodelist;
                case SdmxStructureEnumType.Hierarchy:
                    return ObjectIDTypeConstants.Hierarchy;
                case SdmxStructureEnumType.DimensionDescriptor:
                    return ObjectIDTypeConstants.KeyDescriptor;
                case SdmxStructureEnumType.Dsd:
                    return ObjectIDTypeConstants.KeyFamily;
                case SdmxStructureEnumType.MeasureDimension:
                    return ObjectIDTypeConstants.Measure;
                case SdmxStructureEnumType.MeasureDescriptor:
                    return ObjectIDTypeConstants.MeasureDescriptor;
                case SdmxStructureEnumType.MetadataAttribute:
                    return ObjectIDTypeConstants.MetadataAttribute;
                case SdmxStructureEnumType.MetadataFlow:
                    return ObjectIDTypeConstants.MetadataFlow;
                case SdmxStructureEnumType.MetadataSet:
                    return ObjectIDTypeConstants.MetadataSet;
                case SdmxStructureEnumType.Msd:
                    return ObjectIDTypeConstants.MetadataStructure;
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    return ObjectIDTypeConstants.OrganisationScheme;
                case SdmxStructureEnumType.OrganisationSchemeMap:
                    return ObjectIDTypeConstants.OrganisationSchemeMap;
                case SdmxStructureEnumType.Process:
                    return ObjectIDTypeConstants.Process;
                case SdmxStructureEnumType.ProcessStep:
                    return ObjectIDTypeConstants.ProcessStep;
                case SdmxStructureEnumType.ProvisionAgreement:
                    return ObjectIDTypeConstants.ProvisionAgreement;
                case SdmxStructureEnumType.ReportingTaxonomy:
                    return ObjectIDTypeConstants.ReportingTaxonomy;
                case SdmxStructureEnumType.StructureMap:
                    return ObjectIDTypeConstants.StructureMap;
                case SdmxStructureEnumType.StructureSet:
                    return ObjectIDTypeConstants.StructureSet;
                case SdmxStructureEnumType.MetadataTarget:
                    return ObjectIDTypeConstants.FullTargetIdentifier; // It could be PartialTargetIdentifier.
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, structureType.EnumType);
            }
        }

        /// <summary>
        /// The get sdmx representation type v 20.
        /// </summary>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        /// <returns>
        /// The <see cref="string"/> . 
        /// </returns>
        /// ///
        /// <exception cref="SdmxNotImplementedException">
        /// Throws Unsupported Exception.
        /// </exception>
        public static string GetSdmxRepresentationTypeV20(SdmxStructureType structureType)
        {
            switch (structureType.EnumType)
            {
                case SdmxStructureEnumType.CategoryScheme:
                    return RepresentationTypeTypeConstants.CategoryScheme;
                case SdmxStructureEnumType.CodeList:
                    return RepresentationTypeTypeConstants.Codelist;
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    return RepresentationTypeTypeConstants.OrganisationScheme;
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, structureType);
            }
        }

        /// <summary>
        /// The get sdmx structure type.
        /// </summary>
        /// <param name="xref">
        /// The xref. 
        /// </param>
        /// <returns>
        /// The <see cref="SdmxStructureType"/> . 
        /// </returns>
        public static SdmxStructureType GetSdmxStructureType(MaintainableReferenceBaseType xref)
        {
            return GetSdmxStructureType(xref.GetTypedRef<MaintainableRefBaseType>().@class);
        }

        /// <summary>
        /// The get sdmx structure type.
        /// </summary>
        /// <param name="enumType">
        /// The enum type. 
        /// </param>
        /// <returns>
        /// The <see cref="SdmxStructureType"/> . 
        /// </returns>
        /// ///
        /// <exception cref="SdmxNotImplementedException">
        /// Throws Unsupported Exception.
        /// </exception>
        public static SdmxStructureType GetSdmxStructureType(string enumType)
        {
            switch (enumType)
            {
                case ObjectIDTypeConstants.Agency:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Agency);
                case ObjectIDTypeConstants.AttachmentConstraint:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AttachmentConstraint);
                case ObjectIDTypeConstants.Attribute:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataAttribute);
                case ObjectIDTypeConstants.AttributeDescriptor:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AttributeDescriptor);
                case ObjectIDTypeConstants.Category:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Category);
                case ObjectIDTypeConstants.CategoryMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryMap);
                case ObjectIDTypeConstants.CategoryScheme:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme);
                case ObjectIDTypeConstants.CategorySchemeMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategorySchemeMap);
                case ObjectIDTypeConstants.Code:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Code);
                case ObjectIDTypeConstants.CodeMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeMap);
                case ObjectIDTypeConstants.Codelist:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList);
                case ObjectIDTypeConstants.CodelistMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeListMap);
                case ObjectIDTypeConstants.Component:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Component);
                case ObjectIDTypeConstants.ComponentMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ComponentMap);
                case ObjectIDTypeConstants.Concept:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Concept);
                case ObjectIDTypeConstants.ConceptMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptMap);
                case ObjectIDTypeConstants.ConceptScheme:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme);
                case ObjectIDTypeConstants.ConceptSchemeMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptSchemeMap);
                case ObjectIDTypeConstants.ContentConstraint:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ContentConstraint);
                case ObjectIDTypeConstants.DataFlow:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow);
                case ObjectIDTypeConstants.DataProvider:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProvider);
                case ObjectIDTypeConstants.DataSet:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataset);
                case ObjectIDTypeConstants.TimeDimension:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TimeDimension);
                case ObjectIDTypeConstants.Dimension:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dimension);
                case ObjectIDTypeConstants.GroupKeyDescriptor:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Group);
                case ObjectIDTypeConstants.HierarchicalCodelist:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist);
                case ObjectIDTypeConstants.Hierarchy:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Hierarchy);
                case ObjectIDTypeConstants.KeyDescriptor:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DimensionDescriptor);
                case ObjectIDTypeConstants.KeyFamily:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd);
                case ObjectIDTypeConstants.Measure:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MeasureDimension);
                case ObjectIDTypeConstants.MeasureDescriptor:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MeasureDescriptor);
                case ObjectIDTypeConstants.MetadataAttribute:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataAttribute);
                case ObjectIDTypeConstants.MetadataFlow:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow);
                case ObjectIDTypeConstants.MetadataSet:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataSet);
                case ObjectIDTypeConstants.MetadataStructure:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd);
                case ObjectIDTypeConstants.OrganisationScheme:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnitScheme);
                case ObjectIDTypeConstants.OrganisationSchemeMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationSchemeMap);
                case ObjectIDTypeConstants.Process:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Process);
                case ObjectIDTypeConstants.ProcessStep:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProcessStep);
                case ObjectIDTypeConstants.ProvisionAgreement:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProvisionAgreement);
                case ObjectIDTypeConstants.ReportingTaxonomy:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingTaxonomy);
                case ObjectIDTypeConstants.StructureMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureMap);
                case ObjectIDTypeConstants.StructureSet:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureSet);
                case ObjectIDTypeConstants.FullTargetIdentifier:
                case ObjectIDTypeConstants.PartialTargetIdentifier:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataTarget);
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, enumType);
            }
        }


        /// <summary>
        /// Gets the type of the SDMX representation.
        /// </summary>
        /// <param name="enumType">Type of the ENUM.</param>
        /// <returns></returns>
        /// <exception cref="SdmxNotImplementedException"><paramref name="enumType"/> not supported.</exception>
        public static SdmxStructureType GetSdmxRepresentationType(string enumType)
        {
            switch (enumType)
            {
                case RepresentationTypeTypeConstants.CategoryScheme:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme);
                case RepresentationTypeTypeConstants.Codelist:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList);
                case RepresentationTypeTypeConstants.OrganisationScheme:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnitScheme);
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, enumType);
            }
        }


        /// <summary>
        /// Gets the SDMX structure type from representation scheme type V20.
        /// </summary>
        /// <param name="enumType">Type of the ENUM.</param>
        /// <returns>The <see cref="SdmxStructureType" /></returns>
        /// <exception cref="SdmxNotImplementedException"><paramref name="enumType" /> not supported.</exception>
        public static SdmxStructureType GetSdmxStructureTypeFromRepresentationSchemeTypeV20(string enumType)
        {
            switch (enumType)
            {
                case RepresentationSchemeTypeTypeConstants.Category:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme);
                case RepresentationSchemeTypeTypeConstants.Codelist:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList);
                case RepresentationSchemeTypeTypeConstants.Organisation:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnitScheme);
                case RepresentationSchemeTypeTypeConstants.Concept:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme);
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, enumType);
            }
        }

        /// <summary>
        /// The get sdmx structure type v 21.
        /// </summary>
        /// <param name="enumType">
        /// The enum type. 
        /// </param>
        /// <returns>
        /// The <see cref="SdmxStructureType"/> . 
        /// </returns>
        /// ///
        /// <exception cref="SdmxNotImplementedException">
        /// Throws Unsupported Exception.
        /// </exception>
        public static SdmxStructureType GetSdmxStructureTypeV21(string enumType)
        {
            switch (enumType)
            {
                case ObjectTypeCodelistTypeConstants.Agency:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Agency);
                case ObjectTypeCodelistTypeConstants.AgencyScheme:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AgencyScheme);
                case ObjectTypeCodelistTypeConstants.Any:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Any);
                case ObjectTypeCodelistTypeConstants.AttachmentConstraint:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AttachmentConstraint);
                case ObjectTypeCodelistTypeConstants.Attribute:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataAttribute);
                case ObjectTypeCodelistTypeConstants.AttributeDescriptor:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AttributeDescriptor);
                case ObjectTypeCodelistTypeConstants.Categorisation:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Categorisation);
                case ObjectTypeCodelistTypeConstants.Category:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Category);
                case ObjectTypeCodelistTypeConstants.CategoryScheme:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme);
                case ObjectTypeCodelistTypeConstants.CategorySchemeMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategorySchemeMap);
                case ObjectTypeCodelistTypeConstants.Code:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Code);
                case ObjectTypeCodelistTypeConstants.CodeMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeMap);
                case ObjectTypeCodelistTypeConstants.Codelist:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList);
                case ObjectTypeCodelistTypeConstants.CodelistMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeListMap);
                case ObjectTypeCodelistTypeConstants.ComponentMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ComponentMap);
                case ObjectTypeCodelistTypeConstants.Concept:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Concept);
                case ObjectTypeCodelistTypeConstants.ConceptMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptMap);
                case ObjectTypeCodelistTypeConstants.ConceptScheme:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme);
                case ObjectTypeCodelistTypeConstants.ConceptSchemeMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptSchemeMap);
                case ObjectTypeCodelistTypeConstants.ConstraintTarget:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConstraintContentTarget);
                case ObjectTypeCodelistTypeConstants.ContentConstraint:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ContentConstraint);
                case ObjectTypeCodelistTypeConstants.DataConsumer:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumer);
                case ObjectTypeCodelistTypeConstants.DataConsumerScheme:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumerScheme);
                case ObjectTypeCodelistTypeConstants.DataProvider:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProvider);
                case ObjectTypeCodelistTypeConstants.DataProviderScheme:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProviderScheme);
                case ObjectTypeCodelistTypeConstants.DataSetTarget:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DatasetTarget);
                case ObjectTypeCodelistTypeConstants.DataStructure:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd);
                case ObjectTypeCodelistTypeConstants.Dataflow:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow);
                case ObjectTypeCodelistTypeConstants.Dimension:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dimension);
                case ObjectTypeCodelistTypeConstants.DimensionDescriptor:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DimensionDescriptor);
                case ObjectTypeCodelistTypeConstants.DimensionDescriptorValuesTarget:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DimensionDescriptorValuesTarget);
                case ObjectTypeCodelistTypeConstants.GroupDimensionDescriptor:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Group);
                case ObjectTypeCodelistTypeConstants.HierarchicalCode:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCode);
                case ObjectTypeCodelistTypeConstants.HierarchicalCodelist:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist);
                case ObjectTypeCodelistTypeConstants.Hierarchy:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Hierarchy);
                case ObjectTypeCodelistTypeConstants.HybridCodeMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HybridCode);
                case ObjectTypeCodelistTypeConstants.HybridCodelistMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HybridCodelistMap);
                case ObjectTypeCodelistTypeConstants.IdentifiableObjectTarget:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.IdentifiableObjectTarget);
                case ObjectTypeCodelistTypeConstants.Level:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Level);
                case ObjectTypeCodelistTypeConstants.MeasureDescriptor:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MeasureDescriptor);
                case ObjectTypeCodelistTypeConstants.MeasureDimension:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MeasureDimension);
                case ObjectTypeCodelistTypeConstants.MetadataAttribute:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataAttribute);
                case ObjectTypeCodelistTypeConstants.MetadataSet:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataSet);
                case ObjectTypeCodelistTypeConstants.MetadataStructure:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd);
                case ObjectTypeCodelistTypeConstants.MetadataTarget:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataTarget);
                case ObjectTypeCodelistTypeConstants.Metadataflow:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow);

                // case ObjectTypeCodelistType.IntOrganisation : return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationSchemeMap); 
                case ObjectTypeCodelistTypeConstants.OrganisationMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationMap);

                // case ObjectTypeCodelistType.IntOrganisationScheme : return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OR); 
                case ObjectTypeCodelistTypeConstants.OrganisationSchemeMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationSchemeMap);
                case ObjectTypeCodelistTypeConstants.OrganisationUnit:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnit);
                case ObjectTypeCodelistTypeConstants.OrganisationUnitScheme:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnitScheme);
                case ObjectTypeCodelistTypeConstants.PrimaryMeasure:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.PrimaryMeasure);
                case ObjectTypeCodelistTypeConstants.Process:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Process);
                case ObjectTypeCodelistTypeConstants.ProcessStep:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProcessStep);
                case ObjectTypeCodelistTypeConstants.ProvisionAgreement:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProvisionAgreement);
                case ObjectTypeCodelistTypeConstants.ReportPeriodTarget:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportPeriodTarget);
                case ObjectTypeCodelistTypeConstants.ReportStructure:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportStructure);
                case ObjectTypeCodelistTypeConstants.ReportingCategory:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingCategory);
                case ObjectTypeCodelistTypeConstants.ReportingCategoryMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryMap);
                case ObjectTypeCodelistTypeConstants.ReportingTaxonomy:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingTaxonomy);
                case ObjectTypeCodelistTypeConstants.ReportingTaxonomyMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingTaxonomyMap);

                // case ObjectTypeCodelistType.IntReportingYearStartDay : return SdmxStructureType.R; 
                case ObjectTypeCodelistTypeConstants.StructureMap:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureMap);
                case ObjectTypeCodelistTypeConstants.StructureSet:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureSet);
                case ObjectTypeCodelistTypeConstants.TimeDimension:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TimeDimension);
                case ObjectTypeCodelistTypeConstants.Transition:
                    return SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Transition);
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, enumType);
            }
        }

        #endregion

        /// <summary>
        /// Gets the type of the SDMX representation scheme.
        /// </summary>
        /// <param name="structureType">Type of the structure.</param>
        /// <returns>The SDMX v2.0 representation scheme type.</returns>
        public static string GetSdmxRepresentationSchemeType(SdmxStructureType structureType)
        {
            switch (structureType.EnumType)
            {
                case SdmxStructureEnumType.CategoryScheme:
                    return RepresentationSchemeTypeTypeConstants.Category;
                    case SdmxStructureEnumType.CodeList:
                    return RepresentationSchemeTypeTypeConstants.Codelist;
                    case SdmxStructureEnumType.ConceptScheme:
                    return RepresentationSchemeTypeTypeConstants.Concept;
                    case SdmxStructureEnumType.OrganisationUnitScheme:
                    case SdmxStructureEnumType.DataProviderScheme:
                    case SdmxStructureEnumType.DataConsumerScheme:
                    case SdmxStructureEnumType.AgencyScheme:
                    return RepresentationSchemeTypeTypeConstants.Organisation;
                    default: throw new SdmxNotImplementedException(ExceptionCode.Unsupported, structureType);
            }   
        }
    }
}