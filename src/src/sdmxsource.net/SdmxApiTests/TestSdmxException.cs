// -----------------------------------------------------------------------
// <copyright file="TestSdmxException.cs" company="Eurostat">
//   Date Created : 2014-04-25
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace SdmxApiTests
{

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Util.ResourceBundle;

    /// <summary>
    /// Test unit for <see cref="SdmxException"/>
    /// </summary>
    [TestFixture]
    public class TestSdmxException
    {

        /// <summary>
        /// Test unit for <see cref="SdmxException.Message"/> 
        /// </summary>
        [Test]
        public void TestMessage()
        {
            var exception = new SdmxNotImplementedException(ExceptionCode.Unsupported, "Any");
            Assert.AreEqual("Not Implemented - 405", exception.Message);
        }

        /// <summary>
        /// Test unit for <see cref="SdmxException.Message"/> 
        /// </summary>
        [Test]
        public void TestMessageNoArgs()
        {
            var exception = new SdmxNotImplementedException();
            Assert.AreEqual("Not Implemented", exception.Message);
        }

        /// <summary>
        /// Test unit for <see cref="SdmxException.Message"/> 
        /// </summary>
        [Test]
        public void TestMessageResolver()
        {
            var exception = new SdmxNotImplementedException(ExceptionCode.Unsupported, "Any");
            SdmxException.SetMessageResolver(new MessageDecoder());
            Assert.AreEqual("Not Implemented - Unsupported Any", exception.Message);
        }

        /// <summary>
        /// Test unit for <see cref="SdmxException.Message"/> 
        /// </summary>
        [Test]
        public void TestMessageResolver1Arg()
        {
            var exception = new SdmxNotImplementedException("Any");
            SdmxException.SetMessageResolver(new MessageDecoder());
            Assert.AreEqual("Any", exception.Message);
        }

        /// <summary>
        /// Test unit for <see cref="SdmxException.Message"/> 
        /// </summary>
        [Test]
        public void TestMessageResolverNoArgs()
        {
            var exception = new SdmxNotImplementedException();
            SdmxException.SetMessageResolver(new MessageDecoder());
            Assert.AreEqual("Not Implemented", exception.Message);
        }
        /// <summary>
        /// Test unit for <see cref="SdmxException.Message"/> 
        /// </summary>
        [Test]
        public void TestMessageResolverWrongArgs()
        {
            var exception = new SdmxNotImplementedException(ExceptionCode.ReferenceErrorUnexpectedResultsCount, "Any");
            SdmxException.SetMessageResolver(new MessageDecoder());
            Assert.AreEqual("Not Implemented - 607 - Unexpected number of results for {0} with arguments {1} expected {2} got {3} ", exception.Message);
        }
    }
}