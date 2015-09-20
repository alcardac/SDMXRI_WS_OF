// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxSoapValidatorAttribute.cs" company="Eurostat">
//   Date Created : 2010-11-24
//   Copyright (c) 2010 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Attribute that enables validation of Soap Messages and optionally the soap body contents using the current service WSDL and SDMX schema
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.SdmxSoapValidatorExtension
{
    using System;
    using System.Web.Services.Protocols;

    /// <summary>
    /// Attribute that enables validation of Soap Messages and optionally the soap body contents using the current service WSDL and SDMX schema
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class SdmxSoapValidatorAttribute : SoapExtensionAttribute
    {
        /*/// <summary>
        /// Whether to require the parameter container element under operation in SOAP message
        /// Defaults to : true
        /// </summary>
        bool _requireParameterContainer = true;*/
        #region Public Properties

        /// <summary>
        /// Getter for the SOAP Extension type. Which is in all cases <see cref="SdmxSoapValidator"/>
        /// </summary>
        public override Type ExtensionType
        {
            get
            {
                /*       if (_requireParameterContainer)
                {*/
                return typeof(SdmxSoapValidator);

                /*}
                else
                {
                    return typeof(SdmxSoapValidatorNoParameter);
                }*/
            }
        }

        /// <summary>
        /// Gets or sets the priority of the SOAP Extension
        /// Defaults to : 0
        /// </summary>
        public override int Priority { get; set; }

        #endregion

     
    }
}