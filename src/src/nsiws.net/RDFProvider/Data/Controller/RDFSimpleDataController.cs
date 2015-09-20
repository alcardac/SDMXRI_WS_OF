// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleDataController.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The SDMX v20 SOAP controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace RDFProvider.Controller
{
    using System;
    using System.ServiceModel.Channels;
    using System.Xml;
    using System.Xml.Linq;

    using Estat.Nsi.AuthModule;
    using Estat.Sri.Ws.Controllers.Extension;
    using Estat.Sri.Ws.Controllers.Controller;
    using Estat.Sri.Ws.Controllers.Properties;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query;

    /// <summary>
    /// The SDMX v20 SOAP controller.
    /// </summary>
    /// <typeparam name="TWriter">
    /// The type of the writer.
    /// </typeparam>
    public class RDFSimpleDataController<TWriter> : DataControllerBase,
                                                 IController<XmlNode, TWriter>,
                                                 IController<IReadableDataLocation, TWriter>,
                                                 IController<IDataQuery, TWriter>,
                                                 IController<IRestDataQuery, TWriter>,
                                                 IController<XElement, TWriter>,
                                                 IController<Message, TWriter>
    {
        #region Fields

        /// <summary>
        ///     The _log.
        /// </summary>
        private readonly ILog _log = LogManager.GetLogger(typeof(RDFSimpleDataController<TWriter>));

        /// <summary>
        ///     The _response generator.
        /// </summary>
        private readonly IResponseGenerator<TWriter, IDataQuery> _responseGenerator;

        /// <summary>
        ///     The _validator
        /// </summary>
        private readonly IDataRequestValidator _validator;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDataController{TWriter}"/> class.
        /// </summary>
        /// <param name="responseGenerator">
        /// The response generator.
        /// </param>
        /// <param name="validator">
        /// The validator.
        /// </param>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="responseGenerator"/> is null -or-
        ///     <paramref name="validator"/> is null.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Operation not accepted with query used
        /// </exception>
        public RDFSimpleDataController(IResponseGenerator<TWriter, IDataQuery> responseGenerator, IDataRequestValidator validator, DataflowPrincipal principal)
            : base(principal)
        {
            if (responseGenerator == null)
            {
                throw new ArgumentNullException("responseGenerator");
            }

            if (validator == null)
            {
                throw new ArgumentNullException("validator");
            }

            this._responseGenerator = responseGenerator;
            this._validator = validator;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Parse request from <paramref name="input"/>
        /// </summary>
        /// <param name="input">
        /// The reader for the SDMX-ML or REST request
        /// </param>
        /// <returns>
        /// The <see cref="IStreamController{TWriter}"/>.
        /// </returns>
        public IStreamController<TWriter> ParseRequest(XmlNode input)
        {
            using (IReadableDataLocation xmlReadable = input.GetReadableDataLocation())
            {
                return this.ParseRequest(xmlReadable);
            }
        }

        /// <summary>
        /// Parse request from <paramref name="input"/>
        /// </summary>
        /// <param name="input">
        /// The reader for the SDMX-ML or REST request
        /// </param>
        /// <returns>
        /// The <see cref="IStreamController{TWriter}"/>.
        /// </returns>
        public IStreamController<TWriter> ParseRequest(IReadableDataLocation input)
        {
            var dataQuery = this.GetDataQueryFromStream(input);
            return this.ParseRequest(dataQuery);
        }

        /// <summary>
        /// Parse request from <paramref name="input"/>
        /// </summary>
        /// <param name="input">
        /// The reader for the SDMX-ML or REST request
        /// </param>
        /// <returns>
        /// The <see cref="IStreamController{TWriter}"/>.
        /// </returns>
        public IStreamController<TWriter> ParseRequest(IDataQuery input)
        {
            if (input == null)
            {
                throw new SdmxSemmanticException(Resources.ErrorOperationNotAccepted);
            }

            // 1. check if we should be authorized and are 
            this.Authorize(input.Dataflow);

            // 2. Validate
            this._validator.Validate(input);

            // 3. Return the Stream
            return new StreamController<TWriter>(this._responseGenerator.GenerateResponseFunction(input));
        }

        /// <summary>
        /// Parse request from <paramref name="input"/>
        /// </summary>
        /// <param name="input">
        /// The reader for the SDMX-ML or REST request
        /// </param>
        /// <returns>
        /// The <see cref="IStreamController{TWriter}"/>.
        /// </returns>
        public IStreamController<TWriter> ParseRequest(IRestDataQuery input)
        {
            if (input == null)
            {
                throw new SdmxSemmanticException(Resources.ErrorOperationNotAccepted);
            }

            IDataQuery dataQuery = new DataQueryImpl(input, this.SdmxRetrievalManager);
            this._log.DebugFormat("Converted REST data query '{0}' to IDataQuery {1}", input, dataQuery);
            return this.ParseRequest(dataQuery);
        }

        /// <summary>
        /// Parse request from <paramref name="input"/>
        /// </summary>
        /// <param name="input">
        /// The reader for the SDMX-ML or REST request
        /// </param>
        /// <returns>
        /// The <see cref="IStreamController{TWriter}"/>.
        /// </returns>
        public IStreamController<TWriter> ParseRequest(XElement input)
        {
            if (input == null)
            {
                throw new SdmxSemmanticException(Resources.ErrorOperationNotAccepted);
            }

            using (IReadableDataLocation xmlReadable = input.GetReadableDataLocation())
            {
                return this.ParseRequest(xmlReadable);
            }
        }

        /// <summary>
        /// Parse request from <paramref name="input"/>
        /// </summary>
        /// <param name="input">
        /// The reader for the SDMX-ML or REST request
        /// </param>
        /// <returns>
        /// The <see cref="IStreamController{TWriter}"/>.
        /// </returns>
        public IStreamController<TWriter> ParseRequest(Message input)
        {
            if (input == null)
            {
                throw new SdmxSemmanticException(Resources.ErrorOperationNotAccepted);
            }

            using (IReadableDataLocation xmlReadable = input.GetReadableDataLocation(new XmlQualifiedName(SdmxConstants.QueryMessageRootNode, SdmxConstants.MessageNs20)))
            {
                return this.ParseRequest(xmlReadable);
            }
        }

        #endregion
    }
}