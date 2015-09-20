// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEdiParseEngine.cs" company="Eurostat">
//   Date Created : 2014-07-29
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The EdiParseEngine interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Engine
{
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.EdiParser.Model.Document;

    /// <summary>
    /// The EdiParseEngine interface.
    /// </summary>
    public interface IEdiParseEngine
    {
        #region Public Methods and Operators

        /// <summary>
        /// Validates the Edi is valid and returns metadata about the parsed Edi file
        /// </summary>
        /// <param name="ediMessageLocation">
        /// The EDI message location.
        /// </param>
        /// <returns>
        /// The <see cref="IEdiMetadata"/>.
        /// </returns>
        IEdiMetadata ParseEDIMessage(IReadableDataLocation ediMessageLocation);

        #endregion
    }
}