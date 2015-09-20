// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReferencePeriodMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry
{
    #region Using directives

    using System;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     The ReferencePeriodMutableObject interface.
    /// </summary>
    public interface IReferencePeriodMutableObject : IMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the end time.
        /// </summary>
        DateTime? EndTime { get; set; }

        /// <summary>
        ///     Gets or sets the start time.
        /// </summary>
        DateTime? StartTime { get; set; }

        #endregion
    }
}