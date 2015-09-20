// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitStructureReponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The submit structure response builder.
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
    ///     The submit structure response builder.
    /// </summary>
    public class SubmitStructureReponseBuilder : AbstractResponseBuilder<ISubmitStructureResponse>
    {
        #region Static Fields

        /// <summary>
        ///     The _registry message type.
        /// </summary>
        private static readonly RegistryMessageType _registryMessageType =
            RegistryMessageType.GetFromEnum(RegistryMessageEnumType.SubmitStructureResponse);

        #endregion

        #region Fields

        /// <summary>
        ///     The response builder v 21.
        /// </summary>
        private readonly SubmitStructureResponseBuilderV21 _responseBuilderV21 = new SubmitStructureResponseBuilderV21();

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
        /// Build <see cref="SubmitStructureResponse"/> list from the specified <paramref name="registryInterface"/>
        /// </summary>
        /// <param name="registryInterface">
        /// The registry Interface message
        /// </param>
        /// <returns>
        /// The <see cref="SubmitStructureResponse"/> list from the specified <paramref name="registryInterface"/>
        /// </returns>
        internal override IList<ISubmitStructureResponse> BuildInternal(RegistryInterface registryInterface)
        {
            return this._responseBuilderV21.Build(registryInterface);
        }

        #endregion
    }
}