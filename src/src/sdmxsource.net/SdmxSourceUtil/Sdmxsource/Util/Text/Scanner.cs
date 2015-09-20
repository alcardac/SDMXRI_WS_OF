// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Scanner.cs" company="Eurostat">
//   Date Created : 2014-07-25
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The scanner.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Util.Text
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The scanner.
    /// </summary>
    public class Scanner : IDisposable
    {
        #region Fields

        /// <summary>
        /// The _reader.
        /// </summary>
        private readonly TextReader _reader;

        /// <summary>
        /// The remainder.
        /// </summary>
        private readonly StringBuilder _remainder = new StringBuilder();

        /// <summary>
        /// The _tokens.
        /// </summary>
        private readonly Queue<string> _tokens = new Queue<string>();

        /// <summary>
        /// The _disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// The _pattern.
        /// </summary>
        private Regex _pattern;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Scanner"/> class.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        public Scanner(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            this._reader = reader;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scanner"/> class.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <param name="charset">
        /// The charset.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="stream"/> is null.
        /// </exception>
        public Scanner(Stream stream, Encoding charset)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            this._reader = new StreamReader(stream, charset ?? Encoding.UTF8, true);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Closes this instance.
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Determines whether this instance has next token.
        /// </summary>
        /// <returns><c>true</c> if there is another value; otherwise false.</returns>
        public bool HasNext()
        {
            int len;
            var buffer = new char[4096];
            while (this._tokens.Count == 0 && (len = this._reader.Read(buffer, 0, buffer.Length)) > 0)
            {
                var input = new string(buffer, 0, len);
                int lastPos = 0;
                foreach (Match match in this._pattern.Matches(input))
                {
                    int pos = match.Index;
                    string token = input.Substring(lastPos, pos - lastPos);
                    lastPos = pos + 1;
                    if (this._remainder.Length > 0)
                    {
                        token = this._remainder + token;
                        this._remainder.Clear();
                    }

                    this._tokens.Enqueue(token);
                }

                if (lastPos < input.Length)
                {
                    this._remainder.Append(input.Substring(lastPos));
                }
            }

            if (this._tokens.Count > 0)
            {
                return true;
            }

            if (this._remainder.Length > 0)
            {
                this._tokens.Enqueue(this._remainder.ToString());
                this._remainder.Clear();
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Gets the next token.
        /// </summary>
        /// <returns>The next token.</returns>
        public string Next()
        {
            return this._tokens.Dequeue();
        }

        /// <summary>
        /// Uses the delimiter.
        /// </summary>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>
        public Scanner UseDelimiter(string pattern)
        {
            this._pattern = new Regex(pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);
            return this;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="managed">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool managed)
        {
            if (this._disposed)
            {
                return;
            }

            if (managed)
            {
                this._reader.Dispose();
            }

            this._disposed = true;
        }

        #endregion
    }
}