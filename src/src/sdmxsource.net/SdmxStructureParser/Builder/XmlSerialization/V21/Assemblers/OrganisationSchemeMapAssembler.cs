// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationSchemeMapAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The organisation scheme map bean assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;

    using OrganisationMap = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.OrganisationMap;

    /// <summary>
    ///     The organisation scheme map bean assembler.
    /// </summary>
    public class OrganisationSchemeMapAssembler : AbstractItemSchemeMapAssembler, 
                                                  IAssembler<OrganisationSchemeMapType, IOrganisationSchemeMapObject>
    {
        #region Static Fields

        /// <summary>
        ///     The sdmx structure type.
        /// </summary>
        private static readonly SdmxStructureType _sdmxStructureType =
            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationSchemeMap);

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
        public virtual void Assemble(OrganisationSchemeMapType assembleInto, IOrganisationSchemeMapObject assembleFrom)
        {
            // Populate it from inherited super
            this.AssembleSchemeMap(assembleInto, assembleFrom);
        }

        #endregion

        //// <OrganisationSchemeReferenceType, OrganisationSchemeRefType, LocalOrganisationReferenceType, LocalOrganisationRefType>
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
            var refBase = new LocalOrganisationRefType();
            itemReferenceType.SetTypedRef(refBase);
            return refBase;
        }

        /// <summary>
        /// The create new map.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <returns>
        /// The <see cref="ItemAssociationType"/>.
        /// </returns>
        protected internal override ItemAssociationType CreateNewMap(ItemSchemeMapType assembleInto)
        {
            var map = new OrganisationMap();
            assembleInto.ItemAssociation.Add(map);
            return map.Content;
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
            var referenceType = new LocalOrganisationReferenceType();
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
            var referenceType = new LocalOrganisationReferenceType();
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
            var refBase = new OrganisationSchemeRefType();
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
            var referenceType = new OrganisationSchemeReferenceType();
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
            var referenceType = new OrganisationSchemeReferenceType();
            assembleInto.Target = referenceType;
            return referenceType;
        }

        #endregion
    }
}