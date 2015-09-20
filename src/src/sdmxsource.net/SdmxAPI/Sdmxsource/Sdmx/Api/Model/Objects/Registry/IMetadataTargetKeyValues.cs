// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMetadataTargetKeyValues.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMetadataTargetKeyValues : IKeyValues
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<ICrossReference> ObjectReferences { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<IDataSetReference> DatasetReferences { get; }
    }
}
