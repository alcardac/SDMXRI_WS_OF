// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NsiStdV20Service.cs" company="Eurostat">
//   Date Created : 2013-10-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The SDMX v2.0 SOAP with ESTAT extensions implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Soap
{
    using System.ServiceModel;
    using System.ServiceModel.Activation;

    using Estat.Sri.Ws.Controllers.Constants;

    /// <summary>
    ///     The SDMX v2.0 SOAP with ESTAT extensions implementation.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class NsiStdV20Service : NsiV20ServiceBase, INSIStdV20Service
    {
        #region Properties

        /// <summary>
        ///     Gets the type of the endpoint.
        /// </summary>
        /// <value>
        ///     The type of the endpoint.
        /// </value>
        protected override WebServiceEndpoint EndpointType
        {
            get
            {
                return WebServiceEndpoint.StandardEndpoint;
            }
        }

        /// <summary>
        ///     Gets the namespace
        /// </summary>
        /// <value>
        ///     The namespace
        /// </value>
        protected override string Ns
        {
            get
            {
                return SoapNamespaces.SdmxV20JavaStd;
            }
        }

        #endregion
    }
}