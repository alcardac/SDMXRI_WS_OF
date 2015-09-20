// -----------------------------------------------------------------------
// <copyright file="TestMaintainableRefObjectImpl.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// Test unit for <see cref="MaintainableRefObjectImpl"/>
    /// </summary>
    [TestFixture]
    public class TestMaintainableRefObjectImpl
    {

        /// <summary>
        /// Test unit for <see cref="MaintainableRefObjectImpl.Equals(object)"/> 
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id.
        /// </param>
        /// <param name="hasAgency">
        /// The has Agency.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="hasId">
        /// The has Id.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <param name="hasVersion">
        /// The has Version.
        /// </param>
        /// <param name="equals">
        /// The equals.
        /// </param>
        [TestCase(null, false, null, false, null, false, false)]
        [TestCase(null, false, null, false, "", false, false)]
        [TestCase(null, false, null, false, " ", false, false)]
        [TestCase(null, false, "", false, null, false, false)]
        [TestCase(null, false, " ", false, null, false, false)]
        [TestCase(null, false, "", false, "", false, false)]
        [TestCase(null, false, "", false, " ", false, false)]
        [TestCase(null, false, " ", false, "", false, false)]
        [TestCase(null, false, " ", false, " ", false, false)]
        [TestCase("", false, null, false, null, false, false)]
        [TestCase("", false, null, false, null, false, false)]
        [TestCase("", false, null, false, "", false, false)]
        [TestCase("", false, null, false, " ", false, false)]
        [TestCase("", false, "", false, null, false, false)]
        [TestCase("", false, " ", false, null, false, false)]
        [TestCase("", false, "", false, "", false, false)]
        [TestCase("", false, "", false, " ", false, false)]
        [TestCase("", false, " ", false, "", false, false)]
        [TestCase("", false, " ", false, " ", false, false)]
        [TestCase(" ", false, null, false, null, false, false)]
        [TestCase(" ", false, null, false, null, false, false)]
        [TestCase(" ", false, null, false, "", false, false)]
        [TestCase(" ", false, null, false, " ", false, false)]
        [TestCase(" ", false, "", false, null, false, false)]
        [TestCase(" ", false, " ", false, null, false, false)]
        [TestCase(" ", false, "", false, "", false, false)]
        [TestCase(" ", false, "", false, " ", false, false)]
        [TestCase(" ", false, " ", false, "", false, false)]
        [TestCase(" ", false, " ", false, " ", false, false)]
        [TestCase(null, false, null, false, "1.0", true, false)]
        [TestCase("", false, "", false, "1.0", true, false)]
        [TestCase(" ", false, " ", false, "1.0", true, false)]
        [TestCase(null, false, "TEST", true, null, false, false)]
        [TestCase("", false, "TEST", true, null, false, false)]
        [TestCase("", false, "TEST", true, "", false, false)]
        [TestCase(null, false, "TEST", true, "", false, false)]
        [TestCase(null, false, "TEST", true, "1.0", true, false)]
        [TestCase("AGENCY", true, "TEST", true, "", false, false)]
        [TestCase("AGENCY", true, "TEST", true, "1.0", true, false)]
        [TestCase("AGENCY", true, "TST", true, "2.0", true, false)]
        [TestCase("AGNCY", true, "TEST", true, "2.0", true, false)]
        [TestCase("AGENCY", true, "TEST", true, "2.0", true, true)]
        public void TestEquals(string agencyId, bool hasAgency, string id, bool hasId, string version, bool hasVersion, bool equals)
        {
            var maintRef = new MaintainableRefObjectImpl(agencyId, id, version);
            Assert.AreEqual(hasAgency, maintRef.HasAgencyId());
            Assert.AreEqual(hasVersion, maintRef.HasVersion());
            Assert.AreEqual(hasId, maintRef.HasMaintainableId());
            var moq = new Mock<IMaintainableRefObject>();
            moq.Setup(o => o.MaintainableId).Returns("TEST");
            moq.Setup(o => o.AgencyId).Returns("AGENCY");
            moq.Setup(o => o.Version).Returns("2.0");
            moq.Setup(o => o.HasAgencyId()).Returns(true);
            moq.Setup(o => o.HasVersion()).Returns(true);
            moq.Setup(o => o.HasMaintainableId()).Returns(true);
            Assert.AreEqual(equals, maintRef.Equals(moq.Object));
        }
    }
}