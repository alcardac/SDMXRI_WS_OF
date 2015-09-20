// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MappingSetRetriever.cs" company="Eurostat">
//   Date Created : 2011-05-02
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This is a class that retrieves data from the a mapping store.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Config;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// This is a class that retrieves data from the a mapping store.
    /// </summary>
    public static class MappingSetRetriever
    {
        /// <summary>
        /// Gets or sets a value indicating whether to query only production dataflow.
        /// </summary>
        private static bool _productionDataflowOnly = !ConfigManager.Config.DataflowConfiguration.IgnoreProductionForData;

        /// <summary>
        /// Gets or sets a value indicating whether to query only production dataflow.
        /// </summary>
        public static bool ProductionDataflowOnly
        {
            get
            {
                return _productionDataflowOnly;
            }

            set
            {
                _productionDataflowOnly = value;
            }
        }

        #region Public Methods

         /// <summary>
         /// This method queries the mapping store for a mapping set for a specific dataflow.
         /// If such a mapping set exists, it is returned as a <see cref="MappingSetEntity"/>.
         /// Else null is returned.
         /// </summary>
         /// <param name="connectionStringSettings">
         /// The connection string parameters used to connect to the Mapping Store Database
         /// </param>
         /// <param name="dataflowId">
         /// The identifier of the DataFlow
         /// </param>
         /// <param name="dataflowVersion">
         /// The version of the DataFlow
         /// </param>
         /// <param name="dataflowAgency">
         /// The agency DataFlow
         /// </param>
         /// <param name="allowedDataflows">
         /// The collection of dataflow that can be returned. It can be null in which case all dataflows are returned, If is empty or the requested dataflow is not in the allowed collection then null is returned.
         /// </param>
         /// <exception cref="ArgumentNullException">
         /// connectionStringSettings or dataflowId is null
         /// </exception>
         /// <returns>
         /// The populated <see cref="MappingSetEntity"/> or null if the dataflow is not found or it is not in the allowed list
         /// </returns>
         public static MappingSetEntity GetMappingSet(
             ConnectionStringSettings connectionStringSettings, string dataflowId, string dataflowVersion, string dataflowAgency, IList<IMaintainableRefObject> allowedDataflows)
         {
             if (dataflowId == null)
             {
                 throw new ArgumentNullException("dataflowId");
             }

             return GetMappingSet(connectionStringSettings, new MaintainableRefObjectImpl(dataflowAgency, dataflowId, dataflowVersion), allowedDataflows);
         }

        /// <summary>
        /// This method queries the mapping store for a mapping set for a specific dataflow.
        /// If such a mapping set exists, it is returned as a <see cref="MappingSetEntity"/>.
        /// Else null is returned.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection string parameters used to connect to the Mapping Store Database
        /// </param>
        /// <param name="maintainableRef">
        /// The reference of the DataFlow
        /// </param>
        /// <param name="allowedDataflows">
        /// The collection of dataflow that can be returned. It can be null in which case all dataflows are returned, If is empty or the requested dataflow is not in the allowed collection then null is returned.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// connectionStringSettings or dataflowId is null
        /// </exception>
        /// <returns>
        /// The populated <see cref="MappingSetEntity"/> or null if the dataflow is not found or it is not in the allowed list
        /// </returns>
        public static MappingSetEntity GetMappingSet(
            ConnectionStringSettings connectionStringSettings, 
            IMaintainableRefObject maintainableRef,
            IList<IMaintainableRefObject> allowedDataflows)
        {
            if (connectionStringSettings == null)
            {
                throw new ArgumentNullException("connectionStringSettings");
            }

            if (maintainableRef == null)
            {
                throw new ArgumentNullException("maintainableRef");
            }

            if (!maintainableRef.HasMaintainableId())
            {
                throw new ArgumentException(ErrorMessages.DataflowIdIsNull, "maintainableRef");
            }

            if (!SecurityHelper.Contains(allowedDataflows, maintainableRef))
            {
                return null;
            }

            MappingSetEntity ret = null;
            long datasetId = 0;

            var mappingStoreDb = new Database(connectionStringSettings);

            using (DbCommand command = BuildSqlCommand(maintainableRef, mappingStoreDb, allowedDataflows))
            {
                // populate MappingSet
                using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
                {
                    // we expect only 1 record here
                    if (dataReader.Read())
                    {
                        ret = new MappingSetEntity(DataReaderHelper.GetInt64(dataReader, "MAP_SET_ID"));

                        ret.Id = DataReaderHelper.GetString(dataReader, "ID");
                        ret.Description = DataReaderHelper.GetString(dataReader, "DESCRIPTION");
                        ret.Dataflow = new DataflowEntity(DataReaderHelper.GetInt64(dataReader, "DF_ID"));
                        ret.Dataflow.Id = DataReaderHelper.GetString(dataReader, "DFID");

                        ret.Dataflow.Version = DataReaderHelper.GetString(dataReader, "DFVER");
                        ret.Dataflow.Agency = DataReaderHelper.GetString(dataReader, "DFAG");
                        ret.Dataflow.MappingSet = ret;
                        datasetId = DataReaderHelper.GetInt64(dataReader, "DS_ID");
                    }
                }
            }

            if (ret == null)
            {
                // throw new Exception(String.Format(CultureInfo.InvariantCulture,"There was no mapping for dataflow:{0}", dataflowId));
                // MAt-395
                return null;
            }

            // populate the dataset field
            ret.DataSet = GetDatasetById(mappingStoreDb, datasetId);

            // populate the dataflow field
            // ret.Dataflow = GetDataFlowByMappingSetSysId(connectionStringSettings, ret.SysId);
            ret.Dataflow.Dsd = GetDsdByDataFlowSysId(mappingStoreDb, ret.Dataflow.SysId);

            // populate the mapping field
            GetMappingsByMappingSetSysId(mappingStoreDb, ret);

            return ret;
        }

        /// <summary>
        /// Build SQL command for retrieving Mapping Set for the dataflow with the specified <paramref name="maintainableRef"/> .
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable ref.
        /// </param>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="StringBuilder"/>.
        /// </returns>
        private static DbCommand BuildSqlCommand(IMaintainableRefObject maintainableRef, Database mappingStoreDb, IList<IMaintainableRefObject> allowedDataflows)
        {
            var builder = new DataflowCommandBuilder(mappingStoreDb, ProductionDataflowOnly ? DataflowFilter.Production : DataflowFilter.Any);
            var sqlCommand = new StringBuilder(MappingStoreSqlStatements.MappingSetDataflow);
            
            // get a specific version of the dataflow
            if (!maintainableRef.HasVersion())
            {
                // or else get the latest
                SqlHelper.AddWhereClause(
                    sqlCommand,
                    WhereState.And,
                    " (SELECT COUNT(*) FROM ARTEFACT A2 INNER JOIN DATAFLOW T2 ON A2.ART_ID = T2.DF_ID where A2.ID=A.ID AND A2.AGENCY=A.AGENCY AND dbo.isGreaterVersion(A2.VERSION1, A2.VERSION2, A2.VERSION3, A.VERSION1, A.VERSION2, A.VERSION3)=1 {0} ) = 0 ",
                    ProductionDataflowOnly ? DataflowConstant.ProductionWhereLatestClause : string.Empty);
            }

            var sqlQueryInfo = new SqlQueryInfo { QueryFormat = sqlCommand.ToString(), WhereStatus = WhereState.And };
            return builder.Build(new ArtefactSqlQuery(sqlQueryInfo, maintainableRef), allowedDataflows);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add the list of components to the specified component map
        /// </summary>
        /// <param name="componentList">
        /// The <see cref="ComponentEntity"/> list to read from
        /// </param>
        /// <param name="componentMap">
        /// The long to <see cref="ComponentEntity"/> map to populate
        /// </param>
        private static void AddToComponentMap(
            IEnumerable<ComponentEntity> componentList, IDictionary<long, ComponentEntity> componentMap)
        {
            foreach (ComponentEntity comp in componentList)
            {
                componentMap.Add(comp.SysId, comp);
            }
        }

        /// <summary>
        /// The method retrieves the <see cref="CodeListEntity"/> object
        /// by the component system identifier
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The <see cref="Database"/> instance for Mapping Store database
        /// </param>
        /// <param name="sysId">
        /// The component with the specified system identifier
        /// </param>
        /// <returns>
        /// The populated <see cref="CodeListEntity"/>object
        /// </returns>
        private static CodeListEntity GetCodeListByComponentSysId(Database mappingStoreDb, long sysId)
        {
            CodeListEntity ret = null;

            string paramId = mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter);

            var sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT CL.CL_ID, A.ID as CODELIST_ID, A.AGENCY, A.VERSION ");
            sqlCommand.Append("FROM CODELIST CL, COMPONENT COMP, ARTEFACT_VIEW A ");
            sqlCommand.Append("WHERE COMP.CL_ID = CL.CL_ID ");
            sqlCommand.Append("AND CL.CL_ID = A.ART_ID ");
            sqlCommand.AppendFormat("AND COMP.COMP_ID = {0} ", paramId);
            using (DbCommand command = mappingStoreDb.GetSqlStringCommand(sqlCommand.ToString()))
            {
                mappingStoreDb.AddInParameter(command, ParameterNameConstants.IdParameter, DbType.Int64, sysId);

                using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
                {
                    // we expect only 1 record here
                    if (dataReader.Read())
                    {
                        ret = new CodeListEntity(DataReaderHelper.GetInt64(dataReader, "CL_ID"))
                                  {
                                      Id = DataReaderHelper.GetString(dataReader, "CODELIST_ID"),
                                      Agency = DataReaderHelper.GetString(dataReader, "AGENCY"),
                                      Version = DataReaderHelper.GetString(dataReader, "VERSION")
                                  };

                        // ret.CodeList.Add(DataReaderHelper.GetString(dataReader, "CODE_ID"));
                        // while (dataReader.Read())
                        // {
                        // ret.CodeList.Add(DataReaderHelper.GetString(dataReader, "CODE_ID"));
                        // }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// The method retrieves the <see cref="ConceptEntity"/> object
        /// by the component system identifier
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The <see cref="Database"/> instance for Mapping Store database
        /// </param>
        /// <param name="sysId">
        /// The component with the specified system identifier
        /// </param>
        /// <returns>
        /// The populated <see cref="ConceptEntity"/>object
        /// </returns>
        private static ConceptEntity GetConceptByComponentSysId(Database mappingStoreDb, long sysId)
        {
            ConceptEntity ret = null;

            string paramId = mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter);

            var sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT CON.CON_ID, IT.ID ");
            sqlCommand.Append("FROM CONCEPT CON, COMPONENT COMP, ITEM IT ");
            sqlCommand.Append("WHERE COMP.CON_ID = CON.CON_ID ");
            sqlCommand.Append("AND CON.CON_ID = IT.ITEM_ID ");
            sqlCommand.AppendFormat("AND COMP.COMP_ID = {0} ", paramId);

            using (DbCommand command = mappingStoreDb.GetSqlStringCommand(sqlCommand.ToString()))
            {
                mappingStoreDb.AddInParameter(command, ParameterNameConstants.IdParameter, DbType.Int64, sysId);

                using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
                {
                    // we expect only 1 record here
                    if (dataReader.Read())
                    {
                        ret = new ConceptEntity(DataReaderHelper.GetInt64(dataReader, "CON_ID")) { Id = DataReaderHelper.GetString(dataReader, "ID") };
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// This method retrieves a <see cref="ConnectionEntity"/> object
        /// by its identifier
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The <see cref="Database"/> instance for Mapping Store database
        /// </param>
        /// <param name="connectionId">
        /// The connection with the specified identifier
        /// </param>
        /// <returns>
        /// The populated <see cref="ConnectionEntity"/>object
        /// </returns>
        private static ConnectionEntity GetConnectionById(Database mappingStoreDb, long connectionId)
        {
            ConnectionEntity ret = null;

            string paramId = mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter);

            var sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT CON.CONNECTION_ID, CON.DB_NAME, CON.DB_TYPE, CON.NAME, ");
            sqlCommand.Append("CON.DB_PASSWORD, CON.DB_PORT, CON.DB_SERVER, CON.DB_USER, ");
            sqlCommand.Append("CON.ADO_CONNECTION_STRING, CON.JDBC_CONNECTION_STRING ");
            sqlCommand.Append("FROM DB_CONNECTION CON ");
            sqlCommand.AppendFormat("WHERE CON.CONNECTION_ID = {0} ", paramId);

            DbCommand command = mappingStoreDb.GetSqlStringCommand(sqlCommand.ToString());

            mappingStoreDb.AddInParameter(command, ParameterNameConstants.IdParameter, DbType.Int64, connectionId);

            using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
            {
                // we expect only 1 record here
                while (dataReader.Read())
                {
                    ret = new ConnectionEntity(DataReaderHelper.GetInt64(dataReader, "CONNECTION_ID"));
                    ret.DBName = DataReaderHelper.GetString(dataReader, "DB_NAME");
                    ret.DBType = DataReaderHelper.GetString(dataReader, "DB_TYPE");
                    ret.Name = DataReaderHelper.GetString(dataReader, "NAME");

                    // ret.Owner = DataReaderHelper.GetString(dataReader, "OWNER");
                    ret.DBPassword = DataReaderHelper.GetString(dataReader, "DB_PASSWORD");
                    //ret.DBPort = DataReaderHelper.GetInt32(dataReader, "DB_PORT");
                    ret.DBServer = DataReaderHelper.GetString(dataReader, "DB_SERVER");
                    ret.DBUser = DataReaderHelper.GetString(dataReader, "DB_USER");
                    ret.AdoConnectionString = DataReaderHelper.GetString(dataReader, "ADO_CONNECTION_STRING");
                    ret.JdbcConnectionString = DataReaderHelper.GetString(dataReader, "JDBC_CONNECTION_STRING");
                }
            }

            if (ret == null)
            {
                throw new MappingStoreException(
                    string.Format(
                        CultureInfo.InvariantCulture, 
                        ErrorMessages.MappingStoreNoEntityFormat2, 
                        typeof(ConnectionEntity), 
                        connectionId));
            }

            return ret;
        }

        /// <summary>
        /// This method retrieves a <see cref="DataSetEntity"/> by its identifier
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The <see cref="Database"/> instance for Mapping Store database
        /// </param>
        /// <param name="datasetId">
        /// The dataset identifier, by which the dataset will be retrieved
        /// </param>
        /// <returns>
        /// The populated <see cref="DataSetEntity"/> object
        /// </returns>
        private static DataSetEntity GetDatasetById(Database mappingStoreDb, long datasetId)
        {
            DataSetEntity ret = null;
            long connectionId = 0;

            string paramId = mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter);

            var sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT DS.DS_ID, DS.NAME, DS.QUERY, DS.CONNECTION_ID ");
            sqlCommand.Append("FROM DATASET DS ");
            sqlCommand.AppendFormat("WHERE DS.DS_ID = {0} ", paramId);

            DbCommand command = mappingStoreDb.GetSqlStringCommand(sqlCommand.ToString());

            mappingStoreDb.AddInParameter(command, ParameterNameConstants.IdParameter, DbType.Int64, datasetId);

            using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
            {
                // we expect only 1 record here
                while (dataReader.Read())
                {
                    ret = new DataSetEntity(DataReaderHelper.GetInt64(dataReader, "DS_ID"))
                              {
                                  Description = DataReaderHelper.GetString(dataReader, "NAME"),
                                  Query = DataReaderHelper.GetString(dataReader, "QUERY")
                              };

                    connectionId = DataReaderHelper.GetInt64(dataReader, "CONNECTION_ID");
                }
            }

            if (ret == null)
            {
                throw new MappingStoreException(
                    string.Format(CultureInfo.InvariantCulture, "There was no dataset for Id:{0}", datasetId));
            }

            ret.Connection = GetConnectionById(mappingStoreDb, connectionId);

            return ret;
        }

        /// <summary>
        /// The method retrieves the <see cref="DsdEntity"/> object
        /// by the data flow system identifier
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The <see cref="Database"/> instance for Mapping Store database
        /// </param>
        /// <param name="sysId">
        /// The data flow with the specified system identifier
        /// </param>
        /// <returns>
        /// The populated <see cref="DsdEntity"/>object
        /// </returns>
        private static DsdEntity GetDsdByDataFlowSysId(Database mappingStoreDb, long sysId)
        {
            DsdEntity ret = null;

            string paramId = mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter);

            var sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT DSD.DSD_ID, ART.ID, ART.VERSION, ART.AGENCY ");
            sqlCommand.Append("FROM DSD, DATAFLOW DF, ARTEFACT_VIEW ART ");
            sqlCommand.Append("WHERE DF.DSD_ID = DSD.DSD_ID ");
            sqlCommand.Append("AND DSD.DSD_ID = ART.ART_ID ");
            sqlCommand.AppendFormat("AND DF.DF_ID = {0} ", paramId);

            using (DbCommand command = mappingStoreDb.GetSqlStringCommand(sqlCommand.ToString()))
            {
                mappingStoreDb.AddInParameter(command, ParameterNameConstants.IdParameter, DbType.Int64, sysId);

                using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
                {
                    // we expect only 1 record here
                    if (dataReader.Read())
                    {
                        ret = new DsdEntity(DataReaderHelper.GetInt64(dataReader, "DSD_ID"))
                                  {
                                      Id = DataReaderHelper.GetString(dataReader, "ID"),
                                      Version = DataReaderHelper.GetString(dataReader, "VERSION"),
                                      Agency = DataReaderHelper.GetString(dataReader, "AGENCY")
                                  };
                    }
                }
            }

            if (ret == null)
            {
                throw new MappingStoreException(
                    string.Format(
                        CultureInfo.InvariantCulture, 
                        ErrorMessages.MappingStoreNoEntityFormat2, 
                        typeof(DsdEntity), 
                        sysId));
            }

            // populate the Dsd fields by the correct components
            PoulateDsdComponentsByDsdSysId(mappingStoreDb, ret.SysId, ref ret);

            return ret;
        }

        /// <summary>
        /// This method retrieves a list of <see cref="MappingEntity"/> objects
        /// by the mapping set system identifier
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The <see cref="Database"/> instance for Mapping Store database
        /// </param>
        /// <param name="mappingSet">
        /// The mapping set
        /// </param>
        private static void GetMappingsByMappingSetSysId(Database mappingStoreDb, MappingSetEntity mappingSet)
        {
            long sysId = mappingSet.SysId;

            var dictionary = new Dictionary<long, MappingEntity>();

            var componentMap = new Dictionary<long, ComponentEntity>();

            bool transcoded = false;

            MappingEntity timeDimensionMapping = null;

            AddToComponentMap(mappingSet.Dataflow.Dsd.Dimensions, componentMap);
            if (mappingSet.Dataflow.Dsd.TimeDimension != null)
            {
                componentMap.Add(mappingSet.Dataflow.Dsd.TimeDimension.SysId, mappingSet.Dataflow.Dsd.TimeDimension);
            }

            componentMap.Add(mappingSet.Dataflow.Dsd.PrimaryMeasure.SysId, mappingSet.Dataflow.Dsd.PrimaryMeasure);

            AddToComponentMap(mappingSet.Dataflow.Dsd.Attributes, componentMap);
            AddToComponentMap(mappingSet.Dataflow.Dsd.CrossSectionalMeasures, componentMap);

            string paramId = mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter);

            var sqlCommand = new StringBuilder();

            // select
            sqlCommand.Append("SELECT CMAP.MAP_ID, CMAP.TYPE, CMAP.CONSTANT, CMAPCOMP.COMP_ID, TR.TR_ID, TR.EXPRESSION ");
           
            // from1
            sqlCommand.Append("FROM COMPONENT_MAPPING CMAP INNER JOIN  MAPPING_SET MSET ON MSET.MAP_SET_ID = CMAP.MAP_SET_ID INNER JOIN COM_COL_MAPPING_COMPONENT CMAPCOMP ON CMAPCOMP.MAP_ID = CMAP.MAP_ID ");
            sqlCommand.Append(" LEFT OUTER JOIN TRANSCODING TR ON TR.MAP_ID = CMAP.MAP_ID ");

            // where
            sqlCommand.AppendFormat(" WHERE MSET.MAP_SET_ID = {0} ", paramId);
            using (DbCommand command = mappingStoreDb.GetSqlStringCommand(sqlCommand.ToString()))
            {
                mappingStoreDb.AddInParameter(command, ParameterNameConstants.IdParameter, DbType.Int64, sysId);
                using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
                {
                    int mapID = dataReader.GetOrdinal("MAP_ID");
                    int type = dataReader.GetOrdinal("TYPE");
                    int constant = dataReader.GetOrdinal("CONSTANT");
                    int compIDIdx = dataReader.GetOrdinal("COMP_ID");
                    int tridIdx = dataReader.GetOrdinal("TR_ID");
                    int exprIdx  = dataReader.GetOrdinal("EXPRESSION");
                    while (dataReader.Read())
                    {
                        // get existing or create new mapping object
                        long mapId = DataReaderHelper.GetInt64(dataReader, mapID);
                        MappingEntity mapping;
                        if (!dictionary.TryGetValue(mapId, out mapping))
                        {
                            // create new mapping and append it to the mapping set
                            mapping = new MappingEntity(mapId) { MappingType = DataReaderHelper.GetString(dataReader, type), Constant = DataReaderHelper.GetString(dataReader, constant) };

                            long trid = DataReaderHelper.GetInt64(dataReader, tridIdx);
                            if (trid > 0)
                            {
                                mapping.Transcoding = new TranscodingEntity(trid) { Expression = DataReaderHelper.GetString(dataReader, exprIdx) };
                                transcoded = true;
                            }

                            dictionary.Add(mapId, mapping);
                            mappingSet.Mappings.Add(mapping);
                        }

                        long componentSysId = DataReaderHelper.GetInt64(dataReader, compIDIdx);

                        ComponentEntity component;
                        if (componentMap.TryGetValue(componentSysId, out component))
                        {
                            mapping.Components.Add(component);
                            if (component.Equals(mappingSet.Dataflow.Dsd.TimeDimension) && mapping.Transcoding != null)
                            {
                                timeDimensionMapping = mapping;
                            }
                        }
                    }
                }
            }

            // second select
            sqlCommand = new StringBuilder();
            sqlCommand.Append(
                "SELECT CMAP.MAP_ID, CMAP.TYPE, CMAP.CONSTANT, DSCOL.COL_ID, DSCOL.NAME, DSCOL.DS_ID ");
            sqlCommand.Append(
                "FROM COMPONENT_MAPPING CMAP, MAPPING_SET MSET, COM_COL_MAPPING_COLUMN CMAPCOL, DATASET_COLUMN DSCOL ");
            sqlCommand.Append("WHERE MSET.MAP_SET_ID = CMAP.MAP_SET_ID ");
            sqlCommand.Append("AND CMAP.MAP_ID = CMAPCOL.MAP_ID ");
            sqlCommand.Append("AND CMAPCOL.COL_ID = DSCOL.COL_ID ");
            sqlCommand.AppendFormat("AND MSET.MAP_SET_ID = {0} ", paramId);

            using (DbCommand command = mappingStoreDb.GetSqlStringCommand(sqlCommand.ToString()))
            {
                mappingStoreDb.AddInParameter(command, ParameterNameConstants.IdParameter, DbType.Int64, sysId);
                using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
                {
                    int mapID = dataReader.GetOrdinal("MAP_ID");
                    int type = dataReader.GetOrdinal("TYPE");
                    int constant = dataReader.GetOrdinal("CONSTANT");
                    int colIDIdx = dataReader.GetOrdinal("COL_ID");
                    int nameIdx = dataReader.GetOrdinal("NAME");
                    ////int descIdx = dataReader.GetOrdinal("DESCRIPTION");
                    while (dataReader.Read())
                    {
                        // get existing or create new mapping object
                        long mapId = DataReaderHelper.GetInt64(dataReader, mapID);
                        MappingEntity mapping;
                        if (!dictionary.TryGetValue(mapId, out mapping))
                        {
                            // create new mapping and append it to the mapping set
                            mapping = new MappingEntity(mapId) { MappingType = DataReaderHelper.GetString(dataReader, type), Constant = DataReaderHelper.GetString(dataReader, constant) };

                            dictionary.Add(mapId, mapping);
                            mappingSet.Mappings.Add(mapping);
                        }

                        var column = new DataSetColumnEntity(DataReaderHelper.GetInt64(dataReader, colIDIdx)) { Name = DataReaderHelper.GetString(dataReader, nameIdx) };

                        // dsColumn.SysId = DataReaderHelper.GetInt64(dataReader, "DS_ID");
                        ////column.Description = DataReaderHelper.GetString(dataReader, descIdx);

                        mapping.Columns.Add(column);
                    }
                }
            }
            
            if (transcoded)
            {
                GetTranscodingRulesByMapSetId(mappingStoreDb, sysId, timeDimensionMapping, dictionary);
                RetrieveTimeDimensionTranscoding(mappingStoreDb, timeDimensionMapping);
            }
        }

        /// <summary>
        /// The method retrieves the <see cref="TranscodingRulesEntity"/> object
        /// by the transcoding system identifier
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The <see cref="Database"/> instance for Mapping Store database
        /// </param>
        /// <param name="sysId">
        /// The mapping set with the specified system identifier
        /// </param>
        /// <param name="timeDimensionMapping">
        /// The TRANSCODING.TR_ID value of the TimeDimension, if there is TimeDimension and is transcoded; otherwise set to null 
        /// </param>
        /// <param name="mappingsMap">
        /// The map between COMPONENT_MAPPING.MAP_ID value and MappingEntity 
        /// </param>
        private static void GetTranscodingRulesByMapSetId(Database mappingStoreDb, long sysId, MappingEntity timeDimensionMapping, IDictionary<long, MappingEntity> mappingsMap)
        {
            string paramId = mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter);
            string timeDimensionTranscodingIdParam = mappingStoreDb.BuildParameterName(ParameterNameConstants.TranscodingId);

            object timedimensionID = timeDimensionMapping == null ? (object)null : timeDimensionMapping.Transcoding.SysId;

            // holds the dsd codes by rule
            var componentsByRule = new Dictionary<long, CodeCollection>();

            string sql = string.Format(
               CultureInfo.InvariantCulture,
               MappingStoreSqlStatements.TranscodingRulesDsdCodes,
               paramId,
               timeDimensionTranscodingIdParam);

            using (DbCommand command = mappingStoreDb.GetSqlStringCommand(sql))
            {
                mappingStoreDb.AddInParameter(command, ParameterNameConstants.IdParameter, DbType.Int64, sysId);
                mappingStoreDb.AddInParameter(command, ParameterNameConstants.TranscodingId, DbType.Int64, timedimensionID);

                using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
                {
                    int pos = 0;

                    long prevMapId = -1;

                    TranscodingRulesEntity currentRules = null;
                    long firstTrRuleID = -1;
                    long curTrRuleID = -1;
                    long prevTrRuleID = -1;
                    
                    var componentList = new CodeCollection();

                    int ruleIDIdx = dataReader.GetOrdinal("TR_RULE_ID");
                    int componentIdx = dataReader.GetOrdinal("COMPONENT");
                    int codeIdx = dataReader.GetOrdinal("CODE");
                    int mapIdIdx = dataReader.GetOrdinal("MAP_ID");
                    while (dataReader.Read())
                    {
                        long curMapId = DataReaderHelper.GetInt64(dataReader, mapIdIdx);
                        if (prevMapId != curMapId || currentRules == null)
                        {
                            prevMapId = curMapId;
                            if (curTrRuleID > -1)
                            {
                                componentsByRule.Add(curTrRuleID, componentList);
                            }

                            componentList = new CodeCollection();
                            firstTrRuleID = -1;
                            prevTrRuleID = -1;
                            pos = 0;
                            MappingEntity curretMapping = mappingsMap[curMapId];
                            currentRules = new TranscodingRulesEntity();
                            curretMapping.Transcoding.TranscodingRules = currentRules;
                        }

                        curTrRuleID = DataReaderHelper.GetInt64(dataReader, ruleIDIdx);
                        if (firstTrRuleID == -1 || firstTrRuleID == curTrRuleID)
                        {
                            firstTrRuleID = curTrRuleID;
                            prevTrRuleID = curTrRuleID;
                            currentRules.AddComponent(DataReaderHelper.GetInt64(dataReader, componentIdx), pos);

                            pos++;
                        }
                        else if (prevTrRuleID != curTrRuleID)
                        {
                            componentsByRule.Add(prevTrRuleID, componentList);
                            componentList = new CodeCollection();
                            prevTrRuleID = curTrRuleID;
                        }

                        componentList.Add(DataReaderHelper.GetString(dataReader, codeIdx));
                    }

                    if (curTrRuleID > -1)
                    {
                        componentsByRule.Add(curTrRuleID, componentList);
                    }
                }
            }

            if (componentsByRule.Count > 0)
            {
                sql = string.Format(
              CultureInfo.InvariantCulture,
              MappingStoreSqlStatements.TranscodingRulesLocalCodes,
              paramId,
              timeDimensionTranscodingIdParam);
                using (DbCommand command = mappingStoreDb.GetSqlStringCommand(sql))
                {
                    mappingStoreDb.AddInParameter(command, ParameterNameConstants.IdParameter, DbType.Int64, sysId);
                    mappingStoreDb.AddInParameter(command, ParameterNameConstants.TranscodingId, DbType.Int64, timedimensionID);

                    using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
                    {
                        long prevMapId = -1;

                        int pos = 0;
                        TranscodingRulesEntity currentRules = null;
                        long firstTrRuleID = -1;
                        long curTrRuleID = -1;
                        long prevTrRuleID = -1;
                        var columnList = new CodeCollection();

                        int ruleIDIdx = dataReader.GetOrdinal("TR_RULE_ID");
                        int columnIdx = dataReader.GetOrdinal("LOCAL_COLUMN");
                        int codeIdx = dataReader.GetOrdinal("CODE");
                        int mapIdIdx = dataReader.GetOrdinal("MAP_ID");
                        while (dataReader.Read())
                        {
                            long curMapId = DataReaderHelper.GetInt64(dataReader, mapIdIdx);
                            if (prevMapId != curMapId || currentRules == null)
                            {
                                prevMapId = curMapId;
                                if (curTrRuleID > -1 && currentRules != null)
                                {
                                    currentRules.Add(columnList, componentsByRule[curTrRuleID]);
                                }

                                firstTrRuleID = -1;
                                prevTrRuleID = -1;
                                pos = 0;
                                columnList = new CodeCollection();
                                MappingEntity curretMapping = mappingsMap[curMapId];
                                currentRules = curretMapping.Transcoding.TranscodingRules;
                            }

                            curTrRuleID = DataReaderHelper.GetInt64(dataReader, ruleIDIdx);
                            if (firstTrRuleID == -1 || firstTrRuleID == curTrRuleID)
                            {
                                firstTrRuleID = curTrRuleID;
                                prevTrRuleID = curTrRuleID;
                                currentRules.AddColumn(DataReaderHelper.GetInt64(dataReader, columnIdx), pos);
                                pos++;
                            }
                            else if (prevTrRuleID != curTrRuleID)
                            {
                                currentRules.Add(columnList, componentsByRule[prevTrRuleID]);
                                columnList = new CodeCollection();
                                prevTrRuleID = curTrRuleID;
                            }

                            columnList.Add(DataReaderHelper.GetString(dataReader, codeIdx));
                        }

                        if (curTrRuleID > -1 && currentRules != null)
                        {
                            currentRules.Add(columnList, componentsByRule[curTrRuleID]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve time transcoding and populate the <paramref name="timeDimensionMapping"/>.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <param name="timeDimensionMapping">
        /// The time dimension mapping.
        /// </param>
        private static void RetrieveTimeTranscoding(Database mappingStoreDb, MappingEntity timeDimensionMapping)
        {
            if (timeDimensionMapping == null)
            {
                return;
            }

            DbParameter parameter = mappingStoreDb.CreateInParameter(ParameterNameConstants.TranscodingId, DbType.Int64, timeDimensionMapping.Transcoding.SysId);

            using (DbCommand command = mappingStoreDb.GetSqlStringCommandFormat(MappingStoreSqlStatements.TimeTranscoding, parameter))
            {
                using (IDataReader reader = mappingStoreDb.ExecuteReader(command))
                {
                    int freqIdx = reader.GetOrdinal("FREQ");
                    int yearColIdIdx = reader.GetOrdinal("YEAR_COL_ID");
                    int periodColIdIdx = reader.GetOrdinal("PERIOD_COL_ID");
                    int dateColIdIdx = reader.GetOrdinal("DATE_COL_ID");
                    int expressionIdx = reader.GetOrdinal("EXPRESSION");

                    var timeTranscodingCollection = timeDimensionMapping.Transcoding.TimeTranscodingCollection;
                    while (reader.Read())
                    {
                        var timeTranscoding = timeTranscodingCollection.Add(DataReaderHelper.GetString(reader, freqIdx));
                        timeTranscoding.YearColumnId = DataReaderHelper.GetInt64(reader, yearColIdIdx);
                        timeTranscoding.PeriodColumnId = DataReaderHelper.GetInt64(reader, periodColIdIdx);
                        timeTranscoding.DateColumnId = DataReaderHelper.GetInt64(reader, dateColIdIdx);
                        timeTranscoding.Expression = DataReaderHelper.GetString(reader, expressionIdx);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve TimeDimension transcoding, if <paramref name="timeDimensionMapping"/> is null
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <param name="timeDimensionMapping">
        /// The time dimension mapping.
        /// </param>
        private static void RetrieveTimeDimensionTranscoding(
            Database mappingStoreDb, MappingEntity timeDimensionMapping)
        {
            if (timeDimensionMapping == null)
            {
                return;
            }

            RetrieveTimeTranscoding(mappingStoreDb, timeDimensionMapping);
            TimeTranscodingCollection timeTranscodingCollection = timeDimensionMapping.Transcoding.TimeTranscodingCollection;
            if (timeTranscodingCollection.Count == 1
                && TimeFormat.GetTimeFormatFromCodeId(timeTranscodingCollection[0].FrequencyValue).EnumType == TimeFormatEnumType.Year)
            {
                return;
            }

            var codelists = PeriodCodelist.PeriodCodelistIdMap;
            IDictionary<string, TimeTranscodingEntity> transcodingMap = new Dictionary<string, TimeTranscodingEntity>(StringComparer.Ordinal);
            foreach (var periodObject in codelists)
            {
                if (timeTranscodingCollection.Contains(periodObject.Key))
                {
                    var timeTranscodingEntity = timeTranscodingCollection[periodObject.Key];
                    if (timeTranscodingEntity.PeriodColumnId > 0)
                    {
                        var timeDimensionRules = new TranscodingRulesEntity();
                        timeDimensionRules.AddColumn(timeTranscodingEntity.PeriodColumnId, 0);
                        timeDimensionRules.AddComponent(timeDimensionMapping.Components[0].SysId, 0);
                        timeTranscodingEntity.TranscodingRules = timeDimensionRules;
                        transcodingMap.Add(periodObject.Value.Id, timeTranscodingEntity);
                    }

                }
            }

            DbParameter parameter = mappingStoreDb.CreateInParameter(ParameterNameConstants.TranscodingId, DbType.Int64, timeDimensionMapping.Transcoding.SysId);

            using (DbCommand command = mappingStoreDb.GetSqlStringCommandFormat(MappingStoreSqlStatements.TranscodingRulesTimeDimension, parameter))
            using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
            {
                int dsdcodeIdx = dataReader.GetOrdinal("DSDCODE");
                int localcodeIdx = dataReader.GetOrdinal("LOCALCODE");
                int codelistId = dataReader.GetOrdinal("CODELIST_ID");

                while (dataReader.Read())
                {
                    var periodCodelist = DataReaderHelper.GetString(dataReader, codelistId);
                    TimeTranscodingEntity timeTranscodingEntity;
                    if (transcodingMap.TryGetValue(periodCodelist, out timeTranscodingEntity))
                    {
                        var timeDimensionRules = timeTranscodingEntity.TranscodingRules;
                        var dsdperiod = new CodeCollection();
                        var localperiod = new CodeCollection();
                        dsdperiod.Add(DataReaderHelper.GetString(dataReader, dsdcodeIdx));
                        localperiod.Add(DataReaderHelper.GetString(dataReader, localcodeIdx));
                        timeDimensionRules.Add(localperiod, dsdperiod);
                    }
                }
            }
        }

        /// <summary>
        /// The method populates all the <see cref="ComponentEntity">DSD components</see>
        /// of a <see cref="DsdEntity"/> object by the data structure definition identifier
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The <see cref="Database"/> instance for Mapping Store database
        /// </param>
        /// <param name="sysId">
        /// The data flow with the specified system identifier
        /// </param>
        /// <param name="dsd">
        /// The populated <see cref="DsdEntity"/>object containing also the
        /// <see cref="ComponentEntity">DSD components</see> 
        /// </param>
        private static void PoulateDsdComponentsByDsdSysId(Database mappingStoreDb, long sysId, ref DsdEntity dsd)
        {
            GroupEntity group;
            string id;

            string paramId = mappingStoreDb.BuildParameterName(ParameterNameConstants.IdParameter);

            var sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT G.GR_ID AS GR_ID, G.ID AS ID, DG.COMP_ID AS COMP_ID ");
            sqlCommand.Append("FROM DSD D, DSD_GROUP G, DIM_GROUP DG ");
            sqlCommand.AppendFormat("WHERE D.DSD_ID = G.DSD_ID AND G.GR_ID = DG.GR_ID AND D.DSD_ID = {0} ", paramId);
            sqlCommand.Append("ORDER BY G.GR_ID");

            // used to store the groups given the dimension system id. Is used later to gather actual ComponentVO objects and set them to Group
            var groupsFromDim = new Dictionary<long, List<GroupEntity>>();

            // holds the groups created given their group id
            var groupsFromId = new Dictionary<string, GroupEntity>();
            using (DbCommand command = mappingStoreDb.GetSqlStringCommand(sqlCommand.ToString()))
            {
                mappingStoreDb.AddInParameter(command, ParameterNameConstants.IdParameter, DbType.Int64, sysId);

                using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
                {
                    // we expect only 1 record here ?
                    while (dataReader.Read())
                    {
                        id = DataReaderHelper.GetString(dataReader, "ID");

                        // do we have a new group id
                        if (!groupsFromId.ContainsKey(id))
                        {
                            group = new GroupEntity(DataReaderHelper.GetInt64(dataReader, "GR_ID")) { Id = id };

                            dsd.AddGroup(group);
                            groupsFromId.Add(id, group);
                        }
                        else
                        {
                            group = groupsFromId[id];
                        }

                        long dimId = DataReaderHelper.GetInt64(dataReader, "COMP_ID");
                        List<GroupEntity> groupsList;
                        if (!groupsFromDim.TryGetValue(dimId, out groupsList))
                        {
                            groupsList = new List<GroupEntity>();
                            groupsFromDim.Add(dimId, groupsList);
                        }

                        groupsList.Add(group);
                    }
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COMP.COMP_ID, COMP.ID as CID, COMP.TYPE, COMP.DSD_ID, COMP.CON_ID, COMP.CL_ID, ");
            sqlCommand.Append("COMP.IS_FREQ_DIM, COMP.IS_MEASURE_DIM, COMP.ATT_ASS_LEVEL, COMP.ATT_STATUS, ");
            sqlCommand.Append("COMP.ATT_IS_TIME_FORMAT, COMP.XS_ATTLEVEL_DS, COMP.XS_ATTLEVEL_GROUP, ");
            sqlCommand.Append("COMP.XS_ATTLEVEL_SECTION, COMP.XS_ATTLEVEL_OBS, COMP.XS_MEASURE_CODE, ");
            sqlCommand.Append("AG.GR_ID, GR.ID AS GID ");

            sqlCommand.Append("FROM DSD INNER JOIN COMPONENT COMP ON DSD.DSD_ID = COMP.DSD_ID ");
            sqlCommand.Append("LEFT OUTER JOIN ATT_GROUP AG ON COMP.COMP_ID = AG.COMP_ID ");
            sqlCommand.Append("LEFT OUTER JOIN DSD_GROUP GR ON AG.GR_ID = GR.GR_ID ");

            sqlCommand.AppendFormat("WHERE DSD.DSD_ID = {0} ", paramId);

            // make sure that they are returned in the order they were inserted (proper order)
            sqlCommand.Append("ORDER BY COMP.COMP_ID ");

            // hashmap keeping components already met with key the sys id.
            var componentsMap = new Dictionary<long, ComponentEntity>();
            using (DbCommand command = mappingStoreDb.GetSqlStringCommand(sqlCommand.ToString()))
            {
                mappingStoreDb.AddInParameter(command, ParameterNameConstants.IdParameter, DbType.Int64, sysId);

                using (IDataReader dataReader = mappingStoreDb.ExecuteReader(command))
                {
                    // GetOrdinal positions
                    int compIdIdx = dataReader.GetOrdinal("COMP_ID");
                    int type = dataReader.GetOrdinal("TYPE");
                    int isFreqDim = dataReader.GetOrdinal("IS_FREQ_DIM");
                    int isMeasureDim = dataReader.GetOrdinal("IS_MEASURE_DIM");
                    int attAssLevel = dataReader.GetOrdinal("ATT_ASS_LEVEL");
                    int attStatus = dataReader.GetOrdinal("ATT_STATUS");
                    int attIsTimeFormat = dataReader.GetOrdinal("ATT_IS_TIME_FORMAT");
                    int crossSectionalAttachmentlevelDataSet = dataReader.GetOrdinal("XS_ATTLEVEL_DS");
                    int attlevelGroup = dataReader.GetOrdinal("XS_ATTLEVEL_GROUP");
                    int attlevelSection = dataReader.GetOrdinal("XS_ATTLEVEL_SECTION");
                    int attlevelObs = dataReader.GetOrdinal("XS_ATTLEVEL_OBS");
                    int measureCode = dataReader.GetOrdinal("XS_MEASURE_CODE");
                    int gid = dataReader.GetOrdinal("GID");
                    int cid = dataReader.GetOrdinal("CID");
                    while (dataReader.Read())
                    {
                        long compSysId = DataReaderHelper.GetInt64(dataReader, compIdIdx);

                        // check if new populate
                        ComponentEntity component;
                        if (!componentsMap.TryGetValue(compSysId, out component))
                        {
                            component = new ComponentEntity(compSysId);
                            component.Id = DataReaderHelper.GetString(dataReader, cid);
                            component.SetType(DataReaderHelper.GetString(dataReader, type));
                            component.CrossSectionalLevelDataSet = DataReaderHelper.GetBoolean(
                                dataReader, crossSectionalAttachmentlevelDataSet);
                            component.CrossSectionalLevelGroup = DataReaderHelper.GetBoolean(dataReader, attlevelGroup);
                            component.CrossSectionalLevelSection = DataReaderHelper.GetBoolean(
                                dataReader, attlevelSection);
                            component.CrossSectionalLevelObs = DataReaderHelper.GetBoolean(dataReader, attlevelObs);
                            component.CrossSectionalMeasureCode = DataReaderHelper.GetString(dataReader, measureCode);
                            switch (component.ComponentType)
                            {
                                case SdmxComponentType.Dimension:
                                    component.FrequencyDimension = DataReaderHelper.GetBoolean(dataReader, isFreqDim);
                                    component.MeasureDimension = DataReaderHelper.GetBoolean(dataReader, isMeasureDim);
                                    dsd.Dimensions.Add(component);
                                    break;
                                case SdmxComponentType.Attribute:
                                    component.SetAttachmentLevel(DataReaderHelper.GetString(dataReader, attAssLevel));
                                    component.SetAssignmentStatus(DataReaderHelper.GetString(dataReader, attStatus));
                                    component.AttTimeFormat = DataReaderHelper.GetBoolean(dataReader, attIsTimeFormat);
                                    dsd.Attributes.Add(component);
                                    break;
                                case SdmxComponentType.TimeDimension:
                                    dsd.TimeDimension = component;
                                    break;
                                case SdmxComponentType.PrimaryMeasure:
                                    dsd.PrimaryMeasure = component;
                                    break;
                                case SdmxComponentType.CrossSectionalMeasure:
                                    dsd.CrossSectionalMeasures.Add(component);
                                    break;
                            }

                            componentsMap.Add(compSysId, component);
                        }

                        // populate for current groups the related assigned groups if any
                        id = DataReaderHelper.GetString(dataReader, gid);
                        if (!string.IsNullOrEmpty(id))
                        {
                            group = groupsFromId[id];
                            component.AttAssignmentGroups.Add(group);
                        }

                        // if component is a dimension used in group definition, add it in their object
                        List<GroupEntity> groupFromDim;
                        if (groupsFromDim.TryGetValue(compSysId, out groupFromDim))
                        {
                            foreach (GroupEntity groupsItem in groupFromDim)
                            {
                                groupsItem.Dimensions.Add(component);
                            }
                        }
                    }
                }
            }

            foreach (var kv in componentsMap)
            {
                kv.Value.Concept = GetConceptByComponentSysId(mappingStoreDb, kv.Key);
                kv.Value.CodeList = GetCodeListByComponentSysId(mappingStoreDb, kv.Key);
            }
        }

        #endregion
    }
}