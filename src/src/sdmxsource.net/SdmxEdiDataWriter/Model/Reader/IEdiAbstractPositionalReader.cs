// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEdiAbstractPositionalReader.cs" company="EUROSTAT">
//   Date Created : 2014-07-23
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The EdiAbstractPositionalReader interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Model.Reader
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.EdiParser.Model.Document;

    /// <summary>
    ///     The EdiAbstractPositionalReader interface.
    /// </summary>
    public interface IEdiAbstractPositionalReader : IEdiReader
    {
        #region Public Properties

        /// <summary>
        ///     Gets the metadata for the EDI document that this EDI data segment came from
        /// </summary>
        IEdiMetadata EdiDocumentMetadata { get; }

        /// <summary>
        ///     Gets the agency that maintains the structures defined by the data
        /// </summary>
        string MessageAgency { get; }

        /// <summary>
        ///     Gets the preparation date of this EDI data segment
        /// </summary>
        DateTime PreparationDate { get; }

        /// <summary>
        ///     Gets the receiving Agency
        /// </summary>
        string RecievingAgency { get; }

        /// <summary>
        ///     Gets the sending Agency
        /// </summary>
        IParty SendingAgency { get; }

        #endregion
    }
}