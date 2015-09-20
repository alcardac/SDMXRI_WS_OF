// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxRestServiceHostFactory.cs" company="Eurostat">
//   Date Created : 2013-10-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The sdmx rest service host factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Rest
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.ServiceModel.Description;
    using System.ServiceModel.Web;
    using System.Text;

    using log4net;

    /// <summary>
    /// The sdmx rest service host factory.
    /// </summary>
    public class SdmxRestServiceHostFactory : WebServiceHostFactory
    {
        #region Static Fields

        /// <summary>
        /// The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(SdmxRestServiceHostFactory));

        #endregion

        #region Fields

        /// <summary>
        /// The _type.
        /// </summary>
        private readonly Type _type;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxRestServiceHostFactory"/> class.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        public SdmxRestServiceHostFactory(Type type)
        {
            _log.DebugFormat("Init SdmxRestServiceHostFactory({0})", type);
            this._type = type;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create service host.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="baseAddresses">
        /// The base addresses.
        /// </param>
        /// <returns>
        /// The <see cref="ServiceHost"/>.
        /// </returns>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            try
            {
                _log.DebugFormat("Creating REST service host for {0} for uri : {1}", serviceType, baseAddresses[0]);
                ServiceHost serviceHost = base.CreateServiceHost(serviceType, baseAddresses);

                var webBehavior = new WebHttpBehavior { AutomaticFormatSelectionEnabled = false, HelpEnabled = true, FaultExceptionEnabled = false, DefaultBodyStyle = WebMessageBodyStyle.Bare };
                var binding = new WebHttpBinding { TransferMode = TransferMode.Streamed, ContentTypeMapper = new SdmxContentMapper()};
                var endpoint = serviceHost.AddServiceEndpoint(this._type, binding, baseAddresses[0]);

                endpoint.Behaviors.Add(webBehavior);

                return serviceHost;
            }
            catch (Exception e)
            {
                _log.Error("While creating service host", e);
                throw;
            }
        }

        #endregion
    }
}