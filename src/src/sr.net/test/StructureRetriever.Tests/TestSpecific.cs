// -----------------------------------------------------------------------
// <copyright file="TestSpecific.cs" company="Eurostat">
//   Date Created : 2014-05-27
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace StructureRetriever.Tests
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Estat.Nsi.StructureRetriever.Factory;
    using Estat.Sdmxsource.Extension.Manager;
    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Factory;

    using log4net;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference;

    [TestFixture("sqlserver")]
    [TestFixture("sqlserver2")]
    [TestFixture("odp")]
    [TestFixture("odp2")]
    //[TestFixture("msoracle")]
    [TestFixture("mysql")]
    public class TestSpecific
    {

        /// <summary>
        ///     The _from mutable.
        /// </summary>
        private static readonly StructureReferenceFromMutableBuilder _fromMutable = new StructureReferenceFromMutableBuilder();

        /// <summary>
        ///     The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(TestSpecific));
        /// <summary>
        ///     The _AUTH mutable structure search manager factory.
        /// </summary>
        private readonly IStructureSearchManagerFactory<IAuthMutableStructureSearchManager> _authMutableStructureSearchManagerFactory = new AuthMutableStructureSearchManagerFactory();

        /// <summary>
        ///     The _mutable structure search manager factory.
        /// </summary>
        private readonly IMutableStructureSearchManagerFactory _mutableStructureSearchManagerFactory = new MutableStructureSearchManagerFactory();

        /// <summary>
        /// The _connection string.
        /// </summary>
        private readonly ConnectionStringSettings _connectionString;

        /// <summary>
        /// The _full retrieval manager
        /// </summary>
        private readonly ISdmxMutableObjectRetrievalManager _fullRetrievalManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestSpecific"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public TestSpecific(string name)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings [name];
            this._fullRetrievalManager = new MutableRetrievalManagerFactory().GetRetrievalManager(this._connectionString);
        }

        [TestCase(SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.Categorisation)]
        public void TestQueryCodelistUsedByDsd(SdmxStructureEnumType sdmxStructure)
        {
            var structureSearchManager = this._mutableStructureSearchManagerFactory.GetStructureSearchManager(this._connectionString, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));
            foreach (var codelistMutableObject in this.GetCodelistUsedByDsd())
            {
                var specificStructureReference = SdmxStructureType.GetFromEnum(sdmxStructure);
                var mutableObjects = structureSearchManager.GetMaintainables(
                    new RESTStructureQueryCore(
                        StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.Full),
                        StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.Specific),
                        specificStructureReference,
                        codelistMutableObject.ImmutableInstance.AsReference,
                        false));
                Assert.IsNotEmpty(mutableObjects.GetMaintainables(specificStructureReference), _fromMutable.Build(codelistMutableObject).ToString());
            }
        }

        [TestCase(SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.Categorisation)]
        public void TestQueryCodelistUsedByDsdStub(SdmxStructureEnumType sdmxStructure)
        {
            var structureSearchManager = this._mutableStructureSearchManagerFactory.GetStructureSearchManager(this._connectionString, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));
            foreach (var codelistMutableObject in this.GetCodelistUsedByDsd())
            {
                var specificStructureReference = SdmxStructureType.GetFromEnum(sdmxStructure);
                var mutableObjects = structureSearchManager.GetMaintainables(
                    new RESTStructureQueryCore(
                        StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.AllStubs),
                        StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.Specific),
                        specificStructureReference,
                        codelistMutableObject.ImmutableInstance.AsReference,
                        false));
                Assert.IsNotEmpty(mutableObjects.GetMaintainables(specificStructureReference));
            }
        }

        private ISet<ICodelistMutableObject> GetCodelistUsedByDsd()
        {
            var structureSearchManager = this._mutableStructureSearchManagerFactory.GetStructureSearchManager(this._connectionString, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));
            var mutableObjects = structureSearchManager.GetMaintainables(new RESTStructureQueryCore("/categorisation/all/all/all?references=codelist&detail=allstubs"));
            return mutableObjects.Codelists;
        }
    }
}