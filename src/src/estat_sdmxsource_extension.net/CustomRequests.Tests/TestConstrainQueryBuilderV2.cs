// -----------------------------------------------------------------------
// <copyright file="TestConstrainQueryBuilderV2.cs" company="Eurostat">
//   Date Created : 2013-03-28
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace CustomRequests.Tests
{
    using Estat.Sri.CustomRequests.Builder;
    using Estat.Sri.CustomRequests.Model;

    using NUnit.Framework;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.message;
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// Test unit for <see cref="ConstrainQueryBuilderV2"/>
    /// </summary>
    [TestFixture]
    public class TestConstrainQueryBuilderV2
    {
        /// <summary>
        /// Test unit for <see cref="ConstrainQueryBuilderV2"/> 
        /// </summary>
        /// <param name="file">
        /// The file.
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
        public void TestBuild(string file)
        {
            var registry = RegistryInterface.Load(file);
            
            var queryBuilderV2 = new ConstrainQueryBuilderV2();
            var structureReferences = queryBuilderV2.Build(registry.QueryStructureRequest);
            Assert.IsNotEmpty(structureReferences);
        }


        /// <summary>
        /// Test unit for <see cref="ConstrainQueryBuilderV2"/> 
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v20/StructureRequest/get available data ADJUSTMENT  with REF AREA FREQ constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data STS ACTIVITY with ADJUSTMENT   REF AREA FREQ constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data with TIME constrain.xml")]
        [TestCase("tests/v20/StructureRequest/get available data FREQ no constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data REF AREA with FREQ constrains.xml")]
        [TestCase("tests/v20/StructureRequest/get available data TIME fake CL with STS ACTIVITY ADJUSTMENT   REF AREA FREQ constrains.xml")]
        public void TestBuildCustom(string file)
        {
            var registry = RegistryInterface.Load(file);

            var queryBuilderV2 = new ConstrainQueryBuilderV2();
            var structureReferences = queryBuilderV2.Build(registry.QueryStructureRequest);
            Assert.IsNotEmpty(structureReferences);
            foreach (var reference in structureReferences)
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
}