// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubscriptionObjectsBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   This builds Subscription Notifications from base SDMX Subscription types.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.SubscriptionObjects
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry;

    /// <summary>
    ///     This builds Subscription Notifications from base SDMX Subscription types.
    /// </summary>
    public class SubscriptionObjectsBuilder : ISubscriptionObjectsBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// builds Subscription Notifications from base SDMX Subscription types.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// Subscription Notifications from base SDMX Subscription types
        /// </returns>
        public virtual IList<ISubscriptionObject> Build(SubmitSubscriptionsRequestType buildFrom)
        {
            IList<ISubscriptionObject> returnList = new List<ISubscriptionObject>();

            /* foreach */
            foreach (SubscriptionRequestType subscription in buildFrom.SubscriptionRequest)
            {
                returnList.Add(new SubscriptionObjectCore(subscription.Subscription));
            }

            return returnList;
        }

        #endregion
    }
}