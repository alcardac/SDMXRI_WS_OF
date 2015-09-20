// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderRetrieverEngine.cs" company="Eurostat">
//   Date Created : 2013-04-10
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   An engine class that retrieves SDMX Header information from Mapping Store
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     An engine class that retrieves SDMX Header information from Mapping Store
    /// </summary>
    public class HeaderRetrieverEngine
    {
        #region Constants

        /// <summary>
        ///     Mapping store localized text type Department
        /// </summary>
        private const string DepartmentText = "Department";

        /// <summary>
        ///     Mapping store localized text type Name
        /// </summary>
        private const string NameText = "Name";

        /// <summary>
        ///     The receiver text.
        /// </summary>
        private const string ReceiverText = "Receiver";

        /// <summary>
        ///     Mapping store localized text type Role
        /// </summary>
        private const string RoleText = "Role";

        /// <summary>
        ///     The sender text.
        /// </summary>
        private const string SenderText = "Sender";

        /// <summary>
        ///     Mapping store localized text type Source
        /// </summary>
        private const string SourceText = "Source";

        #endregion

        #region Fields

        /// <summary>
        /// The log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(HeaderRetrieverEngine));

        /// <summary>
        ///     The _mapping store DB.
        /// </summary>
        private readonly Database _mappingStoreDb;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderRetrieverEngine"/> class.
        /// </summary>
        /// <param name="settings">
        /// The connection string settings to mapping store
        /// </param>
        public HeaderRetrieverEngine(ConnectionStringSettings settings) : this(new Database(settings))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderRetrieverEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        public HeaderRetrieverEngine(Database mappingStoreDb)
        {
            this._mappingStoreDb = mappingStoreDb;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// This method queries the mapping store for header information for a specific dataflow
        /// </summary>
        /// <param name="dataQuery">
        /// The <see cref="IDataQuery"/> object
        /// </param>
        /// <param name="beginDate">
        /// For ReportingBegin element
        /// </param>
        /// <param name="endDate">
        /// For ReportingEnd element
        /// </param>
        /// <returns>
        /// A <see cref="IHeader"/> object. Otherwise null
        /// </returns>
        public IHeader GetHeader(IDataQuery dataQuery, DateTime? beginDate, DateTime? endDate)
        {
            return this.GetHeader(dataQuery.Dataflow, beginDate, endDate);
        }

        /// <summary>
        /// This method queries the mapping store for header information for a specific dataflow
        /// </summary>
        /// <param name="dataflow">
        /// The <see cref="IDataflowObject"/> object
        /// </param>
        /// <param name="beginDate">
        /// For ReportingBegin element
        /// </param>
        /// <param name="endDate">
        /// For ReportingEnd element
        /// </param>
        /// <returns>
        /// A <see cref="IHeader"/> object. Otherwise null
        /// </returns>
        public IHeader GetHeader(IDataflowObject dataflow, DateTime? beginDate, DateTime? endDate)
        {
            var dataflowId = dataflow.Id;
            var dataflowVersion = dataflow.Version;
            var dataflowAgency = dataflow.AgencyId;

            return this.GetHeader(beginDate, endDate, new MaintainableRefObjectImpl(dataflowAgency, dataflowId, dataflowVersion));
        }

        /// <summary>
        /// This method queries the mapping store for header information for a specific dataflow
        /// </summary>
        /// <param name="dataflow">
        /// The <see cref="DataflowEntity"/> object
        /// </param>
        /// <param name="beginDate">
        /// For ReportingBegin element
        /// </param>
        /// <param name="endDate">
        /// For ReportingEnd element
        /// </param>
        /// <returns>
        /// A <see cref="IHeader"/> object. Otherwise null
        /// </returns>
        public IHeader GetHeader(DataflowEntity dataflow, DateTime? beginDate, DateTime? endDate)
        {
            var dataflowId = dataflow.Id;
            var dataflowVersion = dataflow.Version;
            var dataflowAgency = dataflow.Agency;

            return this.GetHeader(beginDate, endDate, new MaintainableRefObjectImpl(dataflowAgency, dataflowId, dataflowVersion));
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method populates the contact details of a <see cref="IContactMutableObject"/>
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The <see cref="Database"/> instance for Mapping Store database
        /// </param>
        /// <param name="contactSysId">
        /// The contact system identifier. In the database the column CONTACT.CONTACT_ID
        /// </param>
        /// <param name="contact">
        /// The <see cref="IContactMutableObject"/> for which the details will be populated
        /// </param>
        private static void PopulateContactDetails(Database mappingStoreDb, long contactSysId, IContactMutableObject contact)
        {
            string paramId = mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter);

            var sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT CD.CD_ID, CD.CONTACT_ID, CD.TYPE, CD.VALUE ");
            sqlCommand.Append("FROM CONTACT_DETAIL CD ");
            sqlCommand.AppendFormat("WHERE CD.CONTACT_ID = {0} ", paramId);

            using (DbCommand command = mappingStoreDb.GetSqlStringCommand(sqlCommand.ToString()))
            {
                mappingStoreDb.AddInParameter(command, ParameterNameConstants.IdParameter, DbType.Int64, contactSysId);

                using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
                {
                    while (dataReader.Read())
                    {
                        string coordinateType = DataReaderHelper.GetString(dataReader, "TYPE");
                        string coordinateData = DataReaderHelper.GetString(dataReader, "VALUE");

                        contact.AddCoordinateType(coordinateType, coordinateData);
                    }
                }
            }
        }

        /// <summary>
        /// This method populates the Localized Strings (Names, Departments, Roles)
        ///     of a <see cref="IContactMutableObject"/>
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The <see cref="Database"/> instance for Mapping Store database
        /// </param>
        /// <param name="contactSysId">
        /// The contact system identifier. In the database the column CONTACT.CONTACT_ID
        /// </param>
        /// <param name="contact">
        /// The <see cref="IContactMutableObject"/> to be populated in terms of Names, Departments, Roles
        /// </param>
        private static void PopulateContactLocalisedStrings(Database mappingStoreDb, long contactSysId, IContactMutableObject contact)
        {
            string paramId = mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter);

            var sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT HLS.HLS_ID, HLS.TYPE, HLS.HEADER_ID, HLS.PARTY_ID, HLS.CONTACT_ID, HLS.LANGUAGE, HLS.TEXT ");
            sqlCommand.Append("FROM HEADER_LOCALISED_STRING HLS ");
            sqlCommand.AppendFormat("WHERE HLS.CONTACT_ID = {0} ", paramId);

            using (DbCommand command = mappingStoreDb.GetSqlStringCommand(sqlCommand.ToString()))
            {
                mappingStoreDb.AddInParameter(command, ParameterNameConstants.IdParameter, DbType.Int64, contactSysId);

                using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
                {
                    while (dataReader.Read())
                    {
                        var text = new TextTypeWrapperMutableCore { Locale = DataReaderHelper.GetString(dataReader, "LANGUAGE"), Value = DataReaderHelper.GetString(dataReader, "TEXT") };
                        if (!string.IsNullOrWhiteSpace(text.Value) && !string.IsNullOrWhiteSpace(text.Locale))
                        {
                            string textType = DataReaderHelper.GetString(dataReader, "TYPE");

                            if (textType.Equals(NameText, StringComparison.OrdinalIgnoreCase))
                            {
                                contact.Names.Add(text);
                            }
                            else if (textType.Equals(DepartmentText, StringComparison.OrdinalIgnoreCase))
                            {
                                contact.Departments.Add(text);
                            }
                            else if (textType.Equals(RoleText, StringComparison.OrdinalIgnoreCase))
                            {
                                contact.Roles.Add(text);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method populates the Localized Strings (Names, Sources) of a <see cref="IHeader"/>
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The <see cref="Database"/> instance for Mapping Store database
        /// </param>
        /// <param name="headerSysId">
        /// The header system identifier. In the database the column HEADER.HEADER_ID
        /// </param>
        /// <param name="header">
        /// The <see cref="IHeader"/> to be populated in terms of Names and Sources
        /// </param>
        private static void PopulateHeaderLocalisedStrings(Database mappingStoreDb, long headerSysId, IHeader header)
        {
            string paramId = mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter);

            var sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT HLS.HLS_ID, HLS.TYPE, HLS.HEADER_ID, HLS.PARTY_ID, HLS.CONTACT_ID, HLS.LANGUAGE, HLS.TEXT ");
            sqlCommand.Append("FROM HEADER_LOCALISED_STRING HLS ");
            sqlCommand.AppendFormat("WHERE HLS.HEADER_ID = {0} ", paramId);

            using (DbCommand command = mappingStoreDb.GetSqlStringCommand(sqlCommand.ToString()))
            {
                mappingStoreDb.AddInParameter(command, ParameterNameConstants.IdParameter, DbType.Int64, headerSysId);

                using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
                {
                    while (dataReader.Read())
                    {
                        var text = new TextTypeWrapperMutableCore { Locale = DataReaderHelper.GetString(dataReader, "LANGUAGE"), Value = DataReaderHelper.GetString(dataReader, "TEXT") };
                        string textType = DataReaderHelper.GetString(dataReader, "TYPE");

                        // is it a sender or a receiver?
                        if (textType.Equals(NameText, StringComparison.OrdinalIgnoreCase))
                        {
                            header.AddName(new TextTypeWrapperImpl(text, null));
                        }
                        else
                        {
                            if (textType.Equals(SourceText, StringComparison.OrdinalIgnoreCase))
                            {
                                header.AddSource(new TextTypeWrapperImpl(text, null));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method populates the Contacts.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The <see cref="Database"/> instance for Mapping Store database
        /// </param>
        /// <param name="partySysId">
        /// The party system identifier. In the database the column PARTY.PARTY_ID
        /// </param>
        /// <param name="contacts">
        /// The list of contacts to be populated i
        /// </param>
        private static void PopulatePartyContacts(Database mappingStoreDb, long partySysId, ICollection<IContact> contacts)
        {
            string paramId = mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter);

            var sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT CONTACT.CONTACT_ID, CONTACT.PARTY_ID ");
            sqlCommand.Append("FROM CONTACT ");
            sqlCommand.AppendFormat("WHERE CONTACT.PARTY_ID = {0} ", paramId);

            using (DbCommand command = mappingStoreDb.GetSqlStringCommand(sqlCommand.ToString()))
            {
                mappingStoreDb.AddInParameter(command, ParameterNameConstants.IdParameter, DbType.Int64, partySysId);

                using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
                {
                    while (dataReader.Read())
                    {
                        var contact = new ContactMutableObjectCore();
                        long contactSysId = DataReaderHelper.GetInt64(dataReader, "CONTACT_ID");

                        PopulateContactLocalisedStrings(mappingStoreDb, contactSysId, contact);
                        PopulateContactDetails(mappingStoreDb, contactSysId, contact);

                        contacts.Add(new ContactCore(contact));
                    }
                }
            }
        }

        /// <summary>
        /// This method populates the international strings
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The <see cref="Database"/> instance for Mapping Store database
        /// </param>
        /// <param name="partySysId">
        /// The party system identifier. In the database the column PARTY.PARTY_ID
        /// </param>
        /// <param name="partyNames">
        /// The <see cref="ITextTypeWrapper"/>lists  to be populated in terms of Names
        /// </param>
        private static void PopulatePartyLocalisedStrings(Database mappingStoreDb, long partySysId, ICollection<ITextTypeWrapper> partyNames)
        {
            string paramId = mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter);

            var sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT HLS.HLS_ID, HLS.TYPE, HLS.HEADER_ID, HLS.PARTY_ID, HLS.CONTACT_ID, HLS.LANGUAGE, HLS.TEXT ");
            sqlCommand.Append("FROM HEADER_LOCALISED_STRING HLS ");
            sqlCommand.AppendFormat("WHERE HLS.PARTY_ID = {0} ", paramId);

            using (DbCommand command = mappingStoreDb.GetSqlStringCommand(sqlCommand.ToString()))
            {
                mappingStoreDb.AddInParameter(command, ParameterNameConstants.IdParameter, DbType.Int64, partySysId);

                using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
                {
                    while (dataReader.Read())
                    {
                        var text = new TextTypeWrapperMutableCore { Locale = DataReaderHelper.GetString(dataReader, "LANGUAGE"), Value = DataReaderHelper.GetString(dataReader, "TEXT") };
                        string textType = DataReaderHelper.GetString(dataReader, "TYPE");

                        // is it a sender or a receiver?
                        if (textType.Equals(NameText, StringComparison.OrdinalIgnoreCase))
                        {
                            partyNames.Add(new TextTypeWrapperImpl(text, null));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method populates the Senders and Receivers of a <see cref="IHeader"/>
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The <see cref="Database"/> instance for Mapping Store database
        /// </param>
        /// <param name="headerSysId">
        /// The header system identifier. In the database the column HEADER.HEADER_ID
        /// </param>
        /// <param name="header">
        /// The <see cref="IHeader"/> to be populated in terms of Senders and Receivers
        /// </param>
        private static void PoulateHeaderSendersAndReceivers(Database mappingStoreDb, long headerSysId, IHeader header)
        {
            string paramId = mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter);

            var sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT PARTY.PARTY_ID, PARTY.ID, PARTY.HEADER_ID, PARTY.TYPE ");
            sqlCommand.Append("FROM PARTY ");
            sqlCommand.AppendFormat("WHERE PARTY.HEADER_ID = {0} ", paramId);

            using (DbCommand command = mappingStoreDb.GetSqlStringCommand(sqlCommand.ToString()))
            {
                mappingStoreDb.AddInParameter(command, ParameterNameConstants.IdParameter, DbType.Int64, headerSysId);

                using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
                {
                    while (dataReader.Read())
                    {
                        var id = DataReaderHelper.GetString(dataReader, "ID");
                        long partySysId = DataReaderHelper.GetInt64(dataReader, "PARTY_ID");
                        string partyType = DataReaderHelper.GetString(dataReader, "TYPE");

                        var names = new List<ITextTypeWrapper>();
                        PopulatePartyLocalisedStrings(mappingStoreDb, partySysId, names);

                        var contacts = new List<IContact>();
                        PopulatePartyContacts(mappingStoreDb, partySysId, contacts);

                        var party = new PartyCore(names, id, contacts, null);

                        // is it a sender or a receiver?
                        if (partyType.Equals(SenderText, StringComparison.OrdinalIgnoreCase))
                        {
                            header.Sender = party;
                        }
                        else if (partyType.Equals(ReceiverText, StringComparison.OrdinalIgnoreCase))
                        {
                            header.AddReciever(party);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Builds the dataset unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <param name="extracted">The extracted.</param>
        /// <param name="sender">The sender.</param>
        /// <returns>The DataSet ID</returns>
        private static string BuildDatasetId(string id, DateTime extracted, string sender = null)
        {
            var args = extracted.ToString("yyyy-MM-ddTHHmmss", CultureInfo.InvariantCulture);
            return sender != null ? string.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}", id, sender, args) : string.Format(CultureInfo.InvariantCulture, "{0}_{1}", id, args);
        }

        /// <summary>
        /// This method queries the mapping store for header information for a specific dataflow
        /// </summary>
        /// <param name="beginDate">For ReportingBegin element</param>
        /// <param name="endDate">For ReportingEnd element</param>
        /// <param name="dataflowReference">The dataflow reference.</param>
        /// <returns>
        /// A <see cref="IHeader" /> object. Otherwise null
        /// </returns>
        private IHeader GetHeader(DateTime? beginDate, DateTime? endDate, IMaintainableRefObject dataflowReference)
        {
            long headerSysId;
            string paramId = this._mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter);
            string version1 = this._mappingStoreDb.BuildParameterName(ParameterNameConstants.VersionParameter1);
            string version2 = this._mappingStoreDb.BuildParameterName(ParameterNameConstants.VersionParameter2);
            string version3 = this._mappingStoreDb.BuildParameterName(ParameterNameConstants.VersionParameter3);
            string agency = this._mappingStoreDb.BuildParameterName(ParameterNameConstants.AgencyParameter);

            var sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT HD.HEADER_ID, HD.TEST, HD.DATASET_AGENCY, HD.DF_ID ");
            sqlCommand.Append("FROM HEADER HD, DATAFLOW DF, ARTEFACT ART ");
            sqlCommand.Append("WHERE HD.DF_ID = DF.DF_ID ");
            sqlCommand.Append("AND DF.DF_ID = ART.ART_ID ");
            sqlCommand.AppendFormat("AND ART.ID = {0} ", paramId);
            sqlCommand.AppendFormat("AND dbo.isEqualVersion(ART.VERSION1,ART.VERSION2,ART.VERSION3,{0},{1},{2})=1 ", version1, version2, version3);
            sqlCommand.AppendFormat("AND ART.AGENCY = {0} ", agency);

            IDictionary<string, string> additionalAttributes = new Dictionary<string, string>(StringComparer.Ordinal);
            bool test;
            DateTime currentDate = DateTime.Now;
            var dataflowId = dataflowReference.MaintainableId;
            var dataflowAgency = dataflowReference.AgencyId;
            var version = dataflowReference.SplitVersion(3);
            using (DbCommand command = this._mappingStoreDb.GetSqlStringCommand(sqlCommand.ToString()))
            {
                this._mappingStoreDb.AddInParameter(command, ParameterNameConstants.IdParameter, DbType.String, dataflowId);
                this._mappingStoreDb.AddInParameter(command, ParameterNameConstants.VersionParameter1, DbType.Int64, version[0].HasValue ? version[0].Value : 0);
                this._mappingStoreDb.AddInParameter(command, ParameterNameConstants.VersionParameter2, DbType.Int64, version[1].HasValue ? version[1].Value : 0);
                this._mappingStoreDb.AddInParameter(command, ParameterNameConstants.VersionParameter3, DbType.Int64, version[2].HasValue ? (object)version[2].Value : null);
                this._mappingStoreDb.AddInParameter(command, ParameterNameConstants.AgencyParameter, DbType.String, dataflowAgency);

                using (IDataReader dataReader = this._mappingStoreDb.ExecuteReader(command))
                {
                    // we expect only 1 record here
                    if (dataReader.Read())
                    {
                        headerSysId = DataReaderHelper.GetInt64(dataReader, "HEADER_ID");
                        test = DataReaderHelper.GetBoolean(dataReader, "TEST");
                        additionalAttributes.Add("DataSetAgency", DataReaderHelper.GetString(dataReader, "DATASET_AGENCY"));
                        _log.DebugFormat(CultureInfo.InvariantCulture, "Found header information in mapping store for Dataflow {0}", dataflowId);
                    }
                    else
                    {
                        _log.DebugFormat(CultureInfo.InvariantCulture, "No header information found in mapping store for Dataflow {0}", dataflowId);
                        return null;
                    }
                }
            }

            string datasetId = BuildDatasetId(dataflowId, currentDate);

            // DatasetAction: Information (case that is response to a query)
            DatasetAction datasetAction = DatasetAction.GetAction("Information");

            IHeader ret = new HeaderImpl(additionalAttributes, null, null, datasetAction, dataflowId, datasetId, null, currentDate, currentDate, beginDate, endDate, null, null, null, null, test);

            PopulateHeaderLocalisedStrings(this._mappingStoreDb, headerSysId, ret);
            PoulateHeaderSendersAndReceivers(this._mappingStoreDb, headerSysId, ret);
            if (ret.Sender != null)
            {
                DateTime extracted = ret.Extracted.HasValue ? ret.Extracted.Value : currentDate;

                ret.DatasetId = BuildDatasetId(ret.Id, extracted, ret.Sender.Id);
            }

            return ret;
        }

        #endregion
    }
}