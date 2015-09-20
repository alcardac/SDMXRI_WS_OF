// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureSetXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure set xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    /// <summary>
    ///     The structure set xml bean builder.
    /// </summary>
    public class StructureSetXmlBuilder : MaintainableAssembler, IBuilder<StructureSetType, IStructureSetObject>
    {
        #region Fields

        /// <summary>
        ///     The category scheme map bean assembler bean.
        /// </summary>
        private readonly CategorySchemeMapAssembler _categorySchemeMapAssemblerBean = new CategorySchemeMapAssembler();

        /// <summary>
        ///     The codelist map bean assembler bean.
        /// </summary>
        private readonly CodelistMapAssembler _codelistMapAssemblerBean = new CodelistMapAssembler();

        /// <summary>
        ///     The concept scheme map bean assembler bean.
        /// </summary>
        private readonly ConceptSchemeMapAssembler _conceptSchemeMapAssemblerBean = new ConceptSchemeMapAssembler();

        /// <summary>
        ///     The organisation scheme map bean assembler bean.
        /// </summary>
        private readonly OrganisationSchemeMapAssembler _organisationSchemeMapAssemblerBean =
            new OrganisationSchemeMapAssembler();

        /// <summary>
        ///     The structure map bean assembler bean.
        /// </summary>
        private readonly StructureMapAssembler _structureMapAssemblerBean = new StructureMapAssembler();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build a <see cref="StructureSetType"/> from <paramref name="buildFrom"/> and return it
        /// </summary>
        /// <param name="buildFrom">
        /// The <see cref="IStructureSetObject"/>
        /// </param>
        /// <returns>
        /// The <see cref="StructureSetType"/>.
        /// </returns>
        public virtual StructureSetType Build(IStructureSetObject buildFrom)
        {
            // Create outgoing build
            var builtObj = new StructureSetType();

            // Populate it from inherited super
            this.AssembleMaintainable(builtObj, buildFrom);

            // Populate it using this class's specifics
            foreach (IOrganisationSchemeMapObject eachOrganisationMapBean in buildFrom.OrganisationSchemeMapList)
            {
                var newOrganisationSchemeMapType = new OrganisationSchemeMapType();
                builtObj.OrganisationSchemeMap.Add(newOrganisationSchemeMapType);
                this._organisationSchemeMapAssemblerBean.Assemble(newOrganisationSchemeMapType, eachOrganisationMapBean);
            }

            foreach (ICategorySchemeMapObject eachCategoryMapBean in buildFrom.CategorySchemeMapList)
            {
                var newCategorySchemeMapType = new CategorySchemeMapType();
                builtObj.CategorySchemeMap.Add(newCategorySchemeMapType);
                this._categorySchemeMapAssemblerBean.Assemble(newCategorySchemeMapType, eachCategoryMapBean);
            }

            foreach (IConceptSchemeMapObject eachConceptMapBean in buildFrom.ConceptSchemeMapList)
            {
                var newConceptSchemeMapType = new ConceptSchemeMapType();
                builtObj.ConceptSchemeMap.Add(newConceptSchemeMapType);
                this._conceptSchemeMapAssemblerBean.Assemble(newConceptSchemeMapType, eachConceptMapBean);
            }

            // TODO RSG REPORTING TAXONOMY MAP CURRENTLY NOT YET IN MODEL ###
            foreach (ICodelistMapObject eachCodelistMapBean in buildFrom.CodelistMapList)
            {
                var newCodelistMapType = new CodelistMapType();
                builtObj.CodelistMap.Add(newCodelistMapType);
                this._codelistMapAssemblerBean.Assemble(newCodelistMapType, eachCodelistMapBean);
            }

            /* foreach */
            foreach (IStructureMapObject eachStructureMapBean in buildFrom.StructureMapList)
            {
                var newStructureMapType = new StructureMapType();
                builtObj.StructureMap.Add(newStructureMapType);
                this._structureMapAssemblerBean.Assemble(newStructureMapType, eachStructureMapBean);
            }

            return builtObj;
        }

        #endregion
    }
}