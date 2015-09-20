// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportingTaxonomyXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The reporting taxonomy xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    /// <summary>
    ///     The reporting taxonomy xml bean builder.
    /// </summary>
    public class ReportingTaxonomyXmlBuilder : ItemSchemeAssembler, 
                                               IBuilder<ReportingTaxonomyType, IReportingTaxonomyObject>
    {
        #region Fields

        /// <summary>
        ///     The reporting category bean assembler bean.
        /// </summary>
        private readonly ReportingCategoryAssembler _reportingCategoryAssemblerBean = new ReportingCategoryAssembler();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build a <see cref="ReportingTaxonomyType"/> from <paramref name="buildFrom"/> and return it
        /// </summary>
        /// <param name="buildFrom">
        /// The <see cref="IReportingTaxonomyObject"/>
        /// </param>
        /// <returns>
        /// The <see cref="ReportingTaxonomyType"/>.
        /// </returns>
        public virtual ReportingTaxonomyType Build(IReportingTaxonomyObject buildFrom)
        {
            // Create outgoing build
            var builtObj = new ReportingTaxonomyType();

            // Populate it from inherited super
            this.AssembleItemScheme(builtObj, buildFrom);

            // Populate it using this class's specifics
            foreach (IReportingCategoryObject eachReportingCategoryBean in buildFrom.Items)
            {
                var newReportingCategory = new ReportingCategory();
                builtObj.Item.Add(newReportingCategory);
                this._reportingCategoryAssemblerBean.Assemble(newReportingCategory.Content, eachReportingCategoryBean);
            }

            return builtObj;
        }

        #endregion
    }
}