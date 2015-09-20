// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResponseGenerator.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The ResponseGenerator interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Controller
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// The ResponseGenerator interface.
    /// </summary>
    /// <typeparam name="TWriter">
    /// The response writer type
    /// </typeparam>
    /// <typeparam name="TQuery">
    /// The type of the query.
    /// </typeparam>
    public interface IResponseGenerator<in TWriter, in TQuery>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Generates the response function.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <returns>
        /// The <see cref="Action"/> that will write the response.
        /// </returns>
        Action<TWriter, Queue<Action>> GenerateResponseFunction(TQuery query);

        #endregion
    }
}