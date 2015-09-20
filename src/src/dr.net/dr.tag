<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>
<tagfile>
  <compound kind="namespace">
    <name>Estat</name>
    <filename>a00059.html</filename>
    <namespace>Estat::Nsi</namespace>
  </compound>
  <compound kind="namespace">
    <name>Estat::Nsi</name>
    <filename>a00060.html</filename>
    <namespace>Estat::Nsi::DataRetriever</namespace>
  </compound>
  <compound kind="namespace">
    <name>Estat::Nsi::DataRetriever</name>
    <filename>a00061.html</filename>
    <namespace>Estat::Nsi::DataRetriever::Builders</namespace>
    <namespace>Estat::Nsi::DataRetriever::Engines</namespace>
    <namespace>Estat::Nsi::DataRetriever::Manager</namespace>
    <namespace>Estat::Nsi::DataRetriever::Model</namespace>
    <namespace>Estat::Nsi::DataRetriever::Properties</namespace>
    <class kind="class">Estat::Nsi::DataRetriever::DataRetrieverCore</class>
    <class kind="class">Estat::Nsi::DataRetriever::DataRetrieverException</class>
    <class kind="class">Estat::Nsi::DataRetriever::DataRetrieverHelper</class>
    <class kind="class">Estat::Nsi::DataRetriever::ErrorTypes</class>
    <class kind="interface">Estat::Nsi::DataRetriever::IDataRetrieverTabular</class>
  </compound>
  <compound kind="class">
    <name>Estat::Nsi::DataRetriever::DataRetrieverCore</name>
    <filename>a00001.html</filename>
    <base>Estat::Nsi::DataRetriever::IDataRetrieverTabular</base>
    <member kind="function">
      <type></type>
      <name>DataRetrieverCore</name>
      <anchorfile>a00001.html</anchorfile>
      <anchor>a743376c3fd3ffe53083a15d2434bc9a2</anchor>
      <arglist>(ConnectionStringSettings connectionStringSettings)</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>DataRetrieverCore</name>
      <anchorfile>a00001.html</anchorfile>
      <anchor>a49f3851db4a06dec3261c71f3d13427e</anchor>
      <arglist>(IHeader defaultHeader, ConnectionStringSettings connectionStringSettings)</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>DataRetrieverCore</name>
      <anchorfile>a00001.html</anchorfile>
      <anchor>a0c2965ebd47148b4cd218dbec47bdc67</anchor>
      <arglist>(IHeader defaultHeader, ConnectionStringSettings connectionStringSettings, SdmxSchemaEnumType sdmxSchemaVersion)</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>ExecuteSqlQuery</name>
      <anchorfile>a00001.html</anchorfile>
      <anchor>a9dff8fb195040867df22e42ccd476bd8</anchor>
      <arglist>(IDataQuery query, string disseminationDbSql, int limit, ITabularWriter writer)</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>ExecuteSqlQuery</name>
      <anchorfile>a00001.html</anchorfile>
      <anchor>a281113bbfb35e049bc99a54b0de3b2e9</anchor>
      <arglist>(IComplexDataQuery query, string disseminationDbSql, int limit, ITabularWriter writer)</arglist>
    </member>
    <member kind="function">
      <type>string</type>
      <name>GenerateSqlQuery</name>
      <anchorfile>a00001.html</anchorfile>
      <anchor>ac83bf72921d9e1c973f6bd918079b22d</anchor>
      <arglist>(IDataQuery query)</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>GetData</name>
      <anchorfile>a00001.html</anchorfile>
      <anchor>ad9dc5b24c6f9f03e7c2f2214bee637e6</anchor>
      <arglist>(IDataQuery dataQuery, IDataWriterEngine dataWriter)</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>GetData</name>
      <anchorfile>a00001.html</anchorfile>
      <anchor>ae0c75682b12f4d699b200d60e0f5b139</anchor>
      <arglist>(IDataQuery dataQuery, ICrossSectionalWriterEngine dataWriter)</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>GetData</name>
      <anchorfile>a00001.html</anchorfile>
      <anchor>af89b3bb402068b99539781a1321ddbd8</anchor>
      <arglist>(IComplexDataQuery dataQuery, IDataWriterEngine dataWriter)</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>RetrieveData</name>
      <anchorfile>a00001.html</anchorfile>
      <anchor>a73a674366d1ca5b30aa8957bbb494aa0</anchor>
      <arglist>(IDataQuery query, ITabularWriter writer, bool showOriginal)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Nsi::DataRetriever::DataRetrieverException</name>
    <filename>a00002.html</filename>
    <member kind="function">
      <type></type>
      <name>DataRetrieverException</name>
      <anchorfile>a00002.html</anchorfile>
      <anchor>a036b9617ec5fc6723328e717bf2343f8</anchor>
      <arglist>(Exception nestedException, SdmxErrorCode errorCode, string message)</arglist>
    </member>
    <member kind="function">
      <type></type>
      <name>DataRetrieverException</name>
      <anchorfile>a00002.html</anchorfile>
      <anchor>ab6cc54df66c12d374581c23f0cdf0695</anchor>
      <arglist>(string errorMessage, SdmxErrorCode errorCode)</arglist>
    </member>
    <member kind="function">
      <type>override void</type>
      <name>GetObjectData</name>
      <anchorfile>a00002.html</anchorfile>
      <anchor>a96b48f3ad49fe0e552b255556eb99a26</anchor>
      <arglist>(SerializationInfo info, StreamingContext context)</arglist>
    </member>
    <member kind="property">
      <type>override string</type>
      <name>ErrorType</name>
      <anchorfile>a00002.html</anchorfile>
      <anchor>a78ccb5334a158f2ed4a43013544f73a5</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Nsi::DataRetriever::DataRetrieverHelper</name>
    <filename>a00003.html</filename>
    <member kind="function" static="yes">
      <type>static IDataflowObject</type>
      <name>GetDataflowFromQuery</name>
      <anchorfile>a00003.html</anchorfile>
      <anchor>adbe879041af700773210651d69b9b2e0</anchor>
      <arglist>(IDataQuery query)</arglist>
    </member>
  </compound>
  <compound kind="class">
    <name>Estat::Nsi::DataRetriever::ErrorTypes</name>
    <filename>a00004.html</filename>
    <member kind="property" static="yes">
      <type>static global::System.Resources.ResourceManager</type>
      <name>ResourceManager</name>
      <anchorfile>a00004.html</anchorfile>
      <anchor>af13965cbab670aa70a9caeca19f220e4</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static global::System.Globalization.CultureInfo</type>
      <name>Culture</name>
      <anchorfile>a00004.html</anchorfile>
      <anchor>acbb2176910901d0ce65ed10522d36df1</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="interface">
    <name>Estat::Nsi::DataRetriever::IDataRetrieverTabular</name>
    <filename>a00005.html</filename>
    <member kind="function">
      <type>void</type>
      <name>ExecuteSqlQuery</name>
      <anchorfile>a00005.html</anchorfile>
      <anchor>ae2373c1574f5bf44b62a3035e939d8a8</anchor>
      <arglist>(IDataQuery query, string disseminationDbSql, int limit, ITabularWriter writer)</arglist>
    </member>
    <member kind="function">
      <type>string</type>
      <name>GenerateSqlQuery</name>
      <anchorfile>a00005.html</anchorfile>
      <anchor>a6b74a6ea6628566c03d55acaa73db140</anchor>
      <arglist>(IDataQuery query)</arglist>
    </member>
    <member kind="function">
      <type>void</type>
      <name>RetrieveData</name>
      <anchorfile>a00005.html</anchorfile>
      <anchor>a8a771f06d201c34bd270133b0fadd358</anchor>
      <arglist>(IDataQuery query, ITabularWriter writer, bool showOriginal)</arglist>
    </member>
  </compound>
  <compound kind="namespace">
    <name>Estat::Nsi::DataRetriever::Builders</name>
    <filename>a00062.html</filename>
    <class kind="class">Estat::Nsi::DataRetriever::Builders::ComplexSqlBuilder</class>
    <class kind="class">Estat::Nsi::DataRetriever::Builders::CrossSectionalSqlBuilder</class>
    <class kind="class">Estat::Nsi::DataRetriever::Builders::DDbConnectionBuilder</class>
    <class kind="class">Estat::Nsi::DataRetriever::Builders::HeaderBuilder</class>
    <class kind="interface">Estat::Nsi::DataRetriever::Builders::IBuilder&lt; T, TE &gt;</class>
    <class kind="interface">Estat::Nsi::DataRetriever::Builders::ISqlBuilder</class>
    <class kind="class">Estat::Nsi::DataRetriever::Builders::SeriesDataSetSqlBuilder</class>
    <class kind="class">Estat::Nsi::DataRetriever::Builders::SeriesGroupSqlBuilder</class>
    <class kind="class">Estat::Nsi::DataRetriever::Builders::SeriesOrderedDimensionBuilder</class>
    <class kind="class">Estat::Nsi::DataRetriever::Builders::SeriesSqlBuilder</class>
    <class kind="class">Estat::Nsi::DataRetriever::Builders::SqlBuilderBase</class>
    <class kind="class">Estat::Nsi::DataRetriever::Builders::SqlQuery</class>
    <class kind="class">Estat::Nsi::DataRetriever::Builders::TabularSqlBuilder</class>
  </compound>
  <compound kind="class">
    <name>Estat::Nsi::DataRetriever::Builders::SqlQuery</name>
    <filename>a00007.html</filename>
  </compound>
  <compound kind="namespace">
    <name>Estat::Nsi::DataRetriever::Engines</name>
    <filename>a00063.html</filename>
    <class kind="class">Estat::Nsi::DataRetriever::Engines::CrossSectionalDataQueryEngineBase</class>
    <class kind="class">Estat::Nsi::DataRetriever::Engines::CrossSectionalMeasuresDataQueryEngine</class>
    <class kind="class">Estat::Nsi::DataRetriever::Engines::CrossSectionalMeasuresMappedDataQueryEngine</class>
    <class kind="class">Estat::Nsi::DataRetriever::Engines::CrossSectionalPrimaryDataQueryEngine</class>
    <class kind="class">Estat::Nsi::DataRetriever::Engines::DataQueryEngineBase&lt; TDataRetrievalInfo, TMappedValues &gt;</class>
    <class kind="interface">Estat::Nsi::DataRetriever::Engines::IDataQueryEngine&lt; in T &gt;</class>
    <class kind="class">Estat::Nsi::DataRetriever::Engines::SeriesDataQueryEngine</class>
    <class kind="class">Estat::Nsi::DataRetriever::Engines::SeriesDataQueryEngineBase</class>
    <class kind="class">Estat::Nsi::DataRetriever::Engines::SeriesDataQueryEngineXsMeasureBuffered</class>
    <class kind="class">Estat::Nsi::DataRetriever::Engines::TabularDataOriginalQueryEngine</class>
    <class kind="class">Estat::Nsi::DataRetriever::Engines::TabularDataQueryEngine</class>
    <class kind="class">Estat::Nsi::DataRetriever::Engines::TabularDataQueryEngineBase</class>
  </compound>
  <compound kind="namespace">
    <name>Estat::Nsi::DataRetriever::Manager</name>
    <filename>a00064.html</filename>
    <class kind="class">Estat::Nsi::DataRetriever::Manager::CrossSectionalQueryEngineManager</class>
    <class kind="interface">Estat::Nsi::DataRetriever::Manager::IQueryEngineManager&lt; in T &gt;</class>
    <class kind="class">Estat::Nsi::DataRetriever::Manager::SeriesQueryEngineManager</class>
  </compound>
  <compound kind="namespace">
    <name>Estat::Nsi::DataRetriever::Model</name>
    <filename>a00065.html</filename>
    <class kind="class">Estat::Nsi::DataRetriever::Model::ComponentValue</class>
    <class kind="class">Estat::Nsi::DataRetriever::Model::DataRetrievalInfo</class>
    <class kind="class">Estat::Nsi::DataRetriever::Model::DataRetrievalInfoComplex</class>
    <class kind="class">Estat::Nsi::DataRetriever::Model::DataRetrievalInfoSeries</class>
    <class kind="class">Estat::Nsi::DataRetriever::Model::DataRetrievalInfoTabular</class>
    <class kind="class">Estat::Nsi::DataRetriever::Model::DataRetrievalInfoXS</class>
    <class kind="class">Estat::Nsi::DataRetriever::Model::GroupInformation</class>
    <class kind="interface">Estat::Nsi::DataRetriever::Model::IMappedValues</class>
    <class kind="class">Estat::Nsi::DataRetriever::Model::MappedValues</class>
    <class kind="class">Estat::Nsi::DataRetriever::Model::MappedValuesBase</class>
    <class kind="class">Estat::Nsi::DataRetriever::Model::MappedValuesFlat</class>
    <class kind="class">Estat::Nsi::DataRetriever::Model::MappedXsValues</class>
    <class kind="class">Estat::Nsi::DataRetriever::Model::MaxObsStatus</class>
    <class kind="class">Estat::Nsi::DataRetriever::Model::ReadOnlyKey</class>
    <class kind="class">Estat::Nsi::DataRetriever::Model::XsMeasureCache</class>
  </compound>
  <compound kind="namespace">
    <name>Estat::Nsi::DataRetriever::Properties</name>
    <filename>a00066.html</filename>
    <class kind="class">Estat::Nsi::DataRetriever::Properties::Resources</class>
  </compound>
  <compound kind="class">
    <name>Estat::Nsi::DataRetriever::Properties::Resources</name>
    <filename>a00006.html</filename>
    <member kind="property" static="yes">
      <type>static global::System.Resources.ResourceManager</type>
      <name>ResourceManager</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a7f7e2fb76e45339f3add3ca1799024ae</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static global::System.Globalization.CultureInfo</type>
      <name>Culture</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a137b7ac323f0e590f9398a2ea0dec191</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>DataRetriever_ExecuteSqlQuery_Error_during_writing_responce</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a61f26d9b823d0fab0780e94650645c2e</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>DataRetriever_ExecuteSqlQuery_Error_executing_generated_SQL_and_populating_SDMX_model</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a9ddd86e539f023cd736eb8519ec5f85f</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>DataRetriever_GenerateComponentWhere_</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a3f4917db468690be0150e4b5be402e3d</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>DataRetriever_GenerateFrom_Begin_GenerateFrom____</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>aee942279ee9f449f52c80aaba882ff0a</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>DataRetriever_GenerateFrom_End_GenerateFrom____</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a4a49aaa1c8e5f363dbb2e1911c8beb1f</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>DataRetriever_GenerateFrom_Generated_FROM___0_</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a9feecd8f0f5665d2503b9dfefaadf70a</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>DataRetriever_GenerateSqlQuery_Could_not_generate_sql_query</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>ad9aa39ccc98cd04e06c713e236ffd291</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>DataRetriever_GetMessageHeader_Error_populating_header</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a297e5968328ca4083cc66ed457d5610e</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>DataRetriever_Initialize_Could_not_retrieve_Mappings_from__Mapping_Store___Cause__</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>aee938224032bb0d56f89c1fdbcbd133b</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>DataRetriever_Initialize_Error_while_retrieving_Mappings_from__Mapping_Store___Cause__</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a0e6d759c97f84ed4aa3b39893d7b854c</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>DataRetriever_RetrieveData_End_Data_Retriever_initialization</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a2a32307433cc7f3f826e22929a16d6f0</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>DataRetriever_RetrieveData_End_Retrieving_header_information</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>aeeadc83395e32e0c125a1f82e0765a9c</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>DataRetriever_RetrieveData_Query_executed_successfully_and_DataSet_populated</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>ad3e6ce18100e0b050b563f8ceee876a7</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>DataRetriever_RetrieveData_Start_Initializing_Data_Retriever</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>afcda6a2abac8ec13ce6269afa9815cc2</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>DataRetriever_RetrieveData_Start_Retrieving_header_information</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a054e408845cb00ba58950e92822a2043</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>DataRetriever_RetrieveDataTable_End_Executing_SQL_in_dissemination_database_and_populate_DataTable</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a9f0e1e28fe440e8bf33cb45676d97d17</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>DataRetriever_RetrieveDataTable_Query_executed_successfully_and_DataTable_populated</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>aea956f416502a88cdcb83f480b9f818e</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>DataRetriever_RetrieveDataTable_Start_Executing_SQL_in_dissemination_database_and_populate_DataTable</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a91f35cd36c29cb9cfd650e89f4bb0586</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>ErrorComponentValuesNotMappedValuesFlaType</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a9b2b7665a2b43753bca7f603698bcc8c</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>ErrorIncompleteMappingSet</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a55817075ede72ca51344a4eb3b1234e0</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>ErrorInfoNotDataRetrievalInfoTabularType</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a37d4415599eb45b45b3de0a88e16ca3d</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>ErrorNoHeader</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>af36384750aff2c0676da157864f3f42b</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>ErrorTypeNotDataRetrievalInfoSeries</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a48dd223a4a00f6f40f7477808bba1e3b</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>ErrorTypeNotMappedXsValues</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a6b4d694cc5af110835befcb702bf1136</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>ErrorUnableToGenerateSQL</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a8f51bd4760aa2186eb602660b6e300da</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>ExceptionNotInit</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>adbf2d406c0bc1569a031d808f275955e</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoBeginGenerateSelect</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a40f41fd56c5b6ef74987dce0b4d29034</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoBeginGenerateSql</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a3bd41a21b1493c185c401f0acb9bc4e9</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoBeginGenerateWhere</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a9c96849b18dc07bbb2d1e12a142c48ba</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoDataRetrieverBBInvoked</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>aa173f480d83ea564dcdb111f3036afda</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoEndDataRetrieverBBInvoked</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a0248b4e7cccdedd853e6351c7a6071a1</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoEndExecutingSql</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>adad218073b3a0498c840ae83ff3bf656</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoEndGenerateOrderBy</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a94801c247efdbc8f317b964eed2b14ef</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoEndGenerateSelect</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a587dd66536e7bc233018e71968aba51f</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoEndGenerateSql</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>aa675094dad2a25aa89b2cbb5b1aa38c1</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoEndGenerateWhere</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>af4c972f11896030df8ea67261f788695</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoEndGeneratingSQLDDB</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a6e605122ba2af3232f80034f77ee1616</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoGeneratedOrderByFormat1</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a3805451b1afab89a4d6908ab360b30ea</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoGeneratedSelectFormat1</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a9aa2b0fdf9857a976170abdac9436a66</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoGeneratedSQLFormat1</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a89bd6c55b0c03c9e007bcfb6ec594b2d</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoGeneratedWhereFormat1</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a4582c9981e34e1f226254c190ae8c373</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoGenerateOrderBy</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>ab67ca67f3738f43616644f9f87b9bc56</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoOutput</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>ac7b367520d8e811102c966697a314ed4</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoStartExecutingSql</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>ad6a3f1e9cd395b82e48085f2c6f2d32f</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>InfoStartGeneratingSQLDDB</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>ab60de4a2ac2819f0f9626ed8bf0ceb5b</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>MissingDataflowInQuery</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a5e31a9c2dd4550c85a62920b69dfb026</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>MissingFrequency</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a74c666188800fdf6e8ece4a6071260f3</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>No_header_information_in_the_Mapping_Store</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>afd83993c71e00cc5d4494b968785c463</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>NoDataWhereInQuery</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a84ca6c3bf692fcabaf6270c18393a6dd</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>NoMappingForDataflowFormat1</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a3c8a80922dafee8d468908f4963087ef</anchor>
      <arglist></arglist>
    </member>
    <member kind="property" static="yes">
      <type>static string</type>
      <name>NoRecordsFound</name>
      <anchorfile>a00006.html</anchorfile>
      <anchor>a52ca81ab412638ab3cd57bd99d5bb81c</anchor>
      <arglist></arglist>
    </member>
  </compound>
  <compound kind="dir">
    <name>DataRetriever/Builders</name>
    <path>F:/tmp/sources/dr.net/src/dr.net/src/DataRetriever/Builders/</path>
    <filename>dir_39740d52b181ccaf25fdf4a6fe1b51d7.html</filename>
    <file>ComplexSqlBuilder.cs</file>
    <file>CrossSectionalSqlBuilder.cs</file>
    <file>DDbConnectionBuilder.cs</file>
    <file>HeaderBuilder.cs</file>
    <file>IBuilder.cs</file>
    <file>ISqlBuilder.cs</file>
    <file>SeriesDataSetSqlBuilder.cs</file>
    <file>SeriesGroupSqlBuilder.cs</file>
    <file>SeriesOrderedDimensionBuilder.cs</file>
    <file>SeriesSqlBuilder.cs</file>
    <file>SqlBuilderBase.cs</file>
    <file>SqlQuery.cs</file>
    <file>TabularSqlBuilder.cs</file>
  </compound>
  <compound kind="dir">
    <name>DataRetriever</name>
    <path>F:/tmp/sources/dr.net/src/dr.net/src/DataRetriever/</path>
    <filename>dir_e57c04fb91eb79b5dc64ae0282e0e241.html</filename>
    <dir>DataRetriever/Builders</dir>
    <dir>DataRetriever/Engines</dir>
    <dir>DataRetriever/Manager</dir>
    <dir>DataRetriever/Model</dir>
    <dir>DataRetriever/Properties</dir>
    <file>DataRetrieverCore.cs</file>
    <file>DataRetrieverException.cs</file>
    <file>DataRetrieverHelper.cs</file>
    <file>ErrorTypes.Designer.cs</file>
    <file>IDataRetrieverTabular.cs</file>
  </compound>
  <compound kind="dir">
    <name>DataRetriever/Engines</name>
    <path>F:/tmp/sources/dr.net/src/dr.net/src/DataRetriever/Engines/</path>
    <filename>dir_752c70149ee107543aca0dc95ea4bfc5.html</filename>
    <file>CrossSectionalDataQueryEngineBase.cs</file>
    <file>CrossSectionalMeasuresDataQueryEngine.cs</file>
    <file>CrossSectionalMeasuresMappedDataQueryEngine.cs</file>
    <file>CrossSectionalPrimaryDataQueryEngine.cs</file>
    <file>DataQueryEngineBase.cs</file>
    <file>IDataQueryEngine.cs</file>
    <file>SeriesDataQueryEngine.cs</file>
    <file>SeriesDataQueryEngineBase.cs</file>
    <file>SeriesDataQueryEngineXsMeasureBuffered.cs</file>
    <file>TabularDataOriginalQueryEngine.cs</file>
    <file>TabularDataQueryEngine.cs</file>
    <file>TabularDataQueryEngineBase.cs</file>
  </compound>
  <compound kind="dir">
    <name>DataRetriever/Manager</name>
    <path>F:/tmp/sources/dr.net/src/dr.net/src/DataRetriever/Manager/</path>
    <filename>dir_6f847f6523d582c4352c6db64428325c.html</filename>
    <file>CrossSectionalQueryEngineManager.cs</file>
    <file>IQueryEngineManager.cs</file>
    <file>SeriesQueryEngineManager.cs</file>
  </compound>
  <compound kind="dir">
    <name>DataRetriever/Model</name>
    <path>F:/tmp/sources/dr.net/src/dr.net/src/DataRetriever/Model/</path>
    <filename>dir_6c102bc501336d3a4a75356459993060.html</filename>
    <file>ComponentValue.cs</file>
    <file>DataRetrievalInfo.cs</file>
    <file>DataRetrievalInfoComplex.cs</file>
    <file>DataRetrievalInfoSeries.cs</file>
    <file>DataRetrievalInfoTabular.cs</file>
    <file>DataRetrievalInfoXS.cs</file>
    <file>GroupInformation.cs</file>
    <file>IMappedValues.cs</file>
    <file>MappedValues.cs</file>
    <file>MappedValuesBase.cs</file>
    <file>MappedValuesFlat.cs</file>
    <file>MappedXsValues.cs</file>
    <file>MaxObsStatus.cs</file>
    <file>ReadOnlyKey.cs</file>
    <file>XsMeasureCache.cs</file>
  </compound>
  <compound kind="dir">
    <name>DataRetriever/Properties</name>
    <path>F:/tmp/sources/dr.net/src/dr.net/src/DataRetriever/Properties/</path>
    <filename>dir_ac062ecedf3362fe75fa26952b791323.html</filename>
    <file>AssemblyInfo.cs</file>
    <file>AssemblyInfo.log4net.cs</file>
    <file>Resources.Designer.cs</file>
  </compound>
</tagfile>
