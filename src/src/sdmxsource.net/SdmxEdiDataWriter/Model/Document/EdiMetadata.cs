// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EdiMetadata.cs" company="Eurostat">
//   Date Created : 2014-07-28
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The EDI metadata.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Model.Document
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     The EDI metadata.
    /// </summary>
    public class EdiMetadata : IEdiMetadata
    {
        #region Fields

        /// <summary>
        /// The application reference.
        /// </summary>
        private readonly string _applicationReference;

        /// <summary>
        /// The date of preparation.
        /// </summary>
        private readonly DateTime _dateOfPreparation;

        /// <summary>
        /// The document positions.
        /// </summary>
        private readonly IList<IEdiDocumentPosition> _documentPositions = new List<IEdiDocumentPosition>();

        /// <summary>
        /// The interchange reference.
        /// </summary>
        private readonly string _interchangeReference;

        /// <summary>
        /// The is test.
        /// </summary>
        private readonly bool _isTest;

        /// <summary>
        /// The receiver identification.
        /// </summary>
        private readonly string _receiverIdentification;

        /// <summary>
        /// The reporting begin.
        /// </summary>
        private DateTime _reportingBegin;

        /// <summary>
        /// The reporting end.
        /// </summary>
        private DateTime _reportingEnd;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EdiMetadata"/> class.
        /// </summary>
        /// <param name="receiverIdentification">
        /// The receiver identification.
        /// </param>
        /// <param name="dateOfPreparation">
        /// The date of preparation.
        /// </param>
        /// <param name="interchangeReference">
        /// The interchange reference.
        /// </param>
        /// <param name="applicationReference">
        /// The application reference.
        /// </param>
        /// <param name="reportingBegin">
        /// The reporting begin.
        /// </param>
        /// <param name="reportingEnd">
        /// The reporting end.
        /// </param>
        /// <param name="isTest">
        /// The is test.
        /// </param>
        public EdiMetadata(
            string receiverIdentification, 
            DateTime dateOfPreparation, 
            string interchangeReference, 
            string applicationReference, 
            DateTime reportingBegin, 
            DateTime reportingEnd, 
            bool isTest)
        {
            this._receiverIdentification = receiverIdentification;
            this._dateOfPreparation = dateOfPreparation;
            this._interchangeReference = interchangeReference;
            this._applicationReference = applicationReference;
            this._reportingBegin = reportingBegin;
            this._reportingEnd = reportingEnd;
            this._isTest = isTest;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the application reference.
        /// </summary>
        /// <value>
        ///     The application reference.
        /// </value>
        public string ApplicationReference
        {
            get
            {
                return this._applicationReference;
            }
        }

        /// <summary>
        ///     Gets the date of preparation.
        /// </summary>
        /// <value>
        ///     The date of preparation.
        /// </value>
        public DateTime DateOfPreparation
        {
            get
            {
                return this._dateOfPreparation;
            }
        }

        /// <summary>
        ///     Gets the index of the document.
        /// </summary>
        /// <value>
        ///     The index of the document.
        /// </value>
        public IList<IEdiDocumentPosition> DocumentIndex
        {
            get
            {
                return this._documentPositions;
            }
        }

        /// <summary>
        ///     Gets the interchange reference.
        /// </summary>
        /// <value>
        ///     The interchange reference.
        /// </value>
        public string InterchangeReference
        {
            get
            {
                return this._interchangeReference;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is test.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is test; otherwise, <c>false</c>.
        /// </value>
        public bool IsTest
        {
            get
            {
                return this._isTest;
            }
        }

        /// <summary>
        ///     Gets the receiver identification.
        /// </summary>
        /// <value>
        ///     The receiver identification.
        /// </value>
        public string ReceiverIdentification
        {
            get
            {
                return this._receiverIdentification;
            }
        }

        /// <summary>
        ///     Gets or sets the reporting begin.
        /// </summary>
        /// <value>
        ///     The reporting begin.
        /// </value>
        public DateTime ReportingBegin
        {
            get
            {
                return this._reportingBegin;
            }

            set
            {
                this._reportingBegin = value;
            }
        }

        /// <summary>
        ///     Gets or sets the reporting end.
        /// </summary>
        /// <value>
        ///     The reporting end.
        /// </value>
        public DateTime ReportingEnd
        {
            get
            {
                return this._reportingEnd;
            }

            set
            {
                this._reportingEnd = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the index of the document.
        /// </summary>
        /// <param name="pos">
        /// The position.
        /// </param>
        public void AddDocumentIndex(IEdiDocumentPosition pos)
        {
            this._documentPositions.Add(pos);
        }

        #endregion
    }
}