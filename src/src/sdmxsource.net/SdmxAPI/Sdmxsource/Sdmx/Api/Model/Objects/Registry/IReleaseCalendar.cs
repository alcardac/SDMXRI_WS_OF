// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReleaseCalendar.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     The ReleaseCalendar interface.
    /// </summary>
    public interface IReleaseCalendar : ISdmxStructure
    {
        #region Public Properties

        /// <summary>
        ///     Gets the offset.
        /// </summary>
        string Offset { get; }

        /// <summary>
        ///     Gets the periodicity.
        /// </summary>
        string Periodicity { get; }

        /// <summary>
        ///     Gets the tolerance.
        /// </summary>
        string Tolerance { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The create mutable object.
        /// </summary>
        /// <returns>
        ///     The <see cref="IReleaseCalendarMutableObject" /> .
        /// </returns>
        IReleaseCalendarMutableObject CreateMutableObject();

        #endregion
    }
}