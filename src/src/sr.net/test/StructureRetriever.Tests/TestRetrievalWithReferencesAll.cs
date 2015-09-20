// -----------------------------------------------------------------------
// <copyright file="TestRetrievalWithReferencesAll.cs" company="Eurostat">
//   Date Created : 2013-09-17
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace StructureRetriever.Tests
{
    using System.Configuration;
    using System.Linq;

    using Estat.Nsi.StructureRetriever.Factory;
    using Estat.Nsi.StructureRetriever.Manager;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// Test unit for <see cref="StructureRetrieverAvailableData"/>.
    /// Port from SR Java tests.
    /// </summary>
    [TestFixture]
    public class TestRetrievalWithReferencesAll
    {

        /// <summary>
        /// Test unit for <see cref="StructureRetrieverAvailableData.GetMaintainables"/> 
        /// </summary>
        /// <param name="name">The connection string name.</param>
        [TestCase("sqlserver")]
        [TestCase("odp")]
        [TestCase("msoracle")]
        [TestCase("mysql")]
        public void TestGetAllCategorisations(string name)
        {
            var mutableStructureSearchManager = GetStructureSearchManager(name);
            var catRef = new StructureReferenceImpl(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Categorisation));
            var detailLevel = StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.ReferencedStubs);
            var referenceLevel = StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.All);

            var structureQuery = new RESTStructureQueryCore(detailLevel, referenceLevel, null, catRef, false);
            var mutableObjects = mutableStructureSearchManager.GetMaintainables(structureQuery);
            Assert.IsTrue(mutableObjects.Dataflows.Count > 0);
            Assert.IsTrue(mutableObjects.DataStructures.Count > 0);
            Assert.IsTrue(mutableObjects.CategorySchemes.Count > 0);
            Assert.IsTrue(mutableObjects.Categorisations.Count > 0);
            Assert.IsTrue(mutableObjects.Codelists.Count > 0);
            Assert.IsTrue(mutableObjects.ConceptSchemes.Count > 0);
            Assert.IsTrue(mutableObjects.HierarchicalCodelists.Count == 0);
        }

        /// <summary>
        /// Test unit for <see cref="StructureRetrieverAvailableData.GetMaintainables" />
        /// </summary>
        /// <param name="name">The connection string name.</param>
        [TestCase("sqlserver")]
        [TestCase("odp")]
        [TestCase("msoracle")]
        [TestCase("mysql")]
        public void TestGetOneCategorisations(string name)
        {
            var mutableStructureSearchManager = GetStructureSearchManager(name);
            var detailLevel = StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.Full);
            var referenceLevel = StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.All);

            IStructureReference catRef = new StructureReferenceImpl(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Categorisation));
            var categorisation = mutableStructureSearchManager.RetrieveStructures(new[] { catRef }, false, false).Categorisations.First();

            var referenceForOne = new StructureReferenceImpl(categorisation.AgencyId, categorisation.Id,categorisation.Version, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Categorisation));
            var structureQuery = new RESTStructureQueryCore(detailLevel, referenceLevel, null, referenceForOne , false);
            var mutableObjects = mutableStructureSearchManager.GetMaintainables(structureQuery);
            Assert.IsTrue(mutableObjects.Dataflows.Count > 0);
            Assert.IsTrue(mutableObjects.DataStructures.Count > 0);
            Assert.IsTrue(mutableObjects.CategorySchemes.Count > 0);
            Assert.IsTrue(mutableObjects.Categorisations.Count > 0);
            Assert.IsTrue(mutableObjects.Codelists.Count > 0);
            Assert.IsTrue(mutableObjects.ConceptSchemes.Count > 0);
            Assert.IsTrue(mutableObjects.HierarchicalCodelists.Count == 0);
        }

        /// <summary>
        /// Test unit for <see cref="StructureRetrieverAvailableData.GetMaintainables" />
        /// </summary>
        /// <param name="name">The connection string name.</param>
        [TestCase("sqlserver")]
        [TestCase("odp")]
        [TestCase("msoracle")]
        [TestCase("mysql")]
        public void TestGetOneCatScheme(string name)
        {
            var mutableStructureSearchManager = GetStructureSearchManager(name);
            var detailLevel = StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.Full);
            var referenceLevel = StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.All);

            IStructureReference getAllReference = new StructureReferenceImpl(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme));
            var objectToSearch = mutableStructureSearchManager.RetrieveStructures(new[] { getAllReference }, false, false).CategorySchemes.First();

            var referenceForOne = new StructureReferenceImpl(objectToSearch.AgencyId, objectToSearch.Id, objectToSearch.Version, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme));
            var structureQuery = new RESTStructureQueryCore(detailLevel, referenceLevel, null, referenceForOne, false);
            var mutableObjects = mutableStructureSearchManager.GetMaintainables(structureQuery);
            Assert.IsNotEmpty(mutableObjects.Dataflows, referenceForOne.ToString());
            Assert.IsEmpty(mutableObjects.DataStructures, referenceForOne.ToString());
            Assert.IsNotEmpty(mutableObjects.CategorySchemes, referenceForOne.ToString());
            Assert.IsNotEmpty(mutableObjects.Categorisations, referenceForOne.ToString());
            Assert.IsEmpty(mutableObjects.Codelists, referenceForOne.ToString());
            Assert.IsEmpty(mutableObjects.ConceptSchemes, referenceForOne.ToString());
            Assert.IsTrue(mutableObjects.HierarchicalCodelists.Count == 0, referenceForOne.ToString());
        }


        private static IMutableStructureSearchManager GetStructureSearchManager(string name)
        {
            ConnectionStringSettings css = ConfigurationManager.ConnectionStrings[name];
            IMutableStructureSearchManagerFactory factory = new MutableStructureSearchManagerFactory();
            return factory.GetStructureSearchManager(css, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.Null));
        }
    }
}