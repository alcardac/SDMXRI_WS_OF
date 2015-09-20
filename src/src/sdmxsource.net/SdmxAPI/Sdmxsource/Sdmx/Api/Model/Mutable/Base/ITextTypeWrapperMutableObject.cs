// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextTypeWrapperMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base
{
    /// <summary>
    ///     The TextTypeWrapperMutableObject interface.
    /// </summary>
    public interface ITextTypeWrapperMutableObject : IMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the locale.
        /// </summary>
        string Locale { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        string Value { get; set; }

        #endregion
    }
}