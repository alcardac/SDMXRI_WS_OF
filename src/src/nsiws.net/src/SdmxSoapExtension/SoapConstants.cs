// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SoapConstants.cs" company="Eurostat">
//   Date Created : 2011-10-21
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   SOAP XML Tags
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.SdmxSoapValidatorExtension
{
    /// <summary>
    /// SOAP XML Tags
    /// </summary>
    public static class SoapConstants
    {
        #region Constants and Fields

        /// <summary>
        /// SOAP Body tag
        /// </summary>
        public const string Body = "Body";

        /// <summary>
        /// The local name of the SOAP 1.2 Fault Actor/Node element
        /// </summary>
        public const string Soap12ActorLocalName = "Node";

        /// <summary>
        /// XPath of SOAP 1.2 Detail element
        /// </summary>
        public const string Soap12DetailPath = "//soap:Fault/soap:Detail";

        /// <summary>
        /// XPath of SOAP 1.2 Reason/Fault String element
        /// </summary>
        public const string Soap12ReasonPath = "//soap:Reason";

        /// <summary>
        /// XPath of SOAP 1.2 Reason text element
        /// </summary>
        public const string Soap12ReasonTextPath = "soap:Text";

        /// <summary>
        /// The local name of the SOAP Fault Actor/Node element
        /// </summary>
        public const string SoapActorLocalName = "faultactor";

        /// <summary>
        /// XPath of SOAP Fault Code text
        /// </summary>
        public const string SoapCodePath = "//faultcode/text()";

        /// <summary>
        /// XPath of SOAP Detail element
        /// </summary>
        public const string SoapDetailPath = "//soap:Fault/detail";

        /// <summary>
        /// XPath of SOAP Reason/Fault String element
        /// </summary>
        public const string SoapReasonPath = "//faultstring";

        #endregion
    }
}