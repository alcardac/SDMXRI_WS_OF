// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEdiReader.cs" company="Eurostat">
//   Date Created : 2014-07-23
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The EDI Reader interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Model.Reader
{
    using Org.Sdmxsource.Sdmx.EdiParser.Constants;

    /// <summary>
    ///     The EDI Reader interface.
    /// </summary>
    public interface IEdiReader : IFileReader
    {
        #region Public Properties

        /// <summary>
        ///     Gets the type of the line.
        /// </summary>
        /// <value>
        ///     The type of the line.
        /// </value>
        EdiPrefix LineType { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Parses the current line as text
        /// </summary>
        /// <returns>the current line as text</returns>
        string ParseTextString();

        #endregion
    }
}