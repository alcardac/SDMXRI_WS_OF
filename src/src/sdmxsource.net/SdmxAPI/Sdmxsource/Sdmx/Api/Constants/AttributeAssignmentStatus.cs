// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeAssignmentStatus.cs" company="Eurostat">
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
    ///     Contains the attribute assignment options of Optional and Mandatory
    /// </summary>
    public enum AttributeAssignmentStatus
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        /// <summary>
        ///     The mandatory.
        /// </summary>
        Mandatory, 

        /// <summary>
        ///     The conditional.
        /// </summary>
        Conditional
    }
}