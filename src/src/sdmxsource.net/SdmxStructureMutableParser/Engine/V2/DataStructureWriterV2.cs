// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataStructureWriterV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data structure writer v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Helper;
    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///     The data structure writer v 2.
    /// </summary>
    internal class DataStructureWriterV2 : StructureWriterBaseV2, IMutableWriter
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStructureWriterV2"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public DataStructureWriterV2(XmlWriter writer, SdmxNamespaces namespaces)
            : base(writer, namespaces)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The write.
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        public void Write(IMutableObjects structure)
        {
            this.WriteKeyFamilies(structure.DataStructures);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns true if the specified <paramref name="artefact"/> has components.
        /// </summary>
        /// <param name="artefact">
        /// The DSD
        /// </param>
        /// <returns>
        /// The true if the specified <paramref name="artefact"/> has components; otherwise false.
        /// </returns>
        private static bool HasComponents(IDataStructureMutableObject artefact)
        {
            return (artefact.Dimensions.Count > 0)
                   || (artefact.AttributeList != null && artefact.AttributeList.Attributes.Count > 0)
                   || artefact.MeasureList != null || artefact.Groups.Count > 0;
        }

        /// <summary>
        /// Write a KeyFamily/Components/Dimensions element from a IDimensionMutableObject object
        /// </summary>
        /// <param name="dimension">
        /// The IDimensionMutableObject object to write
        /// </param>
        /// <param name="helper">
        /// Helper class for SDMX v2.0 cross-sectional
        /// </param>
        private void WriteComponent(IDimensionMutableObject dimension, CrossSectionalDsdHelper helper)
        {
            if (dimension.TimeDimension)
            {
                this.WriteStartElement(this.DefaultPrefix, ElementNameTable.TimeDimension);
            }
            else
            {
                this.WriteStartElement(this.DefaultPrefix, ElementNameTable.Dimension);
                this.TryWriteAttribute(AttributeNameTable.isMeasureDimension, dimension.MeasureDimension);
                this.TryWriteAttribute(AttributeNameTable.isFrequencyDimension, dimension.FrequencyDimension);
            }

            this.WriteComponentAttributes(dimension);
            if (dimension.MeasureDimension)
            {
                this.WriteRepresentation(helper.GetRepresentation(dimension));
            }
            else
            {
                this.WriteRepresentation(dimension.Representation);
            }
            helper.WriteCrossComponentAttributes(dimension, this.TryWriteAttribute);

            //// TODO not supported by the Common API 0.9.9
            ////this.TryWriteAttribute(AttributeNameTable.isCountDimension, dimension.CountDimension);
            ////this.TryWriteAttribute(AttributeNameTable.isEntityDimension, dimension.IsEntityDimension);
            ////this.TryWriteAttribute(AttributeNameTable.isIdentityDimension, dimension.IsIdentityDimension);
            ////this.TryWriteAttribute(
            ////    AttributeNameTable.isNonObservationTimeDimension, dimension.IsNonObservationTimeDimension);
            this.WriteComponentContent(dimension);
            this.WriteEndElement();
        }

        /// <summary>
        /// Write a KeyFamily/Components/Attribute element from a IAttributeMutableObject object
        /// </summary>
        /// <param name="attribute">
        /// The IAttributeMutableObject object to write
        /// </param>
        /// <param name="helper">
        /// The helper.
        /// </param>
        private void WriteComponent(IAttributeMutableObject attribute, CrossSectionalDsdHelper helper)
        {
            this.WriteStartElement(this.DefaultPrefix, ElementNameTable.Attribute);
            this.WriteComponentAttributes(attribute);
            this.WriteRepresentation(attribute.Representation);
            helper.WriteCrossComponentAttributes(attribute, this.TryWriteAttribute);

            /* 
                  <xs:attribute name="attachmentLevel" type="structure:AttachmentLevelType" use="required"/>
                <xs:attribute name="assignmentStatus" type="structure:AssignmentStatusType" use="required"/>
                <xs:attribute name="isTimeFormat" type="xs:boolean" default="false"/>
                <xs:attribute name="isEntityAttribute" type="xs:boolean" default="false"/>
                <xs:attribute name="isNonObservationalTimeAttribute" type="xs:boolean" default="false"/>
                <xs:attribute name="isCountAttribute" type="xs:boolean" default="false"/>
                <xs:attribute name="isFrequencyAttribute" type="xs:boolean" default="false"/>
                <xs:attribute name="isIdentityAttribute" type="xs:boolean" default="false"/>
             */
            this.TryWriteAttribute(
                AttributeNameTable.attachmentLevel, Helper.GetAttachmentLevelV2(attribute.AttachmentLevel));
            this.TryWriteAttribute(AttributeNameTable.assignmentStatus, attribute.AssignmentStatus);
            this.TryWriteAttribute(AttributeNameTable.isTimeFormat, "TimeFormat".Equals(attribute.Id));

            //////this.TryWriteAttribute(AttributeNameTable.isEntityAttribute, attribute.IsEntityAttribute);
            //////this.TryWriteAttribute(
            //////    AttributeNameTable.isNonObservationalTimeAttribute, attribute.IsNonObservationalTimeAttribute);
            //////this.TryWriteAttribute(AttributeNameTable.isCountAttribute, attribute.IsCountAttribute);
            //////this.TryWriteAttribute(AttributeNameTable.isFrequencyAttribute, attribute.IsFrequencyAttribute);
            //////this.TryWriteAttribute(AttributeNameTable.isIdentityAttribute, attribute.IsIdentityAttribute);
            this.WriteComponentContent(attribute);

            /*
             * <xs:element name="AttachmentGroup" type="common:IDType" minOccurs="0" maxOccurs="unbounded"/>
            <xs:element name="AttachmentMeasure" type="common:IDType" minOccurs="0" maxOccurs="unbounded"/>
             */
            this.TryToWriteElement(this.DefaultNS, ElementNameTable.AttachmentGroup, attribute.AttachmentGroup);

            helper.WriteAttachementMeasures(
                attachmentMeasure =>
                this.TryToWriteElement(this.DefaultNS, ElementNameTable.AttachmentMeasure, attachmentMeasure), 
                attribute.ConceptRef.ChildReference.Id);

            this.WriteEndElement();
        }

        /// <summary>
        /// Write a KeyFamily/Components/PrimaryMeasure element from a IPrimaryMeasureMutableObject object
        /// </summary>
        /// <param name="pm">
        /// The IPrimaryMeasureMutableObject object to write
        /// </param>
        private void WriteComponent(IPrimaryMeasureMutableObject pm)
        {
            this.WriteStartElement(this.DefaultPrefix, ElementNameTable.PrimaryMeasure);
            this.WriteComponentAttributes(pm);
            this.WriteRepresentation(pm.Representation);
            this.WriteComponentContent(pm);
            this.WriteEndElement();
        }

        /// <summary>
        /// Write a KeyFamily/Components/CrossSectionalMeasure element from a ICrossSectionalMeasureMutableObject object
        /// </summary>
        /// <param name="xsm">
        /// The ICrossSectionalMeasureMutableObject object to write
        /// </param>
        private void WriteComponent(ICrossSectionalMeasureMutableObject xsm)
        {
            this.WriteStartElement(this.DefaultPrefix, ElementNameTable.CrossSectionalMeasure);
            this.WriteComponentAttributes(xsm);
            this.WriteRepresentation(xsm.Representation);
            this.TryWriteAttribute(AttributeNameTable.measureDimension, xsm.MeasureDimension);
            this.TryWriteAttribute(AttributeNameTable.code, xsm.Code);
            this.WriteComponentContent(xsm);
            this.WriteEndElement();
        }

        /// <summary>
        /// Write the common xml attribute of a Component
        /// </summary>
        /// <param name="component">
        /// The IComponentMutableObject object
        /// </param>
        private void WriteComponentAttributes(IComponentMutableObject component)
        {
            IStructureReference concept = component.ConceptRef;
            this.TryWriteAttribute(AttributeNameTable.conceptRef, concept.ChildReference.Id);
            this.TryWriteAttribute(AttributeNameTable.conceptSchemeRef, concept.MaintainableReference.MaintainableId);
            this.TryWriteAttribute(AttributeNameTable.conceptSchemeAgency, concept.MaintainableReference.AgencyId);

            //// TODO -- use either MT or ESTAT extensions. NSI Client needs those extensions either.
            ////if (this.TargetSchema == SdmxSchema.VersionTwoPointZeroEuroStat)
            ////{
            ////TryWriteAttribute(AttributeNameTable.conceptSchemeVersion, concept.MaintainableReference.Version);
            TryWriteAttribute(AttributeNameTable.conceptSchemeVersion, concept.MaintainableReference.Version);
            ////}
        }

        /// <summary>
        /// Write the representation xml attribute of a Component
        /// </summary>
        /// <param name="representation">
        /// The representation object
        /// </param>
        private void WriteRepresentation(IRepresentationMutableObject representation)
        {
            if (representation != null && representation.Representation != null)
            {
                IMaintainableRefObject codelist = representation.Representation.MaintainableReference;

                this.TryWriteAttribute(AttributeNameTable.codelist, codelist.MaintainableId);
                this.TryWriteAttribute(AttributeNameTable.codelistVersion, codelist.Version);
                this.TryWriteAttribute(AttributeNameTable.codelistAgency, codelist.AgencyId);
            }
        }

        /// <summary>
        /// Write the common element of a Component
        /// </summary>
        /// <param name="component">
        /// The IComponentMutableObject object
        /// </param>
        private void WriteComponentContent(IComponentMutableObject component)
        {
            if (component.Representation != null && component.Representation.TextFormat != null)
            {
                this.WriteTextFormat(component.Representation.TextFormat);
            }

            this.WriteAnnotations(ElementNameTable.Annotations, component.Annotations);
        }

        /// <summary>
        /// Write a KeyFamily/Components/Group element from a IGroupMutableObject
        /// </summary>
        /// <param name="group">
        /// The IGroupMutableObject object to write
        /// </param>
        private void WriteGroup(IGroupMutableObject group)
        {
            this.WriteStartElement(this.DefaultPrefix, ElementNameTable.Group);
            this.WriteAttributeString(AttributeNameTable.id, group.Id);

            foreach (string dim in group.DimensionRef)
            {
                this.TryToWriteElement(this.DefaultPrefix, ElementNameTable.DimensionRef, dim);
            }

            ////TryToWriteElement(this.DefaultPrefix, ElementNameTable.DimensionRefs, group.AttachmentConstraintRef);
            this.WriteEndElement();
        }

        /// <summary>
        /// Write the Structure/KeyFamilies included in <paramref name="dataStructures"/>
        /// </summary>
        /// <param name="dataStructures">
        /// The list of DSD.
        /// </param>
        private void WriteKeyFamilies(IEnumerable<IDataStructureMutableObject> dataStructures)
        {
            this.WriteStartElement(this.RootNamespace, ElementNameTable.KeyFamilies);
            foreach (IDataStructureMutableObject artefact in dataStructures)
            {
                this.WriteMaintainableArtefact(ElementNameTable.KeyFamily, artefact);
                if (HasComponents(artefact))
                {
                    this.WriteStartElement(this.DefaultPrefix, ElementNameTable.Components);
                    var helper = new CrossSectionalDsdHelper(artefact);

                    //// TODO java 0.9.9 interface issue Missing Position from DimensionMutableBean
                    ////var orderedDimensions = new List<IDimensionMutableObject>(artefact.Dimensions);
                    ////orderedDimensions.Sort((o, mutableObject) => Comparer<int>.Default.Compare(o.Position, mutableObject.Position));
                    foreach (IDimensionMutableObject item in artefact.Dimensions)
                    {
                        this.WriteComponent(item, helper);
                    }

                    foreach (IGroupMutableObject group in artefact.Groups)
                    {
                        this.WriteGroup(group);
                    }

                    if (artefact.PrimaryMeasure != null)
                    {
                        this.WriteComponent(artefact.PrimaryMeasure);
                    }

                    helper.HandleCrossSectionalMeasures(this.WriteComponent);

                    if (artefact.AttributeList != null)
                    {
                        foreach (IAttributeMutableObject attribute in artefact.AttributeList.Attributes)
                        {
                            this.WriteComponent(attribute, helper);
                        }
                    }

                    this.WriteEndElement(); // Components
                }

                this.WriteAnnotations(ElementNameTable.Annotations, artefact.Annotations);
                this.WriteEndElement(); // KeyFamily
            }

            this.WriteEndElement();
        }

        #endregion

        /// <summary>
        ///     The cross sectional DSD helper.
        /// </summary>
        private class CrossSectionalDsdHelper
        {
            #region Fields

            /// <summary>
            ///     The _attribute map.
            /// </summary>
            private readonly IDictionaryOfLists<string, string> _attributeMap;

            /// <summary>
            ///     Gets the component concept id that attach to data set.
            /// </summary>
            private readonly HashSet<string> _crossDataSet;

            /// <summary>
            ///     Gets the component concept id that attach to group
            /// </summary>
            private readonly HashSet<string> _crossGroup;

            /// <summary>
            ///     Gets the component concept id that attach to measure
            /// </summary>
            private readonly HashSet<string> _crossObs;

            /// <summary>
            ///     Gets the component concept id that attach to section
            /// </summary>
            private readonly HashSet<string> _crossSection;

            /// <summary>
            ///     The _cross sectional.
            /// </summary>
            private readonly ICrossSectionalDataStructureMutableObject _crossSectional;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="CrossSectionalDsdHelper"/> class.
            /// </summary>
            /// <param name="dsd">
            /// The DSD.
            /// </param>
            public CrossSectionalDsdHelper(IDataStructureMutableObject dsd)
            {
                this._crossSectional = dsd as ICrossSectionalDataStructureMutableObject;
                if (this._crossSectional != null)
                {
                    this._crossDataSet = new HashSet<string>(
                        this._crossSectional.CrossSectionalAttachDataSet, StringComparer.Ordinal);
                    this._crossGroup = new HashSet<string>(
                        this._crossSectional.CrossSectionalAttachGroup, StringComparer.Ordinal);
                    this._crossSection = new HashSet<string>(
                        this._crossSectional.CrossSectionalAttachSection, StringComparer.Ordinal);
                    this._crossObs = new HashSet<string>(
                        this._crossSectional.CrossSectionalAttachObservation, StringComparer.Ordinal);
                    this._attributeMap = this._crossSectional.AttributeToMeasureMap;
                }
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// The handle cross sectional measures.
            /// </summary>
            /// <param name="writeComponent">
            /// The write component.
            /// </param>
            public void HandleCrossSectionalMeasures(Action<ICrossSectionalMeasureMutableObject> writeComponent)
            {
                if (this._crossSectional == null)
                {
                    return;
                }

                foreach (ICrossSectionalMeasureMutableObject crossSectionalMeasureBean in
                    this._crossSectional.CrossSectionalMeasures)
                {
                    writeComponent(crossSectionalMeasureBean);
                }
            }

            /// <summary>
            /// Writes the attachment measures.
            /// </summary>
            /// <param name="action">
            /// The action used to write the methods. The string parameter is the attachment measure
            /// </param>
            /// <param name="conceptRef">
            /// The concept ref of the attribute.
            /// </param>
            public void WriteAttachementMeasures(Action<string> action, string conceptRef)
            {
                if (this._crossSectional == null)
                {
                    return;
                }

                IList<string> list;
                if (this._attributeMap.TryGetValue(conceptRef, out list))
                {
                    foreach (string attachmentMeasure in list)
                    {
                        action(attachmentMeasure);
                    }
                }
            }

            /// <summary>
            /// The write cross component attributes.
            /// </summary>
            /// <param name="component">
            /// The component.
            /// </param>
            /// <param name="tryWriteAttribute">
            /// The try write attribute.
            /// </param>
            public void WriteCrossComponentAttributes(
                IComponentMutableObject component, Action<AttributeNameTable, bool> tryWriteAttribute)
            {
                if (this._crossSectional == null)
                {
                    return;
                }

                string item = component.ConceptRef.ChildReference.Id;
                tryWriteAttribute(AttributeNameTable.crossSectionalAttachDataSet, this._crossDataSet.Contains(item));

                tryWriteAttribute(AttributeNameTable.crossSectionalAttachGroup, this._crossGroup.Contains(item));
                tryWriteAttribute(AttributeNameTable.crossSectionalAttachSection, this._crossSection.Contains(item));
                tryWriteAttribute(AttributeNameTable.crossSectionalAttachObservation, this._crossObs.Contains(item));
            }

            #endregion

            /// <summary>
            /// Returns the representation of the <paramref name="dimension"/> if it is measure and the parent DSD is <see cref="ICrossSectionalDataStructureMutableObject"/>
            /// </summary>
            /// <param name="dimension">
            /// The measure dimension.
            /// </param>
            /// <returns>
            /// The <see cref="IRepresentationMutableObject"/>.
            /// </returns>
            public IRepresentationMutableObject GetRepresentation(IDimensionMutableObject dimension)
            {
                IStructureReference codelistRef;
                if (this._crossSectional == null || !dimension.MeasureDimension || !this._crossSectional.MeasureDimensionCodelistMapping.TryGetValue(dimension.ConceptRef.ChildReference.Id, out codelistRef))
                {
                    return dimension.Representation;
                }

                return new RepresentationMutableCore { Representation = codelistRef };
            }
        }
    }
}