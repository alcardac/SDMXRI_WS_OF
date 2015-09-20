// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestMutableStructureSearchManagerV20.cs" company="Eurostat">
//   Date Created : 2013-04-16
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Test unit for <see cref="CategorisationV20MutableStructureSearchManager" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace StructureRetriever.Tests
{
    using Estat.Nsi.StructureRetriever.Manager;

    using NUnit.Framework;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    ///     Test unit for <see cref="StructureRetrieverAvailableData" />
    /// </summary>
    [TestFixture]
    public class TestMutableStructureSearchManagerV20 : TestMutableStructureSearchManager
    {
        #region Methods

        /// <summary>
        /// The get schema.
        /// </summary>
        /// <returns>
        /// The <see cref="SdmxSchema"/>.
        /// </returns>
        protected override SdmxSchema GetSchema()
        {
            return SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo);
        }

        #endregion
    }
}