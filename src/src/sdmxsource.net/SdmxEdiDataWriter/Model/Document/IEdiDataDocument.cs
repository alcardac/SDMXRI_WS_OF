// -----------------------------------------------------------------------
// <copyright file="IEdiDataDocument.cs" company="Eurostat">
//   Date Created : 2014-07-23
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Model.Document
{
    using Org.Sdmxsource.Sdmx.EdiParser.Model.Reader;

    /// <summary>
    /// An EDI Data document wraps an EDI data portion of an EDIDocument and can iterate the datasets if given the correct DSD that describes the data.
    /// </summary>
    public interface IEdiDataDocument
    {
        /// <summary>
        /// Gets the data reader.
        /// </summary>
        /// <value>
        /// The data reader.
        /// </value>
        IEdiDataReader DataReader { get; } 
    }
}