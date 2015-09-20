// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractResponseBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The abstract response builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V2
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The abstract response builder.
    /// </summary>
    public abstract class AbstractResponseBuilder
    {
        // DEFAULT CONSTRUCTOR
        #region Public Methods and Operators

        /// <summary>
        /// Add data source from <paramref name="datasourceBean"/> to <paramref name="datasourceType"/>
        /// </summary>
        /// <param name="datasourceBean">
        /// The data source SDMX Object.
        /// </param>
        /// <param name="datasourceType">
        /// The data source LINQ2XSD object.
        /// </param>
        public void AddDatasource(IDataSource datasourceBean, DatasourceType datasourceType)
        {
            if (datasourceBean.SimpleDatasource)
            {
                if (datasourceBean.DataUrl != null)
                {
                    datasourceType.SimpleDatasource = datasourceBean.DataUrl;
                }
            }
            else
            {
                var queryableDatasourceType = new QueryableDatasourceType();
                datasourceType.QueryableDatasource = queryableDatasourceType;
                if (datasourceBean.DataUrl != null)
                {
                    queryableDatasourceType.DataUrl = datasourceBean.DataUrl;
                }

                queryableDatasourceType.isRESTDatasource = datasourceBean.RESTDatasource;
                queryableDatasourceType.isWebServiceDatasource = datasourceBean.WebServiceDatasource;
                if (datasourceBean.WsdlUrl != null)
                {
                    queryableDatasourceType.WSDLUrl = datasourceBean.WsdlUrl;
                }
            }
        }

        /// <summary>
        /// Add status message from <paramref name="ex"/> to <paramref name="statusMessage"/>
        /// </summary>
        /// <param name="statusMessage">
        /// The status message
        /// </param>
        /// <param name="ex">
        /// The exception
        /// </param>
        public void AddStatus(StatusMessageType statusMessage, Exception ex)
        {
            if (ex == null)
            {
                statusMessage.status = StatusTypeConstants.Success;
            }
            else
            {
                statusMessage.status = StatusTypeConstants.Failure;
                var tt = new TextType();
                statusMessage.MessageText.Add(tt);

                var exception = ex as SdmxException;
                tt.TypedValue = exception != null ? exception.FullMessage : ex.Message;
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