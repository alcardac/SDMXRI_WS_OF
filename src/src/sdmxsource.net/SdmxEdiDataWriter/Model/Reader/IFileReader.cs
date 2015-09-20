// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileReader.cs" company="Eurostat">
//   Date Created : 2014-07-22
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The FileReader interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Model.Reader
{
    using System;
    using System.IO;

    /// <summary>
    /// The FileReader interface.
    /// </summary>
    public interface IFileReader : IDisposable
    {
        #region Public Properties

        /// <summary>
        ///     Gets the current line.  minus the prefix, if the prefix is unknown then an exception is thrown
        /// </summary>
        /// <value>
        ///     The current line.
        /// </value>
        string CurrentLine { get; }

        /// <summary>
        ///     Gets the line number of the current line, the first line being '1'.
        /// </summary>
        /// <value>
        ///     The line number.
        /// </value>
        int LineNumber { get; }

        /// <summary>
        ///     Gets the next line. Move the file pointer to the next line and returns that line.
        ///     Null is returned if there is no next line.
        ///     This method is almost the same as calling <see cref="MoveNext" /> followed by <see cref="CurrentLine" /> the
        ///     difference is if there is no next line,
        ///     the call to <see cref="CurrentLine" /> will the same result as it did prior to this call
        /// </summary>
        /// <value>
        ///     The next line.
        /// </value>
        string NextLine { get; }

        /// <summary>
        /// Gets a value indicating whether the reader has flagged this to move back a line, as moving back a line does not actually change the current line
        /// </summary>
        /// <value>
        /// <c>true</c> if the reader has flagged this to move back a line, as moving back a line does not actually change the current line; otherwise, <c>false</c>.
        /// </value>
        bool IsBackLine { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Closes this instance. Close the reader and any resources associated with the reader
        /// </summary>
        void Close();

        /// <summary>
        /// Copies the EDI file to the specified <paramref name="output"/>
        /// </summary>
        /// <param name="output">
        /// The output.
        /// </param>
        void CopyToStream(Stream output);

        /// <summary>
        ///     Moves the reader back a single line.  This can not be called to iterate backwards - it will only move back one line
        /// </summary>
        void MoveBackLine();

        /// <summary>
        ///     Moves the file pointer to the next line.
        /// </summary>
        /// <returns>false if there is no next line.</returns>
        bool MoveNext();

        /// <summary>
        ///     Move the reader back to the start of the document.
        /// </summary>
        void ResetReader();

        #endregion
    }
}