// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EndpointType.cs" company="Eurostat">
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
    ///     Defines the different types of SDMX services that can be accessed over a network (i.e Registry)
    /// </summary>
    public enum EndpointType
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        /// <summary>
        ///     Endpoint is a SDMX Registry, which can consume a ReigstryInterfaceDocument query
        /// </summary>
        Registry, 

        /// <summary>
        ///     ENDPOINT IS A QUERYABLE SOURCE THAT CAN TAKE A SDMX QUERY AND REPLY WITH A RESPONSE
        /// </summary>
        QueryableSource, 

        /// <summary>
        ///     ENDPOINT TYPE IS A FILE THAT IS LOCAL TO THE SYSTEM
        /// </summary>
        File, 

        /// <summary>
        ///     2.1 REST Syntax
        /// </summary>
        Rest
    }
}