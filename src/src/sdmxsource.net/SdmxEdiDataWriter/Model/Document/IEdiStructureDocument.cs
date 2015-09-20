// -----------------------------------------------------------------------
// <copyright file="IEdiStructureDocument.cs" company="Eurostat">
//   Date Created : 2014-07-23
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Model.Document
{
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;

    /// <summary>
    /// An EDI Structure Document provides a view on the structural part of an EDI Document 
    /// </summary>
    public interface IEdiStructureDocument
    {
        /// <summary>
        /// Gets the SDMX objects from the document.
        /// </summary>
        /// <value>
        /// The SDMX objects.
        /// </value>
        ISdmxObjects SdmxObjects { get; } 
    }
}