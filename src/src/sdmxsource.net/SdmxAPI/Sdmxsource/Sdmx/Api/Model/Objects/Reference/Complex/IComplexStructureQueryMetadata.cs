// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComplexStructureQueryMetadata.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    #endregion

    /// <summary>
    /// Contains information about a Complex Structure Query.  The information includes the response detail, 
    /// and which referenced artefacts should be referenced in the response
    /// </summary>
    public interface IComplexStructureQueryMetadata
    {
        #region Public Methods and Operators

        /// <summary>
        /// Return true if the matched artefact should be returned in the query result besides the referenced artefacts.
        /// </summary>
        /// <returns>
        /// The return value
        /// </returns>
        bool IsReturnedMatchedArtefact();

        /// <summary>
        /// Returns the query detail for this structure query, can not be null
        /// </summary>
        ComplexStructureQueryDetail StructureQueryDetail
        {
            get;
        }

        /// <summary>
        /// Returns the query details for the resolved references, can not be null
        /// </summary>
        ComplexMaintainableQueryDetail ReferencesQueryDetail
        {
            get;
        }

        /// <summary>
        /// Returns the reference detail for this structure query, can not be null
        /// </summary>
        StructureReferenceDetail StructureReferenceDetail
        {
            get;
        }

        /// <summary>
        /// If STRUCTURE_REFERENCE_DETAIL == SPECIFIC, this method will return the specific structures for getting references, returns null otherwise
        /// </summary>
        IList<SdmxStructureType> ReferenceSpecificStructures
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the attribute processConstraints is set to true. Triggers potential creation of partial structures.
        /// </summary>
        /// <value><c>true</c> if the attribute processConstraints is set to true. otherwise, <c>false</c>.</value>
        bool IsProcessConstraints { get; }

        #endregion
    }
}
