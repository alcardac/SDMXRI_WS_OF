// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestMaintainableSortByIdentifiers.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit class for <see cref="MaintainableSortByIdentifiers{T}" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxSourceUtilTests
{
    using Moq;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Util.Sort;

    /// <summary>
    ///     Test unit class for <see cref="MaintainableSortByIdentifiers{T}" />
    /// </summary>
    [TestFixture]
    public class TestMaintainableSortByIdentifiers
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Test method for <see cref="MaintainableSortByIdentifiers{T}.Compare" />
        /// </summary>
        [Test]
        public void TestCompare()
        {
            var moq = new Mock<ICodelistObject>();
            moq.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList));
            moq.Setup(o => o.Id).Returns("Test");
            moq.Setup(o => o.AgencyId).Returns("TestAgency");
            moq.Setup(o => o.Version).Returns("1.0");

            var comp = new MaintainableSortByIdentifiers<ICodelistObject>();
            Assert.IsTrue(comp.Compare(moq.Object, moq.Object) == 0);

            var moq2 = new Mock<ICodelistObject>();
            moq2.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList));
            moq2.Setup(o => o.Id).Returns("Test");
            moq2.Setup(o => o.AgencyId).Returns("TestAgency");
            moq2.Setup(o => o.Version).Returns("1.0");

            // when they are different object but have the same agency, id and version it would return -1. TODO check with MT.
            Assert.IsTrue(comp.Compare(moq.Object, moq2.Object) == -1);

            var moq3 = new Mock<ICodelistObject>();
            moq3.Setup(o => o.StructureType).Returns(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList));
            moq3.Setup(o => o.Id).Returns("Test4");
            moq3.Setup(o => o.AgencyId).Returns("TestAgency");
            moq3.Setup(o => o.Version).Returns("1.0");
            Assert.IsFalse(comp.Compare(moq.Object, moq3.Object) == 0);
            moq3.Setup(o => o.Id).Returns("Test");
            moq3.Setup(o => o.AgencyId).Returns("TestAgencyA");
            Assert.IsFalse(comp.Compare(moq.Object, moq3.Object) == 0);
            moq3.Setup(o => o.AgencyId).Returns("TestAgency");
            moq3.Setup(o => o.Version).Returns("2.0");
            Assert.IsFalse(comp.Compare(moq.Object, moq3.Object) == 0);
            Assert.IsFalse(comp.Compare(moq.Object, null) == 0);
            Assert.IsFalse(comp.Compare(null, moq3.Object) == 0);
            Assert.IsTrue(comp.Compare(null, null) == 0);
        }

        #endregion
    }
}