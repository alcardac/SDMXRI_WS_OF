// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHeaderRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Header;

    #endregion

    /// <summary>
    ///     Implementations of this interface are responsible for supplying details that can be used to construct a header
    /// </summary>
    public interface IHeaderRetrievalManager
    {
        #region Public Properties

        /// <summary>
        ///     Gets a header object
        /// </summary>
        /// <value> </value>
        IHeader Header { get; }

        #endregion
    }
}