// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataWriterFactory.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.DataParser.Factory
{
    using System.IO;

    using Estat.Sri.SdmxEdiDataWriter.Engine;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;

    /// <summary>
    ///     The data writer factory.
    /// </summary>
    public class DataWriterFactory : IDataWriterFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the data writer engine.
        /// </summary>
        /// <param name="dataFormat">
        /// The data format.
        /// </param>
        /// <param name="outStream">
        /// The output stream.
        /// </param>
        /// <returns>
        /// The <see cref="IDataWriterEngine"/>.
        /// </returns>
        public IDataWriterEngine GetDataWriterEngine(IDataFormat dataFormat, Stream outStream)
        {
            if (dataFormat.SdmxDataFormat != null)
            {
                switch (dataFormat.SdmxDataFormat.BaseDataFormat.EnumType)
                {
                    case BaseDataFormatEnumType.Generic:
                        {
                            return new GenericDataWriterEngine(outStream, dataFormat.SdmxDataFormat.SchemaVersion);
                        }

                    case BaseDataFormatEnumType.Compact:
                        {
                            return new CompactDataWriterEngine(outStream, dataFormat.SdmxDataFormat.SchemaVersion);
                        }

                    case BaseDataFormatEnumType.Edi:
                        return new GesmesTimeSeriesWriter(outStream, true);
                }
            }

            return null;
        }

        #endregion
    }
}