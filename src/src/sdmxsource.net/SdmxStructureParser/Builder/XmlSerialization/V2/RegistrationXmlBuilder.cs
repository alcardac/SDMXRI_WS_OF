// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The registration xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The registration xml bean builder.
    /// </summary>
    public class RegistrationXmlBuilder : AbstractBuilder
    {
        #region Static Fields

        /// <summary>
        ///     The instance.
        /// </summary>
        private static readonly RegistrationXmlBuilder _instance = new RegistrationXmlBuilder();

        #endregion

        // PRIVATE CONSTRUCTOR
        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref="RegistrationXmlBuilder" /> class from being created.
        /// </summary>
        private RegistrationXmlBuilder()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        public static RegistrationXmlBuilder Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="RegistrationType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="RegistrationType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public RegistrationType Build(IRegistrationObject buildFrom)
        {
            var builtObj = new RegistrationType();
            if (buildFrom.LastUpdated != null)
            {
                builtObj.LastUpdated = buildFrom.LastUpdated.Date;
            }

            if (buildFrom.ValidFrom != null)
            {
                builtObj.ValidFrom = buildFrom.ValidFrom.Date;
            }

            if (buildFrom.ValidTo != null)
            {
                builtObj.ValidTo = buildFrom.ValidTo.Date;
            }

            if (buildFrom.ProvisionAgreementRef != null)
            {
                ICrossReference provRefBean = buildFrom.ProvisionAgreementRef;
                var provRefType = new ProvisionAgreementRefType();
                builtObj.ProvisionAgreementRef = provRefType;

                if (provRefBean.TargetReference.EnumType == SdmxStructureEnumType.ProvisionAgreement)
                {
                    if (ObjectUtil.ValidString(provRefBean.TargetUrn))
                    {
                        provRefType.URN = provRefBean.TargetUrn;
                    }
                }
            }

            if (buildFrom.DataSource != null)
            {
                IDataSource datasourceBean = buildFrom.DataSource;
                var datasourceType = new DatasourceType();
                builtObj.Datasource = datasourceType;
                if (datasourceBean.SimpleDatasource)
                {
                    datasourceType.SimpleDatasource = datasourceBean.DataUrl;
                }
                else
                {
                    var qdst = new QueryableDatasourceType();
                    datasourceType.QueryableDatasource = qdst;
                    qdst.isRESTDatasource = datasourceBean.RESTDatasource;
                    qdst.isWebServiceDatasource = datasourceBean.WebServiceDatasource;
                    qdst.DataUrl = datasourceBean.DataUrl;
                }
            }

            return builtObj;
        }

        #endregion
    }
}