// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataflowReaderV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The dataflow reader v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;

    using Estat.Sri.SdmxParseBase.Helper;
    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxStructureMutableParser.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    ///     The dataflow reader v 2.
    /// </summary>
    internal class DataflowReaderV2 : StructureReaderBaseV20
    {
        #region Fields

        /// <summary>
        ///     The current category ids
        /// </summary>
        private readonly List<string> _currentCategoryIds = new List<string>();

        /// <summary>
        ///     The current dataflow
        /// </summary>
        private IDataflowMutableObject _currentDataflow;

        /// <summary>
        ///     The current reference info.
        /// </summary>
        private IReferenceInfo _currentReferenceInfo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataflowReaderV2"/> class.
        /// </summary>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public DataflowReaderV2(SdmxNamespaces namespaces)
            : base(namespaces)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handle end element.
        /// </summary>
        /// <param name="localname">
        /// The element local name.
        /// </param>
        protected override void HandleEndElement(object localname)
        {
            if (NameTableCache.IsElement(localname, ElementNameTable.KeyFamilyRef))
            {
                Debug.Assert(this._currentReferenceInfo != null, "_currentReferenceInfo is null.", "local name: {0}", localname);
                Debug.Assert(this._currentDataflow != null, "_currentDataflow is null.", "local name: {0}", localname);
                if (this._currentReferenceInfo != null && this._currentDataflow != null)
                {
                    this._currentDataflow.DataStructureRef = this._currentReferenceInfo.CreateReference(new string[] { });
                }

                this._currentReferenceInfo = null;
            }
            else if (NameTableCache.IsElement(localname, ElementNameTable.CategoryRef))
            {
                Debug.Assert(this._currentReferenceInfo != null, "_currentReferenceInfo is null.", "local name: {0}", localname);
                Debug.Assert(this._currentDataflow != null, "_currentDataflow is null.", "local name: {0}", localname);
                if (this._currentReferenceInfo != null && this._currentDataflow != null)
                {
                    ICategorisationMutableObject categorisation = new CategorisationMutableCore();
                    categorisation.StructureReference = CreateReference(this._currentDataflow);
                    categorisation.CategoryReference = this._currentReferenceInfo.CreateReference(this._currentCategoryIds.ToArray());

                    categorisation.AgencyId = this._currentDataflow.AgencyId;
                    categorisation.Id = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", categorisation.CategoryReference.GetHashCode(), categorisation.StructureReference.GetHashCode());

                    categorisation.Names.AddAll(this._currentDataflow.Names);

                    this.Structure.AddCategorisation(categorisation);

                    this._currentCategoryIds.Clear();
                }

                this._currentReferenceInfo = null;
            }
        }

        /// <summary>
        /// Handles the Structure top level elements
        ///     This includes Codelist
        /// </summary>
        /// <param name="parent">
        /// The parent <see cref="IMutableObjects"/>
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        protected override ElementActions HandleTopLevel(IMutableObjects parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.Dataflow))
            {
                var dataflow = new DataflowMutableCore();
                ParseAttributes(dataflow, this.Attributes);
                parent.AddDataflow(dataflow);
                this._currentDataflow = dataflow;
                actions = this.AddNameableAction(dataflow, this.HandleChildElements);
            }

            return actions;
        }

        /// <summary>
        /// Handles the CategoryID element child elements
        /// </summary>
        /// <param name="parent">
        /// The parent CategoryIDBean object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        private ElementActions HandleChildElements(List<string> parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.CategoryID))
            {
                actions = this.AddSimpleAction(parent, this.HandleTextChildElement);
            }

            return actions;
        }

        /// <summary>
        /// Handles the Dataflow element child elements
        /// </summary>
        /// <param name="parent">
        /// The parent IDataflowMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        private ElementActions HandleChildElements(IDataflowMutableObject parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.KeyFamilyRef))
            {
                IReferenceInfo reference = new ReferenceInfo(SdmxStructureEnumType.Dsd);
                this._currentReferenceInfo = reference;
                actions = this.AddReferenceAction(reference, DoNothingComplex, this.HandleTextChildElementKeyFamily);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.CategoryRef))
            {
                IReferenceInfo reference = new ReferenceInfo(SdmxStructureEnumType.Category);
                this._currentReferenceInfo = reference;
                this._currentCategoryIds.Clear();
                actions = this.AddReferenceAction(reference, this.HandleChildElementsCategory, this.HandleTextChildElementCategoryRef);
            }

            return actions;
        }

        /// <summary>
        /// Handles the CategoryRef element child elements
        /// </summary>
        /// <param name="parent">
        /// The parent CategoryRefBean object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        private ElementActions HandleChildElementsCategory(IReferenceInfo parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.CategoryID))
            {
                actions = this.BuildElementActions(this._currentCategoryIds, this.HandleChildElements, this.HandleTextChildElement);
            }

            return actions;
        }

        /// <summary>
        /// Handle the text elements under CategoryID
        /// </summary>
        /// <param name="parent">
        /// The parent
        /// </param>
        /// <param name="localName">
        /// The local name of the XML tag
        /// </param>
        private void HandleTextChildElement(List<string> parent, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.ID))
            {
                parent.Add(this.Text);
            }
        }

        /// <summary>
        /// The handle text child element.
        /// </summary>
        /// <param name="categoryRef">
        /// The category ref.
        /// </param>
        /// <param name="localName">
        /// The local name.
        /// </param>
        private void HandleTextChildElementCategoryRef(IReferenceInfo categoryRef, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.CategorySchemeID))
            {
                categoryRef.ID = this.Text;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.CategorySchemeAgencyID))
            {
                categoryRef.AgencyId = this.Text;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.CategorySchemeVersion))
            {
                categoryRef.Version = this.Text;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.URN))
            {
                categoryRef.URN = Helper.TrySetUri(this.Text);
            }
        }

        /// <summary>
        /// Handles the KeyFamily Ref element child elements
        /// </summary>
        /// <param name="refBean">
        /// The parent reference object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        private void HandleTextChildElementKeyFamily(IReferenceInfo refBean, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.KeyFamilyID))
            {
                refBean.ID = this.Text;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.KeyFamilyAgencyID))
            {
                refBean.AgencyId = this.Text;
            }
        }

        #endregion

        /////// <summary>
        ///////     Initialize handlers based on parent type for Elements and element text
        /////// </summary>
        ////private void InitializeTypeSwitch()
        ////{
        ////    // add element text handlers
        ////    this.AddHandleText<ICategoryIdInfo>(this.HandleTextChildElement);
        ////    this.AddHandleText<IReferenceInfo>(this.HandleTextChildElement);
        ////    this.AddHandleText<IAnnotationMutableObject>(this.HandleTextChildElement);
        ////    this.AddHandleText<INameableMutableObject>(
        ////        (parent, localName) => this.HandleCommonTextChildElement(parent, localName));

        ////    // add element handlers
        ////    this.AddHandleElement<IDataflowMutableObject>(this.HandleChildElements);
        ////    this.AddHandleElement<ICategoryIdInfo>(HandleChildElements);
        ////    this.AddHandleElement<IReferenceInfo>(this.HandleChildElements);
        ////    this.AddHandleElement<ICollection<IAnnotationMutableObject>>(HandleChildElements);
        ////    this.AddHandleElement<IdentifiableMutableCore>(HandleChildElements); // super type move last
        ////    this.AddHandleElement<IMutableObjects>(this.HandleTopLevel);
        ////}
    }
}