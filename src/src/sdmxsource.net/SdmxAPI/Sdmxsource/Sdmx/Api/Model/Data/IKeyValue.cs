// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IKeyValue.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Data
{
    #region Using directives

    using System;

    #endregion

    /// <summary>
    ///     A Dimensions point ties a code and concept together to represent an identifier
    ///     for an item of data
    /// </summary>
    public interface IKeyValue : IComparable<IKeyValue>
    {
        #region Public Properties

        /// <summary>
        ///     Gets the code that the value is for
        /// </summary>
        /// <value> </value>
        string Code { get; }

        /// <summary>
        ///     Gets the key (concept/dimension) that this KeyValue is for
        /// </summary>
        /// <value> </value>
        string Concept { get; }

        #endregion
    }
}