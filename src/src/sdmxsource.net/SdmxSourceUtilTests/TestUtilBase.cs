// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestUtilBase.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The test util base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxSourceUtilTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;

    using Estat.Sri.SdmxStructureMutableParser.Manager;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    /// The test utility base.
    /// </summary>
    public abstract class TestUtilBase
    {
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
        protected static void ReadStructureMutable(string file, Action<ISdmxObjects> test)
        {
            IStructureParsingManager parsingManager = new StructureMutableParsingManager();
            var sw = new Stopwatch();
            sw.Start();
            ISdmxObjects structureBeans;
            using (IReadableDataLocation fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IStructureWorkspace structureWorkspace = parsingManager.ParseStructures(fileReadableDataLocation);
                Assert.NotNull(structureWorkspace);
                structureBeans = structureWorkspace.GetStructureObjects(false);
                Assert.NotNull(structureBeans);
            }

            sw.Stop();
            Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "StructureParser : Reading {0} took {1}", file, sw.Elapsed));
            test(structureBeans);
        }

        /// <summary>
        /// The read structure workspace.
        /// </summary>
        /// <param name="sdmxVersion">
        /// The SDMX Version.
        /// </param>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="test">
        /// The test.
        /// </param>
        protected static void ReadStructureWorkspace(SdmxSchemaEnumType sdmxVersion, string file, Action<ISdmxObjects> test)
        {
            IStructureParsingManager parsingManager = new StructureParsingManager(sdmxVersion);
            var sw = new Stopwatch();
            sw.Start();
            ISdmxObjects structureBeans;
            using (IReadableDataLocation fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IStructureWorkspace structureWorkspace = parsingManager.ParseStructures(fileReadableDataLocation);
                Assert.NotNull(structureWorkspace);
                structureBeans = structureWorkspace.GetStructureObjects(false);
                Assert.NotNull(structureBeans);
            }

            sw.Stop();
            Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "StructureParser : Reading {0} took {1}", file, sw.Elapsed));
            test(structureBeans);
        }

        /// <summary>
        /// The read structure workspace.
        /// </summary>
        /// <param name="sdmxVersion">
        /// The SDMX Version.
        /// </param>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="test">
        /// The test.
        /// </param>
        protected static void ReadStructureMutable(string file, Action<IMaintainableObject, ISet<IMaintainableObject>> test)
        {
            Action<ISdmxObjects> action = objects =>
            {
                ISet<IMaintainableObject> allMaintinables = objects.GetAllMaintainables();
                CollectionAssert.IsNotEmpty(allMaintinables);
                foreach (IMaintainableObject maintainableObject in allMaintinables)
                {
                    test(maintainableObject, allMaintinables);
                }
            };

            ReadStructureMutable(file, action);
        }

        /// <summary>
        /// The read structure workspace.
        /// </summary>
        /// <param name="sdmxVersion">
        /// The SDMX Version.
        /// </param>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="test">
        /// The test.
        /// </param>
        protected static void ReadStructureWorkspace(SdmxSchemaEnumType sdmxVersion, string file, Action<IMaintainableObject, ISet<IMaintainableObject>> test)
        {
            Action<ISdmxObjects> action = objects =>
                {
                    ISet<IMaintainableObject> allMaintinables = objects.GetAllMaintainables();
                    CollectionAssert.IsNotEmpty(allMaintinables);
                    foreach (IMaintainableObject maintainableObject in allMaintinables)
                    {
                        test(maintainableObject, allMaintinables);
                    }
                };

            ReadStructureWorkspace(sdmxVersion, file, action);
        }

        #endregion
    }
}