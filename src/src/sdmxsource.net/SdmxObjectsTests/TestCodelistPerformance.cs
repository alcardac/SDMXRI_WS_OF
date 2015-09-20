// -----------------------------------------------------------------------
// <copyright file="TestCodelistPerformance.cs" company="Eurostat">
//   Date Created : 2014-06-13
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace SdmxObjectsTests
{
    using System.Diagnostics;
    using System.Globalization;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;

    [TestFixture]
    public class TestCodelistPerformance
    {
        [TestCase(10000)]
        public void TestMutableToImmutable(int size)
        {
            ICodelistMutableObject mutable = BuildMutable(size);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ICodelistObject immutableInstance = mutable.ImmutableInstance;
            stopwatch.Stop();
            Assert.NotNull(immutableInstance);
            Assert.LessOrEqual(stopwatch.ElapsedMilliseconds, size >> 5);
        }

        private static ICodelistMutableObject BuildMutable(int size)
        {
            var codelist = new CodelistMutableCore() { Id = "TEST", AgencyId = "TEST_AGENCY", Version = "1.0" };
            codelist.AddName("en", "Test name");
            string lastCode = null;
            for (int i = 0; i < size; i++)
            {
                string codeId = string.Format(CultureInfo.InvariantCulture, "ID{0}", i);
                var code = codelist.CreateItem(codeId, codeId);
                if (lastCode != null && (i % 2) == 0)
                {
                    code.ParentCode = lastCode;
                }

                if ((i % 6) == 0)
                {
                    lastCode = codeId;
                }
            }

            return codelist;
        }
    }
}