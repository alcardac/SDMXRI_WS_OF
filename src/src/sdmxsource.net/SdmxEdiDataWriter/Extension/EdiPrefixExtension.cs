// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EdiPrefixExtension.cs" company="Eurostat">
//   Date Created : 2014-07-22
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Extensions for <see cref="EdiPrefix" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Extension
{
    using System;

    using Org.Sdmxsource.Sdmx.EdiParser.Constants;
    using Org.Sdmxsource.Sdmx.EdiParser.Properties;

    /// <summary>
    ///     Extensions for <see cref="EdiPrefix" />
    /// </summary>
    public static class EdiPrefixExtension
    {
        #region Static Fields

        /// <summary>
        ///     The EDI prefixes
        /// </summary>
        private static readonly string[] _ediPrefixes;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="EdiPrefixExtension" /> class.
        /// </summary>
        static EdiPrefixExtension()
        {
            _ediPrefixes = new string[Enum.GetValues(typeof(EdiPrefix)).Length];
            _ediPrefixes[(int)EdiPrefix.Null] = null;
            _ediPrefixes[(int)EdiPrefix.MessageStart] = "UNA:+.? ";
            _ediPrefixes[(int)EdiPrefix.InterchangeHeader] = "UNB+";
            _ediPrefixes[(int)EdiPrefix.MessageIdentification] = "UNH+";
            _ediPrefixes[(int)EdiPrefix.MessageFunction] = "BGM+";
            _ediPrefixes[(int)EdiPrefix.EndMessageAdministration] = "UNT+";
            _ediPrefixes[(int)EdiPrefix.EndMessage] = "UNZ+";
            _ediPrefixes[(int)EdiPrefix.ContactInformation] = "CTA+";
            _ediPrefixes[(int)EdiPrefix.CommunicationNumber] = "COM+";
            _ediPrefixes[(int)EdiPrefix.DataStart] = "DSI+";
            _ediPrefixes[(int)EdiPrefix.DatasetAction] = "STS+3+";
            _ediPrefixes[(int)EdiPrefix.DatasetDatetime] = "DTM+";
            _ediPrefixes[(int)EdiPrefix.DatasetSendMethod] = "GIS+AR3";
            _ediPrefixes[(int)EdiPrefix.DatasetMissingValueSymbol] = "GIS+1:::";
            _ediPrefixes[(int)EdiPrefix.DatasetData] = "ARR++";
            _ediPrefixes[(int)EdiPrefix.DatasetDataAttribute] = "ARR+";
            _ediPrefixes[(int)EdiPrefix.DatasetFootnoteSection] = "FNS+";
            _ediPrefixes[(int)EdiPrefix.DatasetAttributeScope] = "REL+Z01+";
            _ediPrefixes[(int)EdiPrefix.DatasetAttributeCoded] = "IDE+Z10+";
            _ediPrefixes[(int)EdiPrefix.MessageIdProvidedBySender] = "IDE+10+";
            _ediPrefixes[(int)EdiPrefix.DatasetAttributeUncoded] = "IDE+Z11+";
            _ediPrefixes[(int)EdiPrefix.DsdReference] = "IDE+5+";
            _ediPrefixes[(int)EdiPrefix.MessageAgency] = "NAD+Z02+";
            _ediPrefixes[(int)EdiPrefix.ReceivingAgency] = "NAD+MR+";
            _ediPrefixes[(int)EdiPrefix.SendingAgency] = "NAD+MS+";
            _ediPrefixes[(int)EdiPrefix.Codelist] = "VLI+";
            _ediPrefixes[(int)EdiPrefix.CodeValue] = "CDV+";
            _ediPrefixes[(int)EdiPrefix.Dsd] = "ASI+";
            _ediPrefixes[(int)EdiPrefix.Attribute] = "SCD+Z09+";
            _ediPrefixes[(int)EdiPrefix.Dimension] = "SCD+";
            _ediPrefixes[(int)EdiPrefix.Concept] = "STC+";
            _ediPrefixes[(int)EdiPrefix.String] = "FTX+ACM+++";
            _ediPrefixes[(int)EdiPrefix.FieldLength] = "ATT+3+5+:::";
            _ediPrefixes[(int)EdiPrefix.UseageStatus] = "ATT+3+35+";
            _ediPrefixes[(int)EdiPrefix.AttributeAttachmentValue] = "ATT+3+32+";
            _ediPrefixes[(int)EdiPrefix.CodelistReference] = "IDE+1+";
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// The prefix string
        /// </returns>
        public static string GetPrefix(this EdiPrefix prefix)
        {
            return _ediPrefixes[(int)prefix];
        }

        /// <summary>
        /// Determines whether the specified prefix is attribute.
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// <c>true</c> if the line meets the conditions; otherwise <c>false</c>
        /// </returns>
        public static bool IsAttribute(this EdiPrefix prefix)
        {
            return prefix == EdiPrefix.Attribute;
        }

        /// <summary>
        /// Determines whether the current line is codelist reference
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// <c>true</c> if the line meets the conditions; otherwise <c>false</c>
        /// </returns>
        public static bool IsCodelistReference(this EdiPrefix prefix)
        {
            return prefix == EdiPrefix.CodelistReference;
        }

        /// <summary>
        /// Determines whether the current line is codelist segment
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// <c>true</c> if the line meets the conditions; otherwise <c>false</c>
        /// </returns>
        public static bool IsCodelistSegment(this EdiPrefix prefix)
        {
            return prefix == EdiPrefix.Codelist;
        }

        /// <summary>
        /// Determines whether the current line is concept segment
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// <c>true</c> if the line meets the conditions; otherwise <c>false</c>
        /// </returns>
        public static bool IsConceptSegment(this EdiPrefix prefix)
        {
            return prefix == EdiPrefix.Concept;
        }

        /// <summary>
        /// Determines whether the current line is DSD segment
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// <c>true</c> if the line meets the conditions; otherwise <c>false</c>
        /// </returns>
        public static bool IsDSDSegment(this EdiPrefix prefix)
        {
            return prefix == EdiPrefix.Dsd;
        }

        /// <summary>
        /// Determines whether the current line is data segment
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// <c>true</c> if the line meets the conditions; otherwise <c>false</c>
        /// </returns>
        public static bool IsDataSegment(this EdiPrefix prefix)
        {
            return prefix == EdiPrefix.DataStart;
        }

        /// <summary>
        /// Determines whether the current line is end message administration
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// <c>true</c> if the line meets the conditions; otherwise <c>false</c>
        /// </returns>
        public static bool IsEndMessageAdministration(this EdiPrefix prefix)
        {
            return prefix == EdiPrefix.EndMessageAdministration;
        }

        /// <summary>
        /// Determines whether the current line is the message start
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// true if the current line is the message start; otherwise false.
        /// </returns>
        public static bool IsInterchangeHeader(this EdiPrefix prefix)
        {
            return prefix == EdiPrefix.InterchangeHeader;
        }

        /// <summary>
        /// Returns true if the current line is the message start
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// <c>true</c> if the line meets the conditions; otherwise <c>false</c>
        /// </returns>
        public static bool IsMessageFunction(this EdiPrefix prefix)
        {
            return prefix == EdiPrefix.MessageFunction;
        }

        /// <summary>
        /// Determines whether the current line is message identification
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// <c>true</c> if the line meets the conditions; otherwise <c>false</c>
        /// </returns>
        public static bool IsMessageIdentification(this EdiPrefix prefix)
        {
            return prefix == EdiPrefix.MessageIdentification;
        }

        /// <summary>
        /// Determines whether if the current line is the message start
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// true if the current line is the message start; otherwise false.
        /// </returns>
        public static bool IsMessageStart(this EdiPrefix prefix)
        {
            return prefix == EdiPrefix.MessageStart;
        }

        /// <summary>
        /// Determines whether the current line is structure segment
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// <c>true</c> if the line meets the conditions; otherwise <c>false</c>
        /// </returns>
        public static bool IsStructureSegment(this EdiPrefix prefix)
        {
            return IsCodelistSegment(prefix) || IsConceptSegment(prefix) || IsDSDSegment(prefix);
        }

        /// <summary>
        /// Returns the prefix the specified <paramref name="line"/> starts with.
        /// </summary>
        /// <param name="line">
        /// The line.
        /// </param>
        /// <returns>
        /// The <see cref="EdiPrefix"/>.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Unknown EDI Prefix for Line <paramref name="line"/>
        /// </exception>
        public static EdiPrefix ParseString(string line)
        {
            foreach (EdiPrefix ediPrefix in Enum.GetValues(typeof(EdiPrefix)))
            {
                if (ediPrefix != EdiPrefix.Null)
                {
                    string prefix = ediPrefix.GetPrefix();
                    if (line.StartsWith(prefix, StringComparison.Ordinal))
                    {
                        return ediPrefix;
                    }
                }
            }

            throw new ArgumentException(Resources.UnknownEDIPrefix + line, "line");
        }

        #endregion
    }
}