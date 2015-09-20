// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodelistXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The codelist xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    /// <summary>
    ///     The codelist xml bean builder.
    /// </summary>
    public class CodelistXmlBuilder : ItemSchemeAssembler, IBuilder<CodelistType, ICodelistObject>
    {
        #region Fields

        /// <summary>
        ///     The code bean assembler bean.
        /// </summary>
        private readonly CodeAssembler _codeAssemblerBean = new CodeAssembler();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="CodelistType"/>.
        /// </returns>
        public virtual CodelistType Build(ICodelistObject buildFrom)
        {
            // Create outgoing build
            var builtObj = new CodelistType();

            if (buildFrom.Partial)
                builtObj.isPartial = true;

            // Populate it from inherited super
            this.AssembleItemScheme(builtObj, buildFrom);

            // Populate it using this class's specifics
            IList<ICode> codes = buildFrom.Items;
            if (codes.Count > 0)
            {
                foreach (ICode codeBean in codes)
                {
                    var newCode = new Code();
                    builtObj.Item.Add(newCode);
                    this._codeAssemblerBean.Assemble(newCode.Content, codeBean);
                }
            }

            return builtObj;
        }

        #endregion
    }
}