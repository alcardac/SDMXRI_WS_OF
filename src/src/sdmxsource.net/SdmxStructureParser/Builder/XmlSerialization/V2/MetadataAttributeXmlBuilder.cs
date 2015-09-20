// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataAttributeXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The metadata attribute xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The metadata attribute XML bean builder.
    /// </summary>
    public class MetadataAttributeXmlBuilder : AbstractBuilder, IBuilder<MetadataAttributeType, IMetadataAttributeObject>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="MetadataAttributeType"/> from <paramref name="sourceObject"/>.
        /// </summary>
        /// <param name="sourceObject">
        /// The source Object.
        /// </param>
        /// <returns>
        /// The <see cref="MetadataAttributeType"/> from <paramref name="sourceObject"/> .
        /// </returns>
        /// <remarks>This implementation differs from SdmxSource.org java version. Specifically </remarks>
        /// <example><code>
        /// <![CDATA[
        ///  <xs:complexType name="MetadataAttributeType">
        /// <xs:annotation>
        /// <xs:documentation>Metadata attributes are those concepts - whether taking a coded or uncoded value, or made up of child concepts, or both - which are reported against a full or partial target identifier. If there are nested metadata attributes, these concepts are subordinate to the parent metadata attribute - that is, for the purposes of presentation, the parent concept is made up of the child concepts. This hierarchy is strictly presentational, for the purposes of structuring reports. If the metadata attribute can have a coded or uncoded value, then the charateristics of the value are indicated with the TextFormat child element. If the value is coded, then the representationScheme and representationSchemeAgency attributes must hold values: the representationScheme attribute takes the ID of a representation scheme, and the representationSchemeAgency takes the ID of the agency which maintains that scheme. The conceptRef attribute holds the ID of the metadata attribute's concept. The conceptAgency attribute takes the agency ID of the concept referenced in conceptRef. The conceptSchemeRef attribute holds the ID value of the concept scheme from which the concept is taken, and the conceptSchemeAgency holds the ID of the agency that maintains the concept scheme referenced in the conceptSchemeRef attribute. The useageStatus attribute indicates whether provision of the metadata attribute is conditional or mandatory.</xs:documentation>
        /// </xs:annotation>
        /// <xs:sequence>
        /// <xs:element maxOccurs="unbounded" minOccurs="0" name="MetadataAttribute" type="MetadataAttributeType"/>
        /// <xs:element minOccurs="0" name="TextFormat" type="TextFormatType"/>
        /// <xs:element minOccurs="0" name="Annotations" type="common:AnnotationsType"/>
        /// </xs:sequence>
        /// <xs:attribute name="conceptRef" type="common:IDType" use="required"/>
        /// <xs:attribute name="conceptVersion" type="xs:string" use="optional"/>
        /// <xs:attribute name="conceptAgency" type="common:IDType" use="optional"/>
        /// <xs:attribute name="conceptSchemeRef" type="common:IDType" use="optional"/>
        /// <xs:attribute name="conceptSchemeAgency" type="common:IDType" use="optional"/>
        /// <xs:attribute name="conceptSchemeVersion" type="xs:string" use="optional"/>
        /// <!-- Added by GPA on 12/06/07 -->
        /// <xs:attribute name="representationScheme" type="common:IDType" use="optional"/>
        /// <xs:attribute name="representationSchemeVersion" type="xs:string" use="optional"/>
        /// <!-- Added by GPA on 12/06/07 -->
        /// <xs:attribute name="representationSchemeAgency" type="common:IDType" use="optional"/>
        /// <xs:attribute name="usageStatus" type="UsageStatusType" use="required"/>
        /// </xs:complexType>
        /// ]]>
        /// </code></example>
        public virtual MetadataAttributeType Build(IMetadataAttributeObject sourceObject)
        {
            // .NET avoid recursion
            var stack = new Stack<Tuple<IMetadataAttributeObject, MetadataAttributeType>>();

            var rootObj = new MetadataAttributeType();
            stack.Push(Tuple.Create(sourceObject, rootObj));

            // WARNING in .NET the order of elements need to be the same as in the SDMXStructure.xsd.
            // DO NOT CHANGE TO MATCH JAVA ORDER. See example above.
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                var builtObj = current.Item2;
                var buildFrom = current.Item1;

                // extension: set usageStatus
                var minOccurs = buildFrom.MinOccurs.GetValueOrDefault(0);
                builtObj.usageStatus = minOccurs > 0 ? UsageStatusTypeConstants.Mandatory : UsageStatusTypeConstants.Conditional;

                if (buildFrom.ConceptRef != null)
                {
                    IMaintainableRefObject maintRef = buildFrom.ConceptRef.MaintainableReference;
                    string value = maintRef.AgencyId;
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        builtObj.conceptSchemeAgency = maintRef.AgencyId;
                    }

                    string value1 = maintRef.MaintainableId;
                    if (!string.IsNullOrWhiteSpace(value1))
                    {
                        builtObj.conceptSchemeRef = maintRef.MaintainableId;
                    }

                    string value2 = buildFrom.ConceptRef.ChildReference.Id;
                    if (!string.IsNullOrWhiteSpace(value2))
                    {
                        builtObj.conceptRef = buildFrom.ConceptRef.ChildReference.Id;
                    }

                    string value3 = maintRef.Version;
                    if (!string.IsNullOrWhiteSpace(value3))
                    {
                        builtObj.conceptVersion = maintRef.Version;
                    }
                }

                // extension: set coded representation
                if (buildFrom.HasCodedRepresentation())
                {
                    var maintainableRef = buildFrom.Representation.Representation;
                    if (maintainableRef.HasMaintainableId())
                    {
                        builtObj.representationScheme = maintainableRef.MaintainableId;
                    }

                    if (maintainableRef.HasAgencyId())
                    {
                        builtObj.representationSchemeAgency = maintainableRef.AgencyId;
                    }
                }

                // extension: iterate metadata attributes.
                // must be before all other elements
                if (ObjectUtil.ValidCollection(buildFrom.MetadataAttributes))
                {
                    foreach (var metadataAttribute in buildFrom.MetadataAttributes)
                    {
                        var child = new MetadataAttributeType();
                        builtObj.MetadataAttribute.Add(child);
                        stack.Push(Tuple.Create(metadataAttribute, child));
                    }
                }

                // must be after Metadata Attributes and before annotations
                if (buildFrom.Representation != null && buildFrom.Representation.TextFormat != null)
                {
                    var textFormatType = new TextFormatType();
                    this.PopulateTextFormatType(textFormatType, buildFrom.Representation.TextFormat);
                    builtObj.TextFormat = textFormatType;
                }

                // must be last
                if (this.HasAnnotations(buildFrom))
                {
                    builtObj.Annotations = this.GetAnnotationsType(buildFrom);
                }
            }

            return rootObj;
        }

        #endregion
    }
}