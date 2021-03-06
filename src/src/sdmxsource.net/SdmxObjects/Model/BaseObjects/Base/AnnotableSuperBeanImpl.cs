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
 namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.BaseObjects.Base {
	
	using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
	using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	[Serializable]
	public abstract class AnnotableObjectBaseCore : ObjectBaseCore, 
			IAnnotableObjectBase {
		private const long serialVersionUID = 1L;
	
		private readonly ISet<IAnnotationObjectBase> _annotations;
		private readonly IDictionary<String, IAnnotationObjectBase> _annotationByType;
	
		public AnnotableObjectBaseCore(IAnnotableObject annotableType) : base(annotableType) {
			this._annotations = new HashSet<IAnnotationObjectBase>();
			this._annotationByType = new Dictionary<String, IAnnotationObjectBase>();
			if (annotableType != null && annotableType.Annotations != null) {
				/* foreach */
				foreach (IAnnotation currentAnnotation  in  annotableType
						.Annotations) {
					IAnnotationObjectBase annotation = new AnnotationObjectBaseCore(
							currentAnnotation);
					_annotations.Add(annotation);
					if (Org.Sdmxsource.Util.ObjectUtil.ValidString(annotation.Type)) {
						_annotationByType.Add(annotation.Type, annotation);
					}
				}
			}
		}
	
		/* (non-Javadoc)
		 * @see org.sdmxsource.dataweb.external.model.Annotable#getAnnotationByTitle(java.lang.String)
		 */
		public virtual ISet<IAnnotationObjectBase> GetAnnotationByTitle(String title) {
			ISet<IAnnotationObjectBase> returnSet = new HashSet<IAnnotationObjectBase>();
			if (HasAnnotations()) {
				/* foreach */
				foreach (IAnnotationObjectBase currentAnnotation  in  _annotations) {
					if (currentAnnotation.Title != null
							&& currentAnnotation.Title.Equals(title)) {
						returnSet.Add(currentAnnotation);
					}
				}
			}
			return returnSet;
		}
	
		/* (non-Javadoc)
		 * @see org.sdmxsource.dataweb.external.model.Annotable#getAnnotationByType(java.lang.String)
		 */
		public virtual IAnnotationObjectBase GetAnnotationByType(String type) {
			return _annotationByType[type];
		}
	
		/* (non-Javadoc)
		 * @see org.sdmxsource.dataweb.external.model.Annotable#getAnnotationByUrl(java.lang.String)
		 */
		public virtual ISet<IAnnotationObjectBase> GetAnnotationByUrl(String url) {
			ISet<IAnnotationObjectBase> returnSet = new HashSet<IAnnotationObjectBase>();
			if (HasAnnotations()) {
				foreach (IAnnotationObjectBase currentAnnotation  in  _annotations) {
					if (currentAnnotation.Url != null
							&& currentAnnotation.Url.ToString().Equals(url)) {
						returnSet.Add(currentAnnotation);
					}
				}
			}
			return returnSet;
		}
	
		
		public virtual ISet<IAnnotationObjectBase> Annotations {
		  get {
				return new HashSet<IAnnotationObjectBase>(this._annotations);
			}
		}
		
	
		/* 
		 * @see org.sdmxsource.dataweb.external.model.Annotable#hasAnnotations()
		 */
		public virtual bool HasAnnotations() {
			return this._annotations != null && this._annotations.Count > 0;
		}
	}
}
