// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlDocReadableDataLocation.cs" company="Eurostat">
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
    using System.Text;
    using System.Xml;

    using Org.Sdmxsource.Sdmx.Api.Util;

    /// <summary>
    /// A <see cref="IReadableDataLocation" /> implementation that reads data from a
    /// </summary>
    public class XmlDocReadableDataLocation : BaseReadableDataLocation
    {
        #region Fields

        /// <summary>
        /// The contents of the input document <see cref="XmlNode" /> or <see cref="XmlDocument" /> document
        /// </summary>
        private readonly byte[] _document;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDocReadableDataLocation"/> class.
        /// </summary>
        /// <param name="document">
        /// The input <see cref="XmlNode"/> or <see cref="XmlDocument"/> document. 
        /// </param>
        public XmlDocReadableDataLocation(XmlNode document)
        {
            this._document = Encoding.UTF8.GetBytes(document.OuterXml);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// This methods is guaranteed to return a new input stream on each method call.  
        /// The input stream will be reading the same underlying data source.
        /// </summary>
        public override Stream InputStream
        {
            get
            {
                var stream = new MemoryStream(this._document);
                this.AddDisposable(stream);
                return stream;
            }
        }

        #endregion
    }
}