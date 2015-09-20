// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataHeaderReaderEngine.cs" company="Eurostat">
//   Date Created : 2014-07-16
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The data header reader engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.DataParser.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml;

    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util.Url;

    /// <summary>
    ///     The data header reader engine.
    /// </summary>
    public class DataHeaderRetrievalManager : IHeaderRetrievalManager
    {
        #region Fields

        /// <summary>
        ///     The default data structure object
        /// </summary>
        private readonly IDataStructureObject _defaultDataStructureObject;

        /// <summary>
        ///     The parser
        /// </summary>
        private readonly XmlReader _parser;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataHeaderRetrievalManager"/> class.
        /// </summary>
        /// <param name="parser">
        /// The parser.
        /// </param>
        /// <param name="defaultDataStructureObject">
        /// The default data structure object.
        /// </param>
        public DataHeaderRetrievalManager(XmlReader parser, IDataStructureObject defaultDataStructureObject)
        {
            this._parser = parser;
            this._defaultDataStructureObject = defaultDataStructureObject;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets a header object
        /// </summary>
        /// <value> </value>
        public IHeader Header
        {
            get
            {
                return ProcessHeader(this._parser, this._defaultDataStructureObject);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Processes the header.
        /// </summary>
        /// <param name="parser">
        /// The parser.
        /// </param>
        /// <param name="defaultDataStructureObject">
        /// The default data structure object.
        /// </param>
        /// <returns>
        /// The <see cref="IHeader"/>.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// DataSet does not contain a Header
        /// </exception>
        private static IHeader ProcessHeader(XmlReader parser, IDataStructureObject defaultDataStructureObject)
        {
            var additionalAttributes = new Dictionary<string, string>(StringComparer.Ordinal);
            IStructureReference dataProviderReference = null;
            var structure = new List<IDatasetStructureReference>();

            DatasetAction datasetAction = null;
            string id = null;
            string datasetId = null;
            DateTime? embargoDate = null;
            DateTime? extracted = null;
            DateTime? prepared = null;
            DateTime? reportingBegin = null;
            DateTime? reportingEnd = null;
            var name = new List<ITextTypeWrapper>();
            var source = new List<ITextTypeWrapper>();
            var receiver = new List<IParty>();
            IParty sender = null;
            bool test = false;
            string dsdId = null;
            string dsdAgency = null;
            string text = null;
            string xmlLang = null;
            while (parser.Read())
            {
                if (parser.NodeType == XmlNodeType.Element)
                {
                    string nodeName = parser.LocalName;
                    xmlLang = parser.XmlLang;
                    ElementNameTable elementName;
                    if (Enum.TryParse(nodeName, out elementName))
                    {
                        switch (elementName)
                        {
                            case ElementNameTable.Sender:
                                sender = ProcessParty(elementName, parser);
                                break;
                            case ElementNameTable.Receiver:
                                receiver.Add(ProcessParty(elementName, parser));
                                break;
                            case ElementNameTable.Structure:
                                structure.Add(ProcessStructure(parser));
                                break;
                            case ElementNameTable.DataProvider:
                                dataProviderReference = ParseStructureReference(elementName, SdmxStructureEnumType.DataProvider, parser);
                                break;
                        }
                    }
                }
                else if (parser.NodeType == XmlNodeType.Text)
                {
                    text = parser.Value;
                }
                else if (parser.NodeType == XmlNodeType.EndElement)
                {
                    string nodeName = parser.LocalName;
                    ElementNameTable elementName;
                    if (Enum.TryParse(nodeName, out elementName))
                    {
                        switch (elementName)
                        {
                            case ElementNameTable.ID:
                                id = text;
                                break;
                            case ElementNameTable.Test:
                                test = XmlConvert.ToBoolean(text);
                                break;
                            case ElementNameTable.Prepared:
                                prepared = XmlConvert.ToDateTime(text, XmlDateTimeSerializationMode.RoundtripKind);
                                break;
                            case ElementNameTable.Name:
                                AddItemToLang(name, text, xmlLang);
                                break;
                            case ElementNameTable.DataSetAction:
                                datasetAction = DatasetAction.GetAction(text);
                                break;
                            case ElementNameTable.DataSetID:
                                datasetId = text;
                                break;
                            case ElementNameTable.Extracted:
                                extracted = XmlConvert.ToDateTime(text, XmlDateTimeSerializationMode.RoundtripKind);
                                break;
                            case ElementNameTable.ReportingBegin:
                                reportingBegin = XmlConvert.ToDateTime(text, XmlDateTimeSerializationMode.RoundtripKind);
                                break;
                            case ElementNameTable.ReportingEnd:
                                reportingEnd = XmlConvert.ToDateTime(text, XmlDateTimeSerializationMode.RoundtripKind);
                                break;
                            case ElementNameTable.EmbargoDate:
                                embargoDate = XmlConvert.ToDateTime(text, XmlDateTimeSerializationMode.RoundtripKind);
                                break;
                            case ElementNameTable.Source:
                                AddItemToLang(source, text, xmlLang);
                                break;
                            case ElementNameTable.KeyFamilyAgency:
                                dsdAgency = text;
                                break;
                            case ElementNameTable.KeyFamilyRef:
                                dsdId = text;
                                break;
                            case ElementNameTable.Header:
                                if (!string.IsNullOrWhiteSpace(dsdId))
                                {
                                    if (defaultDataStructureObject != null && dsdId.Equals(defaultDataStructureObject.Id))
                                    {
                                        structure.Add(new DatasetStructureReferenceCore(defaultDataStructureObject.AsReference));
                                    }
                                    else
                                    {
                                        structure.Add(new DatasetStructureReferenceCore(new StructureReferenceImpl(dsdAgency, dsdId, null, SdmxStructureEnumType.Dsd)));
                                    }
                                }

                                return new HeaderImpl(
                                    additionalAttributes, 
                                    structure, 
                                    dataProviderReference, 
                                    datasetAction, 
                                    id, 
                                    datasetId, 
                                    embargoDate, 
                                    extracted, 
                                    prepared, 
                                    reportingBegin, 
                                    reportingEnd, 
                                    name, 
                                    source, 
                                    receiver, 
                                    sender, 
                                    test);
                        }
                    }
                }
            }

            throw new ArgumentException("DataSet does not contain a Header");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the text type at current position to <paramref name="texts"/>
        /// </summary>
        /// <param name="texts">
        /// The localized text collection.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="lang">
        /// The language.
        /// </param>
        private static void AddItemToLang(ICollection<ITextTypeWrapper> texts, string value, string lang)
        {
            texts.Add(new TextTypeWrapperImpl(lang, value, null));
        }

        /// <summary>
        /// Parses the structure reference.
        /// </summary>
        /// <param name="rootElementName">
        /// Name of the root element.
        /// </param>
        /// <param name="sdmxStructureEnumType">
        /// Type of the SDMX structure.
        /// </param>
        /// <param name="parser">
        /// The parser.
        /// </param>
        /// <returns>
        /// The <see cref="IStructureReference"/>.
        /// </returns>
        /// <exception cref="SdmxSemmanticException">
        /// DataSet structure reference incomplete, missing agencyId
        ///     or
        ///     Dataset structure reference incomplete, missing maintainableParentId
        ///     or
        ///     Dataset Structure reference invalid, reference does not match <paramref name="sdmxStructureEnumType"/>
        ///     or
        ///     Dataset structure reference invalid, could not process reference, no Ref node or URN node found
        /// </exception>
        private static IStructureReference ParseStructureReference(ElementNameTable rootElementName, SdmxStructureEnumType sdmxStructureEnumType, XmlReader parser)
        {
            IStructureReference structureReference = null;
            string text = null;
            while (parser.Read())
            {
                if (parser.NodeType == XmlNodeType.Element)
                {
                    ElementNameTable elementName;
                    if (Enum.TryParse(parser.LocalName, out elementName))
                    {
                        switch (elementName)
                        {
                            case ElementNameTable.Ref:
                                {
                                    structureReference = ProcessStructureReferenceRef(sdmxStructureEnumType, parser);
                                }

                                break;
                        }
                    }
                }
                else if (parser.NodeType == XmlNodeType.Text)
                {
                    text = parser.Value;
                }
                else if (parser.NodeType == XmlNodeType.EndElement)
                {
                    string localname = parser.LocalName;
                    if (rootElementName.Is(localname))
                    {
                        return structureReference;
                    }

                    if (ElementNameTable.URN.Is(localname))
                    {
                        structureReference = ProcessStructureReferenceUrn(sdmxStructureEnumType, text);
                    }
                }
            }

            throw new SdmxSemmanticException("Dataset structure reference invalid, could not process reference, no Ref node or URN node found");
        }

        /// <summary>
        /// Processes the contact.
        /// </summary>
        /// <param name="parser">
        /// The parser.
        /// </param>
        /// <returns>
        /// The <see cref="IContact"/>.
        /// </returns>
        /// <exception cref="SdmxSyntaxException">
        /// End element Contact not found.
        /// </exception>
        private static IContact ProcessContact(XmlReader parser)
        {
            var name = new List<ITextTypeWrapper>();
            var role = new List<ITextTypeWrapper>();
            var departments = new List<ITextTypeWrapper>();
            var email = new List<string>();
            var fax = new List<string>();
            var telephone = new List<string>();
            var uri = new List<string>();
            var x400 = new List<string>();
            string text = null;
            string xmlLang = null;
            while (parser.Read())
            {
                if (parser.NodeType == XmlNodeType.Element)
                {
                    xmlLang = parser.XmlLang;
                }
                else if (parser.NodeType == XmlNodeType.Text)
                {
                    text = parser.Value;
                }
                else if (parser.NodeType == XmlNodeType.EndElement)
                {
                    string nodeName = parser.LocalName;
                    ElementNameTable elementName;
                    if (Enum.TryParse(nodeName, out elementName))
                    {
                        switch (elementName)
                        {
                            case ElementNameTable.Name:
                                AddItemToLang(name, text, xmlLang);
                                break;

                            case ElementNameTable.Role:
                                AddItemToLang(role, text, xmlLang);
                                break;
                            case ElementNameTable.Department:
                                AddItemToLang(departments, text, xmlLang);
                                break;
                            case ElementNameTable.Telephone:
                                telephone.Add(text);
                                break;
                            case ElementNameTable.Fax:
                                fax.Add(text);
                                break;
                            case ElementNameTable.X400:
                                x400.Add(text);
                                break;
                            case ElementNameTable.URI:
                                uri.Add(text);
                                break;
                            case ElementNameTable.Email:
                                email.Add(text);
                                break;
                            case ElementNameTable.Contact:
                                return new ContactCore(name, role, departments, email, fax, telephone, uri, x400);
                        }
                    }
                }
            }

            throw new SdmxSyntaxException("End element </Contact> not found.");
        }

        /// <summary>
        /// Processes the party.
        /// </summary>
        /// <param name="rootElementName">
        /// Name of the root element.
        /// </param>
        /// <param name="parser">
        /// The parser.
        /// </param>
        /// <returns>
        /// The <see cref="IParty"/>.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// Could not find end element specified at
        ///     <paramref name="rootElementName"/>
        /// </exception>
        private static IParty ProcessParty(ElementNameTable rootElementName, XmlReader parser)
        {
            bool isEmptyElement = parser.IsEmptyElement;
            string id = parser.GetAttribute(AttributeNameTable.id);
            string timeZone = null;
            var nameMap = new List<ITextTypeWrapper>();
            var contacts = new List<IContact>();
            string text = null;
            string xmlLang = null;
            if (!isEmptyElement)
            {
                while (parser.Read())
                {
                    if (parser.NodeType == XmlNodeType.Element)
                    {
                        xmlLang = parser.XmlLang;
                        ElementNameTable elementName;
                        if (Enum.TryParse(parser.LocalName, out elementName))
                        {
                            switch (elementName)
                            {
                                case ElementNameTable.Contact:
                                    contacts.Add(ProcessContact(parser));
                                    break;
                            }
                        }
                    }
                    else if (parser.NodeType == XmlNodeType.Text)
                    {
                        text = parser.Value;
                    }
                    else if (parser.NodeType == XmlNodeType.EndElement)
                    {
                        if (rootElementName.Is(parser.LocalName))
                        {
                            break;
                        }

                        ElementNameTable elementName;
                        if (Enum.TryParse(parser.LocalName, out elementName))
                        {
                            switch (elementName)
                            {
                                case ElementNameTable.Name:
                                    AddItemToLang(nameMap, text, xmlLang);
                                    break;

                                    // NOTE .NET uses the correct "Timezone" name. In Java it uses the incorrect "TimeZone" which does not exist! PLEASE double check with SDMX v2.1 SDMXMessage.xsd before changing.
                                case ElementNameTable.Timezone:
                                    timeZone = text;
                                    break;
                            }
                        }
                    }
                }
            }

            return new PartyCore(nameMap, id, contacts, timeZone);
        }

        /// <summary>
        /// Processes the structure at current position.
        /// </summary>
        /// <param name="xmlReader">
        /// The XML reader.
        /// </param>
        /// <returns>
        /// The <see cref="IDatasetStructureReference"/>.
        /// </returns>
        /// <exception cref="SdmxSyntaxException">
        /// Dataset Header.Structure expected to have one of the following nodes present
        ///     (ProvisionAgreement|StructureUsage|Structure) to reference either a provision agreement, dataflow, or data
        ///     structure definition
        /// </exception>
        private static IDatasetStructureReference ProcessStructure(XmlReader xmlReader)
        {
            string id = xmlReader.GetAttribute(AttributeNameTable.structureID);
            string serviceUrl = xmlReader.GetAttribute(AttributeNameTable.serviceURL);
            string structureUrl = xmlReader.GetAttribute(AttributeNameTable.structureURL);
            string dimensionAtObservation = xmlReader.GetAttribute(AttributeNameTable.dimensionAtObservation);

            IStructureReference structureReference = null;

            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    ElementNameTable elementName;
                    if (Enum.TryParse(xmlReader.LocalName, out elementName))
                    {
                        switch (elementName)
                        {
                            case ElementNameTable.ProvisionAgreement:
                                structureReference = ParseStructureReference(elementName, SdmxStructureEnumType.ProvisionAgreement, xmlReader);
                                break;
                            case ElementNameTable.StructureUsage:
                                structureReference = ParseStructureReference(elementName, SdmxStructureEnumType.Dataflow, xmlReader);
                                break;
                            case ElementNameTable.Structure:
                                structureReference = ParseStructureReference(elementName, SdmxStructureEnumType.Dsd, xmlReader);
                                break;
                        }
                    }
                }
                else if (xmlReader.NodeType == XmlNodeType.EndElement)
                {
                    if (ElementNameTable.Structure.Is(xmlReader.LocalName))
                    {
                        break;
                    }
                }
            }

            if (structureReference == null)
            {
                throw new SdmxSyntaxException(
                    "Dataset Header.Structure expected to have one of the following nodes present (ProvisionAgreement|StructureUsage|Structure) to reference either a provision agreement, dataflow, or data structure definition");
            }

            // Moved noSeries assignment after process Header.
            return new DatasetStructureReferenceCore(id, structureReference, serviceUrl.ToUri(), structureUrl.ToUri(), dimensionAtObservation);
        }

        /// <summary>
        /// Processes the structure reference preference.
        /// </summary>
        /// <param name="sdmxStructureEnumType">
        /// Type of the SDMX structure.
        /// </param>
        /// <param name="parser">
        /// The parser.
        /// </param>
        /// <returns>
        /// The <see cref="IStructureReference"/>.
        /// </returns>
        /// <exception cref="SdmxSemmanticException">
        /// DataSet structure reference incomplete, missing agencyId
        ///     or
        ///     Dataset structure reference incomplete, missing maintainableParentId
        /// </exception>
        private static IStructureReference ProcessStructureReferenceRef(SdmxStructureEnumType sdmxStructureEnumType, XmlReader parser)
        {
            string agencyId = parser.GetAttribute(AttributeNameTable.agencyID);
            string id = parser.GetAttribute(AttributeNameTable.id);
            string maintainableId = parser.GetAttribute(AttributeNameTable.maintainableParentID);
            string version = parser.GetAttribute(AttributeNameTable.version);
            if (string.IsNullOrWhiteSpace(agencyId))
            {
                throw new SdmxSemmanticException("DataSet structure reference incomplete, missing agencyId");
            }

            if (!SdmxStructureType.GetFromEnum(sdmxStructureEnumType).IsMaintainable)
            {
                if (!string.IsNullOrWhiteSpace(maintainableId))
                {
                    throw new SdmxSemmanticException("Dataset structure reference incomplete, missing maintainableParentId");
                }

                return new StructureReferenceImpl(agencyId, maintainableId, version, sdmxStructureEnumType, id);
            }

            return new StructureReferenceImpl(agencyId, id, version, sdmxStructureEnumType); // Maintainable Reference
        }

        /// <summary>
        /// Processes the structure reference urn.
        /// </summary>
        /// <param name="sdmxStructureEnumType">
        /// Type of the SDMX structure.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="IStructureReference"/>.
        /// </returns>
        /// <exception cref="SdmxSemmanticException">
        /// Dataset Structure reference invalid, reference does not match
        ///     <paramref name="sdmxStructureEnumType"/>
        /// </exception>
        private static IStructureReference ProcessStructureReferenceUrn(SdmxStructureEnumType sdmxStructureEnumType, string text)
        {
            IStructureReference structureReference = null;
            string urn = text;
            if (!string.IsNullOrWhiteSpace(urn))
            {
                structureReference = new StructureReferenceImpl(urn);
                if (structureReference.TargetReference != sdmxStructureEnumType)
                {
                    throw new SdmxSemmanticException(
                        string.Format(
                            CultureInfo.InvariantCulture, 
                            "Dataset Structure reference invalid '{0}' , expecting a reference to '{1}' but got '{2}'", 
                            urn, 
                            SdmxStructureType.GetFromEnum(sdmxStructureEnumType), 
                            structureReference.TargetReference.StructureType));
                }
            }

            return structureReference;
        }

        #endregion
    }
}