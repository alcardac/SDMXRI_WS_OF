// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDataStructureUtil.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit class for <see cref="DataStructureUtil" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxSourceUtilTests
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Util.Objects;

    /// <summary>
    ///     Test unit class for <see cref="DataStructureUtil" />
    /// </summary>
    [TestFixture]
    public class TestDataStructureUtil : TestUtilBase
    {
        #region Public Methods and Operators

        /// <summary>
        /// Test method for <see cref="DataStructureUtil.GetGroupAttribtueConcepts"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [Test]
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml", 0)]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml", 10)]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml", 6)]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml", 0)]
        public void TestGetGroupAttribtueConcepts(string file, int expectedResult)
        {
            Action<IDataStructureObject> action = o =>
                {
                    IList<string> result = DataStructureUtil.GetGroupAttribtueConcepts(o);
                    Assert.NotNull(result);
                    Assert.AreEqual(expectedResult, result.Count);
                    CollectionAssert.AllItemsAreUnique(result);
                    CollectionAssert.AllItemsAreNotNull(result);
                };
            ReadStructureWorkspace(file, action);
            ReadStructureMutableDsd(file, action);
        }

        /// <summary>
        /// Test method for <see cref="DataStructureUtil.GetGroupConcepts"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="expectedGroups">
        /// The expected Groups.
        /// </param>
        /// <param name="expectedAttributes">
        /// The expected Attributes.
        /// </param>
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml", 0, 0)]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml", 1, 6)]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml", 1, 4)]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml", 0, 0)]
        public void TestGetGroupConcepts(string file, int expectedGroups, int expectedAttributes)
        {
            Action<IDataStructureObject> action = o =>
                {
                    IDictionary<string, IList<string>> result = DataStructureUtil.GetGroupConcepts(o);
                    Assert.NotNull(result);
                    Assert.AreEqual(expectedGroups, result.Count);
                    int count = 0;
                    foreach (KeyValuePair<string, IList<string>> groupAttribtueConcept in result)
                    {
                        count += groupAttribtueConcept.Value.Count;
                    }

                    Assert.AreEqual(expectedAttributes, count);
                    CollectionAssert.AllItemsAreUnique(result);
                    CollectionAssert.AllItemsAreNotNull(result);
                };
            ReadStructureWorkspace(
                file, 
                action);
            ReadStructureMutableDsd(file, action);
        }

        /// <summary>
        /// Test method for <see cref="DataStructureUtil.GetMeasureConcept"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml")]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml")]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml")]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml")]
        public void TestGetMeasureConcept(string file)
        {
            Action<IDataStructureObject> action = o =>
                {
                    string result = DataStructureUtil.GetMeasureConcept(o);
                    Assert.IsNotNullOrEmpty(result);
                };
            ReadStructureWorkspace(
                file, 
                action);
            ReadStructureMutableDsd(file, action);
        }

        /// <summary>
        /// Test method for <see cref="DataStructureUtil.GetObservationConcepts"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml", 2)]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml", 5)]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml", 5)]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml", 65)]
        public void TestGetObservationConcepts(string file, int expectedResult)
        {
            Action<IDataStructureObject> action = o =>
                {
                    IList<string> result = DataStructureUtil.GetObservationConcepts(o);
                    Assert.NotNull(result);
                    Assert.AreEqual(expectedResult, result.Count);
                    CollectionAssert.AllItemsAreUnique(result);
                    CollectionAssert.AllItemsAreNotNull(result);
                };
            ReadStructureWorkspace(
                file, 
                action);
            ReadStructureMutableDsd(file, action);
        }

        /// <summary>
        /// Test method for <see cref="DataStructureUtil.GetSeriesAndGroupAttributeConcepts"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml", 4)]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml", 6 + 10)]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml", 3 + 6)]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml", 0)]
        public void TestGetSeriesAndGroupAttributeConcepts(string file, int expectedResult)
        {
            Action<IDataStructureObject> action = o =>
                {
                    IList<string> result = DataStructureUtil.GetSeriesAndGroupAttributeConcepts(o);
                    Assert.NotNull(result);
                    Assert.AreEqual(expectedResult, result.Count);
                    CollectionAssert.AllItemsAreUnique(result);
                    CollectionAssert.AllItemsAreNotNull(result);
                };
            ReadStructureWorkspace(
                file, 
                action);
            ReadStructureMutableDsd(file, action);
        }

        /// <summary>
        /// Test method for <see cref="DataStructureUtil.GetSeriesAttribtueConcepts"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml", 4)]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml", 6)]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml", 3)]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml", 0)]
        public void TestGetSeriesAttribtueConcepts(string file, int expectedResult)
        {
            Action<IDataStructureObject> action = o =>
                {
                    IList<string> result = DataStructureUtil.GetSeriesAttribtueConcepts(o);
                    Assert.NotNull(result);
                    Assert.AreEqual(expectedResult, result.Count);
                    CollectionAssert.AllItemsAreUnique(result);
                    CollectionAssert.AllItemsAreNotNull(result);
                };
            ReadStructureWorkspace(
                file, 
                action);
            ReadStructureMutableDsd(file, action);
        }

        /// <summary>
        /// Test method for <see cref="DataStructureUtil.GetSeriesKeyConcepts"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml", 4)]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml", 7)]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml", 5)]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml", 1)]
        public void TestGetSeriesKeyConcepts(string file, int expectedResult)
        {
            Action<IDataStructureObject> action = o =>
                {
                    IList<string> result = DataStructureUtil.GetSeriesKeyConcepts(o);
                    Assert.NotNull(result);
                    Assert.AreEqual(expectedResult, result.Count);
                    CollectionAssert.AllItemsAreUnique(result);
                    CollectionAssert.AllItemsAreNotNull(result);
                };
            ReadStructureWorkspace(
                file, 
                action);
            ReadStructureMutableDsd(file, action);
        }

        /// <summary>
        /// Test method for <see cref="DataStructureUtil.GetTimeConcept"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml", "TIME")]
        [TestCase("tests/v20/ESTAT+STS+2.0.xml", "TIME_PERIOD")]
        [TestCase("tests/v20/ESTAT_CPI_v1.0.xml", "TIME_PERIOD")]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml", null)]
        public void TestGetTimeConcept(string file, string expectedResult)
        {
            Action<IDataStructureObject> action = o =>
                {
                    string result = DataStructureUtil.GetTimeConcept(o);
                    if (expectedResult != null)
                    {
                        Assert.AreEqual(expectedResult, result);
                    }
                    else
                    {
                        Assert.IsNullOrEmpty(result);
                    }
                };
            ReadStructureWorkspace(
                file, 
                action);
            ReadStructureMutableDsd(file, action);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The read structure workspace.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="test">
        /// The test.
        /// </param>
        private static void ReadStructureMutableDsd(string file, Action<IDataStructureObject> test)
        {
            Action<ISdmxObjects> action = objects =>
                {
                    CollectionAssert.IsNotEmpty(objects.DataStructures);
                    foreach (IDataStructureObject dataStructureObject in objects.DataStructures)
                    {
                        test(dataStructureObject);
                    }
                };

            ReadStructureMutable(file, action);
        }

        /// <summary>
        /// The read structure workspace.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="test">
        /// The test.
        /// </param>
        private static void ReadStructureWorkspace(string file, Action<IDataStructureObject> test)
        {
            Action<ISdmxObjects> action = objects =>
            {
                CollectionAssert.IsNotEmpty(objects.DataStructures);
                foreach (IDataStructureObject dataStructureObject in objects.DataStructures)
                {
                    test(dataStructureObject);
                }
            };

            ReadStructureWorkspace(SdmxSchemaEnumType.VersionTwo, file, action);
        }

        #endregion
    }
}