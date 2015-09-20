// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractSchemeMapAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The abstract scheme map bean assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;

    /// <summary>
    ///     The abstract scheme map bean assembler.
    /// </summary>
    public abstract class AbstractSchemeMapAssembler : NameableAssembler
    {
        #region Public Methods and Operators

        /// <summary>
        /// The assemble map.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <param name="assembleFrom">
        /// The assemble from.
        /// </param>
        public void AssembleMap(ItemSchemeMapType assembleInto, ISchemeMapObject assembleFrom)
        {
            // Populate it from inherited super
            this.AssembleNameable(assembleInto, assembleFrom);
            ItemSchemeReferenceBaseType sourceReference = this.CreateSourceReference(assembleInto);
            ItemSchemeReferenceBaseType targetReference = this.CreateTargetReference(assembleInto);
            ItemSchemeRefBaseType sourceRef = this.CreateRef(sourceReference);
            ItemSchemeRefBaseType targetRef = this.CreateRef(targetReference);
            this.SetReference(sourceRef, assembleFrom.SourceRef);
            this.SetReference(targetRef, assembleFrom.TargetRef);
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
        protected abstract ItemSchemeRefBaseType CreateRef(ItemSchemeReferenceBaseType assembleInto);

        /// <summary>
        /// Create the source reference.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <returns>
        /// The <see cref="ItemSchemeReferenceBaseType"/>.
        /// </returns>
        protected abstract ItemSchemeReferenceBaseType CreateSourceReference(ItemSchemeMapType assembleInto);

        /// <summary>
        /// Create the target reference.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <returns>
        /// The <see cref="ItemSchemeReferenceBaseType"/>.
        /// </returns>
        protected abstract ItemSchemeReferenceBaseType CreateTargetReference(ItemSchemeMapType assembleInto);

        #endregion
    }
}