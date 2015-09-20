// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComplexStructureQuery.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex
{
    /// <summary>
    /// A ComplexStructureQuery defines very complex search rules for finding structures
    /// </summary>
    public interface IComplexStructureQuery
    {
        #region Public Properties

        /// <summary>
        /// Gets the information about the response detail, and which referenced artefacts should be referenced in the response
        /// </summary>
        IComplexStructureQueryMetadata StructureQueryMetadata { get; }

        /// <summary>
        /// Gets the query parameters used to identify the structure(s) begin queried for - this can not be null
        /// </summary>
        IComplexStructureReferenceObject StructureReference { get; }

        #endregion
    }
}
