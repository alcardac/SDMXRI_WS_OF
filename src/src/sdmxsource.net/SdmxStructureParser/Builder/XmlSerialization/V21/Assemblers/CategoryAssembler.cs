// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The category bean assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;

    /// <summary>
    ///     The category bean assembler.
    /// </summary>
    public class CategoryAssembler : NameableAssembler, IAssembler<CategoryType, ICategoryObject>
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
        public virtual void Assemble(CategoryType assembleInto, ICategoryObject assembleFrom)
        {
            this.AssembleNameable(assembleInto, assembleFrom);
        }

        #endregion
    }
}