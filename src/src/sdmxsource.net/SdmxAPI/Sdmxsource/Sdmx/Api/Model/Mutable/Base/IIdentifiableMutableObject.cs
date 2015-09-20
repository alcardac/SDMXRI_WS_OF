// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIdentifiableMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base
{
    using System;

    /// <summary>
    ///     The IdentifiableMutableObject interface.
    /// </summary>
    public interface IIdentifiableMutableObject : IAnnotableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        ///     Gets or sets the uri.
        /// </summary>
        Uri Uri { get; set; }

        /// <summary>
        ///     Gets the urn.
        /// </summary>
        Uri Urn { get; }

        #endregion
    }
}