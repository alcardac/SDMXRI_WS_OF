// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataStructureReaderV2.cs" company="EUROSTAT">
//   Date Created : 2013-01-29
//   //   Copyright (c) 2013 by the European   Commission, represented by EUROSTAT.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The data structure reader v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Estat.Sri.SdmxParseBase.Helper;
    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     The data structure reader v 2.
    /// </summary>
    internal class DataStructureReaderV2 : StructureReaderBaseV20
    {
        /// <summary>
        /// The _builder.
        /// </summary>
        private static readonly IDataStructureFromCrossBuilder _builder = new DataStructureFromCrossBuilder();

        #region Fields

        /// <summary>
        ///     The _current DSD.
        /// </summary>
        private ICrossSectionalDataStructureMutableObject _currentDsd;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStructureReaderV2"/> class.
        /// </summary>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public DataStructureReaderV2(SdmxNamespaces namespaces)
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
            if (NameTableCache.IsElement(localname, ElementNameTable.KeyFamily))
            {
                Debug.Assert(this._currentDsd != null, "_currentDsd is null", "Local name: {0}", localname);
                if (this._currentDsd != null)
                {
                    // do we have components attached to cross sectional levels ?
                    if (IsCrossSectionalDsd(this._currentDsd))
                    {
                        if (this._currentDsd.CrossSectionalMeasures.Count > 0)
                        {
                           DataStructureUtil.ConvertMeasureRepresentation(this._currentDsd);
                        }

                        this.Structure.AddDataStructure(this._currentDsd);
                    }
                    else
                    {
                        this.Structure.AddDataStructure(_builder.Build(this._currentDsd));
                    }

                    this._currentDsd = null;
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
            if (NameTableCache.IsElement(localName, ElementNameTable.KeyFamily))
            {
                var kf = new CrossSectionalDataStructureMutableCore();
                ParseAttributes(kf, this.Attributes);

                //// NOTE this is handled at HandleEndElement because we need to decide if it will remain a XS DSD or should it be converted to a non XS (2.0) one.
                //// parent.AddDataStructure(kf);
                this._currentDsd = kf;
                actions = this.AddNameableAction(kf, this.HandleChildElements);
            }

            return actions;
        }

        /// <summary>
        /// Adds the <paramref name="conceptRef"/> to <paramref name="attachList"/> if <paramref name="attribute"/> exists in
        ///     <paramref name="attributes"/> with value <c>true</c>
        /// </summary>
        /// <param name="conceptRef">
        /// The concept ref.
        /// </param>
        /// <param name="attachList">
        /// The attach list.
        /// </param>
        /// <param name="attribute">
        /// The attribute.
        /// </param>
        /// <param name="attributes">
        /// The attributes.
        /// </param>
        private static void AddCrossSectionalAttach(string conceptRef, ICollection<string> attachList, AttributeNameTable attribute, IDictionary<string, string> attributes)
        {
            if (Helper.TrySetFromAttribute(attributes, attribute, false))
            {
                attachList.Add(conceptRef);
            }
        }

        /// <summary>
        /// Checks if the current DSD is a valid SDMX v2.0 Cross Sectional DSD
        /// </summary>
        /// <param name="dsd">
        /// The DSD.
        /// </param>
        /// <returns>
        /// Returns true if it is a valid SDMX v2.0 Cross Sectional DSD; otherwise false
        /// </returns>
        private static bool IsCrossSectionalDsd(ICrossSectionalDataStructureMutableObject dsd)
        {
            ISet<string> crossSectionalAttached = new HashSet<string>(dsd.CrossSectionalAttachDataSet);
            crossSectionalAttached.UnionWith(dsd.CrossSectionalAttachGroup);
            crossSectionalAttached.UnionWith(dsd.CrossSectionalAttachSection);
            crossSectionalAttached.UnionWith(dsd.CrossSectionalAttachObservation);

            IList<IDimensionMutableObject> dimensions = dsd.Dimensions;
            foreach (IDimensionMutableObject dimension in dimensions)
            {
                if (!dimension.FrequencyDimension && !dimension.MeasureDimension && !dimension.TimeDimension && !crossSectionalAttached.Contains(dimension.ConceptRef.ChildReference.Id))
                {
                    return false;
                }
            }

            if (dsd.AttributeList != null)
            {
                IList<IAttributeMutableObject> attributes = dsd.AttributeList.Attributes;
                foreach (IAttributeMutableObject attribute in attributes)
                {
                    if (!crossSectionalAttached.Contains(attribute.ConceptRef.ChildReference.Id))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        ///// <summary>
        ///// Populates the proporties of the given IPrimaryMeasureMutableObject object from the given xml attributes dictionary
        ///// </summary>
        ///// <param name="measure">The given IPrimaryMeasureMutableObject objectt</param>
        ///// <param name="attributes">The dictionary contains the attributes of the element</param>
        // private static void ParseAttributes(IPrimaryMeasureMutableObject measure, IDictionary<string, string> attributes) {
        // ParseAttributes((IComponentMutableObject) measure, attributes);
        // }

        /// <summary>
        /// Populates the properties of the given ICrossSectionalMeasureMutableObject object from the given xml attributes dictionary
        /// </summary>
        /// <param name="artefact">
        /// The <see cref="ICrossSectionalDataStructureMutableObject"/>
        /// </param>
        /// <param name="measure">
        /// The given ICrossSectionalMeasureMutableObject object
        /// </param>
        /// <param name="attributes">
        /// The dictionary contains the attributes of the element
        /// </param>
        private static void ParseAttributes(ICrossSectionalDataStructureMutableObject artefact, ICrossSectionalMeasureMutableObject measure, IDictionary<string, string> attributes)
        {
            ParseComponentBaseAttributes(artefact, measure, attributes);
            measure.MeasureDimension = Helper.TrySetFromAttribute(attributes, AttributeNameTable.measureDimension, measure.MeasureDimension);
            measure.Code = Helper.TrySetFromAttribute(attributes, AttributeNameTable.code, measure.Code);
        }

        ///// <summary>
        ///// Populates the proporties of the given IDimensionMutableObject object from the given xml attributes dictionary
        ///// </summary>
        ///// <param name="timeDimension">The given IDimensionMutableObject object</param>
        ///// <param name="attributes">The dictionary contains the attributes of the element</param>
        // private static void ParseAttributes(IDimensionMutableObject timeDimension, IDictionary<string, string> attributes) {
        // ParseAttributes((IComponentMutableObject) timeDimension, attributes);
        // }

        /// <summary>
        /// Populates the properties of the given IDimensionMutableObject object from the given xml attributes dictionary
        /// </summary>
        /// <param name="artefact">
        /// The <see cref="ICrossSectionalDataStructureMutableObject"/>
        /// </param>
        /// <param name="dimension">
        /// The given IDimensionMutableObject object
        /// </param>
        /// <param name="attributes">
        /// The dictionary contains the attributes of the element
        /// </param>
        private static void ParseAttributes(ICrossSectionalDataStructureMutableObject artefact, IDimensionMutableObject dimension, IDictionary<string, string> attributes)
        {
            //// TODO java not supported by the Common API as in Java version 0.9.9
            ////dimension.IsCountDimension = Helper.TrySetFromAttribute(
            ////    attributes, AttributeNameTable.isCountAttribute, dimension.IsCountDimension);
            dimension.MeasureDimension = Helper.TrySetFromAttribute(attributes, AttributeNameTable.isMeasureDimension, dimension.MeasureDimension);
            dimension.FrequencyDimension = Helper.TrySetFromAttribute(attributes, AttributeNameTable.isFrequencyDimension, dimension.FrequencyDimension);

            //// TODO java not supported by the Common API as in Java version 0.9.9
            ////dimension.IsEntityDimension = Helper.TrySetFromAttribute(
            ////    attributes, AttributeNameTable.isEntityDimension, dimension.IsEntityDimension);
            ////dimension.IsNonObservationTimeDimension = Helper.TrySetFromAttribute(
            ////    attributes, AttributeNameTable.isNonObservationTimeDimension, dimension.IsNonObservationTimeDimension);
            ////dimension.IsIdentityDimension = Helper.TrySetFromAttribute(
            ////    attributes, AttributeNameTable.isIdentityAttribute, dimension.IsIdentityDimension);
            ParseComponentBaseAttributes(artefact, dimension, attributes);
        }

        /// <summary>
        /// Populates the properties of the given IComponentMutableObject object from the given xml attributes dictionary
        /// </summary>
        /// <param name="parent">
        /// The <see cref="ICrossSectionalDataStructureMutableObject"/>
        /// </param>
        /// <param name="component">
        /// The given IComponentMutableObject object
        /// </param>
        /// <param name="attributes">
        /// The dictionary contains the attributes of the element
        /// </param>
        private static void ParseComponentBaseAttributes(ICrossSectionalDataStructureMutableObject parent, IComponentMutableObject component, IDictionary<string, string> attributes)
        {
            var conceptRef = ParseComponentConceptAttributes(component, attributes);

            // code list attributes
            string codelist = Helper.TrySetFromAttribute(attributes, AttributeNameTable.codelist, string.Empty);
            if (!string.IsNullOrEmpty(codelist))
            {
                component.Representation = new RepresentationMutableCore();
                string codelistVersion = Helper.TrySetFromAttribute(attributes, AttributeNameTable.codelistVersion, string.Empty);
                string codelistAgency = Helper.TrySetFromAttribute(attributes, AttributeNameTable.codelistAgency, string.Empty);
                component.Representation.Representation = new StructureReferenceImpl(codelistAgency, codelist, codelistVersion, SdmxStructureEnumType.CodeList);
            }

            // XS
            var id = component.Id ?? conceptRef;
            AddCrossSectionalAttach(id, parent.CrossSectionalAttachDataSet, AttributeNameTable.crossSectionalAttachDataSet, attributes);
            AddCrossSectionalAttach(id, parent.CrossSectionalAttachGroup, AttributeNameTable.crossSectionalAttachGroup, attributes);
            AddCrossSectionalAttach(id, parent.CrossSectionalAttachSection, AttributeNameTable.crossSectionalAttachSection, attributes);
            AddCrossSectionalAttach(id, parent.CrossSectionalAttachObservation, AttributeNameTable.crossSectionalAttachObservation, attributes);
        }

        /// <summary>
        /// Handles the Component element child elements
        /// </summary>
        /// <param name="parent">
        /// The parent IComponentMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        private ElementActions HandleChildElements(IComponentMutableObject parent, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.TextFormat))
            {
                parent.Representation = new RepresentationMutableCore { TextFormat = HandleTextFormat(this.Attributes) };

                //// TextFormatType has only attributes so we do not expect anything else.
                return ElementActions.Empty;
            }

            return null;
        }

        /// <summary>
        /// Handles the KeyFamily element child elements
        /// </summary>
        /// <param name="parent">
        /// The parent IDataStructureMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        private ElementActions HandleChildElements(ICrossSectionalDataStructureMutableObject parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.Dimension))
            {
                var dimension = new DimensionMutableCore();
                ParseAttributes(parent, dimension, this.Attributes);
                parent.AddDimension(dimension);
                actions = this.BuildElementActions(dimension, this.HandleChildElements, DoNothing);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.TimeDimension))
            {
                var timeDimension = new DimensionMutableCore { TimeDimension = true, Id = DimensionObject.TimeDimensionFixedId };
                ParseAttributes(parent, timeDimension, this.Attributes);
                parent.AddDimension(timeDimension);
                actions = this.BuildElementActions(timeDimension, this.HandleChildElements, DoNothing);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.PrimaryMeasure))
            {
                var measure = new PrimaryMeasureMutableCore();
                ParseComponentBaseAttributes(parent, measure, this.Attributes);
                parent.PrimaryMeasure = measure;
                actions = this.BuildElementActions(measure, this.HandleChildElements, DoNothing);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.CrossSectionalMeasure))
            {
                var measure = new CrossSectionalMeasureMutableCore();
                ParseAttributes(parent, measure, this.Attributes);
                parent.AddCrossSectionalMeasures(measure);
                actions = this.BuildElementActions(measure, this.HandleChildElements, DoNothing);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Attribute))
            {
                var attribute = new AttributeMutableCore();
                ParseAttributes(parent, attribute, this.Attributes);
                parent.AddAttribute(attribute);
                actions = this.BuildElementActions(attribute, this.HandleChildElements, this.HandleTextChildElement);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Components))
            {
                //// Component element contains the rest of elements so we use the same action again.
                actions = this.BuildElementActions(parent, this.HandleChildElements, DoNothing);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Group))
            {
                var groupMutableCore = new GroupMutableCore();
                ParseAttributes(groupMutableCore, this.Attributes);
                parent.AddGroup(groupMutableCore);
                actions = this.BuildElementActions(groupMutableCore, this.HandleAnnotableChildElements, this.HandleTextChildElement);
            }

            return actions;
        }

        /// <summary>
        /// Handles the Attribute element child elements
        /// </summary>
        /// <param name="parent">
        /// The parent IAttributeMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        private void HandleTextChildElement(IAttributeMutableObject parent, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.AttachmentGroup))
            {
                if (string.IsNullOrWhiteSpace(parent.AttachmentGroup))
                {
                    parent.AttachmentGroup = this.Text;
                }
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.AttachmentMeasure))
            {
                IList<string> measureList;
                string key = parent.ConceptRef.ChildReference.Id;
                if (!this._currentDsd.AttributeToMeasureMap.TryGetValue(key, out measureList))
                {
                    measureList = new List<string>();
                    this._currentDsd.AttributeToMeasureMap.Add(key, measureList);
                }

                measureList.Add(this.Text);
            }
        }

        /// <summary>
        /// Handles the Group element child elements
        /// </summary>
        /// <param name="parent">
        /// The parent IGroupMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        private void HandleTextChildElement(IGroupMutableObject parent, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.DimensionRef))
            {
                parent.DimensionRef.Add(this.Text);
            }

            //// NOTE SDMX v2  supported in Commo
            ////else if (NameTableCache.IsElement(localName, ElementNameTable.AttachmentConstraintRef))
            ////{
            ////    parent.AttachmentConstraintRef = this.Text;
            ////}
        }

        /////// <summary>
        ///////     Initialize handlers based on parent type for Elements and element text
        /////// </summary>
        ////private void InitializeTypeSwitch()
        ////{
        ////    // add element text handlers
        ////    this.AddHandleText<IAttributeMutableObject>(this.HandleTextChildElement);
        ////    this.AddHandleText<IGroupMutableObject>(this.HandleTextChildElement);
        ////    this.AddHandleText<IAnnotationMutableObject>(this.HandleTextChildElement);
        ////    this.AddHandleText<INameableMutableObject>(
        ////        (parent, localName) => this.HandleCommonTextChildElement(parent, localName));

        ////    // add element handlers
        ////    this.AddHandleElement<IComponentMutableObject>(this.HandleChildElements);
        ////    this.AddHandleElement<ICrossSectionalDataStructureMutableObject>(this.HandleChildElements);
        ////    this.AddHandleElement<ICollection<IAnnotationMutableObject>>(HandleChildElements);
        ////    this.AddHandleElement<IdentifiableMutableCore>(HandleChildElements); // super type move last
        ////    this.AddHandleElement<IMutableObjects>(this.HandleTopLevel);
        ////}

        /// <summary>
        /// Populates the properties of the given IAttributeMutableObject object from the given xml attributes dictionary
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="component">
        /// The given IAttributeMutableObject object
        /// </param>
        /// <param name="attributes">
        /// The dictionary contains the attributes of the element
        /// </param>
        private void ParseAttributes(ICrossSectionalDataStructureMutableObject parent, IAttributeMutableObject component, IDictionary<string, string> attributes)
        {
            ParseComponentBaseAttributes(parent, component, attributes);
            string attachmentLevel = Helper.TrySetFromAttribute(attributes, AttributeNameTable.attachmentLevel, string.Empty);
            if (attachmentLevel == "Series")
            {
                component.AttachmentLevel = AttributeAttachmentLevel.DimensionGroup;
                foreach (IDimensionMutableObject dimension in this._currentDsd.Dimensions)
                {
                    if (!dimension.TimeDimension)
                    {
                        component.DimensionReferences.Add(dimension.ConceptRef.ChildReference.Id);
                    }
                }
            }
            else
            {
                AttributeAttachmentLevel modelAttachementLevel;

                if (Enum.TryParse(attachmentLevel, out modelAttachementLevel))
                {
                    component.AttachmentLevel = modelAttachementLevel;
                }
            }

            component.AssignmentStatus = Helper.TrySetFromAttribute(attributes, AttributeNameTable.assignmentStatus, component.AssignmentStatus);

            //// TODO JAVA 0.9.9 missing TimeFormat property from IAttributeMutableObject.
            bool timeFormat = Helper.TrySetFromAttribute(attributes, AttributeNameTable.isTimeFormat, false);
            if (timeFormat)
            {
                //// TODO FIXME HACK because of missing TimeFormat property from java 0.9.9
                component.Id = "TIME_FORMAT";
            }

            //// TODO java not supported by the Common API as in Java version 0.9.9
            ////component.IsEntityAttribute = Helper.TrySetFromAttribute(
            ////    attributes, AttributeNameTable.isEntityAttribute, component.IsEntityAttribute);
            ////component.IsNonObservationalTimeAttribute = Helper.TrySetFromAttribute(
            ////    attributes, AttributeNameTable.isNonObservationalTimeAttribute, component.IsNonObservationalTimeAttribute);
            ////component.IsCountAttribute = Helper.TrySetFromAttribute(
            ////    attributes, AttributeNameTable.isCountAttribute, component.IsCountAttribute);
            ////component.IsFrequencyAttribute = Helper.TrySetFromAttribute(
            ////    attributes, AttributeNameTable.isFrequencyAttribute, component.IsFrequencyAttribute);
            ////component.IsIdentityAttribute = Helper.TrySetFromAttribute(
            ////    attributes, AttributeNameTable.isIdentityAttribute, component.IsIdentityAttribute);
        }

        #endregion
    }
}