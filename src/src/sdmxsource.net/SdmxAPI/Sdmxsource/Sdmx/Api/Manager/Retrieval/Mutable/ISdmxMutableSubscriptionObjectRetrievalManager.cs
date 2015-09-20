// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxMutableSubscriptionObjectRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     Manages the retrieval of SubscriptionMutableObjects
    /// </summary>
    public interface ISdmxMutableSubscriptionObjectRetrievalManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets a set of all the subscriptions for a given organisation (Data Consumer, Data Provider or Agency)
        ///     <p/>
        ///     Gets an empty list if no subscriptions match the criteria
        /// </summary>
        /// <param name="dataConsumerReference">
        /// The data Consumer Reference.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{ISubscriptionMutableObject}"/> .
        /// </returns>
        ISet<ISubscriptionMutableObject> GetSubscriptions(IStructureReference dataConsumerReference);

        #endregion
    }
}