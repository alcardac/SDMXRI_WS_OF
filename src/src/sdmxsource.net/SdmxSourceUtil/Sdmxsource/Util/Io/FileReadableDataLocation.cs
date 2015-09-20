// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileReadableDataLocation.cs" company="Eurostat">
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
    /// The readable data location stream.
    /// </summary>
    public class FileReadableDataLocation : BaseReadableDataLocation
    {
        #region Fields

        /// <summary>
        /// The input file.
        /// </summary>
        private readonly FileInfo _file;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileReadableDataLocation"/> class.
        /// </summary>
        /// <param name="fileName">
        /// The file name. 
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fileName"/>
        /// is null
        /// </exception>
        public FileReadableDataLocation(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            this._file = new FileInfo(fileName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileReadableDataLocation"/> class.
        /// </summary>
        /// <param name="file">
        /// The input file. 
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="file"/>
        /// is null
        /// </exception>
        public FileReadableDataLocation(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            this._file = file;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a new input stream on each method call.  
        /// The input stream will be reading the same underlying data source.
        /// </summary>
        public override Stream InputStream
        {
            get
            {
                FileStream fileStream = this._file.OpenRead();
                this.AddDisposable(fileStream);
                return fileStream;
            }
        }

        #endregion
    }
}