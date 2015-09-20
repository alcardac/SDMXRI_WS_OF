// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureResourceServiceTest.cs" company="Eurostat">
//   Date Created : 2013-10-16
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Test unit for <see cref="StructureResource" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TestRest
{
    using NUnit.Framework;

    /// <summary>
    ///     Test unit for REST structure.
    /// </summary>
    [TestFixture]
    public class StructureResourceServiceTest : ResourceServiceTestBase
    {
        #region Public Methods and Operators

        /// <summary>
        /// The test edi structure.
        /// </summary>
        [Test]
        public void TestEDIStructure()
        {
            RestClientNotAcceptable("codelist/ESTAT/CL_AREA/1.0", "application/vnd.sdmx.edistructure");
        }

        /// <summary>
        ///     Test the getStructure service method by defining as MIME Type the <c>vnd.sdmx.structure+xml</c>
        /// </summary>
        [Test]
        public void TestStructure()
        {
            RestClientOk("codelist/ECB/CL_FREQ/1.0", "application/vnd.sdmx.structure+xml");
        }

        /// <summary>
        ///     Test the getStructure service method by defining detail parameter, that can take "allstubs", "referencestubs",
        ///     "full"
        /// </summary>
        [Test]
        public void TestStructureDetailV20()
        {
            RestClientOk("codelist/ECB/CL_FREQ/1.0?detail=allstubs", "application/vnd.sdmx.structure+xml;version=2.0");
        }

        /// <summary>
        ///     Test the getStructure service method by defining invalid detail parameter
        ///     "children", "descendants", "all"
        /// </summary>
        [Test]
        public void TestStructureInvalidDetailV20()
        {
            RestClientBadRequest("codelist/ECB/CL_FREQ/1.0?detail=dummy", "application/vnd.sdmx.structure+xml;version=2.0");
        }

        /// <summary>
        ///     Test the getStructure service method by defining by defining invalid reference parameter
        /// </summary>
        [Test]
        public void TestStructureInvalidReferenceV20()
        {
            RestClientBadRequest("codelist/ECB/CL_FREQ/1.0?references=dummy", "application/vnd.sdmx.structure+xml;version=2.0");
        }

        /// <summary>
        /// The test structure reference v 20.
        /// </summary>
        [Test]
        public void TestStructureReferenceV20()
        {
            RestClientOk("codelist/ECB/CL_FREQ/1.0?references=none", "application/vnd.sdmx.structure+xml;version=2.0");
        }

        /// <summary>
        ///     Test the getStructure service method by defining as MIME Type the <c>vnd.sdmx.structure+xml</c> and version = 1.0
        /// </summary>
        [Test]
        public void TestStructureV10()
        {
            RestClientNotAcceptable("codelist/ECB/CL_FREQ/1.0", "application/vnd.sdmx.structure+xml;version=1.0");
        }

        /// <summary>
        ///     Test the getStructure service method by defining as MIME Type the <c>vnd.sdmx.structure+xml</c> and version = 2.0
        /// </summary>
        [Test]
        public void TestStructureV20()
        {
            RestClientOk("codelist/ECB/CL_FREQ/1.0", "application/vnd.sdmx.structure+xml;version=2.0");
        }

        /// <summary>
        ///     Test the getStructure service method by defining as MIME Type the <c>vnd.sdmx.structure+xml</c> and version = 2.1
        /// </summary>
        [Test]
        public void TestStructureV21()
        {
            RestClientOk("codelist/ECB/CL_FREQ/1.0", "application/vnd.sdmx.structure+xml;version=2.1");
        }

        /// <summary>
        ///     Test the getStructure service method by defining as MIME Type the <c>vnd.sdmx.structure+xml</c> without version
        /// </summary>
        [Test]
        public void TestStructureV50()
        {
            RestClientNotAcceptable("codelist/ECB/CL_FREQ/1.0", "application/vnd.sdmx.structure+xml;version=5.0");
        }

        /// <summary>
        ///     Test the getStructure service method by defining as MIME Type the xml (default)
        /// </summary>
        [Test]
        public void TestXMLStructure()
        {
            RestClientOk("codelist/ECB/CL_FREQ/1.0", "application/xml");
        }

        /// <summary>
        ///     Test the getStructure service method by defining as MIME Type the xml (default)
        /// </summary>
        [Test]
        public void TestXMLStructureV10()
        {
            RestClientOk("codelist/ECB/CL_FREQ/1.0", "application/xml;version=1.0");
        }

        /// <summary>
        ///     Test the getStructure service method by defining as MIME Type the xml (default)
        /// </summary>
        [Test]
        public void TestXMLStructureV20()
        {
            RestClientOk("codelist/ECB/CL_FREQ/1.0", "application/xml;version=2.0");
        }

        /// <summary>
        ///     Test the getStructure service method by defining as MIME Type the xml (default)
        /// </summary>
        [Test]
        public void TestXMLStructureV21()
        {
            RestClientOk("codelist/ECB/CL_FREQ/1.0", "application/xml;version=2.1");
        }

        /// <summary>
        ///     Test the getStructure service method by defining as MIME Type the xml (default)
        /// </summary>
        [Test]
        public void TestXMLStructureV50()
        {
            RestClientOk("codelist/ECB/CL_FREQ/1.0", "application/xml;version=5.0");
        }

        #endregion
    }
}