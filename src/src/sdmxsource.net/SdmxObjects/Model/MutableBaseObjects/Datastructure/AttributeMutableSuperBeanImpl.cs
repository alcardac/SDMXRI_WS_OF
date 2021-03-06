// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.2011051901     
// 5/10/2012 7:21 ��    
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
namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.MutableBaseObjects.Datastructure {
	
	using Org.Sdmxsource.Sdmx.Api.Constants;
	using Org.Sdmxsource.Sdmx.Api.Model.MutableBaseObjects.Datastructure;
	using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Datastructure;
	using Org.Sdmxsource.Sdmx.SdmxObjects.Model.MutableBaseObjects.Base;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	[Serializable]
	public class AttributeMutableObjectBaseCore : 
			ComponentMutableObjectBaseCore, IAttributeMutableObjectBase {
		private const long serialVersionUID = 1L;
	
		private string _assignmentStatus;
		private AttributeAttachmentLevel _attachmantLevel;
		private string _attachmentGroup;
		private IList<string> _attachmentMeasure;
		private bool _isCountAttribute;
		private bool _isEntityAttribute;
		private bool _isFrequencyAttribute;
		private bool _isIdentityAttribute;
		private bool _isMandatory;
		private bool _isNonObservationTimeAttribute;
		private bool _isTimeFormat;
	
		public AttributeMutableObjectBaseCore(IAttributeObjectBase attributeObject) : base(attributeObject) {
			//TODO Implement
			this._assignmentStatus = attributeObject.AssignmentStatus;
			this._attachmantLevel = attributeObject.AttachmentLevel;
			this._isMandatory = attributeObject.Mandatory;
		}
	
		public AttributeMutableObjectBaseCore() {
		}
	
		
		public string AssignmentStatus {
		  get {
				return _assignmentStatus;
			}
		  set {
				this._assignmentStatus = value;
			}
		}
		
	
		
		public AttributeAttachmentLevel AttachmantLevel {
		  get {
				return _attachmantLevel;
			}
		  set {
				this._attachmantLevel = value;
			}
		}
		
	
		
		public string AttachmentGroup {
		  get {
				return _attachmentGroup;
			}
		  set {
				this._attachmentGroup = value;
			}
		}
		
	
		
		public IList<string> AttachmentMeasure {
		  get {
				return _attachmentMeasure;
			}
		  set {
				this._attachmentMeasure = value;
			}
		}
		
	
		
		public bool CountAttribute {
		  get {
				return _isCountAttribute;
			}
		  set {
				this._isCountAttribute = value;
			}
		}
		
	
		
		public bool EntityAttribute {
		  get {
				return _isEntityAttribute;
			}
		  set {
				this._isEntityAttribute = value;
			}
		}
		
	
		
		public bool FrequencyAttribute {
		  get {
				return _isFrequencyAttribute;
			}
		  set {
				this._isFrequencyAttribute = value;
			}
		}
		
	
		
		public bool IdentityAttribute {
		  get {
				return _isIdentityAttribute;
			}
		  set {
				this._isIdentityAttribute = value;
			}
		}
		
	
		
		public bool Mandatory {
		  get {
				return _isMandatory;
			}
		  set {
				this._isMandatory = value;
			}
		}
		
	
		
		public bool NonObservationTimeAttribute {
		  get {
				return _isNonObservationTimeAttribute;
			}
		  set {
				this._isNonObservationTimeAttribute = value;
			}
		}
		
	
		
		public bool TimeFormat {
		  get {
				return _isTimeFormat;
			}
		  set {
				this._isTimeFormat = value;
			}
		}
		
	}
}
