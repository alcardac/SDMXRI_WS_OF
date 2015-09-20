// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EdiDocumentPosition.cs" company="Eurostat">
//   Date Created : 2014-07-28
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The EDI document position.
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
    /// The EDI document position.
    /// </summary>
    public class EdiDocumentPosition : IEdiDocumentPosition
    {
        #region Fields

        /// <summary>
        /// The dataset action.
        /// </summary>
        private readonly DatasetAction _datasetAction;

        /// <summary>
        /// The dataset attributes.
        /// </summary>
        private readonly IList<IKeyValue> _datasetAttributes;

        /// <summary>
        /// The dataset id.
        /// </summary>
        private readonly string _datasetId;

        /// <summary>
        /// The dataset preparation.
        /// </summary>
        private readonly DateTime _datasetPreparation;

        /// <summary>
        /// The end line.
        /// </summary>
        private readonly int _endLine;

        /// <summary>
        /// The is structure.
        /// </summary>
        private readonly bool _isStructure;

        /// <summary>
        /// The key family identifier.
        /// </summary>
        private readonly string _keyFamilyIdentifier;

        /// <summary>
        /// The message agency.
        /// </summary>
        private readonly string _messageAgency;

        /// <summary>
        /// The missing value.
        /// </summary>
        private readonly string _missingValue;

        /// <summary>
        /// The receiving agency.
        /// </summary>
        private readonly string _recievingAgency;

        /// <summary>
        /// The reporting period.
        /// </summary>
        private readonly DateTime _reportingPeriod;

        /// <summary>
        /// The sending agency.
        /// </summary>
        private readonly IParty _sendingAgency;

        /// <summary>
        /// The start line.
        /// </summary>
        private readonly int _startLine;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EdiDocumentPosition"/> class.
        /// </summary>
        /// <param name="startLine">
        /// The start line.
        /// </param>
        /// <param name="endLine">
        /// The end line.
        /// </param>
        /// <param name="isStructure">
        /// The is structure.
        /// </param>
        /// <param name="datasetId">
        /// The dataset id.
        /// </param>
        /// <param name="messageAgency">
        /// The message agency.
        /// </param>
        /// <param name="sendingAgency">
        /// The sending agency.
        /// </param>
        /// <param name="recievingAgency">
        /// The receiving agency.
        /// </param>
        /// <param name="datasetAction">
        /// The dataset action.
        /// </param>
        /// <param name="keyFamilyIdentifier">
        /// The key family identifier.
        /// </param>
        /// <param name="missingValue">
        /// The missing value.
        /// </param>
        /// <param name="datasetPreparation">
        /// The dataset preparation.
        /// </param>
        /// <param name="reportingPeriod">
        /// The reporting period.
        /// </param>
        /// <param name="datasetAttributes">
        /// The dataset attributes.
        /// </param>
        public EdiDocumentPosition(
            int startLine, 
            int endLine, 
            bool isStructure, 
            string datasetId, 
            string messageAgency, 
            IParty sendingAgency, 
            string recievingAgency, 
            DatasetAction datasetAction, 
            string keyFamilyIdentifier, 
            string missingValue, 
            DateTime datasetPreparation, 
            DateTime reportingPeriod, 
            IList<IKeyValue> datasetAttributes)
        {
            this._startLine = startLine;
            this._endLine = endLine;
            this._isStructure = isStructure;
            this._datasetId = datasetId;
            this._messageAgency = messageAgency;
            this._sendingAgency = sendingAgency;
            this._recievingAgency = recievingAgency;
            this._datasetAction = datasetAction;
            this._keyFamilyIdentifier = keyFamilyIdentifier;
            this._missingValue = missingValue;
            this._datasetPreparation = datasetPreparation;
            this._reportingPeriod = reportingPeriod;
            this._datasetAttributes = datasetAttributes;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the data structure identifier.
        /// </summary>
        /// <value>
        ///     The data structure identifier.
        /// </value>
        public string DataStructureIdentifier
        {
            get
            {
                return this._keyFamilyIdentifier;
            }
        }

        /// <summary>
        ///     Gets the dataset action.
        /// </summary>
        /// <value>
        ///     The dataset action.
        /// </value>
        public DatasetAction DatasetAction
        {
            get
            {
                return this._datasetAction;
            }
        }

        /// <summary>
        ///     Gets a list of dataset attributes, or an empty list if there are none
        ///     Only relevant for data messages.
        /// </summary>
        public List<IKeyValue> DatasetAttributes
        {
            get
            {
                return new List<IKeyValue>(this._datasetAttributes);
            }
        }

        /// <summary>
        ///     Gets the dataset identifier.
        /// </summary>
        /// <value>
        ///     The dataset identifier.
        /// </value>
        public string DatasetId
        {
            get
            {
                return this._datasetId;
            }
        }

        /// <summary>
        ///     Gets the end line.
        /// </summary>
        /// <value>
        ///     The end line.
        /// </value>
        public int EndLine
        {
            get
            {
                return this._endLine;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is data.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is data; otherwise, <c>false</c>.
        /// </value>
        public bool IsData
        {
            get
            {
                return !this._isStructure;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is structure.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is structure; otherwise, <c>false</c>.
        /// </value>
        public bool IsStructure
        {
            get
            {
                return this._isStructure;
            }
        }

        /// <summary>
        ///     Gets the message agency.
        /// </summary>
        /// <value>
        ///     The message agency.
        /// </value>
        public string MessageAgency
        {
            get
            {
                return this._messageAgency;
            }
        }

        /// <summary>
        ///     Gets the missing value.
        /// </summary>
        /// <value>
        ///     The missing value.
        /// </value>
        public string MissingValue
        {
            get
            {
                return this._missingValue;
            }
        }

        /// <summary>
        ///     Gets the preparation date.
        /// </summary>
        /// <value>
        ///     The preparation date.
        /// </value>
        public DateTime PreparationDate
        {
            get
            {
                return this._datasetPreparation;
            }
        }

        /// <summary>
        ///     Gets the receiving agency.
        /// </summary>
        /// <value>
        ///     The receiving agency.
        /// </value>
        public string ReceivingAgency
        {
            get
            {
                return this._recievingAgency;
            }
        }

        /// <summary>
        ///     Gets the reporting period.
        /// </summary>
        /// <value>
        ///     The reporting period.
        /// </value>
        public DateTime ReportingPeriod
        {
            get
            {
                return this._reportingPeriod;
            }
        }

        /// <summary>
        ///     Gets the sending agency.
        /// </summary>
        /// <value>
        ///     The sending agency.
        /// </value>
        public IParty SendingAgency
        {
            get
            {
                return this._sendingAgency;
            }
        }

        /// <summary>
        ///     Gets the start line.
        /// </summary>
        /// <value>
        ///     The start line.
        /// </value>
        public int StartLine
        {
            get
            {
                return this._startLine;
            }
        }

        #endregion
    }
}