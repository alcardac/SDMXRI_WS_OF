// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrganisation.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Base
{
    using System.Collections.Generic;

    /// <summary>
    ///     Represents an SDMX organisation such as an Agency, DataProvider or DataConsumer
    /// </summary>
    public interface IOrganisation : IItemObject
    {
        /// <summary>
        /// Gets a list of contacts for the organisation
        /// </summary>
        IList<IContact> Contacts { get; }
    }
}