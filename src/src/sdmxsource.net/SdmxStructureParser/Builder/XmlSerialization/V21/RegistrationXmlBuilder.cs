// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The registration xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    using QueryableDataSourceType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.QueryableDataSourceType;

    /// <summary>
    ///     The registration xml bean builder.
    /// </summary>
    public class RegistrationXmlBuilder : AbstractAssembler, IBuilder<RegistrationType, IRegistrationObject>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build a <see cref="RegistrationType"/> from <paramref name="buildFrom"/> and return it
        /// </summary>
        /// <param name="buildFrom">
        /// The <see cref="IRegistrationObject"/>
        /// </param>
        /// <returns>
        /// The <see cref="RegistrationType"/>.
        /// </returns>
        public virtual RegistrationType Build(IRegistrationObject buildFrom)
        {
            var returnType = new RegistrationType { id = buildFrom.Id };
            if (buildFrom.IndexAttribtues.IsSet())
            {
                returnType.indexAttributes = buildFrom.IndexAttribtues.IsTrue;
            }

            if (buildFrom.IndexDataset.IsSet())
            {
                returnType.indexDataSet = buildFrom.IndexDataset.IsTrue;
            }

            if (buildFrom.IndexReportingPeriod.IsSet())
            {
                returnType.indexReportingPeriod = buildFrom.IndexReportingPeriod.IsTrue;
            }

            if (buildFrom.IndexTimeseries.IsSet())
            {
                returnType.indexTimeSeries = buildFrom.IndexTimeseries.IsTrue;
            }

            var provisionAgreementRefType = new ProvisionAgreementRefType();
            (returnType.ProvisionAgreement = new ProvisionAgreementReferenceType()).SetTypedRef(
                provisionAgreementRefType);

            this.SetReference(provisionAgreementRefType, buildFrom.ProvisionAgreementRef);
            if (buildFrom.DataSource != null)
            {
                var dataSourceType = new DataSourceType();
                returnType.Datasource = dataSourceType;
                IDataSource dataSource = buildFrom.DataSource;
                var dataUrl = dataSource.DataUrl;
                if (dataSource.SimpleDatasource)
                {
                    var simpleDataSource = dataUrl;
                    dataSourceType.SimpleDataSource.Add(simpleDataSource);
                }
                else
                {
                    var queryableDataSource = new QueryableDataSourceType();
                    dataSourceType.QueryableDataSource.Add(queryableDataSource);
                    queryableDataSource.DataURL = dataUrl;
                    if (dataSource.WsdlUrl != null)
                    {
                        queryableDataSource.WADLURL = dataSource.WsdlUrl;
                    }

                    if (dataSource.WadlUrl != null)
                    {
                        queryableDataSource.WSDLURL = dataSource.WadlUrl;
                    }

                    if (dataSource.RESTDatasource)
                    {
                        queryableDataSource.isRESTDatasource = true;
                        queryableDataSource.isWebServiceDatasource = false;
                    }
                    else if (dataSource.WebServiceDatasource)
                    {
                        queryableDataSource.isWebServiceDatasource = true;
                        queryableDataSource.isRESTDatasource = false;
                    }
                }
            }

            if (buildFrom.LastUpdated != null)
            {
                returnType.lastUpdated = buildFrom.LastUpdated.Date;
            }

            if (buildFrom.ValidFrom != null)
            {
                returnType.validFrom = buildFrom.ValidFrom.Date;
            }

            if (buildFrom.ValidTo != null)
            {
                returnType.validTo = buildFrom.ValidTo.Date;
            }

            return returnType;
        }

        #endregion
    }
}