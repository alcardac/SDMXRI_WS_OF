// -----------------------------------------------------------------------
// <copyright file="TestTextSearch.cs" company="Eurostat">
//   Date Created : 2014-05-09
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

    /// <summary>
    /// Test unit for <see cref="TextSearch"/>
    /// </summary>
    [TestFixture]
    public class TestTextSearch
    {

        /// <summary>
        /// Test unit for <see cref="TextSearch"/> 
        /// </summary>
        [Test]
        public void TestM()
        {
            TextSearch text = TextSearchEnumType.Contains;
        }
    }
}