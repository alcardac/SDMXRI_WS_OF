// -----------------------------------------------------------------------
// <copyright file="TestComplexStructureQueryBuilderV21.cs" company="Eurostat">
//   Date Created : 2013-09-20
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace CustomRequests.Tests
{
    using Estat.Sri.CustomRequests.Builder.QueryBuilder;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference.Complex;
    using Org.Sdmxsource.Util.Io;
    using Org.Sdmxsource.XmlHelper;

    /// <summary>
    /// Test unit for <see cref="ComplexStructureQueryBuilderV21"/>
    /// </summary>
    [TestFixture]
    public class TestComplexStructureQueryBuilderV21
    {
        /// <summary>
        /// Test unit for <see cref="ComplexStructureQueryBuilderV21.BuildComplexStructureQuery"/> 
        /// </summary>
        /// <param name="detail">The detail</param>
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureReferenceDetailEnumType.All)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureReferenceDetailEnumType.Children)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureReferenceDetailEnumType.Descendants)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureReferenceDetailEnumType.None)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureReferenceDetailEnumType.Parents)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureReferenceDetailEnumType.ParentsSiblings)]
        [TestCase(SdmxStructureEnumType.CategoryScheme, StructureReferenceDetailEnumType.Specific)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureReferenceDetailEnumType.All)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureReferenceDetailEnumType.Children)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureReferenceDetailEnumType.Descendants)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureReferenceDetailEnumType.None)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureReferenceDetailEnumType.Parents)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureReferenceDetailEnumType.ParentsSiblings)]
        [TestCase(SdmxStructureEnumType.Dsd, StructureReferenceDetailEnumType.Specific)]
        public void TestDetail(SdmxStructureEnumType sdmxStructure, StructureReferenceDetailEnumType detail)
        {
            var agency = new ComplexTextReferenceCore(null, TextSearch.GetFromEnum(TextSearchEnumType.Equal), "TEST");
            var id = new ComplexTextReferenceCore(null, TextSearch.GetFromEnum(TextSearchEnumType.Equal), "TEST");
            IComplexVersionReference versionRef = new ComplexVersionReferenceCore(TertiaryBool.ParseBoolean(false), "1.0", null, null);
            var complexStructureReferenceCore = new ComplexStructureReferenceCore(agency, id, versionRef, SdmxStructureType.GetFromEnum(sdmxStructure), null, null, null, null);
            var complexStructureQueryMetadataCore = new ComplexStructureQueryMetadataCore(
                true,
                ComplexStructureQueryDetail.GetFromEnum(ComplexStructureQueryDetailEnumType.Full),
                ComplexMaintainableQueryDetail.GetFromEnum(ComplexMaintainableQueryDetailEnumType.Full),
                StructureReferenceDetail.GetFromEnum(detail),
                new [] { SdmxStructureType.GetFromEnum(sdmxStructure) });
            IComplexStructureQuery complexStructureQuery = new ComplexStructureQueryCore(complexStructureReferenceCore, complexStructureQueryMetadataCore);

            var builder = new ComplexStructureQueryBuilderV21();
            var structureQuery = builder.BuildComplexStructureQuery(complexStructureQuery);
            var fileName = string.Format("test-ComplexStructureQueryBuilderV21-{0}-{1}.xml", sdmxStructure.ToString(), detail.ToString());
            structureQuery.Save(fileName);
            using (var readable = new FileReadableDataLocation(fileName))
            {
                XMLParser.ValidateXml(readable, SdmxSchemaEnumType.VersionTwoPointOne);
            }
        }
    }
}