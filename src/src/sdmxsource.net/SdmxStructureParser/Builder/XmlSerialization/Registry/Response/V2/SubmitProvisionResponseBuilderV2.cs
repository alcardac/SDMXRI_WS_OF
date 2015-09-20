// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitProvisionResponseBuilderV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The submit provision response builder v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The submit provision response builder v 2.
    /// </summary>
    public class SubmitProvisionResponseBuilderV2 : AbstractResponseBuilder
    {
        #region Static Fields

        /// <summary>
        ///     The instance.
        /// </summary>
        private static readonly SubmitProvisionResponseBuilderV2 _instance = new SubmitProvisionResponseBuilderV2();

        #endregion

        // PRIVATE CONSTRUCTOR
        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref="SubmitProvisionResponseBuilderV2" /> class from being created.
        /// </summary>
        private SubmitProvisionResponseBuilderV2()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        public static SubmitProvisionResponseBuilderV2 Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build error response.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildErrorResponse(Exception exception)
        {
            var responseType = new RegistryInterface();
            RegistryInterfaceType regInterface = responseType.Content;
            var returnType = new SubmitProvisioningResponseType();
            regInterface.SubmitProvisioningResponse = returnType;
            V2Helper.Header = regInterface;
            var statusType = new ProvisioningStatusType();
            returnType.ProvisioningStatus.Add(statusType);
            var statusMessage = new StatusMessageType();
            statusType.StatusMessage = statusMessage;
            this.AddStatus(statusMessage, exception);
            return responseType;
        }

        /// <summary>
        /// The build response.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildResponse(ICollection<IProvisionAgreementObject> response)
        {
            var responseType = new RegistryInterface();
            RegistryInterfaceType regInterface = responseType.Content;
            var returnType = new SubmitProvisioningResponseType();
            regInterface.SubmitProvisioningResponse = returnType;
            V2Helper.Header = regInterface;

            /* foreach */
            foreach (IProvisionAgreementObject provisionAgreement in response)
            {
                this.ProcessResponse(returnType, provisionAgreement);
            }

            return responseType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The process response.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="provisionAgreement">
        /// The provision agreement.
        /// </param>
        private void ProcessResponse(
            SubmitProvisioningResponseType returnType, IProvisionAgreementObject provisionAgreement)
        {
            var statusType = new ProvisioningStatusType();
            returnType.ProvisioningStatus.Add(statusType);
            var statusMessage = new StatusMessageType();
            statusType.StatusMessage = statusMessage;
            this.AddStatus(statusMessage, null);

            var provRefType = new ProvisionAgreementRefType();
            statusType.ProvisionAgreementRef = provRefType;

            if (ObjectUtil.ValidString(provisionAgreement.Urn))
            {
                provRefType.URN = provisionAgreement.Urn;
            }

            if (provisionAgreement.DataproviderRef != null)
            {
                ICrossReference crossRef = provisionAgreement.DataproviderRef;
                IMaintainableRefObject maintRef = crossRef.MaintainableReference;
                string value = maintRef.AgencyId;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    provRefType.OrganisationSchemeAgencyID = maintRef.AgencyId;
                }

                string value1 = maintRef.MaintainableId;
                if (!string.IsNullOrWhiteSpace(value1))
                {
                    provRefType.OrganisationSchemeID = maintRef.MaintainableId;
                }

                string value2 = crossRef.ChildReference.Id;
                if (crossRef.ChildReference != null && !string.IsNullOrWhiteSpace(value2))
                {
                    provRefType.DataProviderID = crossRef.ChildReference.Id;
                }

                string value3 = maintRef.Version;
                if (!string.IsNullOrWhiteSpace(value3))
                {
                    provRefType.DataProviderVersion = maintRef.Version;
                }
            }

            if (provisionAgreement.StructureUseage != null)
            {
                ICrossReference structUseageCrossRef = provisionAgreement.StructureUseage;
                IMaintainableRefObject maintRef0 = structUseageCrossRef.MaintainableReference;
                if (structUseageCrossRef.TargetReference.EnumType == SdmxStructureEnumType.Dataflow)
                {
                    string value = maintRef0.AgencyId;
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        provRefType.DataflowAgencyID = maintRef0.AgencyId;
                    }

                    string value1 = maintRef0.MaintainableId;
                    if (!string.IsNullOrWhiteSpace(value1))
                    {
                        provRefType.DataflowID = maintRef0.MaintainableId;
                    }

                    string value2 = maintRef0.Version;
                    if (!string.IsNullOrWhiteSpace(value2))
                    {
                        provRefType.DataflowVersion = maintRef0.Version;
                    }
                }
            }
        }

        #endregion
    }
}