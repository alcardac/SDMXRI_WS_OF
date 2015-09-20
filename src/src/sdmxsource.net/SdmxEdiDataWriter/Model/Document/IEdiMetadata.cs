// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEdiMetadata.cs" company="Eurostat">
//   Date Created : 2014-07-23
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The EdiMetadata interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Model.Document
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The EdiMetadata interface.
    /// </summary>
    public interface IEdiMetadata
    {
        #region Public Properties

        /// <summary>
        ///     Gets the application reference.
        /// </summary>
        /// <value>
        ///     The application reference.
        /// </value>
        string ApplicationReference { get; }

        /// <summary>
        ///     Gets the date of preparation.
        /// </summary>
        /// <value>
        ///     The date of preparation.
        /// </value>
        DateTime DateOfPreparation { get; }

        /// <summary>
        ///     Gets the index of the document.
        /// </summary>
        /// <value>
        ///     The index of the document.
        /// </value>
        IList<IEdiDocumentPosition> DocumentIndex { get; }

        /// <summary>
        ///     Gets the interchange reference.
        /// </summary>
        /// <value>
        ///     The interchange reference.
        /// </value>
        string InterchangeReference { get; }

        /// <summary>
        ///     Gets a value indicating whether this instance is test.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is test; otherwise, <c>false</c>.
        /// </value>
        bool IsTest { get; }

        /// <summary>
        ///     Gets the receiver identification.
        /// </summary>
        /// <value>
        ///     The receiver identification.
        /// </value>
        string ReceiverIdentification { get; }

        /// <summary>
        ///     Gets or sets the reporting begin.
        /// </summary>
        /// <value>
        ///     The reporting begin.
        /// </value>
        DateTime ReportingBegin { get; set; }

        /// <summary>
        ///     Gets or sets the reporting end.
        /// </summary>
        /// <value>
        ///     The reporting end.
        /// </value>
        DateTime ReportingEnd { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the index of the document.
        /// </summary>
        /// <param name="pos">
        /// The position.
        /// </param>
        void AddDocumentIndex(IEdiDocumentPosition pos);

        #endregion
    }
}