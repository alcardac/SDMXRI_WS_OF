// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataWriterManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.DataParser.Manager
{
    #region Using directives

    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;

    #endregion

    /// <summary>
    /// The data writer manager 
    /// </summary>
    public interface IDataWriterManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns a {@link DataWriterEngine} that can write the given {@link DATA_TYPE}.
        /// </summary>
        /// <param name="dataFormat">
        /// DataFormat defines the data format to write the output in
        /// </param>
        /// <param name="outPutStream">
        /// The {@link OutputStream} to write the data to
        /// </param>
        /// <returns>
        /// The data writer engine
        /// </returns>
        /// throws SdmxNotImplementedException if no {@link DataWriterEngine} could be found to write the requested data
        IDataWriterEngine GetDataWriterEngine(IDataFormat dataFormat, Stream outPutStream);

        #endregion
    }
}
