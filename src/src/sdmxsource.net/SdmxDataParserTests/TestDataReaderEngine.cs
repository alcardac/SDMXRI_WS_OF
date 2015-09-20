// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDataReaderEngine.cs" company="EUROSTAT">
//   TODO
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SdmxDataParserTests
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
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Org.Sdmxsource.Sdmx.DataParser.Engine.Reader;
    using Org.Sdmxsource.Sdmx.DataParser.Factory;
    using Org.Sdmxsource.Sdmx.DataParser.Manager;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Sdmx.StructureRetrieval.Manager;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util.Extensions;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    /// Test unit for <see cref="IDataReaderEngine"/>
    /// </summary>
    [TestFixture]
    public class TestDataReaderEngine
    {
        /// <summary>
        /// The _factory
        /// </summary>
        private readonly IReadableDataLocationFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestDataReaderEngine"/> class.
        /// </summary>
        public TestDataReaderEngine()
        {
            this._factory = new ReadableDataLocationFactory();
        }

        /// <summary>
        /// Tests the compact data reader.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase(@"tests\Data\Compact-VersionTwo.xml")]
        [TestCase(@"tests\Data\Compact-VersionTwoPointOne.xml")]
        public void TestCompactDataReader(string file)
        {
            IDataStructureObject dsd = BuildDsd();
            using (var sourceData = this._factory.GetReadableDataLocation(new FileInfo(file)))
            {
                var dataReaderEngine = new CompactDataReaderEngine(sourceData, null, dsd);
                Assert.NotNull(dataReaderEngine.Header);
                Assert.IsTrue(dataReaderEngine.MoveNextDataset());
                Assert.IsTrue(dataReaderEngine.MoveNextKeyable());
                Assert.AreEqual("Q", dataReaderEngine.CurrentKey.GetKeyValue("FREQ"));
                Assert.AreEqual("N", dataReaderEngine.CurrentKey.GetKeyValue("ADJUSTMENT"));
                Assert.AreEqual("A", dataReaderEngine.CurrentKey.GetKeyValue("STS_ACTIVITY"));
                Assert.IsTrue(dataReaderEngine.MoveNextObservation());
                Assert.AreEqual("2005-Q1", dataReaderEngine.CurrentObservation.ObsTime);
            }
        }

        /// <summary>
        /// Tests the generic data reader.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase(@"tests\Data\Generic-VersionTwo.xml")]
        [TestCase(@"tests\Data\Generic-VersionTwoPointOne.xml")]
        public void TestGenericDataReader(string file)
        {
            IDataStructureObject dsd = BuildDsd();
            using (var sourceData = this._factory.GetReadableDataLocation(new FileInfo(file)))
            {
                var dataReaderEngine = new GenericDataReaderEngine(sourceData, null, dsd);
                Assert.NotNull(dataReaderEngine.Header);
                Assert.IsTrue(dataReaderEngine.MoveNextDataset());
                Assert.IsTrue(dataReaderEngine.MoveNextKeyable());
                Assert.AreEqual("Q", dataReaderEngine.CurrentKey.GetKeyValue("FREQ"));
                Assert.AreEqual("N", dataReaderEngine.CurrentKey.GetKeyValue("ADJUSTMENT"));
                Assert.AreEqual("A", dataReaderEngine.CurrentKey.GetKeyValue("STS_ACTIVITY"));
                Assert.IsTrue(dataReaderEngine.MoveNextObservation());
                Assert.AreEqual("2005-Q1", dataReaderEngine.CurrentObservation.ObsTime);
            }
        }

        /// <summary>
        /// Test unit for <see cref="IDataReaderEngine"/> 
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase(@"tests\Data\Compact-VersionTwo.xml")]
        [TestCase(@"tests\Data\Compact21-alldim.xml")]
        [TestCase(@"tests\Data\Compact-VersionTwoPointOne.xml")]
        [TestCase(@"tests\Data\Generic-VersionTwo.xml")]
        [TestCase(@"tests\Data\Generic-VersionTwoPointOne.xml")]
        public void Test(string file)
        {
            IDataStructureObject dsd = BuildDsd();
            IReportedDateEngine reportedDateEngine = new ReportedDateEngine();
            var sdmxDataReaderFactory = new SdmxDataReaderFactory(new DataInformationManager(new FixedConceptEngine(), reportedDateEngine), null);
            using (var sourceData = this._factory.GetReadableDataLocation(new FileInfo(file)))
            using (var dataReaderEngine = sdmxDataReaderFactory.GetDataReaderEngine(sourceData, dsd, null))
            {
                Assert.NotNull(dataReaderEngine);
                Assert.NotNull(dataReaderEngine.Header);
                Assert.IsTrue(dataReaderEngine.MoveNextDataset());
                Assert.IsTrue(dataReaderEngine.MoveNextKeyable());
                Assert.AreEqual("Q", dataReaderEngine.CurrentKey.GetKeyValue("FREQ"));
                Assert.AreEqual("N", dataReaderEngine.CurrentKey.GetKeyValue("ADJUSTMENT"));
                Assert.AreEqual("A", dataReaderEngine.CurrentKey.GetKeyValue("STS_ACTIVITY"));
                Assert.IsTrue(dataReaderEngine.MoveNextObservation());
                Assert.AreEqual("2005-Q1", dataReaderEngine.CurrentObservation.ObsTime);
            }
        }

        /// <summary>
        /// Tests the bop.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="dsd">The data structure definition.</param>
        [TestCase(@"tests\Data\SDMX-BOP-BdE-sample-corrected.xml", @"tests\v21\Structure\DataStructure-IMF.BOP(1.0).xml")]
        [TestCase(@"tests\Data\SDMX-BOP-BdE-sample.xml", @"tests\v21\Structure\DataStructure-IMF.BOP(1.0).xml")]
        [TestCase(@"tests\Data\compact_demo.xml", @"tests\v20\demo_xs_dsd.xml")]
        [TestCase(@"tests\Data\ESTAT_NA_MAIN_1_0.xml", @"tests\v21\Structure\ESTAT+NA_MAIN+1.0.xml")]
        public void TestBop(string file, string dsd)
        {
            var retrievalManager = new InMemoryRetrievalManager();
            IDataStructureObject dsdObject;
            using (IReadableDataLocation dataLocation = _factory.GetReadableDataLocation(new FileInfo(dsd)))
            {
               IStructureParsingManager manager = new StructureParsingManager();
                var structureWorkspace = manager.ParseStructures(dataLocation);
                retrievalManager.SaveStructures(structureWorkspace.GetStructureObjects(false));

            }

              IReportedDateEngine reportedDateEngine = new ReportedDateEngine();
            var sdmxDataReaderFactory = new SdmxDataReaderFactory(new DataInformationManager(new FixedConceptEngine(), reportedDateEngine), null);
            using (var sourceData = this._factory.GetReadableDataLocation(new FileInfo(file)))
            using (var dataReaderEngine = sdmxDataReaderFactory.GetDataReaderEngine(sourceData, retrievalManager))
            {
                Assert.NotNull(dataReaderEngine);
                Assert.NotNull(dataReaderEngine.Header);
                Assert.IsTrue(dataReaderEngine.MoveNextDataset());
                Assert.IsTrue(dataReaderEngine.MoveNextKeyable());
                IKeyable currentKey = dataReaderEngine.CurrentKey;

                while (!currentKey.Series && dataReaderEngine.MoveNextKeyable())
                {
                    currentKey = dataReaderEngine.CurrentKey;
                }
            }
        }

        /// <summary>
        /// Test unit for <see cref="IDataReaderEngine" />
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="dimAtObs">The dim at OBS.</param>
        [TestCase(@"tests\Data\Compact21-ADJUSTMENT.xml", "ADJUSTMENT")]
        [TestCase(@"tests\Data\Compact21-FREQ.xml", "FREQ")]
        [TestCase(@"tests\Data\Compact21-STS_ACTIVITY.xml", "STS_ACTIVITY")]
        [TestCase(@"tests\Data\Generic21-ADJUSTMENT.xml", "ADJUSTMENT")]
        [TestCase(@"tests\Data\Generic21-FREQ.xml", "FREQ")]
        [TestCase(@"tests\Data\Generic21-STS_ACTIVITY.xml", "STS_ACTIVITY")]
        public void TestDimAtObs(string file, string dimAtObs)
        {
            IDataStructureObject dsd = BuildDsd();
            IReportedDateEngine reportedDateEngine = new ReportedDateEngine();
            var sdmxDataReaderFactory = new SdmxDataReaderFactory(new DataInformationManager(new FixedConceptEngine(), reportedDateEngine), null);
            using (var sourceData = this._factory.GetReadableDataLocation(new FileInfo(file)))
            using (var dataReaderEngine = sdmxDataReaderFactory.GetDataReaderEngine(sourceData, dsd, null))
            {
                Assert.NotNull(dataReaderEngine);
                Assert.NotNull(dataReaderEngine.Header);
                Assert.IsTrue(dataReaderEngine.MoveNextDataset());
                Assert.IsTrue(dataReaderEngine.MoveNextKeyable());
                IKeyable currentKey = dataReaderEngine.CurrentKey;

                Assert.IsTrue(dataReaderEngine.MoveNextObservation());
                var currentObs = dataReaderEngine.CurrentObservation;
                if (!dimAtObs.Equals("FREQ"))
                {
                    Assert.AreEqual("A", currentKey.GetKeyValue("FREQ"));
                }
                else
                {
                    Assert.AreEqual("A", currentObs.CrossSectionalValue.Code);
                }

                if (!dimAtObs.Equals("ADJUSTMENT"))
                {
                    Assert.AreEqual("N", currentKey.GetKeyValue("ADJUSTMENT"));
                }
                else
                {
                    Assert.AreEqual("N", currentObs.CrossSectionalValue.Code);
                }

                if (!dimAtObs.Equals("STS_ACTIVITY"))
                {
                    Assert.AreEqual("A", currentKey.GetKeyValue("STS_ACTIVITY"));
                }
                else
                {
                    Assert.AreEqual("A", currentObs.CrossSectionalValue.Code);
                }

                Assert.AreEqual("2005", dataReaderEngine.CurrentObservation.ObsTime);
            }
        }

        /// <summary>
        /// Tests the like NSIWC.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase(@"tests\Data\Compact-VersionTwo.xml")]
        [TestCase(@"tests\Data\Compact-VersionTwoPointOne.xml")]
        [TestCase(@"tests\Data\Generic-VersionTwo.xml")]
        [TestCase(@"tests\Data\Generic-VersionTwoPointOne.xml")]
        public void TestLikeNsiWc(string file)
        {
            IDataReaderManager manager = new DataReaderManager();
            IDataStructureObject dsd = BuildDsd();
            IList<IDictionary<string, string>> dataSetStoreList = new List<IDictionary<string, string>>();
            int obscount = 0;
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
                            if (currentObservation.CrossSection)
                            {
                                Assert.IsNotNull(currentObservation.CrossSectionalValue);
                                dataSetStore.Add(currentObservation.CrossSectionalValue.Concept, currentObservation.CrossSectionalValue.Code);
                            }

                            dataSetStore.Add(DimensionObject.TimeDimensionFixedId, currentObservation.ObsTime);
                            ISdmxDate sdmxDate = new SdmxDateCore(currentObservation.ObsTime);
                            Assert.AreEqual(sdmxDate.TimeFormatOfDate, currentObservation.ObsTimeFormat);
                            dataSetStore.Add(PrimaryMeasure.FixedId, currentObservation.ObservationValue);
                            int i = int.Parse(currentObservation.ObservationValue, NumberStyles.Any, CultureInfo.InvariantCulture);
                            Assert.AreEqual(index, i, "Expected {0}\nBut was {1} At OBS {2}", index, i, obscount);
                            
                            index++;
                            obscount++;
                            dataSetStoreList.Add(dataSetStore);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tests the like NSIWC.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="dimensionAtObservation">The dimension at observation.</param>
        [TestCase(@"tests\Data\Compact21-ADJUSTMENT.xml", "ADJUSTMENT")]
        [TestCase(@"tests\Data\Compact21-FREQ.xml", "FREQ")]
        [TestCase(@"tests\Data\Compact21-STS_ACTIVITY.xml", "STS_ACTIVITY")]
        [TestCase(@"tests\Data\Generic21-ADJUSTMENT.xml", "ADJUSTMENT")]
        [TestCase(@"tests\Data\Generic21-FREQ.xml", "FREQ")]
        [TestCase(@"tests\Data\Generic21-STS_ACTIVITY.xml", "STS_ACTIVITY")]
        public void TestLikeNsiWcDimAtObs(string file, string dimensionAtObservation)
        {
            IDataReaderManager manager = new DataReaderManager();
            IDataStructureObject dsd = BuildDsd();
            IList<IDictionary<string, string>> dataSetStoreList = new List<IDictionary<string, string>>();
            int obscount = 0;
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
                            if (currentObservation.CrossSection)
                            {
                                Assert.IsNotNull(currentObservation.CrossSectionalValue);
                                dataSetStore.Add(currentObservation.CrossSectionalValue.Concept, currentObservation.CrossSectionalValue.Code);
                                Assert.AreEqual(dimensionAtObservation, currentObservation.CrossSectionalValue.Concept);
                            }

                            dataSetStore.Add(DimensionObject.TimeDimensionFixedId, currentObservation.ObsTime);
                            ISdmxDate sdmxDate = new SdmxDateCore(currentObservation.ObsTime);
                            Assert.AreEqual(sdmxDate.TimeFormatOfDate, currentObservation.ObsTimeFormat);
                            dataSetStore.Add(PrimaryMeasure.FixedId, currentObservation.ObservationValue);
                            int i = int.Parse(currentObservation.ObservationValue, NumberStyles.Any, CultureInfo.InvariantCulture);
                            Assert.AreEqual(index, i, "Expected {0}\nBut was {1} At OBS {2}", index, i, obscount);

                            index++;
                            obscount++;
                            dataSetStoreList.Add(dataSetStore);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tests the like NSIWC.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase(@"tests\Data\Compact21-alldim.xml")]
        public void TestLikeNsiWcAll(string file)
        {
            IDataReaderManager manager = new DataReaderManager();
            IDataStructureObject dsd = BuildDsd();
            IList<IDictionary<string, string>> dataSetStoreList = new List<IDictionary<string, string>>();
            int obscount = 0;
            using (var sourceData = this._factory.GetReadableDataLocation(new FileInfo(file)))
            using (var compact = manager.GetDataReaderEngine(sourceData, dsd, null))
            {
                int index = 0;
                while (compact.MoveNextKeyable())
                {
                    if (compact.CurrentKey.Series)
                    {
                        IList<IKeyValue> keyValues = compact.CurrentKey.Key;

                        if (index >= 100)
                        {
                            index = 0;
                        }

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
                            if (currentObservation.CrossSection)
                            {
                                Assert.IsNotNull(currentObservation.CrossSectionalValue);
                                dataSetStore.Add(currentObservation.CrossSectionalValue.Concept, currentObservation.CrossSectionalValue.Code);
                            }

                            dataSetStore.Add(DimensionObject.TimeDimensionFixedId, currentObservation.ObsTime);
                            ISdmxDate sdmxDate = new SdmxDateCore(currentObservation.ObsTime);
                            Assert.AreEqual(sdmxDate.TimeFormatOfDate, currentObservation.ObsTimeFormat);
                            dataSetStore.Add(PrimaryMeasure.FixedId, currentObservation.ObservationValue);
                            int i = int.Parse(currentObservation.ObservationValue, NumberStyles.Any, CultureInfo.InvariantCulture);
                            Assert.AreEqual(index, i, "Expected {0}\nBut was {1} At OBS {2}", index, i, obscount);

                            index++;
                            obscount++;
                            dataSetStoreList.Add(dataSetStore);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Builds the DSD.
        /// </summary>
        /// <returns>
        /// The <see cref="IDataStructureObject" />.
        /// </returns>
        private static IDataStructureObject BuildDsd()
        {
            IDataStructureMutableObject dsdMutableObject = new DataStructureMutableCore { AgencyId = "TEST", Id = "TEST_DSD", Version = "1.0" };
            dsdMutableObject.AddName("en", "Test data");

            // FREQ="Q" ADJUSTMENT="N" STS_ACTIVITY="A" 
            dsdMutableObject.AddDimension(
                new StructureReferenceImpl("TEST", "TEST_CS", "1.0", SdmxStructureEnumType.Concept, "FREQ"),
                new StructureReferenceImpl("SDMX", "CL_FREQ", "1.0", SdmxStructureEnumType.CodeList));
            dsdMutableObject.AddDimension(
                new StructureReferenceImpl("TEST", "TEST_CS", "1.0", SdmxStructureEnumType.Concept, "ADJUSTMENT"),
                new StructureReferenceImpl("SDMX", "CL_ADJUSTMENT", "1.0", SdmxStructureEnumType.CodeList));
            dsdMutableObject.AddDimension(
                new StructureReferenceImpl("TEST", "TEST_CS", "1.0", SdmxStructureEnumType.Concept, "STS_ACTIVITY"),
                new StructureReferenceImpl("STS", "CL_STS_ACTIVITY", "1.0", SdmxStructureEnumType.CodeList));
            dsdMutableObject.AddDimension(
                new DimensionMutableCore { ConceptRef = new StructureReferenceImpl("TEST", "TEST_CS", "1.0", SdmxStructureEnumType.Concept, "TIME_PERIOD"), TimeDimension = true });

            dsdMutableObject.AddPrimaryMeasure(new StructureReferenceImpl("TEST", "TEST_CS", "1.0", SdmxStructureEnumType.Concept, "OBS_VALUE"));

            var attributeMutableObject = dsdMutableObject.AddAttribute(
                new StructureReferenceImpl("TEST", "TEST_CS", "1.0", SdmxStructureEnumType.Concept, "DECIMALS"),
                new StructureReferenceImpl("STS", "CL_DECIMALS", "1.0", SdmxStructureEnumType.CodeList));
            attributeMutableObject.AttachmentLevel = AttributeAttachmentLevel.DimensionGroup;
            attributeMutableObject.DimensionReferences.AddAll(new[] { "FREQ", "ADJUSTMENT", "STS_ACTIVITY" });
            attributeMutableObject.AssignmentStatus = "Mandatory";
            return dsdMutableObject.ImmutableInstance;
        }
    }
}