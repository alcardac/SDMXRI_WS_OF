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
// 5/10/2012 7:20 ��    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.BaseObjects.Base {
	
	using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
	using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Globalization;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	[Serializable]
	public class AnnotationObjectBaseCore : IAnnotationObjectBase {
		private const long serialVersionUID = 1L;
	
		private String title;
		private Uri url;
		private String type;
		private IDictionary<CultureInfo, String> text;
	
		public AnnotationObjectBaseCore(IAnnotation annotation) {
			this.title = annotation.Title;
			this.url = annotation.URL;
			this.type = annotation.Type;
			this.text = Org.Sdmxsource.Sdmx.Util.Objects.LocaleUtil.BuildLocalMap(annotation.Text);
		}
	
		
		public virtual IDictionary<CultureInfo,String> Texts {
		  get {
				return new Dictionary<CultureInfo, String>(text);
			}
		}
		
	
		/* (non-Javadoc)
		 * @see org.sdmxsource.dataweb.external.model.Annotation#getText(java.util.Locale)
		 */
		public virtual String GetText(CultureInfo locale) {
			return this.title;
		}
	
		
		public virtual string Text {
		  get {
				return Org.Sdmxsource.Sdmx.Util.Objects.LocaleUtil.GetStringByDefaultLocale(text);
			}
		}
		
	
		
		public virtual String Title {
		  get {
				return this.title;
			}
		}
		
	
		
		public virtual String Type {
		  get {
				return this.type;
			}
		}
		
	
		
		public virtual Uri Url {
		  get {
				return this.url;
			}
		}
		
	}
}
