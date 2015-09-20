// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationRoleXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Builds a v2 OrganisationType from a schema independent IOrganisationRole
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     Builds a v2 OrganisationType from a schema independent IOrganisationRole
    /// </summary>
    public class OrganisationRoleXmlBuilder : AbstractBuilder, IBuilder<OrganisationType, IOrganisation>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="OrganisationType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="OrganisationType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual OrganisationType Build(IOrganisation buildFrom)
        {
            var builtObj = new OrganisationType();
            string str0 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                builtObj.id = buildFrom.Id;
            }

            if (buildFrom.Uri != null)
            {
                builtObj.uri = buildFrom.Uri;
            }

            builtObj.urn = buildFrom.Urn;

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

            return builtObj;
        }

        #endregion
    }
}