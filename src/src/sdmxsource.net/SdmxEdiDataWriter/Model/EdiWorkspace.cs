// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EdiWorkspace.cs" company="Eurostat">
//   Date Created : 2014-07-29
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The EDI workspace.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Model
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.EdiParser.Constants;
    using Org.Sdmxsource.Sdmx.EdiParser.Engine;
    using Org.Sdmxsource.Sdmx.EdiParser.Engine.Reader;
    using Org.Sdmxsource.Sdmx.EdiParser.Model.Document;
    using Org.Sdmxsource.Sdmx.EdiParser.Model.Reader;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    /// <summary>
    ///     The EDI workspace.
    /// </summary>
    public class EdiWorkspace : IEdiWorkspace
    {
        #region Fields

        /// <summary>
        ///     The beans.
        /// </summary>
        private readonly IList<ISdmxObjects> _beans = new List<ISdmxObjects>();

        /// <summary>
        ///     The data documents.
        /// </summary>
        private readonly IList<IEdiDataDocument> _dataDocuments = new List<IEdiDataDocument>();

        /// <summary>
        ///     The EDI metadata.
        /// </summary>
        private readonly IEdiMetadata _ediMetadata;

        /// <summary>
        ///     The key family references.
        /// </summary>
        private readonly ISet<IMaintainableRefObject> _keyFamilyReferences = new HashSet<IMaintainableRefObject>();

        /// <summary>
        /// The _writeable data location
        /// </summary>
        private readonly IWriteableDataLocation _writeableDataLocation;

        /// <summary>
        ///     The header bean.
        /// </summary>
        private IHeader _headerBean;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EdiWorkspace"/> class.
        /// </summary>
        /// <param name="ediDocument">
        /// The EDI document.
        /// </param>
        /// <param name="writeableDataLocationFactory">
        /// The writable Data Location Factory.
        /// </param>
        /// <param name="ediParseEngine">
        /// The EDI Parse Engine.
        /// </param>
        public EdiWorkspace(IReadableDataLocation ediDocument, IWriteableDataLocationFactory writeableDataLocationFactory, IEdiParseEngine ediParseEngine)
        {
            this._ediMetadata = ediParseEngine.ParseEDIMessage(ediDocument);
            IFileReader fr = new FileReaderImpl(ediDocument, EdiConstants.EndOfLineRegEx, EdiConstants.CharsetEncoding);

            foreach (var documentPosition in this._ediMetadata.DocumentIndex)
            {
                int start = documentPosition.StartLine;
                this._writeableDataLocation = writeableDataLocationFactory.GetTemporaryWriteableDataLocation();
                using (TextWriter osw = new StreamWriter(this._writeableDataLocation.OutputStream, EdiConstants.CharsetEncoding))
                {
                    while (fr.MoveNext())
                    {
                        int idx = fr.LineNumber;

                        // If this next line is within the bounds of the start and end location then write the current line to the writer  
                        if (idx >= start)
                        {
                            string line = fr.CurrentLine;
                            Write(osw, line + EdiConstants.EndTag);

                            // WriteLineSeparator(osw);

                            // Keep going until we encounter the end segment indicator
                            if (line.StartsWith(EdiPrefix.EndMessageAdministration.ToString()))
                            {
                                break;
                            }
                        }
                    }

                    osw.Flush();
                }

                if (documentPosition.IsData)
                {
                    IEdiDataDocument dataDocument = new EdiDataDocument(this._writeableDataLocation, documentPosition, this._ediMetadata);
                    this._dataDocuments.Add(dataDocument);
                    this._keyFamilyReferences.Add(dataDocument.DataReader.DataSetHeaderObject.DataStructureReference.StructureReference.MaintainableReference);
                }
                else
                {
                    ////IEdiStructureDocument doc = new EDIStructureDocumentImpl(wdl, documentPosition, this._ediMetadata);
                    ////this._beans.Add(doc.SdmxObjects);
                    this._writeableDataLocation.Close();
                }
            }

            this.CreateHeader();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the dataset headers.
        /// </summary>
        /// <value>
        ///     The dataset headers.
        /// </value>
        public IList<IDatasetHeader> DatasetHeaders
        {
            get
            {
                IList<IDatasetHeader> returnList = new List<IDatasetHeader>();
                foreach (var currentReader in this._dataDocuments)
                {
                    IEdiDataReader reader = currentReader.DataReader;
                    IDatasetHeader header = reader.DataSetHeaderObject;
                    returnList.Add(header);
                }

                return returnList;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether any <see cref="IEdiDataDocument" /> exist in this workspace.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [has data]; otherwise, <c>false</c>.
        /// </value>
        public bool HasData
        {
            get
            {
                return this._dataDocuments.Count > 0;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether any <see cref="ISdmxObjects" /> exist in this workspace.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [has structures]; otherwise, <c>false</c>.
        /// </value>
        public bool HasStructures
        {
            get
            {
                return this._beans.Count > 0;
            }
        }

        /// <summary>
        ///     Gets the header.
        /// </summary>
        /// <value>
        ///     The header.
        /// </value>
        public IHeader Header
        {
            get
            {
                return this._headerBean;
            }
        }

        /// <summary>
        ///     Gets the merged objects.
        /// </summary>
        /// <value>
        ///     The merged objects.
        /// </value>
        public ISdmxObjects MergedObjects
        {
            get
            {
                if (!this.HasStructures)
                {
                    throw new SdmxNoResultsException("There are no structures within this Edi file.");
                }

                if (this._beans.Count == 1)
                {
                    return this._beans[0];
                }

                ISdmxObjects returnBeans = new SdmxObjectsImpl(this._headerBean);
                foreach (var currentBeans in this._beans)
                {
                    returnBeans.Merge(currentBeans);
                }

                return returnBeans;
            }
        }

        /// <summary>
        ///     Gets the objects.
        /// </summary>
        /// <value>
        ///     The objects.
        /// </value>
        public IList<ISdmxObjects> Objects
        {
            get
            {
                return new List<ISdmxObjects>(this._beans);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the data reader.
        /// </summary>
        /// <param name="retrievalManager">
        /// The retrieval manager.
        /// </param>
        /// <returns>
        /// The <see cref="IDataReaderEngine"/> if this <see cref="IEdiWorkspace.HasData"/> is <c>True</c>; otherwise null.
        /// </returns>
        public IDataReaderEngine GetDataReader(ISdmxObjectRetrievalManager retrievalManager)
        {
            IList<IEdiDataReader> engines = new List<IEdiDataReader>();
            foreach (var dataDocument in this._dataDocuments)
            {
                engines.Add(dataDocument.DataReader);
            }

            return new EdiDataReaderEngine(this._headerBean, retrievalManager, engines);
        }

        /// <summary>
        /// Gets the data reader.
        /// </summary>
        /// <param name="keyFamily">
        /// The key family.
        /// </param>
        /// <param name="dataflow">
        /// The dataflow (Optional).
        /// </param>
        /// <returns>
        /// The <see cref="IDataReaderEngine"/> if this <see cref="IEdiWorkspace.HasData"/> is <c>True</c>; otherwise null.
        /// </returns>
        public IDataReaderEngine GetDataReader(IDataStructureObject keyFamily, IDataflowObject dataflow)
        {
            if (!this.HasData)
            {
                throw new ArgumentException("Attempting to read data from an Edi file that does not contain data");
            }

            if (!this._keyFamilyReferences.Contains(keyFamily.AsReference.MaintainableReference))
            {
                return null;
            }

            IList<IEdiDataReader> engines = new List<IEdiDataReader>();
            foreach (var dataDocument in this._dataDocuments)
            {
                if (dataDocument.DataReader.DataSetHeaderObject.DataStructureReference.StructureReference.MaintainableReference.Equals(keyFamily.AsReference.MaintainableReference))
                {
                    engines.Add(dataDocument.DataReader);
                }
            }

            return new EdiDataReaderEngine(this._headerBean, dataflow, keyFamily, engines);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Methods
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="managed"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool managed)
        {
            if (managed)
            {
                if (this._writeableDataLocation != null)
                {
                    this._writeableDataLocation.Close();
                }
            }
        }

        /// <summary>
        /// Writes the specified output stream writer.
        /// </summary>
        /// <param name="outputStreamWriter">
        /// The output stream writer.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        private static void Write(TextWriter outputStreamWriter, string value)
        {
            outputStreamWriter.Write(value);
        }

        /// <summary>
        ///     Creates the header.
        /// </summary>
        private void CreateHeader()
        {
            IList<IDatasetStructureReference> structure = new List<IDatasetStructureReference>();
            IList<IParty> receiver = new List<IParty>();
            IParty sender = null;
            ISet<string> allreceivers = new HashSet<string>();
            foreach (var currentReader in this._dataDocuments)
            {
                IEdiDataReader reader = currentReader.DataReader;
                IDatasetHeader header = reader.DataSetHeaderObject;

                // Possible BUG in SdmxSource.Java v1.1.4 variable structure is not used.
                structure.Add(header.DataStructureReference);
                sender = reader.SendingAgency;
                string recievingAgency = reader.RecievingAgency;
                if (!allreceivers.Contains(recievingAgency))
                {
                    allreceivers.Add(recievingAgency);
                    receiver.Add(new PartyCore(null, recievingAgency, null, null));
                }
            }

            // Possible BUG in SdmxSource.Java v1.1.4 variable structure is not used.
            this._headerBean = new HeaderImpl(
                this._ediMetadata.InterchangeReference, 
                this._ediMetadata.DateOfPreparation, 
                this._ediMetadata.ReportingBegin, 
                this._ediMetadata.ReportingEnd, 
                receiver, 
                sender, 
                this._ediMetadata.IsTest);
        }

        #endregion
    }
}