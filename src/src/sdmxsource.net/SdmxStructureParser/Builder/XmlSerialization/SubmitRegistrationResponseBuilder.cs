// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitRegistrationResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The submit registration response builder.
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
    ///     The submit registration response builder.
    /// </summary>
    public class SubmitRegistrationResponseBuilder : AbstractResponseBuilder<ISubmitRegistrationResponse>
    {
        #region Static Fields

        /// <summary>
        ///     The registry message type.
        /// </summary>
        private static readonly RegistryMessageType _registryMessageType =
            RegistryMessageType.GetFromEnum(RegistryMessageEnumType.SubmitRegistrationResponse);

        #endregion

        #region Fields

        /// <summary>
        ///     The submit registration response builder v 21.
        /// </summary>
        private readonly SubmitRegistrationResponseBuilderV21 _submitRegistrationResponseBuilderV21 =
            new SubmitRegistrationResponseBuilderV21();

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
        /// Build and return a list of <see cref="ISubmitRegistrationResponse"/> from the specified
        ///     <paramref name="registryInterface"/>
        /// </summary>
        /// <param name="registryInterface">
        /// The registry interface message
        /// </param>
        /// <returns>
        /// return a list of <see cref="ISubmitRegistrationResponse"/> from the specified <paramref name="registryInterface"/>
        /// </returns>
        internal override IList<ISubmitRegistrationResponse> BuildInternal(RegistryInterface registryInterface)
        {
            return this._submitRegistrationResponseBuilderV21.Build(registryInterface);
        }

        #endregion
    }
}