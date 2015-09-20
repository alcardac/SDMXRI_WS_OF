// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorySchemeMapAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The category scheme map bean assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    /// <summary>
    ///     The category scheme map bean assembler.
    /// </summary>
    public class CategorySchemeMapAssembler : AbstractSchemeMapAssembler, 
                                              IAssembler<CategorySchemeMapType, ICategorySchemeMapObject>
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
        public virtual void Assemble(CategorySchemeMapType assembleInto, ICategorySchemeMapObject assembleFrom)
        {
            // Populate it from inherited super
            this.AssembleMap(assembleInto, assembleFrom);

            // Populate it using this class's specifics
            // Child maps
            foreach (ICategoryMap eachMapBean in assembleFrom.CategoryMaps)
            {
                //// Defer child creation to subclass
                var newMap = new CategoryMap();
                assembleInto.ItemAssociation.Add(newMap);

                //// Annotations
                //// TODO RSG AWAITING MODEL CHANGES
                ////             if(ObjectUtil.validCollection(eachMapBean.getAnnotations())) {
                ////                 newMap.setAnnotations(getAnnotationsType(eachMapBean));
                ////             }
                //// Common source and target id allocation
                var source = new LocalCategoryReferenceType();
                newMap.Source = source;
                var target = new LocalCategoryReferenceType();
                newMap.Target = target;

                var newSourceRef = new CategoryRefType();
                source.SetTypedRef(newSourceRef);

                ////             newSourceRef.setId(eachMapBean.getSourceId().getId());
                // TODO RSG CATEGORYMAPS NEED MORE THAN ID FOR HANDLING HIERARCHY OF SIBLING-UNIQUE ID - NOT SET-UNIQUE ID - SDMX V2.1 BUG?? (RAISED...) #######
                var newTargetRef = new CategoryRefType();
                source.SetTypedRef(newTargetRef);

                ////             newTargetRef.setId(eachMapBean.getTargetId().getId());
            }
        }

        #endregion

        #region Methods

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
            var refBase = new CategorySchemeRefType();
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
            var referenceType = new CategorySchemeReferenceType();
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
            var referenceType = new CategorySchemeReferenceType();
            assembleInto.Target = referenceType;
            return referenceType;
        }

        #endregion
    }
}