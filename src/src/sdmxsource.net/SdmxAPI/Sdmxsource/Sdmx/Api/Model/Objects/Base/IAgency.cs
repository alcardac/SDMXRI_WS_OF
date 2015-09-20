// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAgency.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Base
{
    /// <summary>
    ///     Represents an SDMX Agency
    /// </summary>
    public interface IAgency : IOrganisation
    {
        #region Public Properties

        /// <summary>
        ///     Gets the id of the agency scheme concatenated with the id of the agency, for all agencies that are not in the default scheme.
        ///     If an Agency is in the default scheme, this method will just return the agencies' id.
        /// </summary>
        /// <value> </value>
        string FullId { get; }

        /// <summary>
        /// Gets the Maintainable Parent
        /// </summary>
        IAgencyScheme GetMaintainableParent { get; }

        #endregion
    }
}