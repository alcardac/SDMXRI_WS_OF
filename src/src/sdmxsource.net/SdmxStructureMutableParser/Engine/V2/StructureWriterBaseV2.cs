// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureWriterBaseV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure writer base v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Engine;
    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    /// <summary>
    ///     The structure writer base v 2.
    /// </summary>
    public abstract class StructureWriterBaseV2 : Writer
    {
        #region Static Fields

        /// <summary>
        ///     The SDMX schema version that this class supports
        /// </summary>
        private static readonly SdmxSchema _versionTwo = SdmxSchema.GetFromEnum(SdmxSchemaEnumType.VersionTwo);

        #endregion

        #region Fields

        /// <summary>
        ///     The default ns.
        /// </summary>
        private readonly NamespacePrefixPair _defaultNs;

        /// <summary>
        ///     The root namespace, e.g. for <c>CodeLists</c>
        /// </summary>
        private NamespacePrefixPair _rootNamespace;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWriterBaseV2"/> class.
        ///     Initializes a new instance of the <see cref="StructureWriterV2"/> class.
        ///     Constructor that initialize the internal fields
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// writer is null
        /// </exception>
        /// <param name="writer">
        /// The XmlWriter object use to actually perform the writing
        /// </param>
        /// <param name="namespaces">
        /// The namespaces
        /// </param>
        protected StructureWriterBaseV2(XmlWriter writer, SdmxNamespaces namespaces)
            : base(writer, _versionTwo, namespaces)
        {
            this._defaultNs = this.Namespaces.Structure;
            this._rootNamespace = this.Namespaces.Message;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the root namespace, e.g. for <c>CodeLists</c>
        /// </summary>
        protected internal NamespacePrefixPair RootNamespace
        {
            get
            {
                return this._rootNamespace;
            }
        }

        /// <summary>
        ///     Gets the default ns.
        /// </summary>
        protected override NamespacePrefixPair DefaultNS
        {
            get
            {
                return this._defaultNs;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set the top element namespace prefix
        /// </summary>
        /// <param name="namespaceUri">
        /// The namespace URI
        /// </param>
        internal void SetTopElementsNS(NamespacePrefixPair namespaceUri)
        {
            this._rootNamespace = namespaceUri;
        }

        /// <summary>
        /// Write the xml attributes of the given IIdentifiableMutableObject type object
        /// </summary>
        /// <param name="artefact">
        /// The IIdentifiableMutableObject type object to write
        /// </param>
        protected void WriteIdentifiableArtefactAttributes(IIdentifiableMutableObject artefact)
        {
            this.WriteAttributeString(AttributeNameTable.id, artefact.Id);
            this.TryWriteAttribute(AttributeNameTable.uri, artefact.Uri);
            this.TryWriteAttribute(AttributeNameTable.urn, artefact.Urn);
        }

        /// <summary>
        /// Write the Name(s) and Description(s) of the given IIdentifiableMutableObject type object
        /// </summary>
        /// <param name="artefact">
        /// The IIdentifiableMutableObject type object to write
        /// </param>
        protected void WriteIdentifiableArtefactContent(INameableMutableObject artefact)
        {
            foreach (ITextTypeWrapperMutableObject textTypeBean in artefact.Names)
            {
                this.WriteTextType(this._defaultNs, textTypeBean, ElementNameTable.Name);
            }

            foreach (ITextTypeWrapperMutableObject textTypeBean in artefact.Descriptions)
            {
                this.WriteTextType(this._defaultNs, textTypeBean, ElementNameTable.Description);
            }
        }

        /// <summary>
        /// Write the given IItemMutableObject type object to the given element.
        /// </summary>
        /// <param name="element">
        /// The xml element
        /// </param>
        /// <param name="item">
        /// The IItemMutableObject type object to write
        /// </param>
        protected void WriteItem(ElementNameTable element, IItemMutableObject item)
        {
            // ReSharper restore SuggestBaseTypeForParameter
            this.WriteStartElement(this.DefaultPrefix, element);
            this.WriteIdentifiableArtefactAttributes(item);

            //// TODO NOTE not supported by common api 
            ////TryWriteAttribute(AttributeNameTable.version, item.Version);
            ////TryWriteAttribute(AttributeNameTable.validFrom, item.ValidFrom);
            ////TryWriteAttribute(AttributeNameTable.validTo, item.ValidTo);
            this.WriteIdentifiableArtefactContent(item);
        }

        /// <summary>
        /// Write the given IMaintainableMutableObject type object to the given element.
        /// </summary>
        /// <param name="element">
        /// The xml element
        /// </param>
        /// <param name="artefact">
        /// The IMaintainableMutableObject type object to write
        /// </param>
        protected void WriteMaintainableArtefact(ElementNameTable element, IMaintainableMutableObject artefact)
        {
            this.WriteStartElement(this.DefaultPrefix, element);
            this.WriteMaintainableArtefactAttributes(artefact);
            this.WriteIdentifiableArtefactContent(artefact);
        }

        /// <summary>
        /// Write the TextFormat Element from the given ITextFormatMutableObject
        /// </summary>
        /// <param name="textFormat">
        /// The ITextFormatMutableObject to write
        /// </param>
        protected void WriteTextFormat(ITextFormatMutableObject textFormat)
        {
            this.WriteTextFormat(ElementNameTable.TextFormat, textFormat);
        }

        /// <summary>
        /// Write the specified Element from the given ITextFormatMutableObject
        /// </summary>
        /// <param name="element">
        /// The TextFormatType Element
        /// </param>
        /// <param name="textFormat">
        /// The ITextFormatMutableObject to write
        /// </param>
        protected void WriteTextFormat(ElementNameTable element, ITextFormatMutableObject textFormat)
        {
            this.WriteStartElement(this.DefaultPrefix, element);
            if (textFormat.TextType != null)
            {
                this.TryWriteAttribute(AttributeNameTable.textType, textFormat.TextType.EnumType.ToString());
            }

            if (textFormat.Decimals > -1)
            {
                this.TryWriteAttribute(AttributeNameTable.decimals, textFormat.Decimals);
            }

            if (textFormat.StartValue < textFormat.EndValue)
            {
                this.TryWriteAttribute(AttributeNameTable.startValue, textFormat.StartValue);
                this.TryWriteAttribute(AttributeNameTable.endValue, textFormat.EndValue);
            }

            if (textFormat.Interval > -1)
            {
                this.TryWriteAttribute(AttributeNameTable.interval, textFormat.Interval);
            }

            this.TryWriteAttribute(AttributeNameTable.isSequence, textFormat.Sequence);

            if (textFormat.MaxLength > -1)
            {
                this.TryWriteAttribute(AttributeNameTable.maxLength, textFormat.MaxLength);
            }

            if (textFormat.MinLength > -1)
            {
                this.TryWriteAttribute(AttributeNameTable.minLength, textFormat.MinLength);
            }

            this.TryWriteAttribute(AttributeNameTable.pattern, textFormat.Pattern);
            this.WriteEndElement();
        }

        /// <summary>
        /// Write the xml attributes of the given IMaintainableMutableObject type object
        /// </summary>
        /// <param name="artefact">
        /// The IMaintainableMutableObject type object to write
        /// </param>
        private void WriteMaintainableArtefactAttributes(IMaintainableMutableObject artefact)
        {
            this.WriteIdentifiableArtefactAttributes(artefact);
            this.TryWriteAttribute(AttributeNameTable.version, artefact.Version);
            this.TryWriteAttribute(AttributeNameTable.validFrom, artefact.StartDate);
            this.TryWriteAttribute(AttributeNameTable.validTo, artefact.EndDate);
            this.TryWriteAttribute(AttributeNameTable.agencyID, artefact.AgencyId);
            this.TryWriteAttribute(AttributeNameTable.isFinal, artefact.FinalStructure);
            this.TryWriteAttribute(AttributeNameTable.isExternalReference, artefact.ExternalReference);
        }

        #endregion
    }
}