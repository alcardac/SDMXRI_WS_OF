// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToValue.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Constants
{
    /// <summary>
    ///     The to value.
    /// </summary>
    public enum ToValue
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        /// <summary>
        ///     The value.
        /// </summary>
        Value, 

        /// <summary>
        ///     The name.
        /// </summary>
        Name, 

        /// <summary>
        ///     The description.
        /// </summary>
        Description
    }
}