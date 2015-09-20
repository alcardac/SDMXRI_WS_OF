// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INSIEstatV20Service.cs" company="Eurostat">
//   Date Created : 2013-10-16
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The SDMX v2.0 with ESTAT extensions interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Soap
{
    using System.ComponentModel;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    using Estat.Sri.Ws.Controllers.Constants;
    using Estat.Sri.Ws.Controllers.Model;
    using Estat.Sri.Ws.Wsdl;

    /// <summary>
    ///     The SDMX v2.0 with ESTAT extensions interface.
    /// </summary>
    [ServiceContract(Namespace = SoapNamespaces.SdmxV20Estat, ConfigurationName = "NSIEstatV20Service", SessionMode = SessionMode.NotAllowed, Name = "NSIEstatV20Service")]
    [DispatchByBodyElementBehavior]
    [Description("Web Service used by NSI for data dissemination and structural metadata retrieval. This service uses the SDMX 2.0 Schema files with Eurostat extensions without the extra element.")]
    public interface INSIEstatV20Service
    {
        #region Public Methods and Operators

        /// <summary>
        /// Web Method that is used to retrieve sdmx data in compact format based on a sdmx query
        /// </summary>
        /// <param name="request">
        /// The sdmx query
        /// </param>
        /// <returns>
        /// The queried data in sdmx compact format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetCompactData, SoapNamespaces.SdmxV20Estat)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV20Estat)]
        Message GetCompactData(Message request);

        /// <summary>
        /// Web Method that is used to retrieve sdmx data in cross sectional format based on a sdmx query
        /// </summary>
        /// <param name="request">
        /// The sdmx query
        /// </param>
        /// <returns>
        /// The queried data in sdmx cross sectional format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetCrossSectionalData, SoapNamespaces.SdmxV20Estat)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV20Estat)]
        Message GetCrossSectionalData(Message request);

        /// <summary>
        /// Web Method that is used to retrieve sdmx data in generic format based on a sdmx query
        /// </summary>
        /// <param name="request">
        /// The sdmx query
        /// </param>
        /// <returns>
        /// The queried data in sdmx generic format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetGenericData, SoapNamespaces.SdmxV20Estat)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV20Estat)]
        Message GetGenericData(Message request);

        /// <summary>
        /// Web Method that is used to retrieve sdmx data in Utility format based on a sdmx query
        /// </summary>
        /// <param name="request">
        /// The sdmx query
        /// </param>
        /// <returns>
        /// The queried data in sdmx cross sectional format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetUtilityData, SoapNamespaces.SdmxV20Estat)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV20Estat)]
        Message GetUtilityData(Message request);

        /// <summary>
        /// Web Method that is used to retrieve sdmx structural metadata based on a sdmx query structure request
        /// </summary>
        /// <param name="request">
        /// The sdmx query structure request
        /// </param>
        /// <returns>
        /// The sdmx structural metadata inside a RegistryInterface QueryStructureResponse
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.QueryStructure, SoapNamespaces.SdmxV20Estat)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV20Estat)]
        Message QueryStructure(Message request);

        #endregion
    }
}