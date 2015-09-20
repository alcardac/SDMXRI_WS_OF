// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorySchemeReaderV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The category scheme reader v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System.Collections.Generic;
    using System.Globalization;

    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxStructureMutableParser.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    ///     The category scheme reader v 2.
    /// </summary>
    internal class CategorySchemeReaderV2 : StructureReaderBaseV20
    {
        #region Fields

        /// <summary>
        ///     The current category scheme
        /// </summary>
        private ICategorySchemeMutableObject _currentCategoryScheme;

        /// <summary>
        ///     The current reference info.
        /// </summary>
        private IReferenceInfo _currentDataflowReference;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySchemeReaderV2"/> class.
        /// </summary>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public CategorySchemeReaderV2(SdmxNamespaces namespaces)
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
            if (NameTableCache.IsElement(localname, ElementNameTable.DataflowRef))
            {
                if (this._currentDataflowReference != null)
                {
                    IStructureReference categoryReference = CreateReference(this._currentCategoryScheme, SdmxStructureEnumType.Category, this._currentDataflowReference.ReferenceFrom.Id);
                    IStructureReference dataflowReference = this._currentDataflowReference.CreateReference();
                    var categorisation = new CategorisationMutableCore
                                             {
                                                 CategoryReference = categoryReference, 
                                                 StructureReference = dataflowReference, 
                                                 AgencyId = this._currentCategoryScheme.AgencyId
                                             };
                    categorisation.Id = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", categorisation.CategoryReference.GetHashCode(), categorisation.StructureReference.GetHashCode());

                    if (this._currentDataflowReference.ReferenceFrom.Names.Count > 0)
                    {
                        categorisation.Names.AddAll(this._currentDataflowReference.ReferenceFrom.Names);
                    }
                    else
                    {
                        string name = string.Format(
                            CultureInfo.InvariantCulture, 
                            "Categorisation between Category {0} and Dataflow {1}", 
                            categorisation.CategoryReference, 
                            categorisation.StructureReference.MaintainableReference);
                        categorisation.AddName("en", name);
                    }

                    this.Structure.AddCategorisation(categorisation);
                    this._currentDataflowReference = null;
                }
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
            if (NameTableCache.IsElement(localName, ElementNameTable.CategoryScheme))
            {
                var cs = new CategorySchemeMutableCore();
                ParseAttributes(cs, this.Attributes);
                parent.AddCategoryScheme(cs);
                this._currentCategoryScheme = cs;
                actions = this.AddNameableAction(cs, this.HandleChildElements);
            }

            return actions;
        }

        /// <summary>
        /// Handles the Category element
        /// </summary>
        /// <param name="attributes">
        /// The dictionary contains the attributes of the element
        /// </param>
        /// <returns>
        /// The created ICategoryMutableObject
        /// </returns>
        private static ICategoryMutableObject HandleCategory(IDictionary<string, string> attributes)
        {
            var category = new CategoryMutableCore();
            ParseAttributes(category, attributes);
            return category;
        }

        /// <summary>
        /// Handles the Category element child elements
        /// </summary>
        /// <param name="parent">
        /// The parent category of this category element
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        private ElementActions HandleChildElements(ICategoryMutableObject parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.Category))
            {
                ICategoryMutableObject category = HandleCategory(this.Attributes);
                parent.AddItem(category);
                actions = this.AddNameableAction(category, this.HandleChildElements);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.DataflowRef))
            {
                IReferenceInfo reference = new ReferenceInfo(SdmxStructureEnumType.Dataflow) { ReferenceFrom = parent };
                this._currentDataflowReference = reference;
                actions = this.AddSimpleAction(reference, this.HandleTextChildElement);
            }
            
            return actions;
        }

        /// <summary>
        /// Handles the CategoryScheme element child elements
        /// </summary>
        /// <param name="parent">
        /// The parent ICategorySchemeMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        private ElementActions HandleChildElements(ICategorySchemeMutableObject parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.Category))
            {
                ICategoryMutableObject category = HandleCategory(this.Attributes);
                parent.AddItem(category);
                actions = this.AddNameableAction(category, this.HandleChildElements);
            }

            return actions;
        }

        /// <summary>
        /// Handles the DataflowRef element child elements
        /// </summary>
        /// <param name="parent">
        /// The parent DataflowRefBean object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        private void HandleTextChildElement(IReferenceInfo parent, object localName)
        {
            if (this.HandleReferenceTextChildElement(parent, localName))
            {
                return;
            }

            if (NameTableCache.IsElement(localName, ElementNameTable.DataflowID))
            {
                parent.ID = this.Text;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.AgencyID))
            {
                parent.AgencyId = this.Text;
            }
        }

        #endregion

        /////// <summary>
        ///////     Initialize handlers based on parent type for Elements and element text
        /////// </summary>
        ////private void InitializeTypeSwitch()
        ////{
        ////    // add element text handlers
        ////    this.AddHandleText<IReferenceInfo>(this.HandleTextChildElement);
        ////    this.AddHandleText<IAnnotationMutableObject>(this.HandleTextChildElement);
        ////    this.AddHandleText<INameableMutableObject>(
        ////        (parent, localName) => this.HandleCommonTextChildElement(parent, localName));

        ////    // add element handlers
        ////    this.AddHandleElement<ICategoryMutableObject>(this.HandleChildElements);
        ////    this.AddHandleElement<ICategorySchemeMutableObject>(this.HandleChildElements);
        ////    this.AddHandleElement<ICollection<IAnnotationMutableObject>>(HandleChildElements);
        ////    this.AddHandleElement<IdentifiableMutableCore>(HandleChildElements); // super type move last
        ////    this.AddHandleElement<IMutableObjects>(this.HandleTopLevel);
        ////}
    }
}