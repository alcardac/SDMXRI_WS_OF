// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SchemaCache.cs" company="Eurostat">
//   Date Created : 2010-11-24
//   Copyright (c) 2010 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   A helper class that keeps a cached copy of the various schema sets
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.SdmxSoapValidatorExtension
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Web.Services.Protocols;
    using System.Xml;
    using System.Xml.Schema;

    using Estat.Nsi.SdmxSoapValidatorExtension.Properties;

    /// <summary>
    /// A helper class that keeps a cached copy of the various schema sets
    /// </summary>
    public class SchemaCache
    {
        #region Constants and Fields

        /// <summary>
        /// Constant string that contains the resource location of soap11 xsd
        /// </summary>
        private const string Soap11Location = "Estat.Nsi.SdmxSoapValidatorExtension.soap11.xsd";

        /// <summary>
        /// Constant string that contains the resource location of soap12 xsd
        /// </summary>
        private const string Soap12Location = "Estat.Nsi.SdmxSoapValidatorExtension.soap12.xsd";

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static readonly SchemaCache _instance = new SchemaCache();

        /// <summary>
        /// This XmlSchemaSet field is used to hold the soap 1.1 and soap 1.2 schemas
        /// </summary>
        private readonly XmlSchemaSet _soapSchema;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="SchemaCache"/> class from being created. 
        /// Initialize a new instance of the <see cref="SchemaCache"/> class
        /// </summary>
        private SchemaCache()
        {
            var schemaSet = new XmlSchemaSet();
            schemaSet.ValidationEventHandler -= this.SchemaSetValidationEventHandler;
            schemaSet.ValidationEventHandler += this.SchemaSetValidationEventHandler;
            schemaSet.Add(this.LoadXsdFromResource(GetSoapXsdLocation(SoapProtocolVersion.Soap11)));
            schemaSet.Add(this.LoadXsdFromResource(GetSoapXsdLocation(SoapProtocolVersion.Soap12)));
            this._soapSchema = schemaSet;
            this._soapSchema.Compile();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the soap 1.1, soap 1.2, the web server wsdl and sdmx schemas
        /// </summary>
        public static XmlSchemaSet SoapBodySchema { get; set; }

        /// <summary>
        /// Gets the soap 1.1 and soap 1.2 schemas
        /// </summary>
        public static XmlSchemaSet SoapSchema
        {
            get
            {
                return _instance._soapSchema;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This getter returns the resource location of the soap schemas
        /// </summary>
        /// <param name="version">
        /// The Soap version
        /// </param>
        /// <returns>
        /// The resource location
        /// </returns>
        public static string GetSoapXsdLocation(SoapProtocolVersion version)
        {
            switch (version)
            {
                case SoapProtocolVersion.Soap11:
                    return Soap11Location;
                default:
                    return Soap12Location;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads a xsd file from an embeded resource to a <see cref="System.Xml.Schema.XmlSchema"/> object
        /// </summary>
        /// <param name="name">
        /// The resource location of the xsd 
        /// </param>
        /// <returns>
        /// The xsd file as a <see cref="System.Xml.Schema.XmlSchema"/> object
        /// </returns>
        private XmlSchema LoadXsdFromResource(string name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            XmlSchema schema;

            using (Stream manifestResourceStream = assembly.GetManifestResourceStream(name))
            {
                if (manifestResourceStream != null)
                {
                    using (XmlReader xr = XmlReader.Create(manifestResourceStream))
                    {
                        schema = XmlSchema.Read(xr, this.SchemaSetValidationEventHandler);
                    }
                }
                else
                {
                    schema = new XmlSchema();
                }
            }

            return schema;
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
        /// The handler for schema validation errors. It throws a server Soap Exception
        /// </summary>
        /// <param name="sender">
        /// The XmlSchemaSet object that initiated this event
        /// </param>
        /// <param name="e">
        /// The event argument
        /// </param>
        private void SchemaSetValidationEventHandler(object sender, ValidationEventArgs e)
        {
            throw this.ThrowServerSoapException(e.Exception);
        }

        #endregion
    }
}