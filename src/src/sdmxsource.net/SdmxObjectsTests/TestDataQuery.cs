// -----------------------------------------------------------------------
// <copyright file="TestDataQuery.cs" company="Eurostat">
//   Date Created : 2014-07-10
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace SdmxObjectsTests
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    [TestFixture]
    public class TestDataQuery
    {
        [Test]
        public void TestDimensionAtObservation()
        {
            IDataStructureMutableObject dsd = new DataStructureMutableCore() { Id = "TEST", AgencyId = "TEST_AGENCY", Version = "1.0" };
            dsd.AddName("en", "Test name");
            IDimensionMutableObject dimension = new DimensionMutableCore();
            dimension.ConceptRef = new StructureReferenceImpl("TEST_AGENCY", "TEST_CONCEPTS", "1.0", SdmxStructureEnumType.Concept, "TEST_DIM");
            dimension.Representation = new RepresentationMutableCore { Representation = new StructureReferenceImpl("TEST_AGENCY", "CL_TEST", "2.0", SdmxStructureEnumType.CodeList) };

            dsd.AddDimension(dimension);
            dsd.AddPrimaryMeasure(new StructureReferenceImpl("TEST_AGENCY", "TEST_CONCEPTS", "1.0", SdmxStructureEnumType.Concept, "OBS_VALUE"));

            var immutableDsd = dsd.ImmutableInstance;
            var dataflowMutable = new DataflowMutableCore { Id = "TEST_DF", AgencyId = "TEST_AGENCY", Version = "1.2" };
            dataflowMutable.AddName("en", "Test");
            dataflowMutable.DataStructureRef = immutableDsd.AsReference;
            var dataflow = dataflowMutable.ImmutableInstance;
           
           IDataQuery query = new DataQueryImpl(immutableDsd, null, DataQueryDetail.GetFromEnum(DataQueryDetailEnumType.Full),null,null, null, dataflow, "AllDimensions", new HashSet<IDataQuerySelection>(), null, null);
           Assert.AreEqual("AllDimensions", query.DimensionAtObservation);
        }
    }
}