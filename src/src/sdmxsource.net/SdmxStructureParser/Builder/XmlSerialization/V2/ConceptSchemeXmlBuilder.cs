// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConceptSchemeXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The concept scheme xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The concept scheme xml bean builder.
    /// </summary>
    public class ConceptSchemeXmlBuilder : AbstractBuilder, IBuilder<ConceptSchemeType, IConceptSchemeObject>
    {
        #region Fields

        /// <summary>
        ///     The concept xml bean builder.
        /// </summary>
        private readonly ConceptXmlBuilder _conceptXmlBuilder = new ConceptXmlBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="ConceptSchemeType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="ConceptSchemeType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual ConceptSchemeType Build(IConceptSchemeObject buildFrom)
        {
            var builtObj = new ConceptSchemeType();
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
                builtObj.uri = builtObj.uri;
            }
            else if (buildFrom.StructureUrl != null)
            {
                builtObj.uri = buildFrom.StructureUrl;
            }
            else if (buildFrom.ServiceUrl != null)
            {
                builtObj.uri = buildFrom.StructureUrl;
            }

            builtObj.urn = buildFrom.Urn;

            string value = buildFrom.Version;
            if (!string.IsNullOrWhiteSpace(value))
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
            foreach (IConceptObject categoryBean in buildFrom.Items)
            {
                builtObj.Concept.Add(this._conceptXmlBuilder.Build(categoryBean));
            }

            return builtObj;
        }

        #endregion
    }
}