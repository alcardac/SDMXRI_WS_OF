// -----------------------------------------------------------------------
// <copyright file="CustomQueryParseManager.cs" company="Eurostat">
//   Date Created : 2013-03-28
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.CustomRequests.Manager
{
    using Estat.Sri.CustomRequests.Builder;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.Query;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;

    /// <summary>
    /// A <c>QueryStructureRequest</c> and structure query parser. Supports Constraints in <c>Dataflow</c> references in <c>SDMX</c> v2.0 <c>QueryStructureRequest</c>.
    /// </summary>
    /// <remarks>
    /// This class extends <see cref="QueryParsingManager"/> and uses the <see cref="ConstrainQueryBuilderV2"/> for SDMX v2.0 Query Structure parsing.
    /// </remarks>
    public class CustomQueryParseManager : QueryParsingManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomQueryParseManager"/> class.
        /// </summary>
        /// <param name="sdmxSchema">
        /// The SDMX schema.
        /// </param>
        public CustomQueryParseManager(SdmxSchemaEnumType sdmxSchema)
            : base(sdmxSchema, new QueryBuilder(null, new ConstrainQueryBuilderV2(), null))
        {
        }
    }
}