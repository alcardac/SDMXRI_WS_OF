// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The code xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The code xml bean builder.
    /// </summary>
    public class CodeXmlBuilder : AbstractBuilder, IBuilder<CodeType, ICode>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="CodeType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="CodeType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual CodeType Build(ICode buildFrom)
        {
            var builtObj = new CodeType();
            if (ObjectUtil.ValidString(buildFrom.Urn))
            {
                builtObj.urn = buildFrom.Urn;
            }

            // IN VERSION 2.0 THE CODE DESCRIPTION IS THE HUMAN READABLE CODE WORD, WE STORE THIS IN THE NAME FEILD (AS IN 2.1)
            IList<ITextTypeWrapper> collection = buildFrom.Names;
            if (ObjectUtil.ValidCollection(collection))
            {
                builtObj.Description = this.GetTextType(collection);
            }

            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            string value = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(value))
            {
                builtObj.value = buildFrom.Id;
            }

            string value1 = buildFrom.ParentCode;
            if (!string.IsNullOrWhiteSpace(value1))
            {
                builtObj.parentCode = buildFrom.ParentCode;
            }

            return builtObj;
        }

        #endregion
    }
}