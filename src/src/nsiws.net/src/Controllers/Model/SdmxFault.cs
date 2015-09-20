// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxFault.cs" company="Eurostat">
//   Date Created : 2013-10-24
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The SDMX Fault.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Model
{
    using System.Runtime.Serialization;

    /// <summary>
    ///     The SDMX Fault.
    /// </summary>
    [DataContract(Name = "Error", Namespace = "")]
    public class SdmxFault
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxFault"/> class.
        /// </summary>
        /// <param name="errorMessage">
        /// The error message.
        /// </param>
        /// <param name="errorNumber">
        /// The error number.
        /// </param>
        /// <param name="errorSource">
        /// The error source.
        /// </param>
        public SdmxFault(string errorMessage, int errorNumber, string errorSource)
        {
            this.ErrorMessage = errorMessage;
            this.ErrorNumber = errorNumber;
            this.ErrorSource = errorSource;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the error message.
        /// </summary>
        /// <value>
        ///     The error message.
        /// </value>
        [DataMember]
        public string ErrorMessage { get; private set; }

        /// <summary>
        ///     Gets the error number.
        /// </summary>
        /// <value>
        ///     The error number.
        /// </value>
        [DataMember]
        public int ErrorNumber { get; private set; }

        /// <summary>
        ///     Gets the error source.
        /// </summary>
        /// <value>
        ///     The error source.
        /// </value>
        [DataMember]
        public string ErrorSource { get; private set; }

        #endregion
    }
}