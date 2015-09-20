// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodelistXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The codelist xml bean builder.
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
    ///     The codelist xml bean builder.
    /// </summary>
    public class CodelistXmlBuilder : AbstractBuilder, IBuilder<CodeListType, ICodelistObject>
    {
        #region Fields

        /// <summary>
        ///     The code xml bean builder.
        /// </summary>
        private readonly CodeXmlBuilder _codeXmlBuilder = new CodeXmlBuilder();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="CodelistXmlBuilder" /> class.
        /// </summary>
        static CodelistXmlBuilder()
        {
            Log = LogManager.GetLogger(typeof(CodelistXmlBuilder));
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
        /// The <see cref="CodeListType"/>.
        /// </returns>
        public virtual CodeListType Build(ICodelistObject buildFrom)
        {
            var builtObj = new CodeListType();
            string str0 = buildFrom.AgencyId;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                builtObj.agency = buildFrom.AgencyId;
            }

            string value = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(value))
            {
                builtObj.id = buildFrom.Id;
            }

            if (buildFrom.Uri != null)
            {
                builtObj.uri = buildFrom.Uri;
            }
            else if (buildFrom.StructureUrl != null)
            {
                builtObj.uri = buildFrom.StructureUrl;
            }
            else if (buildFrom.ServiceUrl != null)
            {
                builtObj.uri = buildFrom.StructureUrl;
            }

            string value1 = buildFrom.Version;
            if (!string.IsNullOrWhiteSpace(value1))
            {
                builtObj.version = buildFrom.Version;
            }

            IList<ITextTypeWrapper> names = buildFrom.Names;
            if (ObjectUtil.ValidCollection(names))
            {
                builtObj.Name = this.GetTextType(names);
            }

            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            if (buildFrom.IsExternalReference.IsSet())
            {
                builtObj.isExternalReference = buildFrom.IsExternalReference.IsTrue;
            }

            /* foreach */
            foreach (ICode codeBean in buildFrom.Items)
            {
                builtObj.Code.Add(this._codeXmlBuilder.Build(codeBean));
            }

            return builtObj;
        }

        #endregion
    }
}