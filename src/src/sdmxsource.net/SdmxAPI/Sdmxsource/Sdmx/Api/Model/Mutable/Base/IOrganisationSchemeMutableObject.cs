// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrganisationSchemeMutableObject.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IOrganisationSchemeMutableObject<out TImmutable, TMutable> : IItemSchemeMutableObject<TMutable>
        where TImmutable : IMaintainableObject
        where TMutable : IItemMutableObject
    {
        /// <summary>
        /// Returns a representation of itself in a bean which can not be modified, modifications to the mutable bean
        /// after this point will not be reflected in the returned instance of the MaintainableObject.
        /// </summary>
        //new IOrganisationScheme ImmutableInstance { get; }
        new TImmutable ImmutableInstance { get; }
    }
}
