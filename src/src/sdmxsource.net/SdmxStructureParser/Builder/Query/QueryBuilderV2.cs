// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryBuilderV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Singleton factory pattern to build Version 2 reference beans from query types
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.Query
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.query;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    using QueryMessageType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.QueryMessageType;

    /// <summary>
    ///     Singleton factory pattern to build Version 2 reference beans from query types
    /// </summary>
    public class QueryBuilderV2
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build a list of structure references
        /// </summary>
        /// <param name="queryStructureRequests">
        /// given structure request query containing lists of references for each structure type.
        /// </param>
        /// <returns>
        /// list of structure references
        /// </returns>
        public IList<IStructureReference> Build(QueryStructureRequestType queryStructureRequests)
        {
            IList<IStructureReference> reutrnList = new List<IStructureReference>();

            /* foreach */
            foreach (AgencyRefType agencyRefType in queryStructureRequests.AgencyRef)
            {
                var urn = agencyRefType.URN;
                if (ObjectUtil.ValidString(urn))
                {
                    reutrnList.Add(new StructureReferenceImpl(urn));
                }
                else
                {
                    string agencyId = agencyRefType.AgencyID;
                    string maintId = agencyRefType.OrganisationSchemeID;
                    string version = agencyRefType.OrganisationSchemeAgencyID;
                    reutrnList.Add(
                        new StructureReferenceImpl(
                            agencyId, maintId, version, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Agency)));
                }
            }

            if (queryStructureRequests.CategorySchemeRef != null)
            {
                /* foreach */
                foreach (CategorySchemeRefType refType in queryStructureRequests.CategorySchemeRef)
                {
                    var urn0 = refType.URN;
                    if (ObjectUtil.ValidString(urn0))
                    {
                        reutrnList.Add(new StructureReferenceImpl(urn0));
                    }
                    else
                    {
                        string agencyId1 = refType.AgencyID;
                        string maintId2 = refType.CategorySchemeID;
                        string version3 = refType.Version;
                        reutrnList.Add(
                            new StructureReferenceImpl(
                                agencyId1, 
                                maintId2, 
                                version3, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme)));
                    }
                }
            }

            if (queryStructureRequests.CodelistRef != null)
            {
                /* foreach */
                foreach (CodelistRefType refType4 in queryStructureRequests.CodelistRef)
                {
                    var urn5 = refType4.URN;
                    if (ObjectUtil.ValidString(urn5))
                    {
                        reutrnList.Add(new StructureReferenceImpl(urn5));
                    }
                    else
                    {
                        string agencyId6 = refType4.AgencyID;
                        string maintId7 = refType4.CodelistID;
                        string version8 = refType4.Version;
                        reutrnList.Add(
                            new StructureReferenceImpl(
                                agencyId6, 
                                maintId7, 
                                version8, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList)));
                    }
                }
            }

            if (queryStructureRequests.ConceptSchemeRef != null)
            {
                /* foreach */
                foreach (ConceptSchemeRefType refType9 in queryStructureRequests.ConceptSchemeRef)
                {
                    var urn10 = refType9.URN;
                    if (ObjectUtil.ValidString(urn10))
                    {
                        reutrnList.Add(new StructureReferenceImpl(urn10));
                    }
                    else
                    {
                        string agencyId11 = refType9.AgencyID;
                        string maintId12 = refType9.ConceptSchemeID;
                        string version13 = refType9.Version;
                        reutrnList.Add(
                            new StructureReferenceImpl(
                                agencyId11, 
                                maintId12, 
                                version13, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme)));
                    }
                }
            }
            
            if (queryStructureRequests.DataflowRef != null)
            {
                foreach (DataflowRefType refType14 in queryStructureRequests.DataflowRef)
                {
                    var structureReferenceImpl = this.BuildDataflowQuery(refType14);
                    reutrnList.Add(structureReferenceImpl);
                }
            }

            if (queryStructureRequests.DataProviderRef != null)
            {
                /* foreach */
                foreach (DataProviderRefType refType19 in queryStructureRequests.DataProviderRef)
                {
                    var urn20 = refType19.URN;
                    if (ObjectUtil.ValidString(urn20))
                    {
                        reutrnList.Add(new StructureReferenceImpl(urn20));
                    }
                    else
                    {
                        string agencyId21 = refType19.OrganisationSchemeAgencyID;
                        string maintId22 = refType19.OrganisationSchemeAgencyID;
                        string version23 = refType19.Version;
                        string id = refType19.DataProviderID;
                        reutrnList.Add(
                            new StructureReferenceImpl(
                                agencyId21, 
                                maintId22, 
                                version23, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProvider), 
                                id));
                    }
                }
            }

            if (queryStructureRequests.HierarchicalCodelistRef != null)
            {
                /* foreach */
                foreach (HierarchicalCodelistRefType refType24 in queryStructureRequests.HierarchicalCodelistRef)
                {
                    var urn25 = refType24.URN;
                    if (ObjectUtil.ValidString(urn25))
                    {
                        reutrnList.Add(new StructureReferenceImpl(urn25));
                    }
                    else
                    {
                        string agencyId26 = refType24.AgencyID;
                        string maintId27 = refType24.HierarchicalCodelistID;
                        string version28 = refType24.Version;
                        reutrnList.Add(
                            new StructureReferenceImpl(
                                agencyId26, 
                                maintId27, 
                                version28, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist)));
                    }
                }
            }

            if (queryStructureRequests.KeyFamilyRef != null)
            {
                /* foreach */
                foreach (KeyFamilyRefType refType29 in queryStructureRequests.KeyFamilyRef)
                {
                    var urn30 = refType29.URN;
                    if (ObjectUtil.ValidString(urn30))
                    {
                        reutrnList.Add(new StructureReferenceImpl(urn30));
                    }
                    else
                    {
                        string agencyId31 = refType29.AgencyID;
                        string maintId32 = refType29.KeyFamilyID;
                        string version33 = refType29.Version;
                        reutrnList.Add(
                            new StructureReferenceImpl(
                                agencyId31, 
                                maintId32, 
                                version33, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd)));
                    }
                }
            }

            if (queryStructureRequests.MetadataflowRef != null)
            {
                /* foreach */
                foreach (MetadataflowRefType refType34 in queryStructureRequests.MetadataflowRef)
                {
                    var urn35 = refType34.URN;
                    if (ObjectUtil.ValidString(urn35))
                    {
                        reutrnList.Add(new StructureReferenceImpl(urn35));
                    }
                    else
                    {
                        string agencyId36 = refType34.AgencyID;
                        string maintId37 = refType34.MetadataflowID;
                        string version38 = refType34.Version;
                        reutrnList.Add(
                            new StructureReferenceImpl(
                                agencyId36, 
                                maintId37, 
                                version38, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow)));
                    }
                }
            }

            if (queryStructureRequests.MetadataStructureRef != null)
            {
                /* foreach */
                foreach (MetadataStructureRefType refType39 in queryStructureRequests.MetadataStructureRef)
                {
                    var urn40 = refType39.URN;
                    if (ObjectUtil.ValidString(urn40))
                    {
                        reutrnList.Add(new StructureReferenceImpl(urn40));
                    }
                    else
                    {
                        string agencyId41 = refType39.AgencyID;
                        string maintId42 = refType39.MetadataStructureID;
                        string version43 = refType39.Version;
                        reutrnList.Add(
                            new StructureReferenceImpl(
                                agencyId41, 
                                maintId42, 
                                version43, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd)));
                    }
                }
            }

            if (queryStructureRequests.OrganisationSchemeRef != null)
            {
                /* foreach */
                foreach (OrganisationSchemeRefType refType44 in queryStructureRequests.OrganisationSchemeRef)
                {
                    var urn45 = refType44.URN;
                    if (ObjectUtil.ValidString(urn45))
                    {
                        reutrnList.Add(new StructureReferenceImpl(urn45));
                    }
                    else
                    {
                        string agencyId46 = refType44.AgencyID;
                        string maintId47 = refType44.OrganisationSchemeID;
                        string version48 = refType44.Version;
                        reutrnList.Add(
                            new StructureReferenceImpl(
                                agencyId46, 
                                maintId47, 
                                version48, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AgencyScheme)));
                        reutrnList.Add(
                            new StructureReferenceImpl(
                                agencyId46, 
                                maintId47, 
                                version48, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumerScheme)));
                        reutrnList.Add(
                            new StructureReferenceImpl(
                                agencyId46, 
                                maintId47, 
                                version48, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProviderScheme)));
                    }
                }
            }

            if (queryStructureRequests.ProcessRef != null)
            {
                /* foreach */
                foreach (ProcessRefType refType49 in queryStructureRequests.ProcessRef)
                {
                    var urn50 = refType49.URN;
                    if (ObjectUtil.ValidString(urn50))
                    {
                        reutrnList.Add(new StructureReferenceImpl(urn50));
                    }
                    else
                    {
                        string agencyId51 = refType49.AgencyID;
                        string maintId52 = refType49.ProcessID;
                        string version53 = refType49.Version;
                        reutrnList.Add(
                            new StructureReferenceImpl(
                                agencyId51, 
                                maintId52, 
                                version53, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Process)));
                    }
                }
            }

            if (queryStructureRequests.ReportingTaxonomyRef != null)
            {
                /* foreach */
                foreach (ReportingTaxonomyRefType refType54 in queryStructureRequests.ReportingTaxonomyRef)
                {
                    var urn55 = refType54.URN;
                    if (ObjectUtil.ValidString(urn55))
                    {
                        reutrnList.Add(new StructureReferenceImpl(urn55));
                    }
                    else
                    {
                        string agencyId56 = refType54.AgencyID;
                        string maintId57 = refType54.ReportingTaxonomyID;
                        string version58 = refType54.Version;
                        reutrnList.Add(
                            new StructureReferenceImpl(
                                agencyId56, 
                                maintId57, 
                                version58, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingTaxonomy)));
                    }
                }
            }

            if (queryStructureRequests.StructureSetRef != null)
            {
                /* foreach */
                foreach (StructureSetRefType refType59 in queryStructureRequests.StructureSetRef)
                {
                    var urn60 = refType59.URN;
                    if (ObjectUtil.ValidString(urn60))
                    {
                        reutrnList.Add(new StructureReferenceImpl(urn60));
                    }
                    else
                    {
                        string agencyId61 = refType59.AgencyID;
                        string maintId62 = refType59.StructureSetID;
                        string version63 = refType59.Version;
                        reutrnList.Add(
                            new StructureReferenceImpl(
                                agencyId61, 
                                maintId62, 
                                version63, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureSet)));
                    }
                }
            }

            return reutrnList;
        }

        /// <summary>
        /// Build dataflow query. Override to alter the default behavior
        /// </summary>
        /// <param name="refType">
        /// The Dataflow reference (SDMX v2.0 
        /// </param>
        /// <returns>
        /// The <see cref="IStructureReference"/>.
        /// </returns>
        protected virtual IStructureReference BuildDataflowQuery(DataflowRefType refType)
        {
            var urn = refType.URN;

            StructureReferenceImpl structureReferenceImpl;

            if (ObjectUtil.ValidString(urn))
            {
                structureReferenceImpl = new StructureReferenceImpl(urn);
            }
            else
            {
                string agencyID = refType.AgencyID;
                string dataflowID = refType.DataflowID;
                string version = refType.Version;
                structureReferenceImpl = new StructureReferenceImpl(agencyID, dataflowID, version, SdmxStructureEnumType.Dataflow);
            }

            return structureReferenceImpl;
        }

        /// <summary>
        /// Build a provision agreement reference bean
        ///     If only dataProviderRef is supplied then this is used and the flow type is assumed to be a dataflow
        /// </summary>
        /// <param name="queryRegistrationRequestType">
        /// given registration request query containing references to its beans
        /// </param>
        /// <returns>
        /// provision agreement reference bean of query reference, which ever of provision agreement, dataflow, data provider or metadataflow is given, in that order.
        /// </returns>
        public IStructureReference Build(QueryRegistrationRequestType queryRegistrationRequestType)
        {
            DataflowRefType dataflowRef = queryRegistrationRequestType.DataflowRef;
            DataProviderRefType dataProviderRef = queryRegistrationRequestType.DataProviderRef;
            MetadataflowRefType metadataflowRef = queryRegistrationRequestType.MetadataflowRef;
            ProvisionAgreementRefType provRef = queryRegistrationRequestType.ProvisionAgreementRef;

            if (dataProviderRef != null)
            {
                if (ObjectUtil.ValidString(dataProviderRef.URN))
                {
                    return new StructureReferenceImpl(dataProviderRef.URN);
                }

                string agencyId = dataProviderRef.OrganisationSchemeAgencyID;
                string maintId = dataProviderRef.OrganisationSchemeID;
                string version = dataProviderRef.Version;
                string id = dataProviderRef.DataProviderID;
                return new StructureReferenceImpl(
                    agencyId, maintId, version, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProvider), id);
            }

            if (provRef != null)
            {
                if (ObjectUtil.ValidString(provRef.URN))
                {
                    return new StructureReferenceImpl(provRef.URN);
                }

                throw new ArgumentException(
                    "Version 2.0 query for registration by provision agreement must use the provision URN");
            }

            if (dataflowRef != null)
            {
                if (ObjectUtil.ValidString(dataflowRef.URN))
                {
                    return new StructureReferenceImpl(dataflowRef.URN);
                }

                string agencyId0 = dataflowRef.AgencyID;
                string maintId1 = dataflowRef.DataflowID;
                string version2 = dataflowRef.Version;
                return new StructureReferenceImpl(
                    agencyId0, maintId1, version2, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow));
            }

            if (metadataflowRef != null)
            {
                if (ObjectUtil.ValidString(metadataflowRef.URN))
                {
                    return new StructureReferenceImpl(metadataflowRef.URN);
                }

                string agencyId3 = metadataflowRef.AgencyID;
                string maintId4 = metadataflowRef.MetadataflowID;
                string version5 = metadataflowRef.Version;
                return new StructureReferenceImpl(
                    agencyId3, maintId4, version5, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow));
            }

            return null;
        }

        /// <summary>
        /// Build a provision agreement reference bean
        /// </summary>
        /// <param name="queryProvisionRequestType">
        /// given provision request query containing references to its beans
        /// </param>
        /// <returns>
        /// provision agreement reference bean of query reference, which ever of provision agreement, dataflow, metadataflow and data provider is given, in that order.
        /// </returns>
        public IStructureReference Build(QueryProvisioningRequestType queryProvisionRequestType)
        {
            DataflowRefType dataflowRef = queryProvisionRequestType.DataflowRef;
            DataProviderRefType dataProviderRef = queryProvisionRequestType.DataProviderRef;
            MetadataflowRefType metadataflowRef = queryProvisionRequestType.MetadataflowRef;
            ProvisionAgreementRefType provRef = queryProvisionRequestType.ProvisionAgreementRef;

            if (dataProviderRef != null)
            {
                if (ObjectUtil.ValidString(dataProviderRef.URN))
                {
                    return new StructureReferenceImpl(dataProviderRef.URN);
                }

                string agencyId = dataProviderRef.OrganisationSchemeAgencyID;
                string maintId = dataProviderRef.OrganisationSchemeID;
                string version = dataProviderRef.Version;
                string id = dataProviderRef.DataProviderID;
                return new StructureReferenceImpl(
                    agencyId, maintId, version, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProvider), id);
            }

            if (provRef != null)
            {
                if (ObjectUtil.ValidString(provRef.URN))
                {
                    return new StructureReferenceImpl(provRef.URN);
                }
            }

            if (dataflowRef != null)
            {
                if (ObjectUtil.ValidString(dataflowRef.URN))
                {
                    return new StructureReferenceImpl(dataflowRef.URN);
                }

                string agencyId0 = dataflowRef.AgencyID;
                string maintId1 = dataflowRef.DataflowID;
                string version2 = dataflowRef.Version;
                return new StructureReferenceImpl(
                    agencyId0, maintId1, version2, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow));
            }

            if (metadataflowRef != null)
            {
                if (ObjectUtil.ValidString(metadataflowRef.URN))
                {
                    return new StructureReferenceImpl(metadataflowRef.URN);
                }

                string agencyId3 = metadataflowRef.AgencyID;
                string maintId4 = metadataflowRef.MetadataflowID;
                string version5 = metadataflowRef.Version;
                return new StructureReferenceImpl(
                    agencyId3, maintId4, version5, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow));
            }

            throw new SdmxNotImplementedException(
                ExceptionCode.Unsupported, 
                "At version 2.0 provisions can only be queryies by Provision URN, Dataflow Ref, Data Provider Ref or Metadata Flow Ref");
        }

        /// <summary>
        /// Build a list of structure references
        /// </summary>
        /// <param name="queryMessage">
        /// given message query containing lists of references for each structure type.
        /// </param>
        /// <returns>
        /// list of structure references
        /// </returns>
        public IList<IStructureReference> Build(QueryMessageType queryMessage)
        {
            IList<IStructureReference> reutrnList = new List<IStructureReference>();

            if (queryMessage.Query != null)
            {
                QueryType queryType = queryMessage.Query;
                if (queryType.AgencyWhere != null)
                {
                    if (ObjectUtil.ValidCollection(queryMessage.Query.AgencyWhere))
                    {
                        throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "AgencyWhere");
                    }
                }

                if (queryType.CategorySchemeWhere != null)
                {
                    /* foreach */
                    foreach (CategorySchemeWhereType structQuery in queryType.CategorySchemeWhere)
                    {
                        string agencyId = structQuery.AgencyID;
                        string maintId = structQuery.ID;
                        string version = structQuery.Version;
                        SdmxStructureType structType =
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme);
                        IStructureReference refBean = new StructureReferenceImpl(agencyId, maintId, version, structType);
                        reutrnList.Add(refBean);
                    }
                }

                if (queryType.CodelistWhere != null)
                {
                    /* foreach */
                    foreach (CodelistWhereType structQuery0 in queryType.CodelistWhere)
                    {
                        string codelistId = null;
                        if (structQuery0.Codelist != null)
                        {
                            codelistId = structQuery0.Codelist.id;
                        }

                        string agencyId1 = structQuery0.AgencyID;
                        string maintId2 = codelistId;
                        string version3 = structQuery0.Version;
                        SdmxStructureType structType4 = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList);
                        IStructureReference refBean5 = new StructureReferenceImpl(
                            agencyId1, maintId2, version3, structType4);
                        reutrnList.Add(refBean5);
                        if (structQuery0.Or != null)
                        {
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "CodelistWhere/Or");
                        }

                        if (structQuery0.And != null)
                        {
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "CodelistWhere/And");
                        }
                    }
                }

                if (queryType.ConceptSchemeWhere != null)
                {
                    /* foreach */
                    foreach (ConceptSchemeWhereType structQuery6 in queryType.ConceptSchemeWhere)
                    {
                        string agencyId7 = structQuery6.AgencyID;
                        string maintId8 = structQuery6.ID;
                        string version9 = structQuery6.Version;
                        SdmxStructureType structType10 =
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme);
                        IStructureReference refBean11 = new StructureReferenceImpl(
                            agencyId7, maintId8, version9, structType10);
                        reutrnList.Add(refBean11);
                    }
                }

                if (queryType.ConceptWhere != null)
                {
                    /* foreach */
                    foreach (ConceptWhereType structQuery12 in queryType.ConceptWhere)
                    {
                        string agencyId13 = structQuery12.AgencyID;
                        string conceptId = structQuery12.Concept;
                        SdmxStructureType structType14 = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Concept);
                        IStructureReference refBean15 = new StructureReferenceImpl(
                            agencyId13,
                            ConceptSchemeObject.DefaultSchemeId,
                            ConceptSchemeObject.DefaultSchemeVersion, 
                            structType14, 
                            conceptId);
                        reutrnList.Add(refBean15);
                        if (structQuery12.Or != null)
                        {
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "ConceptWhere/Or");
                        }

                        if (structQuery12.And != null)
                        {
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "ConceptWhere/And");
                        }
                    }
                }

                if (queryType.DataflowWhere != null)
                {
                    /* foreach */
                    foreach (DataflowWhereType structQuery16 in queryType.DataflowWhere)
                    {
                        string agencyId17 = structQuery16.AgencyID;
                        string maintId18 = structQuery16.ID;
                        string version19 = structQuery16.Version;
                        SdmxStructureType structType20 = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow);
                        IStructureReference refBean21 = new StructureReferenceImpl(
                            agencyId17, maintId18, version19, structType20);
                        reutrnList.Add(refBean21);
                    }
                }

                if (queryType.DataProviderWhere != null && queryType.DataProviderWhere.Count > 0)
                {
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "DataProviderWhere");
                }

                if (queryType.DataWhere != null && queryType.DataWhere.Count > 0)
                {
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "DataWhere");
                }

                if (queryType.HierarchicalCodelistWhere != null)
                {
                    /* foreach */
                    foreach (HierarchicalCodelistWhereType structQuery22 in queryType.HierarchicalCodelistWhere)
                    {
                        string agencyId23 = structQuery22.AgencyID;
                        string maintId24 = structQuery22.ID;
                        string version25 = structQuery22.Version;
                        SdmxStructureType structType26 =
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist);
                        IStructureReference refBean27 = new StructureReferenceImpl(
                            agencyId23, maintId24, version25, structType26);
                        reutrnList.Add(refBean27);
                    }
                }

                if (queryType.KeyFamilyWhere != null)
                {
                    /* foreach */
                    foreach (KeyFamilyWhereType structQuery28 in queryType.KeyFamilyWhere)
                    {
                        string agencyId29 = structQuery28.AgencyID;
                        string maintId30 = structQuery28.KeyFamily;
                        string version31 = structQuery28.Version;
                        SdmxStructureType structType32 = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd);
                        IStructureReference refBean33 = new StructureReferenceImpl(
                            agencyId29, maintId30, version31, structType32);
                        reutrnList.Add(refBean33);

                        if (structQuery28.Or != null)
                        {
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "KeyFamilyWhere/Or");
                        }

                        if (structQuery28.And != null)
                        {
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "KeyFamilyWhere/And");
                        }

                        if (structQuery28.Dimension != null)
                        {
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "KeyFamilyWhere/Dimensions");
                        }

                        if (structQuery28.Attribute != null)
                        {
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "KeyFamilyWhere/Attribute");
                        }

                        if (structQuery28.Codelist != null)
                        {
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "KeyFamilyWhere/Codelist");
                        }

                        if (structQuery28.Category != null)
                        {
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "KeyFamilyWhere/Category");
                        }

                        if (structQuery28.Concept != null)
                        {
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "KeyFamilyWhere/Concept");
                        }
                    }
                }

                if (queryType.MetadataflowWhere != null)
                {
                    /* foreach */
                    foreach (MetadataflowWhereType structQuery34 in queryType.MetadataflowWhere)
                    {
                        string agencyId35 = structQuery34.AgencyID;
                        string maintId36 = structQuery34.ID;
                        string version37 = structQuery34.Version;
                        SdmxStructureType structType38 =
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow);
                        IStructureReference refBean39 = new StructureReferenceImpl(
                            agencyId35, maintId36, version37, structType38);
                        reutrnList.Add(refBean39);
                    }
                }

                if (queryType.MetadataWhere != null && queryType.MetadataWhere.Count > 0)
                {
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "DataWhere");
                }

                if (queryType.OrganisationSchemeWhere != null)
                {
                    /* foreach */
                    foreach (OrganisationSchemeWhereType structQuery40 in queryType.OrganisationSchemeWhere)
                    {
                        string agencyId41 = structQuery40.AgencyID;
                        string maintId42 = structQuery40.ID;
                        string version43 = structQuery40.Version;
                        SdmxStructureType structType44 =
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnitScheme);
                        IStructureReference refBean45 = new StructureReferenceImpl(
                            agencyId41, maintId42, version43, structType44);
                        reutrnList.Add(refBean45);
                    }
                }

                if (queryType.ProcessWhere != null)
                {
                    /* foreach */
                    foreach (ProcessWhereType structQuery46 in queryType.ProcessWhere)
                    {
                        string agencyId47 = structQuery46.AgencyID;
                        string maintId48 = structQuery46.ID;
                        string version49 = structQuery46.Version;
                        SdmxStructureType structType50 = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Process);
                        IStructureReference refBean51 = new StructureReferenceImpl(
                            agencyId47, maintId48, version49, structType50);
                        reutrnList.Add(refBean51);
                    }
                }

                if (queryType.StructureSetWhere != null)
                {
                    /* foreach */
                    foreach (StructureSetWhereType structQuery52 in queryType.StructureSetWhere)
                    {
                        string agencyId53 = structQuery52.AgencyID;
                        string maintId54 = structQuery52.ID;
                        string version55 = structQuery52.Version;
                        SdmxStructureType structType56 =
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureSet);
                        IStructureReference refBean57 = new StructureReferenceImpl(
                            agencyId53, maintId54, version55, structType56);
                        reutrnList.Add(refBean57);
                    }
                }

                if (queryType.ReportingTaxonomyWhere != null)
                {
                    /* foreach */
                    foreach (ReportingTaxonomyWhereType structQuery58 in queryType.ReportingTaxonomyWhere)
                    {
                        string agencyId59 = structQuery58.AgencyID;
                        string maintId60 = structQuery58.ID;
                        string version61 = structQuery58.Version;
                        SdmxStructureType structType62 =
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingTaxonomy);
                        IStructureReference refBean63 = new StructureReferenceImpl(
                            agencyId59, maintId60, version61, structType62);
                        reutrnList.Add(refBean63);
                    }
                }
            }

            return reutrnList;
        }

        #endregion
    }
}