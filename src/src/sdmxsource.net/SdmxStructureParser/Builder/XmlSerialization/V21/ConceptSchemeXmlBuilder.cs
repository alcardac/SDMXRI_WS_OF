// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConceptSchemeXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The concept scheme xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    /// <summary>
    ///     The concept scheme xml bean builder.
    /// </summary>
    public class ConceptSchemeXmlBuilder : ItemSchemeAssembler, IBuilder<ConceptSchemeType, IConceptSchemeObject>
    {
        #region Fields

        /// <summary>
        ///     The concept bean assembler bean.
        /// </summary>
        private readonly ConceptAssembler _conceptAssemblerBean = new ConceptAssembler();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="ConceptSchemeType"/>.
        /// </returns>
        public virtual ConceptSchemeType Build(IConceptSchemeObject buildFrom)
        {
            // Create outgoing build
            var builtObj = new ConceptSchemeType();

            if (buildFrom.Partial)
                builtObj.isPartial = true;

            // Populate it from inherited super
            this.AssembleItemScheme(builtObj, buildFrom);

            // Populate it using this class's specifics
            foreach (IConceptObject eachConceptBean in buildFrom.Items)
            {
                var newConcept = new Concept();
                builtObj.Item.Add(newConcept);
                this._conceptAssemblerBean.Assemble(newConcept.Content, eachConceptBean);
            }

            return builtObj;
        }

        #endregion
    }
}