// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IKeyValues.cs" company="Eurostat">
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
    ///     Represents an SDMX IKeyValues as defined in a SDMX ContentConstraint
    /// </summary>
    public interface IKeyValues : ISdmxStructure
    {
        #region Public Properties

        /// <summary>
        ///     Gets the identifier of the component that the values are for
        /// </summary>
        /// <value> </value>
        string Id { get; }

        /// <summary>
        ///     Gets the time range that is being constrained, this is mutually exclusive with the <see cref="Values"/>
        /// </summary>
        /// <value> </value>
        ITimeRange TimeRange { get; }

        /// <summary>
        ///     Gets a copy of the list of the allowed / disallowed values in the constraint.
        ///     <p />
        ///     This is mutually exclusive with the <see cref="TimeRange"/> method, and will return an empty list if <see cref="TimeRange"/> returns a non-null value
        /// </summary>
        /// <value> </value>
        IList<string> Values { get; }

        /// <summary>
        /// Gets the cascade values.
        /// </summary>
        /// <value>
        /// Returns a list of all the values which are cascade values
        /// </value>
        IList<string> CascadeValues { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Where the values are coded, and the codes have child codes, if a value if to be cascaded, then include all the child
        /// codes in the constraint for inclusion / exclusion
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// true if the value is set to be cascaded
        /// </returns>
        bool IsCascadeValue(string value);

        #endregion
    }
}