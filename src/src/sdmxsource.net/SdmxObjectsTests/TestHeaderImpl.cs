// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestHeaderImpl.cs" company="Eurostat">
//   Date Created : 2014-07-01
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Test unit for <see cref="HeaderImpl" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SdmxObjectsTests
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     Test unit for <see cref="HeaderImpl" />
    /// </summary>
    [TestFixture]
    public class TestHeaderImpl
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Test unit for <see cref="HeaderImpl" />
        /// </summary>
        [Test]
        public void TestBase()
        {
            var receivers = BuildReceivers();
            var sender = BuildSender();
            DateTime prepared = DateTime.UtcNow;
            var reportingBegin = new DateTime(2000, 1, 1);
            var reportingEnd = new DateTime(2010, 12, 31);
            bool isTest = true;
            IHeader header = new HeaderImpl("TEST", prepared, reportingBegin, reportingEnd, receivers, sender, isTest);

            // Add DSD reference
            header.AddStructure(new DatasetStructureReferenceCore(new StructureReferenceImpl("DSD_AGENCY", "DSD_ID", "1.0", SdmxStructureEnumType.Dsd)));
            Assert.AreEqual("TEST", header.Id);
            Assert.AreEqual("TestSender", header.Sender.Id);
            Assert.AreEqual(1, header.Receiver.Count);
        }

        /// <summary>
        ///     Test unit for <see cref="HeaderImpl" />
        /// </summary>
        [Test]
        public void TestMinimum()
        {
            IHeader header = new HeaderImpl("TEST", "ZZ");
            Assert.AreEqual("TEST", header.Id);
            Assert.AreEqual("ZZ", header.Sender.Id);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Builds the receivers.
        /// </summary>
        /// <returns>The senders </returns>
        private static IList<IParty> BuildReceivers()
        {
            var receiverNames = new ITextTypeWrapper[] { new TextTypeWrapperImpl("en", "receiver name", null) };
            var mutableContactReceiver = new ContactMutableObjectCore();
            mutableContactReceiver.AddEmail("smith@example.com");
            IList<IContact> receiverContacts = new IContact[] { new ContactCore(mutableContactReceiver) };
            string timeZone = "+02:00";
            var receiver = new PartyCore(receiverNames, "RC0", receiverContacts, timeZone);
            return new IParty[] { receiver };
        }

        /// <summary>
        /// Builds the sender.
        /// </summary>
        /// <returns>
        /// The <see cref="PartyCore"/>.
        /// </returns>
        private static PartyCore BuildSender()
        {
            var senderContact1Mutable = new ContactMutableObjectCore();
            senderContact1Mutable.AddDepartment(new TextTypeWrapperMutableCore("en", "A department"));
            senderContact1Mutable.AddName(new TextTypeWrapperMutableCore("en", "a contact name"));
            senderContact1Mutable.AddName(new TextTypeWrapperMutableCore("it", "a contact name"));
            senderContact1Mutable.AddEmail("sender@example.com");
            senderContact1Mutable.AddTelephone("+12 (0)34567890");
            var senderContact2Mutable = new ContactMutableObjectCore();
            senderContact2Mutable.AddRole(new TextTypeWrapperMutableCore("en", "A role"));
            senderContact2Mutable.AddRole(new TextTypeWrapperMutableCore("it", "A role"));
            senderContact1Mutable.AddEmail("sender2@example.com");

            IContact senderContact1 = new ContactCore(senderContact1Mutable);
            IContact senderContact2 = new ContactCore(senderContact2Mutable);
            IList<IContact> senderContacts = new[] { senderContact1, senderContact2 };
            var sender = new PartyCore(null, "TestSender", senderContacts, null);
            return sender;
        }

        #endregion
    }
}