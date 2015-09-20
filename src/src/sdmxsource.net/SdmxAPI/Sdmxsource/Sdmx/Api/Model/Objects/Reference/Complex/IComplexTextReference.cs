// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComplexTextReference.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// Used to search for items with certain text fields 
    /// </summary>
    public interface IComplexTextReference
    {
        /// <summary>
        /// Gets the language to seach, this can be null
        /// </summary>
        string Language { get; }

        /// <summary>
        /// Gets the operator to apply, can not be null, defaults to EQUAL
        /// </summary>
        TextSearch Operator { get; }

        /// <summary>
        /// Gets the seach String, can not be null or empty
        /// </summary>
        string SearchParameter { get; }
    }
}
