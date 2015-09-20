// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDataWriters.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Test unit for SDMX Data Writers
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxEdiDataWriterTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Estat.Sri.SdmxEdiDataWriter.Engine;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Util.Date;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    ///     Test unit for SDMX Data Writers
    /// </summary>
    [TestFixture]
    public class TestDataWriters
    {
        #region Public Methods and Operators
        /// <summary>
        /// The test edi data writer.
        /// </summary>
        /// <param name="useTimeRangeTechnique">
        /// The use time range technique.
        /// </param>
        [TestCase(true)]
        [TestCase(false)]
        public void TestEdiDataWriter(bool useTimeRangeTechnique)
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
            IConceptObject obsStatusConcept = CreateConcept(parent, "OBS_STATUS", "Name of  observation value");

            ICodelistObject freqCl = CreateCodelist("CL_FREQ", "Freq codelist", "Q", "A", "M");
            ICodelistObject adjCl = CreateCodelist("CL_ADJUSTMENT", "Adjustment codelist", "N", "S", "W");
            ICodelistObject actCl = CreateCodelist("CL_ACTIVITY", "Activity codelist", "A", "B", "C");
            ICodelistObject deciCl = CreateCodelist("CL_DECIMALS", "DECIMALS codelist", "1", "2", "0");
            ICodelistObject obsStatusCl = CreateCodelist("CL_OBS_STATUS", "Obs status codelist", "A", "M", "L");

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

            var obsStatusMutable = new AttributeMutableCore
                                       {
                                           ConceptRef = obsStatusConcept.AsReference, 
                                           Representation = new RepresentationMutableCore { Representation = obsStatusCl.AsReference }, 
                                           AttachmentLevel = AttributeAttachmentLevel.Observation, 
                                           AssignmentStatus = AttributeAssignmentStatus.Mandatory.ToString()
                                       };
            mutable.AddAttribute(obsStatusMutable);

            mutable.PrimaryMeasure = new PrimaryMeasureMutableCore { ConceptRef = obsConcept.AsReference };
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
            
            var sw = new Stopwatch();
            sw.Start();
            var series = from f in freqCl.Items from ad in adjCl.Items from ac in actCl.Items select new { f, ad, ac };

            sw.Stop();
            Trace.WriteLine(sw.Elapsed);
            sw.Reset();

            var startTime = new DateTime(2005, 1, 1);
            sw.Start();
            using (StreamWriter stream = File.CreateText("gesmes-ts-data-writer-tr-" + useTimeRangeTechnique + ".ges"))
            {
                IDataWriterEngine dataWriter = new GesmesTimeSeriesWriter(stream, useTimeRangeTechnique);
                dataWriter.WriteHeader(header);

                dataWriter.StartDataset(null, dataStructureObject, null);
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
                        dataWriter.WriteAttributeValue(obsStatusConcept.Id, "A");
                    }
                }

                dataWriter.Close();

                sw.Stop();
                Trace.WriteLine(sw.Elapsed);
            }
        }

        #endregion

        #region Methods

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