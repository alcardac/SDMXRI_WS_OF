// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderWriter.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   This class is used by all the other message writers classes to write the Sdmx Header
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxParseBase.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.Util.Date;

    /// <summary>
    ///     This class is used by all the other message writers classes to write the Sdmx Header
    /// </summary>
    /// <remarks>
    ///     This class was copied from DataGenerator
    /// </remarks>
    public class HeaderWriter : Writer
    {
        /// <summary>
        /// The _retrieval manager
        /// </summary>
        private readonly IHeaderRetrievalManager _retrievalManager;

        #region Constants

        /// <summary>
        ///     The default sender id.
        /// </summary>
        private const string DefaultSenderId = "ESTAT";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderWriter"/> class.
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
        public HeaderWriter(XmlWriter writer, SdmxNamespaces namespaces, SdmxSchema schema)
            : base(writer, namespaces, schema)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderWriter" /> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="namespaces">The namespaces.</param>
        /// <param name="schema">The schema.</param>
        /// <param name="retrievalManager">The header retrieval manager.</param>
        public HeaderWriter(XmlWriter writer, SdmxNamespaces namespaces, SdmxSchema schema, IHeaderRetrievalManager retrievalManager)
            : base(writer, namespaces, schema)
        {
            this._retrievalManager = retrievalManager;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the default namespace
        /// </summary>
        protected override NamespacePrefixPair DefaultNS
        {
            get
            {
                return this.Namespaces.Message;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// This is the main method of the class that writes the <see cref="IHeader"/>
        ///     The methods write the xml Header tag and add it's attributes.
        /// </summary>
        /// <param name="header">
        /// The <see cref="IHeader"/> object containing the header data to be written
        /// </param>
        public void WriteHeader(IHeader header)
        {
            this.WriteHeader(header, null);
        }

        /// <summary>
        /// This is the main method of the class that writes the <see cref="IHeader"/>
        ///     The methods write the xml Header tag and add it's attributes.
        /// </summary>
        /// <param name="header">
        /// The <see cref="IHeader"/> object containing the header data to be written
        /// </param>
        /// <param name="dimensionAtObservation">
        /// The dimension At Observation.
        /// </param>
        /// <param name="dataStructureObjects">
        /// The data Structure Objects.
        /// </param>
        public void WriteHeader(
            IHeader header, string dimensionAtObservation, params IDataStructureObject[] dataStructureObjects)
        {
            IHeader providedHeader = header;
            SdmxSchemaEnumType sdmxSchema = this.TargetSchema.EnumType;

            // TODO split to v2 and v2.1 to reduce complexity and more readable
            if (header == null)
            {
                header = this._retrievalManager != null ? this._retrievalManager.Header : new HeaderImpl(string.Format("IDREF{0}", DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture)), DefaultSenderId);
            }

            // start header
            this.WriteStartElement(this.Namespaces.Message, ElementNameTable.Header);

            // id
            this.TryToWriteElement(this.Namespaces.Message, ElementNameTable.ID, header.Id);

            // test
            this.WriteElement(this.Namespaces.Message, ElementNameTable.Test, header.Test);

            // only for (2.0)
            if (sdmxSchema != SdmxSchemaEnumType.VersionTwoPointOne)
            {
                // truncated (2.0)
                string elementName = NameTableCache.GetElementName(ElementNameTable.Truncated);
                bool isTruncated = header.HasAdditionalAttribtue(elementName)
                                   && bool.TrueString.Equals(header.GetAdditionalAttribtue(elementName));
                this.TryToWriteElement(this.Namespaces.Message, ElementNameTable.Truncated, isTruncated);

                // names (2.0)
                this.WriteTextType(this.Namespaces.Message, header.Name, ElementNameTable.Name);
            }

            // prepared
            DateTime prepared = header.Prepared.HasValue ? header.Prepared.Value : DateTime.Now;
            this.TryToWriteElement(this.Namespaces.Message, ElementNameTable.Prepared, DateUtil.FormatDate(prepared));

            // sender TODO ensure/check that Sender cannot be null
            this.WritePartyType(header.Sender, ElementNameTable.Sender);

            // receiver TODO ensure/check that Receiver cannot be null
            foreach (IParty text in header.Receiver)
            {
                this.WritePartyType(text, ElementNameTable.Receiver);
            }

            // only for 2.1
            if (sdmxSchema == SdmxSchemaEnumType.VersionTwoPointOne)
            {
                // names (2.1)
                this.WriteTextType(this.Namespaces.Common, header.Name, ElementNameTable.Name);

                // structures (2.1)
                foreach (IDataStructureObject datasetStructure in dataStructureObjects)
                {
                    // start structure  (message ns)
                    this.WriteStartElement(this.Namespaces.Message, ElementNameTable.Structure);

                    // write structureId attribute
                    this.WriteAttributeString(AttributeNameTable.structureID, GetRef(datasetStructure));

                    // structure specific namespace attribute
                    if (this.Namespaces.DataSetStructureSpecific != null)
                    {
                        this.WriteAttributeString(
                            AttributeNameTable.@namespace, this.Namespaces.DataSetStructureSpecific.NS);
                    }

                    // dimension at observation attribute
                    string dimensionAtObs = dimensionAtObservation ?? DimensionObject.TimeDimensionFixedId;
                    this.WriteAttributeString(AttributeNameTable.dimensionAtObservation, dimensionAtObs);

                    // start structure (common ns)
                    this.WriteStartElement(this.Namespaces.Common, ElementNameTable.Structure);

                    // start Ref
                    this.WriteStartElement(ElementNameTable.Ref);
                    this.WriteAttributeString(AttributeNameTable.agencyID, datasetStructure.AgencyId);
                    this.WriteAttributeString(AttributeNameTable.id, datasetStructure.Id);
                    this.WriteAttributeString(AttributeNameTable.version, datasetStructure.Version);

                    // end Ref
                    this.WriteEndElement();

                    // end structure (common ns)
                    this.WriteEndElement();

                    // end structure (message ns)
                    this.WriteEndElement();
                }
            }
            else if (dataStructureObjects.Length == 1)
            {
                IDataStructureObject dsd = dataStructureObjects[0];

                // keyfamily ref (2.0)
                this.TryToWriteElement(this.Namespaces.Message, ElementNameTable.KeyFamilyRef, dsd.Id);

                // keyfamily agency (2.0)
                this.TryToWriteElement(this.Namespaces.Message, ElementNameTable.KeyFamilyAgency, dsd.AgencyId);
            }

            if (providedHeader != null)
            {
                if (sdmxSchema == SdmxSchemaEnumType.VersionTwoPointOne)
                {
                    if (header.DataProviderReference != null)
                    {
                        IMaintainableRefObject refObject = header.DataProviderReference.MaintainableReference;
                        if (string.IsNullOrWhiteSpace(refObject.AgencyId)
                            && string.IsNullOrWhiteSpace(refObject.MaintainableId))
                        {
                            // start DataProvider
                            this.WriteStartElement(this.Namespaces.Message, ElementNameTable.DataProvider);

                            // start Ref
                            // TODO need constant
                            this.WriteStartElement(ElementNameTable.Ref);
                            this.WriteAttributeString(AttributeNameTable.agencyID, refObject.AgencyId);
                            this.WriteAttributeString(AttributeNameTable.id, refObject.MaintainableId);
                            string value = string.IsNullOrWhiteSpace(refObject.Version)
                                               ? refObject.Version
                                               : MaintainableObject.DefaultVersion;
                            this.WriteAttributeString(AttributeNameTable.version, value);

                            // end Ref
                            this.WriteEndElement();

                            // end DataProvider
                            this.WriteEndElement();
                        }
                    }

                    if (header.Action != null)
                    {
                        // datasetaction (2.0) - NOTE in java 0.9.4 because they use buffering, if null they retrieve this info from dataset header. We can't
                        // TODO check also DataSetHeader 
                        this.TryToWriteElement(
                            this.Namespaces.Message, ElementNameTable.DataSetAction, header.Action.Action);
                    }

                    // datasetId (2.1) - NOTE in java 0.9.4 because they use buffering, if null they retrieve this info from dataset header. We can't
                    // TODO check also DataSetHeader 
                    this.TryToWriteElement(this.Namespaces.Message, ElementNameTable.DataSetID, header.DatasetId);
                }
                else
                {
                    // dataset agency (2.0) 
                    string dataSetAgency = NameTableCache.GetElementName(ElementNameTable.DataSetAgency);
                    if (providedHeader.HasAdditionalAttribtue(dataSetAgency))
                    {
                        this.TryToWriteElement(
                            this.Namespaces.Message, 
                            ElementNameTable.DataSetAgency, 
                            header.GetAdditionalAttribtue(dataSetAgency));
                    }

                    // datasetId (2.0) - NOTE in java 0.9.4 because they use buffering, if null they retrieve this info from dataset header. We can't
                    this.TryToWriteElement(this.Namespaces.Message, ElementNameTable.DataSetID, header.DatasetId);
                    if (header.Action != null)
                    {
                        // datasetaction (2.0) - NOTE in java 0.9.4 because they use buffering, if null they retrieve this info from dataset header. We can't
                        this.TryToWriteElement(
                            this.Namespaces.Message, ElementNameTable.DataSetAction, header.Action.Action);
                    }
                }

                // extracted
                this.TryToWriteElement(this.Namespaces.Message, ElementNameTable.Extracted, header.Extracted);

                // report begin
                this.TryToWriteElement(this.Namespaces.Message, ElementNameTable.ReportingBegin, header.ReportingBegin);

                // report end
                this.TryToWriteElement(this.Namespaces.Message, ElementNameTable.ReportingEnd, header.ReportingEnd);

                if (sdmxSchema == SdmxSchemaEnumType.VersionTwoPointOne)
                {
                    // embargo date (2.1)
                    this.TryToWriteElement(this.Namespaces.Message, ElementNameTable.EmbargoDate, header.EmbargoDate);
                }

                // source
                this.WriteTextType(this.Namespaces.Message, header.Source, ElementNameTable.Source);
            }

            this.WriteEndElement();
        }

        #endregion

        #region Methods

        /// <summary>
        /// This is an internal method that is used to write a <see cref="IContact"/>
        ///     The method creates a xml Contact element
        /// </summary>
        /// <param name="contactObj">
        /// The <see cref="IContact"/> object containing the data to be written
        /// </param>
        private void WriteContactType(IContact contactObj)
        {
            this.WriteStartElement(this.Namespaces.Message, ElementNameTable.Contact);
            this.WriteTextType(this.NameNamespace, contactObj.Name, ElementNameTable.Name);
            this.WriteTextType(this.Namespaces.Message, contactObj.Departments, ElementNameTable.Department);
            this.WriteTextType(this.Namespaces.Message, contactObj.Role, ElementNameTable.Role);

            this.WriteListContacts(this.Namespaces.Message, ElementNameTable.Telephone, contactObj.Telephone);
            this.WriteListContacts(this.Namespaces.Message, ElementNameTable.Fax, contactObj.Fax);
            this.WriteListContacts(this.Namespaces.Message, ElementNameTable.X400, contactObj.X400);
            this.WriteListContacts(this.Namespaces.Message, ElementNameTable.URI, contactObj.Uri);
            this.WriteListContacts(this.Namespaces.Message, ElementNameTable.Email, contactObj.Email);
            this.WriteEndElement();
        }

        /// <summary>
        /// Write the list contacts.
        /// </summary>
        /// <param name="namespacePrefixPair">
        /// The namespace prefix pair.
        /// </param>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="list">
        /// The list.
        /// </param>
        private void WriteListContacts(
            NamespacePrefixPair namespacePrefixPair, ElementNameTable element, IEnumerable<string> list)
        {
            foreach (string value in list)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    this.WriteElement(namespacePrefixPair, element, value);
                }
            }
        }

        /// <summary>
        /// This is an internal method that is used to write the specified <paramref name="partyObj"/>
        /// </summary>
        /// <param name="partyObj">
        /// The <see cref="IParty"/> object containing the data to be written
        /// </param>
        /// <param name="name">
        /// The name of the xml element
        /// </param>
        private void WritePartyType(IParty partyObj, ElementNameTable name)
        {
            this.WriteStartElement(this.Namespaces.Message, name);

            this.TryWriteAttribute(AttributeNameTable.id, partyObj.Id);
            this.WriteTextType(this.NameNamespace, partyObj.Name, ElementNameTable.Name);

            foreach (IContact contact in partyObj.Contacts)
            {
                this.WriteContactType(contact);
            }

            this.WriteEndElement();
        }

        #endregion
    }
}