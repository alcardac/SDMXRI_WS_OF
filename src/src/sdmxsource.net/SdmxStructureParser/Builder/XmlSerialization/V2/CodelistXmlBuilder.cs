// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodelistXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The codelist xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
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

        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="CodeListType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="CodeListType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual CodeListType Build(ICodelistObject buildFrom)
        {
            var builtObj = new CodeListType();
            string str0 = buildFrom.AgencyId;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                builtObj.agencyID = buildFrom.AgencyId;
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

            if (ObjectUtil.ValidString(buildFrom.Urn))
            {
                builtObj.urn = buildFrom.Urn;
            }

            string value1 = buildFrom.Version;
            if (!string.IsNullOrWhiteSpace(value1))
            {
                builtObj.version = buildFrom.Version;
            }

            if (buildFrom.StartDate != null)
            {
                builtObj.validFrom = buildFrom.StartDate.Date;
            }

            if (buildFrom.EndDate != null)
            {
                builtObj.validTo = buildFrom.EndDate.Date;
            }

            IList<ITextTypeWrapper> names = buildFrom.Names;
            if (ObjectUtil.ValidCollection(names))
            {
                builtObj.Name = this.GetTextType(names);
            }

            IList<ITextTypeWrapper> descriptions = buildFrom.Descriptions;
            if (ObjectUtil.ValidCollection(descriptions))
            {
                builtObj.Description = this.GetTextType(descriptions);
            }

            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            if (buildFrom.IsExternalReference.IsSet())
            {
                builtObj.isExternalReference = buildFrom.IsExternalReference.IsTrue;
            }

            if (buildFrom.IsFinal.IsSet())
            {
                builtObj.isFinal = buildFrom.IsFinal.IsTrue;
            }

            foreach (ICode code in buildFrom.Items)
            {
                builtObj.Code.Add(this._codeXmlBuilder.Build(code));
            }

            ////int i = 0;
            ////IList<ICode> codes = buildFrom.Items;
            ////var codeTypes = new CodeType[codes.Count];

            /////* foreach */
            ////foreach (ICode codeBean in codes)
            ////{
            ////    codeTypes[i] = this._codeXmlBuilder.Build(codeBean);
            ////    i++;
            ////}

            ////builtObj.Code = codeTypes;
            return builtObj;
        }

        #endregion
    }
}