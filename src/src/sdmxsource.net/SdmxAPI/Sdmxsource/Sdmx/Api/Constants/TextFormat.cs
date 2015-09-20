// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextFormat.cs" company="Eurostat">
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
    ///     Contains the SDMX Text Formats
    /// </summary>
    public enum TextFormat
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        /// <summary>
        ///     The text type.
        /// </summary>
        TextType, 

        /// <summary>
        ///     The is sequence.
        /// </summary>
        IsSequence, 

        /// <summary>
        ///     The min length.
        /// </summary>
        MinLength, 

        /// <summary>
        ///     The max length.
        /// </summary>
        MaxLength, 

        /// <summary>
        ///     The start value.
        /// </summary>
        StartValue, 

        /// <summary>
        ///     The end value.
        /// </summary>
        EndValue, 

        /// <summary>
        ///     The interval.
        /// </summary>
        Interval, 

        /// <summary>
        ///     The time interval.
        /// </summary>
        TimeInterval, 

        /// <summary>
        ///     The decimals.
        /// </summary>
        Decimals, 

        /// <summary>
        ///     The pattern.
        /// </summary>
        Pattern
    }
}