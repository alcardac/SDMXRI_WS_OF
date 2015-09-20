// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorListImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The error list core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The error list core.
    /// </summary>
    public class ErrorListCore : IErrorList
    {
        #region Fields

        /// <summary>
        ///   The _error messages.
        /// </summary>
        private readonly IList<string> _errorMessages;

        /// <summary>
        ///   The _is warning.
        /// </summary>
        private readonly bool _isWarning;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorListCore"/> class.
        /// </summary>
        /// <param name="errorMessages">
        /// The error messages. 
        /// </param>
        /// <param name="isWarning">
        /// The is warning. 
        /// </param>
        /// ///
        /// <exception cref="ArgumentException">
        /// Throws ArgumentException.
        /// </exception>
        public ErrorListCore(IList<string> errorMessages, bool isWarning)
        {
            if (!ObjectUtil.ValidCollection(errorMessages))
            {
                throw new ArgumentException("ErrorListCore requires error message to be provided");
            }

            this._errorMessages = errorMessages;
            this._isWarning = isWarning;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the error message.
        /// </summary>
        public virtual IList<string> ErrorMessage
        {
            get
            {
                return new List<string>(this._errorMessages);
            }
        }

        /// <summary>
        ///   Gets a value indicating whether warning.
        /// </summary>
        public virtual bool Warning
        {
            get
            {
                return this._isWarning;
            }
        }

        #endregion
    }
}