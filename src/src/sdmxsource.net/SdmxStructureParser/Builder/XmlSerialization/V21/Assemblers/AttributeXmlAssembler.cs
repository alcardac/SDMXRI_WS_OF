// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeXmlAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The attribute xml assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The attribute xml assembler.
    /// </summary>
    public class AttributeXmlAssembler : IAssembler<AttributeType, IAttributeObject>
    {
        #region Fields

        /// <summary>
        ///     The component assembler.
        /// </summary>
        private readonly ComponentAssembler<SimpleDataStructureRepresentationType> _componentAssembler =
            new ComponentAssembler<SimpleDataStructureRepresentationType>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The assemble.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <param name="assembleFrom">
        /// The assemble from.
        /// </param>
        public virtual void Assemble(AttributeType assembleInto, IAttributeObject assembleFrom)
        {
            this._componentAssembler.AssembleComponent(assembleInto, assembleFrom);

            if (assembleFrom.ConceptRoles != null)
            {
                foreach (ICrossReference currentConceptRole in assembleFrom.ConceptRoles)
                {
                    var conceptRef = new ConceptReferenceType();
                    assembleInto.ConceptRole.Add(conceptRef);
                    var conceptRefType = new ConceptRefType();
                    conceptRef.SetTypedRef(conceptRefType);
                    this._componentAssembler.SetReference(conceptRefType, currentConceptRole);
                }
            }

            if (assembleFrom.AssignmentStatus != null)
            {
                switch (assembleFrom.AssignmentStatus)
                {
                    case UsageStatusTypeConstants.Conditional:
                    case UsageStatusTypeConstants.Mandatory:
                        assembleInto.assignmentStatus = assembleFrom.AssignmentStatus;
                        break;
                }
            }

            var attributeRelationship = new AttributeRelationshipType();
            assembleInto.AttributeRelationship = attributeRelationship;
            if (ObjectUtil.ValidCollection(assembleFrom.DimensionReferences))
            {
                /* foreach */
                foreach (string currentDimensionRef in assembleFrom.DimensionReferences)
                {
                    var dimRef = new LocalDimensionReferenceType();
                    attributeRelationship.Dimension.Add(dimRef);

                    var localDimensionRefType = new LocalDimensionRefType();
                    dimRef.SetTypedRef(localDimensionRefType);
                    localDimensionRefType.id = currentDimensionRef;
                }
            }
            else
            {
                string value = assembleFrom.AttachmentGroup;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    // TODO do we add this to AttachmentGroup or Group ??? Used Group because this is the case in Java 0.9.1
                    attributeRelationship.Group = new LocalGroupKeyDescriptorReferenceType();
                    attributeRelationship.Group.SetTypedRef(
                        new LocalGroupKeyDescriptorRefType { id = assembleFrom.AttachmentGroup });
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(assembleFrom.PrimaryMeasureReference))
                    {
                        attributeRelationship.PrimaryMeasure = new LocalPrimaryMeasureReferenceType();
                        attributeRelationship.PrimaryMeasure.SetTypedRef(
                            new LocalPrimaryMeasureRefType { id = assembleFrom.PrimaryMeasureReference });
                    }
                    else
                    {
                        attributeRelationship.None = new EmptyType();
                    }
                }
            }
        }

        #endregion
    }
}