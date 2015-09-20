// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GesmesHeaderWriter.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   This class is used by the SDMX-EDI or else GESMES/TS write the GESMES/TS Header.
//   Maybe parts ported from the sdmx_io.data.GesmesDataWriter.java
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxEdiDataWriter.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;

    using Estat.Sri.SdmxEdiDataWriter.Helper;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.EdiParser.Constants;
    using Org.Sdmxsource.Sdmx.Util.Date;

    /// <summary>
    ///     This class is used by the SDMX-EDI or else GESMES/TS write the GESMES/TS Header.
    ///     Many parts ported from the <c>sdmx_io.data.GesmesDataWriter.java</c>
    /// </summary>
    public class GesmesHeaderWriter
    {
        #region Fields

        /// <summary>
        ///     This internal field is used to store the default extraction time in case the <see cref="IHeader" /> doesn't include one
        /// </summary>
        private readonly string _defaultExtractedTime;

        /// <summary>
        ///     This internal field is used to store the default preparation time in case the <see cref="IHeader" /> doesn't include one
        /// </summary>
        private readonly string _defaultPreparationTime;

        /// <summary>
        ///     The internal field used to store the <see cref="IHeader" /> object containing the header data to be written
        /// </summary>
        private readonly IHeader _header;

        /// <summary>
        ///     The internal filed containing the mappings, and required for writing data
        /// </summary>
        private readonly IDataStructureObject _keyFamilyBean;

        /// <summary>
        ///     The internal field used to store the TextWriter object that is used
        ///     internally by this class to perform the GESMES writing
        /// </summary>
        private readonly TextWriter _writer;

        /// <summary>
        ///     The internal field used to store the number of segments
        /// </summary>
        private int _segmentCount;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GesmesHeaderWriter"/> class.
        /// </summary>
        /// <param name="writer">
        /// The TextWriter object use to actually perform the writing
        /// </param>
        /// <param name="keyFamilyBean">
        /// The <see cref="IDataStructureObject"/> object containing the mappings
        /// </param>
        /// <param name="header">
        /// The <see cref="IHeader"/> object containing the header data to be written
        /// </param>
        public GesmesHeaderWriter(TextWriter writer, IDataStructureObject keyFamilyBean, IHeader header)
            : this(writer, header)
        {
            this._keyFamilyBean = keyFamilyBean;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GesmesHeaderWriter"/> class.
        ///     Constructor that initialize the internal fields
        ///     <see cref="_writer"/>
        /// </summary>
        /// <param name="writer">
        /// The TextWriter object use to actually perform the writing
        /// </param>
        /// <param name="header">
        /// The <see cref="IHeader"/> object containing the header data to be written
        /// </param>
        /// ///
        private GesmesHeaderWriter(TextWriter writer, IHeader header)
        {
            IFormatProvider fmt = CultureInfo.InvariantCulture;
            this._writer = writer;
            this._keyFamilyBean = null;
            this._header = header;
            DateTime now = DateTime.Now;
            this._defaultExtractedTime = now.ToString(EdiConstants.ExtractedTimeDateTimeFormat, fmt);
            this._defaultPreparationTime = now.ToString(EdiConstants.PreparedTimeDateTimeFormat, fmt);
        }

        #endregion

        #region Enums

        /// <summary>
        ///     GESMES contact roles.
        /// </summary>
        private enum ContactRoles
        {
            /// <summary>
            ///     The CC contact role.
            /// </summary>
            Cc,

            /// <summary>
            ///     The CF. contact role.
            /// </summary>
            Cf,

            /// <summary>
            ///     The CP. contact role.
            /// </summary>
            Cp,

            /// <summary>
            ///     The CE. contact role.
            /// </summary>
            Ce
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the IREF counter
        /// </summary>
        public int IrefCount { get; set; }

        /// <summary>
        ///     Gets or sets the number of segments
        /// </summary>
        public int SegmentCount
        {
            get
            {
                return this._segmentCount;
            }

            set
            {
                this._segmentCount = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// This method writes the GESMES footer
        /// </summary>
        /// <param name="segmentCount">
        /// The number of segments in the GESMES message
        /// </param>
        public void WriteGesmesFooter(int segmentCount)
        {
            var buffer = new StringBuilder();

            // UNT+4256+MREF000001'
            GesmesHelper.StartSegment(buffer, EdiConstants.UntTag);
            buffer.Append(segmentCount + 1L).Append(EdiConstants.Plus).Append(EdiConstants.Mref);
            GesmesHelper.EndSegment(buffer);

            // _writer.WriteLine("UNT+{0}+MREF000001'",++segmentCount);
            // UNZ+1+IREF000006'
            GesmesHelper.StartSegment(buffer, EdiConstants.UnzTag);
            buffer.AppendFormat(EdiConstants.Iref, this.IrefCount);
            GesmesHelper.EndSegment(buffer);
            this._writer.Write(buffer);

            // _writer.Write("UNZ+1+IREF000001'");
        }

        /// <summary>
        /// This is the main method of the class that writes the <see cref="IHeader"/>
        ///     The methods write the GESMES Header.
        /// </summary>
        /// <param name="isDeleteMessage">
        /// Set to true if this is a delete message
        /// </param>
        public void WriteHeader(bool isDeleteMessage)
        {
            var buffer = new StringBuilder();

            // UNA:+.? '
            buffer.AppendLine(EdiConstants.UnaSegment);

            // UNB+UNOC:3+AT1+4D0+090826:0539+IREF000002++GESMES/TS'
            this.WriteUnb(buffer);

            // UNH+MREF000001+GESMES:2:1:E6'
            buffer.AppendLine(EdiConstants.UnhSegment);
            this._segmentCount++;

            // BGM+74'
            buffer.AppendLine(EdiConstants.BgmDataSegment);
            this._segmentCount++;

            // NAD+Z02+EUROSTAT'
            GesmesHelper.StartSegment(buffer, EdiConstants.NadTag)
                        .Append(EdiConstants.Z02Code)
                        .Append(EdiConstants.Plus)
                        .Append(this.GetAgency());
            GesmesHelper.EndSegment(buffer);

            // _writer.WriteLine("NAD+Z02+{0}'", GetAgency());
            this._segmentCount++;

            // NAD+MR+4D0'
            GesmesHelper.StartSegment(buffer, EdiConstants.NadTag)
                        .Append(EdiConstants.SenderMr)
                        .Append(GetParty(false, this._header.Receiver));
            GesmesHelper.EndSegment(buffer);

            // _writer.WriteLine("NAD+MR+{0}'", GetParty(_header.Receivers));
            this._segmentCount++;

            // NAD+MS+AT1'
            GesmesHelper.StartSegment(buffer, EdiConstants.NadTag)
                        .Append(EdiConstants.SenderMs)
                        .Append(GetParty(true, this._header.Sender));
            GesmesHelper.EndSegment(buffer);

            // _writer.WriteLine("NAD+MS+{0}'", GetParty(_header.Senders));
            this._segmentCount++;

            // IDE+10+ESAP2SEC_0800_A'
            // This is the subject. Maybe transmission name should go there.
            // _writer.WriteLine("IDE+10+{0}'", header.Names[0].Text);
            this.WriteCtaCom(buffer);

            // DSI+ESAP2SEC_0800_A'
            GesmesHelper.StartSegment(buffer, EdiConstants.DsiTag).Append(this._header.Id);
            GesmesHelper.EndSegment(buffer);
            this._segmentCount++;

            // STS+3+7'
            GesmesHelper.StartSegment(buffer, EdiConstants.StsTag).Append(GetDataSetAction(isDeleteMessage));
            GesmesHelper.EndSegment(buffer);

            // _writer.WriteLine("STS+3+{0}'",GetDataSetAction(isDeleteMessage));
            this._segmentCount++;

            // DTM+242:200908260539:203'
            GesmesHelper.StartSegment(buffer, EdiConstants.DtmTag);
            buffer.Append(EdiConstants.ExtractedDay);
            buffer.Append(EdiConstants.Colon);
            buffer.Append(this.GetExtractionDate());
            buffer.Append(EdiConstants.Colon);
            buffer.AppendFormat("{0:D}", EdiTimeFormat.MinuteFourDigYear);
            GesmesHelper.EndSegment(buffer);

            // _writer.WriteLine("DTM+242:{0}:203'", GetExtractionDate());
            this._segmentCount++;

            // DTM+Z02:19952008:702'
            this.WriteDtmZ02(buffer);

            // IDE+5+ESTAT_ESAIEA'
            GesmesHelper.StartSegment(buffer, EdiConstants.IdeTag);
            buffer.Append(EdiConstants.KeyFamilyIdentifierCode);
            buffer.Append(EdiConstants.Plus);
            buffer.Append(this.GetKeyFamily());
            GesmesHelper.EndSegment(buffer);

            // _writer.WriteLine("IDE+5+{0}'",GetKeyFamily());
            this._segmentCount++;

            // GIS+AR3'
            buffer.AppendLine(EdiConstants.GisAr3Segment);
            this._segmentCount++;

            // GIS+1:::-'
            buffer.AppendLine(EdiConstants.Gis1Segment);
            this._segmentCount++;
            this._writer.Write(buffer);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the dataset action, segment STS+3+, from <see cref="_header"/>
        /// </summary>
        /// <param name="isDeleteMessage">
        /// The is Delete Message.
        /// </param>
        /// <returns>
        /// The dataset action code for STS+3
        /// </returns>
        private static int GetDataSetAction(bool isDeleteMessage)
        {
            int sts = EdiConstants.DataSetActionUpdateCode;
            if (isDeleteMessage)
            {
                sts = EdiConstants.DataSetActionDeleteCode;
            }

            return sts;
        }

        /// <summary>
        /// Extracts date in <c>yyyyMMdd</c> format from SDMX-ML HeaderTimeType
        /// </summary>
        /// <param name="headerTimeType">
        /// a string containing the headerTimeType
        /// </param>
        /// <returns>
        /// Date in <c>yyyyMMdd</c> format or null if headerTimeType is null,empty or less than 10 characters
        /// </returns>
        private static string GetDateFromHeaderTimeType(DateTime? headerTimeType)
        {
            if (headerTimeType == null)
            {
                return null;
            }

            return headerTimeType.Value.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Get CTA function from sdmx role if possible or use the default "CC"
        /// </summary>
        /// <param name="sdmxRole">
        /// The SDMX contact role
        /// </param>
        /// <returns>
        /// The CTA function
        /// </returns>
        private static string GetFunction(string sdmxRole)
        {
            ContactRoles gesmesRole;
            if (Enum.TryParse(sdmxRole, true, out gesmesRole))
            {
                return sdmxRole.ToUpperInvariant();
            }

            return ContactRoles.Cc.ToString().ToUpperInvariant();
        }

        /// <summary>
        /// Get the first party from a list of <see cref="IParty" /> objects
        /// or "ZZ9" if none exists
        /// </summary>
        /// <param name="isSender">if set to <c>true</c> it is a sender.</param>
        /// <param name="parties">The list of <see cref="IParty" /> objects</param>
        /// <returns>
        /// the first PartyTimeBean id or ZZ9
        /// </returns>
        private static string GetParty(bool isSender, IList<IParty> parties)
        {
            if (parties != null && parties.Count > 0 && !string.IsNullOrEmpty(parties[0].Id))
            {
                return parties[0].Id;
            }

            return isSender ? EdiConstants.DefaultSenderId : EdiConstants.DefaultRecieverId; // GesmesConstants.DefaultPartyId;
        }

        /// <summary>
        /// Get the first party from a list of <see cref="IParty" /> objects
        /// or "ZZ9" if none exists
        /// </summary>
        /// <param name="isSender">if set to <c>true</c> is a sender.</param>
        /// <param name="parties">The list of <see cref="IParty" /> objects</param>
        /// <returns>
        /// the first PartyTimeBean id or ZZ9
        /// </returns>
        private static string GetParty(bool isSender, params IParty[] parties)
        {
            IList<IParty> list = parties;
            return GetParty(isSender, list);
        }

        /// <summary>
        /// Get the first <c>TextType</c> if it exists else <see cref="EdiConstants.UnknownText"/>
        /// </summary>
        /// <param name="textTypeList">
        /// A list of <c>TextType</c>
        /// </param>
        /// <returns>
        /// The text from the first <c>TextType</c> or else  <see cref="EdiConstants.UnknownText"/>
        /// </returns>
        private static string GetText(IEnumerable<ITextTypeWrapper> textTypeList)
        {
            if (textTypeList != null)
            {
                foreach (var keyValuePair in textTypeList)
                {
                    return keyValuePair.Value;
                }
            }

            return EdiConstants.UnknownText;
        }

        /// <summary>
        ///     Get the Agency from either the <see cref="_keyFamilyBean" /> or if it is null the <see cref="_header" />
        ///     KeyFamilyAgency.
        ///     Agency named <see cref="EdiConstants.SdmxEstatAgency" /> is converted to
        ///     <see
        ///         cref="EdiConstants.GesmesEstatAgency" />
        /// </summary>
        /// <returns>
        ///     The key family agency
        /// </returns>
        private string GetAgency()
        {
            string agency = null;
            if (this._keyFamilyBean != null && !string.IsNullOrEmpty(this._keyFamilyBean.AgencyId))
            {
                agency = this._keyFamilyBean.AgencyId;
            }
            else
            {
                IDatasetStructureReference datasetStructureReference = this._header.GetStructureById(this.GetKeyFamily());
                if (datasetStructureReference != null)
                {
                    agency = datasetStructureReference.StructureReference.MaintainableReference.AgencyId;
                }
            }

            if (EdiConstants.SdmxEstatAgency.Equals(agency))
            {
                agency = EdiConstants.GesmesEstatAgency;
            }

            return agency;
        }

        /// <summary>
        ///     Get the extraction date from the <see cref="IHeader" /> and convert its to GESMES extraction date time.
        /// </summary>
        /// <returns>
        ///     The GESMES extraction time
        /// </returns>
        private string GetExtractionDate()
        {
            if (this._header.Extracted.HasValue)
            {
                return this._header.Extracted.Value.ToString(
                    EdiConstants.ExtractedTimeDateTimeFormat, CultureInfo.InvariantCulture);
            }

            return this._defaultExtractedTime;
        }

        /// <summary>
        ///     Get the key family identifier either from <see cref="_keyFamilyBean" /> or if it is null
        ///     from <see cref="_header" />
        /// </summary>
        /// <returns>
        ///     The key family id
        /// </returns>
        private string GetKeyFamily()
        {
            //////IList<IDatasetStructureReference> datasetStructureReferences = this._header.Structures;
            //////if (this._keyFamilyBean == null)
            //////{
            //////    if (ObjectUtil.ValidCollection(datasetStructureReferences))
            //////    {
            //////        return datasetStructureReferences[0].Id;
            //////    }

            //////    throw new InvalidOperationException("Cannot determine the DataStructureDefinition (KeyFamily/DSD)");
            //////}
            //// TODO fix KF id output
            return this._keyFamilyBean.Id;
        }

        /// <summary>
        ///     Get the preparation date from the <see cref="IHeader" /> and convert its to GESMES preparation date time.
        ///     (Ported from Java SDMX.IO <c>GesmesDataWriter</c>)
        /// </summary>
        /// <returns>
        ///     The GESMES preparation time
        /// </returns>
        private string GetPreparationDate()
        {
            if (this._header.Prepared.HasValue)
            {
                return this._header.Prepared.Value.ToString(
                    EdiConstants.PreparedTimeDateTimeFormat, CultureInfo.InvariantCulture);
            }

            return this._defaultPreparationTime;
        }

        /// <summary>
        /// Writes the COM if this information exists to <paramref name="buffer"/>
        /// </summary>
        /// <param name="buffer">
        /// The <see cref="StringBuilder"/> to write to
        /// </param>
        /// <param name="coordinateType">
        /// The coordinate type - communication channel
        /// </param>
        /// <param name="coordinateDetail">
        /// The coordinate detail - communication details
        /// </param>
        private void WriteCom(StringBuilder buffer, string coordinateType, string coordinateDetail)
        {
            if (string.IsNullOrEmpty(coordinateType) || string.IsNullOrEmpty(coordinateDetail))
            {
                return;
            }

            GesmesHelper.StartSegment(buffer, EdiConstants.ComTag)
                        .Append(GesmesHelper.EdiEscape(GesmesHelper.TrimExtraText(coordinateDetail, 512)))
                        .Append(EdiConstants.Colon)
                        .Append(coordinateType);
            GesmesHelper.EndSegment(buffer);

            // _writer.WriteLine("COM+{0}:{1}'",GesmesUtils.EdiEscape(GesmesUtils.TrimExtraText(coordinateDetail, 512))) , gesmesContactType);
            this._segmentCount++;
        }

        /// <summary>
        /// Writes the COM if this information exists to <paramref name="buffer"/>
        /// </summary>
        /// <param name="buffer">
        /// The <see cref="StringBuilder"/> to write to
        /// </param>
        /// <param name="list">
        /// The list of communication channels of type <paramref name="coordinateType"/>
        /// </param>
        /// <param name="coordinateType">
        /// The coordinate type - communication channel
        /// </param>
        /// <param name="comCount">
        /// The number of communication channels added.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int WriteCom(StringBuilder buffer, IList<string> list, string coordinateType, int comCount)
        {
            for (int index = 0; index < list.Count && comCount < 5; index++)
            {
                string com = list[index];
                this.WriteCom(buffer, coordinateType, com);
                comCount++;
            }

            return comCount;
        }

        /// <summary>
        /// Method that iterates through data sender information
        ///     and writes CTA and COM segments. For COM it calls the <see cref="WriteCom(System.Text.StringBuilder,string,string)"/>
        /// </summary>
        /// <param name="buffer">
        /// The <see cref="StringBuilder"/> to write to
        /// </param>
        private void WriteCtaCom(StringBuilder buffer)
        {
            if (this._header.Sender != null)
            {
                int ctaCount = 0;
                IParty party = this._header.Sender;
                if (party.Contacts != null)
                {
                    int contactsIndex = 0;
                    while (contactsIndex < party.Contacts.Count && ctaCount < 3)
                    {
                        {
                            IContact contact = party.Contacts[contactsIndex++];
                            string name = GesmesHelper.EdiEscape(GesmesHelper.TrimExtraText(GetText(contact.Name), 35));
                            string function = GetFunction(GetText(contact.Role));

                            // default to "CC" like in GesmesDataWriter.java
                            string department =
                                GesmesHelper.EdiEscape(GesmesHelper.TrimExtraText(GetText(contact.Departments), 17));

                            // CTA+CP+NA:SMIDT'
                            GesmesHelper.StartSegment(buffer, EdiConstants.CtaTag);
                            buffer.Append(function).Append(EdiConstants.Plus);
                            buffer.Append(department).Append(EdiConstants.Colon);
                            buffer.Append(name);
                            GesmesHelper.EndSegment(buffer);

                            // _writer.WriteLine("CTA+{0}+{1}:{2}'", function, department, name);
                            ctaCount++;
                            this._segmentCount++;

                            // COM+FOO.BAR@SOMEWHERE.COM:EM'
                            int comCount = 0;

                            comCount = this.WriteCom(buffer, contact.Telephone, EdiConstants.Telephone, comCount);
                            comCount = this.WriteCom(buffer, contact.Email, EdiConstants.Email, comCount);
                            comCount = this.WriteCom(buffer, contact.Fax, EdiConstants.Fax, comCount);
                            this.WriteCom(buffer, contact.X400, EdiConstants.X400, comCount);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method that writes DTM+Z02 segment if <see cref="_header"/> contains at least ReportingBegin
        /// </summary>
        /// <param name="dtm">
        /// The dtm.
        /// </param>
        private void WriteDtmZ02(StringBuilder dtm)
        {
            string begin = GetDateFromHeaderTimeType(this._header.ReportingBegin);
            string end = GetDateFromHeaderTimeType(this._header.ReportingEnd);

            // = new StringBuilder("DTM+Z02:");
            // check if begin is empty or it is not at least xs:date
            if (begin == null)
            {
                return;
            }

            GesmesHelper.StartSegment(dtm, EdiConstants.DtmTag)
                        .Append(EdiConstants.Z02Code)
                        .Append(EdiConstants.Colon);
            dtm.Append(begin);
            GesmesPeriod gesmesPeriod = PeriodicityFactory.Create(TimeFormatEnumType.Date).Gesmes;
            string timeFormat = GesmesHelper.GetTimeFormatCode(gesmesPeriod.DateFormat);
            if (end != null)
            {
                dtm.Append(end);
                timeFormat = GesmesHelper.GetTimeFormatCode(gesmesPeriod.RangeTimeFormat);
            }

            dtm.Append(EdiConstants.Colon);
            dtm.Append(timeFormat);
            GesmesHelper.EndSegment(dtm);
            this._segmentCount++;
        }

        /// <summary>
        /// Get a UNB segment constructed from the information from the <see cref="_header"/>
        /// </summary>
        /// <param name="unb">
        /// The unb.
        /// </param>
        private void WriteUnb(StringBuilder unb)
        {
            GesmesHelper.StartSegment(unb, EdiConstants.UnbTag);

            unb.Append(GetParty(true, this._header.Sender));
            unb.Append(EdiConstants.Plus);
            unb.Append(GetParty(false, this._header.Receiver));
            unb.Append(EdiConstants.Plus);
            unb.Append(this.GetPreparationDate());
            unb.Append(EdiConstants.Plus);
            this.IrefCount++;
            unb.AppendFormat(EdiConstants.Iref, this.IrefCount);
            unb.Append(EdiConstants.Plus);
            unb.Append(EdiConstants.Plus);
            unb.Append(EdiConstants.GesmesTs);

            // unb.Append("IREF000001++GESMES/TS");
            if (this._header.Test)
            {
                unb.Append(EdiConstants.TestFlag);
            }

            GesmesHelper.EndSegment(unb);
        }

        #endregion
    }
}