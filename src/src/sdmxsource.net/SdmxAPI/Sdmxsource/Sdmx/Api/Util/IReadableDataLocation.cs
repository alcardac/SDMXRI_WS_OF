// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReadableDataLocation.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Util
{
    #region Using directives

    using System;
    using System.IO;

    #endregion

    /// <summary>
    ///     ReadableDataLocation is capable of reading and re-reading the same source of data many times
    /// </summary>
    /// <example>
    ///     A sample implementation in C# of <see cref="IReadableDataLocation" />
    ///     <code source="..\ReUsingExamples\DataQuery\ReUsingDataQueryParsingManager.cs" lang="cs" />
    /// </example>
    public interface IReadableDataLocation : IDisposable
    {
        #region Public Properties

        /// <summary>
        ///     Gets a new stream. This method is guaranteed to return a new input stream on each method call.
        ///     The input stream will be reading the same underlying data source.
        /// </summary>
        Stream InputStream { get; }

        /// <summary>
        /// Gets the name. If this ReadableDataLocation originated from a file, then this will be the original file name, regardless of where the stream is held.
        /// This may return null if it is not relevant.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Closes (and removes if appropriate) any resources that are held open
        /// </summary>
        void Close();

        #endregion
    }
}