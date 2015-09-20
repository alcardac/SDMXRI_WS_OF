// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSourceBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data source core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Util;
    using System.Collections.Generic;

    /// <summary>
    ///   The data source core.
    /// </summary>
    [Serializable]
    public class DataSourceCore : SdmxStructureCore, IDataSource
    {
        #region Fields

        /// <summary>
        ///   The data url.
        /// </summary>
        private readonly Uri _dataUrl;

        /// <summary>
        ///   The is rest datasource.
        /// </summary>
        private readonly bool _isRestDatasource;

        /// <summary>
        ///   The is simple datasource.
        /// </summary>
        private readonly bool _isSimpleDatasource;

        /// <summary>
        ///   The is web service datasource.
        /// </summary>
        private readonly bool _isWebServiceDatasource;

        /// <summary>
        ///   The wadl url.
        /// </summary>
        private readonly Uri _wadlUrl;

        /// <summary>
        ///   The wsdl url.
        /// </summary>
        private readonly Uri _wsdlUrl;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS              //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceCore"/> class.
        /// </summary>
        /// <param name="datasource">
        /// The datasource. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public DataSourceCore(IDataSourceMutableObject datasource, ISdmxStructure parent)
            : base(datasource, parent)
        {
            this._dataUrl = datasource.DataUrl;
            this._wsdlUrl = datasource.WSDLUrl;
            this._isRestDatasource = datasource.RESTDatasource;
            this._isSimpleDatasource = datasource.SimpleDatasource;
            this._isWebServiceDatasource = datasource.WebServiceDatasource;
            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceCore"/> class.
        /// </summary>
        /// <param name="datasource">
        /// The datasource. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public DataSourceCore(DatasourceType datasource, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Datasource), parent)
        {
            if (datasource.SimpleDatasource != null) 
            {
                this._isSimpleDatasource = true;
                this._dataUrl = datasource.SimpleDatasource;
            }
            else
            {
                if (datasource.QueryableDatasource != null)
                {
                    this._isSimpleDatasource = false;
                    this._isRestDatasource = datasource.QueryableDatasource.isRESTDatasource;
                    this._isWebServiceDatasource = datasource.QueryableDatasource.isWebServiceDatasource;
                    this._dataUrl = datasource.QueryableDatasource.DataUrl;
                    this._wsdlUrl = datasource.QueryableDatasource.WSDLUrl;
                }
            }

            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceCore"/> class.
        /// </summary>
        /// <param name="datasource">
        /// The datasource. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public DataSourceCore(QueryableDataSourceType datasource, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Datasource), parent)
        {
            this._isSimpleDatasource = false;
            this._isRestDatasource = datasource.isRESTDatasource;
            this._isWebServiceDatasource = datasource.isWebServiceDatasource;
            this._dataUrl = datasource.DataURL;
            this._wsdlUrl = datasource.WSDLURL;
            this._wadlUrl = datasource.WADLURL;
            this.Validate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceCore"/> class.
        /// </summary>
        /// <param name="datasource">
        /// The datasource. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public DataSourceCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.QueryableDataSourceType datasource, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Datasource), parent)
        {
            this._isSimpleDatasource = false;
            this._isRestDatasource = datasource.isRESTDatasource;
            this._isWebServiceDatasource = datasource.isWebServiceDatasource;
            this._dataUrl = datasource.DataURL;
            this._wsdlUrl = datasource.WSDLURL;
            this._wadlUrl = datasource.WADLURL;
            this.Validate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceCore"/> class.
        /// </summary>
        /// <param name="simpleDatasoruce">
        /// The simple datasoruce. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public DataSourceCore(string simpleDatasoruce, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Datasource), parent)
        {
            this._isSimpleDatasource = true;
            this._dataUrl = GetUrl(simpleDatasoruce);
            this.Validate();
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the data url.
        /// </summary>
        public virtual Uri DataUrl
        {
            get
            {
                return this._dataUrl;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether rest datasource.
        /// </summary>
        public virtual bool RESTDatasource
        {
            get
            {
                return this._isRestDatasource;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether simple datasource.
        /// </summary>
        public virtual bool SimpleDatasource
        {
            get
            {
                return this._isSimpleDatasource;
            }
        }

        /// <summary>
        ///   Gets the wadl url.
        /// </summary>
        public virtual Uri WadlUrl
        {
            get
            {
                return this._wadlUrl;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether web service datasource.
        /// </summary>
        public virtual bool WebServiceDatasource
        {
            get
            {
                return this._isWebServiceDatasource;
            }
        }

        /// <summary>
        ///   Gets the wsdl url.
        /// </summary>
        public virtual Uri WsdlUrl
        {
            get
            {
                return this._wsdlUrl;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The create mutable instance.
        /// </summary>
        /// <returns> The <see cref="IDataSourceMutableObject" /> . </returns>
        public virtual IDataSourceMutableObject CreateMutableInstance()
        {
            return new DataSourceMutableCore(this);
        }

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The agencyScheme. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if (sdmxObject == null) return false;
            
            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IDataSource)sdmxObject;

                if (!ObjectUtil.Equivalent(this._dataUrl, that.DataUrl))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._wsdlUrl, that.WadlUrl))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this._wadlUrl, that.WadlUrl))
                {
                    return false;
                }

                if (this._isRestDatasource != that.RESTDatasource)
                {
                    return false;
                }

                if (this._isSimpleDatasource != that.SimpleDatasource)
                {
                    return false;
                }

                if (this._isWebServiceDatasource != that.WebServiceDatasource)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        protected internal void Validate()
        {
            if (this._dataUrl == null)
            {
                throw new SdmxSemmanticException(ExceptionCode.ObjectMissingRequiredAttribute, "DataSource", "Uri");
            }

            // HACK The schema does not allow a constraint to be attached to both a provision and a simple datasource, so here we are setting simple datasouce to false, so that 
            // the constraint will be attached to a provision and a queryable datasource - but the queryable datasource will have the following two attributes set
            // isWebServiceDatasource="false" isRESTDatasource="false"
            // This can be inferred by a system that the datasource is simple
            if (this.Parent.StructureType.EnumType != SdmxStructureEnumType.ContentConstraintAttachment)
            {
                if (!this._isRestDatasource && !this._isWebServiceDatasource && !this._isSimpleDatasource)
                {
                    throw new SdmxSemmanticException(
                        "Registration against a queryable source must either be a REST datasource or a web service datasource");
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////GETTERS                                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        /// <summary>
        /// The get url.
        /// </summary>
        /// <param name="urlString">
        /// The url string. 
        /// </param>
        /// <returns>
        /// The <see cref="Uri"/> . 
        /// </returns>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        private static Uri GetUrl(string urlString)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(urlString))
                {
                    return new Uri(urlString);
                }
            }
            catch (Exception)
            {
                throw new SdmxSemmanticException("Invalid URL '" + urlString + "'");
            }

            return null;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////COMPOSITES                           //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////	

        /// <summary>
        /// The get composites internal.
        /// </summary>
        protected override ISet<ISdmxObject> GetCompositesInternal()
        {
            return new HashSet<ISdmxObject>();
        }

        #endregion
    }
}