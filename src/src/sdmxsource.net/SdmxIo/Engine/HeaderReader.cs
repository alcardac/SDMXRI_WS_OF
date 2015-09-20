// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderReader.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   This class provides the method to read a SDMX Header for a SDMX Message and output a <see cref="IHeader" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxParseBase.Engine
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Helper;
    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     This class provides the method to read a SDMX Header for a SDMX Message and output a <see cref="IHeader" />
    /// </summary>
    public class HeaderReader : Reader
    {
        #region Fields

        /// <summary>
        ///     The stack of elements to parsed, used by xml parser.
        /// </summary>
        private readonly Stack<object> _elements = new Stack<object>();

        /// <summary>
        ///     The sdmx  message header object representation that is generated during the xml parsing
        /// </summary>
        private HeaderMutable _headerObject;

        /// <summary>
        ///     Current XmlLang scope
        /// </summary>
        private string _lang;

        /// <summary>
        ///     The current element text
        /// </summary>
        private string _text;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderReader"/> class.
        /// </summary>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        /// <param name="schema">
        /// The schema.
        /// </param>
        public HeaderReader(SdmxNamespaces namespaces, SdmxSchema schema)
            : base(namespaces, schema)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Parses from the XmlReader the Header element and the included elements and creates
        ///     from them a IHeader object. When the Header end element is reached it will return.
        /// </summary>
        /// <param name="textReader">
        /// The XmlReader to read the header from.
        /// </param>
        /// <returns>
        /// The IHeader object containing the information read from the XmlReader
        /// </returns>
        /// <remarks>
        /// This is the standalone version of <see cref="Read(XmlReader)"/>. It should not be called after reading the MessageType element. e.g. Structure, CompactData e.t.c. i.e. the root element
        /// </remarks>
        public IHeader Read(TextReader textReader)
        {
            using (XmlReader xreader = this.CreateXmlReader(textReader))
            {
                xreader.MoveToContent();
                this.Read(xreader);
            }

            return this._headerObject;
        }

        /// <summary>
        /// Parses from the XmlReader the Header element and the included elements and creates
        ///     from them a IHeader object. When the Header end element is reached it will return.
        /// </summary>
        /// <param name="reader">
        /// The XmlReader to read the header from.
        /// </param>
        /// <returns>
        /// The IHeader object containing the information read from the XmlReader
        /// </returns>
        /// <remarks>
        /// It needs to be called after reading the MessageType element. e.g. Structure, CompactData e.t.c. i.e. the root element
        /// </remarks>
        public IHeader Read(XmlReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            // initialise fields
            this._elements.Clear();
            this._headerObject = new HeaderMutable();
            this._text = null;

            // initialise local variables
            object parent = null;

            bool completed = false;
            while (!completed && reader.Read())
            {
                // if it is not completed , read next node
                object localName;
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            bool isEmpty = reader.IsEmptyElement;
                            localName = reader.LocalName;
                            if (this._elements.Count > 0)
                            {
                                parent = this._elements.Peek();
                            }

                            this.Attributes.Clear();
                            for (int i = 0; i < reader.AttributeCount; i++)
                            {
                                // NOTE, reader moves to attribute
                                reader.MoveToAttribute(i);
                                this.Attributes.Add(reader.Name, reader.Value);
                            }

                            this._lang = reader.XmlLang;
                            object current = this.HandleElement(parent, localName);
                            if (!isEmpty)
                            {
                                this._elements.Push(current);
                            }

                            break;
                        }

                    case XmlNodeType.EndElement:
                        {
                            if (this._elements.Count > 0)
                            {
                                this._elements.Pop();
                                if (this._text != null)
                                {
                                    parent = this._elements.Peek();
                                    localName = reader.LocalName;
                                    this.HandleElementEnd(parent, localName);
                                }
                            }

                            this._text = null;
                            break;
                        }

                    case XmlNodeType.Text:
                        this._text = reader.Value;
                        break;
                }

                completed = this._elements.Count == 0;
            }

            return this._headerObject.ImmutableInstance;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Create a PartyMutable object from the given attribute name,value table
        /// </summary>
        /// <returns>
        ///     A PartyMutable object with the attributes["id"] value
        /// </returns>
        private PartyMutable CreatePartyType()
        {
            var party = new PartyMutable();
            party.Id = Helper.TrySetFromAttribute(this.Attributes, AttributeNameTable.id, party.Id);
            return party;
        }

        /// <summary>
        /// Handle the elements text for child elements of <paramref name="contact"/>
        /// </summary>
        /// <param name="contact">
        /// The contact parent.
        /// </param>
        /// <param name="localName">
        /// The element tag local name.
        /// </param>
        /// <returns>
        /// True if it was handled else false
        /// </returns>
        private bool HandleChildText(IContactMutableObject contact, object localName)
        {
            if (contact == null)
            {
                return false;
            }

            if (NameTableCache.IsElement(localName, ElementNameTable.Name))
            {
                contact.AddName(new TextTypeWrapperMutableCore { Locale = this._lang, Value = this._text });
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Department))
            {
                contact.Departments.Add(new TextTypeWrapperMutableCore { Locale = this._lang, Value = this._text });
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Role))
            {
                contact.AddRole(new TextTypeWrapperMutableCore { Locale = this._lang, Value = this._text });
            }
            else
            {
                ElementNameTable element;
                if (Enum.TryParse(localName as string, out element))
                {
                    switch (element)
                    {
                        case ElementNameTable.Telephone:
                            contact.Telephone.Add(this._text);
                            break;
                        case ElementNameTable.Fax:
                            contact.Fax.Add(this._text);
                            break;
                        case ElementNameTable.X400:
                            contact.X400.Add(this._text);
                            break;
                        case ElementNameTable.URI:
                            contact.Uri.Add(this._text);
                            break;
                        case ElementNameTable.Email:
                            contact.Email.Add(this._text);
                            break;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// The method is called every time a new element is encountered during parsing
        /// </summary>
        /// <param name="parent">
        /// The parent SDMX Model object from Common API namespace
        /// </param>
        /// <param name="localName">
        /// The name of the element
        /// </param>
        /// <returns>
        /// The handle element.
        /// </returns>
        private object HandleElement(object parent, object localName)
        {
            object current = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.Receiver))
            {
                PartyMutable party = this.CreatePartyType();
                this._headerObject.Receiver.Add(party);
                current = party;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Sender))
            {
                PartyMutable party = this.CreatePartyType();
                this._headerObject.Sender = party;
                current = party;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Contact))
            {
                var party = parent as PartyMutable;
                IContactMutableObject c = new ContactMutableObjectCore(); //TODO: hack
                var contact = new ContactCore(c);
                if (party != null)
                {
                    party.Contacts.Add(contact);
                }

                current = contact;
            }

            return current;
        }

        /// <summary>
        /// The method is called every time a closing element is encountered during parsing
        /// </summary>
        /// <param name="parent">
        /// The parent SDMX Model object from Common API namespace
        /// </param>
        /// <param name="localName">
        /// The name of the element
        /// </param>
        private void HandleElementEnd(object parent, object localName)
        {
            var partyType = parent as PartyMutable;
            if (partyType != null)
            {
                if (NameTableCache.IsElement(localName, ElementNameTable.Name))
                {
                    partyType.Name.Add(new TextTypeWrapperImpl(this._lang, this._text, null));  //TODO: check if sending null is correct
                }
            }
            else if (this.HandleChildText(parent as IContactMutableObject, localName))
            {
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.ID))
            {
                this._headerObject.Id = this._text;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Test))
            {
                this._headerObject.Test = XmlConvert.ToBoolean(this._text);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Truncated))
            {
                this._headerObject.AdditionalAttribtues.Add("Truncated", this._text);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Name))
            {
                this._headerObject.Name.Add(new TextTypeWrapperImpl(this._lang, this._text, null));
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Prepared))
            {
                this._headerObject.Prepared = XmlConvert.ToDateTime(
                    this._text, XmlDateTimeSerializationMode.RoundtripKind);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.KeyFamilyRef))
            {
                //// TODO in writer they are not used like this
                this._headerObject.AdditionalAttribtues.Add(
                    NameTableCache.GetElementName(ElementNameTable.KeyFamilyRef), this._text);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.KeyFamilyAgency))
            {
                //// TODO in writer they are not used like this
                this._headerObject.AdditionalAttribtues.Add(
                    NameTableCache.GetElementName(ElementNameTable.KeyFamilyAgency), this._text);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.DataSetAgency))
            {
                this._headerObject.AdditionalAttribtues.Add(
                    NameTableCache.GetElementName(ElementNameTable.DataSetAgency), this._text);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.DataSetID))
            {
                this._headerObject.DatasetId = this._text;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.DataSetAction))
            {
                DatasetActionEnumType action;
                if (Enum.TryParse(this._text, out action))
                {
                    this._headerObject.Action = DatasetAction.GetFromEnum(action);
                }
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Extracted))
            {
                this._headerObject.Extracted = XmlConvert.ToDateTime(
                    this._text, XmlDateTimeSerializationMode.RoundtripKind);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.ReportingBegin))
            {
                this._headerObject.ReportingBegin = XmlConvert.ToDateTime(
                    this._text, XmlDateTimeSerializationMode.RoundtripKind);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.ReportingEnd))
            {
                this._headerObject.ReportingEnd = XmlConvert.ToDateTime(
                    this._text, XmlDateTimeSerializationMode.RoundtripKind);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Source))
            {
                this._headerObject.Source.Add(new TextTypeWrapperImpl(this._lang, this._text, null));
            }
        }

        #endregion

        /// <summary>
        /// The header mutable.
        /// </summary>
        private class HeaderMutable : IHeader
        {
            #region Fields

            /// <summary>
            ///     The _additional attributes.
            /// </summary>
            private readonly IDictionary<string, string> _additionalAttributes =
                new Dictionary<string, string>(StringComparer.Ordinal);

            /// <summary>
            ///     The _name.
            /// </summary>
            private readonly IList<ITextTypeWrapper> _name = new List<ITextTypeWrapper>();

            /// <summary>
            ///     The _receiver.
            /// </summary>
            private readonly List<IParty> _receiver = new List<IParty>();

            /// <summary>
            ///     The _source.
            /// </summary>
            private readonly IList<ITextTypeWrapper> _source = new List<ITextTypeWrapper>();

            /// <summary>
            ///     The _structure references.
            /// </summary>
            private readonly List<IDatasetStructureReference> _structureReferences =
                new List<IDatasetStructureReference>();

            #endregion

            #region Public Properties

            /// <summary>
            /// Gets or sets the action.
            /// </summary>
            public DatasetAction Action { get; set; }

            /// <summary>
            /// Gets the additional attributes.
            /// </summary>
            IDictionary<string, string> IHeader.AdditionalAttribtues
            {
                get
                {
                    return new Dictionary<string, string>(this._additionalAttributes);
                }
            }

            /// <summary>
            /// Gets the additional attributes.
            /// </summary>
            public IDictionary<string, string> AdditionalAttribtues
            {
                get
                {
                    return this._additionalAttributes;
                }
            }

            /// <summary>
            /// Gets or sets the data provider reference.
            /// </summary>
            public IStructureReference DataProviderReference { get; set; }

            /// <summary>
            /// Gets or sets the dataset id.
            /// </summary>
            public string DatasetId { get; set; }

            /// <summary>
            /// Gets or sets the embargo date.
            /// </summary>
            public DateTime? EmbargoDate { get; set; }

            /// <summary>
            /// Gets or sets the extracted.
            /// </summary>
            public DateTime? Extracted { get; set; }

            /// <summary>
            /// Gets or sets the id.
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// Gets the name.
            /// </summary>
            IList<ITextTypeWrapper> IHeader.Name
            {
                get
                {
                    return new List<ITextTypeWrapper>(this._name);
                }
            }

            /// <summary>
            /// Gets the name.
            /// </summary>
            public IList<ITextTypeWrapper> Name
            {
                get
                {
                    return this._name;
                }
            }

            /// <summary>
            /// Gets or sets the prepared.
            /// </summary>
            public DateTime? Prepared { get; set; }

            /// <summary>
            /// Gets the receiver.
            /// </summary>
            IList<IParty> IHeader.Receiver
            {
                get
                {
                    return this._receiver.ToArray();
                }
            }

            /// <summary>
            /// Gets the receiver.
            /// </summary>
            public IList<IParty> Receiver
            {
                get
                {
                    return this._receiver;
                }
            }

            /// <summary>
            /// Gets or sets the reporting begin.
            /// </summary>
            public DateTime? ReportingBegin { get; set; }

            /// <summary>
            /// Gets or sets the reporting end.
            /// </summary>
            public DateTime? ReportingEnd { get; set; }

            /// <summary>
            /// Gets or sets the sender.
            /// </summary>
            public IParty Sender { get; set; }

            /// <summary>
            /// Gets the source.
            /// </summary>
            IList<ITextTypeWrapper> IHeader.Source
            {
                get
                {
                    return new List<ITextTypeWrapper>(this._source);
                }
            }

            /// <summary>
            /// Gets the source.
            /// </summary>
            public IList<ITextTypeWrapper> Source
            {
                get
                {
                    return this._source;
                }
            }

            /// <summary>
            /// Gets the structures.
            /// </summary>
            IList<IDatasetStructureReference> IHeader.Structures
            {
                get
                {
                    return this._structureReferences.ToArray();
                }
            }

            /// <summary>
            /// Gets the structures.
            /// </summary>
            public IList<IDatasetStructureReference> Structures
            {
                get
                {
                    return this._structureReferences;
                }
            }

            /// <summary>
            /// Gets or sets a value indicating whether test.
            /// </summary>
            public bool Test { get; set; }

            /// <summary>
            /// Gets the immutable instance.
            /// </summary>
            /// <value>
            /// The immutable instance.
            /// </value>
            public IHeader ImmutableInstance
            {
                get
                {
                    var receivers = (from party in this._receiver let mutable = party as PartyMutable select mutable != null ? mutable.ImmutableInstance : party).ToArray();
                    var sender = this.Sender is PartyMutable ? ((PartyMutable)this.Sender).ImmutableInstance : this.Sender;
                    return new HeaderImpl(this._additionalAttributes, this._structureReferences, this.DataProviderReference, this.Action, this.Id, this.DatasetId, this.EmbargoDate, this.Extracted, this.Prepared, this.ReportingBegin, this.ReportingEnd, this._name, this._source, receivers, sender, this.Test);
                }
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// The get additional attribute.
            /// </summary>
            /// <param name="headerField">
            /// The header field.
            /// </param>
            /// <returns>
            /// The <see cref="string"/>.
            /// </returns>
            public string GetAdditionalAttribtue(string headerField)
            {
                string value;
                if (this._additionalAttributes.TryGetValue(headerField, out value))
                {
                    return value;
                }

                return string.Empty;
            }

            /// <summary>
            /// The get structure by id.
            /// </summary>
            /// <param name="structureId">
            /// The structure id.
            /// </param>
            /// <returns>
            /// The <see cref="IDatasetStructureReference"/>.
            /// </returns>
            public IDatasetStructureReference GetStructureById(string structureId)
            {
                foreach (IDatasetStructureReference currentStructure in this._structureReferences)
                {
                    if (currentStructure.Id.Equals(structureId))
                    {
                        return currentStructure;
                    }
                }

                return null;
            }

            /// <summary>
            /// The has additional attribute.
            /// </summary>
            /// <param name="headerField">
            /// The header field.
            /// </param>
            /// <returns>
            /// The <see cref="bool"/>.
            /// </returns>
            public bool HasAdditionalAttribtue(string headerField)
            {
                return this._additionalAttributes.ContainsKey(headerField);
            }

            public void AddReciever(IParty recevier)
            {
                this._receiver.Add(recevier);
            }

            public void AddSource(ITextTypeWrapper source)
            {
                this._source.Add(source);
            }

            public void AddStructure(IDatasetStructureReference datasetStructureReference)
            {
                this._structureReferences.Add(datasetStructureReference);
            }

            public void AddName(ITextTypeWrapper name)
            {
                this._name.Add(name);
            }

            #endregion
        }

        /// <summary>
        /// The party mutable.
        /// </summary>
        private class PartyMutable : IParty
        {
            #region Fields

            /// <summary>
            /// The _contacts.
            /// </summary>
            private readonly List<IContact> _contacts = new List<IContact>();

            /// <summary>
            /// The _name.
            /// </summary>
            private readonly IList<ITextTypeWrapper> _name = new List<ITextTypeWrapper>();

            /// <summary>
            /// The _id.
            /// </summary>
            private string _id;

            /// <summary>
            /// The _time zone.
            /// </summary>
            private string _timeZone;

            #endregion

            #region Public Properties

            /// <summary>
            /// Gets the contacts.
            /// </summary>
            IList<IContact> IParty.Contacts
            {
                get
                {
                    return this._contacts.ToArray();
                }
            }

            /// <summary>
            /// Gets the contacts.
            /// </summary>
            public IList<IContact> Contacts
            {
                get
                {
                    return this._contacts;
                }
            }

            /// <summary>
            /// Gets or sets the id.
            /// </summary>
            public string Id
            {
                get
                {
                    return this._id;
                }

                set
                {
                    this._id = value;
                }
            }

            /// <summary>
            /// Gets the name.
            /// </summary>
            IList<ITextTypeWrapper> IParty.Name
            {
                get
                {
                    return new List<ITextTypeWrapper>(this._name);
                }
            }

            /// <summary>
            /// Gets the name.
            /// </summary>
            public IList<ITextTypeWrapper> Name
            {
                get
                {
                    return this._name;
                }
            }

            /// <summary>
            /// Gets or sets the time zone.
            /// </summary>
            public string TimeZone
            {
                get
                {
                    return this._timeZone;
                }

                set
                {
                    this._timeZone = value;
                }
            }

            /// <summary>
            /// Gets the immutable instance.
            /// </summary>
            /// <value>
            /// The immutable instance.
            /// </value>
            public IParty ImmutableInstance
            {
                get
                {
                    return new PartyCore(this._name, this._id, this._contacts, this._timeZone);
                }
            }

            #endregion
        }
    }
}