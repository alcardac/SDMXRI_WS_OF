// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReUsingStructureParsingManagerFast.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The re using structure parsing manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ReUsingExamples.Structure
{
    using System;
    using System.IO;

    using Estat.Sri.SdmxStructureMutableParser.Manager;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Util.Io;
    using Org.Sdmxsource.XmlHelper;

    /// <summary>
    /// The re using structure parsing manager.
    /// </summary>
    public class ReUsingStructureParsingManagerFast
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
            // 1. Initialize the StructureMutableParsingManager. This implementation supports only 2.0. It also supports the SdmxXmlStream
            IStructureParsingManager parsingManager = new StructureMutableParsingManager();

            // 2. Create a SdmxXmlStream. It provides a non-buffered, forward only access to an existing XmlReader or XmlWriter. 
            using (var stream = File.OpenRead("ESTAT_CPI_v1.0.xml"))
            using (var reader = XMLParser.CreateSdmxMlReader(stream, SdmxSchemaEnumType.VersionTwo))
            using (var dataLocation = new SdmxXmlStream(reader, MessageEnumType.Structure, SdmxSchemaEnumType.VersionTwo, RegistryMessageEnumType.Null))
            {
                // 3. Get the IStructureWorkspace
                IStructureWorkspace structureWorkspace = parsingManager.ParseStructures(dataLocation);

                // 4. Get the ISdmxObjects with resolving cross-references.
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