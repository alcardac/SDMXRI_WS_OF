// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadableDataLocationTmp.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Util.Io
{
    using System;
    using System.IO;

    /// <summary>
    ///     Built from an InputStream - copies the data to a local temporary file, and allocates the information out as new input streams on demand
    /// </summary>
    public class ReadableDataLocationTmp : BaseReadableDataLocation
    {
        #region Fields

        /// <summary>
        ///     The file.
        /// </summary>
        private readonly FileInfo _file;

        /// <summary>
        ///     The delete on close.
        /// </summary>
        private bool _deleteOnClose;

        /// <summary>
        /// The _disposed
        /// </summary>
        private bool _disposed;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadableDataLocationTmp"/> class.
        /// </summary>
        /// <param name="uriStr">
        /// The uri str.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="uriStr"/> is null
        /// </exception>
        public ReadableDataLocationTmp(string uriStr)
        {
            this._deleteOnClose = false;
            if (uriStr == null)
            {
                throw new ArgumentNullException("uriStr");
            }

            var uri = new Uri(uriStr);
            if (uri.IsAbsoluteUri)
            {
                if (!uri.Scheme.Equals("file"))
                {
                    this._file = URIUtil.TempUriUtil.GetFileFromUri(uri);
                    this._deleteOnClose = true;
                }
            }

            if (this._file == null)
            {
                this._file = new FileInfo(uri.LocalPath);
            }

            if (!this._file.Exists)
            {
                throw new ArgumentException(this.InstanceFile.FullName, "uriStr");
            }
            this.Name = this._file.Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadableDataLocationTmp"/> class.
        /// </summary>
        /// <param name="inputFile">
        /// The input file.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="inputFile"/> is null
        /// </exception>
        public ReadableDataLocationTmp(FileInfo inputFile)
        {
            this._deleteOnClose = false;
            if (inputFile == null)
            {
                throw new ArgumentNullException("inputFile");
            }

            this._file = inputFile;
            this.Name = this._file.Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadableDataLocationTmp"/> class.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Throws ArgumentNullException
        /// </exception>
        public ReadableDataLocationTmp(Uri url)
        {
            this._deleteOnClose = false;
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            this._file = URIUtil.TempUriUtil.GetFileFromUri(url);
            this.Name = this._file.Name;
            this._deleteOnClose = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadableDataLocationTmp"/> class.
        /// </summary>
        /// <param name="inputStream">
        /// The input stream.
        /// </param>
        public ReadableDataLocationTmp(Stream inputStream)
        {
            this._deleteOnClose = false;
            this._file = URIUtil.TempUriUtil.GetFileFromStream(inputStream);
            this.Name = this._file.Name;
            this._deleteOnClose = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the input stream.
        /// </summary>
        public override Stream InputStream
        {
            get
            {
                if (this._disposed)
                {
                    throw new ObjectDisposedException(this.ToString());
                }
            
                FileStream fileStream = this._file.OpenRead();
                this.AddDisposable(fileStream);
                return fileStream;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets a value indicating whether to delete on close.
        /// </summary>
        protected bool DeleteOnClose
        {
            get
            {
                return this._deleteOnClose;
            }

            set
            {
                this._deleteOnClose = value;
            }
        }

        /// <summary>
        ///     Gets the uri.
        /// </summary>
        protected Uri Uri
        {
            get
            {
                return URIUtil.GetUriFromFile(this._file);
            }
        }

        /// <summary>
        ///     Gets the file.
        /// </summary>
        protected FileInfo InstanceFile
        {
            get
            {
                return this._file;
            }
        }

         #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Returns a <see cref="T:System.String" /> that represents the current <see cref="ReadableDataLocationTmp" />.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that represents the current <see cref="ReadableDataLocationTmp" /> .
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return this.Uri.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="dispose">
        /// If set to true managed resources will be disposed as well.
        /// </param>
        /// <filterpriority>2</filterpriority>
        protected override void Dispose(bool dispose)
        {
            base.Dispose(dispose);
            if (!this._disposed)
            {
                if (this.DeleteOnClose)
                {
                    this.InstanceFile.Refresh();
                    this.InstanceFile.Delete();
                }

                this._disposed = true;
            }
        }

        #endregion
    }
}