// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEdiDocumentPosition.cs" company="Eurostat">
//   Date Created : 2014-07-23
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The EDI document position interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Model.Document
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;

    /// <summary>
    ///     The EDI document position interface.
    /// </summary>
    public interface IEdiDocumentPosition
    {
        #region Public Properties

        /// <summary>
        ///     Gets the data structure identifier.
        /// </summary>
        /// <value>
        ///     The data structure identifier.
        /// </value>
        string DataStructureIdentifier { get; }

        /// <summary>
        ///     Gets the dataset action.
        /// </summary>
        /// <value>
        ///     The dataset action.
        /// </value>
        DatasetAction DatasetAction { get; }

        /// <summary>
        ///     Gets a list of dataset attributes, or an empty list if there are none
        ///     Only relevant for data messages.
        /// </summary>
        List<IKeyValue> DatasetAttributes { get; }

        /// <summary>
        ///     Gets the dataset identifier.
        /// </summary>
        /// <value>
        ///     The dataset identifier.
        /// </value>
        string DatasetId { get; }

        /// <summary>
        ///     Gets the end line.
        /// </summary>
        /// <value>
        ///     The end line.
        /// </value>
        int EndLine { get; }

        /// <summary>
        ///     Gets a value indicating whether this instance is data.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is data; otherwise, <c>false</c>.
        /// </value>
        bool IsData { get; }

        /// <summary>
        ///     Gets a value indicating whether this instance is structure.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is structure; otherwise, <c>false</c>.
        /// </value>
        bool IsStructure { get; }

        /// <summary>
        ///     Gets the message agency.
        /// </summary>
        /// <value>
        ///     The message agency.
        /// </value>
        string MessageAgency { get; }

        /// <summary>
        ///     Gets the missing value.
        /// </summary>
        /// <value>
        ///     The missing value.
        /// </value>
        string MissingValue { get; }

        /// <summary>
        ///     Gets the preparation date.
        /// </summary>
        /// <value>
        ///     The preparation date.
        /// </value>
        DateTime PreparationDate { get; }

        /// <summary>
        ///     Gets the receiving agency.
        /// </summary>
        /// <value>
        ///     The receiving agency.
        /// </value>
        string ReceivingAgency { get; }

        /// <summary>
        ///     Gets the reporting period.
        /// </summary>
        /// <value>
        ///     The reporting period.
        /// </value>
        DateTime ReportingPeriod { get; }

        /// <summary>
        ///     Gets the sending agency.
        /// </summary>
        /// <value>
        ///     The sending agency.
        /// </value>
        IParty SendingAgency { get; }

        /// <summary>
        ///     Gets the start line.
        /// </summary>
        /// <value>
        ///     The start line.
        /// </value>
        int StartLine { get; }

        #endregion
    }
}