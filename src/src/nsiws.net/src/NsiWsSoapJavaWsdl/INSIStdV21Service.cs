// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INSIStdV21Service.cs" company="Eurostat">
//   Date Created : 2013-10-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The SDMX v2.1 SOAP interface.
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
    ///     The SDMX v2.1 SOAP interface.
    /// </summary>
    [ServiceContract(Namespace = SoapNamespaces.SdmxV21, ConfigurationName = "SdmxServiceService", SessionMode = SessionMode.NotAllowed, Name = "SdmxService")]
    [DispatchByBodyElementBehavior]
    [Description("Web Service used by NSI for data dissemination and structural metadata retrieval. This service uses the SDMX 2.1 Schema files and the standard WSDL.")]
    public interface INSIStdV21Service
    {
        #region Public Methods and Operators

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "*", ReplyAction = "*")]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message DefaultHandler(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetCategorisation, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetCategorisation(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetCategoryScheme, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetCategoryScheme(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetCodelist, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetCodelist(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetConceptScheme, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetConceptScheme(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetDataStructure, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetDataStructure(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetDataflow, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetDataflow(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> data in generic format based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried data in <c>SDMX</c> generic format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetGenericData, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetGenericData(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> data in generic format based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried data in <c>SDMX</c> generic format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetGenericTimeSeriesData, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetGenericTimeSeriesData(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetHierarchicalCodelist, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetHierarchicalCodelist(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> data in structure specific format based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried data in <c>SDMX</c> compact format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetStructureSpecificData, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetStructureSpecificData(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> data in structure specific format based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried data in <c>SDMX</c> compact format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetStructureSpecificTimeSeriesData, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetStructureSpecificTimeSeriesData(Message request);

        /// <summary>
        /// Web Method that is used to retrieve <c>SDMX</c> structure based on a <c>SDMX</c> query
        /// </summary>
        /// <param name="request">
        /// The <c>SDMX</c> query
        /// </param>
        /// <returns>
        /// The queried structure in <c>SDMX</c> SDMX-ML v2.1  format
        /// </returns>
        [OperationContract(Action = "", ReplyAction = "*")]
        [DispatchBodyElement(SoapOperation.GetStructures, SoapNamespaces.SdmxV21)]
        [FaultContract(typeof(SdmxFault), Name = "Error", Namespace = SoapNamespaces.SdmxV21)]
        Message GetStructures(Message request);

        #endregion
    }
}