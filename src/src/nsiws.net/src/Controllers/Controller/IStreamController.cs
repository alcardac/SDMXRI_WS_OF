// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamController.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The Stream controller
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Controller
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The Stream controller
    /// </summary>
    /// <typeparam name="T">
    /// The type of the output writer
    /// </typeparam>
    public interface IStreamController<in T>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Stream XML output to <paramref name="writer"/>
        /// </summary>
        /// <param name="writer">
        ///     The writer to write the output to
        /// </param>
        /// <param name="actions"></param>
        void StreamTo(T writer, Queue<Action> actions);

        #endregion
    }
}