// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamController.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The stream controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace RDFProvider.Controller
{
    using System;
    using System.Collections.Generic;
    using Estat.Sri.Ws.Controllers.Controller;

    /// <summary>
    /// The stream controller.
    /// </summary>
    /// <typeparam name="TWriter">
    /// The type of the writer.
    /// </typeparam>
    public class StreamController<TWriter> : IStreamController<TWriter>
    {
        #region Fields

        /// <summary>
        ///     The _action.
        /// </summary>
        private readonly Action<TWriter, Queue<Action>> _action;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamController{TWriter}"/> class.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="action"/> is null.
        /// </exception>
        public StreamController(Action<TWriter, Queue<Action>> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this._action = action;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Stream XML output to <paramref name="writer"/>
        /// </summary>
        /// <param name="writer">
        ///     The writer to write the output to
        /// </param>
        /// <param name="actions"></param>
        public void StreamTo(TWriter writer, Queue<Action> actions)
        {
            this._action(writer, actions);
        }

        #endregion
    }
}