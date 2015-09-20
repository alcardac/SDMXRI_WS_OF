// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EdiParseEngine.cs" company="Eurostat">
//   Date Created : 2014-07-29
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The edi parse engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.EdiParser.Constants;
    using Org.Sdmxsource.Sdmx.EdiParser.Extension;
    using Org.Sdmxsource.Sdmx.EdiParser.Model.Document;
    using Org.Sdmxsource.Sdmx.EdiParser.Model.Reader;
    using Org.Sdmxsource.Sdmx.EdiParser.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Date;

    /// <summary>
    ///     The edi parse engine.
    /// </summary>
    internal class EdiParseEngine : IEdiParseEngine
    {
        #region Public Methods and Operators

        /// <summary>
        /// Validates the Edi is valid and returns metadata about the parsed Edi file
        /// </summary>
        /// <param name="ediMessageLocation">
        /// The EDI message location.
        /// </param>
        /// <returns>
        /// The <see cref="IEdiMetadata"/>.
        /// </returns>
        public IEdiMetadata ParseEDIMessage(IReadableDataLocation ediMessageLocation)
        {
            var parserEngine = new EDIStructureParserEngine(ediMessageLocation);
            return parserEngine.ProcessMessage();
        }

        #endregion

        /// <summary>
        ///     The edi structure parser engine.
        /// </summary>
        private class EDIStructureParserEngine
        {
            // The current line being processed
            #region Fields

            /// <summary>
            ///     The _two letter ISO language name.
            /// </summary>
            private readonly string _twoLetterIsoLanguageName;

            /// <summary>
            ///     The edi reader.
            /// </summary>
            private readonly IEdiReader _ediReader;

            /// <summary>
            ///     The current line being processed
            /// </summary>
            private string _currentLine;

            /// <summary>
            ///     The dataset action.
            /// </summary>
            private DatasetAction _datasetAction;

            //// Data Related
            
            /// <summary>
            ///     The dataset id.
            /// </summary>
            private string _datasetId;

            /// <summary>
            ///     The dataset preparation.
            /// </summary>
            private DateTime _datasetPreperation;

            /// <summary>
            ///     The key family identifier.
            /// </summary>
            private string _keyFamilyIdentifier;

            /// <summary>
            ///     The missing value.
            /// </summary>
            private string _missingValue;

            /// <summary>
            ///     The reporting begin.
            /// </summary>
            private DateTime _reportingBegin;

            /// <summary>
            ///     The reporting end.
            /// </summary>
            private DateTime _reportingEnd;

            /// <summary>
            ///     The reporting period.
            /// </summary>
            private DateTime _reportingPeriod;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="EDIStructureParserEngine"/> class.
            /// </summary>
            /// <param name="sourceData">
            /// The source data.
            /// </param>
            public EDIStructureParserEngine(IReadableDataLocation sourceData)
            {
                this._ediReader = new EdiReader(sourceData);
                this._twoLetterIsoLanguageName = CultureInfo.GetCultureInfo("en").TwoLetterISOLanguageName;
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The process message.
            /// </summary>
            /// <returns>
            ///     The <see cref="IEdiMetadata" />.
            /// </returns>
            public IEdiMetadata ProcessMessage()
            {
                try
                {
                    return this.ValidateMessage();
                }
                finally
                {
                    this._ediReader.Close();
                }
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The assert move next.
            /// </summary>
            /// <exception cref="SdmxSyntaxException">
            /// Unexpected end of file
            /// </exception>
            private void AssertMoveNext()
            {
                if (!this._ediReader.MoveNext())
                {
                    throw new SdmxSyntaxException("Unexpected end of file" + this._ediReader.CurrentLine);
                }
            }

            /// <summary>
            ///     Determines the dataset metadata, such as the missing value valueOfString, in the Edi message
            /// </summary>
            private void DetermineDatasetMetadata()
            {
                this.ReadNextLine();
                EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.DataStart, true);
                this._datasetId = this._ediReader.CurrentLine;

                this.AssertMoveNext();
                EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.DatasetAction, true);
                if (this._ediReader.CurrentLine.Equals("6"))
                {
                    this._datasetAction = DatasetActionEnumType.Delete;
                }
                else if (this._ediReader.CurrentLine.Equals("7"))
                {
                    this._datasetAction = DatasetActionEnumType.Replace;
                }
                else
                {
                    throw new SdmxSyntaxException("Unknown Edi-Ts Dataset Action value : " + this._ediReader.CurrentLine);
                }

                this.AssertMoveNext();

                // Sort out the dates
                this._reportingBegin = default(DateTime);
                this._reportingEnd = default(DateTime);

                EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.DatasetDatetime, true);
                string currentLine = this._ediReader.CurrentLine;
                if (!currentLine.StartsWith("242:"))
                {
                    throw new SdmxSyntaxException("Could not parse dataset preparation date '" + currentLine + "' expecting '" + EdiPrefix.DatasetDatetime + "242:' but did not find '242:'");
                }

                if (this._ediReader.CurrentLine.Length <= 4)
                {
                    throw new SdmxSyntaxException(
                        "Could not parse dataset preparation date '" + currentLine
                        + "' expecting '242:' to be followed by a date in the following format 'yyyyMMddHHmm:Sss' example : 19811807810530:000 (18th July 1981, 05:30am)");
                }

                string datePart = currentLine.Substring(4);
                var df = new DateFormat("yyyyMMddHHmm:ssF"); // .SSSz
                try
                {
                    this._datasetPreperation = df.Parse(datePart);
                }
                catch (FormatException e)
                {
                    throw new SdmxSyntaxException(
                        "Could not parse dataset preparation date '" + datePart + "', please ensure format is yyyyMMddHHmm:Sss example : 19811807810530:000 (18th July 1981, 05:30am)", e);
                }

                this.AssertMoveNext();
                if (EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.DatasetDatetime, false))
                {
                    currentLine = this._ediReader.CurrentLine;
                    if (!currentLine.StartsWith("Z02:"))
                    {
                        throw new SdmxSyntaxException("Could not parse dataset preparation date '" + currentLine + "' expecting '" + EdiPrefix.DatasetDatetime + "242:' but did not find 'Z02:'");
                    }

                    if (this._ediReader.CurrentLine.Length <= 4)
                    {
                        throw new SdmxSyntaxException(
                            "Could not parse dataset preparation date '" + currentLine
                            + "' expecting 'Z02:' to be followed by a date in the following format 'yyyyMMddHHmm:Sss' example : 19811807810530:000 (18th July 1981, 05:30am)");
                    }

                    // Evaluate the suffix to determine the reporting period format
                    int dateFormatStart = currentLine.LastIndexOf(EdiConstants.Colon, StringComparison.Ordinal);
                    string timeFormatString = currentLine.Substring(dateFormatStart + 1); // Add 1 since we don't want the colon character 
                    EdiTimeFormat timeFormat = EdiTimeFormatExtension.ParseString(timeFormatString);
                    datePart = currentLine.Substring(4, dateFormatStart - 4);

                    this._reportingBegin = timeFormat.ParseDate(datePart);
                    if (timeFormat.IsRange())
                    {
                        this._reportingEnd = timeFormat.ParseEndDate(datePart);
                    }
                    else
                    {
                        this._reportingEnd = this._reportingBegin;
                    }

                    this._reportingPeriod = this._reportingBegin;
                    this._reportingEnd = DateUtil.MoveToEndofPeriod(this._reportingEnd, timeFormat.GetSdmxTimeFormat());
                }
                else
                {
                    this._ediReader.MoveBackLine();
                }

                this.AssertMoveNext();
                EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.DsdReference, true);
                this._keyFamilyIdentifier = this._ediReader.CurrentLine;

                this.AssertMoveNext();
                EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.DatasetSendMethod, true);

                this.AssertMoveNext();
                EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.DatasetMissingValueSymbol, true);
                this._missingValue = this._ediReader.CurrentLine;
            }

            /// <summary>
            /// The process message sender.
            /// </summary>
            /// <returns>
            /// The <see cref="IParty" />.
            /// </returns>
            /// <exception cref="SdmxSyntaxException">
            /// Edi Message can not contain more then 3 CTA+ segments (contact information)
            /// or
            /// Unexpected ':' in valueOfString, expected only one colon to eliminate between the contact id and the contact name:  + ediCurrentLine
            /// or
            /// Unexpected  + EdiPrefix.CommunicationNumber + , this segment must come after a contact information segment:  + EdiPrefix.ContactInformation
            /// or
            /// Edi Message can not contain more then 5 COM+ segments per CTA+ segment (communication number)
            /// or
            /// or
            /// </exception>
            private IParty ProcessMessageSender()
            {
                // Seventh Line is the sending agency
                this.ReadNextLine();
                EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.SendingAgency, true);
                string sendingAgency = this._currentLine;

                //// this.ReadNextLine();

                IList<ITextTypeWrapper> partyNames = new List<ITextTypeWrapper>();

                // Note possible bug in SdmxSource.Java. IDE+10 is conditional. But SdmxSource.Java treats it as mandatory.
                // This is fixed here.
                ////if (EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.MessageIdProvidedBySender, false))
                ////{
                ////    string sendersMessageId = this._ediReader.CurrentLine;
                ////    partyNames.Add(new TextTypeWrapperImpl(this._twoLetterIsoLanguageName, sendersMessageId, null));
                ////}
                ////else
                ////{
                ////    this._ediReader.MoveBackLine();
                ////    return new PartyCore(null, sendingAgency, null, null);
                ////}

                int contactInfoCount = 0;
                int communicationChannellInfoCount = 0;

                IList<IContact> contacts = new List<IContact>();

                IList<ITextTypeWrapper> contactName = null;
                IList<ITextTypeWrapper> contactRole = null;
                IList<string> email = null;
                IList<string> fax = null;
                IList<string> telephone = null;
                IList<string> uri = null;
                IList<string> x400 = null;

                while (this._ediReader.MoveNext())
                {
                    if (EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.MessageIdProvidedBySender, false))
                    {
                        string sendersMessageId = this._ediReader.CurrentLine;
                        partyNames.Add(new TextTypeWrapperImpl(this._twoLetterIsoLanguageName, sendersMessageId, null));
                    }
                    else if (this._ediReader.LineType == EdiPrefix.ContactInformation)
                    {
                        contactInfoCount++;
                        if (contactInfoCount > 3)
                        {
                            throw new SdmxSyntaxException("Edi Message can not contain more then 3 CTA+ segments (contact information)");
                        }

                        if (contactInfoCount > 1)
                        {
                            // This is the second contact info
                            contacts.Add(new ContactCore(contactName, contactRole, null, email, fax, telephone, uri, x400));
                        }

                        contactName = new List<ITextTypeWrapper>();
                        contactRole = new List<ITextTypeWrapper>();
                        email = new List<string>();
                        fax = new List<string>();
                        telephone = new List<string>();
                        uri = new List<string>();
                        x400 = new List<string>();
                        communicationChannellInfoCount = 0; // reset the communication channel count

                        string ediCurrentLine = this._ediReader.CurrentLine;
                        string[] contactInfoSplit = EDIUtil.SplitOnPlus(ediCurrentLine, 2);

                        string contactRoleTxt = contactInfoSplit[0];
                        if (!contactRoleTxt.Equals("CC") && !contactRoleTxt.Equals("CP") && !contactRoleTxt.Equals("CF") && !contactRoleTxt.Equals("CE"))
                        {
                            throw new SdmxSyntaxException(
                                EdiPrefix.ContactInformation + " segment must have a contact function which must be either: CC, CP, CF, or CE - contact function provided was: " + contactRoleTxt);
                        }

                        string[] contactInfoReSplit = EDIUtil.SplitOnColon(contactInfoSplit[1]);
                        string contactNameTxt;
                        if (contactInfoReSplit.Length == 1)
                        {
                            contactNameTxt = contactInfoReSplit[0];
                        }
                        else if (contactInfoReSplit.Length == 2)
                        {
                            contactNameTxt = contactInfoReSplit[1];
                        }
                        else
                        {
                            throw new SdmxSyntaxException("Unexpected ':' in valueOfString, expected only one colon to eliminate between the contact id and the contact name: " + ediCurrentLine);
                        }

                        contactName.Add(new TextTypeWrapperImpl(this._twoLetterIsoLanguageName, contactNameTxt, null));
                        contactRole.Add(new TextTypeWrapperImpl(this._twoLetterIsoLanguageName, contactRoleTxt, null));
                    }
                    else if (this._ediReader.LineType == EdiPrefix.CommunicationNumber)
                    {
                        // We do not expect to see contact phone numbers before we have hit the contact information segment defining who the contact is
                        if (contactInfoCount == 0)
                        {
                            throw new SdmxSyntaxException(
                                "Unexpected " + EdiPrefix.CommunicationNumber + ", this segment must come after a contact information segment: " + EdiPrefix.ContactInformation);
                        }

                        communicationChannellInfoCount++;
                        if (communicationChannellInfoCount > 5)
                        {
                            throw new SdmxSyntaxException("Edi Message can not contain more then 5 Com+ segments per Cta+ segment (communication number)");
                        }

                        string ediCurrentLine = this._ediReader.CurrentLine;

                        string[] numberChannelSplit = EDIUtil.SplitOnColon(ediCurrentLine, 2);
                        string communicationNumber = numberChannelSplit[0];
                        string communicationChannel = numberChannelSplit[1];
                        if (communicationNumber.Length > 512)
                        {
                            throw new SdmxSyntaxException(
                                EdiPrefix.CommunicationNumber + " contains a communication number which is longer then the maximum length of 512 characters.  Actual length : "
                                + communicationNumber.Length);
                        }

                        if (communicationChannel.Equals(EdiConstants.Email))
                        {
                            Debug.Assert(email != null, "email != null");
                            email.Add(communicationNumber);
                        }
                        else if (communicationChannel.Equals(EdiConstants.Telephone))
                        {
                            Debug.Assert(telephone != null, "telephone != null");
                            telephone.Add(communicationNumber);
                        }
                        else if (communicationChannel.Equals(EdiConstants.Fax))
                        {
                            Debug.Assert(fax != null, "fax != null");
                            fax.Add(communicationNumber);
                        }
                        else if (communicationChannel.Equals(EdiConstants.X400))
                        {
                            Debug.Assert(x400 != null, "x400 != null");
                            x400.Add(communicationNumber);
                        }
                        else
                        {
                            throw new SdmxSyntaxException(
                                EdiPrefix.CommunicationNumber + " contains an invalid communication channel of '" + communicationChannel + "' - allowed channels are EM, TE, FX, or XF");
                        }
                    }
                    else
                    {
                        if (contactInfoCount > 0)
                        {
                            // We have a contact to add
                            contacts.Add(new ContactCore(contactName, contactRole, null, email, fax, telephone, uri, x400));
                        }

                        this._ediReader.MoveBackLine();
                        return new PartyCore(partyNames, sendingAgency, contacts, null);
                    }
                }

                return new PartyCore(partyNames, sendingAgency, null, null);
            }

            /// <summary>
            /// Reads the next line.
            /// </summary>
            /// <returns>true if the current line exists; otherwise false.</returns>
            private bool ReadNextLine()
            {
                this._currentLine = this._ediReader.NextLine;
                return this._currentLine != null;
            }

            /// <summary>
            /// Validates the message.
            /// </summary>
            /// <exception cref="SdmxSyntaxException">
            /// Edi Message expected first line UNA:+.? '
            /// or
            /// Character set UNOC:3 expected, but was ' + interchangeHeaderSplitOnPlus[0] + '
            /// or
            /// Data preparation date expected in format <c>yyMMdd:HHmm</c> - provided date time was:  <c>preperationDateTime</c>
            /// or
            /// Application reference expected to be SDMX-EDI or GESMES/TS but was ' + appReference + '
            /// </exception>
            /// <exception cref="System.ArgumentException">
            /// The Interchange Header test indicator must have the value of 1
            /// or
            /// Unexpected number of '+' characters in interchange header. Expected either 7 or 11
            /// </exception>
            /// <exception cref="SdmxSemmanticException">
            /// Number of message identification segments expected to be ' + numLines + ' but was ' <c> messageIdentifications </c> '
            /// or
            /// End of message reference expected to be ' + interchangeMessageRef + ' but was ' + messageReference + '
            /// </exception>
            /// <returns>
            /// The <see cref="IEdiMetadata"/>.
            /// </returns>
            private IEdiMetadata ValidateMessage()
            {
                // First line in the message start prefix
                this.ReadNextLine();

                if (!this._ediReader.LineType.IsMessageStart())
                {
                    throw new SdmxSyntaxException("Edi Message expected first line UNA:+.? '");
                }

                // Second line is the interchange header
                this.ReadNextLine();
                EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.InterchangeHeader, true);

                // Current line something like:  UNOC:3+5B0+AT2+120614:2345+IREF003063++GESMES/TS'
                string[] interchangeHeaderSplitOnPlus = EDIUtil.SplitOnPlus(this._currentLine);

                bool isTest;
                if (interchangeHeaderSplitOnPlus.Length == 7)
                {
                    isTest = false;
                }
                else if (interchangeHeaderSplitOnPlus.Length == 11)
                {
                    if (!interchangeHeaderSplitOnPlus[10].Equals("1"))
                    {
                        throw new ArgumentException("The Interchange Header test indicator must have the value of 1");
                    }

                    isTest = true;
                }
                else
                {
                    throw new ArgumentException("Unexpected number of '+' characters in interchange header. Expected either 7 or 11 but the actual was: '" + interchangeHeaderSplitOnPlus.Length + "'");
                }

                // Split on plus: UNOC:3, 5B0, AT2, 120614:2345, IREF003063, +GESMES/TS'

                // Split on colon: UNB+UNOC, 3+5B0+AT2+120614, 2345+IREF003063++GESMES/TS
                if (!interchangeHeaderSplitOnPlus[0].Equals("UNOC:3"))
                {
                    throw new SdmxSyntaxException("Character set UNOC:3 expected, but was '" + interchangeHeaderSplitOnPlus[0] + "'");
                }

                // string[] interchangeRefSplit = interchangeHeaderSplitOnPlus[2].Split("(?<!\\?)\\+");
                string revieverIdentification = interchangeHeaderSplitOnPlus[2];
                string preperationDateTime = interchangeHeaderSplitOnPlus[3]; // In format yyMMdd:HHmm example 120605:1802
                string interchangeMessageRef = interchangeHeaderSplitOnPlus[4];
                string appReference = interchangeHeaderSplitOnPlus[6];

                if (preperationDateTime.Length != 11)
                {
                    throw new SdmxSyntaxException("Data preparation date expected in format yyMMdd:HHmm - provided date time was:" + preperationDateTime);
                }

                DateFormat df = DateUtil.GetDateFormatter("yyMMdd:HHmm");
                DateTime prepDate;
                try
                {
                    prepDate = df.Parse(preperationDateTime);
                }
                catch (FormatException e)
                {
                    throw new SdmxSyntaxException("Data preparation date expected in format yyMMdd:HHmm - provided date time was:" + preperationDateTime, e);
                }

                if (!appReference.Equals("SDMX-EDI") && !appReference.Equals("GESMES/TS"))
                {
                    throw new SdmxSyntaxException("Application reference expected to be SDMX-EDI or GESMES/TS but was '" + appReference + "'");
                }

                // Third line is the message identification
                this.ReadNextLine();

                IEdiMetadata metadata = new EdiMetadata(revieverIdentification, prepDate, interchangeMessageRef, appReference, this._reportingBegin, this._reportingEnd, isTest);
                int messageIdentifications = this.ValidateMessageInterchangeHeader(metadata, 1);
                EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.EndMessage, true);

                string[] endMessageSplit = EDIUtil.SplitOnPlus(this._currentLine, 2);
                int numLines = EDIUtil.ParseStringAsInt(endMessageSplit[0]);
                if (messageIdentifications != numLines)
                {
                    throw new SdmxSemmanticException("Number of message identification segments expected to be '" + numLines + "' but was '" + messageIdentifications + "'");
                }

                string messageReference = endMessageSplit[1];
                if (!messageReference.Equals(interchangeMessageRef))
                {
                    throw new SdmxSemmanticException("End of message reference expected to be '" + interchangeMessageRef + "' but was '" + messageReference + "'");
                }

                return metadata;
            }

            /// <summary>
            /// Recursively called for each message (structure or data) in an Edi Document. Each time this is called a
            ///     <see cref="IEdiDocumentPosition"/> is stored in the IEdiMetadata
            /// </summary>
            /// <param name="metadata">
            /// The metadata.
            /// </param>
            /// <param name="currentnum">
            /// The current number .
            /// </param>
            /// <returns>
            /// The <see cref="int"/>.
            /// </returns>
            private int ValidateMessageInterchangeHeader(IEdiMetadata metadata, int currentnum)
            {
                int segmentStart = this._ediReader.LineNumber;
                EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.MessageIdentification, true);
                string[] splitOnPlus = EDIUtil.SplitOnPlus(this._currentLine, 2);
                string messageIdentification = splitOnPlus[0];

                if (!splitOnPlus[1].Equals("GESMES:2:1:E6"))
                {
                    throw new ArgumentException("Expecting GESMES:2:1:E6 but was " + splitOnPlus[1]);
                }

                this.ReadNextLine();
                EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.MessageFunction, true);
                MessageFunction messageFunction = MessageFunctionExtension.GetFromEdiStr(this._currentLine);

                // Fifth Line is the message agency
                this.ReadNextLine();
                EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.MessageAgency, true);
                string messageAgency = this._currentLine;

                // Sixth Line is the receiving agency
                this.ReadNextLine();
                EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.ReceivingAgency, true);
                string recievingAgency = this._currentLine;

                IParty sendingAgency = this.ProcessMessageSender();

                int documentStart = this._ediReader.LineNumber;
                if (messageFunction.IsData())
                {
                    this.DetermineDatasetMetadata();
                    metadata.ReportingBegin = this._reportingBegin;
                    metadata.ReportingEnd = this._reportingEnd;
                }

                bool inRecursive = false;
                IList<IKeyValue> datasetAttributes = new List<IKeyValue>();
                while (this._currentLine != null)
                {
                    try
                    {
                        this.ReadNextLine();
                        if (!messageFunction.IsData())
                        {
                            if (this._ediReader.LineType.IsDataSegment())
                            {
                                documentStart = this._ediReader.LineNumber + 1;
                                throw new SdmxSyntaxException("Message function is " + messageFunction.GetEdiString() + " but message contains a data segment. Line number : " + documentStart);
                            }
                        }
                        else if (!messageFunction.IsStructure())
                        {
                            if (this._ediReader.LineType.IsStructureSegment())
                            {
                                throw new SdmxSyntaxException("Message function is " + messageFunction.GetEdiString() + " but message contains a structure segment");
                            }
                        }

                        // Check For Any Dataset Attributes And Store Them
                        if (messageFunction.IsData())
                        {
                            if (this._ediReader.LineType == EdiPrefix.DatasetAttributeScope)
                            {
                                int scope = int.Parse(this._ediReader.CurrentLine);

                                // 1 = dataset, 4=mix of dimensions, 5=observation
                                if (scope == 1)
                                {
                                    this.ReadNextLine();
                                    EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.DatasetDataAttribute, true);
                                    while (true)
                                    {
                                        this.ReadNextLine();
                                        if (this._ediReader.LineType != EdiPrefix.DatasetAttributeCoded && this._ediReader.LineType != EdiPrefix.DatasetAttributeUncoded)
                                        {
                                            this._ediReader.MoveBackLine();
                                            break;
                                        }

                                        string attributeConceptId = this._ediReader.CurrentLine;
                                        string attributeValue = null;
                                        if (this._ediReader.LineType == EdiPrefix.DatasetAttributeCoded)
                                        {
                                            // Move to the code value Line
                                            this.AssertMoveNext();

                                            // If the current line is the attribute value then store it, otherwise
                                            if (EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.CodeValue, false))
                                            {
                                                attributeValue = this._ediReader.CurrentLine;
                                            }
                                            else
                                            {
                                                this._ediReader.MoveBackLine();
                                            }
                                        }
                                        else if (this._ediReader.LineType == EdiPrefix.DatasetAttributeUncoded)
                                        {
                                            string compositeValue = string.Empty;
                                            while (true)
                                            {
                                                // Move to the next line and see if it is Free Text
                                                this.AssertMoveNext();
                                                if (EDIUtil.AssertPrefix(this._ediReader, EdiPrefix.String, false))
                                                {
                                                    compositeValue += this._ediReader.ParseTextString();
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }

                                            attributeValue = compositeValue;
                                            this._ediReader.MoveBackLine();
                                        }

                                        datasetAttributes.Add(new KeyValueImpl(attributeValue, attributeConceptId));
                                    }
                                }
                            }
                        }

                        if (this._ediReader.LineType.IsEndMessageAdministration())
                        {
                            if (this._ediReader.IsBackLine)
                            {
                                this._ediReader.MoveNext();
                            }

                            string[] splitMessAdminOnPlus = EDIUtil.SplitOnPlus(this._currentLine, 2);
                            string numLinesString = splitMessAdminOnPlus[0];
                            int numLines = EDIUtil.ParseStringAsInt(numLinesString);
                            int segmentCount = this._ediReader.LineNumber - segmentStart + 1;
                            if (segmentCount != numLines)
                            {
                                throw new SdmxSemmanticException("Expected segment count '" + numLines + "' does not match actual segment count '" + segmentCount + "'");
                            }

                            IEdiDocumentPosition documentPosition = new EdiDocumentPosition(
                                documentStart, 
                                this._ediReader.LineNumber, 
                                messageFunction.IsStructure(), 
                                this._datasetId, 
                                messageAgency, 
                                sendingAgency, 
                                recievingAgency, 
                                this._datasetAction, 
                                this._keyFamilyIdentifier, 
                                this._missingValue, 
                                this._datasetPreperation, 
                                this._reportingPeriod, 
                                datasetAttributes);
                            metadata.AddDocumentIndex(documentPosition);

                            string messageRef = splitMessAdminOnPlus[1];
                            if (!messageIdentification.Equals(messageRef))
                            {
                                throw new SdmxSemmanticException("Message ref expected to be '" + messageIdentification + "' but was '" + messageRef + "'");
                            }

                            // Either we have another message identification or an end message
                            this.ReadNextLine();
                            if (this._ediReader.LineType.IsMessageIdentification())
                            {
                                inRecursive = true;
                                return this.ValidateMessageInterchangeHeader(metadata, ++currentnum);
                            }

                            return currentnum;
                        }
                    }
                    catch (SdmxException th)
                    {
                        if (inRecursive)
                        {
                            throw;
                        }

                        throw new SdmxException("Error while trying to validate Edi Message:" + messageIdentification, th);
                    }
                }

                throw new SdmxSyntaxException("Message identification" + EdiPrefix.MessageIdentification + " is not terminated with an end identification " + EdiPrefix.EndMessageAdministration);
            }

            #endregion
        }
    }
}