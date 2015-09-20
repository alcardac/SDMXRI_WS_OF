// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataRetrieverCore.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This is the main class of the "Data Retriever" building block as identified in Eurostat's "SDMX Reference Architecture". It can be used with any "Mapping Store" complying with the database design specified there.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RDFProvider.Retriever
{
    using System;
    using System.Configuration;
    using System.Data.Common;
    using System.Globalization;
    using RDFProvider.Retriever.Builders;
    using RDFProvider.Retriever.Engines;
    using RDFProvider.Retriever.Manager;
    using RDFProvider.Retriever.Model;
    using Estat.Nsi.DataRetriever.Properties;
    using Estat.Sri.MappingStoreRetrieval;
    using Estat.Sri.MappingStoreRetrieval.Config;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Engine;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;
    using log4net;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using RDFProvider.Writer.Engine;

    /// <summary>
    /// This is the main class of the "Data Retriever" building block as identified in Eurostat's "SDMX Reference Architecture". It can be used with any "Mapping Store" complying with the database design specified there.
    /// </summary>
    /// <example>
    /// Code example for using Data Retriever and the Stream CompactWriter from SDMX IO in C# 
    /// <code source="ReUsingExamples\DataRetriever\ReUsingDataRetriever.cs" lang="cs">
    /// </code>
    /// </example>
    public class RDFDataRetrieverCore : IRDFDataRetrievalWithWriter
    {
        #region Constants and Fields

        /// <summary>
        ///   The SqlBuilder instance. This is responsible for building/generating the various SQL queries for DDB for TimeSeries data
        /// </summary>
        private static readonly ISqlBuilder _sqlBuilder = SeriesSqlBuilder.Instance;

        /// <summary>
        ///   The <see cref="ConnectionStringSettings" /> for Mapping store
        /// </summary>
        private readonly ConnectionStringSettings _connectionStringSettings;

        /// <summary>
        ///   This <see cref="Estat.Sdmx.Model.Base.HeaderBean" /> stores the default header
        /// </summary>
        private readonly IHeader _defaultHeader;

        private static readonly ILog Logger = LogManager.GetLogger(typeof(RDFDataRetrieverCore));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRetrieverCore"/> class. Constructor for building a new DataRetriever object. It's function is to instantiate the <see cref="DataRetrieverCore._log"/>
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection to the "Mapping Store", from which the "Mapping Set" will be retrieved 
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// ConnectionStringSettings is null
        /// </exception>
        public RDFDataRetrieverCore(ConnectionStringSettings connectionStringSettings)
        {
            if (connectionStringSettings == null)
            {
                throw new ArgumentNullException("connectionStringSettings");
            }

            this._connectionStringSettings = connectionStringSettings;
            NormallizeDatabaseProvider(connectionStringSettings);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRetrieverCore"/> class. Constructor for building a new DataRetriever object.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// defaultHeader is null
        /// </exception>
        /// <param name="defaultHeader">
        /// The default header to use for dataflows without header 
        /// </param>
        /// <param name="connectionStringSettings">
        /// The connection to the "Mapping Store", from which the "Mapping Set" will be retrieved 
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// ConnectionStringSettings is null
        /// </exception>
        public RDFDataRetrieverCore(IHeader defaultHeader, ConnectionStringSettings connectionStringSettings)
            : this(connectionStringSettings)
        {
            if (defaultHeader != null)
            {
                this._defaultHeader = defaultHeader;
            }
            else
            {
                // throw new Exception();
                throw new ArgumentNullException("defaultHeader");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRetrieverCore" /> class. Constructor for building a new DataRetriever object.
        /// </summary>
        /// <param name="defaultHeader">The default header to use for dataflows without header</param>
        /// <param name="connectionStringSettings">The connection to the "Mapping Store", from which the "Mapping Set" will be retrieved</param>
        /// <param name="sdmxSchemaVersion">The SDMX schema version. 
        /// This controls the behavior of <see cref="ISdmxDataRetrievalWithWriter"/> when no results are found. If set to <see cref="SdmxSchemaEnumType.VersionTwoPointOne"/> DR will throw a <see cref="SdmxNoResultsException"/>. Otherwise a empty dataset will be written. This affects only <see cref="ISdmxDataRetrievalWithWriter"/> behavior. <see cref="IAdvancedSdmxDataRetrievalWithWriter"/> will always throw an exception. 
        /// While <see cref="ISdmxDataRetrievalWithCrossWriter"/> and <see cref="IDataRetrieverTabular"/> will never throw an exception for no results.</param>
        /// <exception cref="System.ArgumentNullException">connectionStringSettings is null</exception>
        /// <exception cref="System.ArgumentNullException">defaultHeader is null</exception>
        public RDFDataRetrieverCore(IHeader defaultHeader, ConnectionStringSettings connectionStringSettings, SdmxSchemaEnumType sdmxSchemaVersion)
            : this(defaultHeader, connectionStringSettings)
        {
        }

        #endregion

        #region Public Methods      

       

        /// <summary>
        /// This method generates the SQL SELECT statement for the dissemination database that will return the data for the incoming Query.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="query"/>
        ///   is null
        /// </exception>
        /// <exception cref="DataRetrieverException">
        /// <see cref="ErrorTypes"/>
        /// </exception>
        /// <param name="query">
        /// The <see cref="Estat.Sdmx.Model.Query.QueryBean"/> modeling an SDMX-ML Query 
        /// </param>
        /// <returns>
        /// The generated sql query. 
        /// </returns>
        /// <example>
        /// An example using this method in C#
        ///  <code source="ReUsingExamples\DataRetriever\ReUsingDataRetrieverManySteps.cs" lang="cs" />
        /// </example>
        public string GenerateSqlQuery(IDataQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            try
            {
                // Get mapping set
                MappingSetEntity mappingSet = this.Initialize(query);

                // build the data retrieval state
                var info = new DataRetrievalInfo(mappingSet, query, this._connectionStringSettings);

                // check if mapping set is complete. TODO Remove this when SRA-166 is implemented
                ValidateMappingSet(info);

                // return the sql query
                return info.SqlString;
            }
            catch (DataRetrieverException)
            {
                throw;
            }
            catch (SdmxException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataRetrieverException(ex,
                    SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.SemanticError),
                    Resources.DataRetriever_GenerateSqlQuery_Could_not_generate_sql_query);
            }
        }

        
        /// <summary>
        /// Retrieve data from a DDB and write it to the specified <paramref name="writer"/> This is the main public method of the DataRetriever class. It is called with a populated QueryBean (containing essentially an SDMX-ML Query) and a database Connection to a "Mapping Store" database. This method is responsible for: 
        /// <list type="bullet">
        /// <item>
        /// Retrieving the <see cref="MappingSetEntity"/> (the class containing the performed mappings), according to the provided Dataflow ID, from the "Mapping Store". Mapping Sets are defined on a Dataflow basis. Thus, this method checks the input QueryBean for the Dataflow that data are requested and fetches the appropriate
        /// <see cref="MappingSetEntity"/>. If no <see cref="MappingSetEntity"/> exists, an exception (<see cref="DataRetrieverException"/>) is thrown.
        /// </item>
        /// <item>
        /// Calling the method generating the appropriate SQL for the dissemination database.
        /// </item>
        /// <item>
        /// Calling the method that executes the generated SQL and uses the
        ///  <paramref name="writer"/>
        ///  to write the output.
        /// </item>
        /// </list>
        /// <note type="note">
        /// The "Data Retriever" expects exactly one Dataflow clause under the DataWhere clause, exactly one
        ///        DataFlowBean within the DataWhereBean (which in turn resides inside the incoming QueryBean).
        /// </note>
        /// </summary>
        /// <exception cref="DataRetrieverException">
        /// See the
        ///   <see cref="ErrorTypes"/>
        ///   for more details
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="query"/>
        ///   is null
        ///   -or-
        ///   <paramref name="writer"/>
        ///   is null
        /// </exception>
        /// <param name="query">
        /// The query bean for which data will be requested 
        /// </param>
        /// <param name="writer">
        /// The <see cref="ISeriesWriter"/> (e.g. Compact, Generic) writer 
        /// </param>
        /// <example>
        /// An example using this method in C# with <see cref="CompactWriter"/> 
        /// <code source="ReUsingExamples\DataRetriever\ReUsingDataRetriever.cs" lang="cs">
        /// </code>
        /// An example using this method in C# with <see cref="GenericDataWriter"/> 
        /// <code source="ReUsingExamples\DataRetriever\ReUsingDataRetrieverGeneric.cs" lang="cs">
        /// </code>
        /// </example>
        public void GetData(IDataQuery dataQuery, IRDFDataWriterEngine dataWriter)
        {

            if (dataQuery == null)
            {
                throw new ArgumentNullException("query");
            }

            if (dataWriter == null)
            {
                throw new ArgumentNullException("writer");
            }

            try
            {
                Logger.Info(Resources.InfoDataRetrieverBBInvoked);
                Logger.Info(Resources.InfoOutput + dataWriter.GetType().Name);

                // validate input and initialize the mappingset entitiy
                MappingSetEntity mappingSet = this.Initialize(dataQuery);

                var info = new DataRetrievalInfoSeries(mappingSet, dataQuery, this._connectionStringSettings, dataWriter)
                {
                    DefaultHeader = this._defaultHeader
                };
                
                ValidateMappingSet(info);

                //this.WriteHeader(dataWriter, info);

                //(SRA-345) 
                //DR the info from I*DataQuery. DimensionAtObservation to IDataWriterEngine.StartDataSet  
                IDatasetStructureReference dsr = new DatasetStructureReferenceCore("", dataQuery.DataStructure.AsReference, null, null, dataQuery.DimensionAtObservation);
                IDatasetHeader header = new DatasetHeaderCore(this._defaultHeader.DatasetId, this._defaultHeader.Action, dsr);

                dataWriter.StartDataset(dataQuery.Dataflow, dataQuery.DataStructure, header, info);                
                

                // Generate sql query
                this.GenerateSql(info, _sqlBuilder);

                this.GenerateSql(info, SeriesDataSetSqlBuilder.Instance);

                this.GenerateSql(info, SeriesGroupSqlBuilder.Instance);

                // execute sql query
                this.ExecuteSql(info, RDFSeriesQueryEngineManager.Instance.GetQueryEngine(info));

                // close output
                dataWriter.Close();
                Logger.Info(Resources.InfoEndDataRetrieverBBInvoked);
            }
            catch (DataRetrieverException)
            {
                throw;
            }
            catch (SdmxException)
            {
                throw;
            }
            catch (DbException ex)
            {
                Logger.Error(ex.ToString());
                throw new DataRetrieverException(ex,
                    SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.InternalServerError),
                    Resources.DataRetriever_ExecuteSqlQuery_Error_executing_generated_SQL_and_populating_SDMX_model);
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                throw new DataRetrieverException(ex,
                    SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.InternalServerError),
                    Resources.DataRetriever_ExecuteSqlQuery_Error_during_writing_responce);
            }
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
        /// Check if the <see cref="DataRetrievalInfo.MappingSet"/> from <paramref name="info"/> is complete
        /// </summary>
        /// <param name="info">
        /// The current data retrieval state 
        /// </param>
        /// <exception cref="DataRetrieverException">
        /// Incomplete mapping set. Please check if all dimensions, measure(s) and mandatory attributes are mapped
        /// </exception>
        private static void ValidateMappingSet(DataRetrievalInfo info)
        {
            // check if mapping is complete first
            if (!MappingUtils.IsMappingSetComplete(info.MappingSet.Dataflow.Dsd, info.ComponentMapping))
            {
                throw new IncompleteMappingSetException(Resources.ErrorIncompleteMappingSet);
            }
        }

        /// <summary>
        /// This method executes an SQL query on the dissemination database
        /// </summary>
        /// <param name="info">
        /// The current Data Retrieval state 
        /// </param>
        /// <param name="queryEngine">
        /// The query engine. 
        /// </param>
        private void ExecuteSql(DataRetrievalInfo info, IRDFDataQueryEngine queryEngine)
        {
            Logger.Info(Resources.InfoStartExecutingSql);
            queryEngine.ExecuteSqlQuery(info);
            Logger.Info(Resources.InfoEndExecutingSql);

            if (info.RecordsRead == 0 && (info.ComplexQuery != null))
            {
                throw new DataRetrieverException(Resources.NoRecordsFound,
                            SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.NoResultsFound));
            }
        }

        /// <summary>
        /// Generate the SQL string using <paramref name="sqlBuilder"/> with <paramref name="info"/>
        /// </summary>
        /// <param name="info">
        /// The current dataretrieval state. 
        /// </param>
        /// <param name="sqlBuilder">
        /// The sql builder. 
        /// </param>
        private void GenerateSql(DataRetrievalInfo info, ISqlBuilder sqlBuilder)
        {
            // Generate sql query
            Logger.Info(Resources.InfoStartGeneratingSQLDDB);
            sqlBuilder.GenerateSql(info);
            Logger.Info(Resources.InfoEndGeneratingSQLDDB);
        }

        /// <summary>
        /// This method initializes the Data Retriever. It performs some validation the Query, and retrieves from the Mapping Store the mapping set (if any) of the dataflow of the QueryBean 
        /// <note type="note">
        /// The
        ///                                                                                                                                                                                       <see cref="KeyFamilyBean"/>
        ///                                                                                                                                                                                       method should be called only after
        ///                                                                                                                                                                                       this method has been called.
        /// </note>
        /// </summary>
        /// <exception cref="DataRetrieverException">
        /// See the
        ///   <see cref="ErrorTypes"/>
        ///   for more details
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// ConnectionStringSettings is null
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// query is null
        /// </exception>
        /// <param name="query">
        /// The QueryBean containing the SDMX query 
        /// </param>
        /// <returns>
        /// The mapping set for the dataflow in <paramref name="query"/> 
        /// </returns>
        private MappingSetEntity Initialize(IDataQuery query)
        {
            Logger.Info(Resources.DataRetriever_RetrieveData_Start_Initializing_Data_Retriever);
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            // Get the dataflow from the query
            IDataflowObject dataFlow = DataRetrieverHelper.GetDataflowFromQuery(query);

            // get the mapping set and the keyfamilybean
            MappingSetEntity mappingSet = this.RetrieveMappingSet(dataFlow);

            // MAT-395
            if (mappingSet == null)
            {
                throw new DataRetrieverException(string.Format(CultureInfo.CurrentCulture, Resources.NoMappingForDataflowFormat1, dataFlow.Id),
                    SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.NoResultsFound));
                //ErrorTypes.NO_MAPPING_SET,
                //string.Format(CultureInfo.CurrentCulture, Resources.NoMappingForDataflowFormat1, dataFlow.Id));
            }

            mappingSet = FilterMappingSet(query, mappingSet);

            Logger.Info(Resources.DataRetriever_RetrieveData_End_Data_Retriever_initialization);
            return mappingSet;
        }

        private MappingSetEntity InitializeAdvanced(IComplexDataQuery query)
        {
            Logger.Info(Resources.DataRetriever_RetrieveData_Start_Initializing_Data_Retriever);
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            // Get the dataflow from the query
            IDataflowObject dataFlow = query.Dataflow;

            // get the mapping set and the keyfamilybean
            MappingSetEntity mappingSet = this.RetrieveMappingSet(dataFlow);

            // MAT-395
            if (mappingSet == null)
            {
                throw new DataRetrieverException(string.Format(CultureInfo.CurrentCulture, Resources.NoMappingForDataflowFormat1, dataFlow.Id),
                    SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.NoResultsFound));
            }

            //mappingSet = FilterMappingSet(query, mappingSet);

            Logger.Info(Resources.DataRetriever_RetrieveData_End_Data_Retriever_initialization);
            return mappingSet;
        }

        /// <summary>
        /// SRA-349 Filter data to be returned
        /// </summary>
        /// <param name="query">
        /// The IDataQuery containing the SDMX query 
        /// </param>
        /// <returns>
        /// The filtered mapping set entity for the dataflow in <paramref name="query"/> 
        /// </returns>
        private MappingSetEntity FilterMappingSet(IDataQuery query, MappingSetEntity mappingSet)
        {
            return mappingSet;

            if (query.DataQueryDetail.EnumType == DataQueryDetailEnumType.DataOnly || query.DataQueryDetail.EnumType == DataQueryDetailEnumType.SeriesKeysOnly)
            {
                mappingSet.Dataflow.Dsd.Attributes.Clear();
                mappingSet.Dataflow.Dsd.Groups.Clear();
            }

            bool bReiterate = false;
            //Request details of the data to be returned (Full, DataOnly, SeriesKeyOnly, NoData) SRA-349
            switch (query.DataQueryDetail.EnumType)
            {
                case DataQueryDetailEnumType.DataOnly: //No Data attributes.
                    //Remove all attributes mappings from the MappingSetEntity
                    foreach (MappingEntity mapEntity in mappingSet.Mappings)
                    {
                        bReiterate = true;
                        while (bReiterate)
                        {
                            bReiterate = false;
                            foreach (ComponentEntity comp in mapEntity.Components)
                            {
                                if (IsAttributeComponent(comp))
                                {
                                    bReiterate = true;
                                    mapEntity.Components.Remove(comp);
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case DataQueryDetailEnumType.SeriesKeysOnly: //Only dimensions, only the Series element
                    //Remove all attributes & observation mapping  from the MappingSetEntity 
                    foreach (MappingEntity mapEntity in mappingSet.Mappings)
                    {
                        bReiterate = true;
                        while (bReiterate)
                        {
                            bReiterate = false;
                            foreach (ComponentEntity comp in mapEntity.Components)
                            {
                                if (IsObservationComponent(comp) || IsAttributeComponent(comp))
                                {
                                    bReiterate = true;
                                    mapEntity.Components.Remove(comp);
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case DataQueryDetailEnumType.NoData: //Groups, Series, Annotations, Attributes, Dimensions but no observations.
                    //Remove all observation mapping  from the MappingSetEntity
                    foreach (MappingEntity mapEntity in mappingSet.Mappings)
                    {
                        bReiterate = true;
                        while (bReiterate)
                        {
                            bReiterate = false;
                            foreach (ComponentEntity comp in mapEntity.Components)
                            {
                                if (IsObservationComponent(comp))
                                {
                                    bReiterate = true;
                                    mapEntity.Components.Remove(comp);
                                    break;
                                }
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            return mappingSet;
        }

        /// <summary>
        /// SRA-349 check the component entity type observation
        /// </summary>
        /// <param name="component">
        /// The ComponentEntity containing the component
        /// </param>
        /// <returns>
        /// True for an observation component, false for all other types 
        /// </returns>
        private bool IsObservationComponent(ComponentEntity component)
        {
            switch (component.ComponentType)
            {
                case SdmxComponentType.Dimension:
                    return true;
                case SdmxComponentType.Attribute:
                    switch (component.AttributeAttachmentLevel)
                    {
                        case AttachmentLevel.Series:
                        case AttachmentLevel.Observation:
                            return true;
                        default:
                            break;
                    }
                    break;
                case SdmxComponentType.PrimaryMeasure:
                    return true;
                case SdmxComponentType.CrossSectionalMeasure:
                    return true;
                default:
                    break;
            }
            return false;
        }

        /// <summary>
        /// SRA-349 check the component entity type attribute
        /// </summary>
        /// <param name="component">
        /// The ComponentEntity containing the component
        /// </param>
        /// <returns>
        /// True for an attribute component, false for all other types 
        /// </returns>
        private bool IsAttributeComponent(ComponentEntity component)
        {
            switch (component.ComponentType)
            {
                case SdmxComponentType.Attribute:
                    switch (component.AttributeAttachmentLevel)
                    {
                        case AttachmentLevel.DataSet:
                            return true;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }

        /// <summary>
        /// Returns the mapping set of the specified <paramref name="dataFlow"/> .
        /// </summary>
        /// <param name="dataFlow">
        /// The Query dataflow 
        /// </param>
        /// <returns>
        /// The Mapping Set 
        /// </returns>
        /// <exception cref="DataRetrieverException">
        /// See the
        ///   <see cref="ErrorTypes"/>
        /// </exception>
        private MappingSetEntity RetrieveMappingSet(IDataflowObject dataFlow)
        {
            MappingSetEntity mappingSet;
            try
            {
                mappingSet = MappingSetRetriever.GetMappingSet(
                    this._connectionStringSettings, dataFlow.Id, dataFlow.Version, dataFlow.AgencyId, null);
            }
            catch (DataRetrieverException)
            {
                throw;
            }
            catch (DbException ex)
            {
                string error =
                    Resources.DataRetriever_Initialize_Could_not_retrieve_Mappings_from__Mapping_Store___Cause__
                    + ex.Message;
                throw new DataRetrieverException(ex,
                    SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.InternalServerError),
                    error);
                //ErrorTypes.MS_CONNECTION_ERROR, error, ex);
            }
            catch (Exception ex)
            {
                string error =
                    Resources.DataRetriever_Initialize_Error_while_retrieving_Mappings_from__Mapping_Store___Cause__
                    + ex.Message;
                throw new DataRetrieverException(ex,
                    SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.InternalServerError),
                    error);
                //ErrorTypes.MS_CONNECTION_ERROR, error, ex);
            }

            return mappingSet;
        }        
        
        ///// <summary>
        ///// Write the SDMX header (from <paramref name="info"/> ) to <paramref name="writer"/>
        ///// </summary>
        ///// <param name="writer">
        ///// The writer. 
        ///// </param>
        ///// <param name="info">
        ///// The info. 
        ///// </param>
        //private void WriteHeader(IWriterEngine writer, DataRetrievalInfo info)
        //{
        //    // retrieve header information
        //    Logger.Info(Resources.DataRetriever_RetrieveData_Start_Retrieving_header_information);
        //    IHeader header = _headerBuilder.Build(info);
        //    Logger.Info(Resources.DataRetriever_RetrieveData_End_Retrieving_header_information);

        //    writer.WriteHeader(header);
        //}


        private void RDFWriterStrucInfo(DataRetrievalInfoSeries info, string dataset, string struc)
        {
            info.SeriesWriter.RDFWriterStrucInfo(dataset, struc);
        }

        #endregion
    }
}