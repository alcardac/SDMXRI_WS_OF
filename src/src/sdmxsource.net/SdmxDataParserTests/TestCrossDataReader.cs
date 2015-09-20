// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestCrossDataReader.cs" company="EUROSTAT">
//   Date Created : 2014-08-01
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The test cross data reader.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxDataParserTests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Xml;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Org.Sdmxsource.Sdmx.DataParser.Factory;
    using Org.Sdmxsource.Sdmx.DataParser.Manager;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    ///     The test cross data reader.
    /// </summary>
    public class TestCrossDataReader
    {
        #region Fields

        /// <summary>
        ///     The _factory
        /// </summary>
        private readonly IReadableDataLocationFactory _factory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TestCrossDataReader" /> class.
        /// </summary>
        public TestCrossDataReader()
        {
            this._factory = new ReadableDataLocationFactory();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Tests the cross writer with no XS measure and without time.
        /// </summary>
        [Test]
        public void TestCrossWriterNoXSMeasureWithOutTime()
        {
            ICrossSectionalDataStructureObject crossSectionalDataStructureObject = BuildCrossDsdNoTime();
            var fileInfo = new FileInfo("cross-TestCrossWriterNoXSMeasureWithOutTime.xml");
            WriteCrossDataNoTime(fileInfo.ToString(), crossSectionalDataStructureObject);
            IReportedDateEngine reportedDateEngine = new ReportedDateEngine();
            var sdmxDataReaderFactory = new SdmxDataReaderFactory(new DataInformationManager(new FixedConceptEngine(), reportedDateEngine), null);
            using (var sourceData = this._factory.GetReadableDataLocation(fileInfo))
            using (var dataReaderEngine = sdmxDataReaderFactory.GetDataReaderEngine(sourceData, crossSectionalDataStructureObject, null))
            {
                Assert.NotNull(dataReaderEngine);
                Assert.NotNull(dataReaderEngine.Header);
                Assert.IsTrue(dataReaderEngine.MoveNextDataset());
                Assert.IsTrue(dataReaderEngine.MoveNextKeyable());
                Assert.AreEqual("Q", dataReaderEngine.CurrentKey.GetKeyValue("FREQ"));
                Assert.AreEqual("N", dataReaderEngine.CurrentKey.GetKeyValue("ADJUSTMENT"));
                Assert.AreEqual("A", dataReaderEngine.CurrentKey.GetKeyValue("STS_ACTIVITY"));
                Assert.IsTrue(dataReaderEngine.MoveNextObservation());
                IObservation currentObservation = dataReaderEngine.CurrentObservation;
                Assert.IsNotNull(currentObservation);
                Assert.IsNotNullOrEmpty(currentObservation.ObservationValue);
            }
        }

        /// <summary>
        ///     Tests the cross writer with no XS measure with out time flat.
        /// </summary>
        [Test]
        public void TestCrossWriterNoXSMeasureWithOutTimeFlat()
        {
            ICrossSectionalDataStructureObject crossSectionalDataStructureObject = BuildCrossDsdNoTimeFlat();
            var fileInfo = new FileInfo("cross-test.TestCrossWriterNoXSMeasureWithOutTimeFlat.xml");
            WriteCrossDataNoTimeFlat(fileInfo.ToString(), crossSectionalDataStructureObject);
            IReportedDateEngine reportedDateEngine = new ReportedDateEngine();
            var sdmxDataReaderFactory = new SdmxDataReaderFactory(new DataInformationManager(new FixedConceptEngine(), reportedDateEngine), null);
            using (var sourceData = this._factory.GetReadableDataLocation(fileInfo))
            using (var dataReaderEngine = sdmxDataReaderFactory.GetDataReaderEngine(sourceData, crossSectionalDataStructureObject, null))
            {
                Assert.NotNull(dataReaderEngine);
                Assert.NotNull(dataReaderEngine.Header);
                Assert.IsTrue(dataReaderEngine.MoveNextDataset());
                Assert.IsTrue(dataReaderEngine.MoveNextKeyable());
                Assert.AreEqual("Q", dataReaderEngine.CurrentKey.GetKeyValue("FREQ"));
                Assert.AreEqual("N", dataReaderEngine.CurrentKey.GetKeyValue("ADJUSTMENT"));
                Assert.AreEqual("A", dataReaderEngine.CurrentKey.GetKeyValue("STS_ACTIVITY"));
                Assert.IsTrue(dataReaderEngine.MoveNextObservation());
                IObservation currentObservation = dataReaderEngine.CurrentObservation;
                Assert.IsNotNull(currentObservation);
                Assert.IsNotNullOrEmpty(currentObservation.ObservationValue);
            }
        }

        /// <summary>
        ///     Tests the cross writer no XS measure with time.
        /// </summary>
        [Test]
        public void TestCrossWriterNoXSMeasureWithTime()
        {
            ICrossSectionalDataStructureObject crossSectionalDataStructureObject = BuildCrossDsd();
            var fileInfo = new FileInfo("cross-TestCrossWriterNoXSMeasureWithTime.xml");
            WriteCrossData(fileInfo.ToString(), crossSectionalDataStructureObject);
            IReportedDateEngine reportedDateEngine = new ReportedDateEngine();
            var sdmxDataReaderFactory = new SdmxDataReaderFactory(new DataInformationManager(new FixedConceptEngine(), reportedDateEngine), null);
            using (var sourceData = this._factory.GetReadableDataLocation(fileInfo))
            using (var dataReaderEngine = sdmxDataReaderFactory.GetDataReaderEngine(sourceData, crossSectionalDataStructureObject, null))
            {
                Assert.NotNull(dataReaderEngine);
                Assert.NotNull(dataReaderEngine.Header);
                Assert.IsTrue(dataReaderEngine.MoveNextDataset());
                Assert.IsTrue(dataReaderEngine.MoveNextKeyable());
                Assert.AreEqual("Q", dataReaderEngine.CurrentKey.GetKeyValue("FREQ"));
                Assert.AreEqual("N", dataReaderEngine.CurrentKey.GetKeyValue("ADJUSTMENT"));
                Assert.AreEqual("A", dataReaderEngine.CurrentKey.GetKeyValue("STS_ACTIVITY"));
                Assert.IsTrue(dataReaderEngine.MoveNextObservation());
                IObservation currentObservation = dataReaderEngine.CurrentObservation;
                Assert.AreEqual("2005-Q1", currentObservation.ObsTime);
            }
        }

        /// <summary>
        ///     Tests the cross writer XS measure with out time measure.
        /// </summary>
        [Test]
        public void TestCrossWriterXSMeasureWithOutTimeMeasure()
        {
            ICrossSectionalDataStructureObject crossSectionalDataStructureObject = BuildCrossDsdNoTimeMeasure();
            var fileInfo = new FileInfo("cross-test.TestCrossWriterXSMeasureWithOutTimeMeasure.xml");
            WriteCrossDataNoTimeMeasure(fileInfo.ToString(), crossSectionalDataStructureObject);
            IReportedDateEngine reportedDateEngine = new ReportedDateEngine();
            var sdmxDataReaderFactory = new SdmxDataReaderFactory(new DataInformationManager(new FixedConceptEngine(), reportedDateEngine), null);
            using (var sourceData = this._factory.GetReadableDataLocation(fileInfo))
            using (var dataReaderEngine = sdmxDataReaderFactory.GetDataReaderEngine(sourceData, crossSectionalDataStructureObject, null))
            {
                Assert.NotNull(dataReaderEngine);
                Assert.NotNull(dataReaderEngine.Header);
                Assert.IsTrue(dataReaderEngine.MoveNextDataset());
                Assert.IsTrue(dataReaderEngine.MoveNextKeyable());
                Assert.AreEqual("Q", dataReaderEngine.CurrentKey.GetKeyValue("FREQ"));
                Assert.AreEqual("N", dataReaderEngine.CurrentKey.GetKeyValue("ADJUSTMENT"));
                Assert.AreEqual("A", dataReaderEngine.CurrentKey.GetKeyValue("STS_ACTIVITY"));
                Assert.IsTrue(dataReaderEngine.MoveNextObservation());
                IObservation currentObservation = dataReaderEngine.CurrentObservation;
                Assert.IsNotNull(currentObservation);
                Assert.IsNotNullOrEmpty(currentObservation.ObservationValue);
            }
        }

        /// <summary>
        ///     Tests the like NSIWC.
        /// </summary>
        [Test]
        public void TestLikeNsiWcXsNoXsMeasure()
        {
            ICrossSectionalDataStructureObject dsd = BuildCrossDsdNoTime();
            var fileInfo = new FileInfo("cross-TestLikeNsiWcXsNoXsMeasure.xml");
            WriteCrossDataNoTime(fileInfo.ToString(), dsd);
            IDataReaderManager manager = new DataReaderManager();
            IList<IDictionary<string, string>> dataSetStoreList = new List<IDictionary<string, string>>();
            using (var sourceData = this._factory.GetReadableDataLocation(fileInfo))
            using (var compact = manager.GetDataReaderEngine(sourceData, dsd, null))
            {
                while (compact.MoveNextKeyable())
                {
                    if (compact.CurrentKey.Series)
                    {
                        IList<IKeyValue> keyValues = compact.CurrentKey.Key;

                        int index = 1;
                        while (compact.MoveNextObservation())
                        {
                            var dataSetStore = new Dictionary<string, string>(StringComparer.Ordinal);
                            foreach (var key in keyValues)
                            {
                                dataSetStore.Add(key.Concept, key.Code);
                            }

                            IObservation currentObservation = compact.CurrentObservation;
                            Assert.IsNotNullOrEmpty(currentObservation.ObservationValue);
                            Assert.IsNullOrEmpty(currentObservation.ObsTime);
                            if (currentObservation.CrossSection)
                            {
                                Assert.IsNotNull(currentObservation.CrossSectionalValue);
                                dataSetStore.Add(currentObservation.CrossSectionalValue.Concept, currentObservation.CrossSectionalValue.Code);
                            }

                            dataSetStore.Add(PrimaryMeasure.FixedId, currentObservation.ObservationValue);
                            var i = int.Parse(currentObservation.ObservationValue, NumberStyles.Any, CultureInfo.InvariantCulture);
                            Assert.AreEqual(index, i);

                            index++;
                            dataSetStoreList.Add(dataSetStore);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Tests the like NSIWC.
        /// </summary>
        [Test]
        public void TestLikeNsiWcXsNoXsMeasureFlat()
        {
            ICrossSectionalDataStructureObject crossSectionalDataStructureObject = BuildCrossDsdNoTimeFlat();
            var fileInfo = new FileInfo("cross-test.TestCrossWriterNoXSMeasureWithOutTimeFlat.xml");
            WriteCrossDataNoTimeFlat(fileInfo.ToString(), crossSectionalDataStructureObject);
            IDataReaderManager manager = new DataReaderManager();
            IList<IDictionary<string, string>> dataSetStoreList = new List<IDictionary<string, string>>();
            using (var sourceData = this._factory.GetReadableDataLocation(fileInfo))
            using (var compact = manager.GetDataReaderEngine(sourceData, crossSectionalDataStructureObject, null))
            {
                while (compact.MoveNextKeyable())
                {
                    if (compact.CurrentKey.Series)
                    {
                        IList<IKeyValue> keyValues = compact.CurrentKey.Key;

                        int index = 1;
                        while (compact.MoveNextObservation())
                        {
                            var dataSetStore = new Dictionary<string, string>(StringComparer.Ordinal);
                            foreach (var key in keyValues)
                            {
                                dataSetStore.Add(key.Concept, key.Code);
                            }

                            IObservation currentObservation = compact.CurrentObservation;
                            Assert.IsNotNullOrEmpty(currentObservation.ObservationValue);
                            Assert.IsNullOrEmpty(currentObservation.ObsTime);
                            if (currentObservation.CrossSection)
                            {
                                Assert.IsNotNull(currentObservation.CrossSectionalValue);
                                dataSetStore.Add(currentObservation.CrossSectionalValue.Concept, currentObservation.CrossSectionalValue.Code);
                            }

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

        /// <summary>
        ///     Tests the like NSIWC.
        /// </summary>
        [Test]
        public void TestLikeNsiWcXsXsMeasure()
        {
            ICrossSectionalDataStructureObject crossSectionalDataStructureObject = BuildCrossDsdNoTimeMeasure();
            var fileInfo = new FileInfo("cross-test.TestCrossWriterXSMeasureWithOutTimeMeasure.xml");
            WriteCrossDataNoTimeMeasure(fileInfo.ToString(), crossSectionalDataStructureObject);
            IDataReaderManager manager = new DataReaderManager();
            IList<IDictionary<string, string>> dataSetStoreList = new List<IDictionary<string, string>>();
            using (var sourceData = this._factory.GetReadableDataLocation(fileInfo))
            using (var compact = manager.GetDataReaderEngine(sourceData, crossSectionalDataStructureObject, null))
            {
                while (compact.MoveNextKeyable())
                {
                    if (compact.CurrentKey.Series)
                    {
                        IList<IKeyValue> keyValues = compact.CurrentKey.Key;

                        int index = 1;
                        while (compact.MoveNextObservation())
                        {
                            var dataSetStore = new Dictionary<string, string>(StringComparer.Ordinal);
                            foreach (var key in keyValues)
                            {
                                dataSetStore.Add(key.Concept, key.Code);
                            }

                            IObservation currentObservation = compact.CurrentObservation;
                            Assert.IsNotNullOrEmpty(currentObservation.ObservationValue);
                            Assert.IsNullOrEmpty(currentObservation.ObsTime);
                            if (currentObservation.CrossSection)
                            {
                                Assert.IsNotNull(currentObservation.CrossSectionalValue);
                            }

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

        #endregion

        #region Methods

        /// <summary>
        ///     Builds the cross DSD.
        /// </summary>
        /// <returns>The XS DSD</returns>
        private static ICrossSectionalDataStructureObject BuildCrossDsd()
        {
            ICrossSectionalDataStructureMutableObject dsdMutableObject = new CrossSectionalDataStructureMutableCore { AgencyId = "TEST", Id = "TEST_DSD", Version = "1.0" };

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
            dsdMutableObject.AddDimension(new DimensionMutableCore { ConceptRef = new StructureReferenceImpl("TEST", "TEST_CS", "1.0", SdmxStructureEnumType.Concept, "TIME"), TimeDimension = true });

            dsdMutableObject.AddPrimaryMeasure(new StructureReferenceImpl("TEST", "TEST_CS", "1.0", SdmxStructureEnumType.Concept, "OBS_VALUE"));

            dsdMutableObject.AddCrossSectionalAttachGroup("FREQ");
            dsdMutableObject.AddCrossSectionalAttachGroup("TIME_PERIOD");
            dsdMutableObject.AddCrossSectionalAttachSection("ADJUSTMENT");
            dsdMutableObject.AddCrossSectionalAttachObservation("STS_ACTIVITY");
            var attributeMutableObject = dsdMutableObject.AddAttribute(
                new StructureReferenceImpl("TEST", "TEST_CS", "1.0", SdmxStructureEnumType.Concept, "DECIMALS"), 
                new StructureReferenceImpl("STS", "CL_DECIMALS", "1.0", SdmxStructureEnumType.CodeList));
            dsdMutableObject.AddCrossSectionalAttachObservation("DECIMALS");
            attributeMutableObject.AttachmentLevel = AttributeAttachmentLevel.Observation;
            attributeMutableObject.AssignmentStatus = "Mandatory";
            return dsdMutableObject.ImmutableInstance;
        }

        /// <summary>
        ///     Builds the cross DSD.
        /// </summary>
        /// <returns>The XS DSD</returns>
        private static ICrossSectionalDataStructureObject BuildCrossDsdNoTime()
        {
            ICrossSectionalDataStructureMutableObject dsdMutableObject = new CrossSectionalDataStructureMutableCore { AgencyId = "TEST", Id = "TEST_DSD", Version = "1.0" };

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

            dsdMutableObject.AddPrimaryMeasure(new StructureReferenceImpl("TEST", "TEST_CS", "1.0", SdmxStructureEnumType.Concept, "OBS_VALUE"));

            dsdMutableObject.AddCrossSectionalAttachGroup("FREQ");
            dsdMutableObject.AddCrossSectionalAttachSection("ADJUSTMENT");
            dsdMutableObject.AddCrossSectionalAttachObservation("STS_ACTIVITY");
            var attributeMutableObject = dsdMutableObject.AddAttribute(
                new StructureReferenceImpl("TEST", "TEST_CS", "1.0", SdmxStructureEnumType.Concept, "DECIMALS"), 
                new StructureReferenceImpl("STS", "CL_DECIMALS", "1.0", SdmxStructureEnumType.CodeList));
            dsdMutableObject.AddCrossSectionalAttachObservation("DECIMALS");
            attributeMutableObject.AttachmentLevel = AttributeAttachmentLevel.Observation;
            attributeMutableObject.AssignmentStatus = "Mandatory";
            return dsdMutableObject.ImmutableInstance;
        }

        /// <summary>
        ///     Builds the cross DSD.
        /// </summary>
        /// <returns>The XS DSD</returns>
        private static ICrossSectionalDataStructureObject BuildCrossDsdNoTimeFlat()
        {
            ICrossSectionalDataStructureMutableObject dsdMutableObject = new CrossSectionalDataStructureMutableCore { AgencyId = "TEST", Id = "TEST_DSD", Version = "1.0" };

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

            dsdMutableObject.AddPrimaryMeasure(new StructureReferenceImpl("TEST", "TEST_CS", "1.0", SdmxStructureEnumType.Concept, "OBS_VALUE"));

            dsdMutableObject.AddCrossSectionalAttachObservation("FREQ");
            dsdMutableObject.AddCrossSectionalAttachObservation("ADJUSTMENT");
            dsdMutableObject.AddCrossSectionalAttachObservation("STS_ACTIVITY");
            var attributeMutableObject = dsdMutableObject.AddAttribute(
                new StructureReferenceImpl("TEST", "TEST_CS", "1.0", SdmxStructureEnumType.Concept, "DECIMALS"), 
                new StructureReferenceImpl("STS", "CL_DECIMALS", "1.0", SdmxStructureEnumType.CodeList));
            dsdMutableObject.AddCrossSectionalAttachObservation("DECIMALS");
            attributeMutableObject.AttachmentLevel = AttributeAttachmentLevel.Observation;
            attributeMutableObject.AssignmentStatus = "Mandatory";
            return dsdMutableObject.ImmutableInstance;
        }

        /// <summary>
        ///     Builds the cross DSD.
        /// </summary>
        /// <returns>The XS DSD</returns>
        private static ICrossSectionalDataStructureObject BuildCrossDsdNoTimeMeasure()
        {
            ICrossSectionalDataStructureMutableObject dsdMutableObject = new CrossSectionalDataStructureMutableCore { AgencyId = "TEST", Id = "TEST_DSD", Version = "1.0" };

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
                new StructureReferenceImpl("STS", "CL_STS_ACTIVITY", "1.0", SdmxStructureEnumType.CodeList)).MeasureDimension = true;

            dsdMutableObject.AddPrimaryMeasure(new StructureReferenceImpl("TEST", "TEST_CS", "1.0", SdmxStructureEnumType.Concept, "OBS_VALUE"));
            dsdMutableObject.AddCrossSectionalMeasures(
                new CrossSectionalMeasureMutableCore
                    {
                        Code = "A", 
                        ConceptRef = new StructureReferenceImpl("TEST", "TEST_CS", "1.0", SdmxStructureEnumType.Concept, "A"), 
                        MeasureDimension = "STS_ACTIVITY"
                    });
            dsdMutableObject.AddCrossSectionalMeasures(
                new CrossSectionalMeasureMutableCore
                    {
                        Code = "B", 
                        ConceptRef = new StructureReferenceImpl("TEST", "TEST_CS", "1.0", SdmxStructureEnumType.Concept, "B"), 
                        MeasureDimension = "STS_ACTIVITY"
                    });
            dsdMutableObject.AddCrossSectionalAttachGroup("FREQ");
            dsdMutableObject.AddCrossSectionalAttachSection("ADJUSTMENT");
            var attributeMutableObject = dsdMutableObject.AddAttribute(
                new StructureReferenceImpl("TEST", "TEST_CS", "1.0", SdmxStructureEnumType.Concept, "DECIMALS"), 
                new StructureReferenceImpl("STS", "CL_DECIMALS", "1.0", SdmxStructureEnumType.CodeList));
            dsdMutableObject.AddCrossSectionalAttachObservation("DECIMALS");
            attributeMutableObject.AttachmentLevel = AttributeAttachmentLevel.Observation;
            attributeMutableObject.AssignmentStatus = "Mandatory";
            return dsdMutableObject.ImmutableInstance;
        }

        /// <summary>
        /// Writes the cross data.
        /// </summary>
        /// <param name="outputFileName">
        /// Name of the output file.
        /// </param>
        /// <param name="crossSectionalDataStructureObject">
        /// The cross sectional data structure object.
        /// </param>
        private static void WriteCrossData(string outputFileName, ICrossSectionalDataStructureObject crossSectionalDataStructureObject)
        {
            using (XmlWriter writer = XmlWriter.Create(outputFileName, new XmlWriterSettings { Indent = true }))
            using (ICrossSectionalWriterEngine crossSectionalWriter = new CrossSectionalWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo)))
            {
                crossSectionalWriter.WriteHeader(new HeaderImpl("TEST_ID", "TEST_SENDER"));
                crossSectionalWriter.StartDataset(null, crossSectionalDataStructureObject, null);
                crossSectionalWriter.StartXSGroup();
                crossSectionalWriter.WriteXSGroupKeyValue("FREQ", "Q");
                crossSectionalWriter.WriteXSGroupKeyValue("TIME_PERIOD", "2005-Q1");
                crossSectionalWriter.StartSection();
                crossSectionalWriter.WriteSectionKeyValue("ADJUSTMENT", "N");
                crossSectionalWriter.StartXSObservation("OBS_VALUE", "1");
                crossSectionalWriter.WriteXSObservationKeyValue("STS_ACTIVITY", "A");
                crossSectionalWriter.WriteAttributeValue("DECIMALS", "3");
            }
        }

        /// <summary>
        /// Writes the cross data.
        /// </summary>
        /// <param name="outputFileName">
        /// Name of the output file.
        /// </param>
        /// <param name="crossSectionalDataStructureObject">
        /// The cross sectional data structure object.
        /// </param>
        private static void WriteCrossDataNoTime(string outputFileName, ICrossSectionalDataStructureObject crossSectionalDataStructureObject)
        {
            using (XmlWriter writer = XmlWriter.Create(outputFileName, new XmlWriterSettings { Indent = true }))
            using (ICrossSectionalWriterEngine crossSectionalWriter = new CrossSectionalWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo)))
            {
                crossSectionalWriter.WriteHeader(new HeaderImpl("TEST_ID", "TEST_SENDER"));
                crossSectionalWriter.StartDataset(null, crossSectionalDataStructureObject, null);
                crossSectionalWriter.StartXSGroup();
                crossSectionalWriter.WriteXSGroupKeyValue("FREQ", "Q");
                crossSectionalWriter.StartSection();
                crossSectionalWriter.WriteSectionKeyValue("ADJUSTMENT", "N");
                crossSectionalWriter.StartXSObservation("OBS_VALUE", "1");
                crossSectionalWriter.WriteXSObservationKeyValue("STS_ACTIVITY", "A");
                crossSectionalWriter.WriteAttributeValue("DECIMALS", "3");
            }
        }

        /// <summary>
        /// Writes the cross data.
        /// </summary>
        /// <param name="outputFileName">
        /// Name of the output file.
        /// </param>
        /// <param name="crossSectionalDataStructureObject">
        /// The cross sectional data structure object.
        /// </param>
        private static void WriteCrossDataNoTimeFlat(string outputFileName, ICrossSectionalDataStructureObject crossSectionalDataStructureObject)
        {
            using (XmlWriter writer = XmlWriter.Create(outputFileName, new XmlWriterSettings { Indent = true }))
            using (ICrossSectionalWriterEngine crossSectionalWriter = new CrossSectionalWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo)))
            {
                crossSectionalWriter.WriteHeader(new HeaderImpl("TEST_ID", "TEST_SENDER"));
                crossSectionalWriter.StartDataset(null, crossSectionalDataStructureObject, null);
                crossSectionalWriter.StartXSGroup();
                crossSectionalWriter.StartSection();
                crossSectionalWriter.StartXSObservation("OBS_VALUE", "1");
                crossSectionalWriter.WriteXSObservationKeyValue("FREQ", "Q");
                crossSectionalWriter.WriteXSObservationKeyValue("ADJUSTMENT", "N");
                crossSectionalWriter.WriteXSObservationKeyValue("STS_ACTIVITY", "A");
                crossSectionalWriter.WriteAttributeValue("DECIMALS", "3");
            }
        }

        /// <summary>
        /// Writes the cross data.
        /// </summary>
        /// <param name="outputFileName">
        /// Name of the output file.
        /// </param>
        /// <param name="crossSectionalDataStructureObject">
        /// The cross sectional data structure object.
        /// </param>
        private static void WriteCrossDataNoTimeMeasure(string outputFileName, ICrossSectionalDataStructureObject crossSectionalDataStructureObject)
        {
            using (XmlWriter writer = XmlWriter.Create(outputFileName, new XmlWriterSettings { Indent = true }))
            using (ICrossSectionalWriterEngine crossSectionalWriter = new CrossSectionalWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo)))
            {
                crossSectionalWriter.WriteHeader(new HeaderImpl("TEST_ID", "TEST_SENDER"));
                crossSectionalWriter.StartDataset(null, crossSectionalDataStructureObject, null);
                crossSectionalWriter.StartXSGroup();
                crossSectionalWriter.WriteXSGroupKeyValue("FREQ", "Q");
                crossSectionalWriter.StartSection();
                crossSectionalWriter.WriteSectionKeyValue("ADJUSTMENT", "N");
                crossSectionalWriter.StartXSObservation("A", "1");
                crossSectionalWriter.WriteAttributeValue("DECIMALS", "3");
                crossSectionalWriter.StartXSObservation("B", "2");
                crossSectionalWriter.WriteAttributeValue("DECIMALS", "3");
            }
        }

        #endregion
    }
}