// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReUsingCrossWriter.cs" company="EUROSTAT">
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
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    ///     The re using compact writer.
    /// </summary>
    public class ReUsingCrossWriter
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
            using (IReadableDataLocation readable = new FileReadableDataLocation("ESTAT+DEMOGRAPHY+2.1.xml"))
            {
                IStructureWorkspace structureWorkspace = _parsingManager.ParseStructures(readable);

                ISdmxObjects structureBeans = structureWorkspace.GetStructureObjects(false);
                dataStructure = structureBeans.DataStructures.FirstOrDefault();
            }

            if (dataStructure == null)
            {
                throw new InvalidOperationException("Could not build dataStructure object");
            }

            using (XmlWriter writer = XmlWriter.Create("re-using-cross-writer.xml", new XmlWriterSettings { Indent = true }))
            {
                // initialize the data writing engine. It can be for SDMX versions 2.0 only.
                var dataWriterEngine = new CrossSectionalWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo));

                // write header
                dataWriterEngine.WriteHeader(new HeaderImpl("ZZ9", "ZZ9"));

                // start dataset
                dataWriterEngine.StartDataset(null, dataStructure, null);

                // write dataset attributes
                dataWriterEngine.WriteAttributeValue("REV_NUM", "1");
                dataWriterEngine.WriteAttributeValue("TAB_NUM", "RQEL11V1");

                // write 1 group entry
                dataWriterEngine.StartXSGroup();
                dataWriterEngine.WriteXSGroupKeyValue("FREQ", "A");
                dataWriterEngine.WriteXSGroupKeyValue("COUNTRY", "LU");
                dataWriterEngine.WriteXSGroupKeyValue("TIME", "1920");
                dataWriterEngine.WriteAttributeValue("TIME_FORMAT", "P1Y");

                // write a series entry
                dataWriterEngine.StartSection();
                dataWriterEngine.WriteAttributeValue("UNIT_MULT", "0");
                dataWriterEngine.WriteAttributeValue("DECI", "1");
                dataWriterEngine.WriteAttributeValue("UNIT", "UNITS");

                // write observations for the abose section
                dataWriterEngine.StartXSObservation("PJAN1T", "2030.1");
                dataWriterEngine.WriteXSObservationKeyValue("SEX", "F");
                dataWriterEngine.WriteAttributeValue("OBS_STATUS", "P");

                dataWriterEngine.StartXSObservation("LBIRTHST", "2030.1");
                dataWriterEngine.WriteXSObservationKeyValue("SEX", "F");
                dataWriterEngine.WriteAttributeValue("OBS_STATUS", "P");

                dataWriterEngine.StartXSObservation("DEATHST", "2030.1");
                dataWriterEngine.WriteXSObservationKeyValue("SEX", "F");
                dataWriterEngine.WriteAttributeValue("OBS_STATUS", "P");

                dataWriterEngine.StartXSObservation("ADJT", "2030.1");
                dataWriterEngine.WriteXSObservationKeyValue("SEX", "F");
                dataWriterEngine.WriteAttributeValue("OBS_STATUS", "P");

                dataWriterEngine.StartXSObservation("LBIRTHOUT", "2030.1");
                dataWriterEngine.WriteXSObservationKeyValue("SEX", "F");
                dataWriterEngine.WriteAttributeValue("OBS_STATUS", "P");

                dataWriterEngine.StartXSObservation("DEATHUN1", "2030.1");
                dataWriterEngine.WriteXSObservationKeyValue("SEX", "F");
                dataWriterEngine.WriteAttributeValue("OBS_STATUS", "P");

                dataWriterEngine.StartXSObservation("MAR", "2030.1");
                dataWriterEngine.WriteXSObservationKeyValue("SEX", "F");
                dataWriterEngine.WriteAttributeValue("OBS_STATUS", "P");

                dataWriterEngine.StartXSObservation("DIV", "2030.1");
                dataWriterEngine.WriteXSObservationKeyValue("SEX", "F");
                dataWriterEngine.WriteAttributeValue("OBS_STATUS", "P");

                dataWriterEngine.StartXSObservation("IMMIT", "2030.1");
                dataWriterEngine.WriteXSObservationKeyValue("SEX", "F");
                dataWriterEngine.WriteAttributeValue("OBS_STATUS", "P");

                dataWriterEngine.StartXSObservation("EMIGT", "2030.1");
                dataWriterEngine.WriteXSObservationKeyValue("SEX", "F");
                dataWriterEngine.WriteAttributeValue("OBS_STATUS", "P");

                dataWriterEngine.StartXSObservation("NETMT", "2030.1");
                dataWriterEngine.WriteXSObservationKeyValue("SEX", "F");
                dataWriterEngine.WriteAttributeValue("OBS_STATUS", "P");

                dataWriterEngine.StartXSObservation("TFRNSI", "2030.1");
                dataWriterEngine.WriteXSObservationKeyValue("SEX", "F");
                dataWriterEngine.WriteAttributeValue("OBS_STATUS", "P");

                dataWriterEngine.StartXSObservation("LEXPNSIT", "2030.1");
                dataWriterEngine.WriteXSObservationKeyValue("SEX", "F");
                dataWriterEngine.WriteAttributeValue("OBS_STATUS", "P");

                // close cross Writer
                dataWriterEngine.Close();
            }
        }

        #endregion
    }
}