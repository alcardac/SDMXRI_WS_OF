// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxObjectsInfo.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects
{
    #region Using directives

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     The SdmxObjectsInfo interface.
    /// </summary>
    public interface ISdmxObjectsInfo
    {
        #region Public Properties

        /// <summary>
        ///     Gets the list of agency metadata belonging to the Objects info
        /// </summary>
        /// <value> </value>
        IList<IAgencyMetadata> AgencyMetadata { get; }

        /// <summary>
        ///     Gets the number maintainables.
        /// </summary>
        int NumberMaintainables { get; }

        #endregion
    }
}