// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICategoryMapMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     The CategoryMapMutableObject interface.
    /// </summary>
    public interface ICategoryMapMutableObject : IMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the alias.
        /// </summary>
        string Alias { get; set; }

        /// <summary>
        ///     Gets the source id.
        /// </summary>
        IList<string> SourceId { get; }

        /// <summary>
        ///     Gets the target id.
        /// </summary>
        IList<string> TargetId { get; }

        #endregion
    }
}