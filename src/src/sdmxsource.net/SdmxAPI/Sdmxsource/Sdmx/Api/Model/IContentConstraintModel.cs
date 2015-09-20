// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContentConstraintModel.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IContentConstraintModel
    {
        /// <summary>
        /// Returns true if the observation is valid with respect to the constraint
        /// </summary>
        /// <param name="obs"></param>
        /// <returns></returns>
        bool IsValidObservation(IObservation obs);

        /// <summary>
        /// Returns true if the key is valid with respect to the constraint
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsValidKey(IKeyable key);

        /// <summary>
        /// Returns true if the key value pair are valid with respect to the constraint
        /// </summary>
        /// <param name="kv"></param>
        /// <returns></returns>
        bool IsValidKeyValue(IKeyValue kv);
    }
}
