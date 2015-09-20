// -----------------------------------------------------------------------
// <copyright file="TestCustomQueryParseManager.cs" company="Eurostat">
//   Date Created : 2013-03-28
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace CustomRequests.Tests
{
    using Estat.Sri.CustomRequests.Manager;
    using Estat.Sri.CustomRequests.Model;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    /// Test unit for <see cref="CustomQueryParseManager"/>
    /// </summary>
    [TestFixture]
    public class TestCustomQueryParseManager
    {

        /// <summary>
        /// Test unit for <see cref="QueryParsingManager.ParseQueries(Org.Sdmxsource.Sdmx.Api.Util.IReadableDataLocation)"/> 
        /// </summary>
        /// <param name="testFile">
        /// The test File.
        /// </param>
        [TestCase("tests/v20/StructureRequest/get available data ADJUSTMENT  with REF AREA FREQ constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data STS ACTIVITY with ADJUSTMENT   REF AREA FREQ constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data with TIME constrain.xml")]
        [TestCase("tests/v20/StructureRequest/get available data FREQ no constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data REF AREA with FREQ constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data TIME fake CL with STS ACTIVITY ADJUSTMENT   REF AREA FREQ constrains.xml")]
        public void TestParseQueriesCustom(string testFile)
        {
            var manager = new CustomQueryParseManager(SdmxSchemaEnumType.VersionTwo);
            using (var readable = new FileReadableDataLocation(testFile))
            {
                var result = manager.ParseQueries(readable);
                Assert.IsNotEmpty(result.SimpleStructureQueries);
                foreach (var reference in result.SimpleStructureQueries)
                {
                    if (reference.MaintainableStructureEnumType.EnumType == SdmxStructureEnumType.Dataflow)
                    {
                        var constrainable = reference as ConstrainableStructureReference;
                        Assert.NotNull(constrainable);
                        Assert.NotNull(constrainable.ConstraintObject);
                    }
                }
            }
        }

        /// <summary>
        /// Test unit for <see cref="QueryParsingManager.ParseQueries(Org.Sdmxsource.Sdmx.Api.Util.IReadableDataLocation)"/> 
        /// </summary>
        /// <param name="testFile">
        /// The test File.
        /// </param>
        [TestCase("tests/v20/StructureRequest/get a category with resolve ref.xml")]
        [TestCase("tests/v20/StructureRequest/get a category.xml")]
        [TestCase("tests/v20/StructureRequest/get a codelist failure.xml")]
        [TestCase("tests/v20/StructureRequest/get a codelist.xml")]
        [TestCase("tests/v20/StructureRequest/get a concept scheme.xml")]
        [TestCase("tests/v20/StructureRequest/get a dataflow resolve ref.xml")]
        [TestCase("tests/v20/StructureRequest/get a keyfamily resolve ref.xml")]
        [TestCase("tests/v20/StructureRequest/get a syntax error.xml", ExpectedException = typeof(Org.Sdmxsource.Sdmx.Util.Exception.ParseException))]
        [TestCase("tests/v20/StructureRequest/get all codelist.xml")]
        [TestCase("tests/v20/StructureRequest/get all concept schemes.xml")]
        [TestCase("tests/v20/StructureRequest/get all concept schemes resolve ref.xml")]
        [TestCase("tests/v20/StructureRequest/get available data ADJUSTMENT  with REF AREA FREQ constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data STS ACTIVITY with ADJUSTMENT   REF AREA FREQ constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data with TIME constrain.xml")]
        [TestCase("tests/v20/StructureRequest/get available data FREQ no constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data REF AREA with FREQ constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data TIME fake CL with STS ACTIVITY ADJUSTMENT   REF AREA FREQ constrains.xml")]
        public void TestParseQueries(string testFile)
        {
            var manager = new CustomQueryParseManager(SdmxSchemaEnumType.VersionTwo);
            using (var readable = new FileReadableDataLocation(testFile))
            {
                var result = manager.ParseQueries(readable);
                Assert.IsNotEmpty(result.SimpleStructureQueries);
            }
        }
    }
}