// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISubscriptionPersistenceManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Persist
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    #endregion

    /// <summary>
    ///     Manages the persistence of subscription notifications
    /// </summary>
    public interface ISubscriptionPersistenceManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Deletes a subscription message
        /// </summary>
        /// <param name="subscription"> The subscription collection. </param>
        void DeleteSubscriptions(ICollection<ISubscriptionObject> subscription);

        /// <summary>
        /// Saves a subscription message
        /// </summary>
        /// <param name="subscription">The subscription collection. </param>
        void SaveSubscriptions(ICollection<ISubscriptionObject> subscription);

        #endregion
    }
}