// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.2011051901     
// 12/10/2012 10:38 ��    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (c) 2012 Metadata Technology Ltd.
/// All rights reserved. This program and the accompanying materials
/// are made available under the terms of the GNU Public License v3.0
/// which accompanies this distribution, and is available at
/// http://www.gnu.org/licenses/gpl.html
/// This file is part of the SDMX Component Library.
/// The SDMX Component Library is free software: you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
/// The SDMX Component Library is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU General Public License for more details.
/// You should have received a copy of the GNU General Public License
/// along with The SDMX Component Library If not, see <http://www.gnu.org/licenses/>.
/// Contributors:
/// Metadata Technology - initial API and implementation
/// </summary>
///
namespace Org.Sdmxsource.Sdmx.Structureretrieval.Manager {
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Categoryscheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Conceptscheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Datastructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Metadatastructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference;

    /// <summary>
	/// This class wrappers a IStructureSearchManager to offer the interfaces as defined in the ISdmxRetrievalManager.
	/// </summary>
	///
	public class StructureSearchBeanRetrievalManager : 
			ISdmxRetrievalManager {
		internal IStructureSearchManager structureSearchManager;
	
		public StructureSearchBeanRetrievalManager(
				IStructureSearchManager structureSearchManager0) {
			this.structureSearchManager = structureSearchManager0;
		}
	
		public virtual ISet<IMaintainableObject> GetMaintainableWithReferences(
				IStructureReference query) {
			IStructureQuery sQuery = new RESTStructureQuery(
					Org.Sdmxsource.Sdmx.Api.Constants.StructureQueryDetail.Full,
					Org.Sdmxsource.Sdmx.Api.Constants.StructureReferenceDetailEnumType.Descendants, null, query, false);
			return structureSearchManager.GetMaintainables(sQuery)
					.GetAllMaintinables();
		}
	
		public virtual IMaintainableObject GetMaintainable(IStructureReference query) {
			IStructureQuery sQuery = new RESTStructureQuery(
                    Org.Sdmxsource.Sdmx.Api.Constants.StructureQueryDetail.Full, Org.Sdmxsource.Sdmx.Api.Constants.StructureReferenceDetailEnumType.None,
					null, query, false);
			ISdmxObjects beans = structureSearchManager.GetMaintainables(sQuery);
			ISet<IMaintainableObject> allMaintainables = beans.GetAllMaintinables();
			if (allMaintainables.Count == 0) {
				return null;
			}
			if (allMaintainables.Count > 1) {
				throw new Exception("Expected only one bean from query : "
						+ query.ToString());
			}
			return (IMaintainableObject) ILOG.J2CsMapping.Collections.Generics.Collections.ToArray(allMaintainables)[0];
		}
	
		public virtual IAgency GetAgency(String id) {
			//FUNC Support this
			throw new UnsupportedException("getAgency");
		}
	
		public virtual IAgencyScheme GetAgencySchemeBean(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IAgencyScheme> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AgencyScheme)).Bean;
		}
	
		public virtual IAttachmentConstraintObject GetAttachmentConstraint(
				IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IAttachmentConstraintObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AttachmentConstraint)).Bean;
		}
	
		public virtual ICategorisationObject GetCategorisation(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<ICategorisationObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Categorisation)).Bean;
		}
	
		public virtual ICodelistObject GetCodelist(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<ICodelistObject> (this, xref, Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList))
					.Bean;
		}
	
		public virtual IConceptSchemeObject GetConceptScheme(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IConceptSchemeObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme)).Bean;
		}
	
		public virtual IContentConstraintObject GetContentConstraint(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IContentConstraintObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ContentConstraint)).Bean;
		}
	
		public virtual ICategorySchemeObject GetCategoryScheme(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<ICategorySchemeObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme)).Bean;
		}
	
		public virtual IDataConsumerScheme GetDataConsumerSchemeBean(
				IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IDataConsumerScheme> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumerScheme)).Bean;
		}
	
		public virtual IDataflowObject GetDataflow(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IDataflowObject> (this, xref, Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow))
					.Bean;
		}
	
		public virtual IDataProviderScheme GetDataProviderSchemeBean(
				IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IDataProviderScheme> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProviderScheme)).Bean;
		}
	
		public virtual IHierarchicalCodelistObject GetHierarchicCodeList(
				IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IHierarchicalCodelistObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist)).Bean;
		}
	
		public virtual IMetadataFlow GetMetadataflow(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IMetadataFlow> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow)).Bean;
		}
	
		public virtual IDataStructureObject GetDataStructure(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IDataStructureObject> (this, xref, Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd))
					.Bean;
		}
	
		public virtual IMetadataStructureDefinitionObject GetMetadataStructure(
				IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IMetadataStructureDefinitionObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd)).Bean;
		}
	
		public virtual IOrganisationUnitSchemeObject GetOrganisationUnitScheme(
				IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IOrganisationUnitSchemeObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnitScheme)).Bean;
		}
	
		public virtual IProcessObject GetProcessBean(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IProcessObject> (this, xref, Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Process))
					.Bean;
		}
	
		public virtual IProvisionAgreementObject GetProvisionAgreementBean(
				IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IProvisionAgreementObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProvisionAgreement)).Bean;
		}
	
		public virtual IStructureSetObject GetStructureSet(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IStructureSetObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureSet)).Bean;
		}
	
		public virtual IReportingTaxonomyObject GetReportingTaxonomy(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IReportingTaxonomyObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingTaxonomy)).Bean;
		}
	
		public virtual ISet<IAttachmentConstraintObject> GetAttachmentConstraints(
				IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IAttachmentConstraintObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AttachmentConstraint)).Objects;
		}
	
		public virtual ISet<ICategorisationObject> GetCategorisationBeans(
				IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<ICategorisationObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Categorisation)).Objects;
		}
	
		public virtual ISet<ICodelistObject> GetCodelistBeans(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<ICodelistObject> (this, xref, Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList))
					.Objects;
		}
	
		public virtual ISet<IConceptSchemeObject> GetConceptSchemeBeans(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IConceptSchemeObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme)).Objects;
		}
	
		public virtual ISet<IContentConstraintObject> GetContentConstraints(
				IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IContentConstraintObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ContentConstraint)).Objects;
		}
	
		public virtual ISet<ICategorySchemeObject> GetCategorySchemeBeans(
				IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<ICategorySchemeObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme)).Objects;
		}
	
		public virtual ISet<IDataflowObject> GetDataflowBeans(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IDataflowObject> (this, xref, Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow))
					.Objects;
		}
	
		public virtual ISet<IHierarchicalCodelistObject> GetHierarchicCodeListBeans(
				IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IHierarchicalCodelistObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist)).Objects;
		}
	
		public virtual ISet<IMetadataFlow> GetMetadataflowBeans(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IMetadataFlow> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow)).Objects;
		}
	
		public virtual ISet<IDataStructureObject> GetDataStructureBeans(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IDataStructureObject> (this, xref, Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd))
					.Objects;
		}
	
		public virtual ISet<IMetadataStructureDefinitionObject> GetMetadataStructureBeans(
				IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IMetadataStructureDefinitionObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd)).Objects;
		}
	
		public virtual ISet<IOrganisationUnitSchemeObject> GetOrganisationUnitSchemeBeans(
				IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IOrganisationUnitSchemeObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnitScheme)).Objects;
		}
	
		public virtual ISet<IDataProviderScheme> GetDataProviderSchemeBeans(
				IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IDataProviderScheme> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProviderScheme)).Objects;
		}
	
		public virtual ISet<IDataConsumerScheme> GetDataConsumerSchemeBeans(
				IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IDataConsumerScheme> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumerScheme)).Objects;
		}
	
		public virtual ISet<IAgencyScheme> GetAgencySchemeBeans(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IAgencyScheme> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AgencyScheme)).Objects;
		}
	
		public virtual ISet<IProcessObject> GetProcessBeans(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IProcessObject> (this, xref, Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Process))
					.Objects;
		}
	
		public virtual ISet<IProvisionAgreementObject> GetProvisionAgreementBeans(
				IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IProvisionAgreementObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProvisionAgreement)).Objects;
		}
	
		public virtual ISet<IReportingTaxonomyObject> GetReportingTaxonomyBeans(
				IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IReportingTaxonomyObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingTaxonomy)).Objects;
		}
	
		public virtual ISet<IStructureSetObject> GetStructureSetBeans(IMaintainableRefObject xref) {
			return new StructureSearchBeanRetrievalManager.Resolver<IStructureSetObject> (this, xref,
					Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureSet)).Objects;
		}
	
		internal class Resolver<T> {
				private StructureSearchBeanRetrievalManager outerStructureSearchBeanRetrievalManager;
		
				private IStructureReference sRef;
		
				private SdmxStructureType type;
		
				public Resolver(StructureSearchBeanRetrievalManager manager, IMaintainableRefObject xref, SdmxStructureType type0) {
					outerStructureSearchBeanRetrievalManager = manager;
					this.sRef = new StructureReferenceCore(xref, type0);
					this.type = type0;
				}
		
				/* @SuppressWarnings("unchecked")*/ internal T GetBean() {
					ISet<T> beans = GetBeans();
					if (beans.Count > 1) {
						throw new Exception(
								"Expected only one bean from query : "
										+ sRef.ToString());
					}
					if (beans.Count == 0) {
						return  default(T)/* was: null */;
					}
					return (T) ILOG.J2CsMapping.Collections.Generics.Collections.ToArray(beans)[0];
				}
		
				/* @SuppressWarnings("unchecked")*/ internal ISet<T> GetBeans() {
					IStructureQuery sQuery = new RESTStructureQuery(
							Org.Sdmxsource.Sdmx.Api.Constants.StructureQueryDetail.Full,
							Org.Sdmxsource.Sdmx.Api.Constants.StructureReferenceDetail.None, type, sRef, false);
					ISdmxObjects beans = outerStructureSearchBeanRetrievalManager.structureSearchManager.GetMaintainables(sQuery);
					return (ISet<T>) beans.GetMaintinables(type);
				}
			}
	
	}
}
