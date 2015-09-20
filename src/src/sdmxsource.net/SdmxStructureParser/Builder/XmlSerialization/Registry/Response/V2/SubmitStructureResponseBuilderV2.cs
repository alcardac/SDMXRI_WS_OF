// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitStructureResponseBuilderV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The submit structure response builder v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2;
    using Org.Sdmxsource.Util;

    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType;

    /// <summary>
    ///     The submit structure response builder v 2.
    /// </summary>
    public class SubmitStructureResponseBuilderV2 : AbstractResponseBuilder
    {
        #region Fields

        /// <summary>
        ///     The header xml beans builder.
        /// </summary>
        private readonly StructureHeaderXmlBuilder _headerXmlsBuilder = new StructureHeaderXmlBuilder();

        #endregion

        ////// PRIVATE CONSTRUCTOR 
        ///// TODO java 0.9.1 has private ctor that is not accessed anywhere 
        ////#region Constructors and Destructors

        /////// <summary>
        /////// Prevents a default instance of the <see cref="SubmitStructureResponseBuilderV2"/> class from being created.
        /////// </summary>
        ////private SubmitStructureResponseBuilderV2()
        ////{
        ////}

        ////#endregion
        #region Public Methods and Operators

        /// <summary>
        /// The build error response.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="errorBean">
        /// The error bean.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildErrorResponse(Exception exception, IStructureReference errorBean)
        {
            var responseType = new RegistryInterface();
            RegistryInterfaceType regInterface = responseType.Content;
            V2Helper.Header = regInterface;
            var returnType = new SubmitStructureResponseType();
            regInterface.SubmitStructureResponse = returnType;
            ProcessMaintainable(returnType, errorBean, exception);
            return responseType;
        }

        /// <summary>
        /// The build success response.
        /// </summary>
        /// <param name="beans">
        /// The beans.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildSuccessResponse(ISdmxObjects beans)
        {
            var responseType = new RegistryInterface();
            RegistryInterfaceType regInterface = responseType.Content;

            HeaderType headerType;
            if (beans.Header != null)
            {
                headerType = this._headerXmlsBuilder.Build(beans.Header);
                regInterface.Header = headerType;
            }
            else
            {
                headerType = new HeaderType();
                regInterface.Header = headerType;
                V2Helper.SetHeader(headerType, beans);
            }

            var returnType = new SubmitStructureResponseType();
            regInterface.SubmitStructureResponse = returnType;
            ProcessMaintainables(returnType, beans.GetAllMaintainables());
            return responseType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds new category scheme ref submission result.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <param name="th">
        /// The exception.
        /// </param>
        private static void AddNewCategorySchemeRefSubmissionResult(
            SubmitStructureResponseType returnType, IStructureReference structureReference, Exception th)
        {
            SubmissionResultType result = GetNewSubmissionResultType(returnType, th);
            var submittedStructure = new SubmittedStructureType();
            result.SubmittedStructure = submittedStructure;
            var refType = new CategorySchemeRefType();
            submittedStructure.CategorySchemeRef = refType;

            IMaintainableRefObject maintainableReference = structureReference.MaintainableReference;
            refType.AgencyID = maintainableReference.AgencyId;
            refType.CategorySchemeID = maintainableReference.MaintainableId;
            if (ObjectUtil.ValidString(structureReference.MaintainableUrn))
            {
                refType.URN = structureReference.MaintainableUrn;
            }

            string value = maintainableReference.Version;
            if (!string.IsNullOrWhiteSpace(value))
            {
                refType.Version = maintainableReference.Version;
            }
        }

        /// <summary>
        /// Adds new codelist ref submission result.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="structureReference">
        /// The structure reference
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        private static void AddNewCodelistRefSubmissionResult(
            SubmitStructureResponseType returnType, IStructureReference structureReference, Exception exception)
        {
            SubmissionResultType result = GetNewSubmissionResultType(returnType, exception);
            var submittedStructure = new SubmittedStructureType();
            result.SubmittedStructure = submittedStructure;
            var refType = new CodelistRefType();
            submittedStructure.CodelistRef = refType;

            IMaintainableRefObject maintainableReference = structureReference.MaintainableReference;
            refType.AgencyID = maintainableReference.AgencyId;
            refType.CodelistID = maintainableReference.MaintainableId;
            if (ObjectUtil.ValidString(structureReference.MaintainableUrn))
            {
                refType.URN = structureReference.MaintainableUrn;
            }

            if (!string.IsNullOrWhiteSpace(maintainableReference.Version))
            {
                refType.Version = maintainableReference.Version;
            }
        }

        /// <summary>
        /// Adds new concept scheme ref submission result.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        private static void AddNewConceptSchemeRefSubmissionResult(
            SubmitStructureResponseType returnType, IStructureReference structureReference, Exception exception)
        {
            SubmissionResultType result = GetNewSubmissionResultType(returnType, exception);
            var submittedStructure = new SubmittedStructureType();
            result.SubmittedStructure = submittedStructure;
            var refType = new ConceptSchemeRefType();
            submittedStructure.ConceptSchemeRef = refType;

            IMaintainableRefObject maintainableReference = structureReference.MaintainableReference;
            refType.AgencyID = maintainableReference.AgencyId;
            refType.ConceptSchemeID = maintainableReference.MaintainableId;
            if (ObjectUtil.ValidString(structureReference.MaintainableUrn))
            {
                refType.URN = structureReference.MaintainableUrn;
            }

            string value = maintainableReference.Version;
            if (!string.IsNullOrWhiteSpace(value))
            {
                refType.Version = maintainableReference.Version;
            }
        }

        /// <summary>
        /// Adds new data structure ref submission result.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="structureReference">
        /// The structure reference
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        private static void AddNewDataStructureRefSubmissionResult(
            SubmitStructureResponseType returnType, IStructureReference structureReference, Exception exception)
        {
            SubmissionResultType result = GetNewSubmissionResultType(returnType, exception);
            var submittedStructure = new SubmittedStructureType();
            result.SubmittedStructure = submittedStructure;
            var refType = new KeyFamilyRefType();
            submittedStructure.KeyFamilyRef = refType;

            IMaintainableRefObject maintainableReference = structureReference.MaintainableReference;
            refType.AgencyID = maintainableReference.AgencyId;
            refType.KeyFamilyID = maintainableReference.MaintainableId;
            if (ObjectUtil.ValidString(structureReference.MaintainableUrn))
            {
                refType.URN = structureReference.MaintainableUrn;
            }

            string value = maintainableReference.Version;
            if (!string.IsNullOrWhiteSpace(value))
            {
                refType.Version = maintainableReference.Version;
            }
        }

        /// <summary>
        /// Adds new dataflow ref submission result.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <param name="th">
        /// The exception.
        /// </param>
        private static void AddNewDataflowRefSubmissionResult(
            SubmitStructureResponseType returnType, IStructureReference structureReference, Exception th)
        {
            SubmissionResultType result = GetNewSubmissionResultType(returnType, th);
            var submittedStructure = new SubmittedStructureType();
            result.SubmittedStructure = submittedStructure;
            var refType = new DataflowRefType();
            submittedStructure.DataflowRef = refType;

            IMaintainableRefObject maintainableReference = structureReference.MaintainableReference;
            refType.AgencyID = maintainableReference.AgencyId;
            refType.DataflowID = maintainableReference.MaintainableId;
            if (ObjectUtil.ValidString(structureReference.MaintainableUrn))
            {
                refType.URN = structureReference.MaintainableUrn;
            }

            string value = maintainableReference.Version;
            if (!string.IsNullOrWhiteSpace(value))
            {
                refType.Version = maintainableReference.Version;
            }
        }

        /// <summary>
        /// Adds new hierarchical codelist ref submission result.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", 
            Justification = "Reviewed. Suppression is OK here.")]
        private static void AddNewHierarchicalCodelistRefSubmissionResult(
            SubmitStructureResponseType returnType, IStructureReference structureReference, Exception exception)
        {
            SubmissionResultType result = GetNewSubmissionResultType(returnType, exception);
            var submittedStructure = new SubmittedStructureType();
            result.SubmittedStructure = submittedStructure;
            var refType = new HierarchicalCodelistRefType();
            submittedStructure.HierarchicalCodelistRef = refType;

            IMaintainableRefObject mRef = structureReference.MaintainableReference;
            refType.AgencyID = mRef.AgencyId;
            refType.HierarchicalCodelistID = mRef.MaintainableId;
            if (ObjectUtil.ValidString(structureReference.MaintainableUrn))
            {
                refType.URN = structureReference.MaintainableUrn;
            }

            string value = mRef.Version;
            if (!string.IsNullOrWhiteSpace(value))
            {
                refType.Version = mRef.Version;
            }
        }

        /// <summary>
        /// Adds new metadata structure ref submission result.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        private static void AddNewMetadataStructureRefSubmissionResult(
            SubmitStructureResponseType returnType, IStructureReference structureReference, Exception exception)
        {
            SubmissionResultType result = GetNewSubmissionResultType(returnType, exception);
            var submittedStructure = new SubmittedStructureType();
            result.SubmittedStructure = submittedStructure;
            var refType = new MetadataStructureRefType();
            submittedStructure.MetadataStructureRef = refType;

            IMaintainableRefObject maintainableReference = structureReference.MaintainableReference;
            refType.AgencyID = maintainableReference.AgencyId;
            refType.MetadataStructureID = maintainableReference.MaintainableId;
            if (ObjectUtil.ValidString(structureReference.MaintainableUrn))
            {
                refType.URN = structureReference.MaintainableUrn;
            }

            string value = maintainableReference.Version;
            if (!string.IsNullOrWhiteSpace(value))
            {
                refType.Version = maintainableReference.Version;
            }
        }

        /// <summary>
        /// Adds new metadataflow ref submission result.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        private static void AddNewMetadataflowRefSubmissionResult(
            SubmitStructureResponseType returnType, IStructureReference structureReference, Exception exception)
        {
            SubmissionResultType result = GetNewSubmissionResultType(returnType, exception);
            var submittedStructure = new SubmittedStructureType();
            result.SubmittedStructure = submittedStructure;
            var refType = new MetadataflowRefType();
            submittedStructure.MetadataflowRef = refType;

            IMaintainableRefObject maintainableReference = structureReference.MaintainableReference;
            refType.AgencyID = maintainableReference.AgencyId;
            refType.MetadataflowID = maintainableReference.MaintainableId;
            if (ObjectUtil.ValidString(structureReference.MaintainableUrn))
            {
                refType.URN = structureReference.MaintainableUrn;
            }

            string value = maintainableReference.Version;
            if (!string.IsNullOrWhiteSpace(value))
            {
                refType.Version = maintainableReference.Version;
            }
        }

        /// <summary>
        /// Adds new organisation scheme ref submission result.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        private static void AddNewOrganisationSchemeRefSubmissionResult(
            SubmitStructureResponseType returnType, IStructureReference structureReference, Exception exception)
        {
            SubmissionResultType result = GetNewSubmissionResultType(returnType, exception);
            var submittedStructure = new SubmittedStructureType();
            result.SubmittedStructure = submittedStructure;
            var refType = new OrganisationSchemeRefType();
            submittedStructure.OrganisationSchemeRef = refType;
            if (structureReference == null)
            {
                refType.AgencyID = "NotApplicable";
            }
            else
            {
                IMaintainableRefObject maintainableReference = structureReference.MaintainableReference;
                refType.AgencyID = maintainableReference.AgencyId;
                refType.OrganisationSchemeID = maintainableReference.MaintainableId;
                if (ObjectUtil.ValidString(structureReference.MaintainableUrn))
                {
                    refType.URN = structureReference.MaintainableUrn;
                }

                string value = maintainableReference.Version;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    refType.Version = maintainableReference.Version;
                }
            }
        }

        /// <summary>
        /// Adds new process ref submission result.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <param name="th">
        /// The exception.
        /// </param>
        private static void AddNewProcessRefSubmissionResult(
            SubmitStructureResponseType returnType, IStructureReference structureReference, Exception th)
        {
            SubmissionResultType result = GetNewSubmissionResultType(returnType, th);
            var submittedStructure = new SubmittedStructureType();
            result.SubmittedStructure = submittedStructure;
            var refType = new ProcessRefType();
            submittedStructure.ProcessRef = refType;

            IMaintainableRefObject maintainableReference = structureReference.MaintainableReference;
            refType.AgencyID = maintainableReference.AgencyId;
            refType.ProcessID = maintainableReference.MaintainableId;
            if (ObjectUtil.ValidString(structureReference.MaintainableUrn))
            {
                refType.URN = structureReference.MaintainableUrn;
            }

            string value = maintainableReference.Version;
            if (!string.IsNullOrWhiteSpace(value))
            {
                refType.Version = maintainableReference.Version;
            }
        }

        /// <summary>
        /// Adds new reporting taxonomy set ref submission result.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <param name="th">
        /// The exception.
        /// </param>
        private static void AddNewReportingTaxonomySetRefSubmissionResult(
            SubmitStructureResponseType returnType, IStructureReference structureReference, Exception th)
        {
            SubmissionResultType result = GetNewSubmissionResultType(returnType, th);
            var submittedStructure = new SubmittedStructureType();
            result.SubmittedStructure = submittedStructure;
            var refType = new ReportingTaxonomyRefType();
            submittedStructure.ReportingTaxonomyRef = refType;

            IMaintainableRefObject maintainableReference = structureReference.MaintainableReference;
            refType.AgencyID = maintainableReference.AgencyId;
            refType.ReportingTaxonomyID = maintainableReference.MaintainableId;
            if (ObjectUtil.ValidString(structureReference.MaintainableUrn))
            {
                refType.URN = structureReference.MaintainableUrn;
            }

            string value = maintainableReference.Version;
            if (!string.IsNullOrWhiteSpace(value))
            {
                refType.Version = maintainableReference.Version;
            }
        }

        /// <summary>
        /// Adds new structure set ref submission result.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        private static void AddNewStructureSetRefSubmissionResult(
            SubmitStructureResponseType returnType, IStructureReference structureReference, Exception exception)
        {
            SubmissionResultType result = GetNewSubmissionResultType(returnType, exception);
            var submittedStructure = new SubmittedStructureType();
            result.SubmittedStructure = submittedStructure;
            var refType = new StructureSetRefType();
            submittedStructure.StructureSetRef = refType;

            IMaintainableRefObject maintainableReference = structureReference.MaintainableReference;
            refType.AgencyID = maintainableReference.AgencyId;
            refType.StructureSetID = maintainableReference.MaintainableId;
            if (ObjectUtil.ValidString(structureReference.MaintainableUrn))
            {
                refType.URN = structureReference.MaintainableUrn;
            }

            string value = maintainableReference.Version;
            if (!string.IsNullOrWhiteSpace(value))
            {
                refType.Version = maintainableReference.Version;
            }
        }

        /// <summary>
        /// Gets new submission result type.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <returns>
        /// The <see cref="SubmissionResultType"/>.
        /// </returns>
        private static SubmissionResultType GetNewSubmissionResultType(
            SubmitStructureResponseType returnType, Exception exception)
        {
            var submissionResult = new SubmissionResultType();
            returnType.SubmissionResult.Add(submissionResult);
            var statusMessage = new StatusMessageType();
            submissionResult.StatusMessage = statusMessage;
            if (exception == null)
            {
                statusMessage.status = StatusTypeConstants.Success;
            }
            else
            {
                statusMessage.status = StatusTypeConstants.Failure;
                var tt = new TextType();
                statusMessage.MessageText.Add(tt);
                var uncheckedException = exception as SdmxException;
                tt.TypedValue = uncheckedException != null ? uncheckedException.FullMessage : exception.Message;
            }

            return submissionResult;
        }

        /// <summary>
        /// Process maintainable.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <param name="th">
        /// The exception.
        /// </param>
        private static void ProcessMaintainable(
            SubmitStructureResponseType returnType, IStructureReference structureReference, Exception th)
        {
            if (structureReference == null)
            {
                AddNewOrganisationSchemeRefSubmissionResult(returnType, null, th);
                return; // TODO MISSING from Java
            }

            switch (structureReference.TargetReference.EnumType)
            {
                case SdmxStructureEnumType.AgencyScheme:
                    AddNewOrganisationSchemeRefSubmissionResult(returnType, structureReference, th);
                    break;
                case SdmxStructureEnumType.DataProviderScheme:
                    AddNewOrganisationSchemeRefSubmissionResult(returnType, structureReference, th);
                    break;
                case SdmxStructureEnumType.DataConsumerScheme:
                    AddNewOrganisationSchemeRefSubmissionResult(returnType, structureReference, th);
                    break;
                case SdmxStructureEnumType.CategoryScheme:
                    AddNewCategorySchemeRefSubmissionResult(returnType, structureReference, th);
                    break;
                case SdmxStructureEnumType.Dataflow:
                    AddNewDataflowRefSubmissionResult(returnType, structureReference, th);
                    break;
                case SdmxStructureEnumType.MetadataFlow:
                    AddNewMetadataflowRefSubmissionResult(returnType, structureReference, th);
                    break;
                case SdmxStructureEnumType.CodeList:
                    AddNewCodelistRefSubmissionResult(returnType, structureReference, th);
                    break;
                case SdmxStructureEnumType.HierarchicalCodelist:
                    AddNewHierarchicalCodelistRefSubmissionResult(returnType, structureReference, th);
                    break;
                case SdmxStructureEnumType.ConceptScheme:
                    AddNewConceptSchemeRefSubmissionResult(returnType, structureReference, th);
                    break;
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    AddNewOrganisationSchemeRefSubmissionResult(returnType, structureReference, th);
                    break;
                case SdmxStructureEnumType.Dsd:
                    AddNewDataStructureRefSubmissionResult(returnType, structureReference, th);
                    break;
                case SdmxStructureEnumType.Msd:
                    AddNewMetadataStructureRefSubmissionResult(returnType, structureReference, th);
                    break;
                case SdmxStructureEnumType.Process:
                    AddNewProcessRefSubmissionResult(returnType, structureReference, th);
                    break;
                case SdmxStructureEnumType.StructureSet:
                    AddNewStructureSetRefSubmissionResult(returnType, structureReference, th);
                    break;
                case SdmxStructureEnumType.ReportingTaxonomy:
                    AddNewReportingTaxonomySetRefSubmissionResult(returnType, structureReference, th);
                    break;
            }
        }

        /// <summary>
        /// Process the <paramref name="maintainableObjects"/> to produce <paramref name="returnType"/>
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="maintainableObjects">
        /// The maintainable Objects.
        /// </param>
        private static void ProcessMaintainables(
            SubmitStructureResponseType returnType, IEnumerable<IMaintainableObject> maintainableObjects)
        {
            /* foreach */
            foreach (IMaintainableObject maint in maintainableObjects)
            {
                ProcessMaintainable(returnType, maint.AsReference, null);
            }
        }

        #endregion
    }
}