// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestStructureRetrieverAvailableData.cs" company="Eurostat">
//   Date Created : 2013-09-30
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The test structure retriever available data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace StructureRetriever.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Estat.Nsi.StructureRetriever;
    using Estat.Nsi.StructureRetriever.Factory;
    using Estat.Sdmxsource.Extension.Builder;
    using Estat.Sdmxsource.Extension.Constant;
    using Estat.Sri.CustomRequests.Constants;
    using Estat.Sri.CustomRequests.Model;
    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Extensions;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    ///     The test structure retriever available data.
    /// </summary>
    [TestFixture]
    public class TestStructureRetrieverAvailableData
    {
        #region Static Fields

        /// <summary>
        ///     The _cross reference set builder.
        /// </summary>
        private static readonly ICrossReferenceSetBuilder _crossReferenceSetBuilder = new CrossReferenceChildBuilder();

        /// <summary>
        ///     The _from mutable builder.
        /// </summary>
        private static readonly StructureReferenceFromMutableBuilder _fromMutableBuilder = new StructureReferenceFromMutableBuilder();

        /// <summary>
        ///     The _mutable structure search manager factory.
        /// </summary>
        private static readonly IStructureSearchManagerFactory<IMutableStructureSearchManager> _mutableStructureSearchManagerFactory = new MutableStructureSearchManagerFactory();

        /// <summary>
        /// The _advanced structure search manager factory.
        /// </summary>
        private static readonly IStructureSearchManagerFactory<IAdvancedMutableStructureSearchManager> _advancedStructureSearchManagerFactory = new AdvancedMutableStructureSearchManagerFactory();

        /// <summary>
        /// The _complex query builder.
        /// </summary>
        private static readonly StructureQuery2ComplexQueryBuilder _complexQueryBuilder = new StructureQuery2ComplexQueryBuilder();

        /// <summary>
        ///     The _random.
        /// </summary>
        private static readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Tests the emulate SDMX-RI Web Client calls (SDMX V2.1 REST).
        /// </summary>
        /// <param name="name">The name.</param>
        [TestCase("sqlserver")]
        public void TestEmulateNsiClientCallsRest(string name)
        {
            ConnectionStringSettings css = ConfigurationManager.ConnectionStrings[name];
            var mutableSearchManager = _mutableStructureSearchManagerFactory.GetStructureSearchManager(css, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));
            var mutableSearchManagerV20 = _mutableStructureSearchManagerFactory.GetStructureSearchManager(css, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo));
            Assert.IsNotNull(mutableSearchManager);

            // get dataflows & categorisations
            IStructureReference dataflowReference = new StructureReferenceImpl(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow));
            IStructureReference categorySchemeReference = new StructureReferenceImpl(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme));
            var dataflowAndDsdRef = new RESTStructureQueryCore(StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.Full), StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.Children), null, dataflowReference, true);
            IMutableObjects mutableObjects = mutableSearchManager.GetMaintainables(dataflowAndDsdRef);
            IMutableObjects mutableObjectsCat = mutableSearchManager.GetMaintainables(new RESTStructureQueryCore(StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.Full), StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.Parents), null, categorySchemeReference, false));

            Assert.IsNotNull(mutableObjects);
            Assert.IsNotEmpty(mutableObjects.Dataflows);
            Assert.IsTrue(mutableObjects.Dataflows.All(o => o.ExternalReference == null || !o.ExternalReference.IsTrue));
            Assert.IsNotEmpty(mutableObjects.DataStructures);
            Assert.IsNotEmpty(mutableObjectsCat.CategorySchemes);
            Assert.IsNotEmpty(mutableObjectsCat.Categorisations);

            // build lookup for SDMX v2.0 only DSD.
            var sdmxv20Only =
                mutableObjects.DataStructures.Where(o => o.Annotations.Any(a => a.FromAnnotation() == CustomAnnotationType.SDMXv20Only))
                    .ToDictionary(o => _fromMutableBuilder.Build(o).MaintainableReference);
            
            // for each dataflow get a dsd
            foreach (var dataflow in mutableObjects.Dataflows)
            {
                // request count
                RequestCount(dataflow, mutableSearchManagerV20);

                // get dsd
                IDataStructureMutableObject firstDsd;
                if (sdmxv20Only.TryGetValue(dataflow.DataStructureRef.MaintainableReference, out firstDsd))
                {
                    Func<IStructureReference, IMutableObjects> searchManager = reference => mutableSearchManagerV20.RetrieveStructures(new[] { reference }, false, false);
                    Func<IDataStructureMutableObject, IMutableObjects> conceptFunc = dsd =>
                        {
                            var structureReferences = _crossReferenceSetBuilder.Build(dsd).Select(reference => reference.TargetReference.IsMaintainable ? reference : new StructureReferenceImpl(reference.MaintainableReference, reference.MaintainableStructureEnumType)).ToArray();
                            return mutableSearchManagerV20.RetrieveStructures(structureReferences, false, false);
                        };

                    firstDsd = RequestDsd(dataflow, searchManager);

                    // get concept schems
                    RequestConceptScheme(firstDsd, conceptFunc);
                }
                else
                {
                    firstDsd = mutableObjects.DataStructures.First(o => _fromMutableBuilder.Build(o).Equals(dataflow.DataStructureRef));
                }

                // get codelists
                var criteria = RequestPartialCodelist(dataflow, firstDsd, mutableSearchManagerV20);
                RequestCount(dataflow, mutableSearchManagerV20, criteria);
            }
        }

        /// <summary>
        /// Tests the emulate SDMX-RI Web Client calls (SDMX V2.1 REST).
        /// </summary>
        /// <param name="name">The name.</param>
        [TestCase("sqlserver", "DF_WDI")]
        public void TestEmulateNsiClientCallsRestSpecific(string name, string dataflowId)
        {
            ConnectionStringSettings css = ConfigurationManager.ConnectionStrings[name];
            var mutableSearchManager = _mutableStructureSearchManagerFactory.GetStructureSearchManager(css, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));
            var mutableSearchManagerV20 = _mutableStructureSearchManagerFactory.GetStructureSearchManager(css, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo));
            Assert.IsNotNull(mutableSearchManager);

            // get dataflows & categorisations
            IStructureReference dataflowReference = new StructureReferenceImpl(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow));
            IStructureReference categorySchemeReference = new StructureReferenceImpl(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme));
            var dataflowAndDsdRef = new RESTStructureQueryCore(
                StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.Full),
                StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.Children),
                null,
                dataflowReference,
                true);
            IMutableObjects mutableObjects = mutableSearchManager.GetMaintainables(dataflowAndDsdRef);
            IMutableObjects mutableObjectsCat =
                mutableSearchManager.GetMaintainables(
                    new RESTStructureQueryCore(
                        StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.Full),
                        StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.Parents),
                        null,
                        categorySchemeReference,
                        false));

            Assert.IsNotNull(mutableObjects);
            Assert.IsNotEmpty(mutableObjects.Dataflows);
            Assert.IsTrue(mutableObjects.Dataflows.All(o => o.ExternalReference == null || !o.ExternalReference.IsTrue));
            Assert.IsNotEmpty(mutableObjects.DataStructures);
            Assert.IsNotEmpty(mutableObjectsCat.CategorySchemes);
            Assert.IsNotEmpty(mutableObjectsCat.Categorisations);

            // build lookup for SDMX v2.0 only DSD.
            var sdmxv20Only =
                mutableObjects.DataStructures.Where(o => o.Annotations.Any(a => a.FromAnnotation() == CustomAnnotationType.SDMXv20Only))
                    .ToDictionary(o => _fromMutableBuilder.Build(o).MaintainableReference);

            // for each dataflow get a dsd
            var dataflow = mutableObjects.Dataflows.First(o => o.Id.Equals(dataflowId));
            // request count
            RequestCount(dataflow, mutableSearchManagerV20);

            // get dsd
            IDataStructureMutableObject firstDsd;
            if (sdmxv20Only.TryGetValue(dataflow.DataStructureRef.MaintainableReference, out firstDsd))
            {
                Func<IStructureReference, IMutableObjects> searchManager = reference => mutableSearchManagerV20.RetrieveStructures(new[] { reference }, false, false);
                Func<IDataStructureMutableObject, IMutableObjects> conceptFunc = dsd =>
                    {
                        var structureReferences =
                            _crossReferenceSetBuilder.Build(dsd)
                                .Select(
                                    reference =>
                                    reference.TargetReference.IsMaintainable ? reference : new StructureReferenceImpl(reference.MaintainableReference, reference.MaintainableStructureEnumType))
                                .ToArray();
                        return mutableSearchManagerV20.RetrieveStructures(structureReferences, false, false);
                    };

                firstDsd = RequestDsd(dataflow, searchManager);

                // get concept schems
                RequestConceptScheme(firstDsd, conceptFunc);
            }
            else
            {
                firstDsd = mutableObjects.DataStructures.First(o => _fromMutableBuilder.Build(o).Equals(dataflow.DataStructureRef));
            }

            // get codelists
            var criteria = RequestPartialCodelist(dataflow, firstDsd, mutableSearchManagerV20);
            RequestCount(dataflow, mutableSearchManagerV20, criteria);
        }

        /// <summary>
        /// Tests the emulate SDMX-RI Web Client calls (SDMX V2.1 SOAP).
        /// </summary>
        /// <param name="name">The name.</param>
        [TestCase("sqlserver")]
        public void TestEmulateNsiClientCallsV21Soap(string name)
        {
            ConnectionStringSettings css = ConfigurationManager.ConnectionStrings[name];
            var mutableSearchManager = _advancedStructureSearchManagerFactory.GetStructureSearchManager(css, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwoPointOne));
            var mutableSearchManagerV20 = _mutableStructureSearchManagerFactory.GetStructureSearchManager(css, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo));
            Assert.IsNotNull(mutableSearchManager);

            // get dataflows & categorisations
            IStructureReference dataflowReference = new StructureReferenceImpl(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow));
            IStructureReference categorySchemeReference = new StructureReferenceImpl(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme));

            var complexStructureQuery = _complexQueryBuilder.Build(new RESTStructureQueryCore(StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.Full), StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.Children), null, dataflowReference, true));
            IMutableObjects mutableObjects = mutableSearchManager.GetMaintainables(complexStructureQuery);
            IMutableObjects mutableObjectsCat = mutableSearchManager.GetMaintainables(_complexQueryBuilder.Build(new RESTStructureQueryCore(StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.Full), StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.Parents), null, categorySchemeReference, false)));

            Assert.IsNotNull(mutableObjects);
            Assert.IsNotEmpty(mutableObjects.Dataflows);
            Assert.IsNotEmpty(mutableObjects.DataStructures);
            Assert.IsNotEmpty(mutableObjectsCat.CategorySchemes);
            Assert.IsNotEmpty(mutableObjectsCat.Categorisations);

            // build lookup for SDMX v2.0 only DSD.
            // build lookup for SDMX v2.0 only DSD.
            var sdmxv20Only =
                mutableObjects.DataStructures.Where(o => o.Annotations.Any(a => a.FromAnnotation() == CustomAnnotationType.SDMXv20Only))
                    .ToDictionary(o => _fromMutableBuilder.Build(o).MaintainableReference);
            
            // for each dataflow get a dsd
            foreach (var dataflow in mutableObjects.Dataflows)
            {
                // request count
                RequestCount(dataflow, mutableSearchManagerV20);

                IDataStructureMutableObject firstDsd;
                if (sdmxv20Only.TryGetValue(dataflow.DataStructureRef.MaintainableReference, out firstDsd))
                {
                    Func<IStructureReference, IMutableObjects> searchManager = reference => mutableSearchManagerV20.RetrieveStructures(new[] { reference }, false, false);
                    Func<IDataStructureMutableObject, IMutableObjects> conceptFunc = dsd =>
                        {
                            var structureReferences = _crossReferenceSetBuilder.Build(dsd).Select(reference => reference.TargetReference.IsMaintainable ? reference : new StructureReferenceImpl(reference.MaintainableReference, reference.MaintainableStructureEnumType)).ToArray();
                            return mutableSearchManagerV20.RetrieveStructures(structureReferences, false, false);
                        };

                    firstDsd = RequestDsd(dataflow, searchManager);

                    // get concept schems
                    RequestConceptScheme(firstDsd, conceptFunc);
                }
                else
                {
                    firstDsd = mutableObjects.DataStructures.First(o => _fromMutableBuilder.Build(o).Equals(dataflow.DataStructureRef));
                }

                // get codelists
                var criteria = RequestPartialCodelist(dataflow, firstDsd, mutableSearchManagerV20);
                RequestCount(dataflow, mutableSearchManagerV20, criteria);
            }
        }

        /// <summary>
        /// Tests the emulate SDMX-RI Web Client calls (SDMX V2.0).
        /// </summary>
        /// <param name="name">The name.</param>
        [TestCase("sqlserver")]
        public void TestEmulateNsiClientCallsV20(string name)
        {
            ConnectionStringSettings css = ConfigurationManager.ConnectionStrings[name];
            var mutableSearchManager = _mutableStructureSearchManagerFactory.GetStructureSearchManager(css, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo));
            Assert.IsNotNull(mutableSearchManager);

            // get dataflows & categorisations
            IStructureReference dataflowReference = new StructureReferenceImpl(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow));
            IStructureReference categorySchemeReference = new StructureReferenceImpl(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryScheme));
            IMutableObjects mutableObjects = mutableSearchManager.RetrieveStructures(new[] { dataflowReference, categorySchemeReference }, false, false);

            Assert.IsNotNull(mutableObjects);
            Assert.IsNotEmpty(mutableObjects.Dataflows);
            Assert.IsNotEmpty(mutableObjects.CategorySchemes);
            Assert.IsNotEmpty(mutableObjects.Categorisations);

            // for each dataflow get a dsd
            foreach (var dataflow in mutableObjects.Dataflows)
            {
                // request count
                RequestCount(dataflow, mutableSearchManager);

                // get dsd
                Func<IStructureReference, IMutableObjects> searchManager = reference => mutableSearchManager.RetrieveStructures(new[] { reference }, false, false);
                Func<IDataStructureMutableObject, IMutableObjects> conceptFunc = dsd =>
                {
                    var structureReferences = _crossReferenceSetBuilder.Build(dsd).Select(reference => reference.TargetReference.IsMaintainable ? reference : new StructureReferenceImpl(reference.MaintainableReference, reference.MaintainableStructureEnumType)).ToArray();
                    return mutableSearchManager.RetrieveStructures(structureReferences, false, false);
                };

                IDataStructureMutableObject firstDsd = RequestDsd(dataflow, searchManager);

                // get concept schems
                RequestConceptScheme(firstDsd, conceptFunc);

                // get codelists
                var criteria = RequestPartialCodelist(dataflow, firstDsd, mutableSearchManager);
                RequestCount(dataflow, mutableSearchManager, criteria);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The build constrainable structure reference.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="previousMembers">
        /// The previous members.
        /// </param>
        /// <param name="currentDataflowReference">
        /// The current dataflow reference.
        /// </param>
        /// <returns>
        /// The <see cref="IConstrainableStructureReference"/>.
        /// </returns>
        private static IConstrainableStructureReference BuildConstrainableStructureReference(
            string id, 
            string agencyId, 
            string version, 
            string name, 
            IEnumerable<IKeyValuesMutable> previousMembers, 
            IStructureReference currentDataflowReference)
        {
            IContentConstraintMutableObject mutableConstraint = new ContentConstraintMutableCore { Id = id, AgencyId = agencyId, Version = version };
            mutableConstraint.AddName("en", name);

            var cubeRegion = new CubeRegionMutableCore();
            mutableConstraint.IncludedCubeRegion = cubeRegion;
            IKeyValuesMutable requestedDimension = new KeyValuesMutableImpl();
            requestedDimension.Id = id;
            requestedDimension.AddValue(SpecialValues.DummyMemberValue);
            cubeRegion.AddKeyValue(requestedDimension);
            if (previousMembers != null)
            {
                cubeRegion.KeyValues.AddAll(previousMembers);
            }

            IContentConstraintObject constraint = new ContentConstraintObjectCore(mutableConstraint);
            IConstrainableStructureReference specialRequest = new ConstrainableStructureReference(currentDataflowReference, constraint);
            return specialRequest;
        }

        /// <summary>
        /// Requests the concept scheme.
        /// </summary>
        /// <param name="firstDsd">The first DSD.</param>
        /// <param name="conceptFunc">The concept function.</param>
        private static void RequestConceptScheme(IDataStructureMutableObject firstDsd, Func<IDataStructureMutableObject, IMutableObjects> conceptFunc)
        {
            var conceptSchemeContainer = conceptFunc(firstDsd);
            Assert.IsNotNull(conceptSchemeContainer);
            Assert.IsNotEmpty(conceptSchemeContainer.ConceptSchemes);
        }

        /// <summary>
        /// Requests the count.
        /// </summary>
        /// <param name="dataflow">The dataflow.</param>
        /// <param name="structureSearch">The structure search.</param>
        /// <param name="previousMembers">The previous members.</param>
        private static void RequestCount(IDataflowMutableObject dataflow, IMutableStructureSearchManager structureSearch, IEnumerable<IKeyValuesMutable> previousMembers = null)
        {
            var currentDataflowReference = _fromMutableBuilder.Build(dataflow);

            var specialRequest = BuildConstrainableStructureReference("PAOK", "SR", "1.1.1", "PAOK OLE", previousMembers, currentDataflowReference);
            RequestCount(specialRequest, structureSearch);
        }

        /// <summary>
        /// Requests the count.
        /// </summary>
        /// <param name="dataflowRef">The dataflow preference.</param>
        /// <param name="structureSearch">The structure search.</param>
        private static void RequestCount(IStructureReference dataflowRef, IMutableStructureSearchManager structureSearch)
        {
            var countRef = new StructureReferenceImpl(CustomCodelistConstants.Agency, CustomCodelistConstants.CountCodeList, CustomCodelistConstants.Version, SdmxStructureEnumType.CodeList);
            var queries = new[] { dataflowRef, countRef };
            var countResponse = structureSearch.RetrieveStructures(queries, false, false);
            Assert.IsNotNull(countResponse);
            Assert.IsNotEmpty(countResponse.Codelists);
            var countCodelist = countResponse.Codelists.First();
            Assert.AreEqual(CustomCodelistConstants.CountCodeList, countCodelist.Id);
            Assert.AreEqual(1, countCodelist.Items.Count);
            int count;
            Assert.IsTrue(int.TryParse(countCodelist.Items.First().Id, NumberStyles.Integer, CultureInfo.InvariantCulture, out count), "Dataflow {0}", dataflowRef);
            Assert.IsTrue(count > 0, "Dataflow {0}", ExtractInfo(dataflowRef));
        }

        /// <summary>
        /// Extracts the information.
        /// </summary>
        /// <param name="structureReference">The structure reference.</param>
        /// <returns>The info from <paramref name="structureReference"/></returns>
        private static string ExtractInfo(IStructureReference structureReference)
        {
            var info = new StringBuilder(structureReference.ToString());
            var constrainableStructureReference = structureReference as IConstrainableStructureReference;
            if (constrainableStructureReference != null)
            {
                foreach (var keyValuese in constrainableStructureReference.ConstraintObject.IncludedCubeRegion.KeyValues)
                {
                    info.AppendFormat("Key: {0}, Values : [{1}]\n", keyValuese.Id, string.Join(",", keyValuese.Values));
                }
            }

            return info.ToString();
        }

        /// <summary>
        /// Requests the DSD.
        /// </summary>
        /// <param name="dataflow">The dataflow.</param>
        /// <param name="mutableSearchManager">The mutable search manager.</param>
        /// <returns>The <see cref="IDataStructureMutableObject"/></returns>
        private static IDataStructureMutableObject RequestDsd(IDataflowMutableObject dataflow, Func<IStructureReference, IMutableObjects> mutableSearchManager)
        {
            IStructureReference dsdReference = dataflow.DataStructureRef;
            IMutableObjects dsdMutableObjects = mutableSearchManager(dsdReference);
            Assert.IsNotNull(dsdMutableObjects);
            Assert.IsNotEmpty(dsdMutableObjects.DataStructures);

            var firstDsd = dsdMutableObjects.DataStructures.First();
            Assert.IsTrue(firstDsd.ExternalReference == null || !firstDsd.ExternalReference.IsTrue, "Failed for dataflow: '{0}', DSD : '{1}'", _fromMutableBuilder.Build(dataflow), dsdReference);
            return firstDsd;
        }

        /// <summary>
        /// Requests the partial codelist.
        /// </summary>
        /// <param name="dataflow">The dataflow.</param>
        /// <param name="firstDsd">The first DSD.</param>
        /// <param name="mutableSearchManager">The mutable search manager.</param>
        /// <returns>The criteria used.</returns>
        private static IEnumerable<IKeyValuesMutable> RequestPartialCodelist(IDataflowMutableObject dataflow, IDataStructureMutableObject firstDsd, IMutableStructureSearchManager mutableSearchManager)
        {
            var currentDataflowReference = _fromMutableBuilder.Build(dataflow);

            var previousMembers = new List<IKeyValuesMutable>();
            foreach (var dimension in firstDsd.Dimensions)
            {
                var representation = dimension.GetEnumeratedRepresentation(firstDsd);
                if (representation != null)
                {
                    IStructureReference codelistRef = new StructureReferenceImpl(representation.MaintainableReference, SdmxStructureEnumType.CodeList);

                    var id = dimension.Id;
                    var agencyId = firstDsd.AgencyId;
                    var version = firstDsd.Version;
                    var name = dimension.ConceptRef.ChildReference.Id;
                    var specialRequest = BuildConstrainableStructureReference(id, agencyId, version, name, previousMembers, currentDataflowReference);
                    var queries = new[] { specialRequest, codelistRef };
                    var objects = mutableSearchManager.RetrieveStructures(queries, false, false);
                    var message = string.Format("DataflowRef : {0}, CodelistRef : {1}", specialRequest, codelistRef);
                    Assert.IsNotNull(objects, message);
                    Assert.IsNotEmpty(objects.Codelists, message);
                    Assert.AreEqual(1, objects.Codelists.Count, message);

                    var codeIds = objects.Codelists.First().Items.TakeWhile(o => _random.Next() % 2 == 0).Select(o => o.Id).ToArray();

                    if (codeIds.Length > 0)
                    {
                        var criteria = new KeyValuesMutableImpl { Id = id };
                        criteria.KeyValues.AddAll(codeIds);
                        previousMembers.Add(criteria);
                    }
                }
                else if (dimension.TimeDimension)
                {
                    var id = dimension.Id;
                    var agencyId = firstDsd.AgencyId;
                    var version = firstDsd.Version;
                    var name = dimension.ConceptRef.ChildReference.Id;
                    IStructureReference specialRequest = BuildConstrainableStructureReference(id, agencyId, version, name, previousMembers, currentDataflowReference);
                    IStructureReference timeCodelistReference = new StructureReferenceImpl(
                        CustomCodelistConstants.Agency, 
                        CustomCodelistConstants.TimePeriodCodeList, 
                        CustomCodelistConstants.Version, 
                        SdmxStructureEnumType.CodeList);
                    var queries = new[] { specialRequest, timeCodelistReference };
                    var objects = mutableSearchManager.RetrieveStructures(queries, false, false);
                    var message = string.Format("DataflowRef : {0}, CodelistRef : {1}", specialRequest, timeCodelistReference);
                    Assert.IsNotNull(objects);
                    Assert.IsNotEmpty(objects.Codelists, message);
                    Assert.AreEqual(1, objects.Codelists.Count, message);
                    var timeCodelist = objects.Codelists.First();
                    Assert.GreaterOrEqual(timeCodelist.Items.Count, 1, message);
                    Assert.LessOrEqual(timeCodelist.Items.Count, 2, message);
                    foreach (var item in timeCodelist.Items)
                    {
                        DateTime date;
                        Assert.IsTrue(DateTime.TryParseExact(item.Id, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out date), "Invalid date : '{0}' for {1}", item.Id, specialRequest);
                    }
                    
                    if (timeCodelist.Items.Count > 0)
                    {
                        var criteria = new KeyValuesMutableImpl { Id = id };
                        criteria.KeyValues.AddAll(timeCodelist.Items.Select(o => o.Id));
                        previousMembers.Add(criteria);
                    }
                }
            }

            return previousMembers;
        }

        #endregion
    }
}