// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupXmlAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The group xml assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    /// <summary>
    ///     The group xml assembler.
    /// </summary>
    public class GroupXmlAssembler : IdentifiableAssembler, IAssembler<GroupType, IGroup>
    {
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
        public virtual void Assemble(GroupType assembleInto, IGroup assembleFrom)
        {
            this.AssembleIdentifiable(assembleInto, assembleFrom);
            if (assembleFrom.DimensionRefs != null)
            {
                /* foreach */
                foreach (string currentDimensionRef in assembleFrom.DimensionRefs)
                {
                    var groupDimension = new GroupDimension();
                    assembleInto.GroupDimension.Add(groupDimension);

                    var dimRefType = new LocalDimensionReferenceType();
                    groupDimension.DimensionReference = dimRefType;
                    var xref = new LocalDimensionRefType();
                    dimRefType.SetTypedRef(xref);
                    xref.id = currentDimensionRef;
                }
            }

            if (assembleFrom.AttachmentConstraintRef != null)
            {
                var attachmentConstraintReferenceType = new AttachmentConstraintReferenceType();
                assembleInto.AttachmentConstraint = attachmentConstraintReferenceType;
                var attachmentConstraintRefType = new AttachmentConstraintRefType();
                attachmentConstraintReferenceType.SetTypedRef(attachmentConstraintRefType);
                this.SetReference(attachmentConstraintRefType, assembleFrom.AttachmentConstraintRef);
            }
        }

        #endregion
    }
}