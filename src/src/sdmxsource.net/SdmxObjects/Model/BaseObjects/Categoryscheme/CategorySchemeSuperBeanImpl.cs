/*******************************************************************************
 * Copyright (c) 2012 Metadata Technology Ltd.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the GNU Public License v3.0
 * which accompanies this distribution, and is available at
 * http://www.gnu.org/licenses/gpl.html
 * 
 * This file is part of the SDMX Component Library.
 * 
 * The SDMX Component Library is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * The SDMX Component Library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with The SDMX Component Library If not, see <http://www.gnu.org/licenses/>.
 * 
 * Contributors:
 *     Metadata Technology - initial API and implementation
 ******************************************************************************/
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.2011051901     
// 5/10/2012 7:21 ��    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------

using Org.Sdmxsource.Sdmx.Api.Model.Objects.Datastructure;

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.BaseObjects.Categoryscheme {
	
	using Org.Sdmxsource.Sdmx.Api.Model.Objects.Categoryscheme;
	using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Categoryscheme;
	using Org.Sdmxsource.Sdmx.SdmxObjects.Model.BaseObjects.Base;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	[Serializable]
	public class CategorySchemeObjectBaseCore : MaintainableObjectBaseCore
		, ICategorySchemeObjectBase {
		private const long serialVersionUID = 1L;
	
		private IList<ICategoryObjectBase> categories;
	
		private ICategorySchemeObject builtFrom;
	
		public CategorySchemeObjectBaseCore(ICategorySchemeObject categoryScheme) : base(categoryScheme) {
			this.builtFrom = categoryScheme;
			categories = new List<ICategoryObjectBase>();
			if (categoryScheme.Items != null) {
				/* foreach */
				foreach (ICategoryObject currentCategory  in  categoryScheme.Items) {
					ICategoryObjectBase category = new CategoryObjectBaseCore(this,
							currentCategory, null);
					categories.Add(category);
				}
			}
		}
	
		
		public override IAttribute BuiltFrom {
		  get {
				return builtFrom;
			}
		}
		
	
		
		public virtual IList<ICategoryObjectBase> Categories {
		  get {
				return new List<ICategoryObjectBase>(categories);
			}
		}
		
	}
}
