// -----------------------------------------------------------------------
// <copyright file="TestDimension.cs" company="Eurostat">
//   Date Created : 2014-07-10
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.SdmxSource.Sdmx.SdmxObjects
{
    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// Tests for DSD
    /// </summary>
    [TestFixture]
    public class TestDSD
    {
        /// <summary>
        /// Tests the freq with concept role.
        /// </summary>
        [Test]
        public void TestFreqWithConceptRole()
        {
            IDimensionMutableObject dimension = new DimensionMutableCore();
            dimension.Id = "FREQ";
            dimension.ConceptRole.Add(new StructureReferenceImpl("TEST_AGENCY", "TEST_CONCEPTS", "1.0", SdmxStructureEnumType.Concept, "FREQ"));
            dimension.ConceptRef = new StructureReferenceImpl("TEST_AGENCY", "TEST_CONCEPTS", "1.0", SdmxStructureEnumType.Concept, "FREQ");
            var immutable = BuildDataStructureObject(dimension);
            Assert.NotNull(immutable.FrequencyDimension);
        }

        /// <summary>
        /// Tests the freq with freq concept.
        /// </summary>
        [Test]
        public void TestFreqWithFreqConcept()
        {
            IDimensionMutableObject dimension = new DimensionMutableCore();
            dimension.ConceptRef = new StructureReferenceImpl("TEST_AGENCY", "TEST_CONCEPTS", "1.0", SdmxStructureEnumType.Concept, "FREQ");
            var immutable = BuildDataStructureObject(dimension);
            Assert.NotNull(immutable.FrequencyDimension); 
        }

        /// <summary>
        /// Tests the freq with freq identifier.
        /// </summary>
        [Test]
        public void TestFreqWithFreqId()
        {
            IDimensionMutableObject dimension = new DimensionMutableCore();
            dimension.Id = "FREQ";
            dimension.ConceptRef = new StructureReferenceImpl("TEST_AGENCY", "TEST_CONCEPTS", "1.0", SdmxStructureEnumType.Concept, "SOMETHING_ELSE");
            var immutable = BuildDataStructureObject(dimension);
            Assert.NotNull(immutable.FrequencyDimension);
        }

        /// <summary>
        /// Tests the coded time dimension.
        /// </summary>
        [Test]
        public void TestCodedTimeDimension()
        {
            IDimensionMutableObject dimension = new DimensionMutableCore();
            dimension.TimeDimension = true;
            dimension.Id = DimensionObject.TimeDimensionFixedId;
            dimension.ConceptRef = new StructureReferenceImpl("TEST_AGENCY", "TEST_CONCEPTS", "1.0", SdmxStructureEnumType.Concept, "TIME_PERIOD");
            dimension.Representation = new RepresentationMutableCore() {Representation = new StructureReferenceImpl("TEST_AGENCY", "CL_TIME_PERIOD", "1.0", SdmxStructureEnumType.CodeList) };
            var immutable = BuildDataStructureObject(dimension);
            var timeDimension = immutable.TimeDimension;
            Assert.NotNull(timeDimension);
            Assert.IsTrue(timeDimension.HasCodedRepresentation());
            var structureReference = dimension.Representation.Representation;
            Assert.AreEqual(timeDimension.Representation.Representation.AgencyId, structureReference.AgencyId);
            Assert.AreEqual(timeDimension.Representation.Representation.MaintainableId, structureReference.MaintainableId);
            Assert.AreEqual(timeDimension.Representation.Representation.Version, structureReference.Version);
        }

        /// <summary>
        /// Tests the not coded time dimension.
        /// </summary>
        [Test]
        public void TestUnCodeTimeDimension()
        {
            IDimensionMutableObject dimension = new DimensionMutableCore();
            dimension.TimeDimension = true;
            dimension.Id = DimensionObject.TimeDimensionFixedId;
            dimension.ConceptRef = new StructureReferenceImpl("TEST_AGENCY", "TEST_CONCEPTS", "1.0", SdmxStructureEnumType.Concept, "TIME_PERIOD");
            dimension.Representation = new RepresentationMutableCore() { TextFormat = new TextFormatMutableCore() { TextType = TextType.GetFromEnum(TextEnumType.TimePeriod) } };
            var immutable = BuildDataStructureObject(dimension);
            Assert.NotNull(immutable.TimeDimension);
            Assert.IsFalse(immutable.TimeDimension.HasCodedRepresentation());
            Assert.NotNull(immutable.TimeDimension.Representation);
            Assert.NotNull(immutable.TimeDimension.Representation.TextFormat);
        }

        /// <summary>
        /// Builds the data structure object.
        /// </summary>
        /// <param name="dimension">The dimension.</param>
        /// <returns>
        /// The <see cref="IDataStructureObject"/>
        /// </returns>
        private static IDataStructureObject BuildDataStructureObject(IDimensionMutableObject dimension)
        {
            IDataStructureMutableObject dsd = new DataStructureMutableCore() { Id = "TEST_DSD", AgencyId = "TEST", Version = "1.0" };
            dsd.AddName("en", "TEST_DSD");
            dsd.AddPrimaryMeasure(new StructureReferenceImpl("TEST_AGENCY", "TEST_CONCEPTS", "1.0", SdmxStructureEnumType.Concept, "OBS_VALUE"));
            dsd.AddDimension(dimension);

            var immutable = dsd.ImmutableInstance;
            return immutable;
        }
    }
}