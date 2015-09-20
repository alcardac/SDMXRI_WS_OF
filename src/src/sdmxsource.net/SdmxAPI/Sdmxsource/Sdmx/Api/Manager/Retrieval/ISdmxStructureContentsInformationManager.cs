// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxStructureContentsInformationManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;

    /// <summary>
    /// Provides meta information about the contents of a structure source
    /// </summary>
    public interface ISdmxStructureContentsInformationManager
    {
        /// <summary>
        /// Returns a count of maintainable structures for the given structure type and agency.
        /// </summary>
        /// <param name="agencyId">The agency to count the structures for</param>
        /// <param name="structureType">The maintainable structure type</param>
        /// <returns>A count of the number of maintainables for the given structure type and agency.</returns>
        int GetCountOfMaintainables(String agencyId, SdmxStructureType structureType);

        /// <summary>
        /// Gets a list of all available languages available at this structure source
        /// </summary>
        IList<string> AllLanguages { get; }
        
        /// <summary>
        /// Gets a List of AgencyMetadata objects containing information about the Agencies in the system
        /// </summary>
        IList<IAgencyMetadata> AllAgencies { get; }
    }
}
