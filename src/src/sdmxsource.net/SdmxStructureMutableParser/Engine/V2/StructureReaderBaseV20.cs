// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureReaderBaseV20.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure reader base for SDMX v 20.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Engine;
    using Estat.Sri.SdmxParseBase.Helper;
    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxStructureMutableParser.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Util.Exception;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     The structure reader base for SDMX v 20.
    /// </summary>
    public abstract class StructureReaderBaseV20 : Reader, IMutableReader
    {
        #region Fields

        /// <summary>
        ///     The element actions.
        /// </summary>
        private readonly Stack<ElementActions> _elementActions = new Stack<ElementActions>();

        /// <summary>
        ///     Current XmlLang scope
        /// </summary>
        private string _lang;

        /// <summary>
        ///     The XML reader.
        /// </summary>
        private XmlReader _reader;

        /// <summary>
        ///     The sdmx structure message object representation that is generated during the xml parsing
        /// </summary>
        private IMutableObjects _structure;

        /// <summary>
        ///     The current element text
        /// </summary>
        private string _text;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureReaderBaseV20"/> class.
        /// </summary>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        protected StructureReaderBaseV20(SdmxNamespaces namespaces)
            : base(namespaces, SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo))
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the current XmlLang scope
        /// </summary>
        protected internal string Lang
        {
            get
            {
                return this._lang;
            }
        }

        /// <summary>
        ///     Gets or sets the sdmx structure message object representation that is generated during the xml parsing
        /// </summary>
        protected internal IMutableObjects Structure
        {
            get
            {
                return this._structure;
            }

            set
            {
                this._structure = value;
            }
        }

        /// <summary>
        ///     Gets the current element text
        /// </summary>
        protected internal string Text
        {
            get
            {
                return this._text;
            }
        }

        /// <summary>
        ///     Gets the XML reader.
        /// </summary>
        protected XmlReader Reader
        {
            get
            {
                return this._reader;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Parses the reader opened against the stream containing the contents of a SDMX-ML Structure message or
        ///     RegistryInterface structure contents and populates the given IMutableObjects object.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// textReader is null
        /// </exception>
        /// <exception cref="ParseException">
        /// SDMX structure message parsing error
        /// </exception>
        /// <param name="reader">
        /// The xml reader opened against the stream containing the structure contents
        /// </param>
        /// <param name="structure">
        /// The IMutableObjects object to populate
        /// </param>
        /// <returns>
        /// The <paramref name="structure"/>
        /// </returns>
        public IMutableObjects Read(XmlReader reader, IMutableObjects structure)
        {
            if (structure == null)
            {
                throw new ArgumentNullException("structure");
            }

            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            this._structure = structure;
            this.ReadContents(reader, localName => this.HandleTopLevel(structure, localName));
            return structure;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a reference from <paramref name="mutable"/> and optionally from <paramref name="items"/>
        /// </summary>
        /// <param name="mutable">
        /// The mutable.
        /// </param>
        /// <param name="itemType">
        /// The item Type.
        /// </param>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <returns>
        /// The <see cref="IStructureReference"/> from <paramref name="mutable"/>.
        /// </returns>
        protected static IStructureReference CreateReference(IMaintainableMutableObject mutable, SdmxStructureEnumType itemType, params string[] items)
        {
            return new StructureReferenceImpl(mutable.AgencyId, mutable.Id, mutable.Version, itemType, items);
        }

        /// <summary>
        /// Create a reference from <paramref name="mutable"/>.
        /// </summary>
        /// <param name="mutable">
        /// The mutable.
        /// </param>
        /// <returns>
        /// The <see cref="IStructureReference"/> from <paramref name="mutable"/>.
        /// </returns>
        protected static IStructureReference CreateReference(IMaintainableMutableObject mutable)
        {
            return new StructureReferenceImpl(mutable.AgencyId, mutable.Id, mutable.Version, mutable.StructureType);
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <typeparam name="T">
        /// The type of the <paramref name="noused"/>
        /// </typeparam>
        /// <param name="noused">
        /// The parameter is not used.
        /// </param>
        /// <param name="ignore">
        /// The parameter is not used.
        /// </param>
        protected static void DoNothing<T>(T noused, object ignore)
        {
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <typeparam name="T">
        /// The type of the <paramref name="noused"/>
        /// </typeparam>
        /// <param name="noused">
        /// The parameter is not used.
        /// </param>
        /// <param name="ignore">
        /// The parameter is not used.
        /// </param>
        /// <returns>
        /// The <see cref="ElementActions.Empty"/>.
        /// </returns>
        protected static ElementActions DoNothingComplex<T>(T noused, object ignore)
        {
            return null;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="ignore">
        /// This parameter is not used.
        /// </param>
        protected static void DoNothing(object ignore)
        {
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="ignore">
        /// This parameter is not used.
        /// </param>
        /// <returns>
        /// The <see cref="ElementActions.Empty"/>.
        /// </returns>
        protected static ElementActions DoNothingComplex(object ignore)
        {
            return null;
        }

        /// <summary>
        /// Handles the TextFormat element
        /// </summary>
        /// <param name="attributes">
        /// The dictionary contains the attributes of the element
        /// </param>
        /// <returns>
        /// The created ITextFormatMutableObject
        /// </returns>
        protected static ITextFormatMutableObject HandleTextFormat(IDictionary<string, string> attributes)
        {
            var textFormat = new TextFormatMutableCore();

            TextEnumType textType = Helper.TrySetEnumFromAttribute(attributes, AttributeNameTable.textType, TextEnumType.Null);
            if (textType != TextEnumType.Null)
            {
                textFormat.TextType = TextType.GetFromEnum(textType);
            }

            textFormat.Sequence = Helper.TrySetFromAttribute(attributes, AttributeNameTable.isSequence, textFormat.Sequence);
            textFormat.MinLength = Helper.TrySetFromAttribute(attributes, AttributeNameTable.minLength, textFormat.MinLength);
            textFormat.MaxLength = Helper.TrySetFromAttribute(attributes, AttributeNameTable.maxLength, textFormat.MaxLength);
            textFormat.StartValue = Helper.TrySetFromAttribute(attributes, AttributeNameTable.startValue, textFormat.StartValue);
            textFormat.EndValue = Helper.TrySetFromAttribute(attributes, AttributeNameTable.endValue, textFormat.EndValue);
            textFormat.TimeInterval = Helper.TrySetFromAttribute(attributes, AttributeNameTable.timeInterval, textFormat.TimeInterval);
            textFormat.Interval = Helper.TrySetFromAttribute(attributes, AttributeNameTable.interval, textFormat.Interval);
            textFormat.Decimals = Helper.TrySetFromAttribute(attributes, AttributeNameTable.decimals, textFormat.Decimals);
            textFormat.Pattern = Helper.TrySetFromAttribute(attributes, AttributeNameTable.pattern, textFormat.Pattern);
            return textFormat;
        }

        /// <summary>
        /// Populates the properties of the given IMaintainableMutableObject object from the given xml attributes dictionary
        /// </summary>
        /// <param name="artefact">
        /// The given IMaintainableMutableObject object
        /// </param>
        /// <param name="attributes">
        /// The dictionary contains the attributes of the element
        /// </param>
        protected static void ParseAttributes(IMaintainableMutableObject artefact, IDictionary<string, string> attributes)
        {
            ParseAttributes((INameableMutableObject)artefact, attributes);
            artefact.Version = Helper.TrySetFromAttribute(attributes, AttributeNameTable.version, artefact.Version);
            artefact.StartDate = Helper.TrySetFromAttribute(attributes, AttributeNameTable.validFrom, artefact.StartDate);
            artefact.EndDate = Helper.TrySetFromAttribute(attributes, AttributeNameTable.validTo, artefact.EndDate);
            artefact.AgencyId = Helper.TrySetFromAttribute(attributes, AttributeNameTable.agencyID, artefact.AgencyId);
            artefact.FinalStructure = Helper.TrySetFromAttribute(attributes, AttributeNameTable.isFinal, artefact.FinalStructure);
            artefact.ExternalReference = Helper.TrySetFromAttribute(attributes, AttributeNameTable.isExternalReference, artefact.ExternalReference);
        }

        /// <summary>
        /// Populates the properties of the given IItemMutableObject object from the given xml attributes dictionary
        /// </summary>
        /// <param name="artefact">
        /// The given IItemMutableObject object
        /// </param>
        /// <param name="attributes">
        /// The dictionary contains the attributes of the element
        /// </param>
        protected static void ParseAttributes(IItemMutableObject artefact, IDictionary<string, string> attributes)
        {
            ParseAttributes((INameableMutableObject)artefact, attributes);
        }

        /// <summary>
        /// Populates the properties of the given IIdentifiableMutableObject object from the given xml attributes dictionary
        /// </summary>
        /// <param name="artefact">
        /// The given IIdentifiableMutableObject object
        /// </param>
        /// <param name="attributes">
        /// The dictionary contains the attributes of the element
        /// </param>
        protected static void ParseAttributes(IIdentifiableMutableObject artefact, IDictionary<string, string> attributes)
        {
            string name = NameTableCache.GetAttributeName(AttributeNameTable.id);
            string value;
            if (attributes.TryGetValue(name, out value))
            {
                artefact.Id = value;
            }

            //// Java 0.9.9 does not allow setting the URN
            ////artefact.Urn = Helper.TrySetFromAttribute(attributes, AttributeNameTable.urn, artefact.Urn);
            artefact.Uri = Helper.TrySetFromAttribute(attributes, AttributeNameTable.uri, artefact.Uri);
        }

        /// <summary>
        /// Adds a <see cref="ElementActions"/> to the stack which is executed at <see cref="ReadContents"/>
        ///     for a <see cref="INameableMutableObject"/> with default actions.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <typeparam name="T">
        /// The type of <paramref name="parent"/>
        /// </typeparam>
        /// <returns>
        /// The <see cref="ElementActions"/>.
        /// </returns>
        protected ElementActions AddNameableAction<T>(T parent) where T : INameableMutableObject
        {
            return this.BuildElementActions<INameableMutableObject>(parent, this.HandleAnnotableChildElements, this.HandleNameableTextChildElement);
        }

        /// <summary>
        /// Adds a <see cref="ElementActions"/> to the stack which is executed at <see cref="ReadContents"/>
        ///     for a <see cref="INameableMutableObject"/> with default actions together with <paramref name="action"/>.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <typeparam name="T">
        /// The type of <paramref name="parent"/>
        /// </typeparam>
        /// <returns>
        /// The <see cref="ElementActions"/>.
        /// </returns>
        protected ElementActions AddNameableAction<T>(T parent, Func<T, object, ElementActions> action) where T : INameableMutableObject
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            Func<object, ElementActions> complexAction = BuildAction(parent, this.HandleAnnotableChildElements, action);
            Action<object> simpleAction = BuildSimpleAction(parent, this.HandleNameableTextChildElement);
            var elementActions = new ElementActions(complexAction, simpleAction);
            return elementActions;
        }

        /// <summary>
        /// Adds a <see cref="ElementActions"/> to the stack which is executed at <see cref="ReadContents"/>
        ///     for a <see cref="INameableMutableObject"/> with default actions together with <paramref name="action"/> and
        ///     <paramref name="textAction"/>
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <param name="textAction">
        /// The simple element action
        /// </param>
        /// <typeparam name="T">
        /// The type of <paramref name="parent"/>
        /// </typeparam>
        /// <returns>
        /// The <see cref="ElementActions"/>.
        /// </returns>
        protected ElementActions AddNameableAction<T>(T parent, Func<T, object, ElementActions> action, Action<T, object> textAction) where T : INameableMutableObject
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (textAction == null)
            {
                throw new ArgumentNullException("textAction");
            }

            Func<object, ElementActions> complexAction = BuildAction(parent, this.HandleAnnotableChildElements, action);
            Action<object> simpleAction = BuildFuncAction(parent, this.HandleCommonTextChildElement, textAction);
            var elementActions = new ElementActions(complexAction, simpleAction);
            return elementActions;
        }

        /// <summary>
        /// Adds a <see cref="ElementActions"/> to the stack which is executed at <see cref="ReadContents"/>
        ///     for a <see cref="INameableMutableObject"/> with default actions together with <paramref name="action"/> and
        ///     <paramref name="textAction"/>
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <param name="textAction">
        /// The simple element action
        /// </param>
        /// <typeparam name="T">
        /// The type of <paramref name="parent"/>
        /// </typeparam>
        /// <returns>
        /// The <see cref="ElementActions"/>.
        /// </returns>
        protected ElementActions AddAnnotableAction<T>(T parent, Func<T, object, ElementActions> action, Action<T, object> textAction) where T : IAnnotableMutableObject
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (textAction == null)
            {
                throw new ArgumentNullException("textAction");
            }

            Func<object, ElementActions> complexAction = BuildAction(parent, this.HandleAnnotableChildElements, action);
            Action<object> simpleAction = BuildSimpleAction(parent, textAction);
            var elementActions = new ElementActions(complexAction, simpleAction);
            return elementActions;
        }

        /// <summary>
        /// Adds a <see cref="ElementActions"/> to the stack which is executed at <see cref="ReadContents"/>
        ///     for a <see cref="IReferenceInfo"/> with default actions together with <paramref name="action"/> and
        ///     <paramref name="textAction"/>
        ///     .
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <param name="textAction">
        /// The simple element action
        /// </param>
        /// <typeparam name="T">
        /// The type of <paramref name="parent"/>
        /// </typeparam>
        /// <returns>
        /// The <see cref="ElementActions"/>.
        /// </returns>
        protected ElementActions AddReferenceAction<T>(T parent, Func<T, object, ElementActions> action, Action<T, object> textAction) where T : IReferenceInfo
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (textAction == null)
            {
                throw new ArgumentNullException("textAction");
            }

            Func<object, ElementActions> complexAction = BuildAction(parent, action);
            Action<object> simpleAction = BuildFuncAction(parent, this.HandleReferenceTextChildElement, textAction);
            var elementActions = new ElementActions(complexAction, simpleAction);
            return elementActions;
        }

        /// <summary>
        /// Adds a <see cref="ElementActions"/> to the stack which is executed at <see cref="ReadContents"/>
        ///     with no complex action.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="action">
        /// The simple action.
        /// </param>
        /// <typeparam name="T">
        /// The type of <paramref name="parent"/>
        /// </typeparam>
        /// <returns>
        /// The <see cref="ElementActions"/>.
        /// </returns>
        protected ElementActions AddSimpleAction<T>(T parent, Action<T, object> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            Action<object> simpleAction = BuildSimpleAction(parent, action);
            return new ElementActions(DoNothingComplex, simpleAction);
        }

        /// <summary>
        /// Builds a <see cref="ElementActions"/> which is added to the stack and executed at <see cref="ReadContents"/>
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="action">
        /// The action for complex elements.
        /// </param>
        /// <param name="simpleAction">
        /// The action for simple elements, meaning with text
        /// </param>
        /// <typeparam name="T">
        /// The type of <paramref name="parent"/>
        /// </typeparam>
        /// <returns>
        /// The <see cref="ElementActions"/>.
        /// </returns>
        protected ElementActions BuildElementActions<T>(T parent, Func<T, object, ElementActions> action, Action<T, object> simpleAction)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (simpleAction == null)
            {
                throw new ArgumentNullException("simpleAction");
            }

            return new ElementActions(BuildAction(parent, action), BuildSimpleAction(parent, simpleAction));
        }

        /// <summary>
        /// Handles the <see cref="IAnnotableMutableObject"/> type elements child elements
        /// </summary>
        /// <typeparam name="T">
        /// The type of <paramref name="parent"/>
        /// </typeparam>
        /// <param name="parent">
        /// The parent <see cref="IAnnotableMutableObject"/> object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="ElementActions"/>.
        /// </returns>
        protected ElementActions HandleAnnotableChildElements<T>(T parent, object localName) where T : IAnnotableMutableObject
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.Annotations))
            {
                return this.BuildElementActions(parent.Annotations, this.HandleChildElements, DoNothing);
            }

            return null;
        }

        /// <summary>
        /// Handle the child elements of <paramref name="parent"/>
        /// </summary>
        /// <param name="parent">
        /// The parent
        /// </param>
        /// <param name="localName">
        /// The element tag local name.
        /// </param>
        /// <returns>
        /// The <see cref="ElementActions"/>.
        /// </returns>
        protected ElementActions HandleChildElements(ICollection<IAnnotationMutableObject> parent, object localName)
        {
            if (localName == null)
            {
                return null;
            }

            IAnnotationMutableObject annotation = new AnnotationMutableCore();
            parent.Add(annotation);
            return this.AddSimpleAction(annotation, this.HandleTextChildElement);
        }

        /// <summary>
        /// Handle the common text elements for IIdentifiableMutableObject based objects
        /// </summary>
        /// <typeparam name="T">
        /// The type of <paramref name="artefact"/>
        /// </typeparam>
        /// <param name="artefact">
        /// The parent <see cref="INameableMutableObject"/>
        /// </param>
        /// <param name="localName">
        /// The local name of the XML tag
        /// </param>
        /// <returns>
        /// The handle common text child element.
        /// </returns>
        protected bool HandleCommonTextChildElement<T>(T artefact, object localName) where T : INameableMutableObject
        {
            bool found = false;
            if (NameTableCache.IsElement(localName, ElementNameTable.Name))
            {
                artefact.Names.Add(new TextTypeWrapperMutableCore { Value = this.Text, Locale = this.Lang });
                found = true;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Description))
            {
                artefact.Descriptions.Add(new TextTypeWrapperMutableCore { Value = this.Text, Locale = this.Lang });
                found = true;
            }

            return found;
        }

        /// <summary>
        /// When override handles end element.
        /// </summary>
        /// <param name="localname">
        /// The element local name.
        /// </param>
        protected virtual void HandleEndElement(object localname)
        {
        }

        /// <summary>
        /// Handle the common text elements for IIdentifiableMutableObject based objects
        /// </summary>
        /// <typeparam name="T">
        /// The type of <paramref name="artefact"/>
        /// </typeparam>
        /// <param name="artefact">
        /// The parent <see cref="INameableMutableObject"/>
        /// </param>
        /// <param name="localName">
        /// The local name of the XML tag
        /// </param>
        protected void HandleNameableTextChildElement<T>(T artefact, object localName) where T : INameableMutableObject
        {
            this.HandleCommonTextChildElement(artefact, localName);
        }

        /// <summary>
        /// Handles the Ref type element child elements
        /// </summary>
        /// <typeparam name="T">
        /// The type of <paramref name="parent"/>
        /// </typeparam>
        /// <param name="parent">
        /// The parent RefBean object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The handle common text child element.
        /// </returns>
        protected bool HandleReferenceTextChildElement<T>(T parent, object localName) where T : IReferenceInfo
        {
            bool found = false;
            if (NameTableCache.IsElement(localName, ElementNameTable.URN))
            {
                parent.URN = Helper.TrySetUri(this._text);
                found = true;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Version))
            {
                parent.Version = this._text;
                found = true;
            }

            return found;
        }

        /// <summary>
        /// The handle text child element.
        /// </summary>
        /// <param name="annotation">
        /// The annotation.
        /// </param>
        /// <param name="localName">
        /// The local name.
        /// </param>
        protected void HandleTextChildElement(IAnnotationMutableObject annotation, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.AnnotationTitle))
            {
                annotation.Title = this.Text;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.AnnotationType))
            {
                annotation.Type = this.Text;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.AnnotationURL))
            {
                annotation.Uri = Helper.TrySetUri(this.Text);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.AnnotationText))
            {
                annotation.Text.Add(new TextTypeWrapperMutableCore { Value = this.Text, Locale = this.Lang });
            }
        }

        /// <summary>
        /// Handle top level elements.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="localName">
        /// The local name.
        /// </param>
        /// <returns>
        /// The <see cref="ElementActions"/>.
        /// </returns>
        protected abstract ElementActions HandleTopLevel(IMutableObjects parent, object localName);

        /// <summary>
        /// Handles the Structure top level elements
        ///     This includes ConceptSchemes, Codelists, Dataflows, CategorySchemes and KeyFamilies
        /// </summary>
        /// <param name="parent">
        /// The parent <see cref="IMutableObjects"/>
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The created SDMX model entity object corresponding to the current xml element or null if there is none
        /// </returns>
        protected object HandleTopLevelBase(IMutableObjects parent, object localName)
        {
            IMutableReader mutableReader;
            if (NameTableCache.IsElement(localName, ElementNameTable.Dataflows))
            {
                mutableReader = new DataflowReaderV2(this.Namespaces);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.CodeLists))
            {
                mutableReader = new CodeListReaderV2(this.Namespaces);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.CategorySchemes))
            {
                mutableReader = new CategorySchemeReaderV2(this.Namespaces);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.KeyFamilies))
            {
                mutableReader = new DataStructureReaderV2(this.Namespaces);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Concepts))
            {
                mutableReader = new ConceptSchemeReaderV2(this.Namespaces);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.HierarchicalCodelists))
            {
                mutableReader = new HierarchicalCodeListReaderV2(this.Namespaces);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.MetadataStructureDefinitions))
            {
                mutableReader = new MetadataStructureReaderV2(this.Namespaces);
            }
            else
            {
                return null;
            }

            return mutableReader.Read(this._reader, parent);
        }

        /// <summary>
        /// Read the the contents of a structure message and populate the <paramref name="rootObject"/>
        /// </summary>
        /// <param name="reader">
        /// The xml reader of the structure message
        /// </param>
        /// <param name="rootObject">
        /// The root object
        /// </param>
        protected virtual void ReadContents(XmlReader reader, Func<object, ElementActions> rootObject)
        {
            bool completed = false;
            this._elementActions.Clear();
            this._elementActions.Push(new ElementActions(rootObject, DoNothing));
            this._text = null;
            this._reader = reader;
            while (!completed && reader.Read())
            {
                object localName;
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            bool isEmpty = reader.IsEmptyElement;
                            localName = reader.LocalName;

                            // read the attributes
                            this.Attributes.Clear();
                            for (int i = 0; i < reader.AttributeCount; i++)
                            {
                                reader.MoveToAttribute(i);
                                this.Attributes.Add(reader.Name, reader.Value);
                            }

                            // get the language xml:lang if set
                            this._lang = reader.XmlLang;

                            // check if we have any actions left
                            if (this._elementActions.Count > 0)
                            {
                                // peek the last action. We pop at EndElement only.
                                var elementActions = this._elementActions.Peek();

                                // if it returns null we use ElementActions.Empty
                                ElementActions actions = elementActions.ExecuteComplexAction(localName) ?? elementActions;
                                if (!isEmpty)
                                {
                                    this._elementActions.Push(actions);
                                }
                            }

                            ////object current = this.ExecuteHandleElement(parent, localName) ?? parent;
                            ////if (!isEmpty)
                            ////{
                            ////    this.Elements.Push(current);
                            ////}
                            break;
                        }

                    case XmlNodeType.EndElement:
                        {
                            if (this._elementActions.Count > 0)
                            {
                                localName = reader.LocalName;
                                ElementActions currentActions = this._elementActions.Pop();
                                if (this._text != null)
                                {
                                    currentActions.ExecuteSimpleAction(localName);
                                }
                                
                                this.HandleEndElement(localName);
                            }

                            this._text = null;
                            break;
                        }

                    case XmlNodeType.Text:
                        {
                            this._text = reader.Value;
                            break;
                        }
                }

                completed = this._elementActions.Count == 0;
            }
        }

        /// <summary>
        /// Builds an action.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <typeparam name="T">
        /// The type of <paramref name="parent"/>.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Action"/>.
        /// </returns>
        private static Func<object, ElementActions> BuildAction<T>(T parent, Func<T, object, ElementActions> action)
        {
            return localName => action(parent, localName);
        }

        /// <summary>
        /// Builds an action.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="baseAction">
        /// The base action.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <typeparam name="T">
        /// The type of <paramref name="parent"/>.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Action"/>.
        /// </returns>
        private static Func<object, ElementActions> BuildAction<T>(T parent, Func<T, object, ElementActions> baseAction, Func<T, object, ElementActions> action)
        {
            return localName =>
                {
                    var fromBaseAction = baseAction(parent, localName);
                    return fromBaseAction ?? action(parent, localName);
                };
        }

        /// <summary>
        /// Builds an action.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="baseAction">
        /// The base action. A function that returns true if it matches an element; otherwise false
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <typeparam name="T">
        /// The type of <paramref name="parent"/>.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Action"/>.
        /// </returns>
        private static Action<object> BuildFuncAction<T>(T parent, Func<T, object, bool> baseAction, Action<T, object> action)
        {
            return localName =>
                {
                    if (!baseAction(parent, localName))
                    {
                        action(parent, localName);
                    }
                };
        }

        /// <summary>
        /// Builds an action.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <typeparam name="T">
        /// The type of <paramref name="parent"/>.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Action"/>.
        /// </returns>
        private static Action<object> BuildSimpleAction<T>(T parent, Action<T, object> action)
        {
            return localName => action(parent, localName);
        }

        #endregion

        /// <summary>
        ///     The element actions.
        /// </summary>
        protected class ElementActions
        {
            #region Static Fields

            /// <summary>
            ///     Empty <see cref="ElementActions" />
            /// </summary>
            private static readonly ElementActions _empty = new ElementActions(null, null);

            #endregion

            #region Fields

            /// <summary>
            ///     The _complex action.
            /// </summary>
            private readonly Func<object, ElementActions> _complexAction;

            /// <summary>
            ///     The _simple action.
            /// </summary>
            private readonly Action<object> _simpleAction;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="ElementActions"/> class.
            /// </summary>
            /// <param name="complexAction">
            /// The complex action.
            /// </param>
            /// <param name="simpleAction">
            /// The simple action.
            /// </param>
            public ElementActions(Func<object, ElementActions> complexAction, Action<object> simpleAction)
            {
                this._complexAction = complexAction;
                this._simpleAction = simpleAction;
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets the empty <see cref="ElementActions" />
            /// </summary>
            public static ElementActions Empty
            {
                get
                {
                    return _empty;
                }
            }

            #endregion

            /// <summary>
            /// The execute complex action.
            /// </summary>
            /// <param name="localName">
            /// The local name.
            /// </param>
            /// <returns>
            /// The <see cref="ElementActions"/>.
            /// </returns>
            public ElementActions ExecuteComplexAction(object localName)
            {
                ElementActions actions = null;
                if (this._complexAction != null)
                {
                    actions = this._complexAction(localName);
                }

                return actions;
            }

            /// <summary>
            /// The execute simple action.
            /// </summary>
            /// <param name="localName">
            /// The local name.
            /// </param>
            public void ExecuteSimpleAction(object localName)
            {
                if (this._simpleAction != null)
                {
                    this._simpleAction(localName);
                }
            }
        }

        /// <summary>
        /// Parses the component concept attributes.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns>The concept Reference</returns>
        protected static string ParseComponentConceptAttributes(IComponentMutableObject component, IDictionary<string, string> attributes)
        {
            // concepts attributes
            string conceptRef = Helper.TrySetFromAttribute(attributes, AttributeNameTable.conceptRef, string.Empty);

            string conceptSchemeAgency = Helper.TrySetFromAttribute(attributes, AttributeNameTable.conceptSchemeAgency, string.Empty);
            string conceptSchemeVersion = Helper.TrySetFromAttribute(attributes, AttributeNameTable.conceptSchemeVersion, string.Empty);
            if (string.IsNullOrWhiteSpace(conceptSchemeVersion))
            {
                conceptSchemeVersion = Helper.TrySetFromAttribute(attributes, AttributeNameTable.conceptVersion, string.Empty);
            }

            string conceptSchemeRef = Helper.TrySetFromAttribute(attributes, AttributeNameTable.conceptSchemeRef, string.Empty);

            component.ConceptRef = new StructureReferenceImpl(conceptSchemeAgency, conceptSchemeRef, conceptSchemeVersion, SdmxStructureEnumType.Concept, conceptRef);

            return conceptRef;
        }
    }
}