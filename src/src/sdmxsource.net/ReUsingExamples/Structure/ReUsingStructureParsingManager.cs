// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReUsingStructureParsingManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The re using structure parsing manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ReUsingExamples.Structure
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    /// The re using structure parsing manager.
    /// </summary>
    public class ReUsingStructureParsingManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Could not load Structure objects
        /// </exception>
        public static void Main(string[] args)
        {
            // 1. Initialize the StructureParsingManager. This implementation supports both 2.0 and 2.1.
            IStructureParsingManager parsingManager = new StructureParsingManager(SdmxSchemaEnumType.VersionTwo);

            // 2. open IReadableDataLocation
            using (IReadableDataLocation dataLocation = new FileReadableDataLocation("ESTAT_CPI_v1.0.xml"))
            {
                // 3. Get the IStructureWorkspace
                IStructureWorkspace structureWorkspace = parsingManager.ParseStructures(dataLocation);

                // 4. Get the ISdmxObjects without resolving cross-references.
                ISdmxObjects structureObjects = structureWorkspace.GetStructureObjects(false);

                if (structureObjects == null)
                {
                    throw new InvalidOperationException("Could not load Structure objects");
                }
            }
        }

        #endregion
    }
}