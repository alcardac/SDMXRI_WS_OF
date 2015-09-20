// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EdiConstants.cs" company="Eurostat">
//   Date Created : 2014-07-22
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Various GESMES/SDMX-EDI Constants. Please consult the SDMX-EDI guide for more details
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Constants
{
    using System.Text;

    /// <summary>
    ///     Various GESMES/SDMX-EDI Constants. Please consult the SDMX-EDI guide for more details
    /// </summary>
    public static class EdiConstants
    {
        #region Constants

        /// <summary>
        ///     The ARR Tag.
        /// </summary>
        public const string ArrTag = "ARR+";

        /// <summary>
        ///     GESMES/TS BGM segment for data (Fixed in GESMES/TS)
        /// </summary>
        public const string BgmDataSegment = "BGM+74'";

        /// <summary>
        ///     The CDV+ tag. For data GESMES files this segment contains the coded attribute values
        /// </summary>
        public const string CdvTag = "CDV+";

        /// <summary>
        ///     The coded attribute code. It follows the <see cref="IdeTag" />
        /// </summary>
        public const string CodedAttrZ10 = "Z10+";

        /// <summary>
        ///     The : separator.
        /// </summary>
        public const string Colon = ":";

        /// <summary>
        ///     The COM+ tag
        /// </summary>
        public const string ComTag = "COM+";

        /// <summary>
        ///     A common string format for appending colon separated sections
        /// </summary>
        public const string ComponentColonFormat = "{0}:";

        /// <summary>
        ///     The CTA+ tag
        /// </summary>
        public const string CtaTag = "CTA+";

        /// <summary>
        ///     Data Set Action Delete code
        /// </summary>
        public const int DataSetActionDeleteCode = 6;

        /// <summary>
        ///     Data Set Action Update code
        /// </summary>
        public const int DataSetActionUpdateCode = 7;

        /// <summary>
        ///     Default Sender or Receiver id
        /// </summary>
        public const string DefaultPartyId = "ZZ9";

        /// <summary>
        ///     Default Receiver id
        /// </summary>
        public const string DefaultRecieverId = "unknown";

        /// <summary>
        ///     Default Sender id
        /// </summary>
        public const string DefaultSenderId = "unknown";

        /// <summary>
        ///     Code for Deleting dataset. It follows a <see cref="ArrTag" />
        /// </summary>
        public const string DeleteDataSet = "0";

        /// <summary>
        ///     The DSI+ tag
        /// </summary>
        public const string DsiTag = "DSI+";

        /// <summary>
        ///     The DTM+ tag
        /// </summary>
        public const string DtmTag = "DTM+";

        /// <summary>
        ///     Contact type email
        /// </summary>
        public const string Email = "EM";

        /// <summary>
        ///     The end of line REGEX
        /// </summary>
        public const string EndOfLineRegEx = "(?<!\\?)\\'";

        /// <summary>
        ///     The segment separator
        /// </summary>
        public const string EndTag = "'";

        /// <summary>
        ///     The EDI escape character
        /// </summary>
        public const string EscapeChar = "?";

        /// <summary>
        ///     The extracted time code for <see cref="DtmTag" />
        /// </summary>
        public const string ExtractedDay = "242";

        /// <summary>
        ///     Extracted Time, DateTime format
        /// </summary>
        public const string ExtractedTimeDateTimeFormat = "yyyyMMddHHmm";

        /// <summary>
        ///     Contact type Fax
        /// </summary>
        public const string Fax = "FX";

        /// <summary>
        ///     The FNS+Attributes:10' segment
        /// </summary>
        public const string FnsAttributes = "FNS+Attributes:10'";

        /// <summary>
        ///     The FTX+ACM+++ tag. For data GESMES files this segment contains the uncoded attribute values.
        /// </summary>
        public const string FtxTag = "FTX+ACM+++";

        /// <summary>
        ///     ESTAT specific used for converting ESTAT agency to EUROSTAT
        /// </summary>
        public const string GesmesEstatAgency = "EUROSTAT";

        /// <summary>
        ///     The GESMES/TS data format identifier used in <see cref="UnbTag" />
        /// </summary>
        public const string GesmesTs = "GESMES/TS";

        /// <summary>
        ///     The GIS+1:::-' segment (Fixed in GESMES/TS)
        /// </summary>
        public const string Gis1Segment = "GIS+1:::-'";

        /// <summary>
        ///     The GIS+AR3' segment (Fixed in GESMES/TS)
        /// </summary>
        public const string GisAr3Segment = "GIS+AR3'";

        /// <summary>
        ///     The IDE+ tag. For data is used for declaring attribute names and at the header.
        /// </summary>
        public const string IdeTag = "IDE+";

        /// <summary>
        ///     The IREF string format
        /// </summary>
        public const string Iref = "IREF{0:000000}";

        /// <summary>
        ///     The KeyFamilyIdentifierCode for use with <see cref="IdeTag" />
        /// </summary>
        public const string KeyFamilyIdentifierCode = "5";

        /// <summary>
        ///     Max number characters in a <see cref="FtxTag" /> segment
        /// </summary>
        public const int MaxFtx = 350;

        /// <summary>
        ///     Max number of colon separated sections in a <see cref="FtxTag" /> segment
        /// </summary>
        public const int MaxFtxComponent = 70;

        /// <summary>
        ///     The character used when observation is missing or not provided: -
        /// </summary>
        public const string MissingVal = "-";

        /// <summary>
        ///     The first message MREF000001
        /// </summary>
        public const string Mref = "MREF000001";

        /// <summary>
        ///     The NAD+ tag
        /// </summary>
        public const string NadTag = "NAD+";

        /// <summary>
        ///     The OBS_CONF flag
        /// </summary>
        public const string ObsConf = "OBS_CONF";

        /// <summary>
        ///     The OBS_PRE_BREAK flag
        /// </summary>
        public const string ObsPreBreak = "OBS_PRE_BREAK";

        /// <summary>
        ///     The OBS_STATUS flag
        /// </summary>
        public const string ObsStatus = "OBS_STATUS";

        /// <summary>
        ///     The + separator
        /// </summary>
        public const string Plus = "+";

        /// <summary>
        ///     Prepared Time, DateTime format
        /// </summary>
        public const string PreparedTimeDateTimeFormat = "yyMMdd:HHmm";

        /// <summary>
        ///     The REL+Z01+ tag.
        /// </summary>
        public const string RelZ01Tag = "REL+Z01+";

        /// <summary>
        ///     ESTAT specific used for converting ESTAT agency to EUROSTAT
        /// </summary>
        public const string SdmxEstatAgency = "ESTAT";

        /// <summary>
        ///     MR+ code for <see cref="NadTag" /> (Message receiver)
        /// </summary>
        public const string SenderMr = "MR+";

        /// <summary>
        ///     MS+ code for <see cref="NadTag" /> (Message Sender)
        /// </summary>
        public const string SenderMs = "MS+";

        /// <summary>
        ///     The STS+3+ tag
        /// </summary>
        public const string StsTag = "STS+3+";

        /// <summary>
        ///     Contact type Telephone
        /// </summary>
        public const string Telephone = "TE";

        /// <summary>
        ///     Message test flag
        /// </summary>
        public const string TestFlag = "++++1";

        /// <summary>
        ///     GESMES/TS UNA segment(Fixed in GESMES/TS)
        /// </summary>
        public const string UnaSegment = "UNA:+.? '";

        /// <summary>
        ///     The UNB+UNOC:3+ tag
        /// </summary>
        public const string UnbTag = "UNB+UNOC:3+";

        /// <summary>
        ///     The uncoded attribute code. It follows the <see cref="IdeTag" />
        /// </summary>
        public const string UncodedAttrZ11 = "Z11+";

        /// <summary>
        ///     GESMES/TS UNH segment (Fixed in GESMES/TS)
        /// </summary>
        public const string UnhSegment = "UNH+MREF000001+GESMES:2:1:E6'";

        /// <summary>
        ///     Dummy text to avoid empty mandatory text fields
        /// </summary>
        public const string UnknownText = "UNKNOWN";

        /// <summary>
        ///     UNT+ tag
        /// </summary>
        public const string UntTag = "UNT+";

        /// <summary>
        ///     UNZ+1+ tag (only one message/dataset)
        /// </summary>
        public const string UnzTag = "UNZ+1+";

        /// <summary>
        ///     Contact type X400
        /// </summary>
        public const string X400 = "XF";

        /// <summary>
        ///     Year format
        /// </summary>
        public const string YearFormat = "0000";

        /// <summary>
        ///     Z02 code
        /// </summary>
        public const string Z02Code = "Z02";

        /// <summary>
        ///     The _default field length coded
        /// </summary>
        public const string DefaultFieldLengthCoded = "AN..18";

        /// <summary>
        ///     The _default field length primary measure
        /// </summary>
        public const string DefaultFieldLengthPrimaryMeasure = "AN..15";

        /// <summary>
        ///     The _default field length uncoded
        /// </summary>
        public const string DefaultFieldLengthUncoded = "AN..35";

        #endregion

        #region Static Fields

        /// <summary>
        /// The _charset encoding
        /// </summary>
        private static readonly Encoding _charsetEncoding;
       
        #endregion

        /// <summary>
        /// Initializes static members of the <see cref="EdiConstants"/> class.
        /// </summary>
        static EdiConstants()
        {
            _charsetEncoding = Encoding.GetEncoding("ISO-8859-1");
        }

        #region Public Properties

        /// <summary>
        ///     Gets the char set encoding.
        /// </summary>
        /// <value>
        ///     The char-set encoding.
        /// </value>
        public static Encoding CharsetEncoding
        {
            get
            {
                return _charsetEncoding;
            }
        }

        #endregion
    }
}