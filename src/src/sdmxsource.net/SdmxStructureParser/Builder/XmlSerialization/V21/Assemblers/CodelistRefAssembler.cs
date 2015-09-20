// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodelistRefAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The codelist ref bean assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The codelist ref bean assembler.
    /// </summary>
    public class CodelistRefAssembler : AbstractAssembler, IAssembler<IncludedCodelistReferenceType, ICodelistRef>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Assemble from <paramref name="assembleFrom"/> into <paramref name="assembleInto"/>.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <param name="assembleFrom">
        /// The assemble from.
        /// </param>
        public virtual void Assemble(IncludedCodelistReferenceType assembleInto, ICodelistRef assembleFrom)
        {
            // Populate it from inherited super
            ICrossReference codelistReference = assembleFrom.CodelistReference;
            if (codelistReference != null)
            {
                var codelistRefType = new CodelistRefType();
                assembleInto.SetTypedRef(codelistRefType);
                this.SetReference(codelistRefType, codelistReference);
            }

            // Populate it using this class's specifics
            string value = assembleFrom.Alias;
            if (!string.IsNullOrWhiteSpace(value))
            {
                assembleInto.alias = assembleFrom.Alias;
            }
        }

        #endregion
    }
}