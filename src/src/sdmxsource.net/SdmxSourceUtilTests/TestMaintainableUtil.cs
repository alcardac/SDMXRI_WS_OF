// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestMaintainableUtil.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit class for <see cref="MaintainableUtil{T}" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxSourceUtilTests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Moq;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Util.Objects;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     Test unit class for <see cref="MaintainableUtil{T}" />
    /// </summary>
    [TestFixture]
    public class TestMaintainableUtil : TestUtilBase
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Test method for <see cref="MaintainableUtil{T}.FilterCollection" />
        /// </summary>
        public void TestFilterCollection()
        {
            var dataflows = new List<IDataflowObject>();
            for (int i = 0; i < 5; i++)
            {
                var moq = new Mock<IDataflowObject>();
                moq.Setup(o => o.AgencyId).Returns((i % 2) == 0 ? "ESTAT" : "TEST");
                moq.Setup(o => o.Id).Returns("ID" + i.ToString(CultureInfo.InvariantCulture));
                moq.Setup(o => o.Version).Returns("1." + i.ToString(CultureInfo.InvariantCulture));
                dataflows.Add(moq.Object);
            }

            var dataflowUtil = new MaintainableUtil<IDataflowObject>();
            CollectionAssert.AreEquivalent(dataflows, dataflowUtil.FilterCollection(dataflows, null));
            CollectionAssert.AreEquivalent(dataflows, dataflowUtil.FilterCollection(dataflows, new MaintainableRefObjectImpl()));
            CollectionAssert.IsSubsetOf(dataflowUtil.FilterCollection(dataflows, new MaintainableRefObjectImpl("ESTAT", null, null)), dataflows);
            CollectionAssert.AreNotEquivalent(dataflowUtil.FilterCollection(dataflows, new MaintainableRefObjectImpl("ESTAT", null, null)), dataflows);
            CollectionAssert.IsSubsetOf(dataflowUtil.FilterCollection(dataflows, new MaintainableRefObjectImpl("TEST", null, null)), dataflows);
            CollectionAssert.AreNotEquivalent(dataflowUtil.FilterCollection(dataflows, new MaintainableRefObjectImpl("TEST", null, null)), dataflows);
            CollectionAssert.IsSubsetOf(dataflowUtil.FilterCollection(dataflows, new MaintainableRefObjectImpl(null, "ID1", null)), dataflows);
            CollectionAssert.AreNotEquivalent(dataflowUtil.FilterCollection(dataflows, new MaintainableRefObjectImpl(null, "ID1", null)), dataflows);
            CollectionAssert.IsSubsetOf(dataflowUtil.FilterCollection(dataflows, new MaintainableRefObjectImpl(null, "ID1", "1.1")), dataflows);
            CollectionAssert.AreNotEquivalent(dataflowUtil.FilterCollection(dataflows, new MaintainableRefObjectImpl(null, "ID1", "1.1")), dataflows);
            CollectionAssert.IsSubsetOf(dataflowUtil.FilterCollection(dataflows, new MaintainableRefObjectImpl(null, null, "1.1")), dataflows);
            CollectionAssert.AreNotEquivalent(dataflowUtil.FilterCollection(dataflows, new MaintainableRefObjectImpl(null, null, "1.1")), dataflows);
            CollectionAssert.IsEmpty(dataflowUtil.FilterCollection(dataflows, new MaintainableRefObjectImpl("NOTHING", null, null)));
            CollectionAssert.IsEmpty(dataflowUtil.FilterCollection(dataflows, new MaintainableRefObjectImpl(null, "NO", null)));
            CollectionAssert.IsEmpty(dataflowUtil.FilterCollection(dataflows, new MaintainableRefObjectImpl(null, null, "5.0")));
        }

        /// <summary>
        /// Test method for
        ///     <see cref="MaintainableUtil{T}.FindMatches{TMaint}(System.Collections.Generic.ICollection{Org.Sdmxsource.Sdmx.Api.Model.Objects.Base.IMaintainableObject},System.Collections.Generic.ICollection{TMaint},Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.IStructureReference)"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v21/demography.xml")]
        [TestCase("tests/v21/ecb_exr_ng_full.xml")]
        [TestCase("tests/v21/repsonse_cl_all.xml")]
        public void TestFindMatchV21(string file)
        {
            Action<IMaintainableObject, ISet<IMaintainableObject>> action = (maintainableObject, maintainableObjects) =>
                {
                    Assert.AreEqual(1, MaintainableUtil<IMaintainableObject>.FindMatches(maintainableObjects, maintainableObject.AsReference).Count);
                    Assert.AreEqual(
                        1, 
                        MaintainableUtil<IMaintainableObject>.FindMatches(maintainableObjects, new StructureReferenceImpl(maintainableObject.AgencyId, maintainableObject.Id, maintainableObject.Version, maintainableObject.StructureType)).Count);
                    CollectionAssert.IsEmpty(
                        MaintainableUtil<IMaintainableObject>.FindMatches(maintainableObjects, new StructureReferenceImpl(maintainableObject.AgencyId + "A", maintainableObject.Id, maintainableObject.Version, maintainableObject.StructureType)));
                    CollectionAssert.IsEmpty(
                        MaintainableUtil<IMaintainableObject>.FindMatches(maintainableObjects, new StructureReferenceImpl(maintainableObject.AgencyId, maintainableObject.Id + "NO", maintainableObject.Version, maintainableObject.StructureType)));
                    CollectionAssert.IsEmpty(
                        MaintainableUtil<IMaintainableObject>.FindMatches(maintainableObjects, new StructureReferenceImpl(maintainableObject.AgencyId, maintainableObject.Id, maintainableObject.Version + ".1", maintainableObject.StructureType)));
                };

            ReadStructureWorkspace(SdmxSchemaEnumType.VersionTwoPointOne, file, action);
        }

        /// <summary>
        /// Test method for
        ///     <see cref="MaintainableUtil{T}.FindMatches{TMaint}(System.Collections.Generic.ICollection{Org.Sdmxsource.Sdmx.Api.Model.Objects.Base.IMaintainableObject},System.Collections.Generic.ICollection{TMaint},Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.IStructureReference)"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v20/CENSAGR_CAPOAZ_GEN+IT1+1.3.xml")]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml")]
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml")]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml")]
        [TestCase("tests/v20/CL_SEX_v1.1.xml")]
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml")]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE+2.0.xml")]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE_NZ+2.1.xml")]
        [TestCase("tests/v20/ESTAT+SSTSCONS_PROD_M+2.0.xml")]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/ESTAT+TESTLEVELS+1.0.xml")]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml")]
        [TestCase("tests/v20/queryResponse-estat-sts.xml")]
        [TestCase("tests/v20/QueryResponseDataflowCategories.xml")]
        public void TestFindMatchesV20(string file)
        {
            Action<IMaintainableObject, ISet<IMaintainableObject>> action = (maintainableObject, maintainableObjects) =>
                {
                    Assert.AreEqual(1, MaintainableUtil<IMaintainableObject>.FindMatches(maintainableObjects, maintainableObject.AsReference).Count);
                    Assert.AreEqual(
                        1, 
                        MaintainableUtil<IMaintainableObject>.FindMatches(
                            maintainableObjects, new StructureReferenceImpl(maintainableObject.AgencyId, maintainableObject.Id, maintainableObject.Version, maintainableObject.StructureType)).Count);
                    CollectionAssert.IsEmpty(
                        MaintainableUtil<IMaintainableObject>.FindMatches(
                            maintainableObjects, new StructureReferenceImpl(maintainableObject.AgencyId + "A", maintainableObject.Id, maintainableObject.Version, maintainableObject.StructureType)));
                    CollectionAssert.IsEmpty(
                        MaintainableUtil<IMaintainableObject>.FindMatches(
                            maintainableObjects, new StructureReferenceImpl(maintainableObject.AgencyId, maintainableObject.Id + "NO", maintainableObject.Version, maintainableObject.StructureType)));
                    CollectionAssert.IsEmpty(
                        MaintainableUtil<IMaintainableObject>.FindMatches(
                            maintainableObjects, new StructureReferenceImpl(maintainableObject.AgencyId, maintainableObject.Id, maintainableObject.Version + ".1", maintainableObject.StructureType)));
                };

            ReadStructureWorkspace(SdmxSchemaEnumType.VersionTwo, file, action);
            ReadStructureMutable(file, action);
        }

        /// <summary>
        /// Test method for
        ///     <see cref="MaintainableUtil{T}.FindMatches{TMaint}(System.Collections.Generic.ICollection{Org.Sdmxsource.Sdmx.Api.Model.Objects.Base.IMaintainableObject},System.Collections.Generic.ICollection{TMaint},Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.IStructureReference)"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v20/CENSAGR_CAPOAZ_GEN+IT1+1.3.xml")]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml")]
        [TestCase("tests/v20/CATEGORY_SCHEME_ESTAT_DATAFLOWS_SCHEME.xml")]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml")]
        [TestCase("tests/v20/CL_SEX_v1.1.xml")]
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml")]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE+2.0.xml")]
        [TestCase("tests/v20/ESTAT+HCL_SAMPLE_NZ+2.1.xml")]
        [TestCase("tests/v20/ESTAT+SSTSCONS_PROD_M+2.0.xml")]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/ESTAT+TESTLEVELS+1.0.xml")]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml")]
        [TestCase("tests/v20/queryResponse-estat-sts.xml")]
        [TestCase("tests/v20/QueryResponseDataflowCategories.xml")]
        public void TestResolveReferenceV20(string file)
        {
            Action<IMaintainableObject, ISet<IMaintainableObject>> action = (maintainableObject, maintainableObjects) =>
                {
                    IMaintainableObject resolveReference = MaintainableUtil<IMaintainableObject>.ResolveReference(maintainableObjects, maintainableObject.AsReference);
                    Assert.AreEqual(maintainableObject, resolveReference);
                    resolveReference = MaintainableUtil<IMaintainableObject>.ResolveReference(maintainableObjects, maintainableObject.AsReference.MaintainableReference);
                    Assert.AreEqual(maintainableObject, resolveReference);
                };

            ReadStructureWorkspace(SdmxSchemaEnumType.VersionTwo, file, action);
            ReadStructureMutable(file, action);
        }

        /// <summary>
        /// Test method for
        ///     <see cref="MaintainableUtil{T}.FindMatches{TMaint}(System.Collections.Generic.ICollection{Org.Sdmxsource.Sdmx.Api.Model.Objects.Base.IMaintainableObject},System.Collections.Generic.ICollection{TMaint},Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.IStructureReference)"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v21/demography.xml")]
        [TestCase("tests/v21/ecb_exr_ng_full.xml")]
        [TestCase("tests/v21/repsonse_cl_all.xml")]
        public void TestResolveReferenceV21(string file)
        {
            Action<IMaintainableObject, ISet<IMaintainableObject>> action = (maintainableObject, maintainableObjects) =>
                {
                    IMaintainableObject resolveReference = MaintainableUtil<IMaintainableObject>.ResolveReference(maintainableObjects, maintainableObject.AsReference);
                    Assert.AreEqual(maintainableObject, resolveReference);
                    resolveReference = MaintainableUtil<IMaintainableObject>.ResolveReference(maintainableObjects, maintainableObject.AsReference.MaintainableReference);
                    Assert.AreEqual(maintainableObject, resolveReference);
                };

            ReadStructureWorkspace(SdmxSchemaEnumType.VersionTwoPointOne, file, action);
        }

        #endregion
    }
}