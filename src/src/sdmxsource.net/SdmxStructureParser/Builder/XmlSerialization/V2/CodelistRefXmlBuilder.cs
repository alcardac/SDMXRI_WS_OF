// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodelistRefXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The codelist ref xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The codelist ref xml bean builder.
    /// </summary>
    public class CodelistRefXmlBuilder : AbstractBuilder, IBuilder<CodelistRefType, ICodelistRef>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="CodelistRefType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="CodelistRefType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual CodelistRefType Build(ICodelistRef buildFrom)
        {
            var builtObj = new CodelistRefType();
            if (buildFrom.CodelistReference != null)
            {
                IMaintainableRefObject maintRef = buildFrom.CodelistReference.MaintainableReference;
                string str0 = maintRef.AgencyId;
                if (!string.IsNullOrWhiteSpace(str0))
                {
                    builtObj.AgencyID = maintRef.AgencyId;
                }

                string str1 = buildFrom.Alias;
                if (!string.IsNullOrWhiteSpace(str1))
                {
                    builtObj.Alias = buildFrom.Alias;
                }

                string str2 = maintRef.MaintainableId;
                if (!string.IsNullOrWhiteSpace(str2))
                {
                    builtObj.CodelistID = maintRef.MaintainableId;
                }

                builtObj.URN = buildFrom.CodelistReference.TargetUrn;

                string str4 = maintRef.Version;
                if (!string.IsNullOrWhiteSpace(str4))
                {
                    builtObj.Version = maintRef.Version;
                }
            }

            return builtObj;
        }

        #endregion
    }
}