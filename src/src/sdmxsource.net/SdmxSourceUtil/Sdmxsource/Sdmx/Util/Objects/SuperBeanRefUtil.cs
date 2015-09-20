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

using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

namespace Org.Sdmxsource.Sdmx.Util.Objects {
	
	using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
	using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
	using Objects.Reference;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// This class provides utility methods that can be performed on a MaintainableSuperBean
	/// </summary>
	///
	public class SuperBeanRefUtil<T> where T : IMaintainableObjectBase  {
	
		/// <summary>
		/// For a set of maintainables, this method will return the maintainable that has the same agencyId, Id and version as the reference bean.
		/// <p/>
		/// If the ref bean does not have a agency id, id and then it will result in an error.  If version information is missing, then the latest version
		/// will be assumed.
		/// <p/>
		/// This method will stop at the first match and return it, no checks are performed on the type of maintainables in the set.
		/// <p/>
		/// If no match is found null is returned.  
		/// </summary>
		///
		/// <param name="maintianables"></param>
		/// <param name="ref"></param>
		/// <returns></returns>
		public static IMaintainableObjectBase ResolveReference<T>(ICollection<T> maintianables, IMaintainableRefObject xref) where T : IMaintainableObjectBase
        {
			if (xref == null) {
				throw new ArgumentException("Ref is null");
			}
			if (!xref.HasAgencyId()) {
				throw new ArgumentException("Ref is mising AgencyId");
			}
			if (!xref.HasMaintainableId()) {
				throw new ArgumentException("Ref is mising Id");
			}

            IMaintainableObjectBase latestVersion = null;
			if (maintianables != null) {

                foreach (IMaintainableObjectBase currentMaintainable in maintianables)
                {
					if (currentMaintainable.AgencyId.Equals(xref.AgencyId)) {
						if (currentMaintainable.Id.Equals(xref.MaintainableId)) {
							if (!xref.HasVersion()) {
								if (latestVersion == null
										|| Org.Sdmxsource.Util.VersionableUtil.IsHigherVersion(
												currentMaintainable.Version,latestVersion.Version)) {
									latestVersion = currentMaintainable;
								}
							} else if (currentMaintainable.Version.Equals(xref.Version)) {
								return currentMaintainable;
							}
						}
					}
				}
			}
			return latestVersion;
		}
	
		public ISet<T> ResolveReferences(ICollection<T> maintianables,
				IMaintainableRefObject xref) {
			ISet<T> returnSet = new HashSet<T>();
	
			if (xref == null) {
				xref = new MaintainableRefObjectImpl();
			}
			bool hasAgencyFilter = Org.Sdmxsource.Util.ObjectUtil.ValidString(xref.AgencyId);
			bool hasIdFilter = Org.Sdmxsource.Util.ObjectUtil.ValidString(xref.MaintainableId);
			bool hasVersionFilter = Org.Sdmxsource.Util.ObjectUtil.ValidString(xref.Version);
	
			String agencyId = xref.AgencyId;
			String id = xref.MaintainableId;
			String version = xref.Version;
	
			if (maintianables != null) {
				foreach (T currentMaintainable  in  maintianables) {
					if (hasAgencyFilter
							&& !agencyId.Equals(currentMaintainable.AgencyId)) {
						continue;
					}
					if (hasIdFilter && !id.Equals(currentMaintainable.Id)) {
						continue;
					}
					if (hasVersionFilter
							&& !version.Equals(currentMaintainable.Version)) {
						continue;
					}
					ILOG.J2CsMapping.Collections.Generics.Collections.Add(returnSet,currentMaintainable);
				}
			}
			return returnSet;
		}
	}
}
