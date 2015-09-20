// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMetadataTargetRegion.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMetadataTargetRegion : ISdmxStructure
    {
        /// <summary>
        /// Gets true if the information reported is to be included or excluded 
        /// </summary>
        bool IsInclude { get; }

        /// <summary>
        /// Gets the report
        /// </summary>
        String Report { get; }

        /// <summary>
        /// Gets the metadata target
        /// </summary>
        String MetadataTarget { get; }

        /// <summary>
        /// Gets the key values restrictions for the metadata target region
        /// </summary>
        IList<IMetadataTargetKeyValues> Key { get; }

        /// <summary>
        /// Gets the attributes restrictions for the metadata target region
        /// </summary>
        IList<IKeyValues> Attributes { get; }
    }
}
