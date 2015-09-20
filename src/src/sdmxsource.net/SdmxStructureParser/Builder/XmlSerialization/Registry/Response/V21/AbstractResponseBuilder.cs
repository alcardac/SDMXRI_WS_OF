// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The abstract response builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Model.Impl;

    using QueryableDataSourceType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.QueryableDataSourceType;
    using StatusMessageType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.StatusMessageType;

    // TODO v2.1

    /// <summary>
    ///     The abstract response builder.
    /// </summary>
    public abstract class AbstractResponseBuilder
    {
        // DEFAULT CONSTRUCTOR
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AbstractResponseBuilder" /> class.
        /// </summary>
        internal AbstractResponseBuilder()
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds <paramref name="datasourceBean"/> to <paramref name="datasourceType"/>
        /// </summary>
        /// <param name="datasourceBean">
        /// The <see cref="IDataSource"/>.
        /// </param>
        /// <param name="datasourceType">
        /// The <see cref="DataSourceType"/>
        /// </param>
        public void AddDatasource(IDataSource datasourceBean, DataSourceType datasourceType)
        {
            if (datasourceBean.SimpleDatasource)
            {
                datasourceType.SimpleDataSource.Add(datasourceBean.DataUrl);
            }
            else
            {
                var queryableDatasourceType = new QueryableDataSourceType();
                datasourceType.QueryableDataSource.Add(queryableDatasourceType);
                queryableDatasourceType.DataURL = datasourceBean.DataUrl;
                queryableDatasourceType.isRESTDatasource = datasourceBean.RESTDatasource;
                queryableDatasourceType.isWebServiceDatasource = datasourceBean.WebServiceDatasource;
                if (datasourceBean.WsdlUrl != null)
                {
                    queryableDatasourceType.WSDLURL = datasourceBean.WsdlUrl;
                }

                if (datasourceBean.WadlUrl != null)
                {
                    queryableDatasourceType.WADLURL = datasourceBean.WadlUrl;
                }
            }
        }

        /// <summary>
        /// Adds status to <paramref name="statusMessage"/> from <paramref name="exception"/>
        /// </summary>
        /// <param name="statusMessage">
        /// The status message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void AddStatus(StatusMessageType statusMessage, Exception exception)
        {
            // FUNC 2.1 Where is the status attribute?
            if (exception == null)
            {
                statusMessage.status = StatusTypeConstants.Success;
            }
            else
            {
                statusMessage.status = StatusTypeConstants.Failure;
                ErrorReport errorReport = ErrorReport.Build(exception);
                if (ObjectUtil.ValidCollection(errorReport.ErrorMessage))
                {
                    var statusMessageType = new Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.StatusMessageType();
                    statusMessage.MessageText.Add(statusMessageType);

                    foreach (string errors in errorReport.ErrorMessage)
                    {
                        var text = new TextType();
                        statusMessageType.Text.Add(text);
                        text.TypedValue = errors;
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The has annotations.
        /// </summary>
        /// <param name="annotable">
        /// The annotable.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        internal bool HasAnnotations(IAnnotableObject annotable)
        {
            if (ObjectUtil.ValidCollection(annotable.Annotations))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}