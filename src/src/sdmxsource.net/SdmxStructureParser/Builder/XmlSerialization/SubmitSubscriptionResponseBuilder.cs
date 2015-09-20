// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitSubscriptionResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The submit subscription response builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.SubmissionResponse;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Response;

    /// <summary>
    ///     The submit subscription response builder.
    /// </summary>
    public class SubmitSubscriptionResponseBuilder : AbstractResponseBuilder<ISubmitSubscriptionResponse>
    {
        #region Static Fields

        /// <summary>
        ///     The registry message type.
        /// </summary>
        private static readonly RegistryMessageType _registryMessageType =
            RegistryMessageType.GetFromEnum(RegistryMessageEnumType.SubmitSubscriptionResponse);

        #endregion

        #region Fields

        /// <summary>
        ///     The subscription builder v 21.
        /// </summary>
        private readonly SubmitSubscriptionResponseBuilderV21 _subscriptionResponseBuilderV21 =
            new SubmitSubscriptionResponseBuilderV21();

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the expected message type.
        /// </summary>
        internal override RegistryMessageType ExpectedMessageType
        {
            get
            {
                return _registryMessageType;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Build <see cref="ISubmitSubscriptionResponse"/> list from the specified <paramref name="registryInterface"/>
        /// </summary>
        /// <param name="registryInterface">
        /// The registry Interface message
        /// </param>
        /// <returns>
        /// The <see cref="ISubmitSubscriptionResponse"/> list from the specified <paramref name="registryInterface"/>
        /// </returns>
        internal override IList<ISubmitSubscriptionResponse> BuildInternal(RegistryInterface registryInterface)
        {
            return this._subscriptionResponseBuilderV21.Build(registryInterface);
        }

        #endregion
    }
}