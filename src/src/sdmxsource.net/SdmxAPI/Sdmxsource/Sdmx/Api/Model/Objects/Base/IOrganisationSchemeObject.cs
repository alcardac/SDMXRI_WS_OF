// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrganisationSchemeObject.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    /// <summary>
	/// TODO: Update summary.
	/// </summary>
	public interface IOrganisationScheme<T> : IItemSchemeObject<T>
        where T: IItemObject
	{
		/// <summary>
		/// Gets a representation of itself in a bean which can be modified, modifications to the mutable bean
		/// are not reflected in the instance of the MaintainableObject.
		/// </summary>
        new IOrganisationSchemeMutableObject<IOrganisationScheme<T>, IOrganisationMutableObject> MutableInstance { get; }
	}
	
}
