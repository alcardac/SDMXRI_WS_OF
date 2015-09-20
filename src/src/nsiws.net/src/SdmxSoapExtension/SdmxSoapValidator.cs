// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxSoapValidator.cs" company="Eurostat">
//   Date Created : 2011-11-24
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   A SoapExtension class that validates the soap message
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.SdmxSoapValidatorExtension
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web.Services;
    using System.Web.Services.Protocols;
    using System.Xml;
    using System.Xml.Schema;

    using Estat.Nsi.SdmxSoapValidatorExtension.Properties;

    /// <summary>
    /// A SoapExtension class that validates the soap message
    /// </summary>
    public sealed class SdmxSoapValidator : SoapExtension, IDisposable
    {
        #region Constants and Fields

        /// <summary>
        /// XPath of SOAP 1.2 Fault Code text
        /// </summary>
        private const string Soap12CodePath = "//soap:Code/soap:Value/text()";

        /// <summary>
        /// A prefix used with <see cref="XmlNamespaceManager"/> with soap namespace.
        /// </summary>
        private const string SoapPrefix = "soap";

        /// <summary>
        /// A value object containing the information that is passed from a GetInitializer method to Initializer
        /// </summary>
        private InitializerValueObject _initialiserVo;

        /// <summary>
        /// The new stream. Used only for Exception handling
        /// </summary>
        private Stream _newStream;

        /// <summary>
        /// the old stream. Used only for Exception handling
        /// </summary>
        private Stream _oldStream;

        /// <summary>
        /// A value indicationg whether we are post the normal <see cref="SoapMessageStage.AfterSerialize"/> stage.
        /// This will occur only if an exception is thrown.
        /// </summary>
        private bool _postNormalAfterSerialize;

        /// <summary>
        /// A value indicationg whether we are post <see cref="SoapMessageStage.BeforeDeserialize"/> stage
        /// </summary>
        private bool _postBeforeDeserialize;

        #endregion

        #region Public Methods

        /// <summary>
        /// When overridden in a derived class, allows a SOAP extension access to the memory buffer containing the SOAP request or response.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.IO.Stream"/> representing a new memory buffer that this SOAP extension can modify.
        /// </returns>
        /// <param name="stream">
        /// A memory buffer containing the SOAP request or response. 
        /// </param>
        public override Stream ChainStream(Stream stream)
        {
            // handle only output streams
            ////if (stream.GetType().Name.Contains("Soap")) 
            if (this._postBeforeDeserialize)
            {
                if (this._postNormalAfterSerialize)
                {
                    // we want to modify only the exception. 
                    this._newStream = new MemoryStream();
                    this._oldStream = stream;
                    return this._newStream;
                }

                this._postNormalAfterSerialize = true;
            }

            return base.ChainStream(stream);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (this._newStream != null)
            {
                this._newStream.Dispose();
            }
        }

        /// <summary>
        /// Allows the SdmxSoapValidator soap extension to initialise the <see cref="InitializerValueObject"/> and <see cref="SchemaCache"/>
        /// This GetInitializer overload is called when Soap Extension is specified in the Web.config file.
        /// <seealso cref="System.Web.Services.Protocols.SoapExtension.GetInitializer(System.Type)"/>
        /// </summary>
        /// <param name="serviceType">
        /// The type of the class implementing the XML Web service to which the SOAP extension is applied. 
        /// </param>
        /// <returns>
        /// The <see cref="InitializerValueObject"/> object
        /// </returns>
        public override object GetInitializer(Type serviceType)
        {
            // var attributes =
            // (SdmxSoapValidatorAttribute[])
            // serviceType.GetCustomAttributes(typeof (SdmxSoapValidatorAttribute), true);

            // var attribute = new SdmxSoapValidatorAttribute();
            // if (attributes.Length > 0) {
            // attribute = attributes[0];
            // }
            return this.GetInitiliaserCommon(serviceType, serviceType.FullName);
        }

        /// <summary>
        /// Allows the SdmxSoapValidator soap extension to initialise the <see cref="InitializerValueObject"/> and <see cref="SchemaCache"/>
        /// This GetInitializer overload is called from applying <see cref="SdmxSoapValidatorAttribute"/> to Web Methods
        /// <seealso cref="System.Web.Services.Protocols.SoapExtension.GetInitializer(System.Web.Services.Protocols.LogicalMethodInfo, System.Web.Services.Protocols.SoapExtensionAttribute)"/>
        /// </summary>
        /// <param name="methodInfo">
        /// A <see cref="System.Web.Services.Protocols.LogicalMethodInfo"/> representing the specific function prototype for the XML Web service method to which the SOAP extension is applied. 
        /// </param>
        /// <param name="attribute">
        /// The <see cref="SdmxSoapValidatorAttribute"/> applied to the XML Web service method. 
        /// </param>
        /// <returns>
        /// The <see cref="InitializerValueObject"/> object
        /// </returns>
        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return this.GetInitiliaserCommon(methodInfo.MethodInfo.ReflectedType, methodInfo.Name);
        }

        /// <summary>
        /// Uses  <see cref="InitializerValueObject"/> object from GetInitializer to set the private field <see cref="_initialiserVo"/>
        /// <seealso cref="System.Web.Services.Protocols.SoapExtension.Initialize"/>
        /// </summary>
        /// <param name="initializer">
        /// The <see cref="InitializerValueObject"/> object
        /// </param>
        public override void Initialize(object initializer)
        {
            this._initialiserVo = initializer as InitializerValueObject;
        }

        /// <summary>
        /// This method processes the soap message received and send at various stages.
        /// Currently only the <see cref="System.Web.Services.Protocols.SoapMessageStage.BeforeDeserialize"/> is handled.
        /// <seealso cref="System.Web.Services.Protocols.SoapExtension.ProcessMessage"/>
        /// </summary>
        /// <param name="message">
        /// The <see cref="System.Web.Services.Protocols.SoapMessage"/> to process
        /// </param>
        public override void ProcessMessage(SoapMessage message)
        {
            this._initialiserVo.MethodName = message.MethodInfo.Name;

            switch (message.Stage)
            {
                case SoapMessageStage.BeforeDeserialize:
                    this.HandleBeforeDeserialize(message);
                    this._postBeforeDeserialize = true;
                    break;
                case SoapMessageStage.AfterSerialize:
                    if (message.Exception != null)
                    {
                        this.HandleException(message);
                        Copy(this._newStream, this._oldStream);
                        this._newStream.Dispose();
                        this._newStream = null;
                    }

                    break;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Copy the contents of <paramref name="source"/> to <paramref name="dest"/>.
        /// The <paramref name="source"/> will rewind.
        /// If <paramref name="source"/> and <paramref name="dest"/> are the same instance then nothing happens.
        /// </summary>
        /// <param name="source">
        /// The source stream
        /// </param>
        /// <param name="dest">
        /// The destination stream
        /// </param>
        private static void Copy(Stream source, Stream dest)
        {
            if (ReferenceEquals(source, dest))
            {
                return;
            }

            source.Position = 0;
            var buffer = new byte[4096];
            int len;
            while ((len = source.Read(buffer, 0, 4096)) > 0)
            {
                dest.Write(buffer, 0, len);
            }

            dest.Flush();
        }

        /// <summary>
        /// Checks if the soap message is valid with schema validation and checking if the soap operation matches the soap message operation
        /// </summary>
        /// <param name="xr">
        /// The <see cref="System.Xml.XmlReader"/> to the soap message
        /// </param>
        /// <param name="message">
        /// The soap message
        /// </param>
        private void CheckSoapXml(XmlReader xr, SoapMessage message)
        {
            xr.MoveToContent();
            string ns = xr.NamespaceURI;
            while (xr.Read() && !xr.IsStartElement(SoapConstants.Body, ns))
            {
            }

            if (xr.Read())
            {
                string webns = xr.NamespaceURI;
                if (!string.IsNullOrEmpty(this._initialiserVo.WsdlNamespace))
                {
                    if (!this._initialiserVo.WsdlNamespace.Equals(webns))
                    {
                        throw this.ThrowClientSoapException(
                            string.Format(
                                CultureInfo.CurrentCulture, 
                                Resources.error_expected_ns, 
                                this._initialiserVo.WsdlNamespace, 
                                webns));
                    }
                }

                // check if the soap message operation == actual operation
                if (!xr.IsStartElement(message.MethodInfo.Name, webns))
                {
                    throw this.ThrowClientSoapException(
                        string.Format(
                            CultureInfo.CurrentCulture, 
                            Resources.error_expected_op, 
                            xr.Prefix, 
                            message.MethodInfo.Name, 
                            xr.Name));
                }

                while (xr.Read())
                {
                }
            }
        }

        /// <summary>
        /// Creates a <see cref="System.Xml.XmlReaderSettings"/> object with various defaults and optionally a SchemaSet
        /// </summary>
        /// <param name="schemaSet">
        /// Optional <see cref="System.Xml.Schema.XmlSchemaSet"/>, when set it will be included in the created <see cref="System.Xml.XmlReaderSettings"/> object 
        /// </param>
        /// <returns>
        /// A <see cref="System.Xml.XmlReaderSettings"/> object
        /// </returns>
        private XmlReaderSettings CreateXmlReaderSettings(XmlSchemaSet schemaSet)
        {
            var xmlSettings = new XmlReaderSettings();
            if (schemaSet != null)
            {
                xmlSettings.ValidationType = ValidationType.Schema;
                xmlSettings.ValidationEventHandler -= this.XrsValidationEventHandler;
                xmlSettings.ValidationEventHandler += this.XrsValidationEventHandler;
                xmlSettings.Schemas = schemaSet;
            }

            xmlSettings.IgnoreComments = true;
            xmlSettings.IgnoreWhitespace = true;
            return xmlSettings;
        }

        /// <summary>
        /// Initialises the <see cref="InitializerValueObject"/> and <see cref="SchemaCache"/>
        /// </summary>
        /// <exception cref="System.Web.Services.Protocols.SoapException">
        /// A server error occured
        /// </exception>
        /// <param name="type">
        /// The type of the class implementing the XML Web service to which the SOAP extension is applied.
        /// </param>
        /// <param name="name">
        /// The web method name or the type name
        /// </param>
        /// <returns>
        /// The <see cref="InitializerValueObject"/> object
        /// </returns>
        private object GetInitiliaserCommon(Type type, string name)
        {
            try
            {
                var iv = new InitializerValueObject { MethodName = name };

                // iv.ValidateSoapBody = attribute.ValidateSoapBody;
                // InitSchemaCache(iv.ValidateSoapBody, type);
                var webAttributes = (WebServiceAttribute[])type.GetCustomAttributes(typeof(WebServiceAttribute), true);
                if (webAttributes.Length > 0)
                {
                    iv.WsdlNamespace = webAttributes[0].Namespace;
                }

                return iv;
            }
            catch (Exception ex)
            {
                throw this.ThrowServerSoapException(ex);
            }
        }

        /// <summary>
        /// Handle Before Deserialize <see cref="System.Web.Services.Protocols.SoapMessageStage"/> of the SoapMessage
        /// meaning before the server deserialises the soap message sent by the client.
        /// It creates an <see cref="System.Xml.XmlReader"/> from the SoapMessage stream and
        /// using the xml reader settings with the appopriate XmlSchemaSet depending is soap body validation is selected.
        /// </summary>
        /// <param name="message">
        /// The soap message as it was sent by the client
        /// </param>
        private void HandleBeforeDeserialize(SoapMessage message)
        {
            try
            {
                XmlReaderSettings settings = this.CreateXmlReaderSettings(SchemaCache.SoapSchema);
                using (XmlReader xr = XmlReader.Create(message.Stream, settings))
                {
                    this.CheckSoapXml(xr, message);
                }
            }
            catch (XmlException e)
            {
                throw this.ThrowClientSoapException(e.Message);
            }
            catch (InvalidOperationException e)
            {
                throw this.ThrowServerSoapException(e);
            }
            catch (ArgumentException e)
            {
                throw this.ThrowServerSoapException(e);
            }
            finally
            {
                message.Stream.Position = 0;
            }
        }

        /// <summary>
        /// Handle exceptions where the base exception is a <see cref="SoapException"/>.
        /// </summary>
        /// <param name="message">
        /// The Soap message
        /// </param>
        private void HandleException(SoapMessage message)
        {
            var soapException = message.Exception;
            if (soapException != null && soapException.Detail != null)
            {
                return;
            }

            soapException = message.Exception.GetBaseException() as SoapException;

            if (soapException == null)
            {
                return;
            }

            try
            {
                var document = new XmlDocument();

                this._newStream.Position = 0;
                document.Load(this._newStream);
                this._newStream.Dispose();
                this._newStream = null;

                var root = document.DocumentElement;
                if (root == null)
                {
                    return;
                }

                bool soap12 = message.SoapVersion == SoapProtocolVersion.Soap12;
                var xmlNamespaceManager = new XmlNamespaceManager(document.NameTable);
                xmlNamespaceManager.AddNamespace(SoapPrefix, root.NamespaceURI);

                var code = document.SelectSingleNode(
                    soap12 ? Soap12CodePath : SoapConstants.SoapCodePath, xmlNamespaceManager);
                if (code != null)
                {
                    code.Value = string.Format(
                        CultureInfo.InvariantCulture, "{0}:{1}", root.Prefix, soapException.Code.Name);
                }

                var reason =
                    document.SelectSingleNode(
                        soap12 ? SoapConstants.Soap12ReasonPath : SoapConstants.SoapReasonPath, xmlNamespaceManager);
                if (reason != null)
                {
                    var text = soap12
                                   ? reason.SelectSingleNode(SoapConstants.Soap12ReasonTextPath, xmlNamespaceManager)
                                   : reason;
                    if (text != null)
                    {
                        text.InnerText = soapException.Message;
                    }

                    var node = soap12
                                   ? document.CreateElement(
                                       root.Prefix, SoapConstants.Soap12ActorLocalName, root.NamespaceURI)
                                   : document.CreateElement(SoapConstants.SoapActorLocalName);
                    node.InnerText = message.MethodInfo.Name;
                    if (reason.ParentNode != null)
                    {
                        reason.ParentNode.InsertAfter(node, reason);
                    }
                }

                var details =
                    document.SelectSingleNode(
                        soap12 ? SoapConstants.Soap12DetailPath : SoapConstants.SoapDetailPath, xmlNamespaceManager);
                if (details != null)
                {
                    details.InnerXml = soapException.Detail.InnerXml;
                }

                this._newStream = new MemoryStream();
                document.Save(this._newStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        /// Creates a client SoapException
        /// </summary>
        /// <param name="message">
        /// The message that should appear at Details/ErrorMessage
        /// </param>
        /// <returns>
        /// A SoapException
        /// </returns>
        private SoapException ThrowClientSoapException(string message)
        {
            return SoapFaultFactory.CreateSoapException(
                this._initialiserVo.MethodName, 
                string.Empty, 
                message, 
                Resources.error_client_code, 
                this._initialiserVo.MethodName, 
                true, 
                Resources.error_client_msg);
        }

        /// <summary>
        /// Creates a server SoapException
        /// </summary>
        /// <param name="ex">
        /// The exception that causes this error
        /// </param>
        /// <returns>
        /// A SoapException
        /// </returns>
        private SoapException ThrowServerSoapException(Exception ex)
        {
            // TODO log server exceptions
            string source = this.GetType().Name;
            if (this._initialiserVo != null)
            {
                source = this._initialiserVo.MethodName;
            }

            return SoapFaultFactory.CreateSoapException(
                source, 
                string.Empty, 
                ex.ToString(), 
                Resources.error_server_code, 
                source, 
                false, 
                Resources.error_server_msg);
        }

        /// <summary>
        /// Handle client validation errors, i.e. in soap message
        /// </summary>
        /// <param name="sender">
        /// The object that initiated this event
        /// </param>
        /// <param name="e">
        /// The event args
        /// </param>
        private void XrsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            throw this.ThrowClientSoapException(e.Message);
        }

        #endregion
    }
}