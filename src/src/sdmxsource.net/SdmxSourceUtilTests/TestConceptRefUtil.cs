// -----------------------------------------------------------------------
// <copyright file="TestConceptRefUtil.cs" company="Eurostat">
//   Date Created : 2013-01-11
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace SdmxSourceUtilTests
{
    using Moq;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects;

    /// <summary>
    /// Test unit for <see cref="ConceptRefUtil"/>
    /// </summary>
    [TestFixture]
    public class TestConceptRefUtil
    {

        /// <summary>
        /// Test unit for <see cref="ConceptRefUtil.BuildConceptRef"/> 
        /// </summary>
        [Test]
        public void TestBuildConceptRef()
        {
            var moq = new Mock<IDimension>();
            moq.Setup(dimension => dimension.Id).Returns("DIM1");
            ICrossReference buildConceptRef = ConceptRefUtil.BuildConceptRef(moq.Object, "AGENCYTEST", "TESTCS", "1.2", "AGENCYTEST", "CAT1");
            Assert.NotNull(buildConceptRef);
            Assert.NotNull(buildConceptRef.ChildReference);
            Assert.AreEqual(buildConceptRef.ChildReference.StructureEnumType.EnumType, SdmxStructureEnumType.Concept);
            Assert.AreEqual("urn:sdmx:org.sdmx.infomodel.conceptscheme.Concept=AGENCYTEST:TESTCS(1.2).CAT1", buildConceptRef.TargetUrn.ToString());
        }

        /// <summary>
        /// Test unit for <see cref="ConceptRefUtil.GetConceptId"/> 
        /// </summary>
        [Test]
        public void TestGetConceptId()
        {
            var moq = new Mock<IStructureReference>();
            moq.Setup(reference => reference.ChildReference.Id).Returns("CAT1");
            moq.Setup(reference => reference.TargetReference).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Concept));
           
            IStructureReference conceptRef = moq.Object;
            string conceptId = ConceptRefUtil.GetConceptId(conceptRef);
            Assert.AreEqual("CAT1", conceptId);
        }
    }
}