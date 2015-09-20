// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportingTaxonomyXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The reporting taxonomy xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The reporting taxonomy xml bean builder.
    /// </summary>
    public class ReportingTaxonomyXmlBuilder : AbstractBuilder
    {
        #region Fields

        /// <summary>
        ///     The category xml bean builder.
        /// </summary>
        private readonly CategoryXmlBuilder _categoryXmlBuilder = new CategoryXmlBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="ReportingTaxonomyType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="ReportingTaxonomyType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public ReportingTaxonomyType Build(IReportingTaxonomyObject buildFrom)
        {
            var builtObj = new ReportingTaxonomyType();

            // MAINTAINABLE ATTRIBUTES
            string str0 = buildFrom.AgencyId;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                builtObj.agencyID = buildFrom.AgencyId;
            }

            string str1 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(str1))
            {
                builtObj.id = buildFrom.Id;
            }

            if (buildFrom.Uri != null)
            {
                builtObj.uri = buildFrom.Uri;
            }

            builtObj.urn = buildFrom.Urn;

            string str3 = buildFrom.Version;
            if (!string.IsNullOrWhiteSpace(str3))
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

            // CATEGORIES
            foreach (IReportingCategoryObject categoryBean in buildFrom.Items)
            {
                builtObj.Category.Add(this._categoryXmlBuilder.Build(categoryBean));
            }

            return builtObj;
        }

        #endregion
    }
}