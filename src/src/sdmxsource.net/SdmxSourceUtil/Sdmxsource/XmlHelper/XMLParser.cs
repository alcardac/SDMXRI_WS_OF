// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMLParser.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.XmlHelper
{
    #region Using directives

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Util.Log;

    #endregion


    /// <summary>
    ///     The xml parser.
    /// </summary>
    public class XMLParser
    {
        #region Static Fields

        /// <summary>
        ///     The _schema locations.
        /// </summary>
        private static readonly IDictionary<SdmxSchemaEnumType, XmlSchemaSet> _schemaLocations =
            new Dictionary<SdmxSchemaEnumType, XmlSchemaSet>();

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(XMLParser));

        /// <summary>
        ///     The enable validation.
        /// </summary>
        private static bool _enableValidation = true;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="XMLParser" /> class.
        /// </summary>
        static XMLParser()
        {
            var schemaSet = new XmlSchemaSet { XmlResolver = new XmlEmbededResourceResolver("Org.xsd._1_0") };
            schemaSet.Add(null, "res:///SDMXMessage.xsd");
            schemaSet.Compile();
            _schemaLocations.Add(SdmxSchemaEnumType.VersionOne, schemaSet);

            schemaSet = new XmlSchemaSet { XmlResolver = new XmlEmbededResourceResolver("Org.xsd._2_0") };
            schemaSet.Add(null, "res:///SDMXMessage.xsd");

            // TODO do we add ESTAT or MT Extensions ?!?
            _schemaLocations.Add(SdmxSchemaEnumType.VersionTwo, schemaSet);

            schemaSet = new XmlSchemaSet { XmlResolver = new XmlEmbededResourceResolver("Org.xsd._2_1") };
            schemaSet.Add(null, "res:///SDMXMessage.xsd");
            _schemaLocations.Add(SdmxSchemaEnumType.VersionTwoPointOne, schemaSet);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Sets a value indicating whether enable validation.
        /// </summary>
        public bool EnableValidation
        {
            set
            {
                _enableValidation = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Creates and returns an new instance of an <see cref="XmlReader"/> with the settings from
        ///     <see cref="GetSdmxXmlReaderSettings"/>
        ///     .
        /// </summary>
        /// <param name="stream">
        /// The input stream
        /// </param>
        /// <param name="sdmxSchema">
        /// The SDMX Schema version
        /// </param>
        /// <returns>
        /// A new instance of an <see cref="XmlReader"/> with the settings from <see cref="GetSdmxXmlReaderSettings"/>
        /// </returns>
        public static XmlReader CreateSdmxMlReader(Stream stream, SdmxSchemaEnumType sdmxSchema)
        {
            return XmlReader.Create(stream, GetSdmxXmlReaderSettings(sdmxSchema));
        }

        /// <summary>
        /// Creates and returns an new instance of an <see cref="XmlReaderSettings"/> for the specified
        ///     <paramref name="sdmxSchema"/>
        /// </summary>
        /// <param name="sdmxSchema">
        /// The SDMX Schema version
        /// </param>
        /// <returns>
        /// A new instance of an <see cref="XmlReaderSettings"/> for the specified <paramref name="sdmxSchema"/>
        /// </returns>
        public static XmlReaderSettings GetSdmxXmlReaderSettings(SdmxSchemaEnumType sdmxSchema)
        {
            var settings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true
            };

            XmlSchemaSet schemaSet;
            if (_enableValidation && _schemaLocations.TryGetValue(sdmxSchema, out schemaSet))
            {
                settings.Schemas = schemaSet;
                settings.ValidationType = ValidationType.Schema;
            }
            else
            {
                settings.ValidationType = ValidationType.None;
            }

            return settings;
        }

        /// <summary>
        /// Validate SDMX-ML file.
        /// </summary>
        /// <param name="sourceData">
        /// The source data.
        /// </param>
        /// <param name="schemaVersion">
        /// The schema version.
        /// </param>
        public static void ValidateXml(Stream sourceData, SdmxSchemaEnumType schemaVersion)
        {
            XmlReaderSettings settings = GetSdmxXmlReaderSettings(schemaVersion);
            settings.ValidationEventHandler += SettingsOnValidationEventHandler;

            using (XmlReader reader = XmlReader.Create(sourceData, settings))
            {
                while (reader.Read())
                {
                }
            }
        }

        /// <summary>
        /// Validate SDMX-ML file.
        /// </summary>
        /// <param name="sourceData">
        /// The source data.
        /// </param>
        /// <param name="schemaVersion">
        /// The schema version.
        /// </param>
        public static void ValidateXml(IReadableDataLocation sourceData, SdmxSchemaEnumType schemaVersion)
        {
            ValidateXml(sourceData.InputStream, schemaVersion);
        }

        /// <summary>
        /// Validates the XML against the schema version.  Throws a validation exception if any schema errors are found.
        ///     <p/>
        ///     <b>NOTE :</b>In order for this method to work, the schema location must be set, and be accessible via a uri.
        /// </summary>
        /// <param name="sourceData">
        /// The source Data.
        /// </param>
        /// <param name="schemaVersion">
        /// - the schema version to validate against
        /// </param>
        /// <param name="extraLocations">
        /// - any extra schemas required for validation
        /// </param>
        /// <exception cref="SdmxSyntaxException">
        /// Throws Validation Exception
        /// </exception>
        public static void ValidateXml(
            IReadableDataLocation sourceData,
            SdmxSchemaEnumType schemaVersion,
            params IReadableDataLocation[] extraLocations)
        {
            ValidateXml(sourceData.InputStream, schemaVersion, extraLocations);
        }

        /// <summary>
        /// The validate xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <param name="schemaVersion">
        /// The schema version.
        /// </param>
        /// <param name="extraLocations">
        /// The extra locations.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// Throws NotImplementedException
        /// </exception>
        public static void ValidateXml(
            Stream xml, SdmxSchemaEnumType schemaVersion, params IReadableDataLocation[] extraLocations)
        {
            throw new NotImplementedException("Not implememented. Not used in SDMX-RI");

            ////LoggingUtil.Debug(_log, "Validate XML Enabled :" + _enableValidation);
            ////if (!enableValidation) {
            ////  LoggingUtil.Warn(_log, "Validation disabled");
            ////  return;
            ////}
            ////if (!_schemaLocations.ContainsKey(schemaVersion)) {
            ////  throw new ArgumentException(
            ////          "Schema location has not been set for schema : "
            ////                  + schemaVersion);
            ////}
            ////
            ////if(extraLocations == null) {
            ////    Uri schema = _schemaLocations[schemaVersion];
            ////    ValidateXML(xml, schema, schemaURI);
            ////} else {
            ////    ValidateXML(xml, _schemaLocations[schemaVersion]);
            ////}
        }

        /// <summary>
        /// The validate xml.
        /// </summary>
        /// <param name="xmlLocation">
        /// The xml location.
        /// </param>
        /// <param name="schemaLocation">
        /// The schema location.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// Throws NotImplementedException
        /// </exception>
        public static void ValidateXml(Stream xmlLocation, params IReadableDataLocation[] schemaLocation)
        {
            _log.Debug("Validate XML Enabled :" + _enableValidation);
            if (!_enableValidation)
            {
                _log.Warn("Validation disabled");
                return;
            }

            ////if (log.IsDebugEnabled) {
            ////  log.Debug("Validate XML : " + xmlLocation.ToString()
            ////          + " against schema in location(s):");
            ////  for (int i = 0; i < schemaLocation.Length; i++) {
            ////      log.Debug(schemaLocation[i].ToString());
            ////  }
            ////}
            throw new NotImplementedException("Not implememented. Not used in SDMX-RI");

            ////try {
            ////SchemaFactory schemaFactory = Javax.Xml.Validation.SchemaFactory
            ////      .NewInstance("http://www.w3.org/2001/XMLSchema");

            ////Source[] schemaSource = new Source[schemaLocation.Length];
            ////for (int i_0 = 0; i_0 < schemaLocation.Length; i_0++) {
            ////  schemaSource[i_0] = new XmlReader(schemaLocation[i_0].ToString());
            ////}
            ////Schema schema = schemaFactory.NewSchema(schemaSource);
            ////Validator schemaValidator = schema.NewValidator();

            ////XMLParser.ErrorHandler  eh = new XMLParser.ErrorHandler ();
            ////schemaValidator.SetErrorHandler(eh);

            ////XmlReader source = XmlReader.Create(xmlLocation);
            ////schemaValidator.Validate(source);
            ////if (eh.Errors.Count > 0) {
            ////  // Create a "report" for all of the errors that occurred
            ////  StringBuilder buffer = new StringBuilder();
            ////  /* foreach */
            ////  foreach (SAXParseException ex in eh.Errors) {
            ////      String message = ex.Message + "line/column: "
            ////              + ex.GetLineNumber() + "/" + ex.GetColumnNumber();
            ////      buffer.Append(message);
            ////      buffer.Append(System.Console.GetProperties().GetProperty(
            ////              "line.separator"));
            ////  }

            ////  throw new SdmxSemmanticException(buffer.ToString());
            ////}
            ////} catch (Exception th) {
            ////  throw new SchemaValidationException(th.Message);
            ////}
        }

        #endregion

        #region Methods

        /// <summary>
        /// The settings on validation event handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The validation event parameter.
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Schema validation error
        /// </exception>
        private static void SettingsOnValidationEventHandler(object sender, ValidationEventArgs args)
        {
            Trace.WriteLine(args.Message);
            if (args.Exception == null)
            {
                return;
            }
            
            Trace.WriteLine(args.Exception);
            string message = string.Format(
                "{0} Line: {1}. Column {2} Error:\n {3}",
                args.Exception.SourceUri,
                args.Exception.LineNumber,
                args.Exception.LinePosition,
                args.Exception.Message);
            Trace.WriteLine(message);
            _log.Warn(message, args.Exception);

            throw new SdmxSyntaxException(args.Exception, ExceptionCode.XmlParseException, message);
        }

        #endregion

        /////*
        //// * This class is a bit of a work-around for a possible bug that exists within the validate routine of the Validator class.
        //// * When validating an XML file against a schema the problem is that if the validation of the file fails, 
        //// * then the file becomes locked until the application terminates. 
        //// * If the input file was valid then the file does not become locked and everything is fine. 
        //// * So, when an error is encountered, it is simply stored in a List and validation continues successfully.
        //// * Once the validation has completed, this class must be asked if there were any errors and act accordingly.
        //// */
        ////protected internal class ErrorHandler : DefaultHandler {
        ////  public ErrorHandler() {
        ////      this.errors = new List<SAXParseException>();
        ////  }

        ////  private IList<SAXParseException> errors;

        ////  public IList<SAXParseException> Errors {
        ////    get {
        ////              return errors;
        ////          }
        ////  }

        ////  public override void Error(SAXParseException parseException) {
        ////      LoggingUtil.Error(_log, parseException);
        ////      ILOG.J2CsMapping.Collections.Generics.Collections.Add(errors,parseException);
        ////  }

        ////  public override void FatalError(SAXParseException parseException) {
        ////      LoggingUtil.Error(_log, parseException);
        ////      ILOG.J2CsMapping.Collections.Generics.Collections.Add(errors,parseException);
        ////  }

        ////  public override void Warning(SAXParseException parseException) {
        ////      LoggingUtil.Error(_log, parseException);
        ////      ILOG.J2CsMapping.Collections.Generics.Collections.Add(errors,parseException);
        ////  }
        ////}
    }
}