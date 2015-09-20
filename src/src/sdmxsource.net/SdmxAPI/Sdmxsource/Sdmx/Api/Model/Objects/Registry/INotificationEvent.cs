// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INotificationEvent.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry
{
    #region Using directives

    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    #endregion

    /// <summary>
    ///     Defines a notification event sent out from a registry.  The notification contains reference to the attachmentConstraint that
    ///     caused the notification to occur.  In the case of an identifiable the attachmentConstraint urn is provided, from which the attachmentConstraint
    ///     can be queries.  In the case of a Registration, which has no URN the registration itself is sent inside the NotificationEvent.
    /// </summary>
    public interface INotificationEvent
    {
        #region Public Properties

        /// <summary>
        ///     Gets the action on the attachmentConstraint that this event was triggered from
        /// </summary>
        /// <value> </value>
        DatasetAction Action { get; }

        /// <summary>
        ///     Gets the structures contained in this subscription
        /// </summary>
        /// <value> </value>
        ISdmxObjects Objects { get; }

        /// <summary>
        ///     Gets the time that the event was created
        /// </summary>
        /// <value> </value>
        DateTime? EventTime { get; }

        /// <summary>
        ///     Gets the urn of the attachmentConstraint that caused the notification event to be created (no applicable for registrations)
        /// </summary>
        /// <value> </value>
        Uri ObjectUrn { get; }

        /// <summary>
        ///     Gets the Registration that triggered this event.  This can be null if it was not a registration that triggered the
        ///     event.
        /// </summary>
        /// <value> </value>
        IRegistrationObject Registration { get; }

        /// <summary>
        ///     Gets the urn of the subscription that this notification event was triggered from
        /// </summary>
        /// <value> </value>
        Uri SubscriptionUrn { get; }

        #endregion
    }
}