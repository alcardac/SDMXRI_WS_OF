// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.2011051901     
// 5/10/2012 7:20 ��    
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
namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base {
	
	using ILOG.J2CsMapping.Util;
	using Org.Sdmxsource.Sdmx.Api.Constants;
	using Org.Sdmxsource.Sdmx.Api.Model.Base;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	[Serializable]
	public class SdmxDateImpl : ISdmxDate {
		private const long serialVersionUID = 889352854909061494L;
		private DateTime date;
		private TimeFormat timeFormat;
		private String dateInSdmx;
	
		public SdmxDateImpl(DateTime date0, TimeFormat timeFormat1) {
			this.date = date0;
			this.timeFormat = timeFormat1;
			dateInSdmx = Org.Sdmxsource.Sdmx.Util.Date.DateUtil.FormatDate(date0, timeFormat1);
		}
	
		public SdmxDateImpl(String dateInSdmx0) {
			this.date = Org.Sdmxsource.Sdmx.Util.Date.DateUtil.FormatDate(dateInSdmx0);
			this.timeFormat = Org.Sdmxsource.Sdmx.Util.Date.DateUtil.GetTimeFormatOfDate(dateInSdmx0);
			this.dateInSdmx = dateInSdmx0;
		}
	
		public virtual bool IsLater(ISdmxDate date0) {
			return (this.Date.Ticks/10000) > (date0.Date.Ticks/10000);
		}
	
		
		public virtual DateTime Date {
		  get {
				return new DateTime(((date.Ticks/10000))*10000);
			}
		}
		
	
		
		public virtual TimeFormat TimeFormatOfDate {
		  get {
				return timeFormat;
			}
		}
		
	
		
		public virtual String DateInSdmxFormat {
		  get {
				return dateInSdmx;
			}
		}
		
	
		
		public virtual Calendar DateAsCalendar {
		  get {
				return Org.Sdmxsource.Sdmx.Util.Date.DateUtil.CreateCalendar(date);
			}
		}
		
	
		public override int GetHashCode() {
			return dateInSdmx.HashCode;
		}
	
		public override bool Equals(Object obj) {
			if (obj  is  SdmxDate) {
				SdmxDate that = (SdmxDate) obj;
				return that.DateInSdmxFormat
						.Equals(this.DateInSdmxFormat);
			}
			return false;
		}
	
	}
}
