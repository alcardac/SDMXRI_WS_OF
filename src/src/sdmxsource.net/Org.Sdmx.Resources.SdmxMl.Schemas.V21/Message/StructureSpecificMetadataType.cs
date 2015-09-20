// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureSpecificMetadataType.cs" company="EUROSTAT">
//   EUPL
// </copyright>
// <summary>
//   The structure specific metadata type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Xml.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.MetaData.StructureSpecific;

    using Xml.Schema.Linq;

    /// <summary>
    /// The structure specific metadata type.
    /// </summary>
    public partial class StructureSpecificMetadataType
    {
        #region Static Fields

        /// <summary>
        /// The contents type.
        /// </summary>
        private static readonly Type _contentsType = typeof(MetadataSetType);

        /// <summary>
        /// The contents element name
        /// </summary>
        private static readonly XName _contentsXName = XName.Get(
            "MetadataSet", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");

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
        private static readonly Type _headerType = typeof(StructureSpecificMetadataHeaderType);

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

        #region Fields

        /// <summary>
        /// The dataset list
        /// </summary>
        private XTypedList<MetadataSetType> _dataSetType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="StructureSpecificMetadataType"/> class.
        /// </summary>
        static StructureSpecificMetadataType()
        {
            BuildElementDictionary();
            InitFsm();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the Metadata DataSet.
        /// </summary>
        public IList<MetadataSetType> DataSet
        {
            get
            {
                if (this._dataSetType == null)
                {
                    this._dataSetType = new XTypedList<MetadataSetType>(
                        this, LinqToXsdTypeManager.Instance, _contentsXName);
                }

                return this._dataSetType;
            }

            set
            {
                if (value == null)
                {
                    this._dataSetType = null;
                }
                else
                {
                    if (this._dataSetType == null)
                    {
                        this._dataSetType = XTypedList<MetadataSetType>.Initialize(
                            this, LinqToXsdTypeManager.Instance, value, _contentsXName);
                    }
                    else
                    {
                        XTypedServices.SetList(this._dataSetType, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public new StructureSpecificMetadataHeaderType Header
        {
            get
            {
                XElement x = this.GetElement(_headerXName);
                return (StructureSpecificMetadataHeaderType)x;
            }

            set
            {
                this.SetElement(_headerXName, value);
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