// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseReadableDataLocation.cs" company="Eurostat">
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
    using System.Collections.Concurrent;
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Util;

    /// <summary>
    /// The base readable data location.
    /// </summary>
    public abstract class BaseReadableDataLocation : IReadableDataLocation
    {
        #region Fields

        /// <summary>
        /// The disposables
        /// </summary>
        private readonly ConcurrentQueue<IDisposable> _disposables = new ConcurrentQueue<IDisposable>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a guaranteed new input stream on each Property call.  
        /// The input stream will be reading the same underlying data source.
        /// </summary>
        public abstract Stream InputStream { get; }

        /// <summary>
        /// Gets the name. If this ReadableDataLocation originated from a file, then this will be the original file name, regardless of where the stream is held.
        /// This may return null if it is not relevant.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; protected set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Closes (and removes if appropriate) any resources that are held open
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this); 
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a disposable.
        /// </summary>
        /// <param name="disposable">
        /// The disposable. 
        /// </param>
        protected void AddDisposable(IDisposable disposable)
        {
            this._disposables.Enqueue(disposable);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="dispose">
        /// If set to true managed resources will be disposed as well. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                while (this._disposables.Count > 0)
                {
                    IDisposable disposable;
                    if (this._disposables.TryDequeue(out disposable))
                    {
                        disposable.Dispose();
                    }
                }
            }
        }

        #endregion
    }
}