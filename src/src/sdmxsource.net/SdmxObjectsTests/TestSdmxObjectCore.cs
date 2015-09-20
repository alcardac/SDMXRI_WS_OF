// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestSdmxObjectCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit for <see cref="SdmxObjectCore" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxObjectsTests
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    ///     Test unit for <see cref="SdmxObjectCore" />
    /// </summary>
    [TestFixture]
    public class TestSdmxObjectCore
    {
        #region Public Methods and Operators

        /// <summary>
        /// Test unit for <see cref="SdmxObjectCore.Composites"/>
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml")]
        public void TestComposites(string file)
        {
            var structureParsing = new StructureParsingManager(SdmxSchemaEnumType.VersionTwo);
            using (var readable = new FileReadableDataLocation(file))
            {
                IStructureWorkspace structureWorkspace = structureParsing.ParseStructures(readable);
                ISdmxObjects objects = structureWorkspace.GetStructureObjects(false);
                foreach (IMaintainableObject maintainableObject in objects.GetAllMaintainables())
                {
                    ISet<ISdmxObject> sdmxObjects = maintainableObject.Composites;
                    Assert.IsNotEmpty(sdmxObjects);
                }
            }
        }

        #endregion
    }
}