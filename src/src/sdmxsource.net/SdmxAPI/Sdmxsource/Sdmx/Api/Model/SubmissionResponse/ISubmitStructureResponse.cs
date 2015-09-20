// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISubmitStructureResponse.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.SubmissionResponse
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     Created after a submission of a document to a SDMX web service
    /// </summary>
    public interface ISubmitStructureResponse
    {
        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether the getErrorList() returns a not null ErrorList, and the returned ErrorList is of type error (not warning)
        /// </summary>
        /// <value> </value>
        bool IsError { get; }

        /// <summary>
        ///     Gets the list of errors, returns null if there were no errors for this response (if it was a success)
        /// </summary>
        /// <value> </value>
        IErrorList ErrorList { get; }

        /// <summary>
        ///     Gets the structure that this response is for - this may be null if there were errors in the submission
        /// </summary>
        /// <value> </value>
        IStructureReference StructureReference { get; }

        #endregion
    }
}