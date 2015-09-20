// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorySchemeXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Builds a v2 CategorySchemeType from a schema independent ICategorySchemeObject
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
    ///     Builds a v2 CategorySchemeType from a schema independent ICategorySchemeObject
    /// </summary>
    public class CategorySchemeXmlBuilder : AbstractBuilder
    {
        #region Fields

        /// <summary>
        ///     The category xml bean builder.
        /// </summary>
        private readonly CategoryXmlBuilder _categoryXmlBuilder = new CategoryXmlBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="CategorySchemeType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <param name="categorisations">
        /// The categorisations
        /// </param>
        /// <returns>
        /// The <see cref="CategorySchemeType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public CategorySchemeType Build(ICategorySchemeObject buildFrom, ISet<ICategorisationObject> categorisations)
        {
            var builtObj = new CategorySchemeType();
            string value = buildFrom.AgencyId;
            if (!string.IsNullOrWhiteSpace(value))
            {
                builtObj.agencyID = buildFrom.AgencyId;
            }

            string value1 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(value1))
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

            string value2 = buildFrom.Version;
            if (!string.IsNullOrWhiteSpace(value2))
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

            /* foreach */
            foreach (ICategoryObject categoryBean in buildFrom.Items)
            {
                builtObj.Category.Add(this._categoryXmlBuilder.Build(categoryBean, categorisations));
            }

            return builtObj;
        }

        #endregion
    }
}