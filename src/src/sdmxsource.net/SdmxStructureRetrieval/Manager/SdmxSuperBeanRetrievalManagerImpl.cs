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
	
	using System.Collections.Generic;
	using Org.Sdmxsource.Sdmx.Api.Builder;
	using Org.Sdmxsource.Sdmx.Api.Constants;
	using Org.Sdmxsource.Sdmx.Api.Exception;
	using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
	using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects;
	using Org.Sdmxsource.Sdmx.Api.Model.Objects;
	using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
	using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
	using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Categoryscheme;
	using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Codelist;
	using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Conceptscheme;
	using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Datastructure;
	using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Registry;
	using Org.Sdmxsource.Sdmx.Util.Objects.Container;
	using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// This implementation of the ISdmxBaseObjectRetrievalManager wraps a ISdmxRetrievalManager to gather the information required to build SuperBeans.
	/// </summary>
	///
	public class ISdmxBaseObjectRetrievalManagerImpl : AbstractRetrevalManager
		, ISdmxBaseObjectRetrievalManager {
	
        // aspect
		private IBaseObjectsBuilder superBeanBuilder;
	
		public ISdmxBaseObjectRetrievalManagerImpl(
				ISdmxRetrievalManager sdmxObjectRetrievalManager) : base(sdmxObjectRetrievalManager) {
		}
	
		/// <summary>
		/// Returns a set of super beans that match the query parameter
		/// </summary>
		///
		/// <param name="structureQuery"></param>
		/// <returns></returns>
		private IObjectsBase IGetObjectsBase(IStructureReference xref) {
			ISet<IMaintainableObject> maintainables = this.SdmxObjectRetrievalManager
					.GetMaintainableWithReferences(xref);
			ISdmxObjects beans = new SdmxObjectsImpl(null, maintainables);
			if (superBeanBuilder == null) {
				throw new Exception(
						"SuperBeansBuilder not set, ISdmxBaseObjectRetrievalManagerImpl is @Configurable please ensure AspectJ weaving is enabled");
			}
			return superBeanBuilder.Build(beans);
		}
	
		public virtual ICategorySchemeObjectBase IGetCategorySchemeObjectBase(
				IMaintainableRefObject xref) {
			ISet<ICategorySchemeObjectBase> beans = IGetCategorySchemeObjectsBase(xref);
			if (!Org.Sdmxsource.Util.ObjectUtil.ValidCollection(beans)) {
				throw new ReferenceException(
						Org.Sdmxsource.Sdmx.Api.Constants.ExceptionCode.ReferenceErrorUnresolvable,
						Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme), xref);
			}
            return (ICategorySchemeObjectBase)Org.Sdmxsource.Sdmx.Util.Objects.SuperBeanRefUtil<ICategorySchemeObjectBase>.ResolveReference(
					beans, xref);
		}
	
		public virtual ISet<ICategorySchemeObjectBase> IGetCategorySchemeObjectsBase(
				IMaintainableRefObject xref) {
			return IGetObjectsBase(
                    new StructureReferenceImpl(xref,
							Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme)))
					.CategorySchemes;
		}
	
		public virtual ICodelistObjectBase IGetCodelistObjectBase(IMaintainableRefObject xref) {
			ISet<ICodelistObjectBase> beans = IGetCodelistObjectsBase(xref);
			if (!Org.Sdmxsource.Util.ObjectUtil.ValidCollection(beans)) {
				throw new ReferenceException(
						Org.Sdmxsource.Sdmx.Api.Constants.ExceptionCode.ReferenceErrorUnresolvable,
						Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList), xref);
			}
            return (ICodelistObjectBase)Org.Sdmxsource.Sdmx.Util.Objects.SuperBeanRefUtil<ICodelistObjectBase>
					.ResolveReference(beans, xref);
		}
	
		public virtual ISet<ICodelistObjectBase> IGetCodelistObjectsBase(IMaintainableRefObject xref) {
			return IGetObjectsBase(
                    new StructureReferenceImpl(xref,
							Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList))).Codelists;
		}
	
		public virtual IConceptSchemeObjectBase IGetConceptSchemeObjectBase(
				IMaintainableRefObject xref) {
			ISet<IConceptSchemeObjectBase> beans = IGetConceptSchemeObjectsBase(xref);
			if (!Org.Sdmxsource.Util.ObjectUtil.ValidCollection(beans)) {
				throw new ReferenceException(
						Org.Sdmxsource.Sdmx.Api.Constants.ExceptionCode.ReferenceErrorUnresolvable,
						Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme), xref);
			}
            return (IConceptSchemeObjectBase)Org.Sdmxsource.Sdmx.Util.Objects.SuperBeanRefUtil<IConceptSchemeObjectBase>.ResolveReference(
					beans, xref);
		}
	
		public virtual ISet<IConceptSchemeObjectBase> IGetConceptSchemeObjectsBase(
				IMaintainableRefObject xref) {
			return IGetObjectsBase(
                    new StructureReferenceImpl(xref,
							Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme)))
					.ConceptSchemes;
		}
	
		public virtual IDataflowObjectBase IGetDataflowObjectBase(IMaintainableRefObject xref) {
			ISet<IDataflowObjectBase> beans = IGetDataflowObjectsBase(xref);
			if (!Org.Sdmxsource.Util.ObjectUtil.ValidCollection(beans)) {
				throw new ReferenceException(
						Org.Sdmxsource.Sdmx.Api.Constants.ExceptionCode.ReferenceErrorUnresolvable,
						Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow), xref);
			}
            return (IDataflowObjectBase)Org.Sdmxsource.Sdmx.Util.Objects.SuperBeanRefUtil<IDataflowObjectBase>
					.ResolveReference(beans, xref);
		}
	
		public virtual ISet<IDataflowObjectBase> IGetDataflowObjectsBase(IMaintainableRefObject xref) {
			return IGetObjectsBase(
                    new StructureReferenceImpl(xref,
							Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow))).Dataflows;
		}
	
		public virtual IHierarchicalCodelistObjectBase IGetHierarchicCodeListObjectBase(
				IMaintainableRefObject xref) {
			ISet<IHierarchicalCodelistObjectBase> beans = IGetHierarchicCodeListObjectsBase(xref);
			if (!Org.Sdmxsource.Util.ObjectUtil.ValidCollection(beans)) {
				throw new ReferenceException(
						Org.Sdmxsource.Sdmx.Api.Constants.ExceptionCode.ReferenceErrorUnresolvable,
						Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist), xref);
			}
            return (IHierarchicalCodelistObjectBase)Org.Sdmxsource.Sdmx.Util.Objects.SuperBeanRefUtil<IHierarchicalCodelistObjectBase>
					.ResolveReference(beans, xref);
		}
	
		public virtual ISet<IHierarchicalCodelistObjectBase> IGetHierarchicCodeListObjectsBase(
				IMaintainableRefObject xref) {
			return IGetObjectsBase(
                    new StructureReferenceImpl(xref,
							Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.HierarchicalCodelist)))
					.HierarchicalCodelists;
		}
	
		public virtual IDataStructureObjectBase IGetDataStructureObjectBase(
				IMaintainableRefObject xref) {
			ISet<IDataStructureObjectBase> beans = IGetDataStructureObjectsBase(xref);
			if (!Org.Sdmxsource.Util.ObjectUtil.ValidCollection(beans)) {
				throw new ReferenceException(
						Org.Sdmxsource.Sdmx.Api.Constants.ExceptionCode.ReferenceErrorUnresolvable,
						Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd), xref);
			}
            return (IDataStructureObjectBase)Org.Sdmxsource.Sdmx.Util.Objects.SuperBeanRefUtil<IDataStructureObjectBase>.ResolveReference(
					beans, xref);
		}
	
		public virtual ISet<IDataStructureObjectBase> IGetDataStructureObjectsBase(
				IMaintainableRefObject xref) {
			return IGetObjectsBase(
                    new StructureReferenceImpl(xref, Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd)))
					.DataStructures;
		}
	
		public virtual IProvisionAgreementObjectBase IGetProvisionAgreementObjectBase(
				IMaintainableRefObject xref) {
			ISet<IProvisionAgreementObjectBase> beans = IGetProvisionAgreementObjectsBase(xref);
			if (!Org.Sdmxsource.Util.ObjectUtil.ValidCollection(beans)) {
				throw new ReferenceException(
						Org.Sdmxsource.Sdmx.Api.Constants.ExceptionCode.ReferenceErrorUnresolvable,
						Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProvisionAgreement), xref);
			}
            return (IProvisionAgreementObjectBase)Org.Sdmxsource.Sdmx.Util.Objects.SuperBeanRefUtil<IProvisionAgreementObjectBase>.ResolveReference(
					beans, xref);
		}
	
		public virtual ISet<IProvisionAgreementObjectBase> IGetProvisionAgreementObjectsBase(
				IMaintainableRefObject xref) {
			return IGetObjectsBase(
					new StructureReferenceImpl(xref,
							Org.Sdmxsource.Sdmx.Api.Constants.SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProvisionAgreement)))
					.Provisions;
		}
	
	}
}
