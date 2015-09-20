// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDataWriters.cs" company="EUROSTAT">
//   Date Created : 2014-07-22
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Test unit for SDMX Data Writers
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxDataParserTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Org.Sdmxsource.Sdmx.DataParser.Manager;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Sdmx.Util.Date;
    using Org.Sdmxsource.Util.Extensions;
    using Org.Sdmxsource.Util.Io;
    using Org.Sdmxsource.XmlHelper;

    /// <summary>
    ///     Test unit for SDMX Data Writers
    /// </summary>
    [TestFixture]
    public class TestDataWriters
    {
        #region Public Methods and Operators

        /// <summary>
        /// Tests the cross sectional data writer.
        /// </summary>
        /// <param name="dsd">
        /// The DSD.
        /// </param>
        [TestCase("tests/v20/ESTAT+DEMOGRAPHY+2.1.xml")]
        [TestCase("tests/v20/CENSUSHUB+ESTAT+1.1_alllevels.xml")]
        [TestCase("tests/v20/EGR_1_TS+ESTAT+1.4.xml")]
        [TestCase("tests/v20/CENSAGR_CAPOAZ_GEN+IT1+1.3.xml")]
        public void TestCrossSectionalDataWriter(string dsd)
        {
            ISdmxObjects objects;
            var file = new FileInfo(dsd);
            IStructureParsingManager manager = new StructureParsingManager(SdmxSchemaEnumType.Null);
            using (var readable = new FileReadableDataLocation(file))
            {
                IStructureWorkspace structureWorkspace = manager.ParseStructures(readable);
                objects = structureWorkspace.GetStructureObjects(false);
            }

            foreach (var dataStructureObject in objects.DataStructures)
            {
                var crossDsd = dataStructureObject as ICrossSectionalDataStructureObject;
                Assert.IsNotNull(crossDsd);
                using (var writer = XmlWriter.Create("cross-" + file.Name))
                {
                    var crossWriter = new CrossSectionalWriterEngine(writer, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo));
                    crossWriter.WriteHeader(new HeaderImpl("TEST", "TEST"));
                    crossWriter.StartDataset(null, crossDsd, null);
                    foreach (var component in crossDsd.GetCrossSectionalAttachDataSet(true))
                    {
                        switch (component.StructureType.EnumType)
                        {
                            case SdmxStructureEnumType.Dimension:
                                crossWriter.WriteDataSetKeyValue(component.ConceptRef.ChildReference.Id, "DS");
                                break;
                            case SdmxStructureEnumType.DataAttribute:
                                crossWriter.WriteAttributeValue(component.ConceptRef.ChildReference.Id, "DS");
                                break;
                        }
                    }

                    crossWriter.StartXSGroup();
                    var timeDimension = crossDsd.GetDimensions(SdmxStructureEnumType.TimeDimension).FirstOrDefault();
                    IDimension freq = crossDsd.GetDimensions().FirstOrDefault(dimension => dimension.FrequencyDimension);
                    IAttributeObject timeFormat = crossDsd.Attributes.FirstOrDefault(o => o.TimeFormat);
                    if (timeDimension != null)
                    {
                        crossWriter.WriteXSGroupKeyValue(timeDimension.ConceptRef.ChildReference.Id, "2000");
                        if (freq != null)
                        {
                            crossWriter.WriteXSGroupKeyValue(freq.ConceptRef.ChildReference.Id, "A");
                        }

                        if (timeFormat != null)
                        {
                            crossWriter.WriteAttributeValue(timeFormat.ConceptRef.ChildReference.Id, "DS");
                        }
                    }

                    foreach (var component in crossDsd.GetCrossSectionalAttachGroup(true))
                    {
                        switch (component.StructureType.EnumType)
                        {
                            case SdmxStructureEnumType.Dimension:
                                {
                                    if (!component.Equals(freq))
                                    {
                                        crossWriter.WriteXSGroupKeyValue(component.ConceptRef.ChildReference.Id, "DS");
                                    }
                                }

                                break;
                            case SdmxStructureEnumType.DataAttribute:
                                if (!component.Equals(timeFormat))
                                {
                                    crossWriter.WriteAttributeValue(component.ConceptRef.ChildReference.Id, "DS");
                                }

                                break;
                        }
                    }

                    crossWriter.StartSection();
                    foreach (var component in crossDsd.GetCrossSectionalAttachSection(true))
                    {
                        switch (component.StructureType.EnumType)
                        {
                            case SdmxStructureEnumType.Dimension:
                                crossWriter.WriteXSGroupKeyValue(component.ConceptRef.ChildReference.Id, "DS");
                                break;
                            case SdmxStructureEnumType.DataAttribute:
                                crossWriter.WriteAttributeValue(component.ConceptRef.ChildReference.Id, "DS");
                                break;
                        }
                    }

                    if (crossDsd.CrossSectionalMeasures.Count > 0)
                    {
                        for (int index = 0; index < crossDsd.CrossSectionalMeasures.Count; index++)
                        {
                            var crossSectionalMeasure = crossDsd.CrossSectionalMeasures[index];
                            crossWriter.StartXSObservation(crossSectionalMeasure.Code, "1.002");
                            foreach (var component in crossDsd.GetCrossSectionalAttachObservation())
                            {
                                switch (component.StructureType.EnumType)
                                {
                                    case SdmxStructureEnumType.Dimension:
                                        crossWriter.WriteXSGroupKeyValue(component.ConceptRef.ChildReference.Id, "XS" + index);
                                        break;
                                    case SdmxStructureEnumType.DataAttribute:
                                        crossWriter.WriteAttributeValue(component.ConceptRef.ChildReference.Id, "XSOBS");
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        crossWriter.StartXSObservation(crossDsd.PrimaryMeasure.ConceptRef.ChildReference.Id, "1.002");
                        foreach (var component in crossDsd.GetCrossSectionalAttachObservation())
                        {
                            switch (component.StructureType.EnumType)
                            {
                                case SdmxStructureEnumType.Dimension:
                                    crossWriter.WriteXSGroupKeyValue(component.ConceptRef.ChildReference.Id, "PM");
                                    break;
                                case SdmxStructureEnumType.DataAttribute:
                                    crossWriter.WriteAttributeValue(component.ConceptRef.ChildReference.Id, "PM");
                                    break;
                            }
                        }
                    }

                    crossWriter.Close();
                }
            }
        }

        /// <summary>
        /// Test unit for <see cref="CompactDataWriterEngine"/>
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        [TestCase(DataEnumType.Compact20)]
        [TestCase(DataEnumType.Compact21)]
        [TestCase(DataEnumType.Generic20)]
        [TestCase(DataEnumType.Generic21)]
        [TestCase(DataEnumType.EdiTs)]
        public void TestDataWriterEngine(DataEnumType format)
        {
            string outfile = string.Format("{0}.xml", format);
            DataType fromEnum = DataType.GetFromEnum(format);
            using (Stream writer = File.Create(outfile))
            {
                IConceptSchemeMutableObject conceptScheme = new ConceptSchemeMutableCore { Id = "CONCEPTS_TEST", AgencyId = "TEST" };
                conceptScheme.AddName("en", "Dummy concept scheme build for this tests");

                ////conceptScheme.Annotations.Add(new AnnotationMutableCore() { Id = "ANNOTABLETEST", Title = "Test", Type = "ATYPE" });
                conceptScheme.FinalStructure = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.True);

                IConceptSchemeObject parent = conceptScheme.ImmutableInstance;

                IConceptObject freqConcept = CreateConcept(parent, "FREQ", "Frequency");

                IConceptObject adjustmentConcept = CreateConcept(parent, "ADJUSTMENT", "The Adjustment");

                IConceptObject activityConcpet = CreateConcept(parent, "STS_ACTIVITY", "Name of activity ");

                IConceptObject timeDimensionConcpet = CreateConcept(parent, "TIME_PERIOD", "Name of  Time Period");
                IConceptObject decimalsConcept = CreateConcept(parent, "DECIMALS", "Name of concept");

                IConceptObject obsConcept = CreateConcept(parent, "OBS_VALUE", "Name of  observation value");

                ICodelistObject freqCl = CreateCodelist("CL_FREQ", "Freq codelist", "Q", "A", "M");
                ICodelistObject adjCl = CreateCodelist("CL_ADJUSTMENT", "Adjustment codelist", "N", "S", "W");
                ICodelistObject actCl = CreateCodelist("CL_ACTIVITY", "Activity codelist", "A", "B", "C");
                ICodelistObject deciCl = CreateCodelist("CL_DECIMALS", "DECIMALS codelist", "1", "2", "0");

                IDataStructureMutableObject mutable = new DataStructureMutableCore { Id = "TEST_DSD", AgencyId = "TEST" };
                mutable.AddName("en", "FOO BAR");
                mutable.AddDimension(
                    new DimensionMutableCore { ConceptRef = freqConcept.AsReference, FrequencyDimension = true, Representation = new RepresentationMutableCore { Representation = freqCl.AsReference } });
                mutable.AddDimension(new DimensionMutableCore { ConceptRef = adjustmentConcept.AsReference, Representation = new RepresentationMutableCore { Representation = adjCl.AsReference } });
                mutable.AddDimension(new DimensionMutableCore { ConceptRef = activityConcpet.AsReference, Representation = new RepresentationMutableCore { Representation = actCl.AsReference } });
                mutable.AddDimension(new DimensionMutableCore { ConceptRef = timeDimensionConcpet.AsReference, TimeDimension = true });

                IList<string> dimList = new List<string> { freqConcept.Id, adjustmentConcept.Id, adjustmentConcept.Id };
                var attributeMutableCore = new AttributeMutableCore
                                               {
                                                   ConceptRef = decimalsConcept.AsReference, 
                                                   Representation = new RepresentationMutableCore { Representation = deciCl.AsReference }, 
                                                   AttachmentLevel = AttributeAttachmentLevel.DimensionGroup, 
                                                   AssignmentStatus = AttributeAssignmentStatus.Mandatory.ToString()
                                               };
                attributeMutableCore.DimensionReferences.AddAll(dimList);
                mutable.AddAttribute(attributeMutableCore);

                mutable.PrimaryMeasure = new PrimaryMeasureMutableCore { ConceptRef = obsConcept.AsReference };

                var manager = new DataWriterManager();
                IDataWriterEngine dataWriter = manager.GetDataWriterEngine(new SdmxDataFormatCore(fromEnum), writer);

                IDataStructureObject dataStructureObject = mutable.ImmutableInstance;
                IList<IDatasetStructureReference> structures = new List<IDatasetStructureReference> { new DatasetStructureReferenceCore(dataStructureObject.AsReference) };
                IList<IParty> receiver = new List<IParty> { new PartyCore(new List<ITextTypeWrapper>(), "ZZ9", new List<IContact>(), null) };
                var sender = new PartyCore(new List<ITextTypeWrapper> { new TextTypeWrapperImpl("en", "TEST SENDER", null) }, "ZZ1", null, null);
                IHeader header = new HeaderImpl(
                    null, 
                    structures, 
                    null, 
                    DatasetAction.GetFromEnum(DatasetActionEnumType.Information), 
                    "TEST_DATAFLOW", 
                    "DATASET_ID", 
                    null, 
                    DateTime.Now, 
                    DateTime.Now, 
                    null, 
                    null, 
                    new List<ITextTypeWrapper> { new TextTypeWrapperImpl("en", "test header name", null) }, 
                    new List<ITextTypeWrapper> { new TextTypeWrapperImpl("en", "source 1", null) }, 
                    receiver, 
                    sender, 
                    true);
                dataWriter.WriteHeader(header);

                dataWriter.StartDataset(null, dataStructureObject, null);
                var sw = new Stopwatch();
                sw.Start();
                var series = from f in freqCl.Items from ad in adjCl.Items from ac in actCl.Items select new { f, ad, ac };

                sw.Stop();
                Trace.WriteLine(sw.Elapsed);
                sw.Reset();

                var startTime = new DateTime(2005, 1, 1);
                sw.Start();
                foreach (var key in series)
                {
                    dataWriter.StartSeries();
                    dataWriter.WriteSeriesKeyValue(freqConcept.Id, key.f.Id);
                    dataWriter.WriteSeriesKeyValue(adjustmentConcept.Id, key.ad.Id);
                    dataWriter.WriteSeriesKeyValue(activityConcpet.Id, key.ac.Id);

                    Func<int, string> getPeriod = null;
                    switch (key.f.Id)
                    {
                        case "Q":
                            getPeriod = i =>
                                {
                                    DateTime months = startTime.AddMonths(3 * i);
                                    return DateUtil.FormatDate(months, TimeFormatEnumType.QuarterOfYear);
                                };
                            dataWriter.WriteAttributeValue(decimalsConcept.Id, "1");
                            break;
                        case "A":
                            getPeriod = i =>
                                {
                                    DateTime months = startTime.AddMonths(12 * i);
                                    return DateUtil.FormatDate(months, TimeFormatEnumType.Year);
                                };
                            dataWriter.WriteAttributeValue(decimalsConcept.Id, "0");
                            break;
                        case "M":
                            getPeriod = i =>
                                {
                                    DateTime months = startTime.AddMonths(i + 1);
                                    return DateUtil.FormatDate(months, TimeFormatEnumType.Month);
                                };
                            dataWriter.WriteAttributeValue(decimalsConcept.Id, "2");
                            break;
                        default:
                            Assert.Fail("Test bug. Check CL_FREQ codes");
                            break;
                    }

                    for (int i = 0; i < 100; i++)
                    {
                        string period = getPeriod(i);
                        dataWriter.WriteObservation(period, i.ToString(CultureInfo.InvariantCulture));
                    }
                }

                dataWriter.Close();
                sw.Stop();
                Trace.WriteLine(sw.Elapsed);
            }

            if (fromEnum.BaseDataFormat.EnumType == BaseDataFormatEnumType.Generic)
            {
                var fileReadableDataLocation = new FileReadableDataLocation(outfile);

                XMLParser.ValidateXml(fileReadableDataLocation, fromEnum.SchemaVersion);
            }
        }

        /// <summary>
        /// Tests the data writer engine all dimensions.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        [TestCase(DataEnumType.Compact21)]
        [TestCase(DataEnumType.Generic21)]
        public void TestDataWriterEngineAllDimensions(DataEnumType format)
        {
            string outfile = string.Format("{0}-alldim.xml", format);
            DataType fromEnum = DataType.GetFromEnum(format);
            using (Stream writer = File.Create(outfile))
            {
                IConceptSchemeMutableObject conceptScheme = new ConceptSchemeMutableCore { Id = "CONCEPTS_TEST", AgencyId = "TEST" };
                conceptScheme.AddName("en", "Dummy concept scheme build for this tests");

                ////conceptScheme.Annotations.Add(new AnnotationMutableCore() { Id = "ANNOTABLETEST", Title = "Test", Type = "ATYPE" });
                conceptScheme.FinalStructure = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.True);

                IConceptSchemeObject parent = conceptScheme.ImmutableInstance;

                IConceptObject freqConcept = CreateConcept(parent, "FREQ", "Frequency");

                IConceptObject adjustmentConcept = CreateConcept(parent, "ADJUSTMENT", "The Adjustment");

                IConceptObject activityConcpet = CreateConcept(parent, "STS_ACTIVITY", "Name of activity ");

                IConceptObject timeDimensionConcpet = CreateConcept(parent, "TIME_PERIOD", "Name of  Time Period");
                IConceptObject decimalsConcept = CreateConcept(parent, "DECIMALS", "Name of concept");

                IConceptObject obsConcept = CreateConcept(parent, "OBS_VALUE", "Name of  observation value");

                ICodelistObject freqCl = CreateCodelist("CL_FREQ", "Freq codelist", "Q", "A", "M");
                ICodelistObject adjCl = CreateCodelist("CL_ADJUSTMENT", "Adjustment codelist", "N", "S", "W");
                ICodelistObject actCl = CreateCodelist("CL_ACTIVITY", "Activity codelist", "A", "B", "C");
                ICodelistObject deciCl = CreateCodelist("CL_DECIMALS", "DECIMALS codelist", "1", "2", "0");

                IDataStructureMutableObject mutable = new DataStructureMutableCore { Id = "TEST_DSD", AgencyId = "TEST" };
                mutable.AddName("en", "FOO BAR");
                mutable.AddDimension(
                    new DimensionMutableCore { ConceptRef = freqConcept.AsReference, FrequencyDimension = true, Representation = new RepresentationMutableCore { Representation = freqCl.AsReference } });
                mutable.AddDimension(new DimensionMutableCore { ConceptRef = adjustmentConcept.AsReference, Representation = new RepresentationMutableCore { Representation = adjCl.AsReference } });
                mutable.AddDimension(new DimensionMutableCore { ConceptRef = activityConcpet.AsReference, Representation = new RepresentationMutableCore { Representation = actCl.AsReference } });
                mutable.AddDimension(new DimensionMutableCore { ConceptRef = timeDimensionConcpet.AsReference, TimeDimension = true });

                IList<string> dimList = new List<string> { freqConcept.Id, adjustmentConcept.Id, adjustmentConcept.Id };
                var attributeMutableCore = new AttributeMutableCore
                                               {
                                                   ConceptRef = decimalsConcept.AsReference, 
                                                   Representation = new RepresentationMutableCore { Representation = deciCl.AsReference }, 
                                                   AttachmentLevel = AttributeAttachmentLevel.DimensionGroup, 
                                                   AssignmentStatus = AttributeAssignmentStatus.Mandatory.ToString()
                                               };
                attributeMutableCore.DimensionReferences.AddAll(dimList);
                mutable.AddAttribute(attributeMutableCore);

                mutable.PrimaryMeasure = new PrimaryMeasureMutableCore { ConceptRef = obsConcept.AsReference };

                var manager = new DataWriterManager();
                IDataWriterEngine dataWriter = manager.GetDataWriterEngine(new SdmxDataFormatCore(fromEnum), writer);

                IDataStructureObject dataStructureObject = mutable.ImmutableInstance;
                IList<IDatasetStructureReference> structures = new List<IDatasetStructureReference>
                                                                   {
                                                                       new DatasetStructureReferenceCore(
                                                                           null, 
                                                                           dataStructureObject.AsReference, 
                                                                           null, 
                                                                           null, 
                                                                           DatasetStructureReference.AllDimensions)
                                                                   };
                IList<IParty> receiver = new List<IParty> { new PartyCore(new List<ITextTypeWrapper>(), "ZZ9", new List<IContact>(), null) };
                var sender = new PartyCore(new List<ITextTypeWrapper> { new TextTypeWrapperImpl("en", "TEST SENDER", null) }, "ZZ1", null, null);
                IHeader header = new HeaderImpl(
                    null, 
                    structures, 
                    null, 
                    DatasetAction.GetFromEnum(DatasetActionEnumType.Information), 
                    "TEST_DATAFLOW", 
                    "DATASET_ID", 
                    null, 
                    DateTime.Now, 
                    DateTime.Now, 
                    null, 
                    null, 
                    new List<ITextTypeWrapper> { new TextTypeWrapperImpl("en", "test header name", null) }, 
                    new List<ITextTypeWrapper> { new TextTypeWrapperImpl("en", "source 1", null) }, 
                    receiver, 
                    sender, 
                    true);
                dataWriter.WriteHeader(header);

                dataWriter.StartDataset(null, dataStructureObject, new DatasetHeaderCore(null, DatasetAction.GetFromEnum(DatasetActionEnumType.Information), structures.First()));
                var sw = new Stopwatch();
                sw.Start();
                var series = from f in freqCl.Items from ad in adjCl.Items from ac in actCl.Items select new { f, ad, ac };

                sw.Stop();
                Trace.WriteLine(sw.Elapsed);
                sw.Reset();

                var startTime = new DateTime(2005, 1, 1);
                sw.Start();
                foreach (var key in series)
                {
                    dataWriter.StartSeries();
                    dataWriter.WriteSeriesKeyValue(freqConcept.Id, key.f.Id);
                    dataWriter.WriteSeriesKeyValue(adjustmentConcept.Id, key.ad.Id);
                    dataWriter.WriteSeriesKeyValue(activityConcpet.Id, key.ac.Id);

                    Func<int, string> getPeriod = null;
                    switch (key.f.Id)
                    {
                        case "Q":
                            getPeriod = i =>
                                {
                                    DateTime months = startTime.AddMonths(3 * i);
                                    return DateUtil.FormatDate(months, TimeFormatEnumType.QuarterOfYear);
                                };
                            dataWriter.WriteAttributeValue(decimalsConcept.Id, "1");
                            break;
                        case "A":
                            getPeriod = i =>
                                {
                                    DateTime months = startTime.AddMonths(12 * i);
                                    return DateUtil.FormatDate(months, TimeFormatEnumType.Year);
                                };
                            dataWriter.WriteAttributeValue(decimalsConcept.Id, "0");
                            break;
                        case "M":
                            getPeriod = i =>
                                {
                                    DateTime months = startTime.AddMonths(i + 1);
                                    return DateUtil.FormatDate(months, TimeFormatEnumType.Month);
                                };
                            dataWriter.WriteAttributeValue(decimalsConcept.Id, "2");
                            break;
                        default:
                            Assert.Fail("Test bug. Check CL_FREQ codes");
                            break;
                    }

                    for (int i = 0; i < 100; i++)
                    {
                        string period = getPeriod(i);
                        dataWriter.WriteObservation(DimensionObject.TimeDimensionFixedId, period, i.ToString(CultureInfo.InvariantCulture));
                    }
                }

                dataWriter.Close();
                sw.Stop();
                Trace.WriteLine(sw.Elapsed);
            }

            if (fromEnum.BaseDataFormat.EnumType == BaseDataFormatEnumType.Generic)
            {
                var fileReadableDataLocation = new FileReadableDataLocation(outfile);

                XMLParser.ValidateXml(fileReadableDataLocation, fromEnum.SchemaVersion);
            }
        }

        /// <summary>
        /// Test unit for <see cref="CompactDataWriterEngine"/>
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="dimAtObs">
        /// The dim at OBS.
        /// </param>
        [TestCase(DataEnumType.Compact21, "STS_ACTIVITY")]
        [TestCase(DataEnumType.Generic21, "STS_ACTIVITY")]
        [TestCase(DataEnumType.Compact21, "ADJUSTMENT")]
        [TestCase(DataEnumType.Generic21, "ADJUSTMENT")]
        [TestCase(DataEnumType.Compact21, "FREQ")]
        [TestCase(DataEnumType.Generic21, "FREQ")]
        public void TestDataWriterEngineDimensionAtObs(DataEnumType format, string dimAtObs)
        {
            string outfile = string.Format("{0}-{1}.xml", format, dimAtObs);
            DataType fromEnum = DataType.GetFromEnum(format);
            using (Stream writer = File.Create(outfile))
            {
                IConceptSchemeMutableObject conceptScheme = new ConceptSchemeMutableCore { Id = "CONCEPTS_TEST", AgencyId = "TEST" };
                conceptScheme.AddName("en", "Dummy concept scheme build for this tests");

                ////conceptScheme.Annotations.Add(new AnnotationMutableCore() { Id = "ANNOTABLETEST", Title = "Test", Type = "ATYPE" });
                conceptScheme.FinalStructure = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.True);

                IConceptSchemeObject parent = conceptScheme.ImmutableInstance;

                IConceptObject freqConcept = CreateConcept(parent, "FREQ", "Frequency");

                IConceptObject adjustmentConcept = CreateConcept(parent, "ADJUSTMENT", "The Adjustment");

                IConceptObject activityConcpet = CreateConcept(parent, "STS_ACTIVITY", "Name of activity ");

                IConceptObject timeDimensionConcpet = CreateConcept(parent, "TIME_PERIOD", "Name of  Time Period");
                IConceptObject decimalsConcept = CreateConcept(parent, "DECIMALS", "Name of concept");

                IConceptObject obsConcept = CreateConcept(parent, "OBS_VALUE", "Name of  observation value");

                ICodelistObject freqCl = CreateCodelist("CL_FREQ", "Freq codelist", "Q", "A", "M");
                ICodelistObject adjCl = CreateCodelist("CL_ADJUSTMENT", "Adjustment codelist", "N", "S", "W");
                ICodelistObject actCl = CreateCodelist("CL_ACTIVITY", "Activity codelist", "A", "B", "C");
                ICodelistObject deciCl = CreateCodelist("CL_DECIMALS", "DECIMALS codelist", "1", "2", "0");

                IDataStructureMutableObject mutable = new DataStructureMutableCore { Id = "TEST_DSD", AgencyId = "TEST" };
                mutable.AddName("en", "FOO BAR");
                mutable.AddDimension(
                    new DimensionMutableCore { ConceptRef = freqConcept.AsReference, FrequencyDimension = true, Representation = new RepresentationMutableCore { Representation = freqCl.AsReference } });
                mutable.AddDimension(new DimensionMutableCore { ConceptRef = adjustmentConcept.AsReference, Representation = new RepresentationMutableCore { Representation = adjCl.AsReference } });
                mutable.AddDimension(new DimensionMutableCore { ConceptRef = activityConcpet.AsReference, Representation = new RepresentationMutableCore { Representation = actCl.AsReference } });
                mutable.AddDimension(new DimensionMutableCore { ConceptRef = timeDimensionConcpet.AsReference, TimeDimension = true });

                IList<string> dimList = new List<string> { freqConcept.Id, adjustmentConcept.Id, adjustmentConcept.Id };
                var attributeMutableCore = new AttributeMutableCore
                                               {
                                                   ConceptRef = decimalsConcept.AsReference, 
                                                   Representation = new RepresentationMutableCore { Representation = deciCl.AsReference }, 
                                                   AttachmentLevel = AttributeAttachmentLevel.DimensionGroup, 
                                                   AssignmentStatus = AttributeAssignmentStatus.Mandatory.ToString()
                                               };
                attributeMutableCore.DimensionReferences.AddAll(dimList);
                mutable.AddAttribute(attributeMutableCore);

                mutable.PrimaryMeasure = new PrimaryMeasureMutableCore { ConceptRef = obsConcept.AsReference };

                var manager = new DataWriterManager();
                IDataWriterEngine dataWriter = manager.GetDataWriterEngine(new SdmxDataFormatCore(fromEnum), writer);

                IDataStructureObject dataStructureObject = mutable.ImmutableInstance;
                IList<IDatasetStructureReference> structures = new List<IDatasetStructureReference> { new DatasetStructureReferenceCore(null, dataStructureObject.AsReference, null, null, dimAtObs) };
                IList<IParty> receiver = new List<IParty> { new PartyCore(new List<ITextTypeWrapper>(), "ZZ9", new List<IContact>(), null) };
                var sender = new PartyCore(new List<ITextTypeWrapper> { new TextTypeWrapperImpl("en", "TEST SENDER", null) }, "ZZ1", null, null);
                IHeader header = new HeaderImpl(
                    null, 
                    structures, 
                    null, 
                    DatasetAction.GetFromEnum(DatasetActionEnumType.Information), 
                    "TEST_DATAFLOW", 
                    "DATASET_ID", 
                    null, 
                    DateTime.Now, 
                    DateTime.Now, 
                    null, 
                    null, 
                    new List<ITextTypeWrapper> { new TextTypeWrapperImpl("en", "test header name", null) }, 
                    new List<ITextTypeWrapper> { new TextTypeWrapperImpl("en", "source 1", null) }, 
                    receiver, 
                    sender, 
                    true);
                dataWriter.WriteHeader(header);

                dataWriter.StartDataset(null, dataStructureObject, new DatasetHeaderCore(null, DatasetAction.GetFromEnum(DatasetActionEnumType.Information), structures.First()));
                var sw = new Stopwatch();
                sw.Start();

                var startTime = new DateTime(2005, 1, 1);

                var series = (from f in freqCl.Items
                              from ad in adjCl.Items
                              from ac in actCl.Items
                              from t in Enumerable.Range(0, 100)
                              select new { Freq = f.Id, Adj = ad.Id, Activity = ac.Id, Time = BuildPeriodResolver(f.Id, startTime)(t) }).OrderBy(
                                  arg => BuildOrderBy(arg.Freq, arg.Adj, arg.Activity, arg.Time, dimAtObs)).ToArray();

                sw.Stop();
                Trace.WriteLine(sw.Elapsed);
                sw.Reset();

                sw.Start();
                string lastKey = null;
                int i = 0;
                foreach (var key in series)
                {
                    var currentKey = new List<string>();
                    string crossValue = null;
                    if (!string.Equals(freqConcept.Id, dimAtObs))
                    {
                        currentKey.Add(key.Freq);
                    }
                    else
                    {
                        crossValue = key.Freq;
                    }

                    if (!string.Equals(adjustmentConcept.Id, dimAtObs))
                    {
                        currentKey.Add(key.Adj);
                    }
                    else
                    {
                        crossValue = key.Adj;
                    }

                    if (!string.Equals(activityConcpet.Id, dimAtObs))
                    {
                        currentKey.Add(key.Activity);
                    }
                    else
                    {
                        crossValue = key.Activity;
                    }

                    if (!string.Equals(timeDimensionConcpet.Id, dimAtObs))
                    {
                        currentKey.Add(key.Time);
                    }
                    else
                    {
                        crossValue = key.Time;
                    }

                    var currentKeyValues = string.Join(",", currentKey);

                    if (lastKey == null || !lastKey.Equals(currentKeyValues))
                    {
                        lastKey = currentKeyValues;
                        i = 0;

                        dataWriter.StartSeries();
                        if (!string.Equals(freqConcept.Id, dimAtObs))
                        {
                            dataWriter.WriteSeriesKeyValue(freqConcept.Id, key.Freq);
                        }

                        if (!string.Equals(adjustmentConcept.Id, dimAtObs))
                        {
                            dataWriter.WriteSeriesKeyValue(adjustmentConcept.Id, key.Adj);
                        }

                        if (!string.Equals(activityConcpet.Id, dimAtObs))
                        {
                            dataWriter.WriteSeriesKeyValue(activityConcpet.Id, key.Activity);
                        }

                        if (!string.Equals(timeDimensionConcpet.Id, dimAtObs))
                        {
                            dataWriter.WriteSeriesKeyValue(timeDimensionConcpet.Id, key.Time);
                        }
                    }

                    dataWriter.WriteObservation(crossValue, i.ToString(CultureInfo.InvariantCulture));
                    i++;
                }

                dataWriter.Close();
                sw.Stop();
                Trace.WriteLine(sw.Elapsed);
            }

            if (fromEnum.BaseDataFormat.EnumType == BaseDataFormatEnumType.Generic)
            {
                var fileReadableDataLocation = new FileReadableDataLocation(outfile);

                XMLParser.ValidateXml(fileReadableDataLocation, fromEnum.SchemaVersion);
            }
        }

        private static void WriteSeriesKey(string dimAtObs, string dimensionId, IDataWriterEngine dataWriter, string dimensionValue, List<string> currentKey)
        {
            if (!string.Equals(dimensionId, dimAtObs))
            {
                dataWriter.WriteSeriesKeyValue(dimensionId, dimensionValue);
                currentKey.Add(dimensionValue);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the order by string.
        /// </summary>
        /// <param name="freq">The frequency.</param>
        /// <param name="adjustment">The adjustment.</param>
        /// <param name="stsActivity">The STS activity.</param>
        /// <param name="time">The time.</param>
        /// <param name="dimensionAtObs">The dimension at observation.</param>
        /// <returns>
        /// The string to use for comparison.
        /// </returns>
        private static string BuildOrderBy(string freq, string adjustment, string stsActivity, string time, string dimensionAtObs)
        {
            string[] values;
            switch (dimensionAtObs)
            {
                case "FREQ":
                    values = new[] { adjustment, stsActivity, time, freq };
                    break;
                case "ADJUSTMENT":
                    values = new[] { freq, stsActivity, time, adjustment };
                    break;
                case "STS_ACTIVITY":
                    values = new[] { freq, adjustment, time, stsActivity };
                    break;
                default:
                    values = new[] { freq, adjustment, stsActivity, time };
                    break;
            }

            return string.Join(",", values);
        }

        /// <summary>
        /// Builds the period resolver.
        /// </summary>
        /// <param name="freq">
        /// The frequency.
        /// </param>
        /// <param name="startTime">
        /// The start time.
        /// </param>
        /// <returns>
        /// The period resolve method
        /// </returns>
        private static Func<int, string> BuildPeriodResolver(string freq, DateTime startTime)
        {
            Func<int, string> getPeriod = null;
            switch (freq)
            {
                case "Q":
                    getPeriod = i =>
                        {
                            DateTime months = startTime.AddMonths(3 * i);
                            return DateUtil.FormatDate(months, TimeFormatEnumType.QuarterOfYear);
                        };
                    break;
                case "A":
                    getPeriod = i =>
                        {
                            DateTime months = startTime.AddMonths(12 * i);
                            return DateUtil.FormatDate(months, TimeFormatEnumType.Year);
                        };
                    break;
                case "M":
                    getPeriod = i =>
                        {
                            DateTime months = startTime.AddMonths(i + 1);
                            return DateUtil.FormatDate(months, TimeFormatEnumType.Month);
                        };
                    break;
                default:
                    Assert.Fail("Test bug. Check CL_FREQ codes");
                    break;
            }

            return getPeriod;
        }

        /// <summary>
        /// Creates and returns a codelist with the specified <paramref name="id"/>, <paramref name="name"/> and
        ///     <paramref name="codes"/>
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="codes">
        /// The codes.
        /// </param>
        /// <returns>
        /// The <see cref="ICodelistObject"/>.
        /// </returns>
        private static ICodelistObject CreateCodelist(string id, string name, params string[] codes)
        {
            var codelist = new CodelistMutableCore { Id = id, AgencyId = "TEST" };
            codelist.AddName("en", name);

            if (codes == null || codes.Length == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    string s = i.ToString(CultureInfo.InvariantCulture);
                    var code = new CodeMutableCore { Id = "CODE" + s };
                    code.AddName("en", "Description of " + s);
                    codelist.AddItem(code);
                }
            }
            else
            {
                foreach (string codeId in codes)
                {
                    var code = new CodeMutableCore { Id = codeId };
                    code.AddName("en", "Description of " + codeId);
                    codelist.AddItem(code);
                }
            }

            return codelist.ImmutableInstance;
        }

        /// <summary>
        /// Creates and returns a concept with the specified <paramref name="id"/>, <paramref name="name"/> and adds it to
        ///     <paramref name="parent"/>
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="IConceptObject"/>.
        /// </returns>
        private static IConceptObject CreateConcept(IConceptSchemeObject parent, string id, string name)
        {
            var concept = new ConceptMutableCore { Id = id };
            concept.AddName("en", name);

            //// parent.AddItem(concept);
            return new ConceptCore(parent, concept);
        }

        #endregion
    }
}