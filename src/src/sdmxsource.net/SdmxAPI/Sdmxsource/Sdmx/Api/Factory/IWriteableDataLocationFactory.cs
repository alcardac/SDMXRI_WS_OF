// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWriteableDataLocationFactory.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Factory
{
    using Org.Sdmxsource.Sdmx.Api.Util;

    /// <summary>
    /// Used to create WriteableDataLocation
    /// </summary>
    public interface IWriteableDataLocationFactory
    {
        /// <summary>
        /// Returns a write-able data location, where data can be written to for temporary purposes
        /// </summary>
        /// <returns>The <see cref="IWriteableDataLocation"/></returns>
        IWriteableDataLocation GetTemporaryWriteableDataLocation(); 
    }
}
