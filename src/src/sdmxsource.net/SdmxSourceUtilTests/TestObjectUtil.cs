// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestObjectUtil.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit for <see cref="ObjectUtil" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxSourceUtilTests
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     Test unit for <see cref="ObjectUtil" />
    /// </summary>
    [TestFixture]
    public class TestObjectUtil
    {
        #region Public Methods and Operators

        /// <summary>
        /// Test unit for <see cref="ObjectUtil.Equivalent"/>
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase(1, 2, false)]
        [TestCase("PAOK", "OLE", false)]
        [TestCase(4, "4", false)]
        [TestCase(4, 4, true)]
        [TestCase(SdmxStructureEnumType.Category, SdmxStructureEnumType.Category, true)]
        [TestCase(SdmxStructureEnumType.Category, SdmxStructureEnumType.Dataflow, false)]
        [TestCase("4", "4", true)]
        [TestCase(null, "4", false)]
        [TestCase(null, null, true)]
        public void TestEquivalent(object a, object b, bool expectedResult)
        {
            Assert.IsTrue(ObjectUtil.Equivalent(a, b) == expectedResult);
        }

        /// <summary>
        /// Test unit for <see cref="ObjectUtil.Equivalent"/>
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase(new[] { "PAOK" }, new[] { "OLE" }, false)]
        [TestCase(new[] { "PAOK" }, new[] { "PAOK" }, true)]
        [TestCase(new[] { "PAOK" }, new string[] { }, false)]
        [TestCase(new[] { "PAOK", "OLE" }, new[] { "OLE" }, false)]
        [TestCase(new[] { "OLE", "OLE" }, new[] { "OLE" }, false)]
        [TestCase(new[] { "PAOK", "OLE" }, new[] { "OLE", "PAOK" }, false)]
        [TestCase(new[] { "PAOK", "OLE" }, new[] { "PAOK", "OLE" }, true)]
        [TestCase(null, new[] { "4" }, false)]
        [TestCase(null, new string[] { }, false)]
        [TestCase(null, null, true)]
        public void TestEquivalentCollection(ICollection<string> a, ICollection<string> b, bool expectedResult)
        {
            Assert.IsTrue(ObjectUtil.EquivalentCollection(a, b) == expectedResult);
        }

        /// <summary>
        /// Test unit for <see cref="ObjectUtil.Equivalent"/>
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase(new[] { SdmxStructureEnumType.Category, SdmxStructureEnumType.Category }, 
            new[] { SdmxStructureEnumType.CategoryScheme, SdmxStructureEnumType.Category }, false)]
        [TestCase(new[] { SdmxStructureEnumType.CategoryScheme, SdmxStructureEnumType.Category }, 
            new[] { SdmxStructureEnumType.CategoryScheme, SdmxStructureEnumType.Category }, true)]
        public void TestEquivalentCollection(
            ICollection<SdmxStructureEnumType> a, ICollection<SdmxStructureEnumType> b, bool expectedResult)
        {
            Assert.IsTrue(ObjectUtil.EquivalentCollection(a, b) == expectedResult);
        }

        /// <summary>
        /// Test unit for <see cref="ObjectUtil.Equivalent"/>
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase(new int[] { }, new[] { 1, 2 }, false)]
        [TestCase(new int[] { }, new[] { 1, 2 }, false)]
        [TestCase(new[] { 1 }, new int[] { }, false)]
        [TestCase(new[] { 2, 1 }, new[] { 1, 2 }, false)]
        [TestCase(new[] { 2, 2 }, new[] { 2, 2 }, true)]
        [TestCase(new[] { 2, 2 }, new[] { 1, 2 }, false)]
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 2 }, false)]
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4 }, false)]
        [TestCase(new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 4 }, true)]
        [TestCase(new[] { 1 }, new[] { 2 }, false)]
        [TestCase(null, new[] { 2 }, false)]
        [TestCase(null, null, true)]
        public void TestEquivalentCollection(ICollection<int> a, ICollection<int> b, bool expectedResult)
        {
            Assert.IsTrue(ObjectUtil.EquivalentCollection(a, b) == expectedResult);
        }

        /// <summary>
        /// Test unit for <see cref="ObjectUtil.Equivalent"/>
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase(new[] { 1.0 }, new[] { 2.0 }, false)]
        [TestCase(new[] { 2.0 }, new[] { 2.0 }, true)]
        [TestCase(new[] { 1.0, 2.0 }, new[] { 2.0 }, false)]
        [TestCase(null, null, true)]
        public void TestEquivalentCollection(ICollection<double> a, ICollection<double> b, bool expectedResult)
        {
            Assert.IsTrue(ObjectUtil.EquivalentCollection(a, b) == expectedResult);
        }

        /// <summary>
        /// Test unit for <see cref="ObjectUtil.EquivalentString"/>
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("PAOK", "OLE", false)]
        [TestCase("PAOK", "PAOK", true)]
        [TestCase("", "", true)]
        [TestCase("41", "4", false)]
        [TestCase("4", "4", true)]
        [TestCase("A", "a", false)]
        [TestCase(null, "4", false)]
        [TestCase(null, "", false)]
        [TestCase(null, null, true)]
        public void TestEquivalentString(string a, string b, bool expectedResult)
        {
            Assert.IsTrue(ObjectUtil.EquivalentString(a, b) == expectedResult);
        }

        /// <summary>
        /// Test unit for <see cref="ObjectUtil.ValidArray"/>
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase(new[] { "PAOK" }, true)]
        [TestCase(new[] { "PAOK", "OLE" }, true)]
        [TestCase(new object[] { }, false)]
        [TestCase(null, false)]
        public void TestValidArray(object[] a, bool expectedResult)
        {
            Assert.IsTrue(ObjectUtil.ValidArray(a) == expectedResult);
        }

        /// <summary>
        /// Test unit for <see cref="ObjectUtil.ValidArray"/>
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase(new[] { "PAOK" }, true)]
        [TestCase(new[] { "PAOK", "OLE" }, true)]
        [TestCase(new string[] { }, false)]
        [TestCase(null, false)]
        public void TestValidArray(string[] a, bool expectedResult)
        {
            Assert.IsTrue(ObjectUtil.ValidArray(a) == expectedResult);
        }

        /// <summary>
        /// Test unit for <see cref="ObjectUtil.ValidCollection"/>
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase(new[] { "PAOK" }, true)]
        [TestCase(new[] { "PAOK", "OLE" }, true)]
        [TestCase(new string[] { }, false)]
        [TestCase(null, false)]
        public void TestValidArray(ICollection<string> a, bool expectedResult)
        {
            Assert.IsTrue(ObjectUtil.ValidCollection(a) == expectedResult);
        }

        /// <summary>
        /// Test unit for <see cref="ObjectUtil.ValidObject"/>
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase(new[] { "PAOK" }, true)]
        [TestCase(new[] { "PAOK", "OLE" }, true)]
        [TestCase(new[] { "PAOK", "OLE", null }, false)]
        [TestCase(new[] { null, "PAOK", "OLE" }, false)]
        [TestCase(new[] { "PAOK", null, "OLE" }, false)]
        //// The following returns true
        [TestCase(new object[] { }, false)]
        [TestCase(new object[] { null }, false)]
        //// The following causes a null pointer exception
        [TestCase(null, false)]
        public void TestValidObject(object[] a, bool expectedResult)
        {
            Assert.IsTrue(ObjectUtil.ValidObject(a) == expectedResult);
        }

        /// <summary>
        /// Test unit for <see cref="ObjectUtil.ValidOneString"/>
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase(new[] { "PAOK" }, true)]
        [TestCase(new[] { "PAOK", "OLE" }, true)]
        [TestCase(new[] { "PAOK", "OLE", null }, true)]
        [TestCase(new[] { null, "PAOK", "OLE" }, true)]
        [TestCase(new[] { "PAOK", null, "OLE" }, true)]
        [TestCase(new[] { null, "OLE" }, true)]
        [TestCase(new[] { null, "" }, false)]
        [TestCase(new[] { "" }, false)]
        [TestCase(new[] { "                 " }, false)]
        [TestCase(new string[] { null, null }, false)]
        [TestCase(new string[] { null, null, null }, false)]
        [TestCase(new string[] { null }, false)]
        [TestCase(new string[] { }, false)]
        [TestCase(null, false)]
        public void TestValidOneString(string[] a, bool expectedResult)
        {
            Assert.IsTrue(ObjectUtil.ValidOneString(a) == expectedResult);
        }

        /// <summary>
        /// Test unit for <see cref="ObjectUtil.ValidString(string[])"/>
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase(new[] { "PAOK" }, true)]
        [TestCase(new[] { "PAOK", "OLE" }, true)]
        [TestCase(new[] { null, "PAOK", "OLE" }, false)]
        [TestCase(new[] { "PAOK", "OLE", null }, false)]
        [TestCase(new[] { "", "PAOK", "OLE" }, false)]
        [TestCase(new[] { " ", "PAOK", "OLE" }, false)]
        [TestCase(new[] { "PAOK", null, "OLE" }, false)]
        [TestCase(new[] { "PAOK", "", "OLE" }, false)]
        [TestCase(new[] { "PAOK", "  ", "OLE" }, false)]
        [TestCase(new[] { null, "OLE" }, false)]
        [TestCase(new[] { "", "OLE" }, false)]
        [TestCase(new[] { " ", "OLE" }, false)]
        [TestCase(new string[] { null, null }, false)]
        [TestCase(new[] { null, "" }, false)]
        [TestCase(new[] { "" }, false)]
        [TestCase(new[] { "                 " }, false)]
        [TestCase(new string[] { null, null, null }, false)]
        [TestCase(new string[] { null }, false)]
        [TestCase(new string[] { }, false)]
        [TestCase(null, false)]
        public void TestValidString(string[] a, bool expectedResult)
        {
            Assert.IsTrue(ObjectUtil.ValidString(a) == expectedResult);
        }

        /// <summary>
        /// Test unit for <see cref="ObjectUtil.ValidString(Uri)"/>
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="expectedResult">
        /// The expected Result.
        /// </param>
        [TestCase("file://PAOK", true)]
        [TestCase("", false)]
        [TestCase(null, false)]
        [TestCase("urn:1", true)]
        public void TestValidUri(string a, bool expectedResult)
        {
            if (string.IsNullOrWhiteSpace(a))
            {
                Assert.IsTrue(ObjectUtil.ValidString((Uri)null) == expectedResult);
            }
            else
            {
                Assert.IsTrue(ObjectUtil.ValidString(new Uri(a)) == expectedResult);
            }
        }

        #endregion
    }
}