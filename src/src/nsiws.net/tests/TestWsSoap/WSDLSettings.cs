// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WSDLSettings.cs" company="Eurostat">
//   Date Created : 2013-10-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class will parse and expose various WSDL settings
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TestWsSoap
{
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Services.Description;
    using System.Xml;
    using System.Xml.Schema;

    /// <summary>
    ///     This class will parse and expose various WSDL settings
    /// </summary>
    public class WSDLSettings
    {
        #region Fields

        /// <summary>
        ///     This field holds the a map between a web service operation name and the parameter wrapper element name. This is
        ///     used for connecting to .NET WS.
        /// </summary>
        private readonly Dictionary<string, string> _operationParameterName = new Dictionary<string, string>();

        /// <summary>
        ///     This field holds the a map between a web service operation name and the soap action
        /// </summary>
        private readonly Dictionary<string, string> _soapAction = new Dictionary<string, string>();

        /// <summary>
        ///     Holds the service description of the WSDL
        /// </summary>
        private ServiceDescription _wsdl;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WSDLSettings"/> class.
        /// </summary>
        /// <param name="wsdlUri">
        /// The WSDL URL
        /// </param>
        public WSDLSettings(string wsdlUri)
        {
            this.GetWsdl(wsdlUri);
            this.BuildOperationParameterName();
            this.ΒuildSoapActionMap();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the WSDL target namespace
        /// </summary>
        /// <value> The target namespace </value>
        public string TargetNamespace
        {
            get
            {
                return this._wsdl.TargetNamespace;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Get the parameter element name if one exists for the given operation
        /// </summary>
        /// <param name="operationName">
        /// The name of the operation or web method or function
        /// </param>
        /// <returns>
        /// null if there isn't a parameter element name e.g. in Java or the parameter element name (e.g. in .net)
        /// </returns>
        public string GetParameterName(string operationName)
        {
            string ret;
            if (this._operationParameterName.TryGetValue(operationName, out ret))
            {
                return ret;
            }

            return null;
        }

        /// <summary>
        /// Get the soapAction URI if one exists for the given operation
        /// </summary>
        /// <param name="operationName">
        /// The name of the operation or web method or function
        /// </param>
        /// <returns>
        /// The <c>"http://schemas.xmlsoap.org/wsdl/soap/"</c> soapAction value or null if there isn't a soapAction
        /// </returns>
        public string GetSoapAction(string operationName)
        {
            string ret;
            if (this._soapAction.TryGetValue(operationName, out ret))
            {
                return ret;
            }

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Populate the <see cref="_operationParameterName" /> dictionary with operation.Name to Parameter name
        /// </summary>
        private void BuildOperationParameterName()
        {
            // Tested with .net and java NSI WS WSDL
            foreach (XmlSchema schema in this._wsdl.Types.Schemas)
            {
                foreach (XmlSchemaElement element in schema.Elements.Values)
                {
                    if (element.RefName.IsEmpty)
                    {
                        var complexType = element.SchemaType as XmlSchemaComplexType;
                        if (complexType != null)
                        {
                            var seq = complexType.Particle as XmlSchemaSequence;
                            if (seq != null && seq.Items.Count == 1)
                            {
                                var body = seq.Items[0] as XmlSchemaElement;
                                if (body != null && !string.IsNullOrEmpty(body.Name))
                                {
                                    if (!this._operationParameterName.ContainsKey(element.Name))
                                    {
                                        this._operationParameterName.Add(element.Name, body.Name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the WSDL.
        /// </summary>
        /// <param name="wsdlUrl">
        /// Parse the URL.
        /// </param>
        private void GetWsdl(string wsdlUrl)
        {
            var settings = new XmlReaderSettings
                               {
                                   IgnoreComments = true,
                                   IgnoreWhitespace = true,
                                   XmlResolver =
                                       new XmlUrlResolver { Credentials = new NetworkCredential(Properties.Settings.Default.userName, Properties.Settings.Default.password) }
                               };

            using (XmlReader reader = XmlReader.Create(wsdlUrl, settings))
            {
                this._wsdl = ServiceDescription.Read(reader);
            }
        }

        /// <summary>
        ///     Populate the <see cref="_soapAction" /> dictionary with operation.Name to soapAction URI
        /// </summary>
        private void ΒuildSoapActionMap()
        {
            foreach (Binding binding in this._wsdl.Bindings)
            {
                foreach (OperationBinding operation in binding.Operations)
                {
                    foreach (object ext in operation.Extensions)
                    {
                        var operationBinding = ext as SoapOperationBinding;
                        if (operationBinding != null)
                        {
                            if (!this._soapAction.ContainsKey(operation.Name))
                            {
                                this._soapAction.Add(operation.Name, operationBinding.SoapAction);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}