// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataStructureFromCrossBuilder.cs" company="Eurostat">
//   Date Created : 2013-02-07
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The data structure from cross builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.SdmxObjects.Util
{
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure;

    /// <summary>
    ///     The data structure from cross builder.
    /// </summary>
    public class DataStructureFromCrossBuilder : IDataStructureFromCrossBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds an object of type <see cref="IDataStructureMutableObject"/> from the specified <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An <see cref="ICrossSectionalDataStructureMutableObject"/> object to build the output object from
        /// </param>
        /// <returns>
        /// Object of type <see cref="IDataStructureMutableObject"/>
        /// </returns>
        public IDataStructureMutableObject Build(ICrossSectionalDataStructureMutableObject buildFrom)
        {
            var imutableDsd = new DataStructureObjectCore(buildFrom);
            return new DataStructureMutableCore(imutableDsd);
        }

        #endregion
    }
}