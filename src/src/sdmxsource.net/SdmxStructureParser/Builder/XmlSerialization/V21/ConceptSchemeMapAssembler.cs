// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConceptSchemeMapAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The concept scheme map bean assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    using ConceptMap = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.ConceptMap;

    /// <summary>
    ///     The concept scheme map bean assembler.
    /// </summary>
    public class ConceptSchemeMapAssembler : AbstractItemSchemeMapAssembler, 
                                             IAssembler<ConceptSchemeMapType, IConceptSchemeMapObject>
    {
        #region Static Fields

        /// <summary>
        ///     The sdmx structure type.
        /// </summary>
        private static readonly SdmxStructureType _sdmxStructureType =
            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptSchemeMap);

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
        public virtual void Assemble(ConceptSchemeMapType assembleInto, IConceptSchemeMapObject assembleFrom)
        {
            // Populate it from inherited super
            this.AssembleSchemeMap(assembleInto, assembleFrom);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create the item ref.
        /// </summary>
        /// <param name="itemReferenceType">
        /// The item reference
        /// </param>
        /// <returns>
        /// The <see cref="LocalItemRefBaseType"/>.
        /// </returns>
        protected internal override LocalItemRefBaseType CreateItemRef(LocalItemReferenceType itemReferenceType)
        {
            var refBase = new LocalConceptRefType();
            itemReferenceType.SetTypedRef(refBase);
            return refBase;
        }

        /// <summary>
        /// Create the new map.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <returns>
        /// The <see cref="ItemAssociationType"/>.
        /// </returns>
        protected internal override ItemAssociationType CreateNewMap(ItemSchemeMapType assembleInto)
        {
            var conceptMap = new ConceptMap();
            assembleInto.ItemAssociation.Add(conceptMap);
            return conceptMap.Content;
        }

        /// <summary>
        /// Create the source item reference.
        /// </summary>
        /// <param name="itemAssociation">
        /// The item association.
        /// </param>
        /// <returns>
        /// The <see cref="LocalItemReferenceType"/>.
        /// </returns>
        protected internal override LocalItemReferenceType CreateSourceItemReference(
            ItemAssociationType itemAssociation)
        {
            var referenceType = new LocalConceptReferenceType();
            itemAssociation.Source = referenceType;
            return referenceType;
        }

        /// <summary>
        /// Create the target item reference.
        /// </summary>
        /// <param name="itemAssociation">
        /// The item association.
        /// </param>
        /// <returns>
        /// The <see cref="LocalItemReferenceType"/>.
        /// </returns>
        protected internal override LocalItemReferenceType CreateTargetItemReference(
            ItemAssociationType itemAssociation)
        {
            var referenceType = new LocalConceptReferenceType();
            itemAssociation.Target = referenceType;
            return referenceType;
        }

        /// <summary>
        ///     The map structure type.
        /// </summary>
        /// <returns>
        ///     The <see cref="SdmxStructureType" />.
        /// </returns>
        protected internal override SdmxStructureType MapStructureType()
        {
            return _sdmxStructureType;
        }

        /// <summary>
        /// Create the ref. base type
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <returns>
        /// The <see cref="ItemSchemeRefBaseType"/>.
        /// </returns>
        protected override ItemSchemeRefBaseType CreateRef(ItemSchemeReferenceBaseType assembleInto)
        {
            var refBase = new ConceptSchemeRefType();
            assembleInto.SetTypedRef(refBase);
            return refBase;
        }

        /// <summary>
        /// Create the source reference.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <returns>
        /// The <see cref="ItemSchemeReferenceBaseType"/>.
        /// </returns>
        protected override ItemSchemeReferenceBaseType CreateSourceReference(ItemSchemeMapType assembleInto)
        {
            var referenceType = new ConceptSchemeReferenceType();
            assembleInto.Source = referenceType;
            return referenceType;
        }

        /// <summary>
        /// Create the target reference.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <returns>
        /// The <see cref="ItemSchemeReferenceBaseType"/>.
        /// </returns>
        protected override ItemSchemeReferenceBaseType CreateTargetReference(ItemSchemeMapType assembleInto)
        {
            var referenceType = new ConceptSchemeReferenceType();
            assembleInto.Target = referenceType;
            return referenceType;
        }

        #endregion
    }
}