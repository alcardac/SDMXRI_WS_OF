// -----------------------------------------------------------------------
// <copyright file="TestQueryStructureRequestBuilderV2.cs" company="Eurostat">
//   Date Created : 2013-03-28
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace CustomRequests.Tests
{
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    using Estat.Sri.CustomRequests.Builder;

    using NUnit.Framework;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;

    /// <summary>
    /// Test unit for <see cref="QueryStructureRequestBuilderV2"/>
    /// </summary>
    [TestFixture]
    public class TestQueryStructureRequestBuilderV2
    {
        /// <summary>
        /// Test unit for <see cref="QueryStructureRequestBuilderV2.BuildStructureQuery(System.Collections.Generic.IEnumerable{Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.IStructureReference},bool)"/> 
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
        [TestCase("tests/v20/StructureRequest/get a syntax error.xml", ExpectedException = typeof(System.Xml.XmlException))]
        [TestCase("tests/v20/StructureRequest/get all codelist.xml")]
        [TestCase("tests/v20/StructureRequest/get all concept schemes.xml")]
        [TestCase("tests/v20/StructureRequest/get all concept schemes resolve ref.xml")]
        [TestCase("tests/v20/StructureRequest/get available data ADJUSTMENT  with REF AREA FREQ constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data STS ACTIVITY with ADJUSTMENT   REF AREA FREQ constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data with TIME constrain.xml")]
        [TestCase("tests/v20/StructureRequest/get available data FREQ no constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data REF AREA with FREQ constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data TIME fake CL with STS ACTIVITY ADJUSTMENT   REF AREA FREQ constrains.xml")]
        public void TestBuildStructureQuery(string testFile)
        {
            var registry = RegistryInterface.Load(testFile);
            
            var queryBuilderV2 = new ConstrainQueryBuilderV2();
            var structureReferences = queryBuilderV2.Build(registry.QueryStructureRequest);
            
            IQueryStructureRequestBuilder<XDocument> builder = new QueryStructureRequestBuilderV2(new HeaderImpl(registry.Content.Header));
            var document = builder.BuildStructureQuery(structureReferences, registry.QueryStructureRequest.resolveReferences);
            Assert.NotNull(document);
            Assert.NotNull(document.Root);
            var file = new FileInfo(testFile);
            document.Save("TestBuildStructureQuery" + file.Name);
            if (document.Root != null)
            {
                var element = document.Descendants(document.Root.Name.Namespace + "QueryStructureRequest").First();
                Assert.IsTrue(XNode.DeepEquals(registry.QueryStructureRequest.Untyped, element), registry.QueryStructureRequest.Untyped + "\n\nnvs\n\n" + element);
            }
        }
    }
}