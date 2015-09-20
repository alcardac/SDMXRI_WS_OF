// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataWriterBuilder.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The <see cref="IDataWriterEngine" /> engine builder from <see cref="XmlWriter" /> or <see cref="Stream" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Builder
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.ServiceModel;
    using System.Xml;

    using Estat.Sri.Ws.Controllers.Engine;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.DataParser.Engine;

    /// <summary>
    ///     The <see cref="IDataWriterEngine" /> engine builder from <see cref="XmlWriter" /> or <see cref="Stream" />
    /// </summary>
    public class DataWriterBuilder : IWriterBuilder<IDataWriterEngine, XmlWriter>, IWriterBuilder<IDataWriterEngine, Stream>
    {
        #region Fields

        /// <summary>
        ///     The _data format.
        /// </summary>
        private readonly BaseDataFormat _dataFormat;

        /// <summary>
        ///     The _sdmx schema.
        /// </summary>
        private readonly SdmxSchema _sdmxSchema;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataWriterBuilder"/> class.
        /// </summary>
        /// <param name="dataFormat">
        /// The data Format.
        /// </param>
        /// <param name="sdmxSchema">
        /// The sdmx Schema.
        /// </param>
        public DataWriterBuilder(BaseDataFormat dataFormat, SdmxSchema sdmxSchema)
        {
            this._dataFormat = dataFormat;
            this._sdmxSchema = sdmxSchema;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="actions">The actions.</param>
        /// <returns>
        /// The <see cref="IDataWriterEngine" />.
        /// </returns>
        /// <exception cref="Org.Sdmxsource.Sdmx.Api.Exception.SdmxNotImplementedException">Not supported IDataWriterEngine for XmlWriter output</exception>
        public IDataWriterEngine Build(XmlWriter writer, Queue<Action> actions)
        {
            switch (this._dataFormat.EnumType)
            {
                case BaseDataFormatEnumType.Generic:
                    return new DelayedDataWriterEngine(new GenericDataWriterEngine(writer, this._sdmxSchema), actions);
                case BaseDataFormatEnumType.Compact:
                    return new DelayedDataWriterEngine(new CompactDataWriterEngine(writer, this._sdmxSchema), actions);
                default:
                    var message = string.Format(CultureInfo.InvariantCulture, "Not supported IDataWriterEngine for XmlWriter output {0}", this._dataFormat);
                    throw new SdmxNotImplementedException(message);
            }
        }

        /// <summary>
        /// Builds the specified writer engine.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <exception cref="Org.Sdmxsource.Sdmx.Api.Exception.SdmxNotImplementedException">
        /// Not supported IDataWriterEngine for Stream output
        /// </exception>
        /// <returns>
        /// The <see cref="IDataWriterEngine"/>.
        /// </returns>
        public IDataWriterEngine Build(Stream writer, Queue<Action> actions)
        {
            switch (this._dataFormat.EnumType)
            {
                case BaseDataFormatEnumType.Generic:
                case BaseDataFormatEnumType.Compact:
                    return this.Build(XmlWriter.Create(writer), actions);

                    //// case BaseDataFormatEnumType.Edi:
                    ////     // TODO
                    ////      return  GesmesTimeSeriesWriter(writer);
                    ////     break;
                default:
                    var message = string.Format(CultureInfo.InvariantCulture, "Not supported IDataWriterEngine for Stream output {0}", this._dataFormat);
                    throw new SdmxNotImplementedException(message);
            }
        }

        #endregion
    }
}