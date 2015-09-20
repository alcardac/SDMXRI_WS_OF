// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSourceMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data source mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///   The data source mutable core.
    /// </summary>
    [Serializable]
    public class DataSourceMutableCore : MutableCore, IDataSourceMutableObject
    {
        #region Fields

        /// <summary>
        ///   The data url.
        /// </summary>
        private Uri _dataUrl;

        /// <summary>
        ///   The is rest datasource.
        /// </summary>
        private bool _isRestDatasource;

        /// <summary>
        ///   The is simple datasource.
        /// </summary>
        private bool _isSimpleDatasource;

        /// <summary>
        ///   The is web service datasource.
        /// </summary>
        private bool _isWebServiceDatasource;

        /// <summary>
        ///   The wadl url.
        /// </summary>
        private Uri _wadlUrl;

        /// <summary>
        ///   The wsdl url.
        /// </summary>
        private Uri _wsdlUrl;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="DataSourceMutableCore" /> class.
        /// </summary>
        public DataSourceMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Datasource))
        {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM IMMUTABLE OBJECT                 //////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceMutableCore"/> class.
        /// </summary>
        /// <param name="datasource">
        /// The idatasource. 
        /// </param>
        public DataSourceMutableCore(IDataSource datasource)
            : base(datasource)
        {
            if (datasource.DataUrl != null)
            {
                this._dataUrl = datasource.DataUrl;
            }

            if (datasource.WsdlUrl != null)
            {
                this._wsdlUrl = datasource.WsdlUrl;
            }

            if (datasource.WadlUrl != null)
            {
                this._wadlUrl = datasource.WadlUrl;
            }

            this._isRestDatasource = datasource.RESTDatasource;
            this._isSimpleDatasource = datasource.SimpleDatasource;
            this._isWebServiceDatasource = datasource.WebServiceDatasource;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the data url.
        /// </summary>
        public virtual Uri DataUrl
        {
            get
            {
                return this._dataUrl;
            }

            set
            {
                this._dataUrl = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether rest datasource.
        /// </summary>
        public virtual bool RESTDatasource
        {
            get
            {
                return this._isRestDatasource;
            }

            set
            {
                this._isRestDatasource = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether simple datasource.
        /// </summary>
        public virtual bool SimpleDatasource
        {
            get
            {
                return this._isSimpleDatasource;
            }

            set
            {
                this._isSimpleDatasource = value;
            }
        }

        /// <summary>
        ///   Gets or sets the wadl url.
        /// </summary>
        public virtual Uri WADLUrl
        {
            get
            {
                return this._wadlUrl;
            }

            set
            {
                this._wadlUrl = value;
            }
        }

        /// <summary>
        ///   Gets or sets the wsdl url.
        /// </summary>
        public virtual Uri WSDLUrl
        {
            get
            {
                return this._wsdlUrl;
            }

            set
            {
                this._wsdlUrl = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether web service datasource.
        /// </summary>
        public virtual bool WebServiceDatasource
        {
            get
            {
                return this._isWebServiceDatasource;
            }

            set
            {
                this._isWebServiceDatasource = value;
            }
        }

        #endregion
    }
}