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
namespace CSVZip.Controller.Builder
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.ServiceModel;
    using System.Xml;

    using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Estat.Sri.Ws.Controllers.Engine;
    using Estat.Sri.Ws.Controllers.Controller;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;

    using CSVZip.Engine;
    using Estat.Sri.Ws.Controllers.Builder;
    /// <summary>
    ///     The <see cref="IDataWriterEngine" /> engine builder from <see cref="XmlWriter" /> or <see cref="Stream" />
    /// </summary>
    public class CsvZipDataWriterBuilder : IWriterBuilder<ICsvZipDataWriterEngine, CsvZipTextWriter>
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
        public CsvZipDataWriterBuilder(BaseDataFormat dataFormat, SdmxSchema sdmxSchema)
        {
            this._dataFormat = dataFormat;
            this._sdmxSchema = sdmxSchema;
        }

        #endregion

        #region Public Methods and Operators


        public ICsvZipDataWriterEngine Build(CsvZipTextWriter writer, Queue<Action> actions)
        {
            return new CsvZipDelayedDataWriterEngine(new CsvZipBaseDataWriter(writer), actions);
        }


        #endregion


    }
}