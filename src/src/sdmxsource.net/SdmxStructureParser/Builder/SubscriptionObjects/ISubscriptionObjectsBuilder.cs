// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISubscriptionObjectsBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Builds SubscriptionNotifications from a SubmitSubscriptionRequestType.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.SubscriptionObjects
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    /// <summary>
    ///     Builds SubscriptionNotifications from a SubmitSubscriptionRequestType.
    /// </summary>
    public interface ISubscriptionObjectsBuilder : IBuilder<IList<ISubscriptionObject>, SubmitSubscriptionsRequestType>
    {
    }
}