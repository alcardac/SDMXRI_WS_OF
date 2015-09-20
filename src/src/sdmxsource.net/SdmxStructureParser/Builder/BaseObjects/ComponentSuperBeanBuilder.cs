// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.2011051901     
// 11/10/2012 5:50 ��    
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
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.BaseObjects.Impl {
	
	using Org.Apache.Log4j;
	using Org.Sdmxsource.Sdmx.Api.Exception;
	using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
	using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
	using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
	using Org.Sdmxsource.Sdmx.Api.Model.Objects.Conceptscheme;
	using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
	using Org.Sdmxsource.Sdmx.Api.Model.Superbeans;
	using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
	using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Codelist;
	using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Conceptscheme;
	using Org.Sdmxsource.Sdmx.Structureretrieval.Manager;
	using Org.Sdmxsource.Sdmx.Util.Objects.Container;
	using Org.Springframework.Objects.Factory.Annotation;
	using Org.Springframework.Stereotype;
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	public abstract class ComponentSuperBeanBuilder<K, V>
		 : StructureBuilderImpl<K, V>  where K : ISuper  where V : ISDMXObject {
		private static readonly log4net.ILog LOG = log4net.LogManager
				.GetLogger(typeof(ComponentSuperBeanBuilder));
	
		private CodelistSuperBeanBuilder codelistSuperBeanBuilder;
	
		private ConceptSuperBeanBuilder conceptSuperBeanBuilder;
	
		internal ICodelistObjectBase GetCodelist(IComponent componentBean,
				ISdmxRetrievalManager retrievalManager, SuperBeans existingBeans) {
			if (existingBeans == null) {
				existingBeans = new SuperBeansImpl();
			}
			ISdmxBaseObjectRetrievalManager superBeanRetrievalManager = new InMemoryISdmxBaseObjectRetrievalManager(
					existingBeans);
	
			if (componentBean.HasCodedRepresentation()) {
				IMaintainableRefObject codelistRef = componentBean.Representation
						.RepresentationRef.MaintainableReference;
	
				LOG.Debug("get codelist super bean : " + codelistRef);
	
				ICodelistObjectBase codelistSuperBean = superBeanRetrievalManager
						.IGetCodelistObjectBase(codelistRef);
				if (codelistSuperBean == null) {
					ICodelistObject codelistBean = retrievalManager
							.GetCodelist(codelistRef);
	
					if (codelistBean == null) {
						throw new ReferenceException(
								"Can not resolve reference to codelist: "
										+ codelistRef);
					}
					LOG.Debug("no existing super bean found build new : "
							+ codelistBean.Urn);
					codelistSuperBean = codelistSuperBeanBuilder
							.Build(codelistBean);
	
					existingBeans.AddCodelist(codelistSuperBean);
				}
				return codelistSuperBean;
			}
			LOG.Debug("component is uncoded");
			return null;
		}
	
		internal IConceptObjectBase GetConcept(IComponent componentBean,
				ISdmxRetrievalManager retrievalManager, SuperBeans existingBeans) {
	
			if (existingBeans == null) {
				existingBeans = new SuperBeansImpl();
			}
			ISdmxBaseObjectRetrievalManager superBeanRetrievalManager = new InMemoryISdmxBaseObjectRetrievalManager(
					existingBeans);
	
			IConceptObject conceptBean = null;
			ICrossReference conceptRef = componentBean.ConceptRef;
	
			LOG.Debug("get concept super bean : " + conceptRef);
	
			IMaintainableRefObject conceptSchemeRef = conceptRef
					.MaintainableReference;
			String conceptId = conceptRef.ChildReference.Id;
	
			//Return an exiting super bean if you have one
			IConceptSchemeObjectBase csSuperBean = superBeanRetrievalManager
					.IGetConceptSchemeObjectBase(conceptSchemeRef);
			if (csSuperBean != null) {
				LOG.Debug("check existing concept scheme super bean : "
						+ csSuperBean.Urn);
				/* foreach */
				foreach (IConceptObjectBase concept  in  csSuperBean.Concepts) {
					if (concept.Id.Equals(conceptId)) {
						LOG.Debug("existing concept super bean found");
						return concept;
					}
				}
			}
			LOG.Debug("No existing concept super bean found, build new from concept scheme bean");
	
			//Could not find an exiting one, us
			IConceptSchemeObject conceptSchemeBean = retrievalManager
					.GetConceptScheme(conceptSchemeRef);
			if (conceptSchemeBean == null) {
				LOG.Error("Could not find concept scheme bean to build concept super bean from : "
						+ conceptSchemeRef);
				throw new ReferenceException(
						Org.Sdmxsource.Sdmx.Api.Constants.ExceptionCode.ReferenceErrorUnresolvable,
						"Concept Scheme", conceptSchemeRef);
			}
	
			/* foreach */
			foreach (IConceptObject currentConcept  in  conceptSchemeBean.Items) {
				if (currentConcept.Id.Equals(conceptId)) {
					conceptBean = currentConcept;
					break;
				}
			}
			if (conceptBean == null) {
				LOG.Error("Could not find concept bean to build concept super bean from : "
						+ conceptRef);
				throw new ReferenceException(
						Org.Sdmxsource.Sdmx.Api.Constants.ExceptionCode.ReferenceErrorUnresolvable, "Concept",
						conceptRef.ToString());
			}
			return conceptSuperBeanBuilder.Build(conceptBean, retrievalManager,
					existingBeans);
		}
	
	}
}