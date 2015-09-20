// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IErrorList.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model
{
    #region Using directives

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     Contains a list of error messages
    /// </summary>
    public interface IErrorList
    {
        #region Public Properties

        /// <summary>
        ///     Gets a copy of the list, the first in the list is the last error in the stack, the last item in the list is the
        ///     originating error message
        /// </summary>
        /// <value> </value>
        IList<string> ErrorMessage { get; }

        /// <summary>
        ///     Gets a value indicating whether the is just a warning
        /// </summary>
        /// <value> </value>
        bool Warning { get; }

        #endregion
    }
}