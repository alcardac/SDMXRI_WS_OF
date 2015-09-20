// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EdiAbstractPositionalReader.cs" company="Eurostat">
//   Date Created : 2014-07-28
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The EDI abstract positional reader.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Model.Reader
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.EdiParser.Model.Document;

    /// <summary>
    ///     The EDI abstract positional reader.
    /// </summary>
    public class EdiAbstractPositionalReader : EdiReader, IEdiAbstractPositionalReader
    {
        #region Fields

        /// <summary>
        /// The document position.
        /// </summary>
        private readonly IEdiDocumentPosition _documentPosition;

        /// <summary>
        /// The edi metadata.
        /// </summary>
        private readonly IEdiMetadata _ediMetadata;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EdiAbstractPositionalReader"/> class.
        /// </summary>
        /// <param name="dataFile">
        /// The data file.
        /// </param>
        /// <param name="documentPosition">
        /// The document position.
        /// </param>
        /// <param name="ediMetadata">
        /// The edi metadata.
        /// </param>
        public EdiAbstractPositionalReader(IReadableDataLocation dataFile, IEdiDocumentPosition documentPosition, IEdiMetadata ediMetadata)
            : base(dataFile)
        {
            this._ediMetadata = ediMetadata;
            this._documentPosition = documentPosition;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the metadata for the EDI document that this EDI data segment came from
        /// </summary>
        public IEdiMetadata EdiDocumentMetadata
        {
            get
            {
                return this._ediMetadata;
            }
        }

        /// <summary>
        ///     Gets the agency that maintains the structures defined by the data
        /// </summary>
        public string MessageAgency
        {
            get
            {
                return this._documentPosition.MessageAgency;
            }
        }

        /// <summary>
        ///     Gets the preparation date of this EDI data segment
        /// </summary>
        public DateTime PreparationDate
        {
            get
            {
                return this._documentPosition.PreparationDate;
            }
        }

        /// <summary>
        ///     Gets the receiving Agency
        /// </summary>
        public string RecievingAgency
        {
            get
            {
                return this._documentPosition.ReceivingAgency;
            }
        }

        /// <summary>
        ///     Gets the sending Agency
        /// </summary>
        public IParty SendingAgency
        {
            get
            {
                return this._documentPosition.SendingAgency;
            }
        }

        #endregion
    }
}