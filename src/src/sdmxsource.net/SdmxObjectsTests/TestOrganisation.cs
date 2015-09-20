// -----------------------------------------------------------------------
// <copyright file="TestOrganisation.cs" company="EUROSTAT">
//   Date Created : 2014-12-12
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------

namespace SdmxObjectsTests
{
    using System.Linq;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    /// Test unit for organisation related SDMX Objects
    /// </summary>
    [TestFixture]
    public class TestOrganisation
    {
        /// <summary>
        /// Test unit for <see cref="OrganisationUnitMutableCore"/> 
        /// </summary>
        [TestCase("TEST_PARENT", true)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void TestOrganisationUnitMutableCoreSetParentUnit(string parentUnit, bool hasParent)
        {
            var organisationScheme = new OrganisationUnitSchemeMutableCore() { Id = "TEST", AgencyId = "TEST", Version = "1.0" };
            organisationScheme.AddName("en", "Test organisation unit scheme");
            var unit = organisationScheme.CreateItem("TEST_UNIT1", "Test organisation unit");
            unit.ParentUnit = parentUnit;
            if (!string.IsNullOrWhiteSpace(parentUnit))
            {
                organisationScheme.CreateItem(parentUnit, parentUnit);
            }

            var immutable = organisationScheme.ImmutableInstance;
            var first = immutable.Items.First(organisationUnit => organisationUnit.Id.Equals(unit.Id));
            Assert.AreEqual(hasParent, first.HasParentUnit, first.ParentUnit);
            if (first.HasParentUnit)
            {
                Assert.AreEqual(parentUnit, first.ParentUnit);
            }
        }
    }
}