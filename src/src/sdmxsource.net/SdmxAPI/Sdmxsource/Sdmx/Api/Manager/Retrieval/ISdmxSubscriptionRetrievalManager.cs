// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxSubscriptionRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    #endregion

    /// <summary>
    ///     Manages the retrieval of subscriptions
    /// </summary>
    public interface ISdmxSubscriptionRetrievalManager
    {

        /// <summary>
        /// Gets all the subscription count in the system..
        /// </summary>
        /// <value>
        /// The subscription count.
        /// </value>
        int SubscriptionCount { get; }

        #region Public Methods and Operators

        /// <summary>
        /// Gets a set of all the subscriptions which are owned by the a given organisation.
        ///     <p/>
        ///     Gets an empty set if no subscriptions are found
        /// </summary>
        /// <param name="organisation">
        /// The organisation.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{ISubscriptionObject}"/> .
        /// </returns>
        ISet<ISubscriptionObject> GetSubscriptions(IOrganisation organisation);

        /// <summary>
        /// Gets a set of all the subscriptions for a given organisation (Data Consumer, Data Provider or Agency)
        /// </summary>
        /// <param name="organisationReference">
        /// this will be validated that the reference is to a data consumer, and it a full reference (contains all reference parameters)
        /// </param>
        /// <returns>
        /// The <see cref="ISet{ISubscriptionObject}"/> .
        /// </returns>
        /// <exception cref="CrossReferenceException">
        /// if the IStructureReference does not reference a valid Organisation
        /// </exception>
        ISet<ISubscriptionObject> GetSubscriptions(IStructureReference organisationReference);

        /// <summary>
        /// Gets a set of subscriptions of the given type for the trigger identifiable
        /// </summary>
        /// <param name="identifiableObject">The identifiable that triggers this event </param>
        /// <param name="subscriptionType">This defines the type of subscriptions to return </param>
        /// <returns></returns>
        ISet<ISubscriptionObject> GetSubscriptionsForEvent(IIdentifiableObject identifiableObject, SubscriptionType subscriptionType);

        #endregion
    }
}