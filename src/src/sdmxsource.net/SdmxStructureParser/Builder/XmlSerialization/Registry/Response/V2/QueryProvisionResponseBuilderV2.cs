// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryProvisionResponseBuilderV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The query provision response builder v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The query provision response builder v 2.
    /// </summary>
    public class QueryProvisionResponseBuilderV2 : AbstractResponseBuilder
    {
        #region Static Fields

        /// <summary>
        ///     The instance.
        /// </summary>
        private static readonly QueryProvisionResponseBuilderV2 _instance = new QueryProvisionResponseBuilderV2();

        #endregion

        #region Fields

        /// <summary>
        ///     The Provision Agreement builder.
        /// </summary>
        private readonly ProvisionAgreementXmlBuilder _provBuilder;

        #endregion

        // PRIVATE CONSTRUCTOR
        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref="QueryProvisionResponseBuilderV2" /> class from being created.
        /// </summary>
        private QueryProvisionResponseBuilderV2()
        {
            this._provBuilder = ProvisionAgreementXmlBuilder.Instance;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        public static QueryProvisionResponseBuilderV2 Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build error response.
        /// </summary>
        /// <param name="th">
        /// The exception.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildErrorResponse(Exception th)
        {
            var responseType = new RegistryInterface();
            RegistryInterfaceType regInterface = responseType.Content;
            var returnType = new QueryProvisioningResponseType();
            regInterface.QueryProvisioningResponse = returnType;
            V2Helper.Header = regInterface;

            returnType.StatusMessage = new StatusMessageType();

            this.AddStatus(returnType.StatusMessage, th);

            return responseType;
        }

        /// <summary>
        /// The build success response.
        /// </summary>
        /// <param name="provisions">
        /// The provisions.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildSuccessResponse(ICollection<IProvisionAgreementObject> provisions)
        {
            var responseType = new RegistryInterface();
            RegistryInterfaceType regInterface = responseType.Content;
            var returnType = new QueryProvisioningResponseType();
            regInterface.QueryProvisioningResponse = returnType;
            V2Helper.Header = regInterface;

            var statusMessage = new StatusMessageType();
            returnType.StatusMessage = statusMessage;

            this.AddStatus(returnType.StatusMessage, null);

            if (!ObjectUtil.ValidCollection(provisions))
            {
                statusMessage.status = StatusTypeConstants.Warning;
                var tt = new TextType();
                statusMessage.MessageText.Add(tt);

                tt.TypedValue = "No Provisions Match The Query Parameters";
            }
            else
            {
                statusMessage.status = StatusTypeConstants.Success;
                var provTypes = new ProvisionAgreementType[provisions.Count];

                int i = 0;

                /* foreach */
                foreach (IProvisionAgreementObject currentProv in provisions)
                {
                    provTypes[i] = this._provBuilder.Build(currentProv);
                    i++;
                }

                returnType.ProvisionAgreement = provTypes;
            }

            return responseType;
        }

        #endregion
    }
}