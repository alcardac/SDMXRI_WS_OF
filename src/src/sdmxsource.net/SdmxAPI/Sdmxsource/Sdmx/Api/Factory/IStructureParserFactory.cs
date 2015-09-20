// -----------------------------------------------------------------------
// <copyright file="IStructureParserFactory.cs" company="EUROSTAT">
//   Date Created : 2014-03-20
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Api.Factory
{
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Util;

    /// <summary>
    /// Parses a stream of information to build <see cref="ISdmxObjects"/>
    /// </summary>
    public interface IStructureParserFactory
    {
        /// <summary>
        /// Parses the <see cref="IReadableDataLocation"/> to build the <see cref="ISdmxObjects"/>, returns null if the structure format can not be determined from 
        /// the information provided in the <see cref="IReadableDataLocation"/> (or from the underlying data)
        /// </summary>
        /// <param name="readableDataLocation">The readable data location.</param>
        /// <returns>returns null if the structure format can not be determined from 
        /// the information provided in the <see cref="IReadableDataLocation"/> (or from the underlying data)</returns>
        ISdmxObjects GetSdmxObjects(IReadableDataLocation readableDataLocation);
    }
}