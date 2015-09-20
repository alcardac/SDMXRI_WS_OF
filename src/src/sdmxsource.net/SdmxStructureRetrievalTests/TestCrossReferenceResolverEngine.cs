// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestCrossReferenceResolverEngineV20.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The test cross reference resolver engine v 20.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxStructureRetrievalTests
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Sdmx.StructureRetrieval.Engine;
    using Org.Sdmxsource.Sdmx.StructureRetrieval.Manager;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    /// The test cross reference resolver engine v 20.
    /// </summary>
    [TestFixture]
    public class TestCrossReferenceResolverEngine
    {
        #region Constants

        /// <summary>
        /// The file data flow.
        /// </summary>
        private const string FileDataFlow = "tests/v20/ESTAT+SSTSCONS_PROD_M+2.0.xml";

        /// <summary>
        /// The file maintainable object.
        /// </summary>
        private const string FileMaintainableObject = "tests/v20/ESTAT+DEMOGRAPHY+2.1.xml";

        /// <summary>
        /// The filekey bean.
        /// </summary>
        private const string FilekeyBean = "tests/v20/ESTAT+STS+2.0.xml";

        #endregion

        #region Fields

        /// <summary>
        /// The bean retrival manager.
        /// </summary>
        private Mock<ISdmxObjectRetrievalManager> beanRetrivalManager;

        /// <summary>
        /// The data structure object.
        /// </summary>
        private IDataStructureObject dataStructureObject;

        /// <summary>
        /// The maintainable object set.
        /// </summary>
        private ISet<IMaintainableObject> maintainableObjectSet;

        /// <summary>
        /// The parsing manager.
        /// </summary>
        private IStructureParsingManager parsingManager;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The init.
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            IStructureWorkspace structureWorkspace;
            IDataflowObject dataflowObject;

            parsingManager = new StructureParsingManager(SdmxSchemaEnumType.Null);
            beanRetrivalManager = new Mock<ISdmxObjectRetrievalManager>();

            using (var fileDataFlowReadableDataLocation = new FileReadableDataLocation(FileDataFlow))
            {
                structureWorkspace = parsingManager.ParseStructures(fileDataFlowReadableDataLocation);
                ISdmxObjects structureBeans = structureWorkspace.GetStructureObjects(false);
                dataflowObject = structureBeans.Dataflows.First();
                Assert.IsNotNull(dataflowObject);
            }

            using (var fileKeybeanReadableDataLocation = new FileReadableDataLocation(FilekeyBean))
            {
                structureWorkspace = parsingManager.ParseStructures(fileKeybeanReadableDataLocation);
                ISdmxObjects structureBeans = structureWorkspace.GetStructureObjects(false);
                dataStructureObject = structureBeans.DataStructures.First();
                Assert.IsNotNull(dataStructureObject);
            }

            using (var fileMaintainableObject = new FileReadableDataLocation(FileMaintainableObject))
            {
                structureWorkspace = parsingManager.ParseStructures(fileMaintainableObject);
                maintainableObjectSet = structureWorkspace.GetStructureObjects(true).GetAllMaintainables();
            }

            beanRetrivalManager.Setup(m => m.GetMaintainableObject<IDataflowObject>(It.IsAny<IMaintainableRefObject>())).Returns(dataflowObject);
            beanRetrivalManager.Setup(m => m.GetIdentifiableObject<IDataflowObject>(It.IsAny<IStructureReference>())).Returns(dataflowObject);
            beanRetrivalManager.Setup(m => m.GetMaintainableObject<IDataStructureObject>(It.IsAny<IMaintainableRefObject>())).Returns(dataStructureObject);
            beanRetrivalManager.Setup(m => m.GetIdentifiableObject<IDataStructureObject>(It.IsAny<IStructureReference>())).Returns(dataStructureObject);
        }

        /// <summary>
        /// The test cross reference resolver engine.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        [TestCase("tests/v20/ESTAT+STS+2.0.xml")]
        public void TestResolveReference(string file)
        {
            ICrossReferenceResolverEngine crossReferenceResolverEngine = new CrossReferenceResolverEngineCore();
            ISdmxObjects structureBeans;
            using (IReadableDataLocation fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IStructureWorkspace structureWorkspace = this.parsingManager.ParseStructures(fileReadableDataLocation);
                Assert.NotNull(structureWorkspace);
                structureBeans = structureWorkspace.GetStructureObjects(false);
            }

            var retrievalManager = new InMemoryRetrievalManager(structureBeans);
            foreach (var maintainableObject in structureBeans.DataStructures)
            {
                ISet<IIdentifiableObject> identifiableObjects = crossReferenceResolverEngine.ResolveReferences(maintainableObject, false, 10, retrievalManager);
                Assert.IsNotEmpty(identifiableObjects);
            }
        }

        [TestCase("tests/v20/ESTAT+STS+2.0.xml")]
        public void TestResolveCrossReference(string file)
        {

            ICrossReferenceResolverEngine crossReferenceResolverEngine = new CrossReferenceResolverEngineCore();
            ISdmxObjects structureBeans;
            using (IReadableDataLocation fileReadableDataLocation = new FileReadableDataLocation(file))
            {
                IStructureWorkspace structureWorkspace = this.parsingManager.ParseStructures(fileReadableDataLocation);
                Assert.NotNull(structureWorkspace);
                structureBeans = structureWorkspace.GetStructureObjects(false);
            }

            var retrievalManager = new InMemoryRetrievalManager(structureBeans);
            ICodelistObject immutableInstance = CreateSampleCodelist();
            var cross = new CrossReferenceImpl(immutableInstance, "urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure=TFFS:CRED_EXT_DEBT(1.0)");

            int i = 0;

            foreach (IMaintainableObject maintainableObject in this.maintainableObjectSet)
            {
                beanRetrivalManager.Setup(m => m.GetMaintainableObject(It.IsAny<IStructureReference>(), false, false)).Returns(maintainableObject);
                beanRetrivalManager.Setup(m => m.GetIdentifiableObject(It.IsAny<ICrossReference>())).Returns(maintainableObject);
                IIdentifiableObject ret = crossReferenceResolverEngine.ResolveCrossReference(cross, beanRetrivalManager.Object);
                Assert.IsNotNull(ret);
                Assert.AreEqual(maintainableObject.Urn, ret.Urn);
                i++;
            } 
        }

        [Test]
        public void TestGetMissingCrossReferences()
        {
            ICrossReferenceResolverEngine crossReferenceResolverEngine = new CrossReferenceResolverEngineCore();
            InMemoryRetrievalManager retrievalManager = new InMemoryRetrievalManager();
            ISdmxObjects objects = new SdmxObjectsImpl(dataStructureObject);
            IDictionary<IIdentifiableObject, ISet<ICrossReference>> missingCrossRef = crossReferenceResolverEngine.GetMissingCrossReferences(objects, 10, retrievalManager);
            Assert.IsNotEmpty(missingCrossRef);
            Assert.AreEqual(dataStructureObject.DimensionList.Dimensions[0].Urn, missingCrossRef.First().Key.Urn);
            Assert.IsTrue(missingCrossRef.Any(pair => pair.Key.Urn.Equals(this.dataStructureObject.PrimaryMeasure.Urn)));
            Assert.IsTrue(missingCrossRef.Any(pair => pair.Key.Urn.Equals(this.dataStructureObject.TimeDimension.Urn)));
        }

        /// <summary>
        /// Tests the get missing agencies.
        /// </summary>
        [Test]
        public void TestGetMissingAgencies()
        {
            ICodelistObject immutableInstance = CreateSampleCodelist();
            ICrossReferenceResolverEngine crossReferenceResolverEngine = new CrossReferenceResolverEngineCore();
            IDictionary<string, ISet<IMaintainableObject>> missingAgencies = crossReferenceResolverEngine.GetMissingAgencies(new SdmxObjectsImpl(immutableInstance), beanRetrivalManager.Object);
            Assert.IsNotEmpty(missingAgencies);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create sample codelist.
        /// </summary>
        /// <returns>
        /// The <see cref="ICodelistObject"/>.
        /// </returns>
        private static ICodelistObject CreateSampleCodelist()
        {
            ICodelistMutableObject codelist = new CodelistMutableCore();
            codelist.Id = "CL_3166A2";
            codelist.AgencyId = "ISO";
            codelist.AddName("en", "Test CL");
            ICodeMutableObject code = new CodeMutableCore();
            code.Id = "AR";
            code.AddName("en", "Code " + code.Id);
            codelist.AddItem(code);
            ICodelistObject immutableInstance = codelist.ImmutableInstance;
            return immutableInstance;
        }

        #endregion
    }
}