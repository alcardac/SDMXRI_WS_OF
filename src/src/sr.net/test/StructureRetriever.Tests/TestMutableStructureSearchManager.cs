// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestMutableStructureSearchManager.cs" company="Eurostat">
//   Date Created : 2013-09-20
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Test unit for <see cref="StructureReferenceFromMutableBuilder" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace StructureRetriever.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;

    using Estat.Nsi.StructureRetriever.Factory;
    using Estat.Sdmxsource.Extension.Manager;
    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Factory;

    using log4net;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     Test unit for <see cref="StructureReferenceFromMutableBuilder" />
    /// </summary>
    [TestFixture("sqlserver")]
    [TestFixture("sqlserver2")]
    [TestFixture("odp")]
    [TestFixture("odp2")]
    ////[TestFixture("msoracle")]
    [TestFixture("mysql")]
    public class TestMutableStructureSearchManager
    {
        #region Static Fields

        /// <summary>
        ///     The _from mutable.
        /// </summary>
        private static readonly StructureReferenceFromMutableBuilder _fromMutable = new StructureReferenceFromMutableBuilder();

        /// <summary>
        ///     The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(TestMutableStructureSearchManager));

        /// <summary>
        ///     The _reference set builder.
        /// </summary>
        private static readonly ICrossReferenceSetBuilder _referenceSetBuilder = new CrossReferenceChildBuilder();

        #endregion

        #region Fields

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

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TestMutableStructureSearchManager"/> class. 
        /// </summary>
        /// <param name="connectionName">
        /// The name.
        /// </param>
        public TestMutableStructureSearchManager(string connectionName)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings[connectionName];
            this._fullRetrievalManager = new MutableRetrievalManagerFactory().GetRetrievalManager(this._connectionString);
        }

        #region Public Methods and Operators

        /// <summary>
        /// Test unit for <see cref="IMutableStructureSearchManager.GetLatest"/>
        /// </summary>
        /// <param name="structureEnumType">
        /// The structure type
        /// </param>
        [TestCase(SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.CategoryScheme)]
        public void TestGetLatest(SdmxStructureEnumType structureEnumType)
        {
            var structureType = SdmxStructureType.GetFromEnum(structureEnumType);
            var mutableSearchManager = this.GetMutableSearchManager(this._connectionString);
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
                    foreach (var maintainableMutableObject in list)
                    {
                        var mutableObject = mutableSearchManager.GetLatest(maintainableMutableObject);
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
        /// <param name="specificStructure">
        /// The specific structure.
        /// </param>
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.Categorisation, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.Dataflow, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.HierarchicalCodelist, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.CodeList, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.ConceptScheme, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.Full, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.ReferencedStubs, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.None, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Parents, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.ParentsSiblings, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Descendants, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Children, SdmxStructureEnumType.Null)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Categorisation)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dataflow)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.HierarchicalCodelist)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CategoryScheme)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.CodeList)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.ConceptScheme)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.Specific, SdmxStructureEnumType.Dsd)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureQueryDetailEnumType.AllStubs, StructureReferenceDetailEnumType.All, SdmxStructureEnumType.Null)]
        public void TestGetMaintainables(
            SdmxStructureEnumType structureEnumType, 
            StructureQueryDetailEnumType detail, 
            StructureReferenceDetailEnumType refDetailEnum, 
            SdmxStructureEnumType specificStructure)
        {
            var mutableSearchManager = this.GetMutableSearchManager(this._connectionString);
            var refDetail = StructureReferenceDetail.GetFromEnum(refDetailEnum);

            var sdmxStructureType = SdmxStructureType.GetFromEnum(specificStructure == SdmxStructureEnumType.HierarchicalCodelist && structureEnumType == SdmxStructureEnumType.CodeList ? SdmxStructureEnumType.HierarchicalCodelist : SdmxStructureEnumType.Categorisation);
            IStructureReference reference = new StructureReferenceImpl(new MaintainableRefObjectImpl(), sdmxStructureType);

            SdmxStructureType structureType = SdmxStructureType.GetFromEnum(structureEnumType);
            IRestStructureQuery query = new RESTStructureQueryCore(
                StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.Full),
                StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.Specific),
                structureType,
                reference,
                false);
            var allObjects = mutableSearchManager.GetMaintainables(query).GetMaintainables(structureType);
            var referenceLevel = StructureReferenceDetail.GetFromEnum(refDetailEnum);

            _log.DebugFormat(CultureInfo.InvariantCulture, "Structure type: {0}, Detail: {1}, Reference : {2}", structureType, detail, refDetail);
            foreach (var maintainableMutableObject in allObjects)
            {
                if ("MA".Equals(maintainableMutableObject.AgencyId))
                {
                    continue;
                }

                var structureReference = _fromMutable.Build(maintainableMutableObject);
                _log.DebugFormat(CultureInfo.InvariantCulture, "Checking maintainable : {0}", structureReference);
                var mockQuery = new RESTStructureQueryCore(StructureQueryDetail.GetFromEnum(detail), referenceLevel, SdmxStructureType.GetFromEnum(specificStructure), structureReference, false);
                switch (refDetail.EnumType)
                {
                    case StructureReferenceDetailEnumType.Null:
                        break;
                    case StructureReferenceDetailEnumType.None:
                        {
                            var mutableObject = GetMutables(mutableSearchManager, mockQuery, maintainableMutableObject);
                            Assert.IsTrue(mutableObject.GetMaintainables(structureType).All(o => o.StructureType.EnumType == structureType.EnumType));
                        }

                        break;
                    case StructureReferenceDetailEnumType.Parents:
                        {
                            var mutableObject = GetMutables(mutableSearchManager, mockQuery, maintainableMutableObject);
                            ValidateParents(mutableObject, structureType, detail != StructureQueryDetailEnumType.Full, structureReference);
                        }

                        break;
                    case StructureReferenceDetailEnumType.ParentsSiblings:
                        {
                            var mutableObject = GetMutables(mutableSearchManager, mockQuery, maintainableMutableObject);
                            ValidateParentsAndSiblings(mutableObject, structureType, detail != StructureQueryDetailEnumType.Full);
                        }

                        break;
                    case StructureReferenceDetailEnumType.Children:
                        {
                            var mutableObject = GetMutables(mutableSearchManager, mockQuery, maintainableMutableObject);
                            ValidateChildren(mutableObject, structureType, detail != StructureQueryDetailEnumType.Full);
                        }

                        break;
                    case StructureReferenceDetailEnumType.Descendants:
                        {
                            var mutableObject = GetMutables(mutableSearchManager, mockQuery, maintainableMutableObject);
                            ValidateDescendants(mutableObject, structureType, detail != StructureQueryDetailEnumType.Full);
                        }

                        break;
                    case StructureReferenceDetailEnumType.All:
                        {
                            var mutableObject = GetMutables(mutableSearchManager, mockQuery, maintainableMutableObject);
                            ValidateAll(mutableObject, structureType, detail != StructureQueryDetailEnumType.Full);
                        }

                        break;
                    case StructureReferenceDetailEnumType.Specific:
                        {
                            if (specificStructure != SdmxStructureEnumType.Categorisation || detail == StructureQueryDetailEnumType.Full)
                            {
                                var specificStructureReference = SdmxStructureType.GetFromEnum(specificStructure);
                                var specificQuery = new RESTStructureQueryCore(StructureQueryDetail.GetFromEnum(detail), referenceLevel, specificStructureReference, structureReference, false);
                                var mutablesWithSpecific = GetMutables(mutableSearchManager, specificQuery, maintainableMutableObject);
                                ValidateSpecific(mutablesWithSpecific, structureType, detail != StructureQueryDetailEnumType.Full, specificStructureReference, structureReference);
                            }
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion

        #region Methods

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

        /// <summary>
        /// Gets the mutable search manager.
        /// </summary>
        /// <param name="settings">
        /// The connection string settings.
        /// </param>
        /// <returns>
        /// The <see cref="StructureReferenceFromMutableBuilder"/>.
        /// </returns>
        protected virtual IMutableStructureSearchManager GetMutableSearchManager(ConnectionStringSettings settings)
        {
            return this._mutableStructureSearchManagerFactory.GetStructureSearchManager(settings, this.GetSchema());
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
        /// Get component references.
        /// </summary>
        /// <param name="mutableObject">
        /// The mutable object.
        /// </param>
        /// <param name="structureType">
        /// The structure type.
        /// </param>
        /// <returns>
        /// The component reference
        /// </returns>
        private static IEnumerable<IStructureReference> GetComponentReferences(IMutableObjects mutableObject, BaseConstantType<SdmxStructureEnumType> structureType)
        {
            ISet<IStructureReference> structureReferences = new HashSet<IStructureReference>();
            foreach (var dsd in mutableObject.DataStructures)
            {
                structureReferences.UnionWith(_referenceSetBuilder.Build(dsd));
            }

            return
                (from m in structureReferences
                 where m.MaintainableStructureEnumType.EnumType == structureType.EnumType
                 select new StructureReferenceImpl(m.MaintainableReference, m.MaintainableStructureEnumType)).Distinct().ToArray();
        }

        /// <summary>
        /// Gets the mutable container object.
        /// </summary>
        /// <param name="mutableSearchManager">
        /// The mutable search manager.
        /// </param>
        /// <param name="mockQuery">
        /// The mock query.
        /// </param>
        /// <param name="maintainableMutableObject">
        /// The maintainable mutable object.
        /// </param>
        /// <returns>
        /// The mutable object container.
        /// </returns>
        private static IMutableObjects GetMutables(IMutableStructureSearchManager mutableSearchManager, IRestStructureQuery mockQuery, IMaintainableMutableObject maintainableMutableObject)
        {
            var mutableObject = mutableSearchManager.GetMaintainables(mockQuery);
            Assert.IsNotNull(mutableObject, _fromMutable.Build(maintainableMutableObject).ToString());
            Assert.IsNotEmpty(mutableObject.AllMaintainables, _fromMutable.Build(maintainableMutableObject).ToString());
            return mutableObject;
        }

        /// <summary>
        /// Validates all.
        /// </summary>
        /// <param name="mutableObject">
        /// The mutable object.
        /// </param>
        /// <param name="structureType">
        /// Type of the structure.
        /// </param>
        /// <param name="stubs">
        /// if set to <c>true</c> [stubs].
        /// </param>
        private static void ValidateAll(IMutableObjects mutableObject, BaseConstantType<SdmxStructureEnumType> structureType, bool stubs)
        {
            ValidateParentsAndSiblings(mutableObject, structureType, stubs);
            ValidateDescendants(mutableObject, structureType, stubs);
        }

        /// <summary>
        /// The validate children.
        /// </summary>
        /// <param name="mutableObject">The mutable object.</param>
        /// <param name="structureType">The structure type.</param>
        /// <param name="stubs">if set to <c>true</c> [stubs].</param>
        private static void ValidateChildren(IMutableObjects mutableObject, BaseConstantType<SdmxStructureEnumType> structureType, bool stubs)
        {
            switch (structureType.EnumType)
            {
                case SdmxStructureEnumType.Categorisation:

                    Assert.IsNotEmpty(mutableObject.Dataflows);
                    Assert.IsNotEmpty(mutableObject.CategorySchemes);
                    if (!stubs)
                    {
                        var categorySchemeRefs =
                            (from m in mutableObject.Categorisations select new StructureReferenceImpl(m.CategoryReference.MaintainableReference, m.CategoryReference.MaintainableStructureEnumType))
                                .Distinct().ToArray();
                        var got = from o in mutableObject.CategorySchemes select _fromMutable.Build(o);

                        CollectionAssert.AreEquivalent(categorySchemeRefs, got);
                    }

                    if (!stubs)
                    {
                        var dataflows = (from m in mutableObject.Categorisations select m.StructureReference).Distinct();
                        var got = from o in mutableObject.Dataflows select _fromMutable.Build(o);

                        CollectionAssert.AreEquivalent(dataflows, got);
                    }

                    break;
                case SdmxStructureEnumType.CategoryScheme:

                    break;
                case SdmxStructureEnumType.CodeList:

                    break;
                case SdmxStructureEnumType.ConceptScheme:

                    break;
                case SdmxStructureEnumType.Dataflow:
                    {
                        Assert.IsNotEmpty(mutableObject.DataStructures);
                        if (!stubs)
                        {
                            var dataflows = (from m in mutableObject.Dataflows select m.DataStructureRef).Distinct();
                            var got = from o in mutableObject.DataStructures select _fromMutable.Build(o);

                            CollectionAssert.AreEquivalent(dataflows, got);
                        }
                    }

                    break;
                case SdmxStructureEnumType.Dsd:
                    Assert.IsNotEmpty(mutableObject.ConceptSchemes);
                    Assert.IsNotEmpty(mutableObject.Codelists);
                    if (!stubs)
                    {
                        var sdmxStructureType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme);
                        ValidateComponentReferences(mutableObject, sdmxStructureType);
                    }

                    if (!stubs)
                    {
                        var sdmxStructureType = SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList);
                        ValidateComponentReferences(mutableObject, sdmxStructureType);
                    }

                    break;
                case SdmxStructureEnumType.HierarchicalCodelist:
                    Assert.IsNotEmpty(mutableObject.Codelists);
                    if (!stubs)
                    {
                        var got = (from o in mutableObject.Codelists select _fromMutable.Build(o)).Distinct<IStructureReference>().ToArray();

                        var hclReferences =
                            (from m in mutableObject.HierarchicalCodelists from codelistRefMutableObject in m.CodelistRef select codelistRefMutableObject.CodelistReference).Distinct().ToArray();

                        CollectionAssert.IsSubsetOf(hclReferences, got);
                    }

                    break;
            }
        }

        /// <summary>
        /// The validate component references.
        /// </summary>
        /// <param name="mutableObject">
        /// The mutable object.
        /// </param>
        /// <param name="sdmxStructureType">
        /// The SDMX structure type.
        /// </param>
        private static void ValidateComponentReferences(IMutableObjects mutableObject, SdmxStructureType sdmxStructureType)
        {
            var expected = GetComponentReferences(mutableObject, sdmxStructureType);
            var got = (from o in mutableObject.GetMaintainables(sdmxStructureType) select _fromMutable.Build(o)).Distinct<IStructureReference>().ToArray();

            CollectionAssert.AreEquivalent(expected, got);
        }

        /// <summary>
        /// Validates the descendants.
        /// </summary>
        /// <param name="mutableObject">
        /// The mutable object.
        /// </param>
        /// <param name="structureType">
        /// Type of the structure.
        /// </param>
        /// <param name="stubs">
        /// if set to <c>true</c> [stubs].
        /// </param>
        private static void ValidateDescendants(IMutableObjects mutableObject, BaseConstantType<SdmxStructureEnumType> structureType, bool stubs)
        {
            switch (structureType.EnumType)
            {
                case SdmxStructureEnumType.Categorisation:
                    if (!stubs)
                    {
                        Assert.IsNotEmpty(mutableObject.Dataflows);
                        Assert.IsNotEmpty(mutableObject.CategorySchemes);
                        Assert.IsNotEmpty(mutableObject.DataStructures);
                        Assert.IsNotEmpty(mutableObject.Codelists);
                        Assert.IsNotEmpty(mutableObject.ConceptSchemes);
                    }

                    break;
                case SdmxStructureEnumType.CategoryScheme:
                    break;
                case SdmxStructureEnumType.CodeList:
                    break;
                case SdmxStructureEnumType.ConceptScheme:
                    break;
                case SdmxStructureEnumType.Dataflow:
                    {
                        Assert.IsNotEmpty(mutableObject.Dataflows);
                        Assert.IsNotEmpty(mutableObject.DataStructures);
                        Assert.IsNotEmpty(mutableObject.Codelists);
                        Assert.IsNotEmpty(mutableObject.ConceptSchemes);
                    }

                    break;
                case SdmxStructureEnumType.Dsd:
                    {
                        Assert.IsNotEmpty(mutableObject.DataStructures);
                        Assert.IsNotEmpty(mutableObject.Codelists);
                        Assert.IsNotEmpty(mutableObject.ConceptSchemes);
                    }

                    break;
                case SdmxStructureEnumType.HierarchicalCodelist:
                    Assert.IsNotEmpty(mutableObject.Codelists);
                    break;
            }
        }

        /// <summary>
        /// The validate parents.
        /// </summary>
        /// <param name="mutableObject">
        /// The mutable object.
        /// </param>
        /// <param name="structureType">
        /// The structure type.
        /// </param>
        /// <param name="stubs">
        /// if we deal with stubs
        /// </param>
        /// <param name="structureReference">
        /// The structure Reference.
        /// </param>
        private static void ValidateParents(IMutableObjects mutableObject, BaseConstantType<SdmxStructureEnumType> structureType, bool stubs, IStructureReference structureReference)
        {
            switch (structureType.EnumType)
            {
                case SdmxStructureEnumType.Categorisation:
                    break;
                case SdmxStructureEnumType.CategoryScheme:
                    {
                        Assert.IsNotEmpty(mutableObject.Categorisations, structureReference.ToString());
                        if (!stubs)
                        {
                            var referencedByParent =
                                (from m in mutableObject.Categorisations select new StructureReferenceImpl(m.CategoryReference.MaintainableReference, m.CategoryReference.MaintainableStructureEnumType))
                                    .Distinct().ToArray();
                            var got = from o in mutableObject.CategorySchemes select _fromMutable.Build(o);
                            if (referencedByParent.Length > 0)
                            {
                                CollectionAssert.IsSubsetOf(referencedByParent, got, structureReference.ToString());
                            }
                        }
                    }

                    break;
                case SdmxStructureEnumType.CodeList:
                    {
                        if (!mutableObject.Codelists.All(o => "MA".Equals(o.AgencyId)))
                        {
                            Assert.IsTrue(mutableObject.DataStructures.Count + mutableObject.HierarchicalCodelists.Count > 0, structureReference.ToString());
                        }

                        if (!stubs)
                        {
                            var referencedByParent = GetComponentReferences(mutableObject, structureType);
                            var got = (from o in mutableObject.Codelists select _fromMutable.Build(o)).Distinct<IStructureReference>().ToArray();

                            CollectionAssert.IsSubsetOf(superset: referencedByParent, subset: got, message: structureReference.ToString());
                            if (mutableObject.HierarchicalCodelists.Count > 0)
                            {
                                var hclReferences =
                                    (from m in mutableObject.HierarchicalCodelists from codelistRefMutableObject in m.CodelistRef select codelistRefMutableObject.CodelistReference).Distinct()
                                        .ToArray();

                                CollectionAssert.IsSubsetOf(superset: hclReferences, subset: got, message: structureReference.ToString());
                            }
                        }
                    }

                    break;
                case SdmxStructureEnumType.ConceptScheme:
                    {
                        Assert.IsNotEmpty(mutableObject.DataStructures, structureReference.ToString());
                        if (!stubs)
                        {
                            var referencedByParent = GetComponentReferences(mutableObject, structureType);
                            var got = from o in mutableObject.ConceptSchemes select _fromMutable.Build(o);

                            CollectionAssert.IsSubsetOf(superset: referencedByParent, subset: got, message: structureReference.ToString());
                        }
                    }

                    break;
                case SdmxStructureEnumType.Dataflow:
                    {
                        Assert.IsNotEmpty(mutableObject.Categorisations, structureReference.ToString());
                        if (!stubs)
                        {
                            var referencedByParent = (from m in mutableObject.Categorisations select m.StructureReference).Distinct().ToArray();
                            var got = (from o in mutableObject.Dataflows select _fromMutable.Build(o)).Distinct<IStructureReference>().ToArray();

                            if (referencedByParent.Length > 0)
                            {
                                CollectionAssert.IsSubsetOf(referencedByParent, got, structureReference.ToString());
                            }
                        }
                    }

                    break;
                case SdmxStructureEnumType.Dsd:
                    {
                        Assert.IsNotEmpty(mutableObject.Dataflows, structureReference.ToString());
                        if (!stubs)
                        {
                            var referencedByParent = (from m in mutableObject.Dataflows select m.DataStructureRef).Distinct().ToArray();
                            var got = from o in mutableObject.DataStructures select _fromMutable.Build(o);

                            CollectionAssert.IsSubsetOf(referencedByParent, got, structureReference.ToString());
                        }
                    }

                    break;
                case SdmxStructureEnumType.HierarchicalCodelist:
                    break;
            }
        }

        /// <summary>
        /// The validate parents and siblings.
        /// </summary>
        /// <param name="mutableObject">
        /// The mutable object.
        /// </param>
        /// <param name="structureType">
        /// The structure type.
        /// </param>
        /// <param name="stubs">
        /// The stubs.
        /// </param>
        private static void ValidateParentsAndSiblings(IMutableObjects mutableObject, BaseConstantType<SdmxStructureEnumType> structureType, bool stubs)
        {
            switch (structureType.EnumType)
            {
                case SdmxStructureEnumType.Categorisation:
                    break;
                case SdmxStructureEnumType.CategoryScheme:
                    {
                        if (!stubs)
                        {
                            Assert.IsNotEmpty(mutableObject.Categorisations);
                            Assert.IsNotEmpty(mutableObject.Dataflows);
                            Assert.IsNotEmpty(mutableObject.CategorySchemes);
                            var referencedByParent =
                                (from m in mutableObject.Categorisations select new StructureReferenceImpl(m.CategoryReference.MaintainableReference, m.CategoryReference.MaintainableStructureEnumType))
                                    .Distinct().ToArray();
                            var got = from o in mutableObject.CategorySchemes select _fromMutable.Build(o);
                            if (referencedByParent.Length > 0)
                            {
                                CollectionAssert.IsSubsetOf(referencedByParent, got);
                            }
                        }
                    }

                    break;
                case SdmxStructureEnumType.CodeList:
                    {
                        if (!mutableObject.Codelists.All(o => "MA".Equals(o.AgencyId)))
                        {
                            Assert.IsNotEmpty(mutableObject.Codelists);
                            Assert.IsTrue(mutableObject.DataStructures.Count + mutableObject.HierarchicalCodelists.Count > 0, _fromMutable.Build(mutableObject.Codelists.First()).ToString());
                            if (mutableObject.DataStructures.Count > 0)
                            {
                                Assert.IsNotEmpty(mutableObject.ConceptSchemes);
                            }
                        }

                        if (!stubs)
                        {
                            var referencedByParent = GetComponentReferences(mutableObject, structureType);
                            var got = (from o in mutableObject.Codelists select _fromMutable.Build(o)).Distinct<IStructureReference>().ToArray();

                            CollectionAssert.IsSubsetOf(referencedByParent, got);

                            var hclReferences =
                                (from m in mutableObject.HierarchicalCodelists from codelistRefMutableObject in m.CodelistRef select codelistRefMutableObject.CodelistReference).Distinct().ToArray();

                            CollectionAssert.IsSubsetOf(hclReferences, got);
                        }
                    }

                    break;
                case SdmxStructureEnumType.ConceptScheme:
                    {
                        Assert.IsNotEmpty(mutableObject.ConceptSchemes);
                        Assert.IsNotEmpty(mutableObject.Codelists);
                        Assert.IsNotEmpty(mutableObject.DataStructures);
                        if (!stubs)
                        {
                            var referencedByParent = GetComponentReferences(mutableObject, structureType);
                            var got = from o in mutableObject.ConceptSchemes select _fromMutable.Build(o);

                            CollectionAssert.IsSubsetOf(referencedByParent, got);
                        }
                    }

                    break;
                case SdmxStructureEnumType.Dataflow:
                    {
                        Assert.IsNotEmpty(mutableObject.Categorisations);
                        Assert.IsNotEmpty(mutableObject.Dataflows);
                        Assert.IsNotEmpty(mutableObject.CategorySchemes);
                        if (!stubs)
                        {
                            var referencedByParent = (from m in mutableObject.Categorisations select m.StructureReference).Distinct().ToArray();
                            var got = (from o in mutableObject.Dataflows select _fromMutable.Build(o)).Distinct<IStructureReference>().ToArray();

                            if (referencedByParent.Length > 0)
                            {
                                CollectionAssert.IsSubsetOf(referencedByParent, got);
                            }
                        }
                    }

                    break;
                case SdmxStructureEnumType.Dsd:
                    {
                        Assert.IsNotEmpty(mutableObject.Dataflows);
                        Assert.IsNotEmpty(mutableObject.DataStructures);
                        if (!stubs)
                        {
                            var referencedByParent = (from m in mutableObject.Dataflows select m.DataStructureRef).Distinct().ToArray();
                            var got = from o in mutableObject.DataStructures select _fromMutable.Build(o);

                            CollectionAssert.IsSubsetOf(referencedByParent, got);
                        }
                    }

                    break;
                case SdmxStructureEnumType.HierarchicalCodelist:
                    break;
            }
        }

        /// <summary>
        /// Validates the specific.
        /// </summary>
        /// <param name="mutableObject">
        /// The mutable object.
        /// </param>
        /// <param name="structureType">
        /// Type of the structure.
        /// </param>
        /// <param name="stubs">
        /// if set to <c>true</c> [stubs].
        /// </param>
        /// <param name="specificStructureType">
        /// Type of the specific structure.
        /// </param>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        private static void ValidateSpecific(
            IMutableObjects mutableObject, 
            BaseConstantType<SdmxStructureEnumType> structureType, 
            bool stubs, 
            BaseConstantType<SdmxStructureEnumType> specificStructureType, 
            IStructureReference structureReference)
        {
            switch (structureType.EnumType)
            {
                case SdmxStructureEnumType.Categorisation:
                    switch (specificStructureType.EnumType)
                    {
                        case SdmxStructureEnumType.Categorisation:
                            break;
                        case SdmxStructureEnumType.CategoryScheme:
                            Assert.IsNotEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.CodeList:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.ConceptScheme:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.Dataflow:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.Dsd:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.HierarchicalCodelist:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                    }

                    break;
                case SdmxStructureEnumType.CategoryScheme:

                    // if we have stubs then no categorisations can be retrieved. 
                    // as a result we cannot get other structures.
                    if (!stubs)
                    {
                        switch (specificStructureType.EnumType)
                        {
                            case SdmxStructureEnumType.Categorisation:
                                Assert.IsNotEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                                Assert.IsNotEmpty(mutableObject.Categorisations, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                                break;
                            case SdmxStructureEnumType.CategoryScheme:
                                Assert.IsNotEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                                break;
                            case SdmxStructureEnumType.CodeList:
                                Assert.IsNotEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                                Assert.IsNotEmpty(mutableObject.Codelists, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                                break;
                            case SdmxStructureEnumType.ConceptScheme:
                                Assert.IsNotEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                                Assert.IsNotEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                                break;
                            case SdmxStructureEnumType.Dataflow:
                                Assert.IsNotEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                                Assert.IsNotEmpty(mutableObject.Dataflows, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                                break;
                            case SdmxStructureEnumType.Dsd:
                                Assert.IsNotEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                                Assert.IsNotEmpty(mutableObject.DataStructures, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                                break;
                            case SdmxStructureEnumType.HierarchicalCodelist:
                                Assert.IsNotEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                                Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                                break;
                        }
                    }

                    break;
                case SdmxStructureEnumType.CodeList:
                    switch (specificStructureType.EnumType)
                    {
                        case SdmxStructureEnumType.Categorisation:
                            if (!stubs)
                            {
                                Assert.IsNotEmpty(mutableObject.Categorisations, structureReference.ToString());
                            }

                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());

                            break;
                        case SdmxStructureEnumType.CategoryScheme:
                            if (!stubs)
                            {
                                Assert.IsNotEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            }

                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.CodeList:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.ConceptScheme:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.Dataflow:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.Dsd:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.HierarchicalCodelist:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());

                            // Assert.IsNotEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                    }

                    break;
                case SdmxStructureEnumType.ConceptScheme:
                    switch (specificStructureType.EnumType)
                    {
                        case SdmxStructureEnumType.Categorisation:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.CategoryScheme:
                            Assert.IsNotEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.CodeList:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.ConceptScheme:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.Dataflow:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.Dsd:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.HierarchicalCodelist:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                    }

                    break;
                case SdmxStructureEnumType.Dataflow:
                    switch (specificStructureType.EnumType)
                    {
                        case SdmxStructureEnumType.Categorisation:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.CategoryScheme:
                            Assert.IsNotEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.CodeList:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.ConceptScheme:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.Dataflow:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.Dsd:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.HierarchicalCodelist:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                    }

                    break;
                case SdmxStructureEnumType.Dsd:
                    switch (specificStructureType.EnumType)
                    {
                        case SdmxStructureEnumType.Categorisation:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.CategoryScheme:
                            Assert.IsNotEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.CodeList:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.ConceptScheme:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.Dataflow:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.Dsd:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.HierarchicalCodelist:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                    }

                    break;
                case SdmxStructureEnumType.HierarchicalCodelist:
                    switch (specificStructureType.EnumType)
                    {
                        case SdmxStructureEnumType.Categorisation:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.CategoryScheme:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.CodeList:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.ConceptScheme:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.Dataflow:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.Dsd:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                        case SdmxStructureEnumType.HierarchicalCodelist:
                            Assert.IsEmpty(mutableObject.CategorySchemes, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Categorisations, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Dataflows, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.DataStructures, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.Codelists, structureReference.ToString());
                            Assert.IsEmpty(mutableObject.ConceptSchemes, structureReference.ToString());
                            Assert.IsNotEmpty(mutableObject.HierarchicalCodelists, structureReference.ToString());
                            break;
                    }

                    break;
            }
        }

        #endregion
    }
}
