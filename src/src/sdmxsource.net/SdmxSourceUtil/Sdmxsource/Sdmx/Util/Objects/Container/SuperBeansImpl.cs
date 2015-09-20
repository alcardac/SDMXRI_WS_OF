// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20110519_01     
// 10/1/12 2:32 PM    
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
namespace Org.Sdmxsource.Sdmx.Util.Objects.Container {
	
	using ILOG.J2CsMapping.Collections.Generics;
	using Org.Sdmxsource.Sdmx.Api.Constants;
	using Org.Sdmxsource.Sdmx.Api.Exception;
	using Org.Sdmxsource.Sdmx.Api.Model.Superbeans;
	using Org.Sdmxsource.Sdmx.Api.Model.Superbeans.Base;
	using Org.Sdmxsource.Sdmx.Api.Model.Superbeans.Categoryscheme;
	using Org.Sdmxsource.Sdmx.Api.Model.Superbeans.Codelist;
	using Org.Sdmxsource.Sdmx.Api.Model.Superbeans.Conceptscheme;
	using Org.Sdmxsource.Sdmx.Api.Model.Superbeans.Datastructure;
	using Org.Sdmxsource.Sdmx.Api.Model.Superbeans.Process;
	using Org.Sdmxsource.Sdmx.Api.Model.Superbeans.Registry;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	public class SuperBeansImpl : SuperBeans {
		private ILOG.J2CsMapping.Collections.Generics.ISet<CategorySchemeSuperBean> categorySchemes;
		private ILOG.J2CsMapping.Collections.Generics.ISet<CodelistSuperBean> codelists;
		private ILOG.J2CsMapping.Collections.Generics.ISet<ConceptSchemeSuperBean> conceptSchemes;
		private ILOG.J2CsMapping.Collections.Generics.ISet<DataflowSuperBean> dataflows;
		private ILOG.J2CsMapping.Collections.Generics.ISet<HierarchicalCodelistSuperBean> hcls;
		private ILOG.J2CsMapping.Collections.Generics.ISet<DataStructureSuperBean> dataStructures;
		private ILOG.J2CsMapping.Collections.Generics.ISet<ProvisionAgreementSuperBean> provisionAgreement;
		private ILOG.J2CsMapping.Collections.Generics.ISet<ProcessSuperBean> processes;
		private ILOG.J2CsMapping.Collections.Generics.ISet<RegistrationSuperBean> registrations;
	
		public SuperBeansImpl() {
			this.categorySchemes = new HashedSet<CategorySchemeSuperBean>();
			this.codelists = new HashedSet<CodelistSuperBean>();
			this.conceptSchemes = new HashedSet<ConceptSchemeSuperBean>();
			this.dataflows = new HashedSet<DataflowSuperBean>();
			this.hcls = new HashedSet<HierarchicalCodelistSuperBean>();
			this.dataStructures = new HashedSet<DataStructureSuperBean>();
			this.provisionAgreement = new HashedSet<ProvisionAgreementSuperBean>();
			this.processes = new HashedSet<ProcessSuperBean>();
			this.registrations = new HashedSet<RegistrationSuperBean>();
		}
	
		public SuperBeansImpl(ICollection<MaintainableSuperBean> allBeans) {
			this.categorySchemes = new HashedSet<CategorySchemeSuperBean>();
			this.codelists = new HashedSet<CodelistSuperBean>();
			this.conceptSchemes = new HashedSet<ConceptSchemeSuperBean>();
			this.dataflows = new HashedSet<DataflowSuperBean>();
			this.hcls = new HashedSet<HierarchicalCodelistSuperBean>();
			this.dataStructures = new HashedSet<DataStructureSuperBean>();
			this.provisionAgreement = new HashedSet<ProvisionAgreementSuperBean>();
			this.processes = new HashedSet<ProcessSuperBean>();
			this.registrations = new HashedSet<RegistrationSuperBean>();
			if (allBeans != null) {
				/* foreach */
				foreach (MaintainableSuperBean currentBean  in  allBeans) {
					if (currentBean  is  CategorySchemeSuperBean) {
						ILOG.J2CsMapping.Collections.Generics.Collections.Add(categorySchemes,(CategorySchemeSuperBean) currentBean);
					} else if (currentBean  is  CodelistSuperBean) {
						ILOG.J2CsMapping.Collections.Generics.Collections.Add(codelists,(CodelistSuperBean) currentBean);
					} else if (currentBean  is  ConceptSchemeSuperBean) {
						ILOG.J2CsMapping.Collections.Generics.Collections.Add(conceptSchemes,(ConceptSchemeSuperBean) currentBean);
					} else if (currentBean  is  DataflowSuperBean) {
						ILOG.J2CsMapping.Collections.Generics.Collections.Add(dataflows,(DataflowSuperBean) currentBean);
					} else if (currentBean  is  HierarchicalCodelistSuperBean) {
						ILOG.J2CsMapping.Collections.Generics.Collections.Add(hcls,(HierarchicalCodelistSuperBean) currentBean);
					} else if (currentBean  is  DataStructureSuperBean) {
						ILOG.J2CsMapping.Collections.Generics.Collections.Add(dataStructures,(DataStructureSuperBean) currentBean);
					} else if (currentBean.GetBuiltFrom().GetStructureType() == Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.PROVISION_AGREEMENT) {
						ILOG.J2CsMapping.Collections.Generics.Collections.Add(provisionAgreement,(ProvisionAgreementSuperBean) currentBean);
					} else if (currentBean  is  ProcessSuperBean) {
						ILOG.J2CsMapping.Collections.Generics.Collections.Add(processes,(ProcessSuperBean) currentBean);
					} else if (currentBean  is  RegistrationSuperBean) {
						ILOG.J2CsMapping.Collections.Generics.Collections.Add(registrations,(RegistrationSuperBean) currentBean);
					}
				}
			}
		}
	
		public SuperBeansImpl(params SuperBeans[] beans) {
			this.categorySchemes = new HashedSet<CategorySchemeSuperBean>();
			this.codelists = new HashedSet<CodelistSuperBean>();
			this.conceptSchemes = new HashedSet<ConceptSchemeSuperBean>();
			this.dataflows = new HashedSet<DataflowSuperBean>();
			this.hcls = new HashedSet<HierarchicalCodelistSuperBean>();
			this.dataStructures = new HashedSet<DataStructureSuperBean>();
			this.provisionAgreement = new HashedSet<ProvisionAgreementSuperBean>();
			this.processes = new HashedSet<ProcessSuperBean>();
			this.registrations = new HashedSet<RegistrationSuperBean>();
			/* foreach */
			foreach (SuperBeans currentBean  in  beans) {
				ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(currentBean.GetCategorySchemes(),this.categorySchemes);
				ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(currentBean.GetCodelists(),this.codelists);
				ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(currentBean.GetConceptSchemes(),this.conceptSchemes);
				ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(currentBean.GetDataflows(),this.dataflows);
				ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(currentBean.GetHierarchicalCodelists(),this.hcls);
				ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(currentBean.GetDataStructures(),this.dataStructures);
				ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(currentBean.GetProvisions(),this.provisionAgreement);
				ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(currentBean.GetProcesses(),this.processes);
				ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(currentBean.GetRegistartions(),this.registrations);
			}
		}
	
		public virtual void Merge(SuperBeans superBeans) {
			/* foreach */
			foreach (MaintainableSuperBean currentSb  in  superBeans.GetAllMaintainables()) {
				AddMaintainable(currentSb);
			}
		}
	
		public virtual void AddMaintainable(MaintainableSuperBean bean) {
			switch (bean.GetBuiltFrom().GetStructureType()) {
			case Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.CATEGORY_SCHEME:
				AddCategoryScheme((CategorySchemeSuperBean) bean);
				break;
			case Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.CODE_LIST:
				AddCodelist((CodelistSuperBean) bean);
				break;
			case Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.CONCEPT_SCHEME:
				AddConceptScheme((ConceptSchemeSuperBean) bean);
				break;
			case Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.DATAFLOW:
				AddDataflow((DataflowSuperBean) bean);
				break;
			case Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.HIERARCHICAL_CODELIST:
				AddHierarchicalCodelist((HierarchicalCodelistSuperBean) bean);
				break;
			case Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.DSD:
				AddDataStructure((DataStructureSuperBean) bean);
				break;
			case Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.PROCESS:
				AddProcess((ProcessSuperBean) bean);
				break;
			case Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.PROVISION_AGREEMENT:
				AddHierarchicalCodelist((HierarchicalCodelistSuperBean) bean);
				break;
			case Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.REGISTRATION:
				AddRegistration((RegistrationSuperBean) bean);
				break;
			default:
				throw new UnsupportedException(
						"SuperBeansImpl.addMaintainable of type : "
								+ bean.GetBuiltFrom().GetStructureType().GetType());
			}
		}
	
		public virtual void AddCategoryScheme(CategorySchemeSuperBean bean) {
			if (bean != null) {
				categorySchemes.Remove(bean);
				ILOG.J2CsMapping.Collections.Generics.Collections.Add(categorySchemes,bean);
			}
		}
	
		public virtual void AddCodelist(CodelistSuperBean bean) {
			if (bean != null) {
				codelists.Remove(bean);
				ILOG.J2CsMapping.Collections.Generics.Collections.Add(codelists,bean);
			}
		}
	
		public virtual void AddConceptScheme(ConceptSchemeSuperBean bean) {
			if (bean != null) {
				conceptSchemes.Remove(bean);
				ILOG.J2CsMapping.Collections.Generics.Collections.Add(conceptSchemes,bean);
			}
		}
	
		public virtual void AddDataflow(DataflowSuperBean bean) {
			if (bean != null) {
				dataflows.Remove(bean);
				ILOG.J2CsMapping.Collections.Generics.Collections.Add(dataflows,bean);
			}
		}
	
		public virtual void AddHierarchicalCodelist(HierarchicalCodelistSuperBean bean) {
			if (bean != null) {
				hcls.Remove(bean);
				ILOG.J2CsMapping.Collections.Generics.Collections.Add(hcls,bean);
			}
		}
	
		public virtual void AddDataStructure(DataStructureSuperBean bean) {
			if (bean != null) {
				dataStructures.Remove(bean);
				ILOG.J2CsMapping.Collections.Generics.Collections.Add(dataStructures,bean);
			}
		}
	
		public virtual void AddProvision(ProvisionAgreementSuperBean bean) {
			if (bean != null) {
				provisionAgreement.Remove(bean);
				ILOG.J2CsMapping.Collections.Generics.Collections.Add(provisionAgreement,bean);
			}
		}
	
		public virtual void AddProcess(ProcessSuperBean bean) {
			if (bean != null) {
				processes.Remove(bean);
				ILOG.J2CsMapping.Collections.Generics.Collections.Add(processes,bean);
			}
		}
	
		public virtual void AddRegistration(RegistrationSuperBean bean) {
			if (bean != null) {
				registrations.Remove(bean);
				ILOG.J2CsMapping.Collections.Generics.Collections.Add(registrations,bean);
			}
		}
	
		
		public virtual ILOG.J2CsMapping.Collections.Generics.ISet<CategorySchemeSuperBean> CategorySchemes {
		  get {
				return new HashedSet<CategorySchemeSuperBean>(categorySchemes);
			}
		}
		
	
		
		public virtual ILOG.J2CsMapping.Collections.Generics.ISet<CodelistSuperBean> Codelists {
		  get {
				return new HashedSet<CodelistSuperBean>(codelists);
			}
		}
		
	
		
		public virtual ILOG.J2CsMapping.Collections.Generics.ISet<ConceptSchemeSuperBean> ConceptSchemes {
		  get {
				return new HashedSet<ConceptSchemeSuperBean>(conceptSchemes);
			}
		}
		
	
		
		public virtual ILOG.J2CsMapping.Collections.Generics.ISet<DataflowSuperBean> Dataflows {
		  get {
				return new HashedSet<DataflowSuperBean>(dataflows);
			}
		}
		
	
		
		public virtual ILOG.J2CsMapping.Collections.Generics.ISet<HierarchicalCodelistSuperBean> HierarchicalCodelists {
		  get {
				return new HashedSet<HierarchicalCodelistSuperBean>(hcls);
			}
		}
		
	
		
		public virtual ILOG.J2CsMapping.Collections.Generics.ISet<DataStructureSuperBean> DataStructures {
		  get {
				return new HashedSet<DataStructureSuperBean>(dataStructures);
			}
		}
		
	
		
		public virtual ILOG.J2CsMapping.Collections.Generics.ISet<ProvisionAgreementSuperBean> Provisions {
		  get {
				return new HashedSet<ProvisionAgreementSuperBean>(provisionAgreement);
			}
		}
		
	
		
		public virtual ILOG.J2CsMapping.Collections.Generics.ISet<ProcessSuperBean> Processes {
		  get {
				return new HashedSet<ProcessSuperBean>(processes);
			}
		}
		
	
		
		public virtual ILOG.J2CsMapping.Collections.Generics.ISet<RegistrationSuperBean> Registartions {
		  get {
				return new HashedSet<RegistrationSuperBean>(registrations);
			}
		}
		
	
		public virtual void RemoveCategoryScheme(CategorySchemeSuperBean bean) {
			categorySchemes.Remove(bean);
		}
	
		public virtual void RemoveCodelist(CodelistSuperBean bean) {
			codelists.Remove(bean);
		}
	
		public virtual void RemoveConceptScheme(ConceptSchemeSuperBean bean) {
			conceptSchemes.Remove(bean);
		}
	
		public virtual void RemoveDataflow(DataflowSuperBean bean) {
			dataflows.Remove(bean);
		}
	
		public virtual void RemoveHierarchicalCodelist(HierarchicalCodelistSuperBean bean) {
			hcls.Remove(bean);
		}
	
		public virtual void RemoveDataStructure(DataStructureSuperBean bean) {
			dataStructures.Remove(bean);
		}
	
		public virtual void RemoveProvision(ProvisionAgreementSuperBean bean) {
			provisionAgreement.Remove(bean);
		}
	
		public virtual void RemoveProcess(ProcessSuperBean bean) {
			processes.Remove(bean);
		}
	
		public virtual void RemoveRegistration(RegistrationSuperBean bean) {
			registrations.Remove(bean);
		}
	
		
		public virtual ILOG.J2CsMapping.Collections.Generics.ISet<MaintainableSuperBean> AllMaintainables {
		  get {
				ILOG.J2CsMapping.Collections.Generics.ISet<MaintainableSuperBean> returnSet = new HashedSet<MaintainableSuperBean>();
				ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(this.categorySchemes,returnSet);
				ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(this.codelists,returnSet);
				ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(this.conceptSchemes,returnSet);
				ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(this.dataflows,returnSet);
				ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(this.hcls,returnSet);
				ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(this.dataStructures,returnSet);
				ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(this.provisionAgreement,returnSet);
				ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(this.processes,returnSet);
				ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(this.registrations,returnSet);
				return returnSet;
			}
		}
		
	}
}