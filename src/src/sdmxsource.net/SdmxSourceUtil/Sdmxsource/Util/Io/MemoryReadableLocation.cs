// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryReadableLocation.cs" company="Eurostat">
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

    /// <summary>
    /// Memory based buffer
    /// </summary>
    public class MemoryReadableLocation : BaseReadableDataLocation
    {
        #region Fields

        /// <summary>
        /// The buffer.
        /// </summary>
        private readonly byte[] _buffer;

        #endregion


        #region Public Properties

        /// <summary>
        /// Gets a guaranteed new input stream on each Property call.  
        /// The input stream will be reading the same underlying data source.
        /// </summary>
        public override Stream InputStream
        {
            get
            {
                var memoryStream = new MemoryStream(this._buffer);
                this.AddDisposable(memoryStream);
                return memoryStream;
            }
        }

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryReadableLocation"/> class.
        /// </summary>
        /// <param name="buffer">
        /// The buffer. 
        /// </param>
        public MemoryReadableLocation(byte[] buffer)
            : this(buffer, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryReadableLocation"/> class.
        /// </summary>
        /// <param name="buffer">
        /// The buffer. 
        /// </param>
        /// /// <param name="name">
        /// The name. 
        /// </param>
        public MemoryReadableLocation(byte[] buffer, string name)
        {
            this._buffer = buffer;
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryReadableLocation"/> class.
        /// </summary>
        /// <param name="uriStr">The URI string.</param>
        /// <exception cref="System.ArgumentNullException">Can not create StreamSourceData - uriStr can not be null</exception>
        public MemoryReadableLocation(string uriStr)
        {
            if (uriStr == null)
            {
                throw new ArgumentNullException("uriStr", "Can not create StreamSourceData - uriStr can not be null");
            }
            this.Name = uriStr;
            try
            {
                Uri uri = new Uri(uriStr);
                if (uri.IsAbsoluteUri)
                {
                    if (!uri.IsFile)
                    {
                        _buffer = StreamUtil.ToByteArray(URLUtil.GetInputStream(uri.AbsolutePath));
                    }
                    else
                    {
                        _buffer = FileUtil.ReadFileAsBytes(uri.LocalPath);
                    }
                }
            }
            catch (UriFormatException e)
            {
                Console.Error.WriteLine(e.StackTrace);
            }
            catch (WebException e)
            {
                Console.Error.WriteLine(e.StackTrace);
            }
        }
        #endregion
    }
}