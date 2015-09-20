// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Reader.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The reader.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxParseBase.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;

    using Estat.Sri.SdmxParseBase.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Util.Exception;
    using Org.Sdmxsource.Sdmx.Api.Exception;

    /// <summary>
    ///     The reader.
    /// </summary>
    public abstract class Reader : IoBase
    {
        #region Fields

        /// <summary>
        ///     The current XML attributes in a name-value map
        /// </summary>
        private readonly IDictionary<string, string> _attributes = new Dictionary<string, string>(
            StringComparer.Ordinal);

        /// <summary>
        ///     The stack of elements to parsed, used by xml parser.
        /// </summary>
        private readonly Stack<object> _elements = new Stack<object>();

        /// <summary>
        ///     The list of <see cref="ElementTypeSwitch.CaseInfo" /> with methods and types that handle elements without text
        /// </summary>
        private readonly Collection<ElementTypeSwitch.CaseInfo> _handleElementRules =
            new Collection<ElementTypeSwitch.CaseInfo>();

        /// <summary>
        ///     The list of <see cref="ElementTypeSwitch.CaseInfo" /> with methods and types that handle elements with text
        /// </summary>
        private readonly Collection<ElementTypeSwitch.CaseInfo> _handleTextRules =
            new Collection<ElementTypeSwitch.CaseInfo>();

        /// <summary>
        ///     The <c>xsd</c> schemas used by sdmx
        /// </summary>
        private readonly XmlReaderSettings _settings;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Reader"/> class.
        /// </summary>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        /// <param name="schema">
        /// The schema.
        /// </param>
        protected Reader(SdmxNamespaces namespaces, SdmxSchema schema)
            : base(namespaces, schema)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Reader"/> class.
        /// </summary>
        /// <param name="schema">
        /// The schema.
        /// </param>
        protected Reader(SdmxSchema schema)
            : this(schema, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Reader"/> class.
        /// </summary>
        /// <param name="schema">
        /// The schema.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        protected Reader(SdmxSchema schema, XmlReaderSettings settings)
            : this(null, schema, settings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Reader"/> class.
        /// </summary>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        /// <param name="schema">
        /// The schema.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        protected Reader(SdmxNamespaces namespaces, SdmxSchema schema, XmlReaderSettings settings)
            : base(namespaces, schema)
        {
            this._settings = settings;
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate for handling elements
        /// </summary>
        /// <typeparam name="T">
        /// The type of the <paramref name="parent"/>
        /// </typeparam>
        /// <param name="parent">
        /// The parent object
        /// </param>
        /// <param name="localName">
        /// the local name string as object
        /// </param>
        /// <returns>
        /// The current object or null
        /// </returns>
        protected delegate object HandleComplexElement<in T>(T parent, object localName);

        /// <summary>
        /// Delegate for handling simple element types with only text
        /// </summary>
        /// <typeparam name="T">
        /// The type of the <paramref name="parent"/>
        /// </typeparam>
        /// <param name="parent">
        /// The parent object
        /// </param>
        /// <param name="localName">
        /// the local name string as object
        /// </param>
        protected delegate void HandleText<in T>(T parent, object localName);

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the current XML attributes in a name-value map
        /// </summary>
        protected IDictionary<string, string> Attributes
        {
            get
            {
                return this._attributes;
            }
        }

        /// <summary>
        ///     Gets the stack of elements to parsed, used by xml parser.
        /// </summary>
        protected Stack<object> Elements
        {
            get
            {
                return this._elements;
            }
        }

        /// <summary>
        ///     Gets the <c>xsd</c> schemas used by sdmx
        /// </summary>
        protected XmlReaderSettings Settings
        {
            get
            {
                return this._settings;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Handle validation errors
        /// </summary>
        /// <param name="sender">
        /// The source
        /// </param>
        /// <param name="e">
        /// The <see cref="ValidationEventArgs"/>
        /// </param>
        public static void SettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            throw new SdmxSyntaxException(e.Exception, ExceptionCode.XmlParseException, e.Message, e.Severity);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add element handler, <paramref name="handleTextMethod"/> for specific type. The order each handler is added is important
        /// </summary>
        /// <typeparam name="T">
        /// The parent type
        /// </typeparam>
        /// <param name="handleTextMethod">
        /// Text handling method
        /// </param>
        protected void AddHandleElement<T>(HandleComplexElement<T> handleTextMethod)
        {
            this._handleElementRules.Add(ElementTypeSwitch.Case(handleTextMethod));
        }

        /// <summary>
        /// Add element text handler, <paramref name="handleTextMethod"/> for specific type. The order each handler is added is important
        /// </summary>
        /// <typeparam name="T">
        /// The parent type
        /// </typeparam>
        /// <param name="handleTextMethod">
        /// Text handling method
        /// </param>
        protected void AddHandleText<T>(HandleText<T> handleTextMethod)
        {
            this._handleTextRules.Add(ElementTypeSwitch.Case(handleTextMethod));
        }

        /// <summary>
        /// Create an <see cref="XmlReader"/> object with <see cref="_settings"/> and the specified
        ///     <paramref name="textReader"/>
        /// </summary>
        /// <param name="textReader">
        /// The input <see cref="TextReader"/>
        /// </param>
        /// <returns>
        /// an <see cref="XmlReader"/>
        /// </returns>
        protected XmlReader CreateXmlReader(TextReader textReader)
        {
            return XmlReader.Create(textReader, this._settings);
        }

        /// <summary>
        /// Execute the element handler for the <paramref name="parent"/> <see cref="Type"/> with the
        ///     <paramref name="localName"/>
        ///     . The handlers are added using the <see cref="AddHandleElement{T}"/>
        /// </summary>
        /// <param name="parent">
        /// The parent object. The handler that will be executed depends on it's type
        /// </param>
        /// <param name="localName">
        /// The local name string as an object
        /// </param>
        /// <returns>
        /// The execute handle element.
        /// </returns>
        protected object ExecuteHandleElement(object parent, object localName)
        {
            return ElementTypeSwitch.Do<object>(parent, localName, this._handleElementRules);
        }

        /// <summary>
        /// Execute the text handler for the <paramref name="parent"/> <see cref="Type"/> with the <paramref name="localName"/> . The handlers are added using the
        ///     <see cref="AddHandleText{T}"/>
        /// </summary>
        /// <param name="parent">
        /// The parent object. The handler that will be executed depends on it's type
        /// </param>
        /// <param name="localName">
        /// The local name string as an object
        /// </param>
        /// <returns>
        /// <c>true</c> if matching type existed and executed. Else <c>false</c>
        /// </returns>
        protected bool ExecuteHandleText(object parent, object localName)
        {
            return ElementTypeSwitch.Do<bool>(parent, localName, this._handleTextRules);
        }

        /// <summary>
        /// Read the header and return the <see cref="IHeader"/>
        /// </summary>
        /// <param name="reader">
        /// The <see cref="XmlReader"/> to read the header from
        /// </param>
        /// <returns>
        /// the <see cref="IHeader"/>
        /// </returns>
        protected IHeader ReadHeader(XmlReader reader)
        {
            var headerReader = new HeaderReader(this.Namespaces, this.TargetSchema);
            return headerReader.Read(reader);
        }

        #endregion

        /// <summary>
        ///     TypeSwitch from
        ///     <see
        ///         href="http://stackoverflow.com/questions/298976/c-is-there-a-better-alternative-than-this-to-switch-on-type/299120#299120">
        ///         HERE
        ///     </see>
        ///     Modified to use
        ///     <see cref="HandleText{T}" />
        /// </summary>
        protected static class ElementTypeSwitch
        {
            #region Methods

            /// <summary>
            /// A case statement
            /// </summary>
            /// <typeparam name="T">
            /// The target type
            /// </typeparam>
            /// <param name="action">
            /// The action to take
            /// </param>
            /// <returns>
            /// A <see cref="CaseInfo"/> object
            /// </returns>
            internal static CaseInfo Case<T>(HandleText<T> action)
            {
                return new CaseInfo
                           {
                               ElementHandler = (x, y) =>
                                   {
                                       action((T)x, y);
                                       return true;
                                   }, 
                               Target = typeof(T)
                           };
            }

            /// <summary>
            /// A case statement
            /// </summary>
            /// <typeparam name="T">
            /// The target type
            /// </typeparam>
            /// <param name="action">
            /// The action to take
            /// </param>
            /// <returns>
            /// A <see cref="CaseInfo"/> object
            /// </returns>
            internal static CaseInfo Case<T>(HandleComplexElement<T> action)
            {
                return new CaseInfo { ElementHandler = (x, y) => action((T)x, y), Target = typeof(T) };
            }

            /// <summary>
            /// Do the type switch
            /// </summary>
            /// <typeparam name="T">
            /// The type of the <paramref name="parent"/>
            /// </typeparam>
            /// <param name="parent">
            /// The source object
            /// </param>
            /// <param name="localName">
            /// The element local name
            /// </param>
            /// <param name="cases">
            /// The list of cases
            /// </param>
            /// <returns>
            /// True if it succeeds
            /// </returns>
            internal static T Do<T>(object parent, object localName, IEnumerable<CaseInfo> cases)
            {
                Type type = parent.GetType();
                foreach (CaseInfo entry in cases)
                {
                    if (entry != null && entry.Target.IsAssignableFrom(type))
                    {
                        return (T)entry.ElementHandler(parent, localName);
                    }
                }

                return default(T);
            }

            #endregion

            /// <summary>
            ///     Case statement information
            /// </summary>
            internal class CaseInfo
            {
                #region Public Properties

                /// <summary>
                ///     Gets or sets the <see cref="HandleComplexElement{T}" /> to execute
                /// </summary>
                public HandleComplexElement<object> ElementHandler { get; set; }

                /// <summary>
                ///     Gets or sets the target type
                /// </summary>
                public Type Target { get; set; }

                #endregion
            }
        }
    }
}