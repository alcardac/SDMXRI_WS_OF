// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The code bean assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The code bean assembler.
    /// </summary>
    public class CodeAssembler : NameableAssembler, IAssembler<CodeType, ICode>
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
        public virtual void Assemble(CodeType assembleInto, ICode assembleFrom)
        {
            // Populate it from inherited super
            this.AssembleNameable(assembleInto, assembleFrom);

            // Populate it using this class's specifics
            string value = assembleFrom.ParentCode;
            if (!string.IsNullOrWhiteSpace(value))
            {
                LocalItemReferenceType parent = new LocalCodeReferenceType();
                assembleInto.SetTypedParent(parent);
                var xref = new CodeRefType();
                parent.SetTypedRef(xref);
                xref.id = assembleFrom.ParentCode;
            }
        }

        #endregion
    }
}