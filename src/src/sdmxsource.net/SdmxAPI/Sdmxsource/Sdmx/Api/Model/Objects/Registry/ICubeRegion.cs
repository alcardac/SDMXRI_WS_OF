// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICubeRegion.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     The CubeRegion interface.
    /// </summary>
    public interface ICubeRegion : ISdmxStructure
    {
        #region Public Properties

        /// <summary>
        ///     Gets the attribute values.
        /// </summary>
        IList<IKeyValues> AttributeValues { get; }

        /// <summary>
        ///     Gets the key values.
        /// </summary>
        IList<IKeyValues> KeyValues { get; }


        /// <summary>
        /// Gets a set of values for the given component id.  Returns an empty set if there are no values specified for the given component
        /// </summary>
        /// <param name="componentId">The component Id.</param>
        /// <returns></returns>
        ISet<string> GetValues(string componentId);

        #endregion
    }
}