// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationSchemeXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The organisation scheme xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System.Collections.Generic;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The organisation scheme xml bean builder.
    /// </summary>
    public class OrganisationSchemeXmlBuilder : AbstractBuilder
    {
        #region Fields

        /// <summary>
        ///     The organisation role xml bean builder.
        /// </summary>
        private readonly OrganisationRoleXmlBuilder _organisationRoleXmlBuilder = new OrganisationRoleXmlBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="OrganisationSchemeType"/> from <paramref name="dataProviderSchemeBean"/>.
        /// </summary>
        /// <param name="dataProviderSchemeBean">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="OrganisationSchemeType"/> from <paramref name="dataProviderSchemeBean"/> .
        /// </returns>
        public OrganisationSchemeType Build(IDataProviderScheme dataProviderSchemeBean)
        {
            OrganisationSchemeType builtObj = this.GetOrganisationSchemeType(dataProviderSchemeBean);
            if (dataProviderSchemeBean.Items.Count > 0)
            {
                var type = new DataProvidersType();
                builtObj.DataProviders.Add(type);

                /* foreach */
                foreach (IDataProvider currentDc in dataProviderSchemeBean.Items)
                {
                    type.DataProvider.Add(this._organisationRoleXmlBuilder.Build(currentDc));
                }
            }
            return builtObj;
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="dataConsumerScheme">
        /// The data consumer scheme.
        /// </param>
        /// <returns>
        /// The <see cref="OrganisationSchemeType"/> .
        /// </returns>
        public OrganisationSchemeType Build(IDataConsumerScheme dataConsumerScheme)
        {
            OrganisationSchemeType builtObj = this.GetOrganisationSchemeType(dataConsumerScheme);
            if (dataConsumerScheme.Items.Count > 0)
            {
                var dataConsumersType = new DataConsumersType();
                builtObj.DataConsumers.Add(dataConsumersType);

                /* foreach */
                foreach (IDataConsumer currentDc in dataConsumerScheme.Items)
                {
                    dataConsumersType.DataConsumer.Add(this._organisationRoleXmlBuilder.Build(currentDc));
                }
            }
            return builtObj;
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="agencySchemeBean">
        /// The agency scheme bean.
        /// </param>
        /// <returns>
        /// The <see cref="OrganisationSchemeType"/> .
        /// </returns>
        public OrganisationSchemeType Build(IAgencyScheme agencySchemeBean)
        {
            OrganisationSchemeType builtObj = this.GetOrganisationSchemeType(agencySchemeBean);
            if (agencySchemeBean.Items.Count > 0)
            {
                var type = new AgenciesType();
                builtObj.Agencies.Add(type);

                /* foreach */
                foreach (IAgency currentBean in agencySchemeBean.Items)
                {
                    type.Agency.Add(this._organisationRoleXmlBuilder.Build(currentBean));
                }
            }
            return builtObj;
        }

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="currentBean">
        /// The organisation unit scheme object.
        /// </param>
        /// <returns>
        /// The <see cref="OrganisationSchemeType"/> .
        /// </returns>
        public OrganisationSchemeType Build(IOrganisationUnitSchemeObject currentBean)
        {
            throw new SdmxNotImplementedException(ExceptionCode.Unsupported, SdmxStructureEnumType.OrganisationUnitScheme.GetType());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets organisation scheme type.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="OrganisationSchemeType"/> .
        /// </returns>
        private OrganisationSchemeType GetOrganisationSchemeType(IMaintainableObject buildFrom)
        {
            var builtObj = new OrganisationSchemeType();

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

            return builtObj;
        }

        #endregion
    }
}