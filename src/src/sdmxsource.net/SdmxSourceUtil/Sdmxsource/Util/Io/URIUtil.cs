// --------------------------------------------------------------------------------------------------------------------
// <copyright file="URIUtil.cs" company="Eurostat">
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
    using System.Net;

    using log4net;

    /// <summary>
    ///     The uri util.
    /// </summary>
    public class URIUtil
    {
        // SAME FOR ALL INSTANCES
        #region Static Fields

        /// <summary>
        ///     The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(URIUtil));

        /// <summary>
        ///     The temporary uri util.
        /// </summary>
        private static readonly URIUtil _temporaryUriUtil = new URIUtil("tmpFile");

        #endregion

        // INSTANCE SPECIFIC
        #region Fields

        /// <summary>
        ///     The file base name.
        /// </summary>
        private readonly string _fileBaseName;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="URIUtil"/> class.
        /// </summary>
        /// <param name="fileBaseName">
        /// The file base name.
        /// </param>
        private URIUtil(string fileBaseName)
        {
            ////this._fileBaseName = "tmp_file.";

            // this.deleteFilesOlderThen = deleteFilesOlderThen;
            // if(deleteFilesOlderThen > 0) {
            // task = new ScheduledTimerTask();
            // task.setDelay(deleteFilesOlderThen / 2);
            // task.setPeriod(deleteFilesOlderThen);
            // task.setRunnable(new FileDeleter());
            // } 
            // this.uriDirectory = uriDirectory;
            this._fileBaseName = fileBaseName;

            // this.overwright = overwright;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the URIUtil class which creates temporary URI locations for storing files.
        ///     <p />
        ///     Files which have been created through this which have not been modified for 24 hours, are deleted.
        /// </summary>
        /// <value> </value>
        public static URIUtil TempUriUtil
        {
            get
            {
                return _temporaryUriUtil;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a URI from the specified <paramref name="file"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <returns>
        /// The <see cref="Uri"/>.
        /// </returns>
        public static Uri GetUriFromFile(FileInfo file)
        {
            return new Uri(file.FullName);
        }

        /// <summary>
        /// The get uri util.
        /// </summary>
        /// <param name="directoryName">
        /// The directory name.
        /// </param>
        /// <param name="fileBaseName">
        /// The file base name.
        /// </param>
        /// <param name="overwrite">
        /// The overwrite.
        /// </param>
        /// <returns>
        /// The <see cref="URIUtil"/>.
        /// </returns>
        public static URIUtil GetUriUtil(string directoryName, string fileBaseName, bool overwrite)
        {
            return new URIUtil(fileBaseName);
        }

        /// <summary>
        /// Gets a new file from <paramref name="stream"/>. The <paramref name="stream"/> is closed.
        /// </summary>
        /// <param name="stream">
        /// The input stream.
        /// </param>
        /// <returns>
        /// The <see cref="FileInfo"/>.
        /// </returns>
        public FileInfo GetFileFromStream(Stream stream)
        {
            FileInfo outputFile = this.GetTempFile();

            StreamUtil.CopyStream(stream, outputFile.OpenWrite());

            return outputFile;
        }

        /// <summary>
        /// Returns a <see cref="FileInfo"/> pointing to the downloaded <paramref name="sourceUri"/>
        /// </summary>
        /// <param name="sourceUri">
        /// The source uri.
        /// </param>
        /// <returns>
        /// The <see cref="FileInfo"/>.
        /// </returns>
        public FileInfo GetFileFromUri(Uri sourceUri)
        {
            WebRequest webRequest = WebRequest.Create(sourceUri);
            WebResponse webResponse = webRequest.GetResponse();
            return this.GetFileFromStream(webResponse.GetResponseStream());
        }

        /// <summary>
        ///     Gets a new temporary file
        /// </summary>
        /// <returns>
        ///     The <see cref="FileInfo" />.
        /// </returns>
        public FileInfo GetTempFile()
        {
            FileInfo f = FileUtil.CreateTemporaryFile(this._fileBaseName, "tmp");
            return f;
        }

        #endregion
    }
}