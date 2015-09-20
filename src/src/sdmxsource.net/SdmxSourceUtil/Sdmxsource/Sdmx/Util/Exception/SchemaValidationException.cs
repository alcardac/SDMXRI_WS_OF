// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SchemaValidationException.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Exception
{
    #region Using directives

    using System;
    using System.Collections.Generic;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Exception;

    #endregion


    /// <summary>
    /// The schema validation exception.
    /// </summary>
    [Serializable]
    public class SchemaValidationException : SdmxSyntaxException
    {
        #region Constants

        /// <summary>
        /// The serial version1 uid.
        /// </summary>
        private const long SerialVersionUid = 1L;

        #endregion

        #region Fields

        private readonly IList<string> _validationErrors = new List<string>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaValidationException"/> class.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        public SchemaValidationException(Exception args)
            : base(args)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaValidationException"/> class.
        /// </summary>
        /// <param name="errors">
        /// The errors
        /// </param>
        public SchemaValidationException(IList<string> errors)
            : base(MergeErrors(errors))
        {
		    foreach(string currentError in errors) 
            {
			    _validationErrors.Add(currentError);
		    }
	    }

        #endregion


        #region Public Methods and Operators

        /// <summary>
        /// Return the Validation error IList
        /// </summary>
        /// <returns>
        /// The IList of string
        /// </returns>
        public IList<string> GetValidationErrors()
        {
            return new List<string>(this._validationErrors);
        }

        #endregion


        #region Methods

        /// <summary>
        /// Return a string based on error list parameters
        /// </summary>
        /// <param name="errors">
        /// The list of errors
        /// </param>
        /// <returns>
        /// The error string
        /// </returns>
        private static string MergeErrors(IList<string> errors) {
		    var sb = new StringBuilder();

		    foreach(string currentError in errors) 
            {
			    sb.Append(currentError);
		    }

		    return sb.ToString();
	    }


        #endregion

    }
}