// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryBuilderV21.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The query bean builder v 21.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.Query
{
    using System;
    using System.Collections.Generic;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference.Complex;
    using TimeRangeCore = Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference.Complex.TimeRangeCore;
    using System.Xml.Linq;

    /// <summary>
    ///     The query bean builder v 21.
    /// </summary>
    public class QueryBuilderV21
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds a list of provision references from a version 2.1 registry query registration request message
        /// </summary>
        /// <param name="queryRegistrationRequestType">
        /// The query Registration Request Type.
        /// </param>
        /// <returns>
        /// provision references
        /// </returns>
        public IStructureReference Build(QueryRegistrationRequestType queryRegistrationRequestType)
        {
            if (queryRegistrationRequestType != null)
            {
                if (queryRegistrationRequestType.All != null)
                {
                    return
                        new StructureReferenceImpl(
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProvisionAgreement));
                }

                DataflowReferenceType dataflowRef = queryRegistrationRequestType.Dataflow;
                DataProviderReferenceType dataProviderRef = queryRegistrationRequestType.DataProvider;
                ProvisionAgreementReferenceType provRef = queryRegistrationRequestType.ProvisionAgreement;
                MetadataflowReferenceType mdfRef = queryRegistrationRequestType.Metadataflow;

                if (dataProviderRef != null)
                {
                    return RefUtil.CreateReference(dataProviderRef);
                }

                if (provRef != null)
                {
                    return RefUtil.CreateReference(provRef);
                }

                if (dataflowRef != null)
                {
                    return RefUtil.CreateReference(dataflowRef);
                }

                if (mdfRef != null)
                {
                    return RefUtil.CreateReference(mdfRef);
                }
            }

            return null;
        }

        /**
	     * builds an identifiable reference from XML bean values.
	     * @param itemWhereType 
	     * @param itemType the type of the item
	     * @return null if the itemWhereType is null
	     */
        private IComplexIdentifiableReferenceObject BuildIdentifiableReference(ItemWhereType itemWhereType, SdmxStructureType itemType)
        {
            if (itemWhereType == null)
            {
                return null;
            }

            QueryIDType idType = itemWhereType.ID;
            IComplexTextReference id = null;
            if (idType != null)
            {
                id = BuildTextReference(null, idType.@operator.ToString(), idType.ToString());
            }

            IComplexAnnotationReference annotationRef = BuildAnnotationReference(itemWhereType.Annotation);

            QueryTextType nameType = itemWhereType.Name;
            IComplexTextReference nameRef = null;
            if (nameType != null)
            {
                nameRef = BuildTextReference(nameType.lang, nameType.@operator.ToString(), nameType.ToString());
            }

            QueryTextType descriptionType = itemWhereType.Description;
            IComplexTextReference descriptionRef = null;
            if (descriptionType != null)
            {
                descriptionRef = BuildTextReference(descriptionType.lang, descriptionType.@operator.ToString(), descriptionType.ToString());
            }

            IComplexIdentifiableReferenceObject childRef = null;
            IList<ItemWhere> itemWhereList = itemWhereType.ItemWhere;
            if (itemWhereList != null && (itemWhereList.Count > 0))
            { // this should be the case only for the Categories, ReportingTaxonomies.
                if (itemWhereList.Count > 1)
                {
                    //TODO warning or error that is not supported?????
                }
                childRef = BuildIdentifiableReference(itemWhereList[0].Content, itemType);
            }

            return new ComplexIdentifiableReferenceCore(id, itemType, annotationRef, nameRef, descriptionRef, childRef);
        }

        /**
         * builds a complex structure reference from XML beans values.
         * @param maintainableWhere
         * @param childRef if items in the where clauses (for ItemSchemes) otherwise null
         * @return
         */
        private IComplexStructureReferenceObject BuildMaintainableWhere(MaintainableWhereType maintainableWhere, IComplexIdentifiableReferenceObject childRef)
        {
            QueryNestedIDType agencyIDType = maintainableWhere.AgencyID;
            IComplexTextReference agencyId = null;
            if (agencyIDType != null)
            {
                agencyId = BuildTextReference(null, agencyIDType.@operator.ToString(), agencyIDType.TypedValue);
            }

            QueryIDType queryIDType = maintainableWhere.ID;
            IComplexTextReference id = null;
            if (queryIDType != null)
            {
                id = BuildTextReference(null, queryIDType.@operator.ToString(), queryIDType.TypedValue.ToString());
            }

            IComplexVersionReference versionRef = BuildVersionReference(maintainableWhere.Version != null ? maintainableWhere.Version.ToString() : null, maintainableWhere.VersionFrom, maintainableWhere.VersionTo);

            SdmxStructureType structureType;
            if (maintainableWhere.type.Equals("OrganisationScheme"))
            { // hack. This checked are done since it is not identifiable no element name in enum and cannot use parseClass()
                structureType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationScheme);
            }
            else if (maintainableWhere.type.Equals("Constraint"))
            {
                structureType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ContentConstraint);
            }
            else if (maintainableWhere.type.Equals("Any"))
            {
                structureType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Any);
            }
            else
            {
                structureType = SdmxStructureType.ParseClass(maintainableWhere.type);
            }

            IComplexAnnotationReference annotationRef = BuildAnnotationReference(maintainableWhere.Annotation);

            QueryTextType nameType = maintainableWhere.Name;
            IComplexTextReference nameRef = null;
            if (nameType != null)
            {
                nameRef = BuildTextReference(nameType.lang, nameType.@operator.ToString(), nameType.TypedValue.ToString());
            }


            QueryTextType descriptionType = maintainableWhere.Description;
            IComplexTextReference descriptionRef = null;
            if (descriptionType != null)
            {
                descriptionRef = BuildTextReference(descriptionType.lang, descriptionType.@operator.ToString(), descriptionType.TypedValue.ToString());
            }


            return new ComplexStructureReferenceCore(agencyId, id, versionRef, structureType, annotationRef, nameRef, descriptionRef, childRef);
        }

        /**
         * Builds annotation reference from XML beans values.
         * @param annotationWhereType
         * @return null if annotationWhereType is null
         */
        private IComplexAnnotationReference BuildAnnotationReference(AnnotationWhereType annotationWhereType)
        {
            if (annotationWhereType == null)
            {
                return null;
            }

            QueryStringType type = annotationWhereType.Type;
            IComplexTextReference typeRef = null;
            if (type != null)
            {
                typeRef = BuildTextReference(null, type.@operator.ToString(), type.ToString());
            }

            QueryStringType title = annotationWhereType.Title;
            IComplexTextReference titleRef = null;
            if (title != null)
            {
                titleRef = BuildTextReference(null, title.@operator.ToString(), title.ToString());
            }

            QueryTextType text = annotationWhereType.Text;
            IComplexTextReference textRef = null;
            if (text != null)
            {
                textRef = BuildTextReference(text.lang, text.@operator.ToString(), text.ToString());
            }

            return new ComplexAnnotationReferenceCore(typeRef, titleRef, textRef);
        }


        /**
         * builds the version reference from the XML beans values
         * @param version
         * @param versionFromType
         * @param versionToType
         * @return
         */
        private IComplexVersionReference BuildVersionReference(string version, TimeRangeValueType versionFromType, TimeRangeValueType versionToType)
        {
            TertiaryBool returnLatest = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            string emptyCheckedString = null;
            if (!string.IsNullOrWhiteSpace(version))
            {
                if (version.Equals("*"))
                {
                    returnLatest = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.True);
                    emptyCheckedString = null;
                }
                else
                {
                    emptyCheckedString = version;
                    returnLatest = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.False);
                }
            }

            ITimeRange validFrom = BuildTimeRange(versionFromType);
            ITimeRange validTo = BuildTimeRange(versionToType);

            return new ComplexVersionReferenceCore(returnLatest, emptyCheckedString, validFrom, validTo);
        }

        /**
         * Builds a time range from time range value from XML
         * @param timeRangeValueType
         * @return
         */
        private ITimeRange BuildTimeRange(TimeRangeValueType timeRangeValueType)
        {
            if (timeRangeValueType == null)
            {
                return null;
            }

            bool range = false;
            ISdmxDate startDate = null;
            ISdmxDate endDate = null;
            bool endInclusive = false;
            bool startInclusive = false;

            if (timeRangeValueType.AfterPeriod != null)
            {
                TimePeriodRangeType afterPeriod = timeRangeValueType.AfterPeriod;
                startDate = new SdmxDateCore(afterPeriod.TypedValue.ToString());
                startInclusive = afterPeriod.isInclusive;
            }
            else if (timeRangeValueType.BeforePeriod != null)
            {
                TimePeriodRangeType beforePeriod = timeRangeValueType.BeforePeriod;
                endDate = new SdmxDateCore(beforePeriod.TypedValue.ToString());
                endInclusive = beforePeriod.isInclusive;
            }
            else
            { //case that range is set
                range = true;
                TimePeriodRangeType startPeriod = timeRangeValueType.StartPeriod;
                startDate = new SdmxDateCore(startPeriod.TypedValue.ToString());
                startInclusive = startPeriod.isInclusive;

                TimePeriodRangeType endPeriod = timeRangeValueType.EndPeriod;
                endDate = new SdmxDateCore(endPeriod.TypedValue.ToString());
                endInclusive = endPeriod.isInclusive;
            }


            return new TimeRangeCore(range, startDate, endDate, startInclusive, endInclusive);
        }


        /**
         * build the text reference from values acquired from XML beans.
         * Check if empty or unset to check default values according to XSD.
         * @param lang
         * @param operator
         * @param value
         * @return the complex text reference for the given values.
         */
        private IComplexTextReference BuildTextReference(string lang, string sOperator, string value)
        {
            string emptyCheckedLang = "en";
            if (lang != null && lang.Trim().Length > 1)
            {
                emptyCheckedLang = lang;
            }

            TextSearch defaultCheckedOperator = TextSearch.GetFromEnum(TextSearchEnumType.Equal);
            if (sOperator != null && sOperator.Trim().Length > 1)
            {
                defaultCheckedOperator = TextSearch.ParseString(sOperator);
            }

            return new ComplexTextReferenceCore(emptyCheckedLang, defaultCheckedOperator, value);
        }


        /**
         * Builds {@link IComplexStructureQueryMetadata} from the return detail part of the query message
         * @param returnDetails
         * @return
         */
        private IComplexStructureQueryMetadata BuildQueryDetails(StructureReturnDetailsType returnDetails)
        {
            bool returnMatchedartefact = returnDetails.returnMatchedArtefact;

            ComplexStructureQueryDetail queryDetail = ComplexStructureQueryDetail.ParseString(returnDetails.detail);

            ReferencesType references = returnDetails.References;

            ComplexMaintainableQueryDetail referencesQueryDetail;
            if (references.detail != null)
            {
                referencesQueryDetail = ComplexMaintainableQueryDetail.ParseString(references.detail);
            }
            else
            {
                referencesQueryDetail = ComplexMaintainableQueryDetail.GetFromEnum(ComplexMaintainableQueryDetailEnumType.Full);
            }


            StructureReferenceDetail referenceDetail = StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.Null);
            List<SdmxStructureType> referenceSpecificStructures = null;

            if (references.All != null)
            {
                referenceDetail = StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.All);
            }
            else if (references.Children != null)
            {
                referenceDetail = StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.Children);
            }
            else if (references.Descendants != null)
            {
                referenceDetail = StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.Descendants);
            }
            else if (references.None != null)
            {
                referenceDetail = StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.None);
            }
            else if (references.Parents != null)
            {
                referenceDetail = StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.Parents);
            }
            else if (references.ParentsAndSiblings != null)
            {
                referenceDetail = StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.ParentsSiblings);
            }
            else if (references.SpecificObjects != null)
            {
                referenceDetail = StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.Specific);

                referenceSpecificStructures = new List<SdmxStructureType>();

                MaintainableObjectTypeListType specificObjects = references.SpecificObjects;

                //domNode = specificObjects.getDomNode();
                IEnumerable<XNode> childNodes = specificObjects.Untyped.DescendantNodes();
                foreach (XElement item in childNodes)
                {
                    var tagName = item.Name.LocalName;
                    if (!string.IsNullOrWhiteSpace(tagName))
                    {
                        referenceSpecificStructures.Add(SdmxStructureType.ParseClass(tagName));
                    }
                }
            }

            return new ComplexStructureQueryMetadataCore(returnMatchedartefact, queryDetail, referencesQueryDetail, referenceDetail, referenceSpecificStructures);
        }


        public IComplexStructureQuery Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.DataflowQueryType dataflowQueryMsg)
        {
            DataflowQueryType query = dataflowQueryMsg.SdmxQuery;

            // process details
            StructureReturnDetailsType returnDetails = query.ReturnDetails;
            IComplexStructureQueryMetadata queryMetadata = BuildQueryDetails(returnDetails);

            // process common clauses for all maintainables along with specific from child references.
            IComplexStructureReferenceObject structureRef = BuildMaintainableWhere(query.StructuralMetadataWhere.Content, null);
            return new ComplexStructureQueryCore(structureRef, queryMetadata);
        }

        public IComplexStructureQuery Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.MetadataflowQueryType metadataflowQueryMsg)
        {
            MetadataflowQueryType query = metadataflowQueryMsg.SdmxQuery;

            // process details
            StructureReturnDetailsType returnDetails = query.ReturnDetails;
            IComplexStructureQueryMetadata queryMetadata = BuildQueryDetails(returnDetails);

            // process common clauses for all maintainables along with specific from child references.
            IComplexStructureReferenceObject structureRef = BuildMaintainableWhere(query.StructuralMetadataWhere.Content, null);
            return new ComplexStructureQueryCore(structureRef, queryMetadata);
        }

        public IComplexStructureQuery Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.DataStructureQueryType dataStructureQueryMsg)
        {
            DataStructureQueryType query = dataStructureQueryMsg.SdmxQuery;

            // process details
            StructureReturnDetailsType returnDetails = query.ReturnDetails;
            IComplexStructureQueryMetadata queryMetadata = BuildQueryDetails(returnDetails);

            // process common clauses for all maintainables along with specific from child references.
            IComplexStructureReferenceObject structureRef = BuildMaintainableWhere(query.StructuralMetadataWhere.Content, null);
            return new ComplexStructureQueryCore(structureRef, queryMetadata);
        }

        public IComplexStructureQuery Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.MetadataStructureQueryType metadataStructureQueryMsg)
        {
            MetadataStructureQueryType query = metadataStructureQueryMsg.SdmxQuery;

            // process details
            StructureReturnDetailsType returnDetails = query.ReturnDetails;
            IComplexStructureQueryMetadata queryMetadata = BuildQueryDetails(returnDetails);

            // process common clauses for all maintainables along with specific from child references.
            IComplexStructureReferenceObject structureRef = BuildMaintainableWhere(query.StructuralMetadataWhere.Content, null);
            return new ComplexStructureQueryCore(structureRef, queryMetadata);
        }

        public IComplexStructureQuery Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.CategorySchemeQueryType categorySchemeQueryMsg)
        {
            CategorySchemeQueryType query = categorySchemeQueryMsg.SdmxQuery;

            // process details
            StructureReturnDetailsType returnDetails = query.ReturnDetails;
            IComplexStructureQueryMetadata queryMetadata = BuildQueryDetails(returnDetails);

            // process codelist specific clauses.
            CategorySchemeWhereType categorySchemeWhere = (CategorySchemeWhereType)query.StructuralMetadataWhere.Content;//.GetCategorySchemeWhere();
            IList<ItemWhere> itemWhereList = categorySchemeWhere.ItemWhere;
            IComplexIdentifiableReferenceObject childRef = null;
            if (itemWhereList != null && itemWhereList.Count > 0)
            {
                if (itemWhereList.Count > 1)
                {
                    //TODO warning or error that is not supported?????
                }
                childRef = BuildIdentifiableReference(itemWhereList[0].Content, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Category));
            }


            // process common clauses for all maintainables along with specific from child references.
            IComplexStructureReferenceObject structureRef = BuildMaintainableWhere(query.StructuralMetadataWhere.Content, childRef);
            return new ComplexStructureQueryCore(structureRef, queryMetadata);
        }

        public IComplexStructureQuery Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.ConceptSchemeQueryType conceptSchemeQueryMsg)
        {
            ConceptSchemeQueryType query = conceptSchemeQueryMsg.SdmxQuery;

            // process details
            StructureReturnDetailsType returnDetails = query.ReturnDetails;
            IComplexStructureQueryMetadata queryMetadata = BuildQueryDetails(returnDetails);

            // process codelist specific clauses.
            ConceptSchemeWhereType conceptSchemeWhere = (ConceptSchemeWhereType)query.StructuralMetadataWhere.Content;//.getConceptSchemeWhere();
            IList<ItemWhere> itemWhereList = conceptSchemeWhere.ItemWhere;
            IComplexIdentifiableReferenceObject childRef = null;
            if (itemWhereList != null && itemWhereList.Count > 0)
            {
                if (itemWhereList.Count > 1)
                {
                    //TODO warning or error that is not supported?????
                }
                childRef = BuildIdentifiableReference(itemWhereList[0].Content, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Concept));
            }


            // process common clauses for all maintainables along with specific from child references.
            IComplexStructureReferenceObject structureRef = BuildMaintainableWhere(query.StructuralMetadataWhere.Content, childRef);
            return new ComplexStructureQueryCore(structureRef, queryMetadata);
        }

        /**
         * Builds a {@link IComplexStructureQuery} from a Codelist query 2.1 message
         * @param codelistQueryMsg
         * @return
         */
        public IComplexStructureQuery Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.CodelistQueryType codelistQueryMsg)
        {
            CodelistQueryType query = codelistQueryMsg.SdmxQuery;

            // process details
            StructureReturnDetailsType returnDetails = query.ReturnDetails;
            IComplexStructureQueryMetadata queryMetadata = BuildQueryDetails(returnDetails);

            // process codelist specific clauses.
            CodelistWhereType codelistWhere = (CodelistWhereType)query.StructuralMetadataWhere.Content;//.getCodelistWhere();
            IList<ItemWhere> itemWhereList = codelistWhere.ItemWhere;
            IComplexIdentifiableReferenceObject childRef = null;
            if (itemWhereList != null && itemWhereList.Count > 0)
            {
                if (itemWhereList.Count > 1)
                {
                    //TODO warning or error that is not supported?????
                }
                childRef = BuildIdentifiableReference(itemWhereList[0].Content, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Code));
            }

            // process common clauses for all maintainables along with specific from child references.
            IComplexStructureReferenceObject structureRef = BuildMaintainableWhere(query.StructuralMetadataWhere.Content, childRef);
            return new ComplexStructureQueryCore(structureRef, queryMetadata);
        }


        public IComplexStructureQuery Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.HierarchicalCodelistQueryType hierarchicalCodelistQueryMsg)
        {
            HierarchicalCodelistQueryType query = hierarchicalCodelistQueryMsg.SdmxQuery;

            // process details
            StructureReturnDetailsType returnDetails = query.ReturnDetails;
            IComplexStructureQueryMetadata queryMetadata = BuildQueryDetails(returnDetails);

            // process common clauses for all maintainables along with specific from child references.
            IComplexStructureReferenceObject structureRef = BuildMaintainableWhere(query.StructuralMetadataWhere.Content, null);
            return new ComplexStructureQueryCore(structureRef, queryMetadata);
        }


        public IComplexStructureQuery Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.OrganisationSchemeQueryType organisationSchemeQueryMsg)
        {
            OrganisationSchemeQueryType query = organisationSchemeQueryMsg.SdmxQuery;

            // process details
            StructureReturnDetailsType returnDetails = query.ReturnDetails;
            IComplexStructureQueryMetadata queryMetadata = BuildQueryDetails(returnDetails);

            // process OrganisationScheme specific clauses.
            OrganisationSchemeWhereType organisationSchemeWhere = (OrganisationSchemeWhereType)query.StructuralMetadataWhere.Content;//.getOrganisationSchemeWhere();
            string orgSchemeStr = organisationSchemeWhere.type;
            SdmxStructureEnumType orgSchemeType;
            if (orgSchemeStr.Equals("OrganisationScheme"))
            {
                orgSchemeType = SdmxStructureEnumType.OrganisationScheme;
            }
            else
            {
                orgSchemeType = SdmxStructureType.ParseClass(orgSchemeStr).EnumType;
            }

            SdmxStructureType orgType;
            switch (orgSchemeType)
            {
                case SdmxStructureEnumType.AgencyScheme:
                    orgType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Agency);
                    break;
                case SdmxStructureEnumType.DataConsumerScheme:
                    orgType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumer);
                    break;
                case SdmxStructureEnumType.DataProviderScheme:
                    orgType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProvider);
                    break;
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    orgType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnit);
                    break;
                case SdmxStructureEnumType.OrganisationScheme:
                    orgType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Organisation);
                    break;
                default:
                    throw new ArgumentException("An organisation scheme type expected instead of: " + orgSchemeType);
            }

            IList<ItemWhere> itemWhereList = organisationSchemeWhere.ItemWhere;
            IComplexIdentifiableReferenceObject childRef = null;
            if (itemWhereList != null && itemWhereList.Count > 0)
            {
                if (itemWhereList.Count > 1)
                {
                    //TODO warning or error that is not supported?????
                }
                childRef = BuildIdentifiableReference(itemWhereList[0].Content, orgType);
            }

            // process common clauses for all maintainables along with specific from child references.
            IComplexStructureReferenceObject structureRef = BuildMaintainableWhere(query.StructuralMetadataWhere.Content, childRef);
            return new ComplexStructureQueryCore(structureRef, queryMetadata);
        }


        public IComplexStructureQuery Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.ReportingTaxonomyQueryType reportingTaxonomyQueryMsg)
        {
            ReportingTaxonomyQueryType query = reportingTaxonomyQueryMsg.SdmxQuery;

            // process details
            StructureReturnDetailsType returnDetails = query.ReturnDetails;
            IComplexStructureQueryMetadata queryMetadata = BuildQueryDetails(returnDetails);

            // process codelist specific clauses.
            ReportingTaxonomyWhereType reportingTaxonomyWhere = (ReportingTaxonomyWhereType)query.StructuralMetadataWhere.Content;//.getReportingTaxonomyWhere();
            IList<ItemWhere> itemWhereList = reportingTaxonomyWhere.ItemWhere;
            IComplexIdentifiableReferenceObject childRef = null;
            if (itemWhereList != null && itemWhereList.Count > 0)
            {
                if (itemWhereList.Count > 1)
                {
                    //TODO warning or error that is not supported?????
                }
                childRef = BuildIdentifiableReference(itemWhereList[0].Content, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingCategory));
            }

            // process common clauses for all maintainables along with specific from child references.
            IComplexStructureReferenceObject structureRef = BuildMaintainableWhere(query.StructuralMetadataWhere.Content, childRef);
            return new ComplexStructureQueryCore(structureRef, queryMetadata);
        }


        public IComplexStructureQuery Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.StructureSetQueryType structureSetQueryMsg)
        {
            StructureSetQueryType query = structureSetQueryMsg.SdmxQuery;

            // process details
            StructureReturnDetailsType returnDetails = query.ReturnDetails;
            IComplexStructureQueryMetadata queryMetadata = BuildQueryDetails(returnDetails);

            // process common clauses for all maintainables along with specific from child references.
            IComplexStructureReferenceObject structureRef = BuildMaintainableWhere(query.StructuralMetadataWhere.Content, null);
            return new ComplexStructureQueryCore(structureRef, queryMetadata);
        }


        public IComplexStructureQuery Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.ProcessQueryType processQueryMsg)
        {
            ProcessQueryType query = processQueryMsg.SdmxQuery;

            // process details
            StructureReturnDetailsType returnDetails = query.ReturnDetails;
            IComplexStructureQueryMetadata queryMetadata = BuildQueryDetails(returnDetails);

            // process common clauses for all maintainables along with specific from child references.
            IComplexStructureReferenceObject structureRef = BuildMaintainableWhere(query.StructuralMetadataWhere.Content, null);
            return new ComplexStructureQueryCore(structureRef, queryMetadata);
        }


        public IComplexStructureQuery Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.CategorisationQueryType categorisationQueryMsg)
        {
            CategorisationQueryType query = categorisationQueryMsg.SdmxQuery;

            // process details
            StructureReturnDetailsType returnDetails = query.ReturnDetails;
            IComplexStructureQueryMetadata queryMetadata = BuildQueryDetails(returnDetails);

            // process common clauses for all maintainables along with specific from child references.
            IComplexStructureReferenceObject structureRef = BuildMaintainableWhere(query.StructuralMetadataWhere.Content, null);
            return new ComplexStructureQueryCore(structureRef, queryMetadata);
        }


        public IComplexStructureQuery Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.ProvisionAgreementQueryType provisionAgreementQueryMsg)
        {
            ProvisionAgreementQueryType query = provisionAgreementQueryMsg.SdmxQuery;

            // process details
            StructureReturnDetailsType returnDetails = query.ReturnDetails;
            IComplexStructureQueryMetadata queryMetadata = BuildQueryDetails(returnDetails);

            // process common clauses for all maintainables along with specific from child references.
            IComplexStructureReferenceObject structureRef = BuildMaintainableWhere(query.StructuralMetadataWhere.Content, null);
            return new ComplexStructureQueryCore(structureRef, queryMetadata);
        }

        public IComplexStructureQuery Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.ConstraintQueryType constraintQueryMsg)
        {
            ConstraintQueryType query = constraintQueryMsg.SdmxQuery;

            // process details
            StructureReturnDetailsType returnDetails = query.ReturnDetails;
            IComplexStructureQueryMetadata queryMetadata = BuildQueryDetails(returnDetails);

            // process common clauses for all maintainables along with specific from child references.
            IComplexStructureReferenceObject structureRef = BuildMaintainableWhere(query.StructuralMetadataWhere.Content, null);
            return new ComplexStructureQueryCore(structureRef, queryMetadata);
        }

        public IComplexStructureQuery Build(Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.StructuresQueryType structuresQueryMsg)
        {
            StructuresQueryType query = structuresQueryMsg.SdmxQuery;

            // process details
            StructureReturnDetailsType returnDetails = query.ReturnDetails;
            IComplexStructureQueryMetadata queryMetadata = BuildQueryDetails(returnDetails);

            // process common clauses for all maintainables along with specific from child references.
            IComplexStructureReferenceObject structureRef = BuildMaintainableWhere(query.StructuralMetadataWhere.Content, null);
            return new ComplexStructureQueryCore(structureRef, queryMetadata);
        }
        #endregion
    }
}