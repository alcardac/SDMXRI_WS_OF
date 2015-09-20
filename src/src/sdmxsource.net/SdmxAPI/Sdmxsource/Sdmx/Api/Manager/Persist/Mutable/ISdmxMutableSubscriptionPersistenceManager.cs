// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxMutableSubscriptionPersistenceManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Persist.Mutable
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;

    #endregion

    /// <summary>
    ///     	Saves and deletes subsscriptions
    /// </summary>
    public interface ISdmxMutableSubscriptionPersistenceManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Deletes the subscription
        /// </summary>
        /// <param name="subscriptionMutableObject">The SubscriptionMutableObject
        /// </param>
        void DeleteSubscription(ISubscriptionMutableObject subscriptionMutableObject);

        /// <summary>
        /// Stores the subscription and returns a copy of the stored instance, in mutable form
        /// </summary>
        /// <param name="subscriptionMutableObject">The SubscriptionMutableObject
        /// </param>
        /// <returns>
        /// The <see cref="ISubscriptionMutableObject"/> .
        /// </returns>
        ISubscriptionMutableObject SaveSubscription(ISubscriptionMutableObject subscriptionMutableObject);

        #endregion
    }
}