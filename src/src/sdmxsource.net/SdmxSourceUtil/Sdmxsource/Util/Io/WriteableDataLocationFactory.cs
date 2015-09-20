// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteableDataLocationFactory.cs" company="Eurostat">
//   Date Created : 2014-07-29
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The writable data location factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Util.Io
{
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Util;

    /// <summary>
    ///     The writable data location factory.
    /// </summary>
    public class WriteableDataLocationFactory : IWriteableDataLocationFactory
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Returns a write-able data location, where data can be written to for temporary purposes
        /// </summary>
        /// <returns>The <see cref="IWriteableDataLocation" /></returns>
        public IWriteableDataLocation GetTemporaryWriteableDataLocation()
        {
            return new WriteableDataLocationTmp();
        }

        #endregion
    }
}