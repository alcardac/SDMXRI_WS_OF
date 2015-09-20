// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxMessageRest.cs" company="Eurostat">
//   Date Created : 2013-11-18
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The SDMX Message for REST (no soap envelope).
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Model
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Xml;

    using Estat.Sri.Ws.Controllers.Controller;

    /// <summary>
    ///     The SDMX Message for REST (no soap envelope).
    /// </summary>
    public class SdmxMessageRest : SdmxMessageBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxMessageRest"/> class.
        /// </summary>
        /// <param name="controller">
        /// The controller.
        /// </param>
        /// <param name="exceptionHandler">
        /// The exception handler.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// controller
        ///     or
        ///     exceptionHandler
        ///     or
        ///     messageVersion
        /// </exception>
        public SdmxMessageRest(IStreamController<XmlWriter> controller, Func<Exception, FaultException> exceptionHandler)
            : base(controller, exceptionHandler, MessageVersion.None)
        {
        }

        #endregion
    }
}