// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EdiDataReader.cs" company="Eurostat">
//   Date Created : 2014-07-28
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The EDI data reader.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Model.Reader
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.EdiParser.Model.Document;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     The EDI data reader.
    /// </summary>
    public class EdiDataReader : EdiAbstractPositionalReader, IEdiDataReader
    {
        #region Fields

        /// <summary>
        ///     The document position.
        /// </summary>
        private readonly IEdiDocumentPosition _documentPosition;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EdiDataReader"/> class.
        /// </summary>
        /// <param name="dataFile">
        /// The data file.
        /// </param>
        /// <param name="documentPosition">
        /// The document position.
        /// </param>
        /// <param name="ediMetadata">
        /// The EDI metadata.
        /// </param>
        public EdiDataReader(IReadableDataLocation dataFile, IEdiDocumentPosition documentPosition, IEdiMetadata ediMetadata)
            : base(dataFile, documentPosition, ediMetadata)
        {
            this._documentPosition = documentPosition;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the data set header for the data that this reader is reading.
        /// </summary>
        /// <value>
        ///     The data set header object.
        /// </value>
        public IDatasetHeader DataSetHeaderObject
        {
            get
            {
                string datasetId = this._documentPosition.DatasetId;
                DatasetAction datasetAction = this._documentPosition.DatasetAction;
                string dsdId = this._documentPosition.DataStructureIdentifier;

                IStructureReference dsdRef = new StructureReferenceImpl(this.MessageAgency, dsdId, MaintainableObject.DefaultVersion, SdmxStructureEnumType.Dsd);

                IDatasetStructureReference structureReference = new DatasetStructureReferenceCore(dsdRef);

                return new DatasetHeaderCore(datasetId, datasetAction, structureReference);
            }
        }

        /// <summary>
        ///     Gets the dataset attributes.
        /// </summary>
        /// <value>
        ///     The dataset attributes.
        /// </value>
        public IList<IKeyValue> DatasetAttributes
        {
            get
            {
                return this._documentPosition.DatasetAttributes;
            }
        }

        /// <summary>
        ///     Gets the missing value.
        /// </summary>
        /// <value>
        ///     The missing value.
        /// </value>
        public string MissingValue
        {
            get
            {
                return this._documentPosition.MissingValue;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="managed">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected override void Dispose(bool managed)
        {
            if (managed)
            {
                this.DataFile.Close();
            }

            base.Dispose(managed);
        }

        #endregion
    }
}