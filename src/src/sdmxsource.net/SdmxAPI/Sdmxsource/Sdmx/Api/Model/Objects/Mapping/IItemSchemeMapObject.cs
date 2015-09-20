// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IItemSchemeMapObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping
{
    #region Using directives

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     Represents an SDMX Item Scheme Map
    /// </summary>
    public interface IItemSchemeMapObject : ISchemeMapObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the items.
        /// </summary>
        IList<IItemMap> Items { get; }

        #endregion
    }
}