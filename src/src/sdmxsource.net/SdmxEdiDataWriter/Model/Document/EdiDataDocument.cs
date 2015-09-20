// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EdiDataDocument.cs" company="Eurostat">
//   Date Created : 2014-07-29
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The edi data document.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Model.Document
{
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.EdiParser.Model.Reader;

    /// <summary>
    /// The edi data document.
    /// </summary>
    public class EdiDataDocument : IEdiDataDocument
    {
        #region Fields

        /// <summary>
        /// The data location.
        /// </summary>
        private readonly IReadableDataLocation _dataLocation;

        /// <summary>
        /// The document position.
        /// </summary>
        private readonly IEdiDocumentPosition _documentPosition;

        /// <summary>
        /// The edi metadata.
        /// </summary>
        private readonly IEdiMetadata _ediMetadata;

        /// <summary>
        /// The data reader.
        /// </summary>
        private IEdiDataReader _dataReader;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EdiDataDocument"/> class.
        /// </summary>
        /// <param name="documentUri">
        /// The document URI.
        /// </param>
        /// <param name="documentPosition">
        /// The document position.
        /// </param>
        /// <param name="ediMetadata">
        /// The EDI metadata.
        /// </param>
        public EdiDataDocument(IReadableDataLocation documentUri, IEdiDocumentPosition documentPosition, IEdiMetadata ediMetadata)
        {
            this._dataLocation = documentUri;
            this._documentPosition = documentPosition;
            this._ediMetadata = ediMetadata;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the data reader.
        /// </summary>
        /// <value>
        ///     The data reader.
        /// </value>
        public IEdiDataReader DataReader
        {
            get
            {
                // There should only ever be a single DataReader which is used by all calling instances.  
                if (this._dataReader == null)
                {
                    this._dataReader = new EdiDataReader(this._dataLocation, this._documentPosition, this._ediMetadata);
                }

                return this._dataReader;
            }
        }

        #endregion
    }
}