// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteableDataLocationTmp.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Util.Io
{
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Util;

    /// <summary>
    /// The writeable data location tmp.
    /// </summary>
    public class WriteableDataLocationTmp : ReadableDataLocationTmp, IWriteableDataLocation
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableDataLocationTmp"/> class.
        /// </summary>
        public WriteableDataLocationTmp()
            : base(URIUtil.TempUriUtil.GetTempFile())
        {
            DeleteOnClose = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the output stream.
        /// </summary>
        public Stream OutputStream
        {
            get
            {
                var output = this.InstanceFile.OpenWrite();
                this.AddDisposable(output);

                return output;
            }
        }

        #endregion
    }
}