// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISubmitSubscriptionResponse.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.SubmissionResponse
{
    /// <summary>
    ///     Response from a SDMX Registry for a Subscription submission
    /// </summary>
    public interface ISubmitSubscriptionResponse : ISubmitStructureResponse
    {
        #region Public Properties

        /// <summary>
        ///     Gets the id that the subscriber assigned to the subscription
        /// </summary>
        /// <value> </value>
        string SubscriberAssignedId { get; }

        #endregion
    }
}