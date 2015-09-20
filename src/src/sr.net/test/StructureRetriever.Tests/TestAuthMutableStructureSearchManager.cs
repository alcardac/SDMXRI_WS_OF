// -----------------------------------------------------------------------
// <copyright file="TestAuthMutableStructureSearchManager.cs" company="Eurostat">
//   Date Created : 2014-06-11
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace StructureRetriever.Tests
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;

    using Estat.Nsi.StructureRetriever.Factory;
    using Estat.Sdmxsource.Extension.Manager;
    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Factory;

    using log4net;

    using Moq;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     Test unit for <see cref="StructureReferenceFromMutableBuilder" />
    /// </summary>
    [TestFixture("sqlserver")]
    [TestFixture("sqlserver2")]
    [TestFixture("odp")]
    [TestFixture("odp2")]
    //// [TestFixture("msoracle")]
    [TestFixture("mysql")]
    public class TestAuthMutableStructureSearchManager
    {
        /// <summary>
        ///     The _from mutable.
        /// </summary>
        private static readonly StructureReferenceFromMutableBuilder _fromMutable = new StructureReferenceFromMutableBuilder();

        /// <summary>
        ///     The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(TestAuthMutableStructureSearchManager));

        /// <summary>
        ///     The _AUTH mutable structure search manager factory.
        /// </summary>
        private readonly IStructureSearchManagerFactory<IAuthMutableStructureSearchManager> _authMutableStructureSearchManagerFactory = new AuthMutableStructureSearchManagerFactory();

        /// <summary>
        /// The _connection string.
        /// </summary>
        private readonly ConnectionStringSettings _connectionString;

        /// <summary>
        /// The _full retrieval manager
        /// </summary>
        private readonly ISdmxMutableObjectRetrievalManager _fullRetrievalManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAuthMutableStructureSearchManager"/> class.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        public TestAuthMutableStructureSearchManager(string connectionName)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings[connectionName];
            this._fullRetrievalManager = new MutableRetrievalManagerFactory().GetRetrievalManager(this._connectionString);
        }

        /// <summary>
        /// Test unit for <see cref="IMutableStructureSearchManager.GetLatest"/>
        /// </summary>
        /// <param name="structureEnumType">
        /// The structure type
        /// </param>
        [TestCase(SdmxStructureEnumType.Dataflow, ExpectedException = typeof(SdmxNoResultsException))]
        public void TestGetLatestAuthNoResult(SdmxStructureEnumType structureEnumType)
        {
            var structureType = SdmxStructureType.GetFromEnum(structureEnumType);

            var mutableSearchManager = this.GetAuthMutableSearchManager(this._connectionString);
            IStructureReference reference = new StructureReferenceImpl(new MaintainableRefObjectImpl(), structureType);
            switch (structureType.EnumType)
            {
                case SdmxStructureEnumType.Dataflow:
                    var list = this._fullRetrievalManager.GetMutableMaintainables(reference, false, false);

                    foreach (var maintainableMutableObject in list)
                    {
                        var mutableObject = mutableSearchManager.GetLatest(maintainableMutableObject, new IMaintainableRefObject[0]);
                        Assert.IsNull(mutableObject, _fromMutable.Build(maintainableMutableObject).ToString());
                    }

                    break;
            }
        }

        /// <summary>
        /// Test unit for <see cref="IMutableStructureSearchManager.GetLatest"/>
        /// </summary>
        /// <param name="structureEnumType">
        /// The structure type
        /// </param>
        [TestCase(SdmxStructureEnumType.Dataflow)]
        public void TestGetLatestAuthResult(SdmxStructureEnumType structureEnumType)
        {
            var structureType = SdmxStructureType.GetFromEnum(structureEnumType);
            var mutableSearchManager = this.GetAuthMutableSearchManager(this._connectionString);
            IStructureReference reference = new StructureReferenceImpl(new MaintainableRefObjectImpl(), structureType);
            switch (structureType.EnumType)
            {
                case SdmxStructureEnumType.Dataflow:
                    var list = this._fullRetrievalManager.GetMutableMaintainables(reference, false, false);
                    foreach (var maintainableMutableObject in list)
                    {
                        IMaintainableRefObject maintainableRefObject = new MaintainableRefObjectImpl(maintainableMutableObject.AgencyId, maintainableMutableObject.Id, null);

                        var mutableObject = mutableSearchManager.GetLatest(maintainableMutableObject, new[] { maintainableRefObject });
                        Assert.IsNotNull(mutableObject, _fromMutable.Build(maintainableMutableObject).ToString());
                    }

                    break;
            }
        }

        /// <summary>
        /// Test unit for <see cref="IMutableStructureSearchManager.GetMaintainables"/>
        /// </summary>
        /// <param name="structureEnumType">
        /// The structure ENUM Type.
        /// </param>
        /// <param name="detail">
        /// The detail.
        /// </param>
        /// <param name="refDetailEnum">
        /// The ref Detail ENUM.
        /// </param>
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.None, ExpectedException  = typeof(SdmxNoResultsException))]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Parents, ExpectedException = typeof(SdmxNoResultsException))]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Children, ExpectedException = typeof(SdmxNoResultsException))]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, ExpectedException = typeof(SdmxNoResultsException))]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.None, ExpectedException = typeof(SdmxNoResultsException))]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Parents, ExpectedException = typeof(SdmxNoResultsException))]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Children, ExpectedException = typeof(SdmxNoResultsException))]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, ExpectedException = typeof(SdmxNoResultsException))]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.None, ExpectedException = typeof(SdmxNoResultsException))]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Parents, ExpectedException = typeof(SdmxNoResultsException))]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Children, ExpectedException = typeof(SdmxNoResultsException))]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, ExpectedException = typeof(SdmxNoResultsException))]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Parents)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Parents)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Parents)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific)]
        public void TestGetMaintainablesAuthNoResult(SdmxStructureEnumType structureEnumType, StructureQueryDetailEnumType detail, StructureReferenceDetailEnumType refDetailEnum)
        {
            var mutableSearchManager = this.GetAuthMutableSearchManager(this._connectionString);
            var refDetail = StructureReferenceDetail.GetFromEnum(refDetailEnum);
            var structureType = SdmxStructureType.GetFromEnum(structureEnumType);
            var allowedDataflows = new IMaintainableRefObject[0];
            IStructureReference reference = new StructureReferenceImpl(new MaintainableRefObjectImpl(), structureType);
            switch (structureType.EnumType)
            {
                case SdmxStructureEnumType.CategoryScheme:
                case SdmxStructureEnumType.CodeList:
                case SdmxStructureEnumType.ConceptScheme:
                case SdmxStructureEnumType.Dataflow:
                case SdmxStructureEnumType.Dsd:
                case SdmxStructureEnumType.HierarchicalCodelist:
                    var list = this._fullRetrievalManager.GetMutableMaintainables(reference, false, false);
                    var queryMetadataMock = new Mock<IStructureQueryMetadata>();
                    queryMetadataMock.Setup(query => query.IsReturnLatest).Returns(false);
                    queryMetadataMock.Setup(query => query.SpecificStructureReference).Returns((SdmxStructureType)null);
                    queryMetadataMock.Setup(query => query.StructureReferenceDetail).Returns(refDetail);
                    queryMetadataMock.Setup(query => query.StructureQueryDetail).Returns(StructureQueryDetail.GetFromEnum(detail));

                    var mockQuery = new Mock<IRestStructureQuery>();
                    mockQuery.Setup(query => query.StructureReference).Returns(reference);
                    mockQuery.Setup(query => query.StructureQueryMetadata).Returns(() => queryMetadataMock.Object);
                    _log.DebugFormat(CultureInfo.InvariantCulture, "Structure type: {0}, Detail: {1}, Reference : {2}", structureType, detail, refDetail);
                    foreach (var maintainableMutableObject in list)
                    {
                        switch (refDetail.EnumType)
                        {
                            case StructureReferenceDetailEnumType.Null:
                                break;
                            case StructureReferenceDetailEnumType.None:
                                {
                                    var mutableObject = mutableSearchManager.GetMaintainables(mockQuery.Object, allowedDataflows);
                                    Assert.IsNotNull(mutableObject, _fromMutable.Build(maintainableMutableObject).ToString());
                                    Assert.IsEmpty(mutableObject.AllMaintainables, _fromMutable.Build(maintainableMutableObject).ToString());
                                }

                                break;
                            case StructureReferenceDetailEnumType.Parents:
                                {
                                    var mutableObject = mutableSearchManager.GetMaintainables(mockQuery.Object, allowedDataflows);
                                    Assert.IsNotNull(mutableObject, _fromMutable.Build(maintainableMutableObject).ToString());
                                    Assert.IsEmpty(mutableObject.Dataflows, _fromMutable.Build(maintainableMutableObject).ToString());
                                }

                                break;
                            case StructureReferenceDetailEnumType.ParentsSiblings:
                                break;
                            case StructureReferenceDetailEnumType.Children:
                                {
                                    var mutableObject = mutableSearchManager.GetMaintainables(mockQuery.Object, allowedDataflows);
                                    Assert.IsNotNull(mutableObject, _fromMutable.Build(maintainableMutableObject).ToString());
                                    Assert.IsEmpty(mutableObject.AllMaintainables, _fromMutable.Build(maintainableMutableObject).ToString());
                                }

                                break;
                            case StructureReferenceDetailEnumType.Descendants:
                                break;
                            case StructureReferenceDetailEnumType.All:
                                break;
                            case StructureReferenceDetailEnumType.Specific:
                                {
                                    queryMetadataMock.Setup(query => query.StructureReferenceDetail).Returns(StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.None));
                                    var mutableObject = mutableSearchManager.GetMaintainables(mockQuery.Object, allowedDataflows);
                                    queryMetadataMock.Setup(query => query.StructureReferenceDetail).Returns(refDetail);
                                    Assert.IsNotNull(mutableObject, _fromMutable.Build(maintainableMutableObject).ToString());
                                    if (structureType.EnumType == SdmxStructureEnumType.Dsd)
                                    {
                                        Assert.IsEmpty(mutableObject.Dataflows, _fromMutable.Build(maintainableMutableObject).ToString());
                                    }
                                    else
                                    {
                                        Assert.IsEmpty(mutableObject.AllMaintainables, _fromMutable.Build(maintainableMutableObject).ToString());
                                    }
                                }

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// Test unit for <see cref="IMutableStructureSearchManager.GetMaintainables"/>
        /// </summary>
        /// <param name="structureEnumType">
        /// The structure ENUM Type.
        /// </param>
        /// <param name="detail">
        /// The detail.
        /// </param>
        /// <param name="refDetailEnum">
        /// The ref Detail ENUM.
        /// </param>
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.None)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Parents)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Children)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.None)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Parents)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Children)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.None)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Parents)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Children)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Parents)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Parents)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Parents)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific)]
        public void TestGetMaintainablesAuthResult(SdmxStructureEnumType structureEnumType, StructureQueryDetailEnumType detail, StructureReferenceDetailEnumType refDetailEnum)
        {
            var mutableSearchManager = this.GetAuthMutableSearchManager(this._connectionString);
            var refDetail = StructureReferenceDetail.GetFromEnum(refDetailEnum);
            var structureType = SdmxStructureType.GetFromEnum(structureEnumType);
            IStructureReference reference = new StructureReferenceImpl(new MaintainableRefObjectImpl(), structureType);
            switch (structureType.EnumType)
            {
                case SdmxStructureEnumType.Dataflow:
                case SdmxStructureEnumType.Dsd:
                    var allowedDataflows = (from x in this._fullRetrievalManager.GetMutableDataflowObjects(new MaintainableRefObjectImpl(), false, false) select _fromMutable.Build(x).MaintainableReference).ToArray();

                    var list = this._fullRetrievalManager.GetMutableMaintainables(reference, false, false);
                    var queryMetadataMock = new Mock<IStructureQueryMetadata>();
                    queryMetadataMock.Setup(query => query.IsReturnLatest).Returns(false);
                    queryMetadataMock.Setup(query => query.SpecificStructureReference).Returns((SdmxStructureType)null);
                    queryMetadataMock.Setup(query => query.StructureReferenceDetail).Returns(refDetail);
                    queryMetadataMock.Setup(query => query.StructureQueryDetail).Returns(StructureQueryDetail.GetFromEnum(detail));

                    var mockQuery = new Mock<IRestStructureQuery>();
                    mockQuery.Setup(query => query.StructureReference).Returns(reference);
                    mockQuery.Setup(query => query.StructureQueryMetadata).Returns(() => queryMetadataMock.Object);
                    _log.DebugFormat(CultureInfo.InvariantCulture, "Structure type: {0}, Detail: {1}, Reference : {2}", structureType, detail, refDetail);
                    foreach (var maintainableMutableObject in list)
                    {
                        switch (refDetail.EnumType)
                        {
                            case StructureReferenceDetailEnumType.Null:
                                break;
                            case StructureReferenceDetailEnumType.None:
                                {
                                    var mutableObject = mutableSearchManager.GetMaintainables(mockQuery.Object, allowedDataflows);
                                    Assert.IsNotNull(mutableObject, _fromMutable.Build(maintainableMutableObject).ToString());
                                    Assert.IsNotEmpty(mutableObject.AllMaintainables, _fromMutable.Build(maintainableMutableObject).ToString());
                                }

                                break;
                            case StructureReferenceDetailEnumType.Parents:
                                {
                                    var mutableObject = mutableSearchManager.GetMaintainables(mockQuery.Object, allowedDataflows);
                                    Assert.IsNotNull(mutableObject, _fromMutable.Build(maintainableMutableObject).ToString());
                                    Assert.IsNotEmpty(mutableObject.AllMaintainables, _fromMutable.Build(maintainableMutableObject).ToString());
                                }

                                break;
                            case StructureReferenceDetailEnumType.ParentsSiblings:
                                break;
                            case StructureReferenceDetailEnumType.Children:
                                {
                                    var mutableObject = mutableSearchManager.GetMaintainables(mockQuery.Object, allowedDataflows);
                                    Assert.IsNotNull(mutableObject, _fromMutable.Build(maintainableMutableObject).ToString());
                                    Assert.IsNotEmpty(mutableObject.AllMaintainables, _fromMutable.Build(maintainableMutableObject).ToString());
                                }

                                break;
                            case StructureReferenceDetailEnumType.Descendants:
                                break;
                            case StructureReferenceDetailEnumType.All:
                                break;
                            case StructureReferenceDetailEnumType.Specific:
                                {
                                    queryMetadataMock.Setup(query => query.StructureReferenceDetail).Returns(StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.None));
                                    var mutableObject = mutableSearchManager.GetMaintainables(mockQuery.Object, allowedDataflows);
                                    queryMetadataMock.Setup(query => query.StructureReferenceDetail).Returns(refDetail);
                                    Assert.IsNotNull(mutableObject, _fromMutable.Build(maintainableMutableObject).ToString());
                                    Assert.IsNotEmpty(mutableObject.AllMaintainables, _fromMutable.Build(maintainableMutableObject).ToString());
                                }

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    break;
            }
        }

        /// <summary>
        ///     The get schema.
        /// </summary>
        /// <returns>
        ///     The <see cref="SdmxSchema" />.
        /// </returns>
        protected virtual SdmxSchema GetSchema()
        {
            return SdmxSchema.GetFromEnum(SdmxSchemaEnumType.Null);
        }

        /// <summary>
        /// Gets the mutable search manager.
        /// </summary>
        /// <param name="settings">
        /// The connection string settings.
        /// </param>
        /// <returns>
        /// The <see cref="StructureReferenceFromMutableBuilder"/>.
        /// </returns>
        protected virtual IAuthMutableStructureSearchManager GetAuthMutableSearchManager(ConnectionStringSettings settings)
        {
            return this._authMutableStructureSearchManagerFactory.GetStructureSearchManager(settings, this.GetSchema());
        }
    }
}