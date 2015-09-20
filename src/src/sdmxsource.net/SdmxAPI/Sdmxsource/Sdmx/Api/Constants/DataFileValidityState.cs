// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataFileValidityState.cs" company="EUROSTAT">
//   Date Created : 2014-03-20
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The data file validity state.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Api.Constants
{
    /// <summary>
    ///     The data file validity state.
    /// </summary>
    public enum DataFileValidityState
    {
        /// <summary>
        ///     The unset
        /// </summary>
        Unset, 

        /// <summary>
        ///     The valid
        /// </summary>
        Valid, 

        /// <summary>
        ///     The valid including mandatory attributes
        /// </summary>
        ValidIncMandatoryAttributes, 

        /// <summary>
        ///     The invalid
        /// </summary>
        Invalid
    }
}