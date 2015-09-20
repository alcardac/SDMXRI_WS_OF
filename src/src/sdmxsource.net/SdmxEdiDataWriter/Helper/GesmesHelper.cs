// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GesmesHelper.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   A collection of useful static methods for writing GESMES messages
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxEdiDataWriter.Helper
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;

    using Estat.Sri.SdmxEdiDataWriter.Constants;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.EdiParser.Constants;
    using Org.Sdmxsource.Sdmx.Util.Date;

    /// <summary>
    ///     A collection of useful static methods for writing GESMES messages
    /// </summary>
    internal static class GesmesHelper
    {
        #region Public Properties

        /// <summary>
        ///     Gets a new random temporary path of a directory under <see cref="Path.GetTempPath" />
        /// </summary>
        public static string TempPath
        {
            get
            {
                string fileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                while (Directory.Exists(fileName))
                {
                    fileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                }

                return fileName;
            }
        }

        #endregion

        /*
        /// <summary>
        /// This field stores the regular expression used to "convert" the sdmx-ml period to gesmes/ts
        /// </summary>
        private static readonly Regex _removeExtraCharactersFromPeriod = new Regex("[WQTB\\-]");
*/
        #region Public Methods and Operators

        /// <summary>
        /// Write a ARR segment to the specified <paramref name="arr"/> buffer
        /// </summary>
        /// <param name="arr">
        /// The Buffer
        /// </param>
        /// <param name="lastDimensionPosition">
        /// The last dimension position. Use -1 for non attribute usage. Else enter the last dimension position. See SDMX-EDI ARR segment (attribute section) description
        /// </param>
        /// <param name="arrayCellData">
        /// The ARR segment body. It will not escape it
        /// </param>
        /// <returns>
        /// The <see cref="StringBuilder"/> provided at <paramref name="arr"/>
        /// </returns>
        public static StringBuilder ArrSegment(
            StringBuilder arr, int lastDimensionPosition, params string[] arrayCellData)
        {
            string lastPos = lastDimensionPosition > -1
                                 ? lastDimensionPosition.ToString(CultureInfo.InvariantCulture)
                                 : string.Empty;
            StartSegment(arr, EdiConstants.ArrTag).Append(lastPos);
            if (arrayCellData.Length > 0)
            {
                arr.Append(EdiConstants.Plus)
                   .Append(
                       string.Join(EdiConstants.Colon.ToString(CultureInfo.InvariantCulture), arrayCellData));
            }

            return EndSegment(arr);
        }

        /// <summary>
        /// Write a ARR segment to the specified <paramref name="arr"/> buffer
        /// </summary>
        /// <param name="arr">
        /// The Buffer
        /// </param>
        /// <param name="arrayCellData">
        /// The ARR segment body. It will not escape it
        /// </param>
        /// <returns>
        /// The <see cref="StringBuilder"/> provided at <paramref name="arr"/>
        /// </returns>
        public static StringBuilder ArrSegment(StringBuilder arr, params string[] arrayCellData)
        {
            return ArrSegment(arr, -1, arrayCellData);
        }

        /// <summary>
        /// Write a ARR segment to new buffer
        /// </summary>
        /// <param name="arrayCellData">
        /// The ARR segment body. It will not escape it
        /// </param>
        /// <returns>
        /// The new buffer
        /// </returns>
        public static StringBuilder ArrSegment(params string[] arrayCellData)
        {
            return ArrSegment(-1, arrayCellData);
        }

        /// <summary>
        /// Append to <paramref name="buffer"/> the IDE+Z10+.. and CDV+ segments
        /// </summary>
        /// <param name="buffer">
        /// The buffer.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The number of segments added
        /// </returns>
        public static int CodedAttribute(StringBuilder buffer, string name, string value)
        {
            int segmentCount = IdeAttributeSegment(buffer, EdiConstants.CodedAttrZ10, name);
            if (string.IsNullOrEmpty(value))
            {
                return segmentCount;
            }

            StartSegment(buffer, EdiConstants.CdvTag).Append(value);
            EndSegment(buffer);
            return ++segmentCount;
        }

        /// <summary>
        /// Method that escapes EDIFACT special characters with a question mark.
        /// </summary>
        /// <param name="text">
        /// The input text
        /// </param>
        /// <returns>
        /// The input text with all colons, question marks, single quotes and plus signs prefixed with a question mark
        /// </returns>
        public static string EdiEscape(string text)
        {
            // NOTE GesmesConstants.EscapeChar must be first!!!
            var specialChars = new[]
                                   {
                                       EdiConstants.EscapeChar, EdiConstants.Colon, 
                                       EdiConstants.EndTag, EdiConstants.Plus
                                   };
            foreach (var specialChar in specialChars)
            {
                string replacement = string.Format(
                    CultureInfo.InvariantCulture, "{0}{1}", EdiConstants.EscapeChar, specialChar);
                text = text.Replace(specialChar, replacement);
            }

            return text;
        }

        /// <summary>
        /// End segment and append a line
        /// </summary>
        /// <param name="segment">
        /// The Buffer
        /// </param>
        /// <returns>
        /// <paramref name="segment"/>
        ///     The <see cref="StringBuilder"/> provided at <paramref name="segment"/>
        /// </returns>
        public static StringBuilder EndSegment(StringBuilder segment)
        {
            return segment.Append(EdiConstants.EndTag).AppendLine();
        }

        /// <summary>
        /// Getter that returns the GESMES/TS correct observation value. E.g. if it is null or empty then return a
        ///     <see cref="EdiConstants.MissingVal"/>
        ///     character
        ///     else the observation value as it is
        /// </summary>
        /// <param name="p">
        /// The observation value
        /// </param>
        /// <returns>
        /// The observation value or "-"
        /// </returns>
        public static string GetObsValue(string p)
        {
            return GetObsValue(p, EdiConstants.MissingVal);
        }

        /// <summary>
        /// Getter that returns the GESMES/TS correct observation value. E.g. if it is null or empty then return the
        ///     <paramref name="defaultObs"/>
        ///     else the observation value as it is
        /// </summary>
        /// <param name="p">
        /// The observation value
        /// </param>
        /// <param name="defaultObs">
        /// The default observation
        /// </param>
        /// <returns>
        /// The observation value or <paramref name="defaultObs"/>
        /// </returns>
        public static string GetObsValue(string p, string defaultObs)
        {
            return string.IsNullOrEmpty(p) || p.Equals(XmlConstants.NaN) ? defaultObs : p;
        }

        /// <summary>
        /// Method that converts the given the SDMX-ML period to GESMES/TS
        /// </summary>
        /// <param name="timeDimensionValue">
        /// The SDMX-ML time, the value of the time dimension
        /// </param>
        /// <returns>
        /// The GESMES compatible period
        /// </returns>
        public static string GetPeriodValue(string timeDimensionValue)
        {
            if (timeDimensionValue.Length < 6 || timeDimensionValue[4] != '-')
            {
                return timeDimensionValue;
            }

            // 0123456789
            // 2001-01
            // 2001-Q1
            // 2001-H2
            // 2001-W2
            // 2001-W12
            // 0123456789
            // 2001-01-23
            string year = timeDimensionValue.Substring(0, 4);
            string period;
            switch (timeDimensionValue[5])
            {
                case '1':
                case '0':
                    period = timeDimensionValue.Substring(5, 2);
                    break;
                default:
                    period = timeDimensionValue.Substring(6, timeDimensionValue.Length - 6);
                    break;
            }

            string date = string.Empty;
            if (timeDimensionValue.Length == 10)
            {
                date = timeDimensionValue.Substring(8, 2);
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", year, period, date);

            ////return _removeExtraCharactersFromPeriod.Replace(timeDimensionValue, string.Empty);
        }

        /// <summary>
        /// Get the code (number) as string of the specified <paramref name="periodCode"/>
        /// </summary>
        /// <param name="periodCode">
        /// The <see cref="EdiTimeFormat"/>
        /// </param>
        /// <returns>
        /// The code (number) as string of the specified <paramref name="periodCode"/>
        /// </returns>
        public static string GetTimeFormatCode(EdiTimeFormat periodCode)
        {
            return periodCode.ToString("D");
        }

        /// <summary>
        /// Get the code (number) as string of the specified <paramref name="periodCode"/>
        /// </summary>
        /// <param name="periodCode">
        /// The <see cref="EdiTimeFormat"/>
        /// </param>
        /// <returns>
        /// The code (number) as string of the specified <paramref name="periodCode"/>
        /// </returns>
        public static string GetTimeFormatCode(int periodCode)
        {
            return periodCode.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Append to <paramref name="buffer"/> the IDE+Z.. segment
        /// </summary>
        /// <param name="buffer">
        /// The buffer.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The number of segments added
        /// </returns>
        public static int IdeAttributeSegment(StringBuilder buffer, string type, string name)
        {
            StartSegment(buffer, EdiConstants.IdeTag).Append(type).Append(name);
            EndSegment(buffer);
            return 1;
        }

        /// <summary>
        /// Start a segment and write it to the <paramref name="segment"/> buffer
        /// </summary>
        /// <param name="segment">
        /// The Buffer
        /// </param>
        /// <param name="tag">
        /// The segment tag
        /// </param>
        /// <returns>
        /// The <see cref="StringBuilder"/> provided with <paramref name="segment"/>
        /// </returns>
        public static StringBuilder StartSegment(StringBuilder segment, string tag)
        {
            return segment.Append(tag);
        }

        /// <summary>
        /// Method that removes from text, the characters that exceeds the max size
        /// </summary>
        /// <param name="text">
        /// The input text
        /// </param>
        /// <param name="maxSize">
        /// The maximum size
        /// </param>
        /// <returns>
        /// The text with max length = max size
        /// </returns>
        public static string TrimExtraText(string text, int maxSize)
        {
            if (text.Length > maxSize)
            {
                return text.Substring(0, maxSize);
            }

            return text;
        }

        /// <summary>
        /// Append to <paramref name="buffer"/> the IDE+Z11+.. and FTX+ segments
        /// </summary>
        /// <param name="buffer">
        /// The buffer.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The number of segments added
        /// </returns>
        public static int UncodedAttribute(StringBuilder buffer, string name, string value)
        {
            int segmentCount = IdeAttributeSegment(buffer, EdiConstants.UncodedAttrZ11, name);

            if (string.IsNullOrEmpty(value))
            {
                return segmentCount;
            }

            // Each FTX can have up to 350 characters of free text
            for (int ftx = 0; ftx < value.Length; ftx += EdiConstants.MaxFtx)
            {
                string ftxValue = value.Substring(ftx, Math.Min(EdiConstants.MaxFtx, value.Length - ftx)).Trim();
                StartSegment(buffer, EdiConstants.FtxTag);

                // The free text inside the FTX must be split 
                for (int component = 0; component < ftxValue.Length; component += EdiConstants.MaxFtxComponent)
                {
                    string componentValue =
                        EdiEscape(
                            ftxValue.Substring(
                                component, Math.Min(EdiConstants.MaxFtxComponent, ftxValue.Length - component)));
                    buffer.AppendFormat(EdiConstants.ComponentColonFormat, componentValue);
                }

                buffer.Length--;
                EndSegment(buffer);
                segmentCount++;
            }

            return segmentCount;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Write a ARR segment to new buffer
        /// </summary>
        /// <param name="lastDimensionPosition">
        /// The last dimension position. Use -1 for non attribute usage. Else enter the last dimension position. See SDMX-EDI ARR segment (attribute section) description
        /// </param>
        /// <param name="arrayCellData">
        /// The ARR segment body. It will not escape it
        /// </param>
        /// <returns>
        /// The new buffer
        /// </returns>
        private static StringBuilder ArrSegment(int lastDimensionPosition, params string[] arrayCellData)
        {
            return ArrSegment(new StringBuilder(), lastDimensionPosition, arrayCellData);
        }

        #endregion
    }
}