// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReUsingGenericWriter.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The re using compact writer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ReUsingExamples.DataWriting
{
    using System;
    using System.Linq;
    using System.Xml;

    using Estat.Sri.SdmxStructureMutableParser.Manager;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    /// The re using compact writer.
    /// </summary>
    public class ReUsingGenericWriter
    {
        #region Static Fields

        /// <summary>
        ///     The parsing manager.
        /// </summary>
        private static readonly IStructureParsingManager _parsingManager = new StructureMutableParsingManager();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        public static void Main(string[] args)
        {
            // 1. We need a IDataStructureObject. In this example we read it from a file. Alternative we could build it from a mutable object.
            IDataStructureObject dataStructure;
            using (IReadableDataLocation readable = new FileReadableDataLocation("ESTAT+STS+2.0.xml"))
            {
                IStructureWorkspace structureWorkspace = _parsingManager.ParseStructures(readable);

                ISdmxObjects structureObjects = structureWorkspace.GetStructureObjects(false);
                dataStructure = structureObjects.DataStructures.FirstOrDefault();
            }

            if (dataStructure == null)
            {
                throw new InvalidOperationException("Could not build dataStructure object");
            }

            using (XmlWriter writer = XmlWriter.Create("re-using-generic-writer.xml", new XmlWriterSettings { Indent = true }))
            {
                // initialize the data writing engine. It can be for SDMX versions 2.0 or 2.1
                IDataWriterEngine dataWriterEngine = new GenericDataWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));

                // write header
                dataWriterEngine.WriteHeader(new HeaderImpl("ZZ9", "ZZ9"));

                // start dataset
                dataWriterEngine.StartDataset(null, dataStructure, null);

                // write dataset attributes
                dataWriterEngine.WriteAttributeValue("TITLE", "GENERIC test");

                // write 2 group entries
                dataWriterEngine.StartGroup("SIBLING");
                dataWriterEngine.WriteGroupKeyValue("REF_AREA", "EL");
                dataWriterEngine.WriteGroupKeyValue("STS_INDICATOR", "PROD");
                dataWriterEngine.WriteGroupKeyValue("STS_ACTIVITY", "NS0030");
                dataWriterEngine.WriteGroupKeyValue("STS_INSTITUTION", "1");
                dataWriterEngine.WriteGroupKeyValue("ADJUSTMENT", "N");
                dataWriterEngine.WriteAttributeValue("COMPILATION", "test");

                dataWriterEngine.StartGroup("SIBLING");
                dataWriterEngine.WriteGroupKeyValue("REF_AREA", "EL");
                dataWriterEngine.WriteGroupKeyValue("STS_INDICATOR", "IND");
                dataWriterEngine.WriteGroupKeyValue("STS_ACTIVITY", "NS0030");
                dataWriterEngine.WriteGroupKeyValue("STS_INSTITUTION", "1");
                dataWriterEngine.WriteGroupKeyValue("ADJUSTMENT", "N");
                dataWriterEngine.WriteAttributeValue("COMPILATION", "test2");

                // write a series entry
                dataWriterEngine.StartSeries();
                dataWriterEngine.WriteSeriesKeyValue("FREQ", "A");
                dataWriterEngine.WriteSeriesKeyValue("REF_AREA", "EL");
                dataWriterEngine.WriteSeriesKeyValue("STS_INDICATOR", "PROD");
                dataWriterEngine.WriteGroupKeyValue("STS_ACTIVITY", "NS0030");
                dataWriterEngine.WriteGroupKeyValue("STS_INSTITUTION", "1");
                dataWriterEngine.WriteGroupKeyValue("ADJUSTMENT", "N");
                dataWriterEngine.WriteAttributeValue("TIME_FORMAT", "P1Y");

                // write 2 observations for the abose series
                dataWriterEngine.WriteObservation("2001", "1.23");
                dataWriterEngine.WriteAttributeValue("OBS_STATUS", "A");
                dataWriterEngine.WriteAttributeValue("OBS_CONF", "F");

                dataWriterEngine.WriteObservation("2002", "4.56");
                dataWriterEngine.WriteAttributeValue("OBS_STATUS", "A");
                dataWriterEngine.WriteAttributeValue("OBS_CONF", "F");

                // write another series entry
                dataWriterEngine.StartSeries();
                dataWriterEngine.WriteSeriesKeyValue("FREQ", "A");
                dataWriterEngine.WriteSeriesKeyValue("REF_AREA", "EL");
                dataWriterEngine.WriteSeriesKeyValue("STS_INDICATOR", "IND");
                dataWriterEngine.WriteGroupKeyValue("STS_ACTIVITY", "NS0030");
                dataWriterEngine.WriteGroupKeyValue("STS_INSTITUTION", "1");
                dataWriterEngine.WriteGroupKeyValue("ADJUSTMENT", "N");
                dataWriterEngine.WriteAttributeValue("TIME_FORMAT", "P1Y");

                // write 1 observation for the abose series
                dataWriterEngine.WriteObservation("2001", "7.89");
                dataWriterEngine.WriteAttributeValue("OBS_STATUS", "A");
                dataWriterEngine.WriteAttributeValue("OBS_CONF", "F");

                // close compact Writer
                dataWriterEngine.Close();
            }
        }

        #endregion
    }
}