// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWriteableDataLocation.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Util
{
    #region Using directives

    using System.IO;

    #endregion

    /// <summary>
    ///     A WriteableDataLocation extends the capability of being able to read data by giving access to the OutputStream
    ///     to the source data.  This allows additional information to be written to the source
    /// </summary>
    public interface IWriteableDataLocation : IReadableDataLocation
    {
        #region Public Properties

        /// <summary>
        ///     Gets the output stream.
        /// </summary>
        Stream OutputStream { get; }

        #endregion
    }
}