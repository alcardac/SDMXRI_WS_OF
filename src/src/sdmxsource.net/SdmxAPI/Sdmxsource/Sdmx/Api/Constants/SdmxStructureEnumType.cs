// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxStructureEnumType.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Constants
{
    #region Using directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    #endregion

    /// <summary>
    ///     The sdmx structure enumeration type.
    /// </summary>
    public enum SdmxStructureEnumType
    {
        /// <summary>
        ///     The null.
        /// </summary>
        Null, 

        // NON-IDENTIFIABLES

        /// <summary>
        ///     The any.
        /// </summary>
        Any, 

        /// <summary>
        ///     The annotation.
        /// </summary>
        Annotation, 

        /// <summary>
        ///     The category id.
        /// </summary>
        CategoryId, 

        /// <summary>
        ///     The data source.
        /// </summary>
        Datasource, 

        /// <summary>
        ///     The text format.
        /// </summary>
        TextFormat, 

        /// <summary>
        ///     The text type.
        /// </summary>
        TextType, 

        /// <summary>
        ///     The related structures.
        /// </summary>
        RelatedStructures, 

        /// <summary>
        ///     The code list ref.
        /// </summary>
        CodeListRef, 

        /// <summary>
        /// Contact
        /// </summary>
        Contact,

        /// <summary>
        ///     The component.
        /// </summary>
        Component, 

        /// <summary>
        ///     The dataset.
        /// </summary>
        Dataset, 

        /// <summary>
        /// Data reference
        /// </summary>
        DatasetReference,

        /// <summary>
        ///     The item map.
        /// </summary>
        ItemMap, 

        /// <summary>
        ///     The local representation.
        /// </summary>
        LocalRepresentation, 

        /// <summary>
        ///     The computation.
        /// </summary>
        Computation, 

        /// <summary>
        ///     The input output.
        /// </summary>
        InputOutput, 

        /// <summary>
        ///     The constrained data key.
        /// </summary>
        ConstrainedDataKey, 

        /// <summary>
        ///     The constrained data key set.
        /// </summary>
        ConstrainedDataKeyset, 

        /// <summary>
        ///     The attachment constraint attachment.
        /// </summary>
        AttachmentConstraintAttachment, 

        /// <summary>
        ///     The content constraint attachment.
        /// </summary>
        ContentConstraintAttachment, 

        /// <summary>
        ///     The reference period.
        /// </summary>
        ReferencePeriod, 

        /// <summary>
        ///     The release calendar.
        /// </summary>
        ReleaseCalendar, 

        /// <summary>
        ///     The time range.
        /// </summary>
        TimeRange, 

        /// <summary>
        ///     The key values.
        /// </summary>
        KeyValues, 

        /// <summary>
        ///     The cube region.
        /// </summary>
        CubeRegion,

        /// <summary>
        ///     The constraint.
        /// </summary>
        Constraint, 

        /// <summary>
        ///     The metadata document.
        /// </summary>
        MetadataDocument, 

        /// <summary>
        ///     The metadata set.
        /// </summary>
        MetadataSet, 

        /// <summary>
        ///     The metadata report.
        /// </summary>
        MetadataReport, 

        /// <summary>
        ///     The metadata report target.
        /// </summary>
        MetadataReportTarget, 

        /// <summary>
        ///     The metadata reference value.
        /// </summary>
        MetadataReferenceValue, 

        /// <summary>
        ///     The metadata report attribute.
        /// </summary>
        MetadataReportAttribute, 

        // BASE

        /// <summary>
        ///     The agency scheme.
        /// </summary>
        AgencyScheme, 

        /// <summary>
        ///     The agency.
        /// </summary>
        Agency, 

        /// <summary>
        ///     The data provider scheme.
        /// </summary>
        DataProviderScheme, 

        /// <summary>
        ///     The data provider.
        /// </summary>
        DataProvider, 

        /// <summary>
        ///     The data consumer scheme.
        /// </summary>
        DataConsumerScheme, 

        /// <summary>
        ///     The data consumer.
        /// </summary>
        DataConsumer, 

        /// <summary>
        ///     The organisation unit scheme.
        /// </summary>
        OrganisationUnitScheme, 

        /// <summary>
        ///     The organisation unit.
        /// </summary>
        OrganisationUnit, 

        // CODELIST

        /// <summary>
        ///     The code list.
        /// </summary>
        CodeList, 

        /// <summary>
        ///     The code.
        /// </summary>
        Code, 

        /// <summary>
        ///     The hierarchical codelist.
        /// </summary>
        HierarchicalCodelist, 

        /// <summary>
        ///     The hierarchy.
        /// </summary>
        Hierarchy, 

        /// <summary>
        ///     The hierarchical code.
        /// </summary>
        HierarchicalCode, 

        /// <summary>
        ///     The level.
        /// </summary>
        Level, 

        // CATEGORY SCHEME

        /// <summary>
        ///     The categorisation.
        /// </summary>
        Categorisation, 

        /// <summary>
        ///     The category scheme.
        /// </summary>
        CategoryScheme, 

        /// <summary>
        ///     The category.
        /// </summary>
        Category, 

        /// <summary>
        ///     The reporting taxonomy.
        /// </summary>
        ReportingTaxonomy, 

        /// <summary>
        ///     The reporting category.
        /// </summary>
        ReportingCategory, 

        // CONCEPT SCHEME

        /// <summary>
        ///     The concept scheme.
        /// </summary>
        ConceptScheme, 

        /// <summary>
        ///     The concept.
        /// </summary>
        Concept, 

        // DATA STRUCTURE

        /// <summary>
        ///     The DSD.
        /// </summary>
        Dsd, 

        /// <summary>
        ///     The data attribute.
        /// </summary>
        DataAttribute, 

        /// <summary>
        ///     The attribute descriptor.
        /// </summary>
        AttributeDescriptor, 

        /// <summary>
        ///     The dataflow.
        /// </summary>
        Dataflow, 

        /// <summary>
        ///     The dimension.
        /// </summary>
        Dimension, 

        /// <summary>
        ///     The group.
        /// </summary>
        Group, 

        /// <summary>
        ///     The cross sectional measure.
        /// </summary>
        CrossSectionalMeasure, 

        /// <summary>
        ///     The dimension descriptor.
        /// </summary>
        DimensionDescriptor, 

        /// <summary>
        ///     The measure dimension.
        /// </summary>
        MeasureDimension, 

        /// <summary>
        ///     The measure descriptor.
        /// </summary>
        MeasureDescriptor, 

        /// <summary>
        ///     The primary measure.
        /// </summary>
        PrimaryMeasure, 

        /// <summary>
        ///     The time dimension.
        /// </summary>
        TimeDimension, 

        // METADATA STRUCTURE

        /// <summary>
        ///     The MSD.
        /// </summary>
        Msd, 

        /// <summary>
        ///     The report structure.
        /// </summary>
        ReportStructure, 

        /// <summary>
        ///     The metadata attribute.
        /// </summary>
        MetadataAttribute, 

        /// <summary>
        ///     The metadata target.
        /// </summary>
        MetadataTarget, 

        /// <summary>
        ///     The metadata flow.
        /// </summary>
        MetadataFlow, 

        /// <summary>
        ///     The identifiable maintainableObject target.
        /// </summary>
        IdentifiableObjectTarget, 

        /// <summary>
        ///     The dataset target.
        /// </summary>
        DatasetTarget, 

        /// <summary>
        ///     The constraint content target.
        /// </summary>
        ConstraintContentTarget, 

        /// <summary>
        ///     The dimension descriptor values target.
        /// </summary>
        DimensionDescriptorValuesTarget, 

        /// <summary>
        ///     The report period target.
        /// </summary>
        ReportPeriodTarget, 

        // PROCESS

        /// <summary>
        ///     The process.
        /// </summary>
        Process, 

        /// <summary>
        ///     The process step.
        /// </summary>
        ProcessStep, 

        /// <summary>
        ///     The transition.
        /// </summary>
        Transition, 

        // REGISTRY

        /// <summary>
        ///     The provision agreement.
        /// </summary>
        ProvisionAgreement, 

        /// <summary>
        ///     The registration.
        /// </summary>
        Registration, 

        /// <summary>
        ///     The subscription.
        /// </summary>
        Subscription, 

        /// <summary>
        ///     The attachment constraint.
        /// </summary>
        AttachmentConstraint, 

        /// <summary>
        ///     The content constraint.
        /// </summary>
        ContentConstraint, 

        // MAPPING

        /// <summary>
        ///     The structure set.
        /// </summary>
        StructureSet, 

        /// <summary>
        ///     The structure map.
        /// </summary>
        StructureMap, 

        /// <summary>
        ///     The reporting taxonomy map.
        /// </summary>
        ReportingTaxonomyMap, 

        /// <summary>
        ///     The representation map.
        /// </summary>
        RepresentationMap, // IMPORTANT WHAT IS THE OBJECT CLASS HERE?
        
        /// <summary>
        ///     The category map.
        /// </summary>
        CategoryMap, 

        /// <summary>
        ///     The category scheme map.
        /// </summary>
        CategorySchemeMap, 

        /// <summary>
        ///     The concept scheme map.
        /// </summary>
        ConceptSchemeMap, 

        /// <summary>
        ///     The code map.
        /// </summary>
        CodeMap, 

        /// <summary>
        ///     The code list map.
        /// </summary>
        CodeListMap, 

        /// <summary>
        ///     The component map.
        /// </summary>
        ComponentMap, 

        /// <summary>
        ///     The concept map.
        /// </summary>
        ConceptMap, 

        /// <summary>
        ///     The organisation map.
        /// </summary>
        OrganisationMap, 

        /// <summary>
        ///     The organisation scheme map.
        /// </summary>
        OrganisationSchemeMap, 

        /// <summary>
        ///     The hybrid codelist map.
        /// </summary>
        HybridCodelistMap, 

        /// <summary>
        ///     The hybrid code.
        /// </summary>
        HybridCode,

        /// <summary>
        /// Target region
        /// </summary>
        MetadataTargetRegion,

        /// <summary>
        ///     The organisation.
        /// </summary>
        Organisation,

        /// <summary>
        ///     The organisation scheme.
        /// </summary>
        OrganisationScheme
    }

    /// <summary>
    ///     The SDMX structure dictionary test type.
    /// </summary>
    public class SdmxStructureType : BaseConstantType<SdmxStructureEnumType>
    {
        #region Constants

        /// <summary>
        ///     The default version.
        /// </summary>
        private const string DefaultVersion = "1.0";

        #endregion

        #region Static Fields

        /// <summary>
        ///     The all instances.
        /// </summary>
        private static readonly IDictionary<SdmxStructureEnumType, SdmxStructureType> _instances =
            new Dictionary<SdmxStructureEnumType, SdmxStructureType>
                {
                    {
                        // NON-IDENTIFIABLES
                        SdmxStructureEnumType.Any, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Any, "Any")
                    }, 
                    {
                        SdmxStructureEnumType.Annotation, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Annotation, 
                        "Annotation")
                    }, 
                    {
                        SdmxStructureEnumType.CategoryId, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.CategoryId, 
                        "Category ID")
                    }, 
                    {
                        SdmxStructureEnumType.Datasource, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Datasource, 
                        "DataSource")
                    }, 
                    {
                        SdmxStructureEnumType.TextFormat, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.TextFormat, 
                        "Text Format")
                    }, 
                    {
                        SdmxStructureEnumType.TextType, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.TextType, "Text Type")
                    }, 
                    {
                        SdmxStructureEnumType.RelatedStructures, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.RelatedStructures, 
                        "Related Structures")
                    }, 
                    {
                        SdmxStructureEnumType.CodeListRef, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.CodeListRef, 
                        "CodelistRef")
                    },
                    {
                      SdmxStructureEnumType.Contact,
                      new SdmxStructureType(SdmxStructureEnumType.Contact,
                          "Contact")
                    },
                    {
                        SdmxStructureEnumType.Component, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Component, "Component")
                    }, 
                    {
                        SdmxStructureEnumType.Dataset, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Dataset, "DataSet")
                    }, 
                     {
                      SdmxStructureEnumType.DatasetReference,
                      new SdmxStructureType(SdmxStructureEnumType.DatasetReference,
                          "Dataset Reference")
                    },
                    {
                        SdmxStructureEnumType.ItemMap, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.ItemMap, "Item Map")
                    }, 
                    {
                        SdmxStructureEnumType.LocalRepresentation, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.LocalRepresentation, 
                        "Local Representation")
                    }, 
                    {
                        SdmxStructureEnumType.Computation, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Computation, 
                        "Computation")
                    }, 
                    {
                        SdmxStructureEnumType.InputOutput, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.InputOutput, 
                        "Input/Output")
                    }, 
                    {
                        SdmxStructureEnumType.ConstrainedDataKey, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.ConstrainedDataKey, 
                        "Constrained Data Key")
                    }, 
                    {
                        SdmxStructureEnumType.ConstrainedDataKeyset, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.ConstrainedDataKeyset, 
                        "Data Key Set")
                    }, 
                    {
                        SdmxStructureEnumType
                        .AttachmentConstraintAttachment, 
                        new SdmxStructureType(
                        SdmxStructureEnumType
                        .AttachmentConstraintAttachment, 
                        "Attachment Constraint Attachment")
                    }, 
                    {
                        // BASE
                        SdmxStructureEnumType
                        .ContentConstraintAttachment, 
                        new SdmxStructureType(
                        SdmxStructureEnumType
                        .ContentConstraintAttachment, 
                        "Content Constraint Attachment")
                    }, 
                    {
                        SdmxStructureEnumType.ReferencePeriod, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.ReferencePeriod, 
                        "Reference Period")
                    }, 
                    {
                        SdmxStructureEnumType.ReleaseCalendar, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.ReleaseCalendar, 
                        "Release Calendar")
                    }, 
                    {
                        SdmxStructureEnumType.TimeRange, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.TimeRange, "Time Range")
                    }, 
                    {
                        SdmxStructureEnumType.KeyValues, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.KeyValues, "Key Values")
                    }, 
                    {
                        SdmxStructureEnumType.CubeRegion, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.CubeRegion, 
                        "Cube Region")
                    }, 
                     {
                        SdmxStructureEnumType.Constraint, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Constraint, 
                        "Constraint")
                    },
                    {
                        SdmxStructureEnumType.MetadataDocument, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.MetadataDocument, 
                        "Metadata Document")
                    }, 
                    {
                        SdmxStructureEnumType.MetadataSet, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.MetadataSet, 
                        "Metadata Set")
                    }, 
                    {
                        SdmxStructureEnumType.MetadataReport, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.MetadataReport, 
                        "Metadata Report")
                    }, 
                    {
                        SdmxStructureEnumType.MetadataReportTarget, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.MetadataReportTarget, 
                        "Metadata Report Target")
                    }, 
                    {
                        SdmxStructureEnumType.MetadataReferenceValue, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.MetadataReferenceValue, 
                        "Metadata Reference Value")
                    }, 
                    {
                        SdmxStructureEnumType.MetadataReportAttribute, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.MetadataReportAttribute, 
                        "Metadata Reported Attribute")
                    }, 
                        {
                        SdmxStructureEnumType.MetadataTargetRegion, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.MetadataTargetRegion, 
                        "Metadata Target Region")
                    }, 
                    {
                        // BASE        
                        SdmxStructureEnumType.AgencyScheme, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.AgencyScheme, typeof(IAgencyScheme), 
                        "base", 
                        "AgencyScheme", 
                        "Agency Scheme")
                    }, 
                    {
                        SdmxStructureEnumType.Agency, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Agency, 
                        "base", 
                        "Agency", 
                        "Agency", 
                        SdmxStructureEnumType.AgencyScheme)
                    }, 
                    {
                        SdmxStructureEnumType.DataProviderScheme, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.DataProviderScheme, typeof(IDataProviderScheme), 
                        "base", 
                        "DataProviderScheme", 
                        "Data Provider Scheme")
                    }, 
                    {
                        SdmxStructureEnumType.DataProvider, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.DataProvider, 
                        "base", 
                        "DataProvider", 
                        "Data Provider", 
                        SdmxStructureEnumType.DataProviderScheme)
                    }, 
                    {
                        SdmxStructureEnumType.DataConsumerScheme, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.DataConsumerScheme, typeof(IDataConsumerScheme), 
                        "base", 
                        "DataConsumerScheme", 
                        "Data Consumer Scheme")
                    }, 
                    {
                        SdmxStructureEnumType.DataConsumer, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.DataConsumer, 
                        "base", 
                        "DataConsumer", 
                        "Data Consumer", 
                        SdmxStructureEnumType.DataConsumerScheme)
                    }, 
                    {
                        SdmxStructureEnumType.OrganisationUnitScheme, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.OrganisationUnitScheme, typeof(IOrganisationUnitSchemeObject), 
                        "base", 
                        "OrganisationUnitScheme", 
                        "Organisation Unit Scheme")
                    }, 
                    {
                        SdmxStructureEnumType.OrganisationUnit, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.OrganisationUnit, 
                        "base", 
                        "OrganisationUnit", 
                        "Organisation Unit", 
                        SdmxStructureEnumType.OrganisationUnitScheme)
                    }, 
                    {
                        // CODELIST        
                        SdmxStructureEnumType.CodeList, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.CodeList, typeof(ICodelistObject), 
                        "codelist", 
                        "CodeList", 
                        "codelist", 
                        "Codelist", 
                        "Codelist")
                    }, 
                    {
                        SdmxStructureEnumType.Code, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Code, 
                        "codelist", 
                        "Code", 
                        "Code", 
                        SdmxStructureEnumType.CodeList)
                    }, 
                    {
                        SdmxStructureEnumType.HierarchicalCodelist, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.HierarchicalCodelist, typeof(IHierarchicalCodelistObject), 
                        "codelist", 
                        "HierarchicCodeList", 
                        "codelist", 
                        "HierarchicalCodelist", 
                        "Hierarchical Code List")
                    }, 
                    {
                        SdmxStructureEnumType.Hierarchy, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Hierarchy, 
                        "codelist", 
                        "Hierarchy", 
                        "Hierarchy", 
                        SdmxStructureEnumType.HierarchicalCodelist)
                    }, 
                    {
                        SdmxStructureEnumType.HierarchicalCode, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.HierarchicalCode, 
                        "codelist", 
                        "HierarchicalCode", 
                        "Hierarchical Code", 
                        SdmxStructureEnumType.Hierarchy)
                    }, 
                    {
                        SdmxStructureEnumType.Level, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Level, 
                        "codelist", 
                        "Level", 
                        "Level", 
                        SdmxStructureEnumType.Hierarchy)
                    }, 
                    {
                        // CATEGORY SCHEME        
                        SdmxStructureEnumType.Categorisation, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Categorisation, typeof(ICategorisationObject), 
                        "categoryscheme", 
                        "Categorisation", 
                        "Categorisation")
                    }, 
                    {
                        SdmxStructureEnumType.CategoryScheme, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.CategoryScheme, typeof(ICategorySchemeObject), 
                        "categoryscheme", 
                        "CategoryScheme", 
                        "Category Scheme")
                    }, 
                    {
                        SdmxStructureEnumType.Category, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Category, 
                        "categoryscheme", 
                        "Category", 
                        "Category", 
                        SdmxStructureEnumType.CategoryScheme)
                    }, 
                    {
                        SdmxStructureEnumType.ReportingTaxonomy, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.ReportingTaxonomy, typeof(IReportingTaxonomyObject), 
                        "categoryscheme", 
                        "ReportingTaxonomy", 
                        "Reporting Taxonomy")
                    }, 
                    {
                        SdmxStructureEnumType.ReportingCategory, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.ReportingCategory, 
                        "categoryscheme", 
                        "ReportingCategory", 
                        "Reporting Category", 
                        SdmxStructureEnumType.ReportingTaxonomy)
                    }, 
                    {
                        // CONCEPT SCHEME        
                        SdmxStructureEnumType.ConceptScheme, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.ConceptScheme, typeof(IConceptSchemeObject), 
                        "conceptscheme", 
                        "ConceptScheme", 
                        "Concept Scheme")
                    }, 
                    {
                        SdmxStructureEnumType.Concept, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Concept, 
                        "conceptscheme", 
                        "Concept", 
                        "Concept", 
                        SdmxStructureEnumType.ConceptScheme)
                    }, 
                    {
                        // DATA STRUCTURE        
                        SdmxStructureEnumType.Dsd, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Dsd, typeof(IDataStructureObject), 
                        "keyfamily", 
                        "KeyFamily", 
                        "datastructure", 
                        "DataStructure", 
                        "Data Structure Definition")
                    }, 
                    {
                        SdmxStructureEnumType.DataAttribute, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.DataAttribute, 
                        "datastructure", 
                        "DataAttribute", 
                        "Data Attribute", 
                        SdmxStructureEnumType.AttributeDescriptor)
                    }, 
                    {
                        SdmxStructureEnumType.AttributeDescriptor, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.AttributeDescriptor, 
                        "datastructure", 
                        "AttributeDescriptor", 
                        "Attribute Descriptor", 
                        SdmxStructureEnumType.Dsd,
                        AttributeListObject.FixedId)
                    }, 
                    {
                        SdmxStructureEnumType.Dataflow, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Dataflow, typeof(IDataflowObject), 
                        "keyfamily", 
                        "Dataflow", 
                        "datastructure", 
                        "Dataflow", 
                        "Dataflow")
                    }, 
                    {
                        SdmxStructureEnumType.Dimension, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Dimension, 
                        "datastructure", 
                        "Dimension", 
                        "Dimension", 
                        SdmxStructureEnumType.DimensionDescriptor)
                    }, 
                    {
                        SdmxStructureEnumType.Group, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Group, 
                        "datastructure", 
                        "GroupDimensionDescriptor", 
                        "Group Dimension Descriptor", 
                        SdmxStructureEnumType.Dsd)
                    }, 
                    {
                        SdmxStructureEnumType.CrossSectionalMeasure, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.CrossSectionalMeasure, 
                        "datastructure", 
                        "CrossSectionalMeasure", 
                        "Cross Sectional Measure", 
                        SdmxStructureEnumType.Dsd)
                    }, 
                    {
                        SdmxStructureEnumType.DimensionDescriptor, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.DimensionDescriptor, 
                        "datastructure", 
                        "DimensionDescriptor", 
                        "Dimension Descriptor", 
                        SdmxStructureEnumType.Dsd,
                        DimensionList.FixedId)
                    }, 
                    {
                        SdmxStructureEnumType.MeasureDimension, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.MeasureDimension, 
                        "datastructure", 
                        "MeasureDimension", 
                        "Measure Dimension", 
                        SdmxStructureEnumType.Dsd)
                    }, 
                    {
                        SdmxStructureEnumType.MeasureDescriptor, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.MeasureDescriptor, 
                        "datastructure", 
                        "MeasureDescriptor", 
                        "Measure Descriptor", 
                        SdmxStructureEnumType.Dsd,
                        MeasureList.FixedId)
                    }, 
                    {
                        SdmxStructureEnumType.PrimaryMeasure, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.PrimaryMeasure, 
                        "datastructure", 
                        "PrimaryMeasure", 
                        "Primary Measure", 
                        SdmxStructureEnumType.MeasureDescriptor,
                        PrimaryMeasure.FixedId)
                    }, 
                    {
                        SdmxStructureEnumType.TimeDimension, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.TimeDimension, 
                        "datastructure", 
                        "TimeDimension", 
                        "Time Dimension", 
                        SdmxStructureEnumType.Dsd)
                    }, 
                    {
                        // METADATA STRUCTURE        
                        SdmxStructureEnumType.Msd, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Msd, typeof(IMetadataStructureDefinitionObject), 
                        "metadatastructure", 
                        "MetadataStructure", 
                        "Metadata Structure")
                    }, 
                    {
                        SdmxStructureEnumType.ReportStructure, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.ReportStructure, 
                        "metadatastructure", 
                        "ReportStructure", 
                        "Report Structure", 
                        SdmxStructureEnumType.Msd)
                    }, 
                    {
                        SdmxStructureEnumType.MetadataAttribute, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.MetadataAttribute, 
                        "metadatastructure", 
                        "MetadataAttribute", 
                        "Metadata Attribute", 
                        SdmxStructureEnumType.ReportStructure)
                    }, 
                    {
                        SdmxStructureEnumType.MetadataTarget, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.MetadataTarget, 
                        "metadatastructure", 
                        "MetadataTarget", 
                        "Metadata Target", 
                        SdmxStructureEnumType.Msd)
                    }, 
                    {
                        SdmxStructureEnumType.MetadataFlow, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.MetadataFlow, typeof(IMetadataFlow), 
                        "metadatastructure", 
                        "Metadataflow", 
                        "Metadataflow")
                    }, 
                    {
                        SdmxStructureEnumType.IdentifiableObjectTarget, 
                        new SdmxStructureType(
                        SdmxStructureEnumType
                        .IdentifiableObjectTarget, 
                        "metadatastructure", 
                        "IdentifiableObjectTarget", 
                        "Identifiable Object Target", 
                        SdmxStructureEnumType.MetadataTarget)
                    }, 
                    {
                        SdmxStructureEnumType.DatasetTarget, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.DatasetTarget, 
                        "metadatastructure", 
                        "DataSetTarget", 
                        "Data Set Target", 
                        SdmxStructureEnumType.MetadataTarget)
                    }, 
                    {
                        SdmxStructureEnumType.ConstraintContentTarget, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.ConstraintContentTarget, 
                        "metadatastructure", 
                        "ConstraintContentTarget", 
                        "Constraint Content Set Target", 
                        SdmxStructureEnumType.MetadataTarget)
                    }, 
                    {
                        SdmxStructureEnumType
                        .DimensionDescriptorValuesTarget, 
                        new SdmxStructureType(
                        SdmxStructureEnumType
                        .DimensionDescriptorValuesTarget, 
                        "metadatastructure", 
                        "DimensionDescriptorValuesTarget", 
                        "Dimension Descriptor Values Target", 
                        SdmxStructureEnumType.MetadataTarget)
                    }, 
                    {
                        SdmxStructureEnumType.ReportPeriodTarget, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.ReportPeriodTarget, 
                        "metadatastructure", 
                        "ReportPeriodTarget", 
                        "Report Period Target", 
                        SdmxStructureEnumType.MetadataTarget)
                    }, 
                    {
                        // PROCESS        
                        SdmxStructureEnumType.Process, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Process, typeof(IProcessObject), 
                        "process", 
                        "Process", 
                        "Process")
                    }, 
                    {
                        SdmxStructureEnumType.ProcessStep, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.ProcessStep, 
                        "process", 
                        "ProcessStep", 
                        "Process Step", 
                        SdmxStructureEnumType.Process)
                    }, 
                    {
                        SdmxStructureEnumType.Transition, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Transition, 
                        "process", 
                        "Transition", 
                        "Transition", 
                        SdmxStructureEnumType.ProcessStep)
                    }, 
                    {
                        // REGISTRY        
                        SdmxStructureEnumType.ProvisionAgreement, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.ProvisionAgreement, typeof(IProvisionAgreementObject), 
                        "registry", 
                        "ProvisionAgreement", 
                        "Provision Agreement")
                    }, 
                    {
                        SdmxStructureEnumType.Registration, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Registration, typeof(IRegistrationObject), 
                        "registry", 
                        "Registration", 
                        "Registration")
                    }, 
                    {
                        SdmxStructureEnumType.Subscription, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Subscription, typeof(ISubscriptionObject), 
                        "registry", 
                        "Subscription", 
                        "Subscription")
                    }, 
                    {
                        SdmxStructureEnumType.AttachmentConstraint, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.AttachmentConstraint, typeof(IAttachmentConstraintObject), 
                        "registry", 
                        "AttachmentConstraint", 
                        "Attachment Constraint")
                    }, 
                    {
                        SdmxStructureEnumType.ContentConstraint, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.ContentConstraint, typeof(IContentConstraintObject), 
                        "registry", 
                        "ContentConstraint", 
                        "Content Constraint")
                    }, 
                    {
                        // MAPPING        
                        SdmxStructureEnumType.StructureSet, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.StructureSet, typeof(IStructureSetObject), 
                        "mapping", 
                        "StructureSet", 
                        "Structure Set")
                    }, 
                    {
                        SdmxStructureEnumType.StructureMap, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.StructureMap, 
                        "mapping", 
                        "StructureMap", 
                        "Structure Map", 
                        SdmxStructureEnumType.StructureSet)
                    }, 
                    {
                        SdmxStructureEnumType.ReportingTaxonomyMap, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.ReportingTaxonomyMap, 
                        "mapping", 
                        "ReportingTaxonomyMap", 
                        "Reporting Taxonomy Map", 
                        SdmxStructureEnumType.StructureSet)
                    }, 
                    {
                        SdmxStructureEnumType.RepresentationMap, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.RepresentationMap, 
                        "mapping", 
                        "RepresentationMap", 
                        "Representation Map", 
                        SdmxStructureEnumType.StructureSet, 
                        false)
                    }, 
                    {
                        // IMPORTANT WHAT IS THE OBJECT CLASS HERE        
                        SdmxStructureEnumType.CategoryMap, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.CategoryMap, 
                        "mapping", 
                        "CategoryMap", 
                        "Category Map", 
                        SdmxStructureEnumType.StructureSet, 
                        false)
                    }, 
                    {
                        SdmxStructureEnumType.CategorySchemeMap, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.CategorySchemeMap, 
                        "mapping", 
                        "CategorySchemeMap", 
                        "Category Scheme Map", 
                        SdmxStructureEnumType.StructureSet)
                    }, 
                    {
                        SdmxStructureEnumType.ConceptSchemeMap, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.ConceptSchemeMap, 
                        "mapping", 
                        "ConceptSchemeMap", 
                        "Concept Scheme Map", 
                        SdmxStructureEnumType.StructureSet)
                    }, 
                    {
                        SdmxStructureEnumType.CodeMap, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.CodeMap, 
                        "mapping", 
                        "CodeMap", 
                        "Code Map", 
                        SdmxStructureEnumType.StructureSet)
                    }, 
                    {
                        SdmxStructureEnumType.CodeListMap, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.CodeListMap, 
                        "mapping", 
                        "CodelistMap", 
                        "CodelistMap", 
                        SdmxStructureEnumType.StructureSet)
                    }, 
                    {
                        SdmxStructureEnumType.ComponentMap, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.ComponentMap, 
                        "mapping", 
                        "ComponentMap", 
                        "Component Map", 
                        SdmxStructureEnumType.StructureSet, 
                        false)
                    }, 
                    {
                        SdmxStructureEnumType.ConceptMap, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.ConceptMap, 
                        "mapping", 
                        "ConceptMap", 
                        "Concept Map", 
                        SdmxStructureEnumType.StructureSet)
                    }, 
                    {
                        SdmxStructureEnumType.OrganisationMap, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.OrganisationMap, 
                        "mapping", 
                        "OrganisationMap", 
                        "Organisation Map", 
                        SdmxStructureEnumType.StructureSet)
                    }, 
                    {
                        SdmxStructureEnumType.OrganisationSchemeMap, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.OrganisationSchemeMap, 
                        "mapping", 
                        "OrganisationSchemeMap", 
                        "Organisation Scheme Map", 
                        SdmxStructureEnumType.StructureSet)
                    }, 
                    {
                        SdmxStructureEnumType.HybridCodelistMap, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.HybridCodelistMap, 
                        "mapping", 
                        "HybridCodeListMap", 
                        "Hybrid Code List Map", 
                        SdmxStructureEnumType.StructureSet)
                    }, 
                    {
                        SdmxStructureEnumType.HybridCode, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.HybridCode, 
                        "mapping", 
                        "HybridCodeMap", 
                        "Hybrid Code Map", 
                        SdmxStructureEnumType.StructureSet)
                    },
                    {
                        SdmxStructureEnumType.Organisation, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.Organisation, 
                        "Organisation")
                    },
                    {
                        SdmxStructureEnumType.OrganisationScheme, 
                        new SdmxStructureType(
                        SdmxStructureEnumType.OrganisationScheme, 
                        "Organisation Scheme")
                    }
                };

        /// <summary>
        ///     The _maintainable types.
        /// </summary>
        private static readonly IList<SdmxStructureType> _maintainableTypes =
            (from v in _instances.Values where v.IsMaintainable select v).ToList();

        #endregion

        #region Fields

        /// <summary>
        ///     The _is identifiable.
        /// </summary>
        private readonly bool _isIdentifiable;

        /// <summary>
        ///     The _is maintainable.
        /// </summary>
        private readonly bool _isMaintainable;

        /// <summary>
        ///     The parent structure as a <see cref="SdmxStructureEnumType" />
        /// </summary>
        private readonly SdmxStructureEnumType _parentStructureEnumType;

        /// <summary>
        ///     The _type.
        /// </summary>
        private readonly string _structureType;

        /// <summary>
        ///     The _urn class.
        /// </summary>
        private readonly string _urnClass;

        /// <summary>
        ///     The _urn package.
        /// </summary>
        private readonly string _urnPackage;

        /// <summary>
        ///     The _urn prefix.
        /// </summary>
        private readonly string _urnPrefix;

        /// <summary>
        ///     The _fixed id.
        /// </summary>
        private readonly string _fixedId;

        /// <summary>
        ///     The _v2 class.
        /// </summary>
        private readonly string _version2Class;

        /// <summary>
        ///     The _v2 package.
        /// </summary>
        private readonly string _version2Package;

        /// <summary>
        ///     The _version 2 urn prefix.
        /// </summary>
        private readonly string _version2UrnPrefix;

        /// <summary>
        /// The maintainable type
        /// </summary>
        private readonly Type _maintainableType;

        /// <summary>
        ///     The _parent structure type.
        /// </summary>
        private SdmxStructureType _parentStructureType;

        #endregion

        /* !!! */
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxStructureType"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The <see cref="SdmxStructureEnumType"/> type.
        /// </param>
        /// <param name="structureType">
        /// The type.
        /// </param>
        private SdmxStructureType(SdmxStructureEnumType enumType, string structureType)
            : base(enumType)
        {
            this._structureType = structureType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxStructureType"/> class.
        /// </summary>
        /// <param name="enumType">
        ///     The <see cref="SdmxStructureEnumType"/> type.
        /// </param>
        /// <param name="maintainableType"></param>
        /// <param name="urnPackage">
        ///     The urn package.
        /// </param>
        /// <param name="urnClass">
        ///     The urn class.
        /// </param>
        /// <param name="type">
        ///     The type.
        /// </param>
        private SdmxStructureType(SdmxStructureEnumType enumType, Type maintainableType, string urnPackage, string urnClass, string type)
            : this(enumType, maintainableType, null, null, urnPackage, urnClass, type)
        {
            this._urnPackage = urnPackage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxStructureType" /> class.
        /// </summary>
        /// <param name="enumType">The <see cref="SdmxStructureEnumType" /> type.</param>
        /// <param name="maintainableType">Type of the maintainable.</param>
        /// <param name="version2Package">The version 2 package.</param>
        /// <param name="version2Class">The version 2 class.</param>
        /// <param name="urnPackage">The urn package.</param>
        /// <param name="urnClass">The urn class.</param>
        /// <param name="type">The type.</param>
        private SdmxStructureType(SdmxStructureEnumType enumType, Type maintainableType, string version2Package, string version2Class, string urnPackage, string urnClass, string type)
            : this(enumType, urnPackage, urnClass, type, SdmxStructureEnumType.Null)
        {
            this._maintainableType = maintainableType;
            this._version2Class = version2Class;
            this._version2Package = version2Package;
            this._version2UrnPrefix = "urn:sdmx:org.sdmx.infomodel." + version2Package + "." + version2Class + "=";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxStructureType"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The <see cref="SdmxStructureEnumType"/> type.
        /// </param>
        /// <param name="urnPackage">
        /// The urn package.
        /// </param>
        /// <param name="urnClass">
        /// The urn class.
        /// </param>
        /// <param name="structureType">
        /// The type.
        /// </param>
        /// <param name="parentStructureType">
        /// The parent structure type.
        /// </param>
        private SdmxStructureType(
            SdmxStructureEnumType enumType,
            string urnPackage,
            string urnClass,
            string structureType,
            SdmxStructureEnumType parentStructureType)
            : this(enumType, urnPackage, urnClass, structureType, parentStructureType, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxStructureType"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The <see cref="SdmxStructureEnumType"/> type.
        /// </param>
        /// <param name="urnPackage">
        /// The urn package.
        /// </param>
        /// <param name="urnClass">
        /// The urn class.
        /// </param>
        /// <param name="structureType">
        /// The type.
        /// </param>
        /// <param name="parentStructureType">
        /// The parent structure type.
        /// </param>
        /// <param name="fixedId">
        /// The fixed id.
        /// </param>
        private SdmxStructureType(
            SdmxStructureEnumType enumType, 
            string urnPackage, 
            string urnClass, 
            string structureType,
            SdmxStructureEnumType parentStructureType,
            string fixedId)
            : base(enumType)
        {
            this._urnPackage = urnPackage;
            this._urnClass = urnClass;
            this._structureType = structureType;
            this._isMaintainable = parentStructureType == SdmxStructureEnumType.Null;
            this._parentStructureEnumType = parentStructureType;
            this._isIdentifiable = true;
            this._fixedId = fixedId;
            this._urnPrefix = "urn:sdmx:org.sdmx.infomodel." + urnPackage + "." + urnClass + "=";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxStructureType"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The <see cref="SdmxStructureEnumType"/> type.
        /// </param>
        /// <param name="urnPackage">
        /// The urn package.
        /// </param>
        /// <param name="urnClass">
        /// The urn class.
        /// </param>
        /// <param name="structureType">
        /// The type.
        /// </param>
        /// <param name="parentStructureType">
        /// The parent structure type.
        /// </param>
        /// <param name="isIdentifiable">
        /// The is identifiable.
        /// </param>
        private SdmxStructureType(
            SdmxStructureEnumType enumType, 
            string urnPackage, 
            string urnClass, 
            string structureType, 
            SdmxStructureEnumType parentStructureType, 
            bool isIdentifiable)
            : this(enumType, urnPackage, urnClass, structureType, parentStructureType)
        {
            this._isIdentifiable = isIdentifiable;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the maintainable structure types.
        /// </summary>
        public static IList<SdmxStructureType> MaintainableStructureTypes
        {
            get
            {
                return new List<SdmxStructureType>(_maintainableTypes);
            }
        }

        /// <summary>
        ///     Gets the values.
        /// </summary>
        public static IEnumerable<SdmxStructureType> Values
        {
            get
            {
                return _instances.Values;
            }
        }

        /// <summary>
        /// Get the fixed id.
        /// </summary>
        public string FixedId
        {
            get
            {
                return this._fixedId;
            }
        }

        /// <summary>
        ///     Gets a value indicating if a fixed id exists.
        /// </summary>
        public bool HasFixedId
        {
            get
            {
                return this._fixedId != null;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether is identifiable.
        /// </summary>
        public bool IsIdentifiable
        {
            get
            {
                return this._isIdentifiable;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether is maintainable.
        /// </summary>
        public bool IsMaintainable
        {
            get
            {
                return this._isMaintainable;
            }
        }

        /// <summary>
        ///     Gets the minimum nested depth this structure is from the maintainable parent, 0 indexed
        /// </summary>
        public int NestedDepth
        {
            get
            {
                SdmxStructureType currentParent = this.ParentStructureType;
                int i = 0;
                if (currentParent != null)
                {
                    while (!currentParent.IsMaintainable)
                    {
                        i++;
                        currentParent = currentParent.ParentStructureType;
                    }
                }

                return i;
            }
        }

        /// <summary>
        ///     Gets the parent structure type.
        /// </summary>
        public SdmxStructureType ParentStructureType
        {
            get
            {
                SdmxStructureType parentType;
                if (this._parentStructureEnumType != SdmxStructureEnumType.Null && this._parentStructureType == null
                    && _instances.TryGetValue(this._parentStructureEnumType, out parentType))
                {
                    this._parentStructureType = parentType;
                }

                return this._parentStructureType;
            }
        }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        /// <remarks>This corresponds to the Java "getType()". Renamed to <c>StructureType</c> because of <c>FxCop</c> rule <c>CA1721</c></remarks>
        public string StructureType
        {
            get
            {
                return this._structureType;
            }
        }

        /// <summary>
        ///     Gets the urn class.
        /// </summary>
        public string UrnClass
        {
            get
            {
                return this._urnClass;
            }
        }

        /// <summary>
        ///     Gets the urn package.
        /// </summary>
        public string UrnPackage
        {
            get
            {
                return this._urnPackage;
            }
        }

        /// <summary>
        ///     Gets the urn prefix.
        /// </summary>
        public string UrnPrefix
        {
            get
            {
                return this._urnPrefix;
            }
        }

        /// <summary>
        ///     Gets the version 2 class.
        /// </summary>
        public string V2Class
        {
            get
            {
                return this._version2Class;
            }
        }

        /// <summary>
        ///     Gets the version 2 package.
        /// </summary>
        public string V2Package
        {
            get
            {
                return this._version2Package;
            }
        }

        /// <summary>
        ///     Gets the version 2 urn prefix.
        /// </summary>
        public string V2UrnPrefix
        {
            get
            {
                return this._version2UrnPrefix;
            }
        }

        /// <summary>
        /// Gets the maintainable interface.
        /// </summary>
        /// <value>
        /// The maintainable interface.
        /// </value>
        public Type MaintainableInterface
        {
            get
            {
                var maintainableType = this.MaintainableStructureType;
                return maintainableType == null ? null : maintainableType._maintainableType;
            }
        }

        /// <summary>
        /// Gets the type of the maintainable structure.
        /// </summary>
        /// <value>
        /// The type of the maintainable structure.
        /// </value>
        public SdmxStructureType MaintainableStructureType
        {
            get
            {
                SdmxStructureType type = this;
                while (type != null)
                {
                    if (type.IsMaintainable)
                    {
                        return type;
                    }

                    type = type.ParentStructureType;
                }

                return null;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Parses the specified <paramref name="structureType"/> and returns the corresponding SdmxStructureType.
        /// </summary>
        /// <param name="structureType"><see cref="StructureType"/> of the structure.</param>
        /// <returns>The corresponding <see cref="SdmxStructureType"/></returns>
        /// <exception cref="SdmxSyntaxException">Unknown Maintainable Type</exception>
        public static SdmxStructureType ParseClass(Type structureType)
        {
            foreach (var sdmxStructureType in _maintainableTypes)
            {
                if (sdmxStructureType.MaintainableInterface == structureType)
                {
                    return sdmxStructureType;
                }
            }

            throw new SdmxSyntaxException(string.Format(CultureInfo.InvariantCulture, "Unknown Maintainable Type:{0}", structureType));
        }

        /// <summary>
        /// Gets a <see cref="SdmxStructureType"/> from the specified <paramref name="sdmxStructure"/>
        /// </summary>
        /// <param name="sdmxStructure">
        /// The <see cref="SdmxStructureEnumType"/>
        /// </param>
        /// <returns>
        /// a <see cref="SdmxStructureType"/> from the specified <paramref name="sdmxStructure"/>
        /// </returns>
        public static SdmxStructureType GetFromEnum(SdmxStructureEnumType sdmxStructure)
        {
            SdmxStructureType sdmxStructureType;
            if (_instances.TryGetValue(sdmxStructure, out sdmxStructureType))
            {
                return sdmxStructureType;
            }

            return null;
        }

        /// <summary>
        /// Parses a string representation of the Object Class (ignores case) to return the Structure Type.
        ///     Accepts SDMX v2.0 Class and 2.1 Class
        ///     Example : <c>Metadataflow</c> would return <see cref="SdmxStructureEnumType.MetadataFlow"/>
        /// </summary>
        /// <param name="structureClass">
        /// The Structure Class
        /// </param>
        /// <returns>
        /// The <see cref="SdmxStructureType"/> .
        /// </returns>
        public static SdmxStructureType ParseClass(string structureClass)
        {
            if (structureClass == null)
            {
                throw new ArgumentException("SDMX_STRUCTURE_TYPE.parseClass(..) can not be passed a null value");
            }

            foreach (SdmxStructureType currentType in Values)
            {
                if (currentType.IsIdentifiable)
                {
                    if (currentType.UrnClass.Equals(structureClass, StringComparison.OrdinalIgnoreCase))
                    {
                        return currentType;
                    }

                    if (currentType.V2Class != null
                        && currentType.V2Class.Equals(structureClass, StringComparison.OrdinalIgnoreCase))
                    {
                        return currentType;
                    }
                }
            }

            if (structureClass.Equals("Attribute"))
            {
                return GetFromEnum(SdmxStructureEnumType.DataAttribute);
            }

            throw new ArgumentException("Could not find structure type with class '" + structureClass + "'");
        }

        /// <summary>
        /// Parses a string representation of the Object Class (ignores case) and package (ignores case) to return the Structure Type.
        ///     Accepts SDMX v2.0 and 2.1 package and class
        /// </summary>
        /// <param name="structurePackage">The package name. </param>
        /// <param name="structureClass">The structure class. </param>
        /// <returns>
        /// The <see cref="SdmxStructureType"/> .
        /// </returns>
        public static SdmxStructureType ParsePackageAndClass(string structurePackage, string structureClass)
        {

            if (structureClass == null)
            {
                throw new ArgumentException("SDMX_STRUCTURE_TYPE.parsePackageAndClass(..) can not be passed a null value");
            }

            foreach (SdmxStructureType currentType in Values)
            {
                if (currentType.IsIdentifiable)
                {
                    if (currentType.UrnClass.Equals(structureClass) && currentType.UrnPackage.Equals(structurePackage))
                    {
                        return currentType;
                    }

                    if (currentType.V2Class != null && currentType.V2Package != null)
                    {
                        if (currentType.V2Class.Equals(structureClass) && currentType.V2Package.Equals(structurePackage))
                        {
                            return currentType;
                        }
                    }
                }
            }

            if (structureClass.Equals("Attribute"))
            {
                return GetFromEnum(SdmxStructureEnumType.DataAttribute);
            }

            throw new ArgumentException(
                "Could not find structure type with package '" + structurePackage + "' and class '" + structureClass
                + "'");
        }

        /// <summary>
        /// Parses a URN prefix (ignores case) to return the STRUCTURE_TYPE, throws an exception if the prefix does not belong to a structure type
        ///     Accepts SDMX v2.0 prefixes and 2.1 prefixes
        /// </summary>
        /// <param name="prefix">
        /// The URN prefix
        /// </param>
        /// <returns>
        /// The <see cref="SdmxStructureType"/> .
        /// </returns>
        public static SdmxStructureType ParsePrefix(string prefix)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException("prefix");
            }

            if (!prefix.EndsWith("=", StringComparison.Ordinal))
            {
                prefix += "=";
            }

            foreach (SdmxStructureType currentType in Values)
            {
                if (currentType.UrnPrefix != null)
                {
                    if (currentType.UrnPrefix.Equals(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        return currentType;
                    }
                }
            }

            foreach (SdmxStructureType currentType in Values)
            {
                if (currentType.V2UrnPrefix != null)
                {
                    if (currentType.V2UrnPrefix.Equals(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        return currentType;
                    }
                }
            }

            throw new SdmxSyntaxException(ExceptionCode.StructureUrnMalformedUnknownPrefix, prefix);
        }

        /// <summary>
        /// Generates a URN with the given agencyId, maintainable Id, and version, and additional identifiers (if this is not a maintainable type)
        /// </summary>
        /// <param name="agencyId">
        /// the agency Id required
        /// </param>
        /// <param name="maintainableId">
        /// The maintainable Id required
        /// </param>
        /// <param name="version">
        /// version defaults to MaintainableObject.DEFAULT_VERSION
        /// </param>
        /// <param name="identifiableIds">
        /// identifiableIds required if this is an identifiable structure, otherwise not required
        /// </param>
        /// <returns>
        /// SDMX urn
        /// </returns>
        public Uri GenerateUrn(string agencyId, string maintainableId, string version, params string[] identifiableIds)
        {
            if (string.IsNullOrEmpty(agencyId))
            {
                throw new ArgumentNullException("agencyId");
            }

            if (string.IsNullOrEmpty(maintainableId))
            {
                throw new ArgumentNullException("maintainableId");
            }

            if (string.IsNullOrEmpty(version))
            {
                // %%% Hardcoded the 1.0  as we cannot have const on interfaces 
                // %%% version = IMaintainableObject.DEFAULT_VERSION;
                version = DefaultVersion;
            }

            if (this.EnumType == SdmxStructureEnumType.Agency && identifiableIds != null)
            {
                if(agencyId.Equals(InterfaceConstant.AgencyScheme.DefaultScheme)) {
				    return new Uri(this._urnPrefix + identifiableIds[0]);
			    }

			    string id = identifiableIds.Aggregate("", (current, currentIdentId) => current + ("." + currentIdentId));
                    return new Uri(this._urnPrefix + agencyId + id);
            }

            if (identifiableIds != null && identifiableIds.Length > 0)
            {
                if (this.IsMaintainable)
                {
                    throw new ArgumentException(
                        "Generate maintainable URN given too many args (given identifiable ids) ");
                }
            }

            if (!this.IsMaintainable)
            {
                if (identifiableIds == null || identifiableIds.Length == 0)
                {
                    throw new ArgumentException("Generate identifiable URN missing required identifiable id");
                }
            }

            string returnString = string.Format(
                CultureInfo.InvariantCulture, "{0}{1}:{2}({3})", this._urnPrefix, agencyId, maintainableId, version);
            if (identifiableIds != null)
            {
                returnString = identifiableIds.Aggregate(
                    returnString, 
                    (current, currentIdentId) =>
                    string.Format(CultureInfo.InvariantCulture, "{0}.{1}", current, currentIdentId));
            }

            return new Uri(returnString);
        }

        /// <summary>
        ///     Gets a <see cref="string" /> that represents the current <see cref="SdmxStructureType" />.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents the current <see cref="SdmxStructureType" /> .
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return this._structureType;
        }

        #endregion
    }
}