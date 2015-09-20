// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestEnumerations.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Tests for Enumerations
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxApiTests
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    ///     Tests for Enumerations
    /// </summary>
    [TestFixture]
    public class TestEnumerations
    {
        #region Public Methods and Operators

        /// <summary>
        /// The test base data format.
        /// </summary>
        [Test]
        public void TestBaseDataFormat()
        {
            IEnumerable<BaseDataFormat> values = BaseDataFormat.Values;
            foreach (BaseDataFormat baseDataFormat in values)
            {
                string s = baseDataFormat.ToString();
                Assert.NotNull(s);
                Trace.WriteLine(baseDataFormat + ":" + s);
                string rootNode = baseDataFormat.RootNode;
                Assert.IsTrue(rootNode == null || rootNode.Length > 0);
                Trace.WriteLine(baseDataFormat + ":" + rootNode);
            }
        }

        /// <summary>
        /// The test sdmx structure dict type.
        /// </summary>
        [Test]
        public void TestSdmxStructureDictType()
        {
            for (int i = 0; i < 1000000; i++)
            {
                SdmxStructureType x = SdmxStructureType.ParseClass("OrganisationMap");
                foreach (SdmxStructureType sdmxStructureType in SdmxStructureType.Values)
                {
                    SdmxStructureType parentStructureType = sdmxStructureType.ParentStructureType;

                    string v2Class = sdmxStructureType.V2Class;
                }
            }
        }

        /// <summary>
        /// The test sdmx structure type.
        /// </summary>
        [Test]
        public void TestSdmxStructureType()
        {
            for (int i = 0; i < 1000000; i++)
            {
                SdmxStructureType x = SdmxStructureType.ParseClass("OrganisationMap");
                foreach (SdmxStructureType sdmxStructureType in SdmxStructureType.Values)
                {
                    SdmxStructureType parentStructureType = sdmxStructureType.ParentStructureType;

                    string v2Class = sdmxStructureType.V2Class;
                }
            }
        }

        #endregion
    }
}