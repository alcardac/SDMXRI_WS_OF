// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataParseMetadata.cs" company="Eurostat">
//   Date Created : 2014-07-22
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Contains information required for data parsing.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.DataParser.Model
{
    #region Using directives

    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Util;

    #endregion

    /// <summary>
    ///     Contains information required for data parsing.
    /// </summary>
    public class DataParseMetadata
    {
        #region Fields

        /// <summary>
        /// The _data structure.
        /// </summary>
        private readonly IDataStructureObject _dataStructure;

        /// <summary>
        /// The _out put stream.
        /// </summary>
        private readonly Stream _outPutStream;

        /// <summary>
        /// The _output schema version.
        /// </summary>
        private readonly SdmxSchema _outputSchemaVersion;

        /// <summary>
        /// The _source data.
        /// </summary>
        private readonly IReadableDataLocation _sourceData;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataParseMetadata"/> class.
        /// </summary>
        /// <param name="sourceData">
        /// The readable data location
        /// </param>
        /// <param name="outPutStream">
        /// The output stream
        /// </param>
        /// <param name="outputSchemaVersion">
        /// The output schema version
        /// </param>
        /// <param name="keyFamily">
        /// The key family
        /// </param>
        public DataParseMetadata(IReadableDataLocation sourceData, Stream outPutStream, SdmxSchema outputSchemaVersion, IDataStructureObject keyFamily)
        {
            this._sourceData = sourceData;
            this._outPutStream = outPutStream;
            this._dataStructure = keyFamily;
            this._outputSchemaVersion = outputSchemaVersion;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the data structure
        /// </summary>
        public IDataStructureObject DataStructure
        {
            get
            {
                return this._dataStructure;
            }
        }

        /// <summary>
        ///     Gets the output stream
        /// </summary>
        public Stream OutPutStream
        {
            get
            {
                return this._outPutStream;
            }
        }

        /// <summary>
        ///     Gets the output schema version
        /// </summary>
        public SdmxSchema OutputSchemaVersion
        {
            get
            {
                return this._outputSchemaVersion;
            }
        }

        /// <summary>
        ///     Gets the source data
        /// </summary>
        public IReadableDataLocation SourceData
        {
            get
            {
                return this._sourceData;
            }
        }

        #endregion
    }
}