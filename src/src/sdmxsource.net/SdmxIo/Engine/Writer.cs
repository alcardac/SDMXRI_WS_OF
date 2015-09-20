// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Writer.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The base class for all SDMX-ML writers.
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
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Date;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The base class for all SDMX-ML writers.
    /// </summary>
    public abstract class Writer : IoBase
    {
        #region Static Fields

        #endregion

        #region Fields

        /// <summary>
        ///     A value indicating if <see cref="_writer" /> was already started and therefore expected to close outside
        /// </summary>
        private readonly bool _wrapped;

        /// <summary>
        ///     The internal field used to store the XmlTextWriter object that is used
        ///     by this class and classed based on Writer to perform the xml writing
        /// </summary>
        private readonly XmlWriter _writer;

        /// <summary>
        /// The _header retrieval manager
        /// </summary>
        private IHeaderRetrievalManager _headerRetrievalManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Writer"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="namespaces">
        /// The namespaces. If set the null then the default parameters
        /// </param>
        /// <param name="schema">
        /// The SDMX version schema.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer"/> is null
        ///     -or-
        ///     <paramref name="schema"/> is null
        /// </exception>
        protected Writer(XmlWriter writer, SdmxNamespaces namespaces, SdmxSchema schema)
            : base(namespaces, schema)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            if (schema == null)
            {
                throw new ArgumentNullException("schema");
            }

            this._writer = writer;
            this._wrapped = writer.WriteState != WriteState.Start;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Writer"/> class.
        /// </summary>
        /// <param name="writer">
        /// The output <see cref="XmlWriter"/>
        /// </param>
        /// <param name="targetSchema">
        /// The target SDMX Schema version
        /// </param>
        /// <param name="namespaces">
        /// The list of namespaces used by the message
        /// </param>
        protected Writer(
            XmlWriter writer, SdmxSchema targetSchema, SdmxNamespaces namespaces)
            : this(writer, namespaces, targetSchema)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the header retrieval manager.
        /// </summary>
        /// <value>
        /// The header retrieval manager.
        /// </value>
        public IHeaderRetrievalManager HeaderRetrievalManager
        {
            get
            {
                return this._headerRetrievalManager;
            }

            set
            {
                this._headerRetrievalManager = value;
            }
        }

        /// <summary>
        ///     Gets the default prefix that will be used by this message.
        /// </summary>
        protected string DefaultPrefix
        {
            get
            {
                return this.DefaultNS.Prefix;
            }
        }

        /// <summary>
        ///     Gets the name namespace.
        /// </summary>
        protected NamespacePrefixPair NameNamespace
        {
            get
            {
                return this.TargetSchema.EnumType == SdmxSchemaEnumType.VersionTwoPointOne
                           ? this.Namespaces.Common
                           : this.Namespaces.Message;
            }
        }

        /// <summary>
        ///     Gets the internal field used to store the XmlTextWriter object that is used
        ///     by this class and classed based on Writer to perform the xml writing
        /// </summary>
        protected XmlWriter SdmxMLWriter
        {
            get
            {
                return this._writer;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the <see cref="_writer" /> was already started and therefore expected to close outside
        /// </summary>
        protected bool Wrapped
        {
            get
            {
                return this._wrapped;
            }
        }

        /// <summary>
        ///   Gets the default namespace
        /// </summary>
        protected abstract NamespacePrefixPair DefaultNS { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the reference from the specified <paramref name="dataStructureObject"/>
        /// </summary>
        /// <param name="dataStructureObject">
        /// The maintainable reference
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        protected static string GetRef(IMaintainableObject dataStructureObject)
        {
            // TODO this also used in StartDataSet 
            return string.Format(
                CultureInfo.InvariantCulture, 
                "{0}_{1}_{2}", 
                dataStructureObject.AgencyId, 
                dataStructureObject.Id, 
                dataStructureObject.Version.Replace('.', '_'));
        }

        /// <summary>
        /// Write the element if it's value is not empty or null
        /// </summary>
        /// <param name="elementName">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void TryToWriteElement(ElementNameTable elementName, DateTime? value)
        {
            if (value.HasValue)
            {
                this.WriteElement(NamespacePrefixPair.Empty, elementName, DateUtil.FormatDate(value.Value));
            }
        }

        /// <summary>
        /// Write the element if it's value is not empty or null
        /// </summary>
        /// <param name="namespacePrefix">
        /// The namespace prefix pair
        /// </param>
        /// <param name="elementName">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void TryToWriteElement(
            NamespacePrefixPair namespacePrefix, ElementNameTable elementName, DateTime? value)
        {
            if (value.HasValue)
            {
                this.WriteElement(namespacePrefix, elementName, DateUtil.FormatDate(value.Value));
            }
        }

        /// <summary>
        /// Write the element if it's value is not empty or null
        /// </summary>
        /// <param name="namespacePrefix">
        /// The namespace prefix pair
        /// </param>
        /// <param name="element">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void TryToWriteElement(NamespacePrefixPair namespacePrefix, ElementNameTable element, string value)
        {
            this.TryToWriteElement(namespacePrefix, NameTableCache.GetElementName(element), value);
        }

        /// <summary>
        /// Write the element if it's value is not empty or null
        /// </summary>
        /// <param name="namespacePrefix">
        /// The namespace prefix pair
        /// </param>
        /// <param name="element">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void TryToWriteElement(NamespacePrefixPair namespacePrefix, string element, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                this.WriteElement(namespacePrefix, element, value);
            }
        }

        /// <summary>
        /// Write the element if it's value is not empty or null
        /// </summary>
        /// <param name="element">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void TryToWriteElement(ElementNameTable element, string value)
        {
            this.TryToWriteElement(NamespacePrefixPair.Empty, element, value);
        }

        /// <summary>
        /// Write the element if it's value is not empty or null
        /// </summary>
        /// <param name="prefix">
        /// The namespace prefix
        /// </param>
        /// <param name="element">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void TryToWriteElement(string prefix, ElementNameTable element, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                this.WriteElement(prefix, element, value);
            }
        }

        /// <summary>
        /// Write the element if it's value is not empty or null
        /// </summary>
        /// <param name="namespacePrefix">
        /// The namespace prefix
        /// </param>
        /// <param name="element">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void TryToWriteElement(NamespacePrefixPair namespacePrefix, ElementNameTable element, Uri value)
        {
            if (value != null)
            {
                this.WriteElement(namespacePrefix, element, value.ToString());
            }
        }

        /// <summary>
        /// Write the element if it's value is not empty or null
        /// </summary>
        /// <param name="prefix">
        /// The namespace prefix
        /// </param>
        /// <param name="element">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void TryToWriteElement(string prefix, ElementNameTable element, Uri value)
        {
            if (value != null)
            {
                this.WriteElement(prefix, element, value.ToString());
            }
        }

        /// <summary>
        /// Write the element with it's <c>xs:boolean</c> value if it is true
        /// </summary>
        /// <param name="element">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void TryToWriteElement(ElementNameTable element, bool value)
        {
            if (value)
            {
                this.WriteElement(NamespacePrefixPair.Empty, element, true);
            }
        }

        /// <summary>
        /// Write the element with it's <c>xs:boolean</c> value if it is true
        /// </summary>
        /// <param name="prefix">
        /// The namespace prefix
        /// </param>
        /// <param name="element">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void TryToWriteElement(NamespacePrefixPair prefix, ElementNameTable element, bool value)
        {
            if (value)
            {
                this.WriteElement(prefix, element, true);
            }
        }

        /// <summary>
        /// Write the element with it's <c>xs:boolean</c> value if it is true
        /// </summary>
        /// <param name="prefix">
        /// The namespace prefix
        /// </param>
        /// <param name="element">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void TryToWriteElement(string prefix, ElementNameTable element, bool value)
        {
            if (value)
            {
                this.WriteElement(prefix, element, true);
            }
        }

        /// <summary>
        /// Write the attribute if it's value is not empty or null
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(string attribute, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                this._writer.WriteAttributeString(attribute, value);
            }
        }

        /// <summary>
        /// Write the attribute if it's value is not  null
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(string attribute, Uri value)
        {
            if (value != null && value.ToString().Length > 0)
            {
                this._writer.WriteAttributeString(attribute, value.ToString());
            }
        }

        /// <summary>
        /// Write the attribute if it's value is larger than <see cref="DateTime.MinValue"/>
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(string attribute, DateTime value)
        {
            if (value > DateTime.MinValue)
            {
                this._writer.WriteAttributeString(attribute, value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Write the attribute and it's value
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(string attribute, bool value)
        {
            if (value)
            {
                this.WriteAttribute(attribute, true);
            }
        }

        /// <summary>
        /// Write the attribute and it's value
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(string attribute, TertiaryBoolEnumType value)
        {
            switch (value)
            {
                case TertiaryBoolEnumType.False:
                case TertiaryBoolEnumType.True:
                    this.WriteAttribute(attribute, TertiaryBool.GetFromEnum(value).IsTrue);
                    break;
            }
        }

        /// <summary>
        /// Write the attribute and it's value
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(string attribute, int value)
        {
            this._writer.WriteAttributeString(attribute, XmlConvert.ToString(value));
        }

        /// <summary>
        /// Write the attribute and it's value
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(string attribute, long value)
        {
            this._writer.WriteAttributeString(attribute, XmlConvert.ToString(value));
        }

        /// <summary>
        /// Write the attribute and it's value
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(string attribute, double value)
        {
            this._writer.WriteAttributeString(attribute, XmlConvert.ToString(value));
        }

        /// <summary>
        /// Write the attribute and it's value
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(string attribute, decimal value)
        {
            this._writer.WriteAttributeString(attribute, XmlConvert.ToString(value));
        }

        /// <summary>
        /// Write the attribute if it's value is not empty or null
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(AttributeNameTable attribute, string value)
        {
            this.TryWriteAttribute(NameTableCache.GetAttributeName(attribute), value);
        }

        /// <summary>
        /// Write the attribute if it's value is not  null
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(AttributeNameTable attribute, Uri value)
        {
            this.TryWriteAttribute(NameTableCache.GetAttributeName(attribute), value);
        }

        /// <summary>
        /// Write the attribute if it's value is not  null
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(AttributeNameTable attribute, TertiaryBoolEnumType value)
        {
            this.TryWriteAttribute(NameTableCache.GetAttributeName(attribute), value);
        }

        /// <summary>
        /// Write the attribute if it's value is not  null
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(AttributeNameTable attribute, TertiaryBool value)
        {
            if (value != null)
            {
                this.TryWriteAttribute(NameTableCache.GetAttributeName(attribute), value.EnumType);
            }
        }

        /// <summary>
        /// Write the attribute if it's value is larger than <see cref="DateTime.MinValue"/>
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(AttributeNameTable attribute, DateTime value)
        {
            this.TryWriteAttribute(NameTableCache.GetAttributeName(attribute), value);
        }

        /// <summary>
        /// Write the attribute and it's value
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(AttributeNameTable attribute, bool value)
        {
            this.TryWriteAttribute(NameTableCache.GetAttributeName(attribute), value);
        }

        /// <summary>
        /// Write the attribute and it's value
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(AttributeNameTable attribute, int value)
        {
            this.TryWriteAttribute(NameTableCache.GetAttributeName(attribute), value);
        }

        /// <summary>
        /// Write the attribute and it's value
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(AttributeNameTable attribute, long value)
        {
            this.TryWriteAttribute(NameTableCache.GetAttributeName(attribute), value);
        }

        /// <summary>
        /// Write the attribute and it's value
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(AttributeNameTable attribute, double value)
        {
            this.TryWriteAttribute(NameTableCache.GetAttributeName(attribute), value);
        }

        /// <summary>
        /// Write the attribute and it's value
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(AttributeNameTable attribute, decimal value)
        {
            this.TryWriteAttribute(NameTableCache.GetAttributeName(attribute), value);
        }

        /// <summary>
        /// Write the attribute if it's value is larger than <see cref="DateTime.MinValue"/>
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(AttributeNameTable attribute, DateTime? value)
        {
            if (value.HasValue)
            {
                this.TryWriteAttribute(attribute, value.Value);
            }
        }

        /// <summary>
        /// Write the attribute and it's value
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(AttributeNameTable attribute, bool? value)
        {
            if (value.HasValue)
            {
                this.TryWriteAttribute(attribute, value.Value);
            }
        }

        /// <summary>
        /// Write the attribute and it's value
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(AttributeNameTable attribute, int? value)
        {
            if (value.HasValue)
            {
                this.TryWriteAttribute(attribute, value.Value);
            }
        }

        /// <summary>
        /// Write the attribute and it's value
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(AttributeNameTable attribute, long? value)
        {
            if (value.HasValue)
            {
                this.TryWriteAttribute(attribute, value.Value);
            }
        }

        /// <summary>
        /// Write the attribute and it's value
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(AttributeNameTable attribute, double? value)
        {
            if (value.HasValue)
            {
                this.TryWriteAttribute(attribute, value.Value);
            }
        }

        /// <summary>
        /// Write the attribute and it's value
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void TryWriteAttribute(AttributeNameTable attribute, decimal? value)
        {
            if (value.HasValue)
            {
                this.TryWriteAttribute(attribute, value.Value);
            }
        }

        /// <summary>
        /// Write annotations
        /// </summary>
        /// <param name="element">
        /// The root annotations element name
        /// </param>
        /// <param name="annotations">
        /// The list of annotations to write
        /// </param>
        protected void WriteAnnotations(ElementNameTable element, ICollection<IAnnotation> annotations)
        {
            if (annotations != null && annotations.Count > 0)
            {
                if (IsTwoPointOne)
                {
                    this.WriteStartElement(this.Namespaces.Common, ElementNameTable.Annotations);
                }
                else
                {
                    this.WriteStartElement(this.DefaultNS, element);
                }

                foreach (IAnnotation annotation in annotations)
                {
                    this.WriteStartElement(this.Namespaces.Common, ElementNameTable.Annotation);
                    if (IsTwoPointOne && !string.IsNullOrEmpty(annotation.Id))
                    {
                        this.WriteAttributeString("id", annotation.Id);
                    }

                    this.TryToWriteElement(this.Namespaces.Common, ElementNameTable.AnnotationTitle, annotation.Title);
                    this.TryToWriteElement(this.Namespaces.Common, ElementNameTable.AnnotationType, annotation.Type);
                    this.TryToWriteElement(this.Namespaces.Common, ElementNameTable.AnnotationURL, annotation.Uri);
                    if (ObjectUtil.ValidCollection(annotation.Text))
                    {
                        foreach (ITextTypeWrapper text in annotation.Text)
                        {
                            this.WriteTextType(this.Namespaces.Common, text, ElementNameTable.AnnotationText);
                        }
                    }

                    this._writer.WriteEndElement();
                }

                this._writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Write annotations
        /// </summary>
        /// <param name="element">
        /// The root annotations element name
        /// </param>
        /// <param name="annotations">
        /// The list of annotations to write
        /// </param>
        protected void WriteAnnotations(ElementNameTable element, ICollection<IAnnotationMutableObject> annotations)
        {
            if (annotations != null && annotations.Count > 0)
            {
                if (IsTwoPointOne)
                {
                    this.WriteStartElement(this.Namespaces.Common, ElementNameTable.Annotations);
                }
                else
                {
                    this.WriteStartElement(this.DefaultNS, element);
                }

                foreach (var annotation in annotations)
                {
                    this.WriteStartElement(this.Namespaces.Common, ElementNameTable.Annotation);
                    if (IsTwoPointOne && !string.IsNullOrEmpty(annotation.Id))
                    {
                        this.WriteAttributeString("id", annotation.Id);
                    }

                    this.TryToWriteElement(this.Namespaces.Common, ElementNameTable.AnnotationTitle, annotation.Title);
                    this.TryToWriteElement(this.Namespaces.Common, ElementNameTable.AnnotationType, annotation.Type);
                    this.TryToWriteElement(this.Namespaces.Common, ElementNameTable.AnnotationURL, annotation.Uri);
                    if (ObjectUtil.ValidCollection(annotation.Text))
                    {
                        foreach (var text in annotation.Text)
                        {
                            this.WriteTextType(this.Namespaces.Common, text, ElementNameTable.AnnotationText);
                        }
                    }

                    this._writer.WriteEndElement();
                }

                this._writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Write the attribute and it's value
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void WriteAttribute(string attribute, bool value)
        {
            this._writer.WriteAttributeString(attribute, XmlConvert.ToString(value));
        }

        /// <summary>
        /// Write the attribute and it's value
        /// </summary>
        /// <param name="attribute">
        /// The attribute name
        /// </param>
        /// <param name="value">
        /// The attribute value
        /// </param>
        protected void WriteAttribute(AttributeNameTable attribute, bool value)
        {
            this.WriteAttribute(NameTableCache.GetAttributeName(attribute), value);
        }

        /// <summary>
        /// Write an attribute with the specified name and value
        /// </summary>
        /// <param name="name">
        /// Attribute name
        /// </param>
        /// <param name="value">
        /// Attribute value
        /// </param>
        protected void WriteAttributeString(string name, string value)
        {
            this._writer.WriteAttributeString(name, value);
        }

        /// <summary>
        /// Write an attribute with the specified name and value
        /// </summary>
        /// <param name="name">
        /// Attribute name
        /// </param>
        /// <param name="value">
        /// Attribute value
        /// </param>
        protected void WriteAttributeString(AttributeNameTable name, string value)
        {
            this.WriteAttributeString(NameTableCache.GetAttributeName(name), value);
        }

        /// <summary>
        /// Write an attribute with the specified name and value
        /// </summary>
        /// <param name="namespacePrefixPair">
        /// The XML prefix
        /// </param>
        /// <param name="name">
        /// Attribute name
        /// </param>
        /// <param name="value">
        /// Attribute value
        /// </param>
        protected void WriteAttributeString(NamespacePrefixPair namespacePrefixPair, string name, string value)
        {
            this._writer.WriteAttributeString(namespacePrefixPair.Prefix, name, namespacePrefixPair.NS, value);
        }

        /// <summary>
        /// Write an attribute with the specified name and value
        /// </summary>
        /// <param name="namespacePrefixPair">
        /// The XML prefix
        /// </param>
        /// <param name="name">
        /// Attribute name
        /// </param>
        /// <param name="value">
        /// Attribute value
        /// </param>
        protected void WriteAttributeString(
            NamespacePrefixPair namespacePrefixPair, AttributeNameTable name, string value)
        {
            this.WriteAttributeString(namespacePrefixPair, NameTableCache.GetAttributeName(name), value);
        }

        /// <summary>
        /// Write an attribute with the specified name and value
        /// </summary>
        /// <param name="prefix">
        /// The XML prefix
        /// </param>
        /// <param name="name">
        /// Attribute name
        /// </param>
        /// <param name="value">
        /// Attribute value
        /// </param>
        protected void WriteAttributeString(string prefix, string name, string value)
        {
            this._writer.WriteAttributeString(prefix, name, null, value);
        }

        /// <summary>
        /// Write an attribute with the specified name and value
        /// </summary>
        /// <param name="prefix">
        /// The XML prefix
        /// </param>
        /// <param name="name">
        /// Attribute name
        /// </param>
        /// <param name="value">
        /// Attribute value
        /// </param>
        protected void WriteAttributeString(string prefix, string name, Uri value)
        {
            this._writer.WriteAttributeString(prefix, name, null, value.ToString());
        }

        /// <summary>
        /// Write the element
        /// </summary>
        /// <param name="prefix">
        /// The namespace prefix
        /// </param>
        /// <param name="element">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void WriteElement(string prefix, ElementNameTable element, string value)
        {
            this._writer.WriteElementString(prefix, NameTableCache.GetElementName(element), null, value);
        }

        /// <summary>
        /// Write the element with it's <c>xs:boolean</c> value
        /// </summary>
        /// <param name="namespacePrefix">
        /// The namespace prefix
        /// </param>
        /// <param name="element">
        /// The element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void WriteElement(NamespacePrefixPair namespacePrefix, ElementNameTable element, bool value)
        {
            this.WriteElement(namespacePrefix, NameTableCache.GetElementName(element), value);
        }

        /// <summary>
        /// Write the element with it's <c>xs:boolean</c> value
        /// </summary>
        /// <param name="namespacePrefix">
        /// The namespace prefix
        /// </param>
        /// <param name="element">
        /// The element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void WriteElement(NamespacePrefixPair namespacePrefix, ElementNameTable element, int value)
        {
            this.WriteElement(namespacePrefix, NameTableCache.GetElementName(element), value);
        }

        /// <summary>
        /// Write the element with it's <c>xs:boolean</c> value
        /// </summary>
        /// <param name="namespacePrefix">
        /// The namespace prefix
        /// </param>
        /// <param name="element">
        /// The element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void WriteElement(NamespacePrefixPair namespacePrefix, string element, bool value)
        {
            this._writer.WriteElementString(
                namespacePrefix.Prefix, element, namespacePrefix.NS, XmlConvert.ToString(value));
        }

        /// <summary>
        /// Write the element with it's <c>xs:boolean</c> value
        /// </summary>
        /// <param name="namespacePrefix">
        /// The namespace prefix
        /// </param>
        /// <param name="element">
        /// The element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void WriteElement(NamespacePrefixPair namespacePrefix, string element, int value)
        {
            this._writer.WriteElementString(
                namespacePrefix.Prefix, element, namespacePrefix.NS, XmlConvert.ToString(value));
        }

        /// <summary>
        /// Write the element
        /// </summary>
        /// <param name="namespacePrefix">
        /// The namespace prefix
        /// </param>
        /// <param name="element">
        /// The element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void WriteElement(NamespacePrefixPair namespacePrefix, string element, string value)
        {
            this._writer.WriteElementString(namespacePrefix.Prefix, element, namespacePrefix.NS, value);
        }

        /// <summary>
        /// Write the element
        /// </summary>
        /// <param name="namespacePrefix">
        /// The namespace prefix
        /// </param>
        /// <param name="element">
        /// The element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void WriteElement(NamespacePrefixPair namespacePrefix, ElementNameTable element, string value)
        {
            this._writer.WriteElementString(
                namespacePrefix.Prefix, NameTableCache.GetElementName(element), namespacePrefix.NS, value);
        }

        /// <summary>
        /// Write the element with it's <c>xs:boolean</c> value
        /// </summary>
        /// <param name="prefix">
        /// The namespace prefix
        /// </param>
        /// <param name="element">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        /// <param name="value">
        /// The elements text
        /// </param>
        protected void WriteElement(string prefix, ElementNameTable element, bool value)
        {
            this._writer.WriteElementString(
                prefix, NameTableCache.GetElementName(element), null, XmlConvert.ToString(value));
        }

        /// <summary>
        ///     Write end document and flush
        /// </summary>
        protected void WriteEndDocument()
        {
            if (this.Wrapped)
            {
                return;
            }

            this._writer.WriteEndDocument();
            this._writer.Flush();
        }

        /// <summary>
        ///     Close one element
        /// </summary>
        protected void WriteEndElement()
        {
            this._writer.WriteEndElement();
        }

        /// <summary>
        /// Write header using the <see cref="HeaderWriter"/>
        /// </summary>
        /// <param name="header">
        /// The <see cref="IHeader"/> to write
        /// </param>
        protected void WriteMessageHeader(IHeader header)
        {
            new HeaderWriter(this._writer, this.Namespaces, this.TargetSchema, this.HeaderRetrievalManager).WriteHeader(header);
        }

        /// <summary>
        /// This method is used to write the root xml tag and message  tag
        ///     with their corresponding attributes
        /// </summary>
        /// <param name="message">
        /// The message to write
        /// </param>
        protected void WriteMessageTag(ElementNameTable message)
        {
            if (!this._wrapped)
            {
                this._writer.WriteStartDocument();
            }

            this.WriteStartElement(this.Namespaces.Message, message);
            var namespaces = new[]
                                 {
                                     this.DefaultNS, this.Namespaces.Message, this.Namespaces.Common, 
                                     this.Namespaces.Compact, this.Namespaces.Cross, this.Namespaces.Generic, 
                                     this.Namespaces.Query, this.Namespaces.Structure, this.Namespaces.Utility, 
                                     this.Namespaces.Registry
                                 };

            var namespacesWrote = new HashSet<NamespacePrefixPair>();
            for (int i = 0; i < namespaces.Length; i++)
            {
                NamespacePrefixPair nameSpace = namespaces[i];
                if (nameSpace != null && !string.IsNullOrWhiteSpace(nameSpace.NS) && !namespacesWrote.Contains(nameSpace))
                {
                    this.WriteNamespaceDecl(nameSpace);
                    namespacesWrote.Add(nameSpace);
                }
            }

            this._writer.WriteAttributeString(
                XmlConstants.XmlSchemaPrefix, 
                XmlConstants.SchemaLocation, 
                XmlConstants.XmlSchemaNS, 
                this.Namespaces.SchemaLocation);
        }

        /// <summary>
        /// Write the specified <paramref name="ns"/> declaration
        /// </summary>
        /// <param name="ns">
        /// The namespace with prefix
        /// </param>
        protected void WriteNamespaceDecl(NamespacePrefixPair ns)
        {
            this.WriteAttributeString(XmlConstants.Xmlns, ns.Prefix, ns.NS);
        }

        /// <summary>
        /// Write a Start Element to the given XmlTextWriter using the given prefix and the <see cref="ElementNameTable"/> name
        /// </summary>
        /// <param name="ns">
        /// The namespace and prefix
        /// </param>
        /// <param name="element">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        protected void WriteStartElement(NamespacePrefixPair ns, ElementNameTable element)
        {
            this.WriteStartElement(ns, NameTableCache.GetElementName(element));
        }

        /// <summary>
        /// Write a Start Element to the given XmlTextWriter using the given prefix and the <see cref="ElementNameTable"/> name
        /// </summary>
        /// <param name="ns">
        /// The namespace and prefix
        /// </param>
        /// <param name="element">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        protected void WriteStartElement(NamespacePrefixPair ns, string element)
        {
            this._writer.WriteStartElement(ns.Prefix, element, ns.NS);
        }

        /// <summary>
        /// Write a Start Element to the given XmlTextWriter using the given prefix and the <see cref="ElementNameTable"/> name
        /// </summary>
        /// <param name="prefix">
        /// The namespace prefix
        /// </param>
        /// <param name="element">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        protected void WriteStartElement(string prefix, ElementNameTable element)
        {
            this._writer.WriteStartElement(prefix, NameTableCache.GetElementName(element), null);
        }

        /// <summary>
        /// Write a Start Element to the given XmlTextWriter using the given prefix and the <see cref="ElementNameTable"/> name
        /// </summary>
        /// <param name="element">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        protected void WriteStartElement(string element)
        {
            this._writer.WriteStartElement(element);
        }

        /// <summary>
        /// Write a Start Element to the given XmlTextWriter using the given prefix and the <see cref="ElementNameTable"/> name
        /// </summary>
        /// <param name="element">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        protected void WriteStartElement(ElementNameTable element)
        {
            this._writer.WriteStartElement(NameTableCache.GetElementName(element));
        }

        /// <summary>
        /// Write a Start Element to the given XmlTextWriter using the given prefix and the <see cref="ElementNameTable"/> name
        /// </summary>
        /// <param name="element">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        /// <param name="ns">
        /// The namespace
        /// </param>
        protected void WriteStartElement(string element, string ns)
        {
            this._writer.WriteStartElement(element, ns);
        }

        /// <summary>
        /// Write a Start Element to the given XmlTextWriter using the given prefix and the <see cref="ElementNameTable"/> name
        /// </summary>
        /// <param name="element">
        /// The <see cref="ElementNameTable"/> element name
        /// </param>
        /// <param name="ns">
        /// The namespace
        /// </param>
        protected void WriteStartElement(ElementNameTable element, string ns)
        {
            this._writer.WriteStartElement(NameTableCache.GetElementName(element), ns);
        }

        /// <summary>
        /// This is an internal method that is used to write a <see cref="ITextTypeWrapper"/>
        ///     The method create a xml element named by the name parameter.
        ///     It will only write the xml element if the <see cref="ITextTypeWrapper"/> is not null or empty
        /// </summary>
        /// <param name="textObject">
        /// The <see cref="ITextTypeWrapper"/> object containing the data to be written
        /// </param>
        /// <param name="name">
        /// The name of the xml element
        /// </param>
        protected void WriteTextType(ITextTypeWrapper textObject, ElementNameTable name)
        {
            this.WriteTextType(NamespacePrefixPair.Empty, textObject, name);
        }

        /// <summary>
        /// This is an internal method that is used to write the <paramref name="textObject"/>
        ///     The method create a xml element named by the name parameter.
        ///     It will only write the xml element if the text inside <paramref name="textObject"/> is not null or empty
        /// </summary>
        /// <param name="namespacePrefix">
        /// The namespace prefix pair
        /// </param>
        /// <param name="textObject">
        /// The <see cref="KeyValuePair{TKey,TValue}"/> object containing the data to be written, where key is the locale and value the text value
        /// </param>
        /// <param name="name">
        /// The name of the xml element
        /// </param>
        protected void WriteTextType(
            NamespacePrefixPair namespacePrefix, IDictionary<string, string> textObject, ElementNameTable name)
        {
            foreach (KeyValuePair<string, string> valuePair in textObject)
            {
                this.WriteTextType(namespacePrefix, valuePair, name);
            }
        }

        /// <summary>
        /// This is an internal method that is used to write the <paramref name="textObject"/>
        ///     The method create a xml element named by the name parameter.
        ///     It will only write the xml element if the text inside <paramref name="textObject"/> is not null or empty
        /// </summary>
        /// <param name="namespacePrefix">
        /// The namespace Prefix.
        /// </param>
        /// <param name="textObject">
        /// The <see cref="KeyValuePair{TKey,TValue}"/> object containing the data to be written, where key is the locale and value the text value
        /// </param>
        /// <param name="name">
        /// The name of the xml element
        /// </param>
        protected void WriteTextType(
            NamespacePrefixPair namespacePrefix, KeyValuePair<string, string> textObject, ElementNameTable name)
        {
            this.WriteTextType(namespacePrefix, name, textObject.Value, textObject.Key);
        }

        /// <summary>
        /// This is an internal method that is used to write a <see cref="ITextTypeWrapper" />
        /// The method create a XML element named by the name parameter.
        /// It will only write the XML element if the <see cref="ITextTypeWrapper.Value" /> is not null or empty
        /// </summary>
        /// <param name="namespacePrefix">The namespace prefix that should be used</param>
        /// <param name="textObject">The text object.</param>
        /// <param name="name">The name.</param>
        protected void WriteTextType(NamespacePrefixPair namespacePrefix, IList<ITextTypeWrapper> textObject, ElementNameTable name)
        {
            if (textObject != null && textObject.Count > 0)
            {
                this.WriteTextType(namespacePrefix, textObject[0], name);
            }
        }

        /// <summary>
        /// This is an internal method that is used to write a <see cref="ITextTypeWrapper"/>
        ///     The method create a xml element named by the name parameter.
        ///     It will only write the xml element if the <see cref="ITextTypeWrapper.Value"/> is not null or empty
        /// </summary>
        /// <param name="namespacePrefix">
        /// The namespace prefix that should be used
        /// </param>
        /// <param name="textObject">
        /// The <see cref="ITextTypeWrapper"/> object containing the data to be written
        /// </param>
        /// <param name="name">
        /// The name of the xml element
        /// </param>
        protected void WriteTextType(
            NamespacePrefixPair namespacePrefix, ITextTypeWrapper textObject, ElementNameTable name)
        {
            string value = textObject.Value;
            string locale = textObject.Locale;
            this.WriteTextType(namespacePrefix, name, value, locale);
        }

        /// <summary>
        /// This is an internal method that is used to write a <see cref="ITextTypeWrapper"/>
        ///     The method create a xml element named by the name parameter.
        ///     It will only write the xml element if the <see cref="ITextTypeWrapper.Value"/> is not null or empty
        /// </summary>
        /// <param name="namespacePrefix">
        /// The namespace prefix that should be used
        /// </param>
        /// <param name="textObject">
        /// The <see cref="ITextTypeWrapper"/> object containing the data to be written
        /// </param>
        /// <param name="name">
        /// The name of the xml element
        /// </param>
        protected void WriteTextType(
            NamespacePrefixPair namespacePrefix, ITextTypeWrapperMutableObject textObject, ElementNameTable name)
        {
            string value = textObject.Value;
            string locale = textObject.Locale;
            this.WriteTextType(namespacePrefix, name, value, locale);
        }

        /// <summary>
        /// This is an internal method that is used to write a <see cref="ITextTypeWrapper"/>
        ///     The method create a xml element named by the name parameter.
        ///     It will only write the xml element if the <see cref="ITextTypeWrapper.Value"/> is not null or empty
        /// </summary>
        /// <param name="prefix">
        /// The namespace prefix that should be used
        /// </param>
        /// <param name="textObject">
        /// The <see cref="ITextTypeWrapper"/> object containing the data to be written
        /// </param>
        /// <param name="name">
        /// The name of the xml element
        /// </param>
        protected void WriteTextType(string prefix, ITextTypeWrapper textObject, ElementNameTable name)
        {
            string value = textObject.Value;
            string locale = textObject.Locale;
            this.WriteTextType(new NamespacePrefixPair(prefix), name, value, locale);
        }

        /// <summary>
        /// This is an internal method that is used to write the <paramref name="value"/> for the specified
        ///     <paramref name="locale"/>
        ///     The method create a XML element named by the name parameter.
        ///     It will only write the XML element if the text inside <paramref name="value"/> is not null or empty
        /// </summary>
        /// <param name="namespacePrefix">
        /// The namespace prefix
        /// </param>
        /// <param name="name">
        /// The name of the XML element
        /// </param>
        /// <param name="value">
        /// The text value.
        /// </param>
        /// <param name="locale">
        /// The locale.
        /// </param>
        protected void WriteTextType(
            NamespacePrefixPair namespacePrefix, ElementNameTable name, string value, string locale)
        {
            if (!string.IsNullOrEmpty(value))
            {
                this._writer.WriteStartElement(
                    namespacePrefix.Prefix, NameTableCache.GetElementName(name), namespacePrefix.NS);
                if (!string.IsNullOrEmpty(locale))
                {
                    this._writer.WriteAttributeString(XmlConstants.XmlPrefix, XmlConstants.LangAttribute, null, locale);
                }

                this._writer.WriteString(value);
                this._writer.WriteEndElement();
            }
        }

        #endregion
    }
}