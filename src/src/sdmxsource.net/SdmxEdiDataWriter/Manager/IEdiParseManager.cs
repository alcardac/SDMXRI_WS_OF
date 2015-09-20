// -----------------------------------------------------------------------
// <copyright file="IEdiParseManager.cs" company="Eurostat">
//   Date Created : 2014-05-15
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Manager
{
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.EdiParser.Model;

    /// <summary>
    /// The EdiParseManager interface.
    /// </summary>
    public interface IEdiParseManager
    {
        /// <summary>
        /// Writes the <paramref name="objects"/> contents out as EDI-TS to the <paramref name="output"/>
        /// </summary>
        /// <param name="objects">The objects.</param>
        /// <param name="output">The output.</param>
        void WriteToEdi(ISdmxObjects objects, Stream output);

        /// <summary>
        /// Processes an EDI message and returns a workspace containing the SDMX structures and data that were contained in the message
        /// </summary>
        /// <param name="ediMessageLocation">
        /// The EDI message location.
        /// </param>
        /// <returns>
        /// The <see cref="IEdiWorkspace"/>.
        /// </returns>
        IEdiWorkspace ParseEdiMessage(IReadableDataLocation ediMessageLocation);
    }
}