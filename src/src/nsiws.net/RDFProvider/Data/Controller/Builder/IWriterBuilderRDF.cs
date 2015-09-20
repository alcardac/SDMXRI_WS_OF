// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWriterBuilder.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The WriterBuilder interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace RDFProvider.Controller.Builder
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The WriterBuilder interface.
    /// </summary>
    /// <typeparam name="TEngine">
    /// The type of the engine to build
    /// </typeparam>
    /// <typeparam name="TWriter">
    /// The type of the writer.
    /// </typeparam>
    public interface IWriterBuilderRDF<out TEngine, in TWriter>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds the specified writer engine.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="actions">The actions.</param>
        /// <returns>
        /// The <see cref="TEngine" />.
        /// </returns>
        TEngine Build(TWriter writer, Queue<Action> actions);

        #endregion
    }
}
