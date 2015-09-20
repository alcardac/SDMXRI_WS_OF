// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IController.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Web Service Controller interface for streaming output
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Controller
{
    /// <summary>
    /// Web Service Controller interface for streaming output
    /// </summary>
    /// <typeparam name="T">
    /// The type of the request.
    /// </typeparam>
    /// <typeparam name="TWriter">
    /// The type of the writer.
    /// </typeparam>
    public interface IController<in T, in TWriter>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Parse request from <paramref name="input"/>
        /// </summary>
        /// <param name="input">
        /// The reader for the SDMX-ML or REST request
        /// </param>
        /// <returns>
        /// The <see cref="IStreamController{TWriter}"/>.
        /// </returns>
        IStreamController<TWriter> ParseRequest(T input);

        #endregion
    }
}