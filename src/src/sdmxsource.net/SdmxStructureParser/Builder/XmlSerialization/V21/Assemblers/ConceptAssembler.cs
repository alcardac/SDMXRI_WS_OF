// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConceptAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The concept bean assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The concept bean assembler.
    /// </summary>
    public class ConceptAssembler : NameableAssembler, IAssembler<ConceptType, IConceptObject>
    {
        #region Static Fields

        /// <summary>
        ///     The representation xml bean builder.
        /// </summary>
        private static readonly RepresentationXmlBuilder<ConceptRepresentation> _representationXmlBuilder =
            new RepresentationXmlBuilder<ConceptRepresentation>();

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
        public virtual void Assemble(ConceptType assembleInto, IConceptObject assembleFrom)
        {
            // Populate it from inherited super
            this.AssembleNameable(assembleInto, assembleFrom);

            string parentConcept = assembleFrom.ParentConcept;
            if (!string.IsNullOrWhiteSpace(parentConcept))
            {
                var parent = new LocalConceptReferenceType();
                assembleInto.SetTypedParent(parent);
                var xref = new LocalConceptRefType();
                parent.SetTypedRef(xref);
                xref.id = assembleFrom.ParentConcept;
            }

            if (assembleFrom.CoreRepresentation != null)
            {
                var representationType = new ConceptRepresentation();
                assembleInto.CoreRepresentation = representationType;
                _representationXmlBuilder.Assemble(representationType, assembleFrom.CoreRepresentation);
            }
        }

        #endregion
    }
}