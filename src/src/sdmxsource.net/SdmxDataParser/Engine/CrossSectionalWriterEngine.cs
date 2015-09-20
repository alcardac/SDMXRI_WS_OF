// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossSectionalWriterEngine.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.DataParser.Engine
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.DataParser.Properties;

    /// <summary>
    ///     The cross sectional writer.
    /// </summary>
    /// <example>
    ///     A sample in C# for <see cref="CrossSectionalWriterEngine" />
    ///     <code source="..\ReUsingExamples\DataWriting\ReUsingCrossWriter.cs" lang="cs" />
    /// </example>
    public class CrossSectionalWriterEngine : DataStreamWriterBase, ICrossSectionalWriterEngine
    {
        #region Fields
        /// <summary>
        /// A flag indicating whether the writer should be closed.
        /// </summary>
        private readonly bool _closeXmlWriter;

        /// <summary>
        ///     This field holds a value that indicates whether the dataset element is open.
        /// </summary>
        private bool _startedDataSet;

        /// <summary>
        ///     This field holds a value that indicates whether the XS Group element is open.
        /// </summary>
        private bool _startedGroup;

        /// <summary>
        ///     This field holds a value that indicates whether the observation element is open.
        /// </summary>
        private bool _startedObservation;

        /// <summary>
        ///     This field holds a value that indicates whether the Section element is open.
        /// </summary>
        private bool _startedSection;

        /// <summary>
        ///     The total observations written.
        /// </summary>
        private int _totalObservationsWritten;

        /// <summary>
        ///     The total sections written.
        /// </summary>
        private int _totalSectionsWritten;

        /// <summary>
        ///     The total cross sectional groups written.
        /// </summary>
        private int _totalXSGroupsWritten;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool _disposed;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossSectionalWriterEngine"/> class.
        /// </summary>
        /// <param name="writer">
        /// The output <see cref="XmlWriter"/>
        /// </param>
        /// <param name="sdmxVersion">
        /// The SDMX Version.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// keyFamily is null
        /// </exception>
        public CrossSectionalWriterEngine(Stream writer, SdmxSchema sdmxVersion)
            : this(XmlWriter.Create(writer), sdmxVersion)
        {
            this._closeXmlWriter = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossSectionalWriterEngine"/> class.
        /// </summary>
        /// <param name="writer">
        /// The output <see cref="XmlWriter"/>
        /// </param>
        /// <param name="sdmxVersion">
        /// The SDMX Version.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// keyFamily is null
        /// </exception>
        public CrossSectionalWriterEngine(XmlWriter writer, SdmxSchema sdmxVersion)
            : this(writer, sdmxVersion, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossSectionalWriterEngine"/> class.
        /// </summary>
        /// <param name="writer">
        /// The output <see cref="XmlWriter"/>
        /// </param>
        /// <param name="sdmxVersion">
        /// The SDMX Version.
        /// </param>
        /// <param name="useEstatUrn">
        /// The use ESTAT URN format.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// keyFamily is null
        /// </exception>
        public CrossSectionalWriterEngine(XmlWriter writer, SdmxSchema sdmxVersion, bool useEstatUrn)
            : base(writer, sdmxVersion)
        {
            if (sdmxVersion.EnumType == SdmxSchemaEnumType.VersionTwoPointOne)
            {
                throw new ArgumentException(Resources.ErrorCrossWith21, "sdmxVersion");
            }

            this.Namespaces.UseEstatUrn = useEstatUrn;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossSectionalWriterEngine"/> class.
        /// </summary>
        /// <param name="writer">
        /// The output <see cref="XmlWriter"/>
        /// </param>
        /// <param name="namespaces">
        /// The output XML message namespaces.
        /// </param>
        /// <param name="sdmxSchema">
        /// The SDMX version.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="namespaces"/> is null
        /// </exception>
        public CrossSectionalWriterEngine(XmlWriter writer, SdmxNamespaces namespaces, SdmxSchema sdmxSchema)
            : base(writer, namespaces, sdmxSchema)
        {
            if (sdmxSchema.EnumType == SdmxSchemaEnumType.VersionTwoPointOne)
            {
                throw new ArgumentException(Resources.ErrorCrossWith21, "sdmxSchema");
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the total number of Cross Sectional Observations Written.
        /// </summary>
        public int TotalObservationsWritten
        {
            get
            {
                return this._totalObservationsWritten;
            }
        }

        /// <summary>
        ///     Gets the total number of Cross Sectional Sections Written.
        /// </summary>
        public int TotalSectionsWritten
        {
            get
            {
                return this._totalSectionsWritten;
            }
        }

        /// <summary>
        ///     Gets the total number of Cross Sectional Groups Written.
        /// </summary>
        public int TotalXSGroupsWritten
        {
            get
            {
                return this._totalXSGroupsWritten;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the data format namespace Suffix e.g. compact
        /// </summary>
        protected override BaseDataFormatEnumType DataFormatType
        {
            get
            {
                return BaseDataFormatEnumType.CrossSectional;
            }
        }

        /// <summary>
        ///     Gets the dimension at observation.
        /// </summary>
        ///// TODO Waiting for SN and SLI response on what will be used in the registry
        protected override string DimensionAtObservation
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        ///     Gets the Message element tag
        /// </summary>
        protected override string MessageElement
        {
            get
            {
                return NameTableCache.GetElementName(ElementNameTable.CrossSectionalData);
            }
        }

        /// <summary>
        ///   Gets the default namespace
        /// </summary>
        protected override NamespacePrefixPair DefaultNS
        {
            get
            {
                return this.Namespaces.DataSetStructureSpecific;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Close writer
        /// </summary>
        public void Close()
        {
           this.Dispose(); 
        }

        /// <summary>
        /// Starts a dataset with the data conforming to the <paramref name="dsd"/>
        /// </summary>
        /// <param name="dataflow">
        /// The <see cref="IDataflowObject"/>
        /// </param>
        /// <param name="dsd">
        /// The <see cref="ICrossSectionalDataStructureObject"/>
        /// </param>
        /// <param name="header">
        /// The Dataset attributes
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// if the <paramref name="dsd"/> is null
        /// </exception>
        public void StartDataset(IDataflowObject dataflow, ICrossSectionalDataStructureObject dsd, IDatasetHeader header)
        {
            if (dsd == null)
            {
                throw new ArgumentNullException("dsd");
            }

            base.StartDataset(null, dsd, header);
        }

        /// <summary>
        ///     Start a Cross Sectional Section
        /// </summary>
        public void StartSection()
        {
            this.CheckDataSet("Can not StartSection, no call has been made to StartDataset");
            if (!this._startedGroup)
            {
                this.StartXSGroup();
            }

            this.EndSection();

            // <biscs:Section
            this.WriteStartElement(this.Namespaces.DataSetStructureSpecific, ElementNameTable.Section);
            this._startedSection = true;
            this._totalSectionsWritten++;
        }

        /// <summary>
        ///     Start a Cross Sectional group
        /// </summary>
        public void StartXSGroup()
        {
            this.CheckDataSet("Can not StartXSGroup, no call has been made to StartDataset");
            this.EndGroup();

            // <biscs:Group
            this.WriteStartElement(this.Namespaces.DataSetStructureSpecific, ElementNameTable.Group);
            this._startedGroup = true;
            this._totalXSGroupsWritten++;
        }

        /// <summary>
        /// Write a Cross Sectional Measure with <paramref name="measure"/> and <paramref name="value"/>
        /// </summary>
        /// <param name="measure">
        /// The measure concept. This will be used as element tag. In case Primary Measure is used, use the PrimaryMeasure concept.
        /// </param>
        /// <param name="value">
        /// The measure value
        /// </param>
        public void StartXSObservation(string measure, string value)
        {
            this.CheckDataSet("Can not StartXSObservation, no call has been made to StartDataset");
            if (!this._startedSection)
            {
                throw new InvalidOperationException(Resources.ErrorStartSectionNotCalled);
            }

            this.EndObservation();

            this.WriteStartElement(this.Namespaces.DataSetStructureSpecific, measure);
            this.TryWriteAttribute(AttributeNameTable.value, string.IsNullOrEmpty(value) ? this.DefaultObs : value);
            this._startedObservation = true;
            this._totalObservationsWritten++;
        }

        /// <summary>
        /// Write an <paramref name="attribute"/> and the <paramref name="value"/>
        /// </summary>
        /// <param name="attribute">
        /// The attribute concept id
        /// </param>
        /// <param name="value">
        /// The value
        /// </param>
        public void WriteAttributeValue(string attribute, string value)
        {
            attribute = this.GetComponentId(attribute);
            this.CheckDataSet("Can not WriteAttributeValue, no call has been made to StartDataset");
            this.WriteAttributeString(attribute, value);
        }

        /// <summary>
        /// Write a Cross Sectional section <paramref name="key"/> and the <paramref name="value"/>
        /// </summary>
        /// <param name="key">
        /// The key. i.e. the dimension
        /// </param>
        /// <param name="value">
        /// The value
        /// </param>
        public void WriteSectionKeyValue(string key, string value)
        {
            key = this.GetComponentId(key);
            this.CheckDataSet("Can not WriteSectionKeyValue, no call has been made to StartDataset");
            if (!this._startedSection)
            {
                throw new InvalidOperationException(Resources.ErrorStartSectionNotCalled);
            }

            this.WriteAttributeString(key, value);
        }

        /// <summary>
        /// Write a Cross Sectional DataSet <paramref name="key"/> and the <paramref name="value"/>
        /// </summary>
        /// <param name="key">
        /// The key. i.e. the dimension
        /// </param>
        /// <param name="value">
        /// The value
        /// </param>
        public void WriteDataSetKeyValue(string key, string value)
        {
            key = this.GetComponentId(key);
            this.CheckDataSet("Can not WriteXSDataSetKeyValue, no call has been made to StartDataset");
            this.WriteAttributeString(key, value);
        }

        /// <summary>
        /// Write a Cross Sectional Group <paramref name="key"/> and the <paramref name="value"/>
        /// </summary>
        /// <param name="key">
        /// The key. i.e. the dimension
        /// </param>
        /// <param name="value">
        /// The value
        /// </param>
        public void WriteXSGroupKeyValue(string key, string value)
        {
            key = this.GetComponentId(key);
            this.CheckDataSet("Can not WriteXSGroupKeyValue, no call has been made to StartDataset");
            if (!this._startedGroup)
            {
                throw new InvalidOperationException(Resources.ErrorStartGroupNotCalled);
            }

            this.WriteAttributeString(key, value);
        }

        /// <summary>
        /// Write a Cross Sectional measure <paramref name="key"/> and the <paramref name="value"/>
        /// </summary>
        /// <param name="key">
        /// The key. i.e. the dimension
        /// </param>
        /// <param name="value">
        /// The value
        /// </param>
        public void WriteXSObservationKeyValue(string key, string value)
        {
            key = this.GetComponentId(key);
            this.CheckDataSet("Can not WriteXSObservationKeyValue, no call has been made to StartDataset");
            if (!this._startedObservation)
            {
                throw new InvalidOperationException(Resources.ErrorStartObsNotCalled);
            }

            this.WriteAttributeString(key, value);
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
        /// <param name="disposable"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposable)
        {
            if (!this._disposed)
            {
                if (disposable)
                {
                    this.EndObservation();
                    this.EndSection();
                    this.EndGroup();
                    this.EndDataSet();
                    this.CloseMessageTag();
                    if (this._closeXmlWriter)
                    {
                        this.SdmxMLWriter.Close();
                    }

                    this._disposed = true;
                }
            }
        }

        /// <summary>
        /// Build the DSD specific URN
        /// </summary>
        /// <param name="namespacePrefix">
        /// The namespace Prefix.
        /// </param>
        /// <returns>
        /// The <see cref="NamespacePrefixPair"/>.
        /// </returns>
        protected override NamespacePrefixPair BuildDSDSpecificUrn(string namespacePrefix)
        {
            if (this.TargetSchema.EnumType != SdmxSchemaEnumType.VersionTwoPointOne && this.Namespaces.UseEstatUrn)
            {
                this.Namespace = string.Format(
                    CultureInfo.InvariantCulture, 
                    "{0}{1}:{2}:{3}:cross", 
                    "urn:estat:sdmx.infomodel.keyfamily.KeyFamily=", 
                    this.KeyFamily.AgencyId, 
                    this.KeyFamily.Id, 
                    this.KeyFamily.Version);
            }
            else
            {
                // TODO explict measure possibly in a different implementation ?
                this.Namespace = string.Format(CultureInfo.InvariantCulture, "{0}:cross", this.KeyFamily.Urn);
            }

            return new NamespacePrefixPair(this.Namespace, this.NamespacePrefix);
        }

        /// <summary>
        ///     Conditionally end DataSet element
        /// </summary>
        protected override void EndDataSet()
        {
            base.EndDataSet();
            if (this._startedDataSet)
            {
                this._startedDataSet = false;
            }
        }

        /// <summary>
        /// Gets the dimension at observation.
        /// </summary>
        /// <param name="header">
        /// The dataset header.
        /// </param>
        /// <returns>
        /// The dimension at observation
        /// </returns>
        protected override string GetDimensionAtObservation(IDatasetHeader header)
        {
            return null;
        }

        /// <summary>
        /// Conditionally start the DataSet if <see cref="_startedDataSet"/> is false
        /// </summary>
        /// <param name="header">
        /// The Dataset header
        /// </param>
        protected override void WriteFormatDataSet(IDatasetHeader header)
        {
            if (this._startedDataSet)
            {
                return;
            }

            this.WriteStartElement(this.Namespaces.DataSetStructureSpecific, ElementNameTable.DataSet);
            this.WriteDataSetHeader(header);
            this._startedDataSet = true;
        }

        /// <summary>
        ///     Conditionally end group element
        /// </summary>
        private void EndGroup()
        {
            this.EndSection();

            if (this._startedGroup)
            {
                this.WriteEndElement();
                this._startedGroup = false;
            }
        }

        /// <summary>
        ///     Conditionally end observation
        /// </summary>
        private void EndObservation()
        {
            if (this._startedObservation)
            {
                this.WriteEndElement();
                this._startedObservation = false;
            }
        }

        /// <summary>
        ///     Conditionally end series
        /// </summary>
        private void EndSection()
        {
            this.EndObservation();

            if (this._startedSection)
            {
                // in which case close it
                this.WriteEndElement();
                this._startedSection = false;
            }
        }

        #endregion
    }
}