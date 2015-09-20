// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AvailableDataRetriever.cs" company="Eurostat">
//   Date Created : 2011-06-06
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Description of AvailableDataRetriever.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Text;

    using Estat.Nsi.Logger;
    using Estat.Nsi.MappingStore;
    using Estat.Nsi.MappingStore.Config;
    using Estat.Nsi.MappingStore.Mapping;
    using Estat.Nsi.MappingStore.MappingObjectModel;
    using Estat.Nsi.StructureRetriever.Model;
    using Estat.Sdmx.Model.Base;
    using Estat.Sdmx.Model.Query;
    using Estat.Sdmx.Model.Registry;
    using Estat.Sdmx.Model.Structure;

    using Microsoft.Practices.EnterpriseLibrary.Data;

    using CodelistRefBean = Estat.Sdmx.Model.Registry.CodelistRefBean;
    using DataflowRefBean = Estat.Sdmx.Model.Structure.DataflowRefBean;

    /// <summary>
    /// Description of AvailableDataRetriever.
    /// </summary>
    internal class AvailableDataRetriever
    {
        #region Constants and Fields

        /// <summary>
        /// Get or Set the collection of allowed dataflows
        /// </summary>
        private readonly ICollection<DataflowRefBean> _allowedDataflows;

        /// <summary>
        /// The MADB connection settings
        /// </summary>
        private readonly ConnectionStringSettings _connectionStringSettings;

        /// <summary>
        /// This field holds the dataflowRef constrains
        /// </summary>
        private readonly List<MemberBean> _criteria = new List<MemberBean>();

        /// <summary>
        /// The logger object to be used
        /// </summary>
        private readonly Log _logger;

        /// <summary>
        /// The list of XS measures at the constraint or all XS measures.
        /// </summary>
        private readonly Dictionary<string, ComponentEntity> _xsMeasureDimensionConstraints =
            new Dictionary<string, ComponentEntity>();

        /// <summary>
        /// This field holds the mapping between the component name and the ComponentInfo
        /// </summary>
        private Dictionary<string, ComponentInfo> _componentMapping = new Dictionary<string, ComponentInfo>();

        /// <summary>
        /// The dataset SQL QUery
        /// </summary>
        private string _innerSqlQuery;

        /// <summary>
        /// This field holds the mappingSet of the dataflow specified in the constructor
        /// </summary>
        private MappingSetEntity _mappingSet;

        /// <summary>
        /// The Structure access object used to get structural metadata from Mapping Store
        /// </summary>
        private StructureAccess _mastoreAccess;

        /// <summary>
        /// This field holds the measure component name in case it is not mapped. Otherwise it is null.
        /// </summary>
        private string _measureComponent;

        /// <summary>
        /// This field holds the ReferencePeriod
        /// </summary>
        private ReferencePeriodBean _referencePeriod;

        /// <summary>
        /// The field contains the requested component
        /// </summary>
        private string _requestedComponent;

        /// <summary>
        /// This field holds the name of the TimeDimension
        /// </summary>
        private string _timeDimension;

        /// <summary>
        /// This field holds the mapping used by the time dimension
        /// </summary>
        private MappingEntity _timeMapping;

        /// <summary>
        /// This field holds the transcoder used by the time dimension
        /// </summary>
        private ITimeDimension _timeTranscoder;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the AvailableDataRetriever class
        /// </summary>
        /// <param name="dataflow">
        /// The datflow to get the available data for
        /// </param>
        /// <param name="connectionStringSettings">
        /// The Mapping Store connection string settings
        /// </param>
        /// <param name="logger">
        /// The logger to log information, warnings and errors. It can be null
        /// </param>
        /// <param name="allowedDataflows">
        /// The collection of allowed dataflows
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// connectionStringSettings is null
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// dataflow is null
        /// </exception>
        /// <exception cref="StructureRetrieverException">
        /// Parsing error or mapping store exception error
        /// </exception>
        public AvailableDataRetriever(
            Sdmx.Model.Registry.DataflowRefBean dataflow, 
            ConnectionStringSettings connectionStringSettings, 
            Log logger, 
            ICollection<DataflowRefBean> allowedDataflows)
        {
            if (connectionStringSettings == null)
            {
                throw new ArgumentNullException("connectionStringSettings");
            }

            if (dataflow == null)
            {
                throw new ArgumentNullException("dataflow");
            }

            this._connectionStringSettings = connectionStringSettings;
            this._allowedDataflows = allowedDataflows;
            this._logger = logger ?? new Log();

            this.ParserDataflowRef(dataflow);
            try
            {
                this.Initialize(dataflow);
            }
            catch (StructureRetrieverException)
            {
                throw;
            }
            catch (DbException e)
            {
                string mesage = "Mapping Store connection error." + e.Message;
                this._logger.LogError(mesage);
                this._logger.LogError(e.ToString());
                throw new StructureRetrieverException(
                    StructureRetrieverErrorTypes.MappingStoreConnectionError, mesage, e);
            }
            catch (Exception e)
            {
                string mesage = string.Format(
                    CultureInfo.CurrentCulture, 
                    ErrorMessages.ErrorRetrievingMappingSetFormat4, 
                    dataflow.AgencyID, 
                    dataflow.Id, 
                    dataflow.Version, 
                    e.Message);
                this._logger.LogError(mesage);
                this._logger.LogError(e.ToString());
                throw new StructureRetrieverException(
                    StructureRetrieverErrorTypes.MappingStoreConnectionError, mesage, e);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieve the codelist that is referenced by the given codelistRef. The codelist
        /// </summary>
        /// <param name="codelistRef">
        /// The codelist reference containing the id, agency and version of the requested dimension. Can be empty for time dimension
        /// </param>
        /// <returns>
        /// The partial codelist
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// codelist is null
        /// </exception>
        public CodeListBean RetrieveAvailableData(CodelistRefBean codelistRef)
        {
            if (codelistRef == null)
            {
                throw new ArgumentNullException("codelistRef");
            }

            if (this._mappingSet == null)
            {
                return null;
            }

            if (!this.IsRequestedComponentCodelist(codelistRef))
            {
                if (CustomCodelistConstants.IsCountRequest(codelistRef))
                {
                    // COUNT data request
                    return this.GetCountCodelist(); // get the count special codelist
                }

                return null;
            }

            if (this._requestedComponent.Equals(this._measureComponent))
            {
                // measure dimension codelist request if measure dimension is not mapped
                return this.GetCodeList(codelistRef.Id, codelistRef.Version, codelistRef.AgencyID);

                // get the entire codelist for measure dimension
            }

            if (this._requestedComponent.Equals(this._timeDimension))
            {
                // time dimension codelist request
                return this.GetTimeDimensionStartEnd();

                // get the time dimension special codelist with start and possibly end codes.
            }

            return this.GetPartialCodelist(codelistRef); // partial code list request for mapped coded components
        }

        #endregion

        #region Methods

        /// <summary>
        /// Normallizes the DDB provider names so both DDB and MASTORE use the same.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection to the mapping store
        /// </param>
        private static void NormallizeDatabaseProvider(ConnectionStringSettings connectionStringSettings)
        {
            if (connectionStringSettings.ProviderName.Contains("Oracle"))
            {
                DatabaseType.Mappings[MappingStoreDefaultConstants.OracleName].Provider =
                    connectionStringSettings.ProviderName;
            }
        }

        /// <summary>
        /// Setup a CodelistBean object as a TimeDimension codelist containing two codes, startTime and endTime and using as codelist id the <see cref="CustomCodelistConstants.TimePeriodCodeList"/>.
        /// </summary>
        /// <param name="startTime">
        /// The code that will contain the start Time period
        /// </param>
        /// <param name="endTime">
        /// The code that will contain the end Time period
        /// </param>
        /// <param name="timeCodeList">
        /// The codelist to setup
        /// </param>
        private static void SetupTimeCodelist(CodeBean startTime, CodeBean endTime, CodeListBean timeCodeList)
        {
            timeCodeList.Id = CustomCodelistConstants.TimePeriodCodeList;
            timeCodeList.AgencyId = CustomCodelistConstants.Agency;
            timeCodeList.Version = CustomCodelistConstants.Version;
            timeCodeList.Names.Add(
                new TextTypeBean(CustomCodelistConstants.TimePeriodCodeListName, CustomCodelistConstants.Lang));
            startTime.Descriptions.Add(new TextTypeBean(CustomCodelistConstants.TimePeriodStartDescription, CustomCodelistConstants.Lang));
            timeCodeList.AddItem(startTime);

            if (endTime != null)
            {
                endTime.Descriptions.Add(new TextTypeBean(CustomCodelistConstants.TimePeriodEndDescription, CustomCodelistConstants.Lang));

                timeCodeList.AddItem(endTime);
            }
        }

        /// <summary>
        /// setup <see cref="DbCommand"/>. Currently it disables <see cref="DbCommand.CommandTimeout"/>
        /// </summary>
        /// <param name="cmd">
        /// The <see cref="DbCommand"/>
        /// </param>
        private static void SetupUpCmd(IDbCommand cmd)
        {
            cmd.CommandTimeout = 0; // never timeout
        }

        /// <summary>
        /// Create and return the DDB Connection from the <see cref="_mappingSet"/> field
        /// </summary>
        /// <returns>
        /// The DDB connection
        /// </returns>
        private DbConnection CreateDdbConnection()
        {
            // create ddb connection
            string dissdbConnectionString = this._mappingSet.DataSet.Connection.AdoConnectionString;
            string providerName = DatabaseType.GetProviderName(this._mappingSet.DataSet.Connection.DBType);

            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(providerName);

            // for PC-axis it doesn't support DbFactory.CreateCommand() and the enteprise libs seem to call it
            var disseminationDb = new GenericDatabase(dissdbConnectionString, dbFactory);
            DbConnection ddbConnection = disseminationDb.CreateConnection();
            ddbConnection.Open();
            return ddbConnection;
        }

        /// <summary>
        /// Execute the query generated from <see cref="GenerateSql"/>  against the DDB and populate the given set of available codes.
        /// </summary>
        /// <param name="component">
        /// The component to get the codes for
        /// </param>
        /// <param name="codeSet">
        /// The Set to populate with the DDB available codes
        /// </param>
        /// <param name="sqlQuery">
        /// The SQL Query to execute
        /// </param>
        /// <exception cref="DbException">
        /// DDB communication error
        /// </exception>
        private void ExecuteSql(ComponentInfo component, IDictionary<string, object> codeSet, string sqlQuery)
        {
            IComponentMapping cmap = component.ComponentMapping;
            using (DbConnection ddbConnection = this.CreateDdbConnection())
            {
                using (DbCommand cmd = ddbConnection.CreateCommand())
                {
                    cmd.CommandText = sqlQuery;
                    SetupUpCmd(cmd);
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string dsdCode = cmap.MapComponent(reader);
                            if (dsdCode != null && !codeSet.ContainsKey(dsdCode))
                            {
                                codeSet.Add(dsdCode, null);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Execute the query generated from <see cref="GenerateSql"/>  against the DDB and get a single value
        /// </summary>
        /// <param name="sqlQuery">
        /// The SQL Query to execute
        /// </param>
        /// <returns>
        /// The scalar value
        /// </returns>
        /// <exception cref="DbException">
        /// DDB communication error
        /// </exception>
        private object ExecuteSql(string sqlQuery)
        {
            object value;
            using (DbConnection ddbConnection = this.CreateDdbConnection())
            {
                using (DbCommand cmd = ddbConnection.CreateCommand())
                {
                    cmd.CommandText = sqlQuery;
                    SetupUpCmd(cmd);
                    value = cmd.ExecuteScalar();
                }
            }

            return value;
        }

        /// <summary>
        /// Execute the query generated from <see cref="GenerateSql"/>  against the DDB and populate the given set of available codes.
        /// </summary>
        /// <param name="timeTranscoding">
        /// The ITimeDimension implementation to use for mapping and transcoding
        /// </param>
        /// <param name="timeCodeList">
        /// The Codelist to populate with the min and max values
        /// </param>
        /// <param name="sqlQuery">
        /// The SQL Query to execute
        /// </param>
        /// <exception cref="DbException">
        /// DDB communication error
        /// </exception>
        private void ExecuteSql(ITimeDimension timeTranscoding, CodeListBean timeCodeList, string sqlQuery)
        {
            var max = new CodeBean();
            var min = new CodeBean();
            max.Id = CustomCodelistConstants.TimePeriodMin;
            min.Id = CustomCodelistConstants.TimePeriodMax;

            using (DbConnection ddbConnection = this.CreateDdbConnection())
            {
                using (DbCommand cmd = ddbConnection.CreateCommand())
                {
                    SetupUpCmd(cmd);
                    cmd.CommandText = sqlQuery;
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string period = timeTranscoding.MapComponent(reader);
                            if (!string.IsNullOrEmpty(period))
                            {
                                if (string.CompareOrdinal(period, min.Id) < 0)
                                {
                                    min.Id = period;
                                }

                                if (string.CompareOrdinal(period, max.Id) > 0)
                                {
                                    max.Id = period;
                                }
                            }
                        }
                    }
                }
            }

            if (min.Id.Equals(max.Id))
            {
                max = null;
            }

            SetupTimeCodelist(min, max, timeCodeList);

            // TODO discuss this
            // if (min.Id.Equals("9999")) {
            // timeCodeList.Items.Clear();
            // }
        }

        /// <summary>
        /// Generate a SQL SELECT count(*) using the specified sqlWhere query
        /// </summary>
        /// <param name="sqlWhere">
        /// The SQL Query WHERE keyword and where clauses or empty
        /// </param>
        /// <returns>
        /// String with the SQL Query
        /// </returns>
        private string GenerateCountSql(string sqlWhere)
        {
            return string.Format(
                CultureInfo.InvariantCulture, 
                "SELECT COUNT(*) \n FROM ({0}) virtualDataset {1}", 
                this._innerSqlQuery, 
                sqlWhere);
        }

        /// <summary>
        /// Generate a SQL SELECT distinct for the specific mapping using the Dataset query
        /// </summary>
        /// <param name="mapping">
        /// The mapping to generate the SQL for
        /// </param>
        /// <param name="sqlWhere">
        /// The SQL Query WHERE keyword and where clauses or empty
        /// </param>
        /// <returns>
        /// String with the SQL Query
        /// </returns>
        private string GenerateSql(MappingEntity mapping, string sqlWhere)
        {
            var columnList = new StringBuilder();
            foreach (DataSetColumnEntity col in mapping.Columns)
            {
                columnList.AppendFormat(" {0},", col.Name);
            }

            columnList.Length = columnList.Length - 1;
            return string.Format(
                CultureInfo.InvariantCulture, 
                "SELECT DISTINCT {2} \n FROM ({0}) virtualDataset {1} \n ORDER BY {2} ", 
                this._innerSqlQuery, 
                sqlWhere, 
                columnList);
        }

        /// <summary>
        /// This method generates the WHERE part of the SQL query that will be used against the DDB for retrieving the available codes
        /// </summary>
        /// <param name="criteria">
        /// The list of criteria that will be used to generate the where clauses
        /// </param>
        /// <returns>
        /// A string containing the WHERE part of the SQL Query or an Empty string
        /// </returns>
        private string GenerateWhere(IEnumerable<MemberBean> criteria)
        {
            var sb = new StringBuilder();
            int lastClause = 0;
            TimeBean time;

            foreach (MemberBean member in criteria)
            {
                if (!string.IsNullOrEmpty(member.ComponentRef))
                {
                    if (member.ComponentRef.Equals(this._timeDimension))
                    {
                        // TODO FIXME not sure how time constrains will be included
                        time = new TimeBean();
                        if (member.MemberValue.Count > 0)
                        {
                            time.StartTime = member.MemberValue[0].Value;
                            if (member.MemberValue.Count > 1)
                            {
                                time.EndTime = member.MemberValue[1].Value;
                            }
                        }

                        sb.Append("(");
                        sb.Append(this._timeTranscoder.GenerateWhere(time));
                        sb.Append(")");
                        lastClause = sb.Length;
                        sb.Append(" AND ");
                    }
                    else
                    {
                        ComponentInfo compInfo;
                        if (this._componentMapping.TryGetValue(member.ComponentRef, out compInfo))
                        {
                            sb.Append("(");
                            foreach (MemberValueBean value in member.MemberValue)
                            {
                                sb.Append(compInfo.ComponentMapping.GenerateComponentWhere(value.Value));
                                lastClause = sb.Length;
                                sb.Append(" OR ");
                            }

                            sb.Length = lastClause;
                            sb.Append(")");
                            lastClause = sb.Length;
                            sb.Append(" AND ");
                        }
                    }
                }
            }

            if (this._referencePeriod != null)
            {
                // TODO FIXME not sure how time constrains will be included
                IFormatProvider fmt = CultureInfo.InvariantCulture;
                time = new TimeBean();
                time.StartTime = this._referencePeriod.StartTime.ToString("yyyy-MM-dd", fmt);
                time.EndTime = this._referencePeriod.EndTime.ToString("yyyy-MM-dd", fmt);
                sb.Append("(");
                sb.Append(this._timeTranscoder.GenerateWhere(time));
                sb.Append(")");
                lastClause = sb.Length;
            }

            sb.Length = lastClause;
            if (sb.Length > 0)
            {
                return " where " + sb;
            }

            return string.Empty;
        }

        /// <summary>
        /// Get the code list from Mapping Store
        /// </summary>
        /// <param name="id">
        /// The codelist id
        /// </param>
        /// <param name="version">
        /// The codelist version
        /// </param>
        /// <param name="agency">
        /// The codelist agency
        /// </param>
        /// <param name="subset">
        /// The subset of code values to return
        /// </param>
        /// <returns>
        /// The codelist or null
        /// </returns>
        private CodeListBean GetCodeList(string id, string version, string agency, IList<string> subset)
        {
            CodeListBean codelist = null;
            IList<CodeListBean> codelists = this._mastoreAccess.GetCodeList(id, version, agency, subset);
            if (codelists.Count > 0)
            {
                codelist = codelists[0];
            }

            return codelist;
        }

        /// <summary>
        /// Get the code list from Mapping Store
        /// </summary>
        /// <param name="id">
        /// The codelist id
        /// </param>
        /// <param name="version">
        /// The codelist version
        /// </param>
        /// <param name="agency">
        /// The codelist agency
        /// </param>
        /// <returns>
        /// The codelist or null
        /// </returns>
        private CodeListBean GetCodeList(string id, string version, string agency)
        {
            CodeListBean codelist = null;
            IList<CodeListBean> codelists = this._mastoreAccess.GetCodeList(id, version, agency);
            if (codelists.Count > 0)
            {
                codelist = codelists[0];
            }

            return codelist;
        }

        /// <summary>
        /// Getter for the special data count request
        /// </summary>
        /// <returns>
        /// A codelist with one code, the number of data
        /// </returns>
        private CodeListBean GetCountCodelist()
        {
            var codelist = new CodeListBean();
            var name = new TextTypeBean(CustomCodelistConstants.CountCodeListName, CustomCodelistConstants.Lang);
            codelist.Names.Add(name);
            codelist.Id = CustomCodelistConstants.CountCodeList;
            codelist.AgencyId = CustomCodelistConstants.Agency;
            codelist.Version = CustomCodelistConstants.Version;
            this._logger.LogInformation("|-- Generating SQL WHERE for dissemination database...");
            string sqlWhere = this.GenerateWhere(this._criteria);
            this._logger.LogInformation("|-- Generating SQL for dissemination database...");
            string sql = this.GenerateCountSql(sqlWhere);
            this._logger.LogInformation("|-- SQL for dissemination database generated:\n" + sql);
            int xsMeasureMult = 1;
            if (this._measureComponent != null)
            {
                this._logger.LogInformation("|-- Get XS Measure count");
                xsMeasureMult = this.GetXsMeasureCount(this._criteria);
            }

            object value = this.ExecuteSql(sql);

            // setup count codelist
            var countCode = new CodeBean();
            var text = new TextTypeBean(CustomCodelistConstants.CountCodeDescription, CustomCodelistConstants.Lang);
            countCode.Descriptions.Add(text);

            // normally count(*) should always return a number. Checking just in case I missed something.
            if (value != null && !Convert.IsDBNull(value))
            {
                long count = Convert.ToInt64(value, CultureInfo.InvariantCulture);

                // in .net, oracle will return 128bit decimal, sql server 32bit int, while mysql & sqlite 64bit long.
                // check if there are XS measure mappings. In this case there could be multiple measures/obs per row. 
                // even if they are not, then will be static mappings
                count *= xsMeasureMult;

                countCode.Id = count.ToString(CultureInfo.InvariantCulture);
                codelist.AddItem(countCode);
            }
            else
            {
                countCode.Id = CustomCodelistConstants.CountCodeDefault;
            }

            return codelist;
        }

        /// <summary>
        /// Get partial codelist from a coded non-measure dimension
        /// </summary>
        /// <param name="codelistRef">
        /// The codelist reference containing the id, agency and version of the requested dimension.
        /// </param>
        /// <returns>
        /// The partial codelist
        /// </returns>
        private CodeListBean GetPartialCodelist(CodelistRefBean codelistRef)
        {
            ComponentInfo component;
            if (!this._componentMapping.TryGetValue(this._requestedComponent, out component))
            {
                return null;
            }

            ICollection<DataSetColumnEntity> dataSetColumns = component.Mapping.Columns;
            var codesSet = new Dictionary<string, object>();
            CodeListBean codelist = null;

            if (dataSetColumns != null && dataSetColumns.Count > 0)
            {
                // mapped against dataset column
                this._logger.LogInformation("|-- Generating SQL WHERE for dissemination database...");
                string sqlWhere = this.GenerateWhere(this._criteria);
                this._logger.LogInformation("|-- WHERE :..." + sqlWhere);
                this._logger.LogInformation("|-- Generating SQL for dissemination database...");
                string sql = this.GenerateSql(component.Mapping, sqlWhere);
                this._logger.LogInformation("|-- SQL for dissemination database generated:\n" + sql);
                this.ExecuteSql(component, codesSet, sql);
                this._logger.LogInformation("|-- Query executed successfully, found : " + codesSet.Count + " codes");
            }
            else
            {
                // constant time mapping
                codesSet.Add(component.Mapping.Constant, null);
            }

            if (codesSet.Count > 0)
            {
                var subset = new List<string>(codesSet.Keys);
                codelist = this.GetCodeList(codelistRef.Id, codelistRef.Version, codelistRef.AgencyID, subset);
            }

            if (codelist != null && codelist.Items.Count == 0)
            {
                string error = string.Format(
                    CultureInfo.CurrentCulture, 
                    ErrorMessages.ZeroCodesFoundFormat3, 
                    this._mappingSet.Id, 
                    this._requestedComponent, 
                    this._mappingSet.Dataflow.Id);
                this._logger.LogWarning(error);
                throw new StructureRetrieverException(StructureRetrieverErrorTypes.MissingStructure, error);
            }

            return codelist;
        }

        /// <summary>
        /// Getter for time dimension custom codelist. The codelist will contain two codes, one with the start period and the other with end period
        /// </summary>
        /// <returns>
        /// The time dimension custom codelist
        /// </returns>
        private CodeListBean GetTimeDimensionStartEnd()
        {
            ICollection<DataSetColumnEntity> dataSetColumns = this._timeMapping.Columns;
            var codelist = new CodeListBean();
            if (dataSetColumns != null && dataSetColumns.Count > 0)
            {
                this._logger.LogInformation("|-- Generating SQL WHERE for dissemination database...");
                string sqlWhere = this.GenerateWhere(this._criteria);
                this._logger.LogInformation("|-- Generating SQL for dissemination database...");
                string sql = this.GenerateSql(this._timeMapping, sqlWhere);
                this._logger.LogInformation("|-- SQL for dissemination database generated:\n" + sql);

                this.ExecuteSql(this._timeTranscoder, codelist, sql);
            }
            else
            {
                var min = new CodeBean();
                min.Id = this._timeMapping.Constant;
                SetupTimeCodelist(min, null, codelist);
            }

            return codelist;
        }

        /// <summary>
        /// Get CrossSectional Measure count.
        /// </summary>
        /// <param name="criteria">
        /// The criteria.
        /// </param>
        /// <returns>
        /// The CrossSectional Measure count.
        /// </returns>
        private int GetXsMeasureCount(IEnumerable<MemberBean> criteria)
        {
            int xsMeasureCount = this._xsMeasureDimensionConstraints.Count;
            foreach (MemberBean member in criteria)
            {
                if (member.ComponentRef.Equals(this._measureComponent))
                {
                    // get the unmapped measure dimension XS codes to display
                    xsMeasureCount = 0;
                    foreach (MemberValueBean value in member.MemberValue)
                    {
                        if (value != null && value.Value != null
                            && this._xsMeasureDimensionConstraints.ContainsKey(value.Value))
                        {
                            xsMeasureCount++;
                        }
                    }
                }
            }

            return xsMeasureCount == 0 ? 1 : xsMeasureCount;
        }

        /// <summary>
        /// Initialize component mappings for coded components and time dimension used in mappings in the dataflow
        /// </summary>
        /// <param name="dataflowRef">
        /// The dataflow Ref.
        /// </param>
        /// <remarks>
        /// This method should be called only once
        /// </remarks>
        private void Initialize(Sdmx.Model.Registry.DataflowRefBean dataflowRef)
        {
            this._mappingSet = DataAccess.GetMappingSet(
                this._connectionStringSettings, 
                dataflowRef.Id, 
                dataflowRef.Version, 
                dataflowRef.AgencyID, 
                this._allowedDataflows);
            if (this._mappingSet == null)
            {
                return;
            }

            NormallizeDatabaseProvider(this._connectionStringSettings);
            this._mastoreAccess = new StructureAccess(this._connectionStringSettings);
            this._xsMeasureDimensionConstraints.Clear();

            this._componentMapping = new Dictionary<string, ComponentInfo>(this._mappingSet.Mappings.Count);
            this._innerSqlQuery = this._mappingSet.DataSet.Query;
            this._measureComponent = null;
            this._timeTranscoder = null;
            this._timeMapping = null;
            this._timeDimension = null;
            bool measureDimensionMapped = false;

            foreach (MappingEntity mapping in this._mappingSet.Mappings)
            {
                foreach (ComponentEntity component in mapping.Components)
                {
                    if (component.ComponentType == SdmxComponentType.TimeDimension)
                    {
                        this._timeTranscoder = TimeDimensionMapping.Create(
                            mapping, this._mappingSet.DataSet.Connection.DBType);
                        this._timeMapping = mapping;
                        this._timeDimension = component.Concept.Id;
                    }
                    else if (component.CodeList != null)
                    {
                        if (component.MeasureDimension)
                        {
                            measureDimensionMapped = true;
                        }

                        var compInfo = new ComponentInfo();
                        compInfo.Mapping = mapping;
                        compInfo.ComponentMapping = ComponentMapping.CreateComponentMapping(component, mapping);
                        compInfo.CodelistRef.Id = component.CodeList.Id;
                        compInfo.CodelistRef.Version = component.CodeList.Version;
                        compInfo.CodelistRef.AgencyID = component.CodeList.Agency;
                        this._componentMapping.Add(component.Concept.Id, compInfo);
                    }
                }
            }

            if (!measureDimensionMapped)
            {
                foreach (ComponentEntity component in this._mappingSet.Dataflow.Dsd.Dimensions)
                {
                    if (component.MeasureDimension)
                    {
                        this._measureComponent = component.Concept.Id;
                    }
                }

                foreach (ComponentEntity xsMeasure in this._mappingSet.Dataflow.Dsd.CrossSectionalMeasures)
                {
                    this._xsMeasureDimensionConstraints.Add(xsMeasure.Concept.Id, xsMeasure);
                }
            }
        }

        /// <summary>
        /// Check if the requested Component is set and if the requested codelist ref is for that component
        /// </summary>
        /// <param name="codelistRef">
        /// The requested codelistRef
        /// </param>
        /// <returns>
        /// The is requested component codelist.
        /// </returns>
        private bool IsRequestedComponentCodelist(CodelistRefBean codelistRef)
        {
            if (this._requestedComponent == null)
            {
                return false;
            }

            if (this._requestedComponent.Equals(this._timeDimension)
                && (string.IsNullOrEmpty(codelistRef.Id) || CustomCodelistConstants.IsTimeDimensionRequest(codelistRef)))
            {
                return true;
            }

            if (this._measureComponent != null && this._measureComponent.Equals(this._requestedComponent))
            {
                return true;
            }

            ComponentInfo info;
            if (this._componentMapping.TryGetValue(this._requestedComponent, out info))
            {
                CodelistRefBean c = info.CodelistRef;
                return string.Equals(c.Id, codelistRef.Id) && string.Equals(c.AgencyID, codelistRef.AgencyID);
            }

            return false;
        }

        /// <summary>
        /// Parse the specified DataflowRefBean object and populate the <see cref="_requestedComponent"/> and <see cref="_criteria"/> fields
        /// </summary>
        /// <param name="d">
        /// The DataflowRefBean to parse
        /// </param>
        private void ParserDataflowRef(Sdmx.Model.Registry.DataflowRefBean d)
        {
            if (d.Constraint != null)
            {
                foreach (CubeRegionBean cube in d.Constraint.CubeRegion)
                {
                    foreach (MemberBean member in cube.Members)
                    {
                        if (member.MemberValue.Count == 0)
                        {
                            this._requestedComponent = member.ComponentRef;
                        }
                        else
                        {
                            this._criteria.Add(member);
                        }
                    }
                }

                this._referencePeriod = d.Constraint.ReferencePeriod;
            }
        }

        #endregion
    }
}