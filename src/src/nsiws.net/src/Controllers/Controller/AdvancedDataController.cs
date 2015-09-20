// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdvancedDataController.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The advanced data controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Controller
{
    using System;
    using System.Linq;
    using System.ServiceModel.Channels;
    using System.Xml;

    using Estat.Nsi.AuthModule;
    using Estat.Sri.Ws.Controllers.Constants;
    using Estat.Sri.Ws.Controllers.Extension;
    using Estat.Sri.Ws.Controllers.Properties;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    /// The advanced data controller.
    /// </summary>
    /// <typeparam name="TWriter">
    /// The type of the writer
    /// </typeparam>
    public class AdvancedDataController<TWriter> : DataControllerBase, 
                                                   IController<XmlNode, TWriter>, 
                                                   IController<IReadableDataLocation, TWriter>, 
                                                   IController<IComplexDataQuery, TWriter>, 
                                                   IController<Message, TWriter>
    {
        #region Fields

        /// <summary>
        ///     The _response generator.
        /// </summary>
        private readonly IResponseGenerator<TWriter, IComplexDataQuery> _responseGenerator;

        /// <summary>
        ///     The _root node
        /// </summary>
        private readonly XmlQualifiedName _rootNode;

        /// <summary>
        ///     The _validator
        /// </summary>
        private readonly IDataRequestValidator _validator;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedDataController{TWriter}"/> class.
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
        /// <param name="operation">
        /// The operation.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// responseGenerator
        ///     or
        ///     validator
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Operation not accepted with query used
        /// </exception>
        public AdvancedDataController(IResponseGenerator<TWriter, IComplexDataQuery> responseGenerator, IDataRequestValidator validator, DataflowPrincipal principal, SoapOperation operation)
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
            this._rootNode = operation.GetQueryRootElementV21();
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
            using (IReadableDataLocation xmlReadable = new XmlDocReadableDataLocation(input))
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
            IDataQueryParseManager dataQueryParseManager = new DataQueryParseManager(SdmxSchemaEnumType.VersionTwoPointOne);
            var dataQuery = dataQueryParseManager.BuildComplexDataQuery(input, this.SdmxRetrievalManager).FirstOrDefault();
            if (dataQuery == null)
            {
                throw new SdmxSemmanticException(Resources.ErrorOperationNotAccepted);
            }

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
        public IStreamController<TWriter> ParseRequest(IComplexDataQuery input)
        {
            // 1. authorize
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
        public IStreamController<TWriter> ParseRequest(Message input)
        {
            if (input == null)
            {
                throw new SdmxSemmanticException(Resources.ErrorOperationNotAccepted);
            }

            using (IReadableDataLocation xmlReadable = input.GetReadableDataLocation(this._rootNode))
            {
                return this.ParseRequest(xmlReadable);
            }
        }

        #endregion
    }
}