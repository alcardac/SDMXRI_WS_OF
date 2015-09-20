// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitializerValueObject.cs" company="Eurostat">
//   Date Created : 2010-11-24
//   Copyright (c) 2010 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   A VO class that holds the data needed to be passed from <see cref="SdmxSoapValidator.GetInitializer(System.Type)" /> to <see cref="SdmxSoapValidator.Initialize" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.SdmxSoapValidatorExtension
{
    /// <summary>
    /// A VO class that holds the data needed to be passed from <see cref="SdmxSoapValidator.GetInitializer(System.Type)"/> to <see cref="SdmxSoapValidator.Initialize"/>
    /// </summary>
    public sealed class InitializerValueObject
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets name of the called Web Method
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Gets or sets name space used by the current WSDL
        /// </summary>
        public string WsdlNamespace { get; set; }

        #endregion

        ///// <summary>
        ///// Whether to validation soap body
        ///// </summary>
        // public bool ValidateSoapBody { get; set; }
    }
}