// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReferencePeriod.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     Represents an SDMX Rererence Period
    /// </summary>
    public interface IReferencePeriod : ISdmxStructure
    {
        #region Public Properties

        /// <summary>
        ///     Gets an inclusive end time for the reference period
        /// </summary>
        /// <value> an inclusive end time for the reference period, not null </value>
        ISdmxDate EndTime { get; }

        /// <summary>
        ///     Gets an inclusive start time for the reference period
        /// </summary>
        /// <value> an inclusive start time for the reference period, not null </value>
        ISdmxDate StartTime { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets a mutable version
        /// </summary>
        /// <returns>
        ///     The <see cref="IReferencePeriodMutableObject" /> .
        /// </returns>
        IReferencePeriodMutableObject CreateMutableObject();

        #endregion
    }
}