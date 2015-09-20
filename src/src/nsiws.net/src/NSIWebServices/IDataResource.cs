// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataResource.cs" company="Eurostat">
//   Date Created : 2013-10-07
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The DataResource interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Rest
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Web;

    /// <summary>
    /// The DataResource interface.
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.NotAllowed)]
    public interface IDataResource
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get Json data.
        /// </summary>
        /// <param name="flowRef">
        /// The flow ref.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="providerRef">
        /// The provider ref.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "json/{flowRef}/{key}/{providerRef}/")]
        Message GetJsonData(string flowRef, string key, string providerRef);


        /// <summary>
        /// The get generic data.
        /// </summary>
        /// <param name="flowRef">
        /// The flow ref.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="providerRef">
        /// The provider ref.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "{flowRef}/{key}/{providerRef}/")]
        Message GetGenericData(string flowRef, string key, string providerRef);

        /// <summary>
        /// The get generic data all keys.
        /// </summary>
        /// <param name="flowRef">
        /// The flow ref.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "{flowRef}/")]
        Message GetGenericDataAllKeys(string flowRef);

        /// <summary>
        /// The get generic data all providers.
        /// </summary>
        /// <param name="flowRef">
        /// The flow ref.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Message"/>.
        /// </returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "{flowRef}/{key}/")]
        Message GetGenericDataAllProviders(string flowRef, string key);

      
   
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "jsonzip/{flowRef}/{key}/{providerRef}/")]
        Message GetJsonZipData(string flowRef, string key, string providerRef);
        
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "dspl/{flowRef}/{key}/{providerRef}/")]
        Message GetDsplData(string flowRef, string key, string providerRef);

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "csv/{flowRef}/{key}/{providerRef}/")]
        Message GetCSVData(string flowRef, string key, string providerRef);

        
        #endregion


       
    }
}