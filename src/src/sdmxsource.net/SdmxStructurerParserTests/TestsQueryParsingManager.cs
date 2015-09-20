// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestsQueryParsingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The tests query parsing manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxStructureParserTests
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Sdmx.Structureparser.Workspace;
    using Org.Sdmxsource.Sdmx.Util.Exception;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    /// The tests query parsing manager.
    /// </summary>
    [TestFixture]
    public class TestsQueryParsingManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// The test query parser manager v 20.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [Test]
        [TestCase("tests/v20/QueryStructureRequest.xml")]
        [TestCase("tests/v20/QueryStructureRequestDataflowCodelist.xml")]
        [TestCase("tests/v20/StructureRequest/get a category with resolve ref.xml")]
        [TestCase("tests/v20/StructureRequest/get a category.xml")]
        [TestCase("tests/v20/StructureRequest/get a codelist failure.xml")]
        [TestCase("tests/v20/StructureRequest/get a codelist.xml")]
        [TestCase("tests/v20/StructureRequest/get a concept scheme.xml")]
        [TestCase("tests/v20/StructureRequest/get a dataflow resolve ref.xml")]
        [TestCase("tests/v20/StructureRequest/get a keyfamily resolve ref.xml")]
        [TestCase("tests/v20/StructureRequest/get a syntax error.xml", ExpectedException = typeof(ParseException))]
        [TestCase("tests/v20/StructureRequest/get all codelist.xml")]
        [TestCase("tests/v20/StructureRequest/get all concept schemes.xml")]
        [TestCase("tests/v20/StructureRequest/get all concept schemes resolve ref.xml")]
        [TestCase("tests/v20/StructureRequest/get available data ADJUSTMENT  with REF AREA FREQ constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data STS ACTIVITY with ADJUSTMENT   REF AREA FREQ constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data with TIME constrain.xml")]
        public void TestQueryParserManagerV20(string file)
        {
            using (var fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IQueryParsingManager queryParsingManager = new QueryParsingManager(SdmxSchemaEnumType.VersionTwo);
                IQueryWorkspace queryWorkspace = queryParsingManager.ParseQueries(fileReadableDataLocation);
                Assert.IsNotNull(queryWorkspace);
                Assert.IsNotEmpty(queryWorkspace.SimpleStructureQueries);
            }
        }

        /// <summary>
        /// The test query parser manager v 20 deep.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="requests">
        /// The requests.
        /// </param>
        /// <param name="resolveRefs">
        /// The resolve refs.
        /// </param>
        [TestCase("tests/v20/QueryStructureRequest.xml", new[] { SdmxStructureEnumType.CodeList }, false)]
        [TestCase("tests/v20/QueryStructureRequestDataflowCodelist.xml", new[] { SdmxStructureEnumType.CodeList, SdmxStructureEnumType.Dataflow }, false)]
        [TestCase("tests/v20/StructureRequest/get a category with resolve ref.xml", new[] { SdmxStructureEnumType.CategoryScheme }, true)]
        [TestCase("tests/v20/StructureRequest/get a category.xml", new[] { SdmxStructureEnumType.CategoryScheme }, false)]
        [TestCase("tests/v20/StructureRequest/get a codelist failure.xml", new[] { SdmxStructureEnumType.CodeList }, false)]
        [TestCase("tests/v20/StructureRequest/get a codelist.xml", new[] { SdmxStructureEnumType.CodeList }, false)]
        [TestCase("tests/v20/StructureRequest/get a concept scheme.xml", new[] { SdmxStructureEnumType.ConceptScheme }, false)]
        [TestCase("tests/v20/StructureRequest/get a dataflow resolve ref.xml", new[] { SdmxStructureEnumType.Dataflow }, true)]
        [TestCase("tests/v20/StructureRequest/get a keyfamily resolve ref.xml", new[] { SdmxStructureEnumType.Dsd }, true)]
        [TestCase("tests/v20/StructureRequest/get a syntax error.xml", new[] { SdmxStructureEnumType.CodeList }, false, ExpectedException = typeof(ParseException))]
        [TestCase("tests/v20/StructureRequest/get all codelist.xml", new[] { SdmxStructureEnumType.CodeList }, false)]
        [TestCase("tests/v20/StructureRequest/get all concept schemes.xml", new[] { SdmxStructureEnumType.ConceptScheme }, false)]
        [TestCase("tests/v20/StructureRequest/get all concept schemes resolve ref.xml", new[] { SdmxStructureEnumType.ConceptScheme }, true)]
        [TestCase("tests/v20/StructureRequest/get available data ADJUSTMENT  with REF AREA FREQ constrains.xml", 
            new[] { SdmxStructureEnumType.Dataflow, SdmxStructureEnumType.CodeList }, false)]
        [TestCase("tests/v20/StructureRequest/get available data STS ACTIVITY with ADJUSTMENT   REF AREA FREQ constrains.xml", 
            new[] {  SdmxStructureEnumType.Dataflow, SdmxStructureEnumType.CodeList }, false)]
        [TestCase("tests/v20/StructureRequest/get available data with TIME constrain.xml", new[] { SdmxStructureEnumType.Dataflow, SdmxStructureEnumType.CodeList }, 
            false)]
        public void TestQueryParserManagerV20Deep(string file, SdmxStructureEnumType[] requests, bool resolveRefs)
        {
            using (var fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IQueryParsingManager queryParsingManager = new QueryParsingManager(SdmxSchemaEnumType.VersionTwo);
                IQueryWorkspace queryWorkspace = queryParsingManager.ParseQueries(fileReadableDataLocation);
                Assert.IsNotNull(queryWorkspace);
                Assert.IsTrue(queryWorkspace.HasStructureQueries());
                Assert.IsNotEmpty(queryWorkspace.SimpleStructureQueries);
                Assert.AreEqual(requests.Length, queryWorkspace.SimpleStructureQueries.Count);
                var expectedRequests = new LinkedList<SdmxStructureEnumType>(requests);
                Assert.AreEqual(resolveRefs, queryWorkspace.ResolveReferences);
                foreach (IStructureReference query in queryWorkspace.SimpleStructureQueries)
                {
                    Assert.IsTrue(expectedRequests.Remove(query.MaintainableStructureEnumType.EnumType));
                }
            }
        }

        #endregion
    }
}