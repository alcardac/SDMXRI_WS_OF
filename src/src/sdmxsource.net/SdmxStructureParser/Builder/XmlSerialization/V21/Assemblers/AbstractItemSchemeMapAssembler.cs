// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractItemSchemeMapAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The abstract item scheme map codelistRef assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;

    /// <summary>
    ///     The abstract item scheme map codelistRef assembler.
    /// </summary>
    public abstract class AbstractItemSchemeMapAssembler : AbstractSchemeMapAssembler
    {
        #region Public Methods and Operators

        /// <summary>
        /// The assemble scheme map.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <param name="assembleFrom">
        /// The assemble from.
        /// </param>
        public void AssembleSchemeMap(ItemSchemeMapType assembleInto, IItemSchemeMapObject assembleFrom)
        {
            // Populate it from inherited super
            this.AssembleMap(assembleInto, assembleFrom);

            //// Populate it using this class's specifics
            //// Child maps
            foreach (IItemMap eachMapBean in assembleFrom.Items)
            {
                // Defer child creation to subclass
                ItemAssociationType newMap = this.CreateNewMap(assembleInto);

                //// Annotations
                //// TODO RSG AWAITING MODEL CHANGES
                ////             if(ObjectUtil.validCollection(eachMapBean.getAnnotations())) {
                ////                 newMap.setAnnotations(getAnnotationsType(eachMapBean));
                ////             }
                //// Common source and target id allocation
                LocalItemReferenceType sourceItemReference = this.CreateSourceItemReference(newMap);
                LocalItemReferenceType targetItemReference = this.CreateTargetItemReference(newMap);

                RefBaseType newSourceRef = this.CreateItemRef(sourceItemReference);
                newSourceRef.id = eachMapBean.SourceId;

                RefBaseType newTargetRef = this.CreateItemRef(targetItemReference);
                newTargetRef.id = eachMapBean.TargetId;

                SdmxStructureType stype = this.MapStructureType();
                if (stype != null)
                {
                    newSourceRef.@class = XmlobjectsEnumUtil.BuildV21(stype);
                    newTargetRef.@class = XmlobjectsEnumUtil.BuildV21(stype);
                }
            }
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
        protected internal abstract LocalItemRefBaseType CreateItemRef(LocalItemReferenceType itemReferenceType);

        /// <summary>
        /// Create the new map.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <returns>
        /// The <see cref="ItemAssociationType"/>.
        /// </returns>
        protected internal abstract ItemAssociationType CreateNewMap(ItemSchemeMapType assembleInto);

        /// <summary>
        /// Create the source item reference.
        /// </summary>
        /// <param name="itemAssociation">
        /// The item association.
        /// </param>
        /// <returns>
        /// The <see cref="LocalItemReferenceType"/>.
        /// </returns>
        protected internal abstract LocalItemReferenceType CreateSourceItemReference(
            ItemAssociationType itemAssociation);

        /// <summary>
        /// Create the target item reference.
        /// </summary>
        /// <param name="itemAssociation">
        /// The item association.
        /// </param>
        /// <returns>
        /// The <see cref="LocalItemReferenceType"/>.
        /// </returns>
        protected internal abstract LocalItemReferenceType CreateTargetItemReference(
            ItemAssociationType itemAssociation);

        /// <summary>
        ///     Map the structure type.
        /// </summary>
        /// <returns>
        ///     The <see cref="SdmxStructureType" />.
        /// </returns>
        protected internal abstract SdmxStructureType MapStructureType();

        #endregion

        // TODO RSG NOT FOR CONCEPT MAP = NULL
    }
}