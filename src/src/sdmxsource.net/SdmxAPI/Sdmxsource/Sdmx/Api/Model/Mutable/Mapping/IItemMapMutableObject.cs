// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IItemMapMutableObject.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     The ItemMapMutableObject interface.
    /// </summary>
    public interface IItemMapMutableObject : IMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the source id.
        /// </summary>
        string SourceId { get; set; }

        /// <summary>
        ///     Gets or sets the target id.
        /// </summary>
        string TargetId { get; set; }

        #endregion
    }
}