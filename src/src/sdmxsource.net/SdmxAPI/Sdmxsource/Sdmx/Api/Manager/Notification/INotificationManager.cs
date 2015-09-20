// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INotificationManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects;

    /// <summary>
    /// Used to performed notifications of events for Subscriptions
    /// </summary>
    public interface INotificationManager
    {
        
        /// <summary>
        /// Sends notifications for each subscription that matches the structure - handles its own exceptions
        /// </summary>
        /// <param name="sdmxObjects">New Structures</param>
        void SendNotification(ISdmxObjects sdmxObjects);
    }
}
