// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRestStructureQuery.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Query
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    /// Represents a query for Maintainable Structure(s) and maps to the SDMX REST query
    /// </summary>
    public interface IRestStructureQuery
    {
        #region Public Properties

        /// <summary>
        /// Returns information about the response detail, and which referenced artefacts should be  referneced in the response
        /// </summary>
        IStructureQueryMetadata StructureQueryMetadata
        {
            get;
        }

        /// <summary>
        /// Returns the structure reference, which defines the agency, id, version and structure type of the structure being queried
        /// </summary>
        IStructureReference StructureReference
        {
            get;
        }

        #endregion
    }
}
