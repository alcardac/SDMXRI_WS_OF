// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The component assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    /// The component assembler.
    /// </summary>
    /// <typeparam name="T">
    /// The representation type
    /// </typeparam>
    public sealed class ComponentAssembler<T> : IdentifiableAssembler
        where T : RepresentationType, new()
    {
        #region Fields

        /// <summary>
        ///     The representation xml bean builder.
        /// </summary>
        /// <typeparam name="T">
        ///     The representation type
        /// </typeparam>
        private readonly RepresentationXmlBuilder<T> _representationXmlBuilder = new RepresentationXmlBuilder<T>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Assemble component.
        /// </summary>
        /// <param name="builtObj">
        /// The destination component.
        /// </param>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        public void AssembleComponent(ComponentType builtObj, IComponent buildFrom)
        {
            this.AssembleIdentifiable(builtObj, buildFrom);
            if (buildFrom.ConceptRef != null)
            {
                var conceptReference = new ConceptReferenceType();
                builtObj.ConceptIdentity = conceptReference;
                var conceptRefType = new ConceptRefType();
                conceptReference.SetTypedRef(conceptRefType);
                this.SetReference(conceptRefType, buildFrom.ConceptRef);
            }

            if (buildFrom.Representation != null)
            {
                var representationType = new T();
                builtObj.LocalRepresentation = representationType;
                this._representationXmlBuilder.Assemble(representationType, buildFrom.Representation);
            }
        }

        #endregion
    }
}