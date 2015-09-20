// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchicalCodelistXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The hierarchical codelist xml builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    /// <summary>
    ///     The hierarchical codelist xml builder.
    /// </summary>
    public class HierarchicalCodelistXmlBuilder : MaintainableAssembler, 
                                                  IBuilder<HierarchicalCodelistType, IHierarchicalCodelistObject>
    {
        #region Fields

        /// <summary>
        ///     The codelist ref bean assembler bean.
        /// </summary>
        private readonly CodelistRefAssembler _codelistRefAssemblerBean = new CodelistRefAssembler();

        /// <summary>
        ///     The hierarchy bean assembler bean.
        /// </summary>
        private readonly HierarchyAssembler _hierarchyAssemblerBean = new HierarchyAssembler();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="HierarchicalCodelistType"/>.
        /// </returns>
        public virtual HierarchicalCodelistType Build(IHierarchicalCodelistObject buildFrom)
        {
            // Create outgoing build
            var builtObj = new HierarchicalCodelistType();

            // Populate it from inherited super
            this.AssembleMaintainable(builtObj, buildFrom);

            // Populate it using this class's specifics
            // Code refs
            foreach (ICodelistRef eachCodelistRef in buildFrom.CodelistRef)
            {
                var newCodelistRef = new IncludedCodelistReferenceType();
                builtObj.IncludedCodelist.Add(newCodelistRef);
                this._codelistRefAssemblerBean.Assemble(newCodelistRef, eachCodelistRef);
            }

            // Hierarchies
            if (buildFrom.Hierarchies != null)
            {
                /* foreach */
                foreach (IHierarchy eachHierarchy in buildFrom.Hierarchies)
                {
                    var newValueHierarchy = new HierarchyType();
                    builtObj.Hierarchy.Add(newValueHierarchy);
                    this._hierarchyAssemblerBean.Assemble(newValueHierarchy, eachHierarchy);
                }
            }

            return builtObj;
        }

        #endregion
    }
}