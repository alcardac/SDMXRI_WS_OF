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
namespace Org.Sdmxsource.Sdmx.Structureretrieval.Manager.Mutable {
	
	using System.Collections.Generic;
	using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
	using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
	using Org.Sdmxsource.Sdmx.Api.Model.Objects;
	using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
	using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
	using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
	using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference;
	using Org.Springframework.Objects.Factory.Annotation;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	public class MutableStubBeanRetrievalManagerImpl : 
			MutableStubBeanRetrievalManager {
	
		private IStructureSearchManager structureSearchManager;
		private IServiceRetrievalManager serviceRetrievalManager;
	
		public virtual ISet<IMaintainableMutableObject> GetStubBeans(IStructureReference sRef) {
			ISdmxObjects beans = structureSearchManager
					.GetMaintainables(new RESTStructureQuery(sRef));
	
			ISet<IMaintainableMutableObject> mutableBeans = new HashSet<IMaintainableMutableObject>();
	
			/* foreach */
			foreach (IMaintainableObject maintainableBean  in  beans.GetAllMaintinables()) {
				mutableBeans.Add(serviceRetrievalManager.CreateStub(
									maintainableBean).MutableInstance);
			}
			return mutableBeans;
		}
	
		//Spring Controlled
		public void SetStructureSearchManager(
				IStructureSearchManager structureSearchManager0) {
			this.structureSearchManager = structureSearchManager0;
		}
	
		public void SetServiceRetrievalManager(
				IServiceRetrievalManager serviceRetrievalManager0) {
			this.serviceRetrievalManager = serviceRetrievalManager0;
		}
	}
}
