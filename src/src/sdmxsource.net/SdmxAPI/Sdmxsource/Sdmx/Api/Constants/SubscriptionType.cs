﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubscriptionType.cs" company="Eurostat">
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
    ///     The subscription type.
    /// </summary>
    public enum SubscriptionType
    {
        /// <summary>
        ///     The data registration.
        /// </summary>
        DataRegistration, 

        /// <summary>
        ///     The metadata registration.
        /// </summary>
        MetadataRegistration, 

        /// <summary>
        ///     The structure.
        /// </summary>
        Structure
    }

    /// <summary>
    ///     The subscription Object constants.
    /// </summary>
    public static class SubscriptionBeanConstants
    {
        #region Constants

        /// <summary>
        ///     The wildcard.
        /// </summary>
        public const string Wildcard = "%";

        #endregion
    }
}