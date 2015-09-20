// -----------------------------------------------------------------------
// <copyright file="TestDataReaderEngine.cs" company="Eurostat">
//   Date Created : 2014-05-15
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace SdmxEdiDataWriterTests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Org.Sdmxsource.Sdmx.DataParser.Factory;
    using Org.Sdmxsource.Sdmx.DataParser.Manager;
    using Org.Sdmxsource.Sdmx.EdiParser.Constants;
    using Org.Sdmxsource.Sdmx.EdiParser.Manager;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util.Extensions;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    /// Test unit for <see cref="IDataReaderEngine"/>
    /// </summary>
    [TestFixture]
    public class TestEdiDataReaderEngine
    {
        /// <summary>
        /// The _factory
        /// </summary>
        private readonly IReadableDataLocationFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestEdiDataReaderEngine"/> class.
        /// </summary>
        public TestEdiDataReaderEngine()
        {
            this._factory = new ReadableDataLocationFactory();
        }

        /// <summary>
        /// Test unit for <see cref="IDataReaderEngine"/> 
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase(@"tests\Data\gesmes-ts-data-writer-tr-False.ges")]
        [TestCase(@"tests\Data\gesmes-ts-data-writer-tr-True.ges")]
        public void Test(string file)
        {
            IDataStructureObject dsd = BuildDsd();
            IReportedDateEngine reportedDateEngine = new ReportedDateEngine();
            var sdmxDataReaderFactory = new SdmxDataReaderFactory(new DataInformationManager(new FixedConceptEngine(), reportedDateEngine), new EdiParseManager());
            using (var sourceData = this._factory.GetReadableDataLocation(new FileInfo(file)))
            using (var dataReaderEngine = sdmxDataReaderFactory.GetDataReaderEngine(sourceData, dsd, null))
            {
                Assert.NotNull(dataReaderEngine);
                IHeader header = dataReaderEngine.Header;
                Assert.NotNull(header);
                Assert.IsTrue(dataReaderEngine.MoveNextDataset());
                Assert.IsTrue(dataReaderEngine.MoveNextKeyable());
                Assert.AreEqual("Q", dataReaderEngine.CurrentKey.GetKeyValue("FREQ"));
                Assert.AreEqual("N", dataReaderEngine.CurrentKey.GetKeyValue("ADJUSTMENT"));
                Assert.AreEqual("A", dataReaderEngine.CurrentKey.GetKeyValue("STS_ACTIVITY"));
                Assert.IsTrue(dataReaderEngine.MoveNextObservation());
                Assert.AreEqual("2005-Q1", dataReaderEngine.CurrentObservation.ObsTime);
            }
        }

        [TestCase(@"tests\Data\gesmes-ts-data-writer-tr-False.ges")]
        [TestCase(@"tests\Data\gesmes-ts-data-writer-tr-True.ges")]
        public void TestLikeNSIWC(string file)
        {
            IDataReaderManager manager = new DataReaderManager();
            IDataStructureObject dsd = BuildDsd();
            IList<IDictionary<string, string>> dataSetStoreList = new List<IDictionary<string, string>>();
            using (var sourceData = this._factory.GetReadableDataLocation(new FileInfo(file)))
            using (var compact = manager.GetDataReaderEngine(sourceData, dsd, null))
            {
                while (compact.MoveNextKeyable())
                {
                    if (compact.CurrentKey.Series)
                    {
                        IList<IKeyValue> keyValues = compact.CurrentKey.Key;

                        int index = 0;
                        while (compact.MoveNextObservation())
                        {
                            var dataSetStore = new Dictionary<string, string>(StringComparer.Ordinal);
                            foreach (var key in keyValues)
                            {
                                dataSetStore.Add(key.Concept, key.Code);
                            }

                            IObservation currentObservation = compact.CurrentObservation;
                            Assert.IsNotNullOrEmpty(currentObservation.ObservationValue);
                            Assert.IsNotNullOrEmpty(currentObservation.ObsTime);
                            Assert.IsFalse(currentObservation.CrossSection);
                            dataSetStore.Add(DimensionObject.TimeDimensionFixedId, currentObservation.ObsTime);
                            ISdmxDate sdmxDate = new SdmxDateCore(currentObservation.ObsTime);
                            Assert.AreEqual(sdmxDate.TimeFormatOfDate, currentObservation.ObsTimeFormat);
                            dataSetStore.Add(PrimaryMeasure.FixedId, currentObservation.ObservationValue);
                            int i = int.Parse(currentObservation.ObservationValue, NumberStyles.Any, CultureInfo.InvariantCulture);
                            Assert.AreEqual(index, i);

                            index++;
                            dataSetStoreList.Add(dataSetStore);
                        }
                    }
                }
            }
        }

        [TestCase(@"tests\Data\BOP_Q_Q_TEST.ges", "ECB", "EUROSTAT_BOP_01", 9)]
        [TestCase(@"tests\Data\BOP_Q_TEST1.edi", "ECB", "EUROSTAT_BOP_01", 9)]
        [TestCase(@"tests\Data\BOP_FATS_A_TEST1.ges", "EUROSTAT", "EUROSTAT_FATS_01", 9)]
        [TestCase(@"tests\Data\BOP_FATS_ATEST1.ges", "EUROSTAT", "EUROSTAT_FATS_01", 9)]
        [TestCase(@"tests\Data\TEST2.edi", "ECB", "EUROSTAT_BOP_01", 9)]
        [TestCase(@"tests\Data\TESTT1.edi", "ECB", "EUROSTAT_BOP_01", 9)]
        [TestCase(@"tests\Data\ESAP2DBT_2800_Q_TEST.ges", "EUROSTAT", "ESTAT_ESAIEA", 12)]
        [TestCase(@"tests\Data\ESAP2SEC_0801_Q_TEST.ges", "EUROSTAT", "ESTAT_ESAIEA", 12)]
        [TestCase(@"tests\Data\STSCONS_HOUR_Q_TEST.ges", "EUROSTAT", "EUROSTAT_STS", 6)]
        [TestCase(@"tests\Data\STSRTD_EMPL_M_TEST.GES", "EUROSTAT", "EUROSTAT_STS", 6)]
        public void TestLikeNSIWCVariousData(string file, string agency, string dsdId, int dimensionCount)
        {
            IDataReaderManager manager = new DataReaderManager();
            IDataStructureObject dsd = BuildDsd(new MaintainableRefObjectImpl(agency, dsdId, "1.0"), dimensionCount);
            IList<IDictionary<string, string>> dataSetStoreList = new List<IDictionary<string, string>>();
            using (var sourceData = this._factory.GetReadableDataLocation(new FileInfo(file)))
            using (var compact = manager.GetDataReaderEngine(sourceData, dsd, null))
            {
                while (compact.MoveNextKeyable())
                {
                    if (compact.CurrentKey.Series)
                    {
                        IList<IKeyValue> keyValues = compact.CurrentKey.Key;

                        while (compact.MoveNextObservation())
                        {
                            var dataSetStore = new Dictionary<string, string>(StringComparer.Ordinal);
                            foreach (var key in keyValues)
                            {
                                dataSetStore.Add(key.Concept, key.Code);
                            }

                            IObservation currentObservation = compact.CurrentObservation;
                            Assert.IsNotNullOrEmpty(currentObservation.ObservationValue);
                            Assert.IsNotNullOrEmpty(currentObservation.ObsTime);
                            Assert.IsFalse(currentObservation.CrossSection);
                            dataSetStore.Add(DimensionObject.TimeDimensionFixedId, currentObservation.ObsTime);
                            ISdmxDate sdmxDate = new SdmxDateCore(currentObservation.ObsTime);
                            Assert.AreEqual(sdmxDate.TimeFormatOfDate, currentObservation.ObsTimeFormat);
                            dataSetStore.Add(PrimaryMeasure.FixedId, currentObservation.ObservationValue);
                            if (!currentObservation.ObservationValue.Equals(EdiConstants.MissingVal))
                            {
                                double i;
                                Assert.IsTrue(double.TryParse(currentObservation.ObservationValue, NumberStyles.Any, CultureInfo.InvariantCulture, out i), "Cannot convert to int {0}", currentObservation.ObservationValue);
                            }

                            dataSetStoreList.Add(dataSetStore);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Test unit for <see cref="IDataReaderEngine" />
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="expectedValue">if set to <c>true</c> then it is expected to be false.</param>
        [TestCase(@"tests\Data\gesmes-ts-data-writer-tr-False.ges", true)]
        [TestCase(@"tests\Data\gesmes-ts-data-writer-tr-False-no-test.ges", false)]
        public void TestHeaderIsTest(string file, bool expectedValue)
        {
            IDataStructureObject dsd = BuildDsd();
            IReportedDateEngine reportedDateEngine = new ReportedDateEngine();
            var sdmxDataReaderFactory = new SdmxDataReaderFactory(new DataInformationManager(new FixedConceptEngine(), reportedDateEngine), new EdiParseManager());
            using (var sourceData = this._factory.GetReadableDataLocation(new FileInfo(file)))
            using (var dataReaderEngine = sdmxDataReaderFactory.GetDataReaderEngine(sourceData, dsd, null))
            {
                Assert.NotNull(dataReaderEngine);
                IHeader header = dataReaderEngine.Header;
                Assert.NotNull(header);
                Assert.AreEqual(expectedValue, header.Test);
            }
        }

        /// <summary>
        /// Test unit for <see cref="IDataReaderEngine" />
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="expectedDataflow">The expected dataflow.</param>
        [TestCase(@"tests\Data\gesmes-ts-data-writer-tr-False.ges", "TEST_DATAFLOW")]
        public void TestHeaderDataflow(string file, string expectedDataflow)
        {
            IDataStructureObject dsd = BuildDsd();
            var manager = new DataReaderManager();
            using (var sourceData = this._factory.GetReadableDataLocation(new FileInfo(file)))
            using (var dataReaderEngine = manager.GetDataReaderEngine(sourceData, dsd, null))
            {
                Assert.NotNull(dataReaderEngine);
                Assert.IsTrue(dataReaderEngine.MoveNextDataset());
                var header = dataReaderEngine.CurrentDatasetHeader;
                Assert.NotNull(header);

                Assert.AreEqual(expectedDataflow, header.DatasetId);
            }
        }

        /// <summary>
        /// Test unit for <see cref="IDataReaderEngine" />
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="expectedDsd">The expected DSD.</param>
        [TestCase(@"tests\Data\gesmes-ts-data-writer-tr-False.ges", "TEST_DSD")]
        public void TestHeaderDsd(string file, string expectedDsd)
        {
            IDataStructureObject dsd = BuildDsd();
            var manager = new DataReaderManager();
            using (var sourceData = this._factory.GetReadableDataLocation(new FileInfo(file)))
            using (var dataReaderEngine = manager.GetDataReaderEngine(sourceData, dsd, null))
            {
                Assert.NotNull(dataReaderEngine);
                Assert.IsTrue(dataReaderEngine.MoveNextDataset());
                var header = dataReaderEngine.CurrentDatasetHeader;
                Assert.NotNull(header);

                Assert.AreEqual(expectedDsd, header.DataStructureReference.StructureReference.MaintainableId);
            }
        }

        /// <summary>
        /// Test unit for <see cref="IDataReaderEngine" />
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="expectedSender">The expected DSD.</param>
        [TestCase(@"tests\Data\gesmes-ts-data-writer-tr-False.ges", "ZZ1")]
        public void TestHeaderSender(string file, string expectedSender)
        {
            IDataStructureObject dsd = BuildDsd();
            var manager = new DataReaderManager();
            using (var sourceData = this._factory.GetReadableDataLocation(new FileInfo(file)))
            using (var dataReaderEngine = manager.GetDataReaderEngine(sourceData, dsd, null))
            {
                Assert.NotNull(dataReaderEngine);
                Assert.IsTrue(dataReaderEngine.MoveNextDataset());
                var header = dataReaderEngine.Header;
                Assert.NotNull(header);

                Assert.AreEqual(expectedSender, header.Sender.Id);
            }
        }

        /// <summary>
        /// Test unit for <see cref="IDataReaderEngine" />
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="expectedReceiver">The expected DSD.</param>
        [TestCase(@"tests\Data\gesmes-ts-data-writer-tr-False.ges", "ZZ9")]
        public void TestHeaderReceiver(string file, string expectedReceiver)
        {
            IDataStructureObject dsd = BuildDsd();
            var manager = new DataReaderManager();
            using (var sourceData = this._factory.GetReadableDataLocation(new FileInfo(file)))
            using (var dataReaderEngine = manager.GetDataReaderEngine(sourceData, dsd, null))
            {
                Assert.NotNull(dataReaderEngine);
                Assert.IsTrue(dataReaderEngine.MoveNextDataset());
                var header = dataReaderEngine.Header;
                Assert.NotNull(header);
                Assert.IsNotEmpty(header.Receiver);
                Assert.AreEqual(expectedReceiver, header.Receiver.First().Id);
            }
        }

        /// <summary>
        /// Builds the DSD.
        /// </summary>
        /// <returns>The DSD.</returns>
        private static IDataStructureObject BuildDsd(IMaintainableRefObject dsdRef, int dimensions)
        {
            IDataStructureMutableObject dsdMutableObject = new DataStructureMutableCore
                                                               {
                                                                   AgencyId = dsdRef.AgencyId,
                                                                   Id = dsdRef.MaintainableId,
                                                                   Version = dsdRef.HasVersion() ? dsdRef.Version : "1.0"
                                                               };
            dsdMutableObject.AddName("en", "Test data");

            // FREQ="Q" ADJUSTMENT="N" STS_ACTIVITY="A" 
            dsdMutableObject.AddDimension(
                new StructureReferenceImpl(agencyId: dsdRef.AgencyId, maintainableId: "TEST_CS", version: "1.0", targetStructureEnum: SdmxStructureEnumType.Concept, identfiableIds: "FREQ"),
                new StructureReferenceImpl("SDMX", "CL_FREQ", "1.0", SdmxStructureEnumType.CodeList));

            var groupMutableCore = new GroupMutableCore() { Id = "Sibling" };
            dsdMutableObject.AddGroup(groupMutableCore);
            for (int i = 0; i < dimensions; i++)
            {
                string id = "DIM" + i.ToString(CultureInfo.InvariantCulture);
                var dim = dsdMutableObject.AddDimension(
                    new StructureReferenceImpl(
                        agencyId: dsdRef.AgencyId,
                        maintainableId: "TEST_CS",
                        version: "1.0",
                        targetStructureEnum: SdmxStructureEnumType.Concept,
                        identfiableIds: id), codelistRef: new StructureReferenceImpl(dsdRef.AgencyId, "CL_" + id, "1.0", SdmxStructureEnumType.CodeList));
                groupMutableCore.DimensionRef.Add(id);
            }

            dsdMutableObject.AddDimension(
                new DimensionMutableCore
                    {
                        ConceptRef =
                            new StructureReferenceImpl(
                            agencyId: dsdRef.AgencyId,
                            maintainableId: "TEST_CS",
                            version: "1.0",
                            targetStructureEnum: SdmxStructureEnumType.Concept,
                            identfiableIds: "TIME_PERIOD"),
                        TimeDimension = true
                    });

            dsdMutableObject.AddPrimaryMeasure(
                new StructureReferenceImpl(agencyId: dsdRef.AgencyId, maintainableId: "TEST_CS", version: "1.0", targetStructureEnum: SdmxStructureEnumType.Concept, identfiableIds: "OBS_VALUE"));
            
            var obsStatus =
                dsdMutableObject.AddAttribute(
                    new StructureReferenceImpl(agencyId: dsdRef.AgencyId, maintainableId: "TEST_CS", version: "1.0", targetStructureEnum: SdmxStructureEnumType.Concept, identfiableIds: "OBS_STATUS"),
                    new StructureReferenceImpl(dsdRef.AgencyId, "CL_OBS_STATUS", "1.0", SdmxStructureEnumType.CodeList));
            obsStatus.AttachmentLevel = AttributeAttachmentLevel.Observation;
            obsStatus.AssignmentStatus = "Mandatory";

            var obsConf =
                dsdMutableObject.AddAttribute(
                    new StructureReferenceImpl(agencyId: dsdRef.AgencyId, maintainableId: "TEST_CS", version: "1.0", targetStructureEnum: SdmxStructureEnumType.Concept, identfiableIds: "OBS_CONF"),
                    new StructureReferenceImpl(dsdRef.AgencyId, "CL_OBS_CONF", "1.0", SdmxStructureEnumType.CodeList));
            obsConf.AttachmentLevel = AttributeAttachmentLevel.Observation;
            obsConf.AssignmentStatus = "Conditional";

            AddCodedAttribute(dsdMutableObject, "UNIT_MULT");
            AddCodedAttribute(dsdMutableObject, "UNIT");
            AddUnCodedAttribute(dsdMutableObject, "TITLE_COMPL");

            return dsdMutableObject.ImmutableInstance;
        }

        private static void AddCodedAttribute(IDataStructureMutableObject dsdMutableObject, string conceptId)
        {
            var attributeMutableObject =
                dsdMutableObject.AddAttribute(
                    new StructureReferenceImpl(agencyId: dsdMutableObject.AgencyId, maintainableId: "TEST_CS", version: "1.0", targetStructureEnum: SdmxStructureEnumType.Concept, identfiableIds: conceptId),
                    new StructureReferenceImpl(dsdMutableObject.AgencyId, "CL_" + conceptId, "1.0", SdmxStructureEnumType.CodeList));
            attributeMutableObject.AttachmentLevel = AttributeAttachmentLevel.Group;
            attributeMutableObject.AttachmentGroup = "Sibling";
            attributeMutableObject.AssignmentStatus = "Conditional";
        }

        private static void AddUnCodedAttribute(IDataStructureMutableObject dsdMutableObject, string conceptId)
        {
            var attributeMutableObject =
                dsdMutableObject.AddAttribute(new StructureReferenceImpl(agencyId: dsdMutableObject.AgencyId, maintainableId: "TEST_CS", version: "1.0", targetStructureEnum: SdmxStructureEnumType.Concept, identfiableIds: conceptId), null);
            attributeMutableObject.AttachmentLevel = AttributeAttachmentLevel.Group;
            attributeMutableObject.AttachmentGroup = "Sibling";
            attributeMutableObject.AssignmentStatus = "Conditional";
        }

        /// <summary>
        /// Builds the DSD.
        /// </summary>
        /// <returns>The DSD.</returns>
        private static IDataStructureObject BuildDsd()
        {
            IDataStructureMutableObject dsdMutableObject = new DataStructureMutableCore
                                                               {
                                                                   AgencyId = "TEST",
                                                                   Id = "TEST_DSD",
                                                                   Version = "1.0"
                                                               };
            dsdMutableObject.AddName("en", "Test data");

            // FREQ="Q" ADJUSTMENT="N" STS_ACTIVITY="A" 
            dsdMutableObject.AddDimension(new StructureReferenceImpl(agencyId: "TEST", maintainableId: "TEST_CS", version: "1.0", targetStructureEnum: SdmxStructureEnumType.Concept, identfiableIds: "FREQ"), new StructureReferenceImpl("SDMX", "CL_FREQ", "1.0", SdmxStructureEnumType.CodeList));
            dsdMutableObject.AddDimension(new StructureReferenceImpl(agencyId: "TEST", maintainableId: "TEST_CS", version: "1.0", targetStructureEnum: SdmxStructureEnumType.Concept, identfiableIds: "ADJUSTMENT"), new StructureReferenceImpl("SDMX", "CL_ADJUSTMENT", "1.0", SdmxStructureEnumType.CodeList));
            dsdMutableObject.AddDimension(new StructureReferenceImpl(agencyId: "TEST", maintainableId: "TEST_CS", version: "1.0", targetStructureEnum: SdmxStructureEnumType.Concept, identfiableIds: "STS_ACTIVITY"), new StructureReferenceImpl("STS", "CL_STS_ACTIVITY", "1.0", SdmxStructureEnumType.CodeList));
            dsdMutableObject.AddDimension(
                new DimensionMutableCore
                    {
                        ConceptRef =
                            new StructureReferenceImpl(
                            agencyId: "TEST",
                            maintainableId: "TEST_CS",
                            version: "1.0",
                            targetStructureEnum: SdmxStructureEnumType.Concept,
                            identfiableIds: "TIME_PERIOD"),
                        TimeDimension = true
                    });

            dsdMutableObject.AddPrimaryMeasure(
                new StructureReferenceImpl(agencyId: "TEST", maintainableId: "TEST_CS", version: "1.0", targetStructureEnum: SdmxStructureEnumType.Concept, identfiableIds: "OBS_VALUE"));

            var attributeMutableObject = dsdMutableObject.AddAttribute(new StructureReferenceImpl(agencyId: "TEST", maintainableId: "TEST_CS", version: "1.0", targetStructureEnum: SdmxStructureEnumType.Concept, identfiableIds: "DECIMALS"), new StructureReferenceImpl("STS", "CL_DECIMALS", "1.0", SdmxStructureEnumType.CodeList));
            var obsStatus = dsdMutableObject.AddAttribute(new StructureReferenceImpl(agencyId: "TEST", maintainableId: "TEST_CS", version: "1.0", targetStructureEnum: SdmxStructureEnumType.Concept, identfiableIds: "OBS_STATUS"), new StructureReferenceImpl("STS", "CL_OBS_STATUS", "1.0", SdmxStructureEnumType.CodeList));
            attributeMutableObject.AttachmentLevel = AttributeAttachmentLevel.DimensionGroup;
            attributeMutableObject.DimensionReferences.AddAll(new[] { "FREQ", "ADJUSTMENT", "STS_ACTIVITY" });
            attributeMutableObject.AssignmentStatus = "Mandatory";
            obsStatus.AttachmentLevel = AttributeAttachmentLevel.Observation;
            obsStatus.AssignmentStatus = "Mandatory";
            return dsdMutableObject.ImmutableInstance;
        }
    }
}