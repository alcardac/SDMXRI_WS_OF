// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProvisionParsingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The provision parsing manager implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.XmlHelper;

    /// <summary>
    ///     The provision parsing manager implementation.
    /// </summary>
    public class ProvisionParsingManager : BaseParsingManager, IProvisionParsingManager
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProvisionParsingManager" /> class.
        /// </summary>
        public ProvisionParsingManager()
        {
        }

        // TODO Handle error responses

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvisionParsingManager"/> class.
        /// </summary>
        /// <param name="sdmxSchema">
        /// The sdmx schema.
        /// </param>
        public ProvisionParsingManager(SdmxSchemaEnumType sdmxSchema)
            : base(sdmxSchema)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Process a SMDX document to retrieve the Provisions, these can either be in
        ///     a QueryProvisionResponse message or inside a SubmitProvisionRequest message
        /// </summary>
        /// <param name="dataLocation">
        /// The data location of the SDMX document
        /// </param>
        /// <returns>
        /// The Provisions from <paramref name="dataLocation"/>
        /// </returns>
        public virtual IList<IProvisionAgreementObject> ParseXML(IReadableDataLocation dataLocation)
        {
            SdmxSchemaEnumType schemaVersion = this.GetSchemaVersion(dataLocation);
            XMLParser.ValidateXml(dataLocation, schemaVersion);
            Stream stream = dataLocation.InputStream;
            XmlReader reader = XMLParser.CreateSdmxMlReader(stream, schemaVersion);

            IList<IProvisionAgreementObject> returnList = new List<IProvisionAgreementObject>();
            switch (schemaVersion)
            {
                case SdmxSchemaEnumType.VersionTwo:
                    RegistryInterface rid = RegistryInterface.Load(reader);
                    if (rid.QueryProvisioningResponse != null
                        && rid.QueryProvisioningResponse.ProvisionAgreement != null)
                    {
                        /* foreach */
                        foreach (ProvisionAgreementType provType in
                            rid.QueryProvisioningResponse.ProvisionAgreement)
                        {
                            returnList.Add(new ProvisionAgreementObjectCore(provType));
                        }

                        // FUNC Support provisions by these types
                        if (ObjectUtil.ValidCollection(rid.QueryProvisioningResponse.DataflowRef))
                        {
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "Provision for Dataflow");
                        }

                        if (ObjectUtil.ValidCollection(rid.QueryProvisioningResponse.MetadataflowRef))
                        {
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "Provision for Metadataflow");
                        }

                        if (ObjectUtil.ValidCollection(rid.QueryProvisioningResponse.DataProviderRef))
                        {
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "Provision for Dataprovider");
                        }
                    }

                    if (rid.SubmitProvisioningRequest != null
                        && rid.SubmitProvisioningRequest.ProvisionAgreement != null)
                    {
                        /* foreach */
                        foreach (ProvisionAgreementType provType0 in rid.SubmitProvisioningRequest.ProvisionAgreement)
                        {
                            returnList.Add(new ProvisionAgreementObjectCore(provType0));
                        }

                        // FUNC Support provisions by these types
                        if (ObjectUtil.ValidCollection(rid.SubmitProvisioningRequest.DataflowRef))
                        {
                            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "Submit Provision for Dataflow");
                        }

                        if (ObjectUtil.ValidCollection(rid.SubmitProvisioningRequest.MetadatataflowRef))
                        {
                            throw new SdmxNotImplementedException(
                                ExceptionCode.Unsupported, "Submit Provision for Metadataflow");
                        }

                        if (ObjectUtil.ValidCollection(rid.SubmitProvisioningRequest.DataProviderRef))
                        {
                            throw new SdmxNotImplementedException(
                                ExceptionCode.Unsupported, "Submit Provision for Dataprovider");
                        }
                    }

                    break;
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, schemaVersion);
            }

            return returnList;
        }

        #endregion
    }
}