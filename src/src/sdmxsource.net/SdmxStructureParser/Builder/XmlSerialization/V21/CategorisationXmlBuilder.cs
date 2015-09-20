// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorisationXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The categorisation xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    /// <summary>
    ///     The categorisation xml bean builder.
    /// </summary>
    public class CategorisationXmlBuilder : MaintainableAssembler, IBuilder<CategorisationType, ICategorisationObject>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="CategorisationType"/>.
        /// </returns>
        public virtual CategorisationType Build(ICategorisationObject buildFrom)
        {
            var returnType = new CategorisationType();

            this.AssembleMaintainable(returnType, buildFrom);
            if (buildFrom.StructureReference != null)
            {
                var objRefType = new ObjectReferenceType();
                returnType.Source = objRefType;
                var objectRefType = new ObjectRefType();
                objRefType.SetTypedRef(objectRefType);
                this.SetReference(objectRefType, buildFrom.StructureReference);
            }

            if (buildFrom.CategoryReference != null)
            {
                var catRefType = new CategoryReferenceType();
                returnType.Target = catRefType;
                var categoryRefType = new CategoryRefType();
                catRefType.SetTypedRef(categoryRefType);
                this.SetReference(categoryRefType, buildFrom.CategoryReference);
            }

            return returnType;
        }

        #endregion
    }
}