// -----------------------------------------------------------------------
// <copyright file="TestGuideSamples.cs" company="Eurostat">
//   Date Created : 2013-04-18
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------



namespace GuideTests
{
    using GuideTests.Chapter4;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    using GuideTests.Chapter1;

    /// <summary>
    /// Test unit for <see cref="ReadingStructures"/>
    /// </summary>
    [TestFixture]
    public class TestGuideSamples
    {
        private StructuresCreator _creator;

        public TestGuideSamples()
        {
            _creator = new StructuresCreator();
        }

        /// <summary>
        /// Test unit for <see cref="ReadingStructures.readStructures"/> 
        /// </summary>
        [Test]
        public void TestReadingStructuresMain()
        {
            ReadingStructures rs = new ReadingStructures();

            rs.readStructures("tests/v21/structures.xml", SdmxSchemaEnumType.VersionTwoPointOne);
        }

        /// <summary>
        /// Test unit for <see cref="ReadingStructures.readStructures"/> 
        /// </summary>
        [TestCase("tests/v21/codelists_concepts.xml", "tests/v21/dsd.xml")]
        [TestCase("tests/v21/Structures_codelists_concepts.xml", "tests/v21/Structures_dsd_dataflow.xml")]
        public void TestResolveStructuresMain(string dependencies, string structure)
        {
            ResolveStructures resolv = new ResolveStructures();
            resolv.resolveStructures(dependencies, structure, SdmxSchemaEnumType.VersionTwoPointOne);
        }

        [Test]
        public void TestBuildAgency()
        {
            var agencyScheme = this._creator.BuildAgencyScheme();
            Assert.NotNull(agencyScheme);
            Assert.IsNotEmpty(agencyScheme.Items);
        }

        [Test]
        public void TestBuildConceptScheme()
        {
            var artefact = this._creator.BuildConceptScheme();
            Assert.NotNull(artefact);
            Assert.IsNotEmpty(artefact.Items);
        }

        [Test]
        public void TestBuildCountryCodelist()
        {
            var artefact = this._creator.BuildCountryCodelist();
            Assert.NotNull(artefact);
            Assert.IsNotEmpty(artefact.Items);
        }

        [Test]
        public void BuildDataStructure()
        {
            var artefact = this._creator.BuildDataStructure();
            Assert.NotNull(artefact);
            Assert.IsNotEmpty(artefact.Components);
            Assert.IsNotEmpty(artefact.GetDimensions());
        }

        [Test]
        public void Chapter4Test()
        {
            Chapter4ReadingData.Main(null);
        }
    }
}