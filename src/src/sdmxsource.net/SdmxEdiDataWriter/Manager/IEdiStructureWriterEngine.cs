// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEdiStructureWriterEngine.cs" company="Eurostat">
//   Date Created : 2014-07-29
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The EDI structure writer engine
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Manager
{
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects;

    /// <summary>
    ///     The EDI structure writer engine
    /// </summary>
    public interface IEdiStructureWriterEngine
    {
        #region Public Methods and Operators

        /// <summary>
        /// Writes the SDMX Beans contents out as EDI-TS to the output stream.
        ///     <p/>
        ///     Note - this will only write out code-lists, concepts and key families
        /// </summary>
        /// <param name="beans">
        /// The beans.
        /// </param>
        /// <param name="output">
        /// The output.
        /// </param>
        void WriteToEDI(ISdmxObjects beans, Stream output);

        #endregion
    }
}