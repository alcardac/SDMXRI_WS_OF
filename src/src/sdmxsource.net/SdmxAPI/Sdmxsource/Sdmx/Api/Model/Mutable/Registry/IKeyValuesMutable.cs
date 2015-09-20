// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IKeyValuesMutable.cs" company="Eurostat">
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

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     The KeyValuesMutable interface.
    /// </summary>
    public interface IKeyValuesMutable : IMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the cascade.
        /// </summary>
        IList<string> Cascade { get; }

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        ///     Gets the key values.
        /// </summary>
        IList<string> KeyValues { get; }

        /// <summary>
        ///     Gets or sets the time range.
        /// </summary>
        ITimeRangeMutableObject TimeRange { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add cascade.
        /// </summary>
        /// <param name="value_ren">
        /// The value_ren.
        /// </param>
        void AddCascade(string value_ren);

        /// <summary>
        /// The add value.
        /// </summary>
        /// <param name="value_ren">
        /// The value_ren.
        /// </param>
        void AddValue(string value_ren);

        /// <summary>
        /// The is cascade value.
        /// </summary>
        /// <param name="value_ren">
        /// The value_ren.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        bool IsCascadeValue(string value_ren);

        #endregion
    }
}