// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileReaderImpl.cs" company="Eurostat">
//   Date Created : 2014-07-28
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The file reader implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Model.Reader
{
    using System;
    using System.IO;
    using System.Text;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Util.Io;
    using Org.Sdmxsource.Util.Text;

    /// <summary>
    ///     The file reader implementation.
    /// </summary>
    public class FileReaderImpl : IFileReader
    {
        #region Static Fields

        /// <summary>
        ///     The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(FileReaderImpl));

        #endregion

        #region Fields

        /// <summary>
        ///     The character set.
        /// </summary>
        private readonly Encoding _charset;

        /// <summary>
        ///     The end index.
        /// </summary>
        private readonly int _endIndex = -1;

        /// <summary>
        ///     The end of line tag.
        /// </summary>
        private readonly string _endOfLineTag;

        /// <summary>
        ///     The start index.
        /// </summary>
        private readonly int _startIndex = -1;

        /// <summary>
        ///     The back line.
        /// </summary>
        private bool _backLine;

        /// <summary>
        ///     The current line.
        /// </summary>
        private string _currentLine;

        /// <summary>
        ///     The data file.
        /// </summary>
        private IReadableDataLocation _dataFile;

        /// <summary>
        ///     The _disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     The file position.
        /// </summary>
        private int _filePosition;

        /// <summary>
        ///     The scanner.
        /// </summary>
        private Scanner _scanner;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileReaderImpl"/> class.
        /// </summary>
        /// <param name="dataFile">
        /// The data file.
        /// </param>
        /// <param name="endOfLineTag">
        /// The end of line tag.
        /// </param>
        public FileReaderImpl(IReadableDataLocation dataFile, string endOfLineTag)
        {
            this._dataFile = dataFile;
            this._endOfLineTag = endOfLineTag;
            this.ResetReader();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileReaderImpl"/> class.
        /// </summary>
        /// <param name="dataFile">
        /// The data file.
        /// </param>
        /// <param name="endOfLineTag">
        /// The end of line tag.
        /// </param>
        /// <param name="charset">
        /// The charset.
        /// </param>
        public FileReaderImpl(IReadableDataLocation dataFile, string endOfLineTag, Encoding charset)
        {
            this._dataFile = dataFile;
            this._endOfLineTag = endOfLineTag;
            this._charset = charset;
            this.ResetReader();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileReaderImpl"/> class.
        /// </summary>
        /// <param name="dataFile">
        /// The data file.
        /// </param>
        /// <param name="endOfLineTag">
        /// The end of line tag.
        /// </param>
        /// <param name="startindex">
        /// The start index.
        /// </param>
        /// <param name="endIndex">
        /// The end index.
        /// </param>
        public FileReaderImpl(IReadableDataLocation dataFile, string endOfLineTag, int startindex, int endIndex)
        {
            this._dataFile = dataFile;
            this._endOfLineTag = endOfLineTag;
            this._startIndex = startindex;
            this._endIndex = endIndex;
            this.ResetReader();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileReaderImpl"/> class.
        /// </summary>
        /// <param name="dataFile">
        /// The data file.
        /// </param>
        /// <param name="endOfLineTag">
        /// The end of line tag.
        /// </param>
        /// <param name="startindex">
        /// The start index.
        /// </param>
        /// <param name="endIndex">
        /// The end index.
        /// </param>
        /// <param name="charset">
        /// The character set.
        /// </param>
        public FileReaderImpl(IReadableDataLocation dataFile, string endOfLineTag, int startindex, int endIndex, Encoding charset)
        {
            this._dataFile = dataFile;
            this._endOfLineTag = endOfLineTag;
            this._startIndex = startindex;
            this._endIndex = endIndex;
            this._charset = charset;
            this.ResetReader();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the current line.  minus the prefix, if the prefix is unknown then an exception is thrown
        /// </summary>
        /// <value>
        ///     The current line.
        /// </value>
        public string CurrentLine
        {
            get
            {
                return this._currentLine;
            }

            protected set
            {
                this._currentLine = value;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the reader has flagged this to move back a line, as moving back a line does not
        ///     actually change the current line
        /// </summary>
        /// <value>
        ///     <c>true</c> if the reader has flagged this to move back a line, as moving back a line does not actually change the
        ///     current line; otherwise, <c>false</c>.
        /// </value>
        public bool IsBackLine
        {
            get
            {
                return this._backLine;
            }
        }

        /// <summary>
        ///     Gets the line number of the current line, the first line being '1'.
        /// </summary>
        /// <value>
        ///     The line number.
        /// </value>
        public int LineNumber
        {
            get
            {
                return this._filePosition;
            }
        }

        /// <summary>
        ///     Gets the next line. Move the file pointer to the next line and returns that line.
        ///     Null is returned if there is no next line.
        ///     This method is almost the same as calling <see cref="IFileReader.MoveNext" /> followed by
        ///     <see cref="IFileReader.CurrentLine" /> the
        ///     difference is if there is no next line,
        ///     the call to <see cref="IFileReader.CurrentLine" /> will the same result as it did prior to this call
        /// </summary>
        /// <value>
        ///     The next line.
        /// </value>
        public string NextLine
        {
            get
            {
                if (this._backLine)
                {
                    this._backLine = false;
                    return this._currentLine;
                }

                if (this.MoveNext())
                {
                    return this.CurrentLine;
                }

                return null;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets a value indicating whether [back line].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [back line]; otherwise, <c>false</c>.
        /// </value>
        protected internal bool BackLine
        {
            get
            {
                return this._backLine;
            }

            set
            {
                this._backLine = value;
            }
        }

        /// <summary>
        ///     Gets or sets the data file.
        /// </summary>
        /// <value>
        ///     The data file.
        /// </value>
        protected internal IReadableDataLocation DataFile
        {
            get
            {
                return this._dataFile;
            }

            set
            {
                this._dataFile = value;
            }
        }

        /// <summary>
        ///     Gets or sets the file position.
        /// </summary>
        /// <value>
        ///     The file position.
        /// </value>
        protected internal int FilePosition
        {
            get
            {
                return this._filePosition;
            }

            set
            {
                this._filePosition = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Closes this instance. Close the reader and any resources associated with the reader
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }

        /// <summary>
        /// Copies the EDI file to the specified <paramref name="output"/>
        /// </summary>
        /// <param name="output">
        /// The output.
        /// </param>
        public void CopyToStream(Stream output)
        {
            StreamUtil.CopyStream(this._dataFile.InputStream, output);
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
        ///     Moves the reader back a single line.  This can not be called to iterate backwards - it will only move back one line
        /// </summary>
        public void MoveBackLine()
        {
            this._backLine = true;
        }

        /// <summary>
        ///     The move next.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public virtual bool MoveNext()
        {
            if (this._filePosition == this._endIndex)
            {
                this._currentLine = null;
                return false;
            }

            if (this._scanner.HasNext())
            {
                this._filePosition++;
                this._currentLine = this._scanner.Next();
                _log.Debug("Move Next : " + this._currentLine);
            }
            else
            {
                this._currentLine = null;
            }

            this.CleanLine();
            return !string.IsNullOrEmpty(this._currentLine);
        }

        /// <summary>
        ///     Resets the reader to the start of the file
        /// </summary>
        public void ResetReader()
        {
            if (this._scanner != null)
            {
                this._scanner.Close();
            }

            this._scanner = new Scanner(this._dataFile.InputStream, this._charset);

            this._scanner.UseDelimiter(this._endOfLineTag);
            this._filePosition = 0;
            if (this._startIndex > 0)
            {
                while (this._filePosition != this._startIndex)
                {
                    this.MoveNext();
                }
            }
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
                this._scanner.Close();
            }

            this._disposed = true;
        }

        /// <summary>
        ///     The clean line.
        /// </summary>
        private void CleanLine()
        {
            if (this._currentLine != null)
            {
                this._currentLine = this._currentLine.Replace("\\u000A", string.Empty); // Removes all new-line characters 
                this._currentLine = this._currentLine.Replace("\\u000D", string.Empty); // Removes all carriage-return characters
            }
        }

        #endregion
    }
}