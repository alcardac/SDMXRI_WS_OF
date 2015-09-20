// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureType.cs" company="EUROSTAT">
//   EUPL
// </copyright>
// <summary>
//   The structure type partial class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Xml.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;

    using Xml.Schema.Linq;

    /// <summary>
    /// The structure type partial class.
    /// </summary>
    public partial class StructureType
    {
        #region Static Fields

        /// <summary>
        /// The contents type.
        /// </summary>
        private static readonly Type _contentsType = typeof(StructuresType);

        /// <summary>
        /// The contents element name
        /// </summary>
        private static readonly XName _contentsXName = XName.Get(
            "Structures", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");

        /// <summary>
        /// The footer type.
        /// </summary>
        private static readonly Type _footerType = typeof(Footer.Footer);

        /// <summary>
        /// The _footer element name.
        /// </summary>
        private static readonly XName _footerXName = XName.Get(
            "Footer", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer");

        /// <summary>
        /// The header type.
        /// </summary>
        private static readonly Type _headerType = typeof(StructureHeaderType);

        /// <summary>
        /// The _header element name.
        /// </summary>
        private static readonly XName _headerXName = XName.Get(
            "Header", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");

        /// <summary>
        /// The local element dictionary.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Dictionary<XName, Type> _localElementDictionary = new Dictionary<XName, Type>();

        /// <summary>
        /// The validation states.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static FSM validationStates;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="StructureType"/> class.
        /// </summary>
        static StructureType()
        {
            BuildElementDictionary();
            InitFsm();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public new StructureHeaderType Header
        {
            get
            {
                XElement x = this.GetElement(_headerXName);
                return (StructureHeaderType)x;
            }

            set
            {
                this.SetElement(_headerXName, value);
            }
        }

        /// <summary>
        /// Gets or sets the structures.
        /// </summary>
        public StructuresType Structures
        {
            get
            {
                XElement x = this.GetElement(_contentsXName);
                return (StructuresType)x;
            }

            set
            {
                this.SetElement(_contentsXName, value);
            }
        }

        #endregion

        #region Explicit Interface Properties

        /// <summary>
        /// Gets the local elements dictionary.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Dictionary<XName, Type> IXMetaData.LocalElementsDictionary
        {
            get
            {
                return _localElementDictionary;
            }
        }

        #endregion

        #region Explicit Interface Methods

        /// <summary>
        /// Gets validation states.
        /// </summary>
        /// <returns>
        /// The validation states.
        /// </returns>
        FSM IXMetaData.GetValidationStates()
        {
            return validationStates;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Build element dictionary.
        /// </summary>
        private static void BuildElementDictionary()
        {
            _localElementDictionary.Add(_headerXName, _headerType);
            _localElementDictionary.Add(_contentsXName, _contentsType);
            _localElementDictionary.Add(_footerXName, _footerType);
        }

        /// <summary>
        /// Initialize the <c>FSM</c>
        /// </summary>
        private static void InitFsm()
        {
            // TODO check if the transition numbers just need to be in order or need to have specific values ?!? 
            var transitions = new Dictionary<int, Transitions>
                {
                    { 1, new Transitions(new SingleTransition(_headerXName, 2)) }, 
                    { 2, new Transitions(new SingleTransition(_contentsXName, 4)) }, 
                    {
                        4, 
                        new Transitions(
                        new SingleTransition(_footerXName, 6), 
                        new SingleTransition(
                        new WildCard("##targetNamespace", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message"), 2))
                    }
                };
            validationStates = new FSM(1, new Set<int>(new[] { 2, 4, 6 }), transitions);
        }

        #endregion
    }
}