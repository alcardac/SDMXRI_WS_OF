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
	public class DataStructureMutableObjectBaseCore : 
			MaintainableMutableObjectBaseCore, 
			IDataStructureMutableObjectBase {
	
		private const long serialVersionUID = 1L;
	
		private IList<IDimensionMutableObjectBase> _dimensions;
		private IList<IAttributeMutableObjectBase> _attributes;
		private IPrimaryMeasureMutableObjectBase _primaryMeasure;
		private IList<IGroupMutableObjectBase> _groups;
	
		public DataStructureMutableObjectBaseCore(IDataStructureObjectBase keyFamily) : base(keyFamily) {
			this._dimensions = new List<IDimensionMutableObjectBase>();
			this._attributes = new List<IAttributeMutableObjectBase>();
			this._groups = new List<IGroupMutableObjectBase>();
			if (keyFamily.Dimensions != null) {
				foreach (IDimensionObjectBase dimension  in  keyFamily.Dimensions) {
					this._dimensions.Add(new DimensionMutableObjectBaseCore(dimension));
				}
			}
			if (keyFamily.Attributes != null) {
				
				foreach (IAttributeObjectBase attributeObject  in  keyFamily.Attributes) {
					this._attributes.Add(new AttributeMutableObjectBaseCore(attributeObject));
				}
			}
			if (keyFamily.Groups != null) {
				foreach (IGroupObjectBase groupObject  in  keyFamily.Groups) {
					this._groups.Add(new GroupMutableObjectBaseCore(groupObject));
				}
			}
			if (keyFamily.PrimaryMeasure != null) {
				this._primaryMeasure = new PrimaryMeasureMutableObjectBaseCore(keyFamily.PrimaryMeasure);
			}
		}
	
		public DataStructureMutableObjectBaseCore() {
			this._dimensions = new List<IDimensionMutableObjectBase>();
			this._attributes = new List<IAttributeMutableObjectBase>();
			this._groups = new List<IGroupMutableObjectBase>();
	
		}
	
		
		public virtual IList<IDimensionMutableObjectBase> Dimensions {
		  get {
				return _dimensions;
			}
		  set {
				this._dimensions = value;
			}
		}
		
	
		
		public virtual IList<IAttributeMutableObjectBase> Attributes {
		  get {
				return _attributes;
			}
		  set {
				this._attributes = value;
			}
		}
		
	
		
		public virtual IPrimaryMeasureMutableObjectBase PrimaryMeasure {
		  get {
				return _primaryMeasure;
			}
		  set {
				this._primaryMeasure = value;
			}
		}
		
	
		
		public virtual IList<IGroupMutableObjectBase> Groups {
		  get {
				return _groups;
			}
		  set {
				this._groups = value;
			}
		}
		
	}
}
