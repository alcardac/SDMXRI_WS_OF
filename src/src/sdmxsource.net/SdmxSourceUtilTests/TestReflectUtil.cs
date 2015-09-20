// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestReflectUtil.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit for <see cref="ReflectUtil{T}" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxSourceUtilTests
{
    using System.Collections.Generic;
    using System.Reflection;

    using NUnit.Framework;

    using Org.Sdmxsource.Util.Reflect;

    /// <summary>
    ///     Test unit for <see cref="ReflectUtil{T}" />
    /// </summary>
    [TestFixture]
    public class TestReflectUtil
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Test unit for <see cref="ReflectUtil{T}.GetCompositeObjects" />
        /// </summary>
        [Test]
        public void TestGetCompositeObjects()
        {
            var testReflect = new ReflectUtil<string>();
            ISet<string> set = testReflect.GetCompositeObjects(new TestClass(0, new HashSet<string>() { "1" }, new string[] {"2"}));
            Assert.NotNull(set);
            CollectionAssert.IsNotEmpty(set);
            CollectionAssert.AllItemsAreNotNull(set);
            CollectionAssert.AllItemsAreInstancesOfType(set, typeof(string));
            Assert.AreEqual(2, set.Count);
            CollectionAssert.Contains(set, "1");
            CollectionAssert.Contains(set, "2");
        }

        /// <summary>
        ///     Test unit for <see cref="ReflectUtil{T}.GetCompositeObjects" />
        /// </summary>
        [Test]
        public void TestGetCompositeObjectsIgnore()
        {
            PropertyInfo propertyInfo = typeof(TestClass).GetProperty("Field2");

            var testReflect = new ReflectUtil<string>();

            ISet<string> set = testReflect.GetCompositeObjects(new TestClass(0, new HashSet<string>() { "1" }, new string[] {"2"}), propertyInfo);
            Assert.NotNull(set);
            CollectionAssert.IsNotEmpty(set);
            CollectionAssert.AllItemsAreNotNull(set);
            CollectionAssert.AllItemsAreInstancesOfType(set, typeof(string));
            Assert.AreEqual(1, set.Count);
            CollectionAssert.Contains(set, "1");
        }

        #endregion

        /// <summary>
        ///     The test class.
        /// </summary>
        private sealed class TestClass
        {
            #region Fields

            /// <summary>
            ///     The _field 1.
            /// </summary>
            private int _field1;

            /// <summary>
            ///     The _field 3.
            /// </summary>
            private ISet<string> _field3;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="TestClass"/> class.
            /// </summary>
            /// <param name="field1">
            /// The field 1.
            /// </param>
            /// <param name="field3">
            /// The field 3.
            /// </param>
            /// <param name="field2">
            /// The field 2.
            /// </param>
            public TestClass(int field1, ISet<string> field3, IList<string> field2)
            {
                this._field1 = field1;
                this._field3 = field3;
                this.Field2 = field2;
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets or sets the field 1.
            /// </summary>
            public int Field1
            {
                get
                {
                    return this._field1;
                }

                set
                {
                    this._field1 = value;
                }
            }

            /// <summary>
            ///     Gets the field 2.
            /// </summary>
            public IList<string> Field2 { get; private set; }

            /// <summary>
            ///     Gets or sets the field 3.
            /// </summary>
            public ISet<string> Field3
            {
                get
                {
                    return this._field3;
                }

                set
                {
                    this._field3 = value;
                }
            }

            #endregion
        }
    }
}