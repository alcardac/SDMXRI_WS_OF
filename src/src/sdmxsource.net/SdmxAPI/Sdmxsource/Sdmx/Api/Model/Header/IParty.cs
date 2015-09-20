// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IParty.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Header
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     A party represents a individual or group and can have additional contact information
    /// </summary>
    public interface IParty
    {
        #region Public Properties

        /// <summary>
        ///     Gets the contacts.
        /// </summary>
        IList<IContact> Contacts { get; }

        /// <summary>
        ///     Gets the id.
        /// </summary>
        string Id { get; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        IList<ITextTypeWrapper> Name { get; }

        /// <summary>
        ///     Gets the time zone.
        /// </summary>
        string TimeZone { get; }

        #endregion
    }
}