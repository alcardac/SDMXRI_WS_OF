// -----------------------------------------------------------------------
// <copyright file="MetadataStructureReaderV2.cs" company="EUROSTAT">
//   Date Created : 2014-12-17
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System.Collections.Generic;
    using System.Linq;

    using Estat.Sri.SdmxParseBase.Helper;
    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    /// Class MetadataStructureReaderV2.
    /// </summary>
    public class MetadataStructureReaderV2 : StructureReaderBaseV20
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataStructureReaderV2"/> class. 
        /// </summary>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public MetadataStructureReaderV2(SdmxNamespaces namespaces)
            : base(namespaces)
        {
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
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        protected override ElementActions HandleTopLevel(IMutableObjects parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.MetadataStructureDefinition))
            {
                var codelist = new MetadataStructureDefinitionMutableCore();
                ParseAttributes(codelist, this.Attributes);
                parent.AddMetadataStructure(codelist);
                actions = this.AddNameableAction(codelist, this.HandleChildElements);
            }

            return actions;
        }

        /// <summary>
        /// Populates the properties of the given IComponentMutableObject object from the given xml attributes dictionary
        /// </summary>
        /// <param name="component">
        /// The given IComponentMutableObject object
        /// </param>
        /// <param name="attributes">
        /// The dictionary contains the attributes of the element
        /// </param>
        private static void ParseAttributes(IMetadataAttributeMutableObject component, IDictionary<string, string> attributes)
        {
            ParseComponentConceptAttributes(component, attributes);

            var usageStatus = Helper.TrySetFromAttribute(attributes, AttributeNameTable.usageStatus, string.Empty);
            component.MaxOccurs = 1;
            switch (usageStatus)
            {
                case "Mandatory":
                    component.MinOccurs = 1;
                    break;
                default:
                    component.MinOccurs = 0;
                    break;
            }

            // code list attributes
            string codelist = Helper.TrySetFromAttribute(attributes, AttributeNameTable.representationScheme, string.Empty);
            if (!string.IsNullOrEmpty(codelist))
            {
                component.Representation = new RepresentationMutableCore();
                string codelistVersion = Helper.TrySetFromAttribute(attributes, AttributeNameTable.representationSchemeVersion, string.Empty);
                string codelistAgency = Helper.TrySetFromAttribute(attributes, AttributeNameTable.representationSchemeAgency, string.Empty);
                component.Representation.Representation = new StructureReferenceImpl(codelistAgency, codelist, codelistVersion, SdmxStructureEnumType.CodeList);
            }
        }

        /// <summary>
        /// Handles the CodeList element child elements
        /// </summary>
        /// <param name="parent">
        /// The parent ICodelistMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        private ElementActions HandleChildElements(MetadataStructureDefinitionMutableCore parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.TargetIdentifiers))
            {
                actions = this.BuildElementActions(parent, this.HandleChildElements, DoNothing);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.ReportStructure))
            {
                IReportStructureMutableObject reportStructure = new ReportStructureMutableCore();
                ParseAttributes(reportStructure, this.Attributes);
                parent.ReportStructures.Add(reportStructure);
                string target = Helper.TrySetFromAttribute(Attributes, AttributeNameTable.target, (string)null);
                if (target != null)
                {
                    reportStructure.TargetMetadatas.Add(target);
                }

                actions = this.AddAnnotableAction(reportStructure, this.HandleChildElements, DoNothing);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.FullTargetIdentifier))
            {
                var fullTarget = new MetadataTargetMutableCore();
                ParseAttributes(fullTarget, this.Attributes);

                parent.MetadataTargets.Add(fullTarget);
                actions = this.AddAnnotableAction(fullTarget, this.HandleChildElements, DoNothing);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.PartialTargetIdentifier))
            {
                var partialTarget = new MetadataTargetMutableCore();
                ParseAttributes(partialTarget, this.Attributes);
                if (!ObjectUtil.ValidCollection(parent.MetadataTargets))
                {
                    throw new SdmxSyntaxException("FullTargetIdentifier not set or PartialTargetIdentifier in wrong order");
                }

                parent.MetadataTargets.Add(partialTarget);
                var fullTarget = parent.MetadataTargets[0];
                actions = this.AddAnnotableAction(partialTarget, DoNothingComplex, (core, o) => this.HandlePartialChildElement(core, fullTarget, o));
            }

            return actions;
        }

        /// <summary>
        /// Handles the child elements.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="elementLocalName">Name of the element local.</param>
        /// <returns>The <see cref="StructureReaderBaseV20.ElementActions"/>.</returns>
        private ElementActions HandleChildElements(IReportStructureMutableObject parent, object elementLocalName)
        {
            if (ElementNameTable.MetadataAttribute.Is(elementLocalName))
            {
                IMetadataAttributeMutableObject attribute = new MetadataAttributeMutableCore();
                ParseAttributes(attribute, this.Attributes);
                parent.MetadataAttributes.Add(attribute);
                return this.AddAnnotableAction(attribute, this.HandleChildElements, DoNothing);
            }

            return null;
        }

        /// <summary>
        /// Handles the Component element child elements
        /// </summary>
        /// <param name="parent">
        /// The parent IComponentMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current XML element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        private ElementActions HandleChildElements(IMetadataAttributeMutableObject parent, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.TextFormat))
            {
                parent.Representation = new RepresentationMutableCore { TextFormat = HandleTextFormat(this.Attributes) };

                //// TextFormatType has only attributes so we do not expect anything else.
                return ElementActions.Empty;
            }
            
            if (ElementNameTable.MetadataAttribute.Is(localName))
            {
                IMetadataAttributeMutableObject attribute = new MetadataAttributeMutableCore();
                ParseAttributes(attribute, this.Attributes);
                parent.MetadataAttributes.Add(attribute);
                return this.AddAnnotableAction(attribute, this.HandleChildElements, DoNothing);
            }

            return null;
        }

        /// <summary>
        /// Handles the partial child element.
        /// </summary>
        /// <param name="partialTarget">The partial target.</param>
        /// <param name="fullTarget">The full target.</param>
        /// <param name="localName">The local name of the element.</param>
        private void HandlePartialChildElement(IMetadataTargetMutableObject partialTarget, IMetadataTargetMutableObject fullTarget, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.IdentifierComponentRef))
            {
                var identifierRef = this.Text;
                var identifiableTargetMutableObject = fullTarget.IdentifiableTarget.FirstOrDefault(o => string.Equals(identifierRef, o.Id));
                if (identifiableTargetMutableObject != null)
                {
                    partialTarget.IdentifiableTarget.Add(identifiableTargetMutableObject);
                }
            }
        }

        /// <summary>
        /// Handles the child elements.
        /// </summary>
        /// <param name="parent">The full target mutable object.</param>
        /// <param name="localName">The local name of the element.</param>
        /// <returns>The <see cref="StructureReaderBaseV20.ElementActions"/>.</returns>
        private ElementActions HandleChildElements(IMetadataTargetMutableObject parent, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.IdentifierComponent))
            {
                IIdentifiableTargetMutableObject identifiableTarget = new IdentifiableTargetMutableCore();
                parent.IdentifiableTarget.Add(identifiableTarget); 
                ParseAttributes(identifiableTarget, this.Attributes);
                return this.AddAnnotableAction(identifiableTarget, this.HandleChildElements, this.HandleTextChildElement);
            }
            
            return null;
        }

        /// <summary>
        /// Handles the text child element.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="elementLocalName">Name of the element local.</param>
        private void HandleTextChildElement(IIdentifiableTargetMutableObject parent, object elementLocalName)
        {
            if (NameTableCache.IsElement(elementLocalName, ElementNameTable.TargetObjectClass))
            {
                parent.ReferencedStructureType = XmlobjectsEnumUtil.GetSdmxStructureType(this.Text);
            }
        }

        /// <summary>
        /// Handles the child elements.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="elementLocalName">Name of the element local.</param>
        /// <returns>The <see cref="StructureReaderBaseV20.ElementActions"/></returns>
        private ElementActions HandleChildElements(IIdentifiableTargetMutableObject parent, object elementLocalName)
        {
            if (NameTableCache.IsElement(elementLocalName, ElementNameTable.RepresentationScheme))
            {
                string id = Helper.TrySetFromAttribute(this.Attributes, AttributeNameTable.representationScheme, (string)null);
                string agency = Helper.TrySetFromAttribute(this.Attributes, AttributeNameTable.representationSchemeAgency, (string)null);
                string version = Helper.TrySetFromAttribute(this.Attributes, AttributeNameTable.representationSchemeVersion, (string)null);
                string type = Helper.TrySetFromAttribute(this.Attributes, AttributeNameTable.representationSchemeType, (string)null);
                var structureType = XmlobjectsEnumUtil.GetSdmxStructureTypeFromRepresentationSchemeTypeV20(type);

                if (parent.Representation == null)
                {
                    parent.Representation = new RepresentationMutableCore();
                }

                parent.Representation.Representation = new StructureReferenceImpl(agency, id, version, structureType);
            }

            return null;
        }
    }
}