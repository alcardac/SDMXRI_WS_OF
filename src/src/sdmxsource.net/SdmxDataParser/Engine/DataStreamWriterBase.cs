// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataStreamWriterBase.cs" company="Eurostat">
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Engine;
    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Util.Objects;

    /// <summary>
    ///     This is a base class for all the Writers classes that writes XML files.
    /// </summary>
    public abstract class DataStreamWriterBase : Writer
    {
        #region Fields

        /// <summary>
        ///     The dataset namespace
        /// </summary>
        private NamespacePrefixPair _dataSetNS;

        /// <summary>
        ///     Default observation value to use in case it is null or empty
        /// </summary>
        private string _defaultObs = XmlConstants.NaN;

        /// <summary>
        ///     The dimension at observation
        /// </summary>
        private string _dimensionAtObservation;

        /// <summary>
        ///     The comment to be added to the generated file
        /// </summary>
        private string _generatedFileComment;

        /// <summary>
        ///     The message header
        /// </summary>
        private IHeader _header;

        /// <summary>
        ///     This field holds if this message is a delete message
        /// </summary>
        private bool _isDeleteMessage;

        /// <summary>
        ///     The KeyFamily used by the dataset
        /// </summary>
        private IDataStructureObject _keyFamilyBean;

        /// <summary>
        ///     The optional internal field used to store the namespace Uri
        /// </summary>
        private string _namespace;

        /// <summary>
        ///     The optional internal field used to store the namespace prefix
        /// </summary>
        private string _namespacePrefix;

        /// <summary>
        ///     The namespace count
        /// </summary>
        private int _prefixCount = 1;

        /// <summary>
        ///     A value indicating whether the
        ///     <see cref="StartDataset" />
        ///     has been called
        /// </summary>
        private bool _startedDataSet;

        /// <summary>
        ///     A value indicating whether the header has been written
        /// </summary>
        private bool _startedHeader;

        /// <summary>
        ///    Contains a mapping from component id - to concept id
        /// </summary>
        private IDictionary<string, string> _componentIdMapping = new Dictionary<string, string>(StringComparer.Ordinal);

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStreamWriterBase"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="schema">
        /// The schema.
        /// </param>
        protected DataStreamWriterBase(XmlWriter writer, SdmxSchema schema)
            : this(writer, CreateDataNamespaces(schema.EnumType), schema)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStreamWriterBase"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        /// <param name="schema">
        /// The schema.
        /// </param>
        protected DataStreamWriterBase(XmlWriter writer, SdmxNamespaces namespaces, SdmxSchema schema)
            : base(writer, namespaces, schema)
        {
            Validate(namespaces);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the comment to add to the generated file
        /// </summary>
        public string GeneratedFileComment
        {
            get
            {
                return this._generatedFileComment;
            }

            set
            {
                this._generatedFileComment = value;
            }
        }

        /// <summary>
        ///     Gets or sets the namespace uri used by the generated sdmx file
        /// </summary>
        public string Namespace
        {
            get
            {
                return this._namespace;
            }

            set
            {
                this._namespace = value;
            }
        }

        /// <summary>
        ///     Gets or sets the namespace prefix used by the generated sdmx file
        /// </summary>
        public string NamespacePrefix
        {
            get
            {
                return this._namespacePrefix;
            }

            set
            {
                this._namespacePrefix = value;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the data format namespace Suffix e.g. compact
        /// </summary>
        protected abstract BaseDataFormatEnumType DataFormatType { get; }

        /// <summary>
        ///     Gets the default observation value to use in case it is null or empty
        /// </summary>
        protected string DefaultObs
        {
            get
            {
                return this._defaultObs;
            }
        }

        /// <summary>
        ///     Gets the dimension at observation.
        /// </summary>
        protected virtual string DimensionAtObservation
        {
            get
            {
                return this._dimensionAtObservation;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this is a delete message
        /// </summary>
        protected bool IsDeleteMessage
        {
            get
            {
                return this._isDeleteMessage;
            }
        }

        /// <summary>
        ///     Gets or sets the SDMX KeyFamily
        /// </summary>
        protected IDataStructureObject KeyFamily
        {
            get
            {
                return this._keyFamilyBean;
            }

            set
            {
                this._keyFamilyBean = value;
            }
        }

        /// <summary>
        ///     Gets the Message element tag
        /// </summary>
        protected abstract string MessageElement { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether the
        ///     <see cref="DataWriterEngineBase.StartDataset" />
        ///     has been called
        /// </summary>
        protected bool StartedDataSet
        {
            get
            {
                return this._startedDataSet;
            }

            set
            {
                this._startedDataSet = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Starts a dataset with the data conforming to the DSD
        /// </summary>
        /// <param name="dataflow">
        /// dataflow (optional) The dataflow can be provided to give extra information about the dataset
        /// </param>
        /// <param name="dataStructure">
        /// The <see cref="IDataStructureObject"/> for which the dataset will be created
        /// </param>
        /// <param name="header">
        /// The <see cref="IDatasetHeader"/> of the dataset
        /// </param>
        /// <param name="annotations">
        /// The annotations.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// if the <paramref name="dataflow"/> is null
        /// </exception>
        public virtual void StartDataset(IDataflowObject dataflow, IDataStructureObject dataStructure, IDatasetHeader header, params IAnnotation[] annotations)
        {
            if (dataStructure == null)
            {
                throw new ArgumentNullException("dataStructure");
            }

            this._componentIdMapping = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (IComponent currentComponent in dataStructure.Components)
            {
                this._componentIdMapping[currentComponent.Id] = this.GetComponentId(currentComponent);
            }

            this._keyFamilyBean = dataStructure;

            if (!this._startedHeader)
            {
                this._dimensionAtObservation = this.GetDimensionAtObservation(header);

                this.WriteMessageTag();

                var headerWriter = new HeaderWriter(this.SdmxMLWriter, this.Namespaces, this.TargetSchema);

                headerWriter.WriteHeader(this._header, this._dimensionAtObservation, dataStructure);
                this.ParseAction(this._header);
                if (this.IsDeleteMessage)
                {
                    this._defaultObs = null;
                }

                this._startedHeader = true;
            }

            this.WriteFormatDataSet(header);
            this._startedDataSet = true;
        }

        /// <summary>
        /// Write the specified <paramref name="header"/>
        /// </summary>
        /// <param name="header">
        /// The SDMX header.
        /// </param>
        public virtual void WriteHeader(IHeader header)
        {
            this._header = header;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Build the DSD specific URN and set <see cref="_namespace"/>
        /// </summary>
        /// <param name="namespacePrefix">
        /// The namespace prefix
        /// </param>
        /// <returns>
        /// The <see cref="NamespacePrefixPair"/>.
        /// </returns>
        protected virtual NamespacePrefixPair BuildDSDSpecificUrn(string namespacePrefix)
        {
            //// TODO if namespaces.UseEstatUrn is set we use the ESTAT Registry way.. 
            if (this.DimensionAtObservation == null)
            {
                this.Namespaces.UseEstatUrn = true;
            }

            if (this.TargetSchema.EnumType != SdmxSchemaEnumType.VersionTwoPointOne && this.Namespaces.UseEstatUrn)
            {
                this._namespace = string.Format(
                    CultureInfo.InvariantCulture, 
                    "{0}{1}:{2}:{3}:{4}", 
                    "urn:estat:sdmx.infomodel.keyfamily.KeyFamily=", 
                    this._keyFamilyBean.AgencyId, 
                    this._keyFamilyBean.Id, 
                    this._keyFamilyBean.Version, 
                    GetDataFormat(this.DataFormatType));
            }
            else
            {
                // TODO explict measure possibly in a different implementation ?
                this._namespace = string.Format(
                    CultureInfo.InvariantCulture, 
                    "{0}:ObsLevelDim:{1}", 
                    this._keyFamilyBean.Urn, 
                    this.DimensionAtObservation);
            }

            return new NamespacePrefixPair(this._namespace, this._namespacePrefix);
        }

        /// <summary>
        /// Checks if Dataset has started
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="StartDataset"/>
        ///     has not been called
        /// </exception>
        protected void CheckDataSet(string message)
        {
            if (!this._startedDataSet)
            {
                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        ///     Close the root *Data tag
        /// </summary>
        protected void CloseMessageTag()
        {
            this.WriteEndElement();
            this.WriteEndDocument();
        }

        /// <summary>
        ///     Conditionally end DataSet element
        /// </summary>
        protected virtual void EndDataSet()
        {
            if (this._startedDataSet)
            {
                this.WriteEndElement();
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
        protected virtual string GetDimensionAtObservation(IDatasetHeader header)
        {
            string dimensionAtObservation = DimensionObject.TimeDimensionFixedId;
            if (header != null && this.TargetSchema.EnumType == SdmxSchemaEnumType.VersionTwoPointOne
                && (header.DataStructureReference != null
                    && !string.IsNullOrWhiteSpace(header.DataStructureReference.DimensionAtObservation)))
            {
                dimensionAtObservation = header.DataStructureReference.DimensionAtObservation;
            }

            return dimensionAtObservation;
        }

        /// <summary>
        /// Gets schema location for the Structure Specific datasets; otherwise returns an empty string
        /// </summary>
        /// <param name="sdmxSchema">
        /// The sdmx schema.
        /// </param>
        /// <returns>
        /// The schema location for the Structure Specific datasets; otherwise returns an empty string
        /// </returns>
        protected virtual string GetSchemaLocation(SdmxSchemaEnumType sdmxSchema)
        {
            return string.Empty;
        }

        /// <summary>
        /// Parse the Header.DataSetAction and update the <see cref="IsDeleteMessage"/>
        /// </summary>
        /// <param name="header">
        /// The SDMX Header
        /// </param>
        protected void ParseAction(IHeader header)
        {
            this._isDeleteMessage = header != null && header.Action != null
                                    && header.Action.EnumType == DatasetActionEnumType.Delete;
        }

        /// <summary>
        /// Writes the DataSet element
        /// </summary>
        /// <param name="header">
        /// The dataset header
        /// </param>
        protected abstract void WriteFormatDataSet(IDatasetHeader header);

        /// <summary>
        /// Writes the dataset attributes.
        /// </summary>
        /// <param name="header">
        /// The dataset header.
        /// </param>
        protected void WriteDataSetHeader(IDatasetHeader header)
        {
            // populate dataset header
            if (header != null)
            {
                this.TryWriteAttribute(AttributeNameTable.publicationPeriod, header.PublicationPeriod);

                // PublicationYear seems to default to -1.
                if (header.PublicationYear > 0)
                {
                    this.TryWriteAttribute(AttributeNameTable.publicationYear, header.PublicationYear);
                }

                this.TryWriteAttribute(AttributeNameTable.validFrom, header.ValidFrom);
                this.TryWriteAttribute(AttributeNameTable.validTo, header.ValidTo);

                //// TODO MessageGroup support
            }

            if (this.TargetSchema.EnumType == SdmxSchemaEnumType.VersionTwoPointOne)
            {
                switch (this.DataFormatType)
                {
                    case BaseDataFormatEnumType.Compact:
                        this.WriteAttributeString(
                            this.Namespaces.StructureSpecific21, AttributeNameTable.dataScope, "DataStructure");
                        this.WriteAttributeString(
                            this.Namespaces.Xsi, 
                            AttributeNameTable.type, 
                            this.Namespaces.DataSetStructureSpecific.Prefix + ":DataSetType");
                        this.WriteAttributeString(
                            this.Namespaces.StructureSpecific21, AttributeNameTable.structureRef, GetRef(this.KeyFamily));
                        break;
                    case BaseDataFormatEnumType.Generic:
                        this.WriteAttributeString(AttributeNameTable.structureRef, GetRef(this.KeyFamily));
                        break;
                }
            }
        }

        /// <summary>
        ///     This method is used to write the root xml tag and *Data tag
        ///     with their corresponding attributes
        /// </summary>
        protected void WriteMessageTag()
        {
            this.WriteMessageTag(this.MessageElement, this.Namespaces);
        }

        /// <summary>
        /// Returns the id that should be output for a component Id.  For a 1.0 and 2.0 data message this is the Id of the Concept.
        /// </summary>
        /// <param name="componentId">
        /// The component id.
        /// </param>
        /// <returns>
        /// The id that should be output for a component Id.
        /// </returns>
        protected string GetComponentId(string componentId)
        {
            if (this.IsTwoPointOne)
            {
                return componentId;
            }

            string value;
            if (this._componentIdMapping.TryGetValue(componentId, out value))
            {
                return value;
            }

            return componentId;
        }

        /// <summary>
        /// Gets the component identifier.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <returns>The component identifier.</returns>
        protected string GetComponentId(IComponent component)
        {
            if (component == null)
            {
                return null;
            }

            if (this.IsTwoPointOne)
            {
                return component.Id;
            }

            return ConceptRefUtil.GetConceptId(component.ConceptRef);
        }

        /// <summary>
        /// This method is used to write the root xml tag and *Data tag
        ///     with their corresponding attributes
        /// </summary>
        /// <param name="element">
        /// The first element tag name
        /// </param>
        /// <param name="namespaces">
        /// The namespaces used by sdmx that are
        ///     appended by this method as attributes of the *Data tag
        /// </param>
        protected void WriteMessageTag(string element, SdmxNamespaces namespaces)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            // <?xml version="1.0" encoding="UTF-8"?>
            if (!this.Wrapped)
            {
                this.SdmxMLWriter.WriteStartDocument();
            }

            // Generated File comment
            if (!string.IsNullOrEmpty(this._generatedFileComment))
            {
                this.SdmxMLWriter.WriteComment(this._generatedFileComment);
            }

            // <CompactData
            this.WriteStartElement(this.Namespaces.Message, element);

            // xmlns="http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message"
            this.WriteNamespaceDecl(this.Namespaces.Message);
            if (string.IsNullOrEmpty(this._namespacePrefix))
            {
                this._namespacePrefix = string.Format(
                    CultureInfo.InvariantCulture, "{0}{1}", PrefixConstants.DataSetStructureSpecific, this._prefixCount);
                this._prefixCount++;
            }

            switch (this.DataFormatType)
            {
                case BaseDataFormatEnumType.Generic:
                    this._dataSetNS = this.Namespaces.Generic;
                    break;
                case BaseDataFormatEnumType.Compact:
                case BaseDataFormatEnumType.CrossSectional:
                    this._dataSetNS = this._namespace == null
                                          ? this.BuildDSDSpecificUrn(this._namespacePrefix)
                                          : new NamespacePrefixPair(this._namespace, this._namespacePrefix);
                    this.Namespaces.DataSetStructureSpecific = this._dataSetNS;
                    break;
            }

            this.WriteNamespaceDecl(this._dataSetNS);

            // xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
            this.WriteNamespaceDecl(this.Namespaces.Xsi);

            // xsi:schemaLocation="http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message SDMXMessage.xsd">
            string schemaLocation = this.Namespaces.SchemaLocation ?? string.Empty;
            string structureSpecific = this.GetSchemaLocation(this.TargetSchema.EnumType);
            if (!string.IsNullOrWhiteSpace(structureSpecific))
            {
                schemaLocation = string.Format(
                    CultureInfo.InvariantCulture, "{0} {1}", schemaLocation, structureSpecific);
            }

            if (!string.IsNullOrWhiteSpace(schemaLocation))
            {
                this.WriteAttributeString(this.Namespaces.Xsi, XmlConstants.SchemaLocation, schemaLocation);
            }
        }

        /// <summary>
        /// The create namespaces.
        /// </summary>
        /// <param name="sdmxSchema">
        /// The SDMX version
        /// </param>
        /// <returns>
        /// The <see cref="SdmxNamespaces"/>.
        /// </returns>
        private static SdmxNamespaces CreateDataNamespaces(SdmxSchemaEnumType sdmxSchema)
        {
            var namespaces = new SdmxNamespaces
                                 {
                                     Xsi =
                                         new NamespacePrefixPair(
                                         XmlConstants.XmlSchemaNS, XmlConstants.XmlSchemaPrefix)
                                 };
            switch (sdmxSchema)
            {
                case SdmxSchemaEnumType.VersionOne:
                    namespaces.Common = new NamespacePrefixPair(SdmxConstants.CommonNs10, PrefixConstants.Common);
                    namespaces.Message = new NamespacePrefixPair(SdmxConstants.MessageNs10, string.Empty);
                    namespaces.Generic = new NamespacePrefixPair(SdmxConstants.GenericNs10, PrefixConstants.Generic);
                    namespaces.SchemaLocation = string.Format(
                        CultureInfo.InvariantCulture, "{0} SDMXMessage.xsd", SdmxConstants.MessageNs10);
                    break;
                case SdmxSchemaEnumType.VersionTwo:
                    namespaces.Common = new NamespacePrefixPair(SdmxConstants.CommonNs20, PrefixConstants.Common);
                    namespaces.Message = new NamespacePrefixPair(SdmxConstants.MessageNs20, string.Empty);
                    namespaces.Generic = new NamespacePrefixPair(SdmxConstants.GenericNs20, PrefixConstants.Generic);
                    namespaces.SchemaLocation = string.Format(
                        CultureInfo.InvariantCulture, "{0} SDMXMessage.xsd", SdmxConstants.MessageNs20);
                    break;
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    namespaces.Common = new NamespacePrefixPair(SdmxConstants.CommonNs21, PrefixConstants.Common);
                    namespaces.Message = new NamespacePrefixPair(SdmxConstants.MessageNs21, PrefixConstants.Message);
                    namespaces.Generic = new NamespacePrefixPair(SdmxConstants.GenericNs21, PrefixConstants.Generic);
                    namespaces.StructureSpecific21 = new NamespacePrefixPair(
                        SdmxConstants.StructureSpecificNs21, PrefixConstants.StructureSpecific21);
                    namespaces.SchemaLocation = string.Format(
                        CultureInfo.InvariantCulture, "{0} SDMXMessage.xsd", SdmxConstants.MessageNs21);
                    break;
            }

            return namespaces;
        }

        /// <summary>
        /// Convert <see cref="BaseDataFormatEnumType"/> field to lower
        /// </summary>
        /// <param name="type">
        /// The  <see cref="BaseDataFormatEnumType"/> field
        /// </param>
        /// <returns>
        /// The  <see cref="BaseDataFormatEnumType"/> field as string
        /// </returns>
        private static string GetDataFormat(BaseDataFormatEnumType type)
        {
            switch (type)
            {
                case BaseDataFormatEnumType.Compact:
                    return "compact";
                case BaseDataFormatEnumType.CrossSectional:
                    return "cross";
            }

            return null;
        }

        /// <summary>
        /// The validate.
        /// </summary>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="namespaces"/> is not configured correctly
        /// </exception>
        private static void Validate(SdmxNamespaces namespaces)
        {
            if (namespaces == null)
            {
                throw new ArgumentNullException("namespaces");
            }

            if (namespaces.Message == null || namespaces.Common == null || namespaces.Xsi == null
                || (namespaces.DataSetStructureSpecific == null && namespaces.Generic == null))
            {
                throw new ArgumentException(
                    "One or more of the required namespaces are not set. Please set Message, Common, Xsi and either DataSetStructureSpecific or Generic namespaces. Else use another ctor to use the defaults.");
            }
        }

        #endregion
    }
}