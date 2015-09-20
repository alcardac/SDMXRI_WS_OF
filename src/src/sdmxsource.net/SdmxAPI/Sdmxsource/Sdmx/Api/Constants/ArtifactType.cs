// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArtifactType.cs" company="Eurostat">
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
    ///     Contains a high level set of SDMX Artifact types
    /// </summary>
    public enum ArtifactType
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        /// <summary>
        ///     Relates to SDMX Data Messages
        /// </summary>
        Data, 

        /// <summary>
        ///     Relates to SDMX Data Metadata Set Messages
        /// </summary>
        Metadata, 

        /// <summary>
        ///     Relates to SDMX Provision Messages (only relevant in SDMX v2.0)
        /// </summary>
        Provision, 

        /// <summary>
        ///     Relates to SDMX Registration Messages (Could be Submission or Query Response)
        /// </summary>
        Registration, 

        /// <summary>
        ///     Relates to SDMX Structure Messages including Registry Interface submissions and query structure responses
        /// </summary>
        Structure, 

        /// <summary>
        ///     Relates to SDMX Notification Messages
        /// </summary>
        Notification, 

        /// <summary>
        ///     Relates to SDMX Subscription Messages - either submission or query response
        /// </summary>
        Subscription
    }
}