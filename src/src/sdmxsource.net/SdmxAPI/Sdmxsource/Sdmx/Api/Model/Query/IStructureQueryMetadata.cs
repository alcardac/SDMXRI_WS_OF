// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureQueryMetadata.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Query
{
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// Contains information about a Structure Query.  The information includes the response detail, 
    /// and which referenced artefacts should be  referenced in the response
    /// </summary>
    public interface IStructureQueryMetadata
    {
        /// <summary>
        /// Gets true if this is a query for all of the latest versions of the given artefacts
        /// </summary>
        bool IsReturnLatest { get; }


        /// <summary>
        /// Gets the query detail for this structure query, can not be null
        /// </summary>
        StructureQueryDetail StructureQueryDetail { get; }

        /// <summary>
        /// Gets the reference detail for this structure query, can not be null
        /// </summary>
        StructureReferenceDetail StructureReferenceDetail { get; }

        /// <summary>
        /// Gets the specific structure reference if STRUCTURE_REFERENCE_DETAIL == SPECIFIC, otherwise null 
        /// </summary>
        SdmxStructureType SpecificStructureReference { get; }

    }
}
