// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The code xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V1
{
    using System.Collections.Generic;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The code xml bean builder.
    /// </summary>
    public class CodeXmlBuilder : AbstractBuilder, IBuilder<CodeType, ICode>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="CodeXmlBuilder" /> class.
        /// </summary>
        static CodeXmlBuilder()
        {
            Log = LogManager.GetLogger(typeof(CodeXmlBuilder));
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="CodeType"/>.
        /// </returns>
        public virtual CodeType Build(ICode buildFrom)
        {
            var builtObj = new CodeType();

            // IN VERSION 1.0 THE CODE DESCRIPTION IS THE HUMAN READABLE CODE WORD, WE STORE THIS IN THE NAME FEILD (AS IN 2.1)
            IList<ITextTypeWrapper> collection = buildFrom.Names;
            if (ObjectUtil.ValidCollection(collection))
            {
                builtObj.Description = this.GetTextType(buildFrom.Names);
            }

            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            string str0 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                builtObj.value = buildFrom.Id;
            }

            return builtObj;
        }

        #endregion
    }
}