// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMetadataTargetRegionMutableObject.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMetadataTargetRegionMutableObject : IMutableObject
    {
        /**
 * Returns true if the information reported is to be included or excluded 
 * @return
 */

        /// <summary>
        /// Gets true value if the information reported is to be included or excluded 
        /// </summary>
        bool IsInclude { get; }

        /// <summary>
        /// 
        /// </summary>
        string Report { get; }

        /// <summary>
        /// 
        /// </summary>
        string MetadataTarget { get; }

        /// <summary>
        /// Gets the key values restrictions for the metadata target region
        /// </summary>
        IList<IMetadataTargetKeyValuesMutable> Key { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        void AddKey(IMetadataTargetKeyValuesMutable key);

        /**
         * Returns the attributes restrictions for the metadata target region
         * @return
         */

        /// <summary>
        /// Gets the attributes restrictions for the metadata target region
        /// </summary>
        IList<IKeyValuesMutable> Attributes { get; }

        void AddAttribute(IKeyValuesMutable attribute);
    }
}
