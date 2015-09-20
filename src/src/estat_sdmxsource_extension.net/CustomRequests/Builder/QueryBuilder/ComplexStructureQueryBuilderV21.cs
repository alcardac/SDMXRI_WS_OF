// -----------------------------------------------------------------------
// <copyright file="ComplexStructureQueryBuilderV21.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Estat.Sri.CustomRequests.Builder.QueryBuilder
{
	using System;
	using System.Xml.Linq;

	using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
	using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
	using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query;
	using Org.Sdmxsource.Sdmx.Api.Constants;
	using Org.Sdmxsource.Sdmx.Api.Exception;
	using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;
	using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21;

	using CategorisationQueryType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.CategorisationQueryType;
	using CategorySchemeQueryType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.CategorySchemeQueryType;
	using CodelistQueryType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.CodelistQueryType;
	using ConceptSchemeQueryType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.ConceptSchemeQueryType;
	using DataStructureQueryType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.DataStructureQueryType;
	using DataflowQueryType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message.DataflowQueryType;

	/// <summary>
	/// Builder for StructureQueryRequest <code>org.w3c.dom.Document</code> in SDMX 2.1 
	/// rom <code>ComplexStructureQuery</code> object.
	/// </summary>
	public class ComplexStructureQueryBuilderV21 : IComplexStructureQueryBuilder<XDocument>
	{
		public XDocument BuildComplexStructureQuery(IComplexStructureQuery query)
		{
			IComplexStructureReferenceObject complexStructureRef = query.StructureReference;
			if (complexStructureRef == null)
			{
				throw new SdmxException(
					"At least type expected in the reference",
					SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.SemanticError));
			}
			IComplexStructureQueryMetadata complexStructureQueryMetadata = query.StructureQueryMetadata;
			String agencyId = (complexStructureRef.AgencyId != null)
								  ? complexStructureRef.AgencyId.SearchParameter
								  : null;
			String artefactId = (complexStructureRef.Id != null) ? complexStructureRef.Id.SearchParameter : null;
			String version = (complexStructureRef.VersionReference != null)
								 ? complexStructureRef.VersionReference.Version
								 : null;

			//common tags
			StructureReturnDetailsType returnDetailsType;

			XDocument xDoc = null;

			switch (complexStructureRef.ReferencedStructureType.EnumType)
			{
				case SdmxStructureEnumType.Dsd:
					var dataStructureQuery = new DataStructureQuery(new DataStructureQueryType());
					dataStructureQuery.Content.Header = new BasicHeaderType();
					V21Helper.SetHeader(dataStructureQuery.Content.Header, null);

					var dataStructureQueryChild =
						new Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query.DataStructureQueryType();
					dataStructureQuery.Content.SdmxQuery = dataStructureQueryChild;
					returnDetailsType = new StructureReturnDetailsType();
					dataStructureQueryChild.ReturnDetails = returnDetailsType;

					this.FillDataQueryDetails(complexStructureQueryMetadata, returnDetailsType);

                    var dataStructureWhereType = BuildWhereType<DataStructureWhereType>(complexStructureRef);
			        dataStructureQueryChild.StructuralMetadataWhere = new DataStructureWhere(dataStructureWhereType);

					xDoc = new XDocument(dataStructureQuery.Untyped);

					break;
				case SdmxStructureEnumType.Dataflow:
					var dataflowQuery = new DataflowQuery(new DataflowQueryType());
					dataflowQuery.Content.Header = new BasicHeaderType();
					V21Helper.SetHeader(dataflowQuery.Content.Header, null);

					var dataflowQueryTypeChild = new Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query.DataflowQueryType();
                    dataflowQuery.Content.SdmxQuery = dataflowQueryTypeChild;
					returnDetailsType = new StructureReturnDetailsType();
					dataflowQueryTypeChild.ReturnDetails = returnDetailsType;

					this.FillDataQueryDetails(complexStructureQueryMetadata, returnDetailsType);

                    var dataflowWhereType = BuildWhereType<DataflowWhereType>(complexStructureRef);
					dataflowQueryTypeChild.StructuralMetadataWhere = new DataflowWhere(dataflowWhereType);

					xDoc = new XDocument(dataflowQuery.Untyped);
					break;
				case SdmxStructureEnumType.CategoryScheme:
					var categorySchemeQuery = new CategorySchemeQuery(new CategorySchemeQueryType());
					categorySchemeQuery.Content.Header = new BasicHeaderType();
					V21Helper.SetHeader(categorySchemeQuery.Content.Header, null);
					var categorySchemeQueryType = new Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query.CategorySchemeQueryType();
                    categorySchemeQuery.Content.SdmxQuery = categorySchemeQueryType;
					returnDetailsType = new StructureReturnDetailsType();
					categorySchemeQueryType.ReturnDetails = returnDetailsType;
					this.FillDataQueryDetails(complexStructureQueryMetadata, returnDetailsType);
                    var categorySchemeWhere = BuildWhereType<CategorySchemeWhereType>(complexStructureRef);

					categorySchemeQueryType.StructuralMetadataWhere = new CategorySchemeWhere(categorySchemeWhere);

					xDoc = new XDocument(categorySchemeQuery.Untyped);
					break;
				case SdmxStructureEnumType.CodeList:
					var codelistQuery = new CodelistQuery(new CodelistQueryType());
					codelistQuery.Content.Header = new BasicHeaderType();
					V21Helper.SetHeader(codelistQuery.Content.Header, null);
					var codelistQueryType = new Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query.CodelistQueryType();
                    codelistQuery.Content.SdmxQuery = codelistQueryType;
					returnDetailsType = new StructureReturnDetailsType();
					codelistQueryType.ReturnDetails = returnDetailsType;
					this.FillDataQueryDetails(complexStructureQueryMetadata, returnDetailsType);
					var codelistWhereType =  BuildWhereType<CodelistWhereType>(complexStructureRef);
					codelistQueryType.StructuralMetadataWhere = new CodelistWhere(codelistWhereType);

					xDoc = new XDocument(codelistQuery.Untyped);
					break;
				case SdmxStructureEnumType.Categorisation:
					var categorisationQuery = new CategorisationQuery(new CategorisationQueryType());
					categorisationQuery.Content.Header = new BasicHeaderType();
					V21Helper.SetHeader(categorisationQuery.Content.Header, null);
					var categorisationQueryType = new Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query.CategorisationQueryType();
                    categorisationQuery.Content.SdmxQuery = categorisationQueryType;
					returnDetailsType = new StructureReturnDetailsType();
					categorisationQueryType.ReturnDetails = returnDetailsType;
					this.FillDataQueryDetails(complexStructureQueryMetadata, returnDetailsType);
					var categorisationWhereType = BuildWhereType<CategorisationWhereType>(complexStructureRef);
					categorisationQueryType.StructuralMetadataWhere = new CategorisationWhere(categorisationWhereType);

					xDoc = new XDocument(categorisationQuery.Untyped);
					break;

				case SdmxStructureEnumType.ConceptScheme:
					var conceptSchemeQuery = new ConceptSchemeQuery(new ConceptSchemeQueryType());
					conceptSchemeQuery.Content.Header = new BasicHeaderType();
					V21Helper.SetHeader(conceptSchemeQuery.Content.Header, null);
					var conceptSchemeQueryType = new Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query.ConceptSchemeQueryType();
                    conceptSchemeQuery.Content.SdmxQuery = conceptSchemeQueryType;
					returnDetailsType = new StructureReturnDetailsType();
					conceptSchemeQueryType.ReturnDetails = returnDetailsType;
					this.FillDataQueryDetails(complexStructureQueryMetadata, returnDetailsType);
                    var conceptSchemeWhereType = BuildWhereType<ConceptSchemeWhereType>(complexStructureRef);

					conceptSchemeQueryType.StructuralMetadataWhere = new ConceptSchemeWhere(conceptSchemeWhereType);

					xDoc = new XDocument(conceptSchemeQuery.Untyped);
					break;
			}

			return xDoc;
		}

        /// <summary>
        /// Builds the WhereType.
        /// </summary>
        /// <typeparam name="TWhereType">The type of the where type. Must be based on MaintainableWhereType.</typeparam>
        /// <param name="complexStructureRef">The complex structure preference.</param>
        /// <returns>THe where type</returns>
        private static TWhereType BuildWhereType<TWhereType>(IComplexStructureReferenceObject complexStructureRef) where TWhereType : Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query.MaintainableWhereType, new()
        {
            string agencyId = (complexStructureRef.AgencyId != null)
                                  ? complexStructureRef.AgencyId.SearchParameter
                                  : null;
            string artefactId = (complexStructureRef.Id != null) ? complexStructureRef.Id.SearchParameter : null;
            string version = (complexStructureRef.VersionReference != null)
                                 ? complexStructureRef.VersionReference.Version
                                 : null;
            var dataStructureWhereType = new TWhereType();

            if (!string.IsNullOrWhiteSpace(artefactId))
            {
                var queryIdType = new QueryIDType();
                dataStructureWhereType.ID = queryIdType;
                queryIdType.@operator = complexStructureRef.Id.Operator.Operator;
                queryIdType.TypedValue = artefactId;
            }

            if (!string.IsNullOrWhiteSpace(version))
            {
                dataStructureWhereType.Version = version;
            }

            if (!string.IsNullOrWhiteSpace(agencyId))
            {
                var queryAgencyIdType = new QueryNestedIDType();
                dataStructureWhereType.AgencyID = queryAgencyIdType;

                queryAgencyIdType.@operator = OrderedOperator.GetFromEnum(OrderedOperatorEnumType.Equal).OrdOperator;
                queryAgencyIdType.TypedValue = agencyId;
            }

            return dataStructureWhereType;
        }

	    private void FillDataQueryDetails(
			IComplexStructureQueryMetadata structureQueryMetadata, StructureReturnDetailsType returnDetailsType)
		{
			var referencesType = new ReferencesType();
			returnDetailsType.References = referencesType;

			if (structureQueryMetadata.StructureQueryDetail != null)
			{
				switch (structureQueryMetadata.StructureQueryDetail.EnumType)
				{
					case ComplexStructureQueryDetailEnumType.Stub:
						returnDetailsType.detail = "Stub";
						break;

					default:
						returnDetailsType.detail = "Full";
						break;
				}
			}
			if (structureQueryMetadata.ReferencesQueryDetail != null)
			{
				switch (structureQueryMetadata.ReferencesQueryDetail.EnumType)
				{
					case ComplexMaintainableQueryDetailEnumType.Stub:
						referencesType.detail = "Stub";
						break;

					default:
						referencesType.detail = "Full";
						break;
				}
			}
			if (structureQueryMetadata.StructureReferenceDetail != null)
			{
				switch (structureQueryMetadata.StructureReferenceDetail.EnumType)
				{
					case StructureReferenceDetailEnumType.All:
						referencesType.All = new EmptyType();
						break;
					case StructureReferenceDetailEnumType.None:
						referencesType.None = new EmptyType();
						break;
					case StructureReferenceDetailEnumType.Children:
						referencesType.Children = new EmptyType();
						break;
					case StructureReferenceDetailEnumType.Descendants:
						referencesType.Descendants = new EmptyType();
						break;
					case StructureReferenceDetailEnumType.Parents:
						referencesType.Parents = new EmptyType();
						break;
					case StructureReferenceDetailEnumType.ParentsSiblings:
						referencesType.ParentsAndSiblings = new EmptyType();
						break;
				}
			}

			if (structureQueryMetadata.ReferenceSpecificStructures != null
				&& structureQueryMetadata.ReferenceSpecificStructures.Count > 0)
			{
				var objectTypeListType = new MaintainableObjectTypeListType();
				referencesType.SpecificObjects = objectTypeListType;

				foreach (SdmxStructureType sdmxType in structureQueryMetadata.ReferenceSpecificStructures)
				{
					switch (sdmxType.EnumType)
					{
						case SdmxStructureEnumType.AgencyScheme:
							objectTypeListType.AgencyScheme = new AgencyScheme();
							break;
						case SdmxStructureEnumType.AttachmentConstraint:
							objectTypeListType.AttachmentConstraint = new AttachmentConstraint();
							break;
						case SdmxStructureEnumType.Categorisation:
							objectTypeListType.Categorisation = new Categorisation();
							break;
						case SdmxStructureEnumType.CodeList:
							objectTypeListType.Codelist = new Codelist();
							break;
						case SdmxStructureEnumType.ConceptScheme:
							objectTypeListType.ConceptScheme = new ConceptScheme();
							break;
						case SdmxStructureEnumType.ContentConstraint:
							objectTypeListType.ContentConstraint = new ContentConstraint();
							break;
						case SdmxStructureEnumType.Dataflow:
							objectTypeListType.Dataflow = new Dataflow();
							break;
						case SdmxStructureEnumType.DataConsumerScheme:
							objectTypeListType.DataConsumerScheme = new DataConsumerScheme();
							break;
						case SdmxStructureEnumType.DataProviderScheme:
							objectTypeListType.DataProviderScheme = new DataProviderScheme();
							break;
						case SdmxStructureEnumType.Dsd:
							objectTypeListType.DataStructure = new DataStructure();
							break;
						case SdmxStructureEnumType.HierarchicalCodelist:
							objectTypeListType.HierarchicalCodelist = new HierarchicalCodelist();
							break;
						case SdmxStructureEnumType.MetadataFlow:
							objectTypeListType.Metadataflow = new Metadataflow();
							break;
						case SdmxStructureEnumType.Msd:
							objectTypeListType.MetadataSet = new MetadataSet();
							break;
						case SdmxStructureEnumType.OrganisationUnitScheme:
							objectTypeListType.OrganisationUnitScheme = new OrganisationUnitScheme();
							break;
						case SdmxStructureEnumType.Process:
							objectTypeListType.Process = new Process();
							break;
						case SdmxStructureEnumType.ProvisionAgreement:
							objectTypeListType.ProvisionAgreement = new ProvisionAgreement();
							break;
						case SdmxStructureEnumType.ReportingTaxonomy:
							objectTypeListType.ReportingTaxonomy = new ReportingTaxonomy();
							break;
						case SdmxStructureEnumType.StructureSet:
							objectTypeListType.StructureSet = new StructureSet();
							break;
					}
				}
			}
			referencesType.processConstraints = false;
		}
	}
}
