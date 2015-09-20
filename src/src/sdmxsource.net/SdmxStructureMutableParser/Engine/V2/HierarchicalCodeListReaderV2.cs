// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchicalCodeListReaderV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The hierarchical code list reader v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System.Collections.Generic;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxStructureMutableParser.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The hierarchical code list reader v 2.
    /// </summary>
    internal class HierarchicalCodeListReaderV2 : StructureReaderBaseV20
    {
        #region Fields

        /// <summary>
        ///     The current levels.
        /// </summary>
        ///// TODO check if SortedDic or .Sort() would be better.
        private readonly SortedList<int, ILevelMutableObject> _currentLevels = new SortedList<int, ILevelMutableObject>();

        /// <summary>
        ///     The_current codelist ref.
        /// </summary>
        private ICodelistRefMutableObject _currentCodelistRef;

        /// <summary>
        ///     The current hierarchy.
        /// </summary>
        private IHierarchyMutableObject _currentHierarchy;

        /// <summary>
        ///     The current reference
        /// </summary>
        private IReferenceInfo _currentReference;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalCodeListReaderV2"/> class.
        /// </summary>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public HierarchicalCodeListReaderV2(SdmxNamespaces namespaces)
            : base(namespaces)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// When override handles end element.
        /// </summary>
        /// <param name="localName">
        /// The element local name.
        /// </param>
        protected override void HandleEndElement(object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.CodelistRef))
            {
                if (this._currentReference != null && this._currentCodelistRef != null)
                {
                    this._currentCodelistRef.CodelistReference = this._currentReference.CreateReference();
                }

                this._currentCodelistRef = null;
                this._currentReference = null;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Hierarchy))
            {
                var levelMutableObjects = this._currentLevels;
                var currentHierarchy = this._currentHierarchy;
                SetupHierarchyLevels(levelMutableObjects, currentHierarchy);

                SetupCodeRefLevels(currentHierarchy);


                levelMutableObjects.Clear();
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
            if (NameTableCache.IsElement(localName, ElementNameTable.HierarchicalCodelist))
            {
                var hcl = new HierarchicalCodelistMutableCore();
                ParseAttributes(hcl, this.Attributes);
                parent.AddHierarchicalCodelist(hcl);
                actions = this.AddNameableAction(hcl, this.HandleChildElements);
            }

            return actions;
        }

        /// <summary>
        /// Setups the code reference levels.
        /// </summary>
        /// <param name="currentHierarchy">The current hierarchy.</param>
        private static void SetupCodeRefLevels(IHierarchyMutableObject currentHierarchy)
        {
            if (currentHierarchy.ChildLevel != null)
            {
                var stack = new Stack<ICodeRefMutableObject>(currentHierarchy.HierarchicalCodeObjects);
                while (stack.Count != 0)
                {
                    var currentCode = stack.Pop();
                    if (!string.IsNullOrWhiteSpace(currentCode.LevelReference))
                    {
                        currentCode.LevelReference = string.Join(".", BuildLevelRef(currentCode.LevelReference, currentHierarchy));
                    }

                    foreach (var codeRefMutableObject in currentCode.CodeRefs)
                    {
                        stack.Push(codeRefMutableObject);
                    }
                }
            }
        }

        /// <summary>
        /// Setups the hierarchy levels.
        /// </summary>
        /// <param name="levelMutableObjects">The level mutable objects.</param>
        /// <param name="currentHierarchy">The current hierarchy.</param>
        private static void SetupHierarchyLevels(SortedList<int, ILevelMutableObject> levelMutableObjects, IHierarchyMutableObject currentHierarchy)
        {
            ILevelMutableObject previous = null;
            foreach (KeyValuePair<int, ILevelMutableObject> levelMutableObject in levelMutableObjects)
            {
                if (previous == null)
                {
                    currentHierarchy.FormalLevels = true;
                    currentHierarchy.ChildLevel = levelMutableObject.Value;
                    previous = levelMutableObject.Value;
                }
                else
                {
                    previous.ChildLevel = levelMutableObject.Value;
                    previous = levelMutableObject.Value;
                }
            }
        }

        /// <summary>
        /// Build level ref.
        /// </summary>
        /// <param name="levelRef">The level reference.</param>
        /// <param name="hierarchyMutableObject">The hierarchy mutable object.</param>
        /// <returns>The <see cref="IEnumerable{String}" />.</returns>
        private static IEnumerable<string> BuildLevelRef(string levelRef, IHierarchyMutableObject hierarchyMutableObject)
        {
            var hierarchy = hierarchyMutableObject;
            var level = hierarchy.ChildLevel;
            while (!level.Id.Equals(levelRef))
            {
                yield return level.Id;
                level = level.ChildLevel;
            }

            yield return level.Id;
        }

        /// <summary>
        /// Handles the CodeRef type element  child elements
        /// </summary>
        /// <param name="parent">
        /// The parent ICodeRefMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        private ElementActions HandleChildElements(ICodeRefMutableObject parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.CodeRef))
            {
                var cr = new CodeRefMutableCore();
                ParseAttributes(cr, this.Attributes);
                parent.AddCodeRef(cr);
                actions = this.BuildElementActions(cr, HandleChildElements, this.HandleTextChildElement);
            }

            return actions;
        }

        /// <summary>
        /// Handles the Level type element  child elements
        /// </summary>
        /// <param name="parent">
        /// The parent ILevelMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        private ElementActions HandleChildElements(ILevelMutableObject parent, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.CodingType))
            {
                parent.CodingFormat = HandleTextFormat(this.Attributes);
                return ElementActions.Empty;
            }

            return null;
        }

        /// <summary>
        /// Handles the Hierarchy element child elements
        /// </summary>
        /// <param name="parent">
        /// The parent IHierarchyMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        private ElementActions HandleChildElements(IHierarchyMutableObject parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.CodeRef))
            {
                ICodeRefMutableObject cr = new CodeRefMutableCore();
                ParseAttributes(cr, this.Attributes);
                parent.AddHierarchicalCode(cr);
                actions = this.BuildElementActions(cr, HandleChildElements, this.HandleTextChildElement);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Level))
            {
                ILevelMutableObject level = new LevelMutableCore();
                ParseAttributes(level, this.Attributes);
                actions = this.AddNameableAction(level, HandleChildElements, this.HandleTextChildElement);
            }

            return actions;
        }

        /// <summary>
        /// Handles the HierarchicalCodelist element child elements
        /// </summary>
        /// <param name="parent">
        /// The parent IHierarchicalCodelistMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        private ElementActions HandleChildElements(IHierarchicalCodelistMutableObject parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.CodelistRef))
            {
                var codelistRefMutableCore = new CodelistRefMutableCore();
                parent.AddCodelistRef(codelistRefMutableCore);
                this._currentCodelistRef = codelistRefMutableCore;
                IReferenceInfo reference = new ReferenceInfo(SdmxStructureEnumType.CodeList);
                this._currentReference = reference;
                actions = this.BuildElementActions(codelistRefMutableCore, DoNothingComplex, HandleTextChildElement);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Hierarchy))
            {
                var hc = new HierarchyMutableCore();
                ParseAttributes(hc, this.Attributes);
                this._currentHierarchy = hc;
                
                //// TODO java 0.9.9 no isFinal 
                ////hc.IsFinal = Helper.TrySetFromAttribute(this.Attributes, AttributeNameTable.isFinal, hc.IsFinal);
                parent.AddHierarchies(hc);
                actions = this.AddNameableAction(hc, this.HandleChildElements);
            }

            return actions;
        }

        /// <summary>
        /// Handles the CodelistRef element child elements
        /// </summary>
        /// <param name="parent">
        /// The parent ICodelistRefMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        private void HandleTextChildElement(ICodelistRefMutableObject parent, object localName)
        {
            if (!this.HandleReferenceTextChildElement(this._currentReference, localName))
            {
                if (NameTableCache.IsElement(localName, ElementNameTable.CodelistID))
                {
                    this._currentReference.ID = this.Text;
                }
                else if (NameTableCache.IsElement(localName, ElementNameTable.AgencyID))
                {
                    this._currentReference.AgencyId = this.Text;
                }
                else if (NameTableCache.IsElement(localName, ElementNameTable.Alias))
                {
                    parent.Alias = this.Text;
                }
            }
        }

        /// <summary>
        /// Handles the CodeRef type element text only child elements
        /// </summary>
        /// <param name="parent">
        /// The parent ICodeRefMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        private void HandleTextChildElement(ICodeRefMutableObject parent, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.CodelistAliasRef))
            {
                parent.CodelistAliasRef = this.Text;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.CodeID))
            {
                parent.CodeId = this.Text;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.LevelRef))
            {
                parent.LevelReference = this.Text;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.NodeAliasID))
            {
                parent.Id = this.Text;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.ValidFrom))
            {
                parent.ValidFrom = XmlConvert.ToDateTime(this.Text, XmlDateTimeSerializationMode.RoundtripKind);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.ValidTo))
            {
                parent.ValidTo = XmlConvert.ToDateTime(this.Text, XmlDateTimeSerializationMode.RoundtripKind);
            }
        }

        /// <summary>
        /// Handles the Level type element text only child elements
        /// </summary>
        /// <param name="parent">
        /// The parent ILevelMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        private void HandleTextChildElement(ILevelMutableObject parent, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.Order))
            {
                this._currentLevels.Add(XmlConvert.ToInt32(this.Text), parent);
            }
        }

        #endregion

        /////// <summary>
        ///////     Initialize handlers based on parent type for Elements and element text
        /////// </summary>
        ////private void InitializeTypeSwitch()
        ////{
        ////    // add element text handlers
        ////    this.AddHandleText<ILevelMutableObject>(this.HandleTextChildElement);
        ////    this.AddHandleText<ICodelistRefMutableObject>(this.HandleTextChildElement);
        ////    this.AddHandleText<ICodeRefMutableObject>(this.HandleTextChildElement);
        ////    this.AddHandleText<IAnnotationMutableObject>(this.HandleTextChildElement);
        ////    this.AddHandleText<INameableMutableObject>(
        ////        (parent, localName) => this.HandleCommonTextChildElement(parent, localName));

        ////    // add element handlers
        ////    this.AddHandleElement<IHierarchicalCodelistMutableObject>(this.HandleChildElements);
        ////    this.AddHandleElement<IHierarchyMutableObject>(this.HandleChildElements);
        ////    this.AddHandleElement<ILevelMutableObject>(this.HandleChildElements);
        ////    this.AddHandleElement<IdentifiableMutableCore>(HandleChildElements); // super type move last
        ////    this.AddHandleElement<ICodeRefMutableObject>(HandleChildElements);
        ////    this.AddHandleElement<ICollection<IAnnotationMutableObject>>(HandleChildElements);
        ////    this.AddHandleElement<IdentifiableMutableCore>(HandleChildElements); // super type move last
        ////    this.AddHandleElement<IMutableObjects>(this.HandleTopLevel);
        ////}
    }
}