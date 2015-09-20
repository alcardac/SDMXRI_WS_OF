// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMaintainableRefObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference
{
    /// <summary>
    ///     The MaintainableRefObject interface.
    /// </summary>
    public interface IMaintainableRefObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the maintainable id attribute
        /// </summary>
        /// <value> </value>
        string AgencyId { get; }

        /// <summary>
        ///     Gets the maintainable id attribute
        /// </summary>
        /// <value> </value>
        string MaintainableId { get; }

        /// <summary>
        ///     Gets the version attribute
        /// </summary>
        /// <value> </value>
        string Version { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets a value indicating whether the there is an agency Id set
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasAgencyId();

        /// <summary>
        ///     Gets a value indicating whether the there is a maintainable id set
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasMaintainableId();

        /// <summary>
        ///     Gets a value indicating whether the there is a value for version set
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasVersion();

        #endregion
    }
}