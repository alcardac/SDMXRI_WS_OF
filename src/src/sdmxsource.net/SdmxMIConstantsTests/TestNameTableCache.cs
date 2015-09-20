// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestNameTableCache.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit for <see cref="NameTableCache" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxMlConstantsTests
{
    using Estat.Sri.SdmxXmlConstants;

    using NUnit.Framework;

    /// <summary>
    ///     Test unit for <see cref="NameTableCache" />
    /// </summary>
    [TestFixture]
    public class TestNameTableCache
    {
        #region Public Methods and Operators

        /// <summary>
        /// The test is attribute.
        /// </summary>
        /// <param name="localName">
        /// The local name.
        /// </param>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="shouldEqual">
        /// The should equal.
        /// </param>
        [Test]
        [TestCase("id", AttributeNameTable.id, true)]
        [TestCase("returnDetails", AttributeNameTable.returnDetails, true)]
        [TestCase("FooStructure", AttributeNameTable.returnDetails, false)]
        public void TestIsAttribute(string localName, AttributeNameTable element, bool shouldEqual)
        {
            string add = NameTableCache.Instance.NameTable.Add(localName);
            Assert.IsTrue(NameTableCache.IsAttribute(add, element) == shouldEqual);
        }

        /// <summary>
        /// Test unit for <see cref="NameTableCache.IsElement"/>
        /// </summary>
        /// <param name="localName">
        /// The local Name.
        /// </param>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="shouldEqual">
        /// The should Equal.
        /// </param>
        [Test]
        [TestCase("Structure", ElementNameTable.Structure, true)]
        [TestCase("FooStructure", ElementNameTable.Structure, false)]
        public void TestIsElement(string localName, ElementNameTable element, bool shouldEqual)
        {
            string add = NameTableCache.Instance.NameTable.Add(localName);
            Assert.IsTrue(NameTableCache.IsElement(add, element) == shouldEqual);
        }

        #endregion
    }
}