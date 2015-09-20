// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStaticWsdlService.cs" company="Eurostat">
//   Date Created : 2013-10-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The static <c>WSDL</c> service
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Wsdl
{
    using System.IO;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    /// <summary>
    ///     The static <c>WSDL</c> service
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.NotAllowed)]
    public interface IStaticWsdlService
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the WSDL.
        /// </summary>
        /// <param name="name">
        /// The service name.
        /// </param>
        /// <returns>
        /// The stream to the WSDL.
        /// </returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "{name}")]
        Stream GetWsdl(string name);

        #endregion
    }
}