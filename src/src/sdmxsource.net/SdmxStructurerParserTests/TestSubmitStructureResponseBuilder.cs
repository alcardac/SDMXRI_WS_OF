// -----------------------------------------------------------------------
// <copyright file="TestSubmitStructureResponseBuilder.cs" company="EUROSTAT">
//   Date Created : 2015-03-06
//   Copyright (c) 2015 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace SdmxStructureParserTests
{
    using System.Globalization;
    using System.IO;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util.Io;
    using Org.Sdmxsource.XmlHelper;

    /// <summary>
    /// Test unit for <see cref="SubmitStructureResponseBuilder"/>
    /// </summary>
    [TestFixture]
    public class TestSubmitStructureResponseBuilder
    {
        /// <summary>
        /// Test unit for <see cref="SubmitStructureResponseBuilder.BuildSuccessResponse" />
        /// </summary>
        /// <param name="version">The version.</param>
        [TestCase(SdmxSchemaEnumType.VersionTwo)]
        [TestCase(SdmxSchemaEnumType.VersionTwoPointOne)]
        public void TestBuildSuccessResponse(SdmxSchemaEnumType version)
        {
            var responseBuilder = new SubmitStructureResponseBuilder();
            ISdmxObjects sdmxObjects = new SdmxObjectsImpl();
            var codelist = new CodelistMutableCore() { Id = "CL_TEST", Version = "1.0", AgencyId = "TEST" };
            codelist.AddName("en", "Test Codelist");
            for (int i = 0; i < 10; i++)
            {
                ICodeMutableObject item = new CodeMutableCore() { Id = "TEST_" + i.ToString(CultureInfo.InvariantCulture) };
                item.AddName("en", "Name for " + item.Id);
                codelist.AddItem(item);
            }

            sdmxObjects.AddCodelist(codelist.ImmutableInstance);
            var output = responseBuilder.BuildSuccessResponse(sdmxObjects, SdmxSchema.GetFromEnum(version));
            var fileName = "TestBuildSuccessResponse" + version + ".xml";
            output.Untyped.Save(fileName);
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation(fileName))
            {
                XMLParser.ValidateXml(dataLocation, version);
            }
        }

        [TestCase(SdmxSchemaEnumType.VersionTwoPointOne, @"tests\v21\Structure\NA_PENS+ESTAT+1.0.xml")]
        public void TestBuildSuccessResponseFile(SdmxSchemaEnumType version, string file)
        {
            var responseBuilder = new SubmitStructureResponseBuilder();
            ISdmxObjects sdmxObjects;
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation(file))
            {
                StructureParsingManager parsingManager = new StructureParsingManager();
                var structureWorkspace = parsingManager.ParseStructures(dataLocation);
                sdmxObjects = structureWorkspace.GetStructureObjects(false);
            }

            var output = responseBuilder.BuildSuccessResponse(sdmxObjects, SdmxSchema.GetFromEnum(version));
            var fileName = "TestBuildSuccessResponse" + version + ".xml";
            output.Untyped.Save(fileName);
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation(fileName))
            {
                XMLParser.ValidateXml(dataLocation, version);
            }
        }

        /// <summary>
        /// Test unit for <see cref="SubmitStructureResponseBuilder.BuildSuccessResponse" />
        /// </summary>
        /// <param name="version">The version.</param>
        [TestCase(SdmxSchemaEnumType.VersionTwo)]
        [TestCase(SdmxSchemaEnumType.VersionTwoPointOne)]
        public void TestBuildErrorResponse(SdmxSchemaEnumType version)
        {
            var responseBuilder = new SubmitStructureResponseBuilder();
            ISdmxObjects sdmxObjects = new SdmxObjectsImpl();
            var codelist = new CodelistMutableCore() { Id = "CL_TEST", Version = "1.0", AgencyId = "TEST"};
            codelist.AddName("en", "Test Codelist");
            for (int i = 0; i < 10; i++)
            {
                ICodeMutableObject item = new CodeMutableCore() { Id = "TEST_" + i.ToString(CultureInfo.InvariantCulture) };
                item.AddName("en", "Name for " + item.Id);
                codelist.AddItem(item);
            }
            
            sdmxObjects.AddCodelist(codelist.ImmutableInstance);
            const string ErrorMessage = "Invalid syntax";
            var output = responseBuilder.BuildErrorResponse(new SdmxSyntaxException(ErrorMessage), new StructureReferenceImpl("TEST", "CL_TEST", "1.0", SdmxStructureEnumType.CodeList), SdmxSchema.GetFromEnum(version));
            var fileName = "TestBuildErrorResponse" + version + ".xml";
            output.Untyped.Save(fileName);
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation(fileName))
            {
                XMLParser.ValidateXml(dataLocation, version);
            }

            Assert.IsTrue(File.ReadAllText(fileName).Contains(ErrorMessage));
        }
    }
}