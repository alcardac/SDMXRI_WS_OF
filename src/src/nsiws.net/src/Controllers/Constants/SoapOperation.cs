// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SoapOperation.cs" company="Eurostat">
//   Date Created : 2013-10-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The soap operation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Constants
{
    using System;
    using System.Xml;

    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    ///     The soap operation.
    /// </summary>
    public enum SoapOperation
    {
        /// <summary>
        ///     The default value.
        /// </summary>
        Null = 0, 

        /// <summary>
        ///     The get compact data.
        /// </summary>
        GetCompactData, 

        /// <summary>
        ///     The get compact data.
        /// </summary>
        GetUtilityData, 

        /// <summary>
        ///     The get cross sectional data.
        /// </summary>
        GetCrossSectionalData, 

        /// <summary>
        ///     The get generic data.
        /// </summary>
        GetGenericData, 

        /// <summary>
        ///     The get generic time series data.
        /// </summary>
        GetGenericTimeSeriesData, 

        /// <summary>
        ///     The get structure specific data.
        /// </summary>
        GetStructureSpecificData, 

        /// <summary>
        ///     The get structure specific time series data.
        /// </summary>
        GetStructureSpecificTimeSeriesData, 

        /// <summary>
        ///     The get generic metadata.
        /// </summary>
        GetGenericMetadata, 

        /// <summary>
        ///     The get structure specific metadata.
        /// </summary>
        GetStructureSpecificMetadata, 

        /// <summary>
        ///     The get structures.
        /// </summary>
        GetStructures, 

        /// <summary>
        ///     The get dataflow.
        /// </summary>
        GetDataflow, 

        /// <summary>
        ///     The get metadataflow.
        /// </summary>
        GetMetadataflow, 

        /// <summary>
        ///     The get data structure.
        /// </summary>
        GetDataStructure, 

        /// <summary>
        ///     The get metadata structure.
        /// </summary>
        GetMetadataStructure, 

        /// <summary>
        ///     The get category scheme.
        /// </summary>
        GetCategoryScheme, 

        /// <summary>
        ///     The get concept scheme.
        /// </summary>
        GetConceptScheme, 

        /// <summary>
        ///     The get codelist.
        /// </summary>
        GetCodelist, 

        /// <summary>
        ///     The get hierarchical codelist.
        /// </summary>
        GetHierarchicalCodelist, 

        /// <summary>
        ///     The get organisation scheme.
        /// </summary>
        GetOrganisationScheme, 

        /// <summary>
        ///     The get reporting taxonomy.
        /// </summary>
        GetReportingTaxonomy, 

        /// <summary>
        ///     The get structure set.
        /// </summary>
        GetStructureSet, 

        /// <summary>
        ///     The get process.
        /// </summary>
        GetProcess, 

        /// <summary>
        ///     The get categorisation.
        /// </summary>
        GetCategorisation, 

        /// <summary>
        ///     The get provision agreement.
        /// </summary>
        GetProvisionAgreement, 

        /// <summary>
        ///     The get constraint.
        /// </summary>
        GetConstraint, 

        /// <summary>
        ///     The get data schema.
        /// </summary>
        GetDataSchema, 

        /// <summary>
        ///     The get metadata schema.
        /// </summary>
        GetMetadataSchema, 

        /// <summary>
        ///     The query structure
        /// </summary>
        QueryStructure
    }

    /// <summary>
    ///     The soap operation extension.
    /// </summary>
    public static class SoapOperationExtension
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get message type.
        /// </summary>
        /// <param name="operation">
        /// The operation.
        /// </param>
        /// <returns>
        /// The <see cref="MessageEnumType"/>.
        /// </returns>
        public static MessageEnumType GetMessageType(this SoapOperation operation)
        {
            switch (operation)
            {
                case SoapOperation.GetCompactData:
                case SoapOperation.GetStructureSpecificData:
                case SoapOperation.GetStructureSpecificTimeSeriesData:
                    return MessageEnumType.CompactData;
                case SoapOperation.GetUtilityData:
                    return MessageEnumType.UtilityData;
                case SoapOperation.GetCrossSectionalData:
                    return MessageEnumType.CrossSectionalData;
                case SoapOperation.GetGenericData:
                    return MessageEnumType.GenericData;
                case SoapOperation.GetGenericTimeSeriesData:
                    return MessageEnumType.GenericData;
                case SoapOperation.GetGenericMetadata:
                    return MessageEnumType.GenericMetadata;
                case SoapOperation.GetStructureSpecificMetadata:
                    return MessageEnumType.MetadataReport;
                case SoapOperation.GetStructures:
                case SoapOperation.GetDataflow:
                case SoapOperation.GetMetadataflow:
                case SoapOperation.GetDataStructure:
                case SoapOperation.GetMetadataStructure:
                case SoapOperation.GetCategoryScheme:
                case SoapOperation.GetConceptScheme:
                case SoapOperation.GetCodelist:
                case SoapOperation.GetHierarchicalCodelist:
                case SoapOperation.GetOrganisationScheme:
                case SoapOperation.GetReportingTaxonomy:
                case SoapOperation.GetStructureSet:
                case SoapOperation.GetProcess:
                case SoapOperation.GetCategorisation:
                case SoapOperation.GetProvisionAgreement:
                case SoapOperation.GetConstraint:
                    return MessageEnumType.Structure;
                case SoapOperation.GetDataSchema:
                    return MessageEnumType.Unknown;
                case SoapOperation.QueryStructure:
                    return MessageEnumType.RegistryInterface;
            }

            return MessageEnumType.Null;
        }

        /// <summary>
        /// Gets the query root element for SDMX V20.
        /// </summary>
        /// <param name="operation">
        /// The operation.
        /// </param>
        /// <returns>
        /// The query root element qualified name; otherwise <see cref="XmlQualifiedName.Empty"/>
        /// </returns>
        public static XmlQualifiedName GetQueryRootElementV20(this SoapOperation operation)
        {
            ElementNameTable element;
            switch (operation)
            {
                case SoapOperation.GetUtilityData:
                case SoapOperation.GetCompactData:
                case SoapOperation.GetCrossSectionalData:
                case SoapOperation.GetGenericData:
                case SoapOperation.GetGenericMetadata:
                    element = ElementNameTable.QueryMessage;
                    break;
                case SoapOperation.QueryStructure:
                    element = ElementNameTable.RegistryInterface;
                    break;
                default:
                    return XmlQualifiedName.Empty;
            }

            return new XmlQualifiedName(NameTableCache.GetElementName(element), SdmxConstants.MessageNs20);
        }

        /// <summary>
        /// Gets the query root element for SDMX V21.
        /// </summary>
        /// <param name="operation">
        /// The operation.
        /// </param>
        /// <returns>
        /// The query root element qualified name; otherwise <see cref="XmlQualifiedName.Empty"/>
        /// </returns>
        public static XmlQualifiedName GetQueryRootElementV21(this SoapOperation operation)
        {
            ElementNameTable element;
            switch (operation)
            {
                case SoapOperation.GetGenericData:
                    element = ElementNameTable.GenericDataQuery;
                    break;
                case SoapOperation.GetGenericTimeSeriesData:
                    element = ElementNameTable.GenericTimeSeriesDataQuery;
                    break;
                case SoapOperation.GetStructureSpecificData:
                    element = ElementNameTable.StructureSpecificDataQuery;
                    break;
                case SoapOperation.GetStructureSpecificTimeSeriesData:
                    element = ElementNameTable.StructureSpecificTimeSeriesDataQuery;
                    break;
                case SoapOperation.GetGenericMetadata:
                    element = ElementNameTable.GenericMetadataQuery;
                    break;
                case SoapOperation.GetStructureSpecificMetadata:
                    element = ElementNameTable.StructureSpecificMetadataQuery;
                    break;
                case SoapOperation.GetStructures:
                    element = ElementNameTable.StructuresQuery;
                    break;
                case SoapOperation.GetDataflow:
                    element = ElementNameTable.DataflowQuery;
                    break;
                case SoapOperation.GetMetadataflow:
                    element = ElementNameTable.MetadataflowQuery;
                    break;
                case SoapOperation.GetDataStructure:
                    element = ElementNameTable.DataStructureQuery;
                    break;
                case SoapOperation.GetMetadataStructure:
                    element = ElementNameTable.MetadataStructureQuery;
                    break;
                case SoapOperation.GetCategoryScheme:
                    element = ElementNameTable.CategorySchemeQuery;
                    break;
                case SoapOperation.GetConceptScheme:
                    element = ElementNameTable.ConceptSchemeQuery;
                    break;
                case SoapOperation.GetCodelist:
                    element = ElementNameTable.CodelistQuery;
                    break;
                case SoapOperation.GetHierarchicalCodelist:
                    element = ElementNameTable.HierarchicalCodelistQuery;
                    break;
                case SoapOperation.GetOrganisationScheme:
                    element = ElementNameTable.OrganisationSchemeQuery;
                    break;
                case SoapOperation.GetReportingTaxonomy:
                    element = ElementNameTable.ReportingTaxonomyQuery;
                    break;
                case SoapOperation.GetStructureSet:
                    element = ElementNameTable.StructureSetQuery;
                    break;
                case SoapOperation.GetProcess:
                    element = ElementNameTable.ProcessQuery;
                    break;
                case SoapOperation.GetCategorisation:
                    element = ElementNameTable.CategorisationQuery;
                    break;
                case SoapOperation.GetProvisionAgreement:
                    element = ElementNameTable.ProvisionAgreementQuery;
                    break;
                case SoapOperation.GetConstraint:
                    element = ElementNameTable.ConstraintQuery;
                    break;
                case SoapOperation.GetDataSchema:
                    element = ElementNameTable.DataSchemaQuery;
                    break;
                case SoapOperation.GetMetadataSchema:
                    element = ElementNameTable.MetadataSchemaQuery;
                    break;
                default:
                    return XmlQualifiedName.Empty;
            }

            return new XmlQualifiedName(NameTableCache.GetElementName(element), SdmxConstants.MessageNs21);
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <param name="operation">
        /// The operation.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// operation;Not valid operation.
        /// </exception>
        /// <returns>
        /// The <see cref="SoapOperationResponse"/>.
        /// </returns>
        public static SoapOperationResponse GetResponse(this SoapOperation operation)
        {
            switch (operation)
            {
                case SoapOperation.Null:
                    return SoapOperationResponse.Null;
                case SoapOperation.QueryStructure:
                    return SoapOperationResponse.QueryStructureResponse;
                case SoapOperation.GetCompactData:
                    return SoapOperationResponse.GetCompactDataResponse;
                case SoapOperation.GetCrossSectionalData:
                    return SoapOperationResponse.GetCrossSectionalDataResponse;
                case SoapOperation.GetGenericData:
                    return SoapOperationResponse.GetGenericDataResponse;
                case SoapOperation.GetGenericTimeSeriesData:
                    return SoapOperationResponse.GetGenericTimeSeriesDataResponse;
                case SoapOperation.GetStructureSpecificData:
                    return SoapOperationResponse.GetStructureSpecificDataResponse;
                case SoapOperation.GetStructureSpecificTimeSeriesData:
                    return SoapOperationResponse.GetStructureSpecificTimeSeriesDataResponse;
                case SoapOperation.GetGenericMetadata:
                    return SoapOperationResponse.GetGenericMetadataResponse;
                case SoapOperation.GetStructureSpecificMetadata:
                    return SoapOperationResponse.GetStructureSpecificMetadataResponse;
                case SoapOperation.GetStructures:
                    return SoapOperationResponse.GetStructuresResponse;
                case SoapOperation.GetDataflow:
                    return SoapOperationResponse.GetDataflowResponse;
                case SoapOperation.GetMetadataflow:
                    return SoapOperationResponse.GetMetadataflowResponse;
                case SoapOperation.GetDataStructure:
                    return SoapOperationResponse.GetDataStructureResponse;
                case SoapOperation.GetMetadataStructure:
                    return SoapOperationResponse.GetMetadataStructureResponse;
                case SoapOperation.GetCategoryScheme:
                    return SoapOperationResponse.GetCategorySchemeResponse;
                case SoapOperation.GetConceptScheme:
                    return SoapOperationResponse.GetConceptSchemeResponse;
                case SoapOperation.GetCodelist:
                    return SoapOperationResponse.GetCodelistResponse;
                case SoapOperation.GetHierarchicalCodelist:
                    return SoapOperationResponse.GetHierarchicalCodelistResponse;
                case SoapOperation.GetOrganisationScheme:
                    return SoapOperationResponse.GetOrganisationSchemeResponse;
                case SoapOperation.GetReportingTaxonomy:
                    return SoapOperationResponse.GetReportingTaxonomyResponse;
                case SoapOperation.GetStructureSet:
                    return SoapOperationResponse.GetStructureSetResponse;
                case SoapOperation.GetProcess:
                    return SoapOperationResponse.GetProcessResponse;
                case SoapOperation.GetCategorisation:
                    return SoapOperationResponse.GetCategorisationResponse;
                case SoapOperation.GetProvisionAgreement:
                    return SoapOperationResponse.GetProvisionAgreementResponse;
                case SoapOperation.GetConstraint:
                    return SoapOperationResponse.GetConstraintResponse;
                case SoapOperation.GetDataSchema:
                    return SoapOperationResponse.GetDataSchemaResponse;
                case SoapOperation.GetMetadataSchema:
                    return SoapOperationResponse.GetMetadataSchemaResponse;
                default:
                    throw new ArgumentOutOfRangeException("operation", operation, "Not valid operation.");
            }
        }

        #endregion
    }
}