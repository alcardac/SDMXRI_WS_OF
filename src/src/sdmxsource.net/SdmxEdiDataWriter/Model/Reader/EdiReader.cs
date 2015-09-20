// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EdiReader.cs" company="Eurostat">
//   Date Created : 2014-07-28
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The edi reader.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Model.Reader
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.EdiParser.Constants;
    using Org.Sdmxsource.Sdmx.EdiParser.Extension;
    using Org.Sdmxsource.Sdmx.EdiParser.Util;

    /// <summary>
    /// The EDI reader.
    /// </summary>
    public class EdiReader : FileReaderImpl, IEdiReader
    {
        #region Static Fields

        /// <summary>
        /// The unreserved dash.
        /// </summary>
        private static readonly Regex _unreservedDash = new Regex(EdiConstants.EndOfLineRegEx, RegexOptions.Compiled);

        /// <summary>
        /// The unreserved plus.
        /// </summary>
        private static readonly Regex _unreservedPlus = new Regex("(?<!\\?)\\+", RegexOptions.Compiled);

        /// <summary>
        /// The unreserved question mark.
        /// </summary>
        private static readonly Regex _unreservedQuestionMark = new Regex("(?<!\\?)\\?(?![\\+\\.'\\?:])", RegexOptions.Compiled);

        #endregion

        #region Fields

        /// <summary>
        /// The end of file reached.
        /// </summary>
        private bool _endOfFileReached;

        /// <summary>
        /// The line type.
        /// </summary>
        private EdiPrefix _lineType;

        /// <summary>
        /// The TMP store.
        /// </summary>
        private IList<string> _tmpStore;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EdiReader"/> class.
        /// </summary>
        /// <param name="dataFile">
        /// The data file.
        /// </param>
        public EdiReader(IReadableDataLocation dataFile)
            : base(dataFile, EdiConstants.EndOfLineRegEx, EdiConstants.CharsetEncoding)
        {
            // End of line terminator is any ' char that is not immediately preceded by a ?
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdiReader"/> class.
        /// </summary>
        /// <param name="dataFile">
        /// The data file.
        /// </param>
        /// <param name="startindex">
        /// The start-index.
        /// </param>
        /// <param name="endIndex">
        /// The end index.
        /// </param>
        public EdiReader(IReadableDataLocation dataFile, int startindex, int endIndex)
            : base(dataFile, EdiConstants.EndOfLineRegEx, startindex, endIndex, EdiConstants.CharsetEncoding)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the type of the line.
        /// </summary>
        /// <value>
        ///     The type of the line.
        /// </value>
        public EdiPrefix LineType
        {
            get
            {
                return this._lineType;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Moves the file pointer to the next line.
        /// </summary>
        /// <returns>false if there is no next line.</returns>
        public override bool MoveNext()
        {
            string linePrefix;
            if (this._tmpStore != null)
            {
                // If there are entries in the temporaryStore, pop the first entry, make it the currentLine
                // and remember to increment the filePosition counter of the superclass, otherwise we'll end up 
                // with a mismatch of actual to expected lines.
                this.CurrentLine = this._tmpStore[0];
                this._tmpStore.RemoveAt(0);
                this.FilePosition++;
                if (this._tmpStore.Count > 0)
                {
                    this._tmpStore = null;
                }

                this._lineType = EdiPrefixExtension.ParseString(this.CurrentLine);
                linePrefix = this._lineType.GetPrefix();
                this.CurrentLine = this.CurrentLine.Substring(linePrefix.Length);

                return true;
            }

            if (this.BackLine)
            {
                this.BackLine = false;
                return true;
            }

            bool nextLine = base.MoveNext();
            if (!nextLine)
            {
                return false;
            }

            // We cannot use the string method Trim() here, since it is important to trim Only the leading 
            // whitespace. There could be trailing whitespace which has meaning in Edi. At this point the 
            // variable currentLine does not contain the end-marker character. 
            // So the Edi "Una:+.? '" would now be in currentLine as "Una:+.? ".
            // The trailing whitespace is Vital in this case, so only trim the leading whitespace
            // otherwise we may produce errors.
            this.CurrentLine = this.CurrentLine.TrimStart();

            if (this._endOfFileReached)
            {
                if (this.CurrentLine.Trim().Length == 0)
                {
                    // There is whitespace after the end of the file. Just simply ignore it.
                    this.CurrentLine = string.Empty;
                    return true;
                }
            }

            // Does this line contain a ' character? If so, this line needs splitting
            int idx = this.CurrentLine.IndexOf(EdiConstants.EndTag, StringComparison.Ordinal);
            if (idx != -1)
            {
                // Reset the temporaryStore and evaluate the entire "current line"
                this._tmpStore = new List<string>();
                string evaluationString = this.CurrentLine;

                while (idx != -1)
                {
                    // Count the number of sequential ? characters at the end of the evaluation valueOfString
                    int questionMarkCount = 0;
                    for (int j = idx - 1; j >= 0; j--)
                    {
                        if (evaluationString[j] == '?')
                        {
                            questionMarkCount++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (questionMarkCount == 0 || questionMarkCount % 2 == 0)
                    {
                        // This should have been a split but wasn't. 
                        // Put the 1st part of the evaluation valueOfString into the temporary store, and the process everything after the 1st ' character
                        // So if the evaluation valueOfString is currently: foo??'bar
                        // foo?? goes into temporary store
                        // bar becomes the next bit to evaluate
                        this._tmpStore.Add(evaluationString.Substring(0, idx));
                        evaluationString = evaluationString.Substring(idx + 1); // The +1 here ensures that the ' character is not included
                        idx = evaluationString.IndexOf(EdiConstants.EndTag, StringComparison.Ordinal);
                    }
                    else
                    {
                        // This was an escaped ' character so hunt for the next one
                        idx = evaluationString.IndexOf(EdiConstants.EndTag[0], idx + 1);
                    }

                    if (idx == -1)
                    {
                        // There are no more ' characters so put whatever remains into the temporary store
                        this._tmpStore.Add(evaluationString);
                    }
                }

                // Pop the first item out of the temporary store
                // Note: there is no need to increment the filePosition counter of the superclass since it was 
                // done in the prior call:  super.MoveNext(); 
                this.CurrentLine = this._tmpStore[0];
                this._tmpStore.RemoveAt(0);
                if (this._tmpStore.Count > 0)
                {
                    this._tmpStore = null;
                }
            }

            this._lineType = EdiPrefixExtension.ParseString(this.CurrentLine);
            linePrefix = this._lineType.GetPrefix();
            if (linePrefix.Equals("UNZ+"))
            {
                this._endOfFileReached = true;
            }

            this.CurrentLine = this.CurrentLine.Substring(linePrefix.Length);
            return true;
        }

        /// <summary>
        ///     Parses the current line as text
        /// </summary>
        /// <returns>the current line as text</returns>
        public string ParseTextString()
        {
            string inputString = this.CurrentLine;
            if (_unreservedPlus.IsMatch(inputString))
            {
                throw new SdmxSyntaxException("Error processing line '" + this.CurrentLine + "' Reserved character '+' must be escaped by escape character '?'");
            }

            if (_unreservedDash.IsMatch(inputString))
            {
                throw new SdmxSyntaxException("Reserved character ''' must be escaped by escape character '?'");
            }

            if (_unreservedQuestionMark.IsMatch(inputString))
            {
                throw new SdmxSyntaxException("Reserved character '?' must be escaped by escape character '?'");
            }

            return EDIUtil.EdiToString(inputString);
        }

        #endregion
    }
}