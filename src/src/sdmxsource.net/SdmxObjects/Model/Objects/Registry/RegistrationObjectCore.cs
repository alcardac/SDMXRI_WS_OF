// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The registration dataStructureObject core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry
{
    using System;
    using System.Collections.Generic;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Random;

    using RegistrationType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.RegistrationType;

    /// <summary>
    ///   The registration dataStructureObject core.
    /// </summary>
    [Serializable]
    public class RegistrationObjectCore : MaintainableObjectCore<IMaintainableObject, IRegistrationMutableObject>, 
                                          IRegistrationObject
    {
        #region Static Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(RegistrationObjectCore));

        #endregion

        #region Fields

        /// <summary>
        ///   The _data source.
        /// </summary>
        private readonly IDataSource _dataSource;

        /// <summary>
        ///   The _index attribtues.
        /// </summary>
        private readonly TertiaryBool _indexAttribtues;

        /// <summary>
        ///   The _index dataset.
        /// </summary>
        private readonly TertiaryBool _indexDataset;

        /// <summary>
        ///   The _index reporting period.
        /// </summary>
        private readonly TertiaryBool _indexReportingPeriod;

        /// <summary>
        ///   The _index time series.
        /// </summary>
        private readonly TertiaryBool indexTimeseries;

        /// <summary>
        ///   The _last updated.
        /// </summary>
        private  ISdmxDate _lastUpdated;

        /// <summary>
        ///   The _provision agreement ref.
        /// </summary>
        private readonly ICrossReference _provisionAgreementRef;

        /// <summary>
        ///   The _valid from.
        /// </summary>
        private readonly ISdmxDate _validFrom;

        /// <summary>
        ///   The _valid to.
        /// </summary>
        private readonly ISdmxDate _validTo;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationObjectCore"/> class.
        /// </summary>
        /// <param name="registrationMutable">
        /// The registration mutable. 
        /// </param>
        public RegistrationObjectCore(IRegistrationMutableObject registrationMutable)
            : base(registrationMutable)
        {
            this._lastUpdated = new SdmxDateCore(DateTime.Now, TimeFormatEnumType.DateTime);
            this.indexTimeseries = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._indexDataset = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._indexAttribtues = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._indexReportingPeriod = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            Log.Debug("Building IRegistrationObject from Mutable Object");
            if (registrationMutable.LastUpdated != null)
            {
                this._lastUpdated = new SdmxDateCore(registrationMutable.LastUpdated, TimeFormatEnumType.DateTime);
            }

            if (registrationMutable.ValidFrom != null)
            {
                this._validFrom = new SdmxDateCore(registrationMutable.ValidFrom, TimeFormatEnumType.DateTime);
            }

            if (registrationMutable.ValidTo != null)
            {
                this._validTo = new SdmxDateCore(registrationMutable.ValidTo, TimeFormatEnumType.DateTime);
            }

            if (registrationMutable.ProvisionAgreementRef != null)
            {
                this._provisionAgreementRef = new CrossReferenceImpl(this, registrationMutable.ProvisionAgreementRef);
            }

            if (registrationMutable.IndexAttributes != null)
            {
                this._indexAttribtues = registrationMutable.IndexAttributes;
            }

            if (registrationMutable.IndexDataset != null)
            {
                this._indexDataset = registrationMutable.IndexDataset;
            }

            if (registrationMutable.IndexReportingPeriod != null)
            {
                this._indexReportingPeriod = registrationMutable.IndexReportingPeriod;
            }

            if (registrationMutable.IndexTimeseries != null)
            {
                this.indexTimeseries = registrationMutable.IndexTimeseries;
            }

            if (registrationMutable.DataSource != null)
            {
                this._dataSource = new DataSourceCore(registrationMutable.DataSource, this);
            }

            this.Validate();
            if (Log.IsDebugEnabled)
            {
                Log.Debug("IRegistrationObject Built " + this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationObjectCore"/> class.
        /// </summary>
        /// <param name="registrationType">
        /// The registration type. 
        /// </param>
        public RegistrationObjectCore(RegistrationType registrationType)
            : base(
                null, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Registration), 
                null, 
                null, 
                null, 
                TertiaryBool.GetFromEnum(TertiaryBoolEnumType.False), 
                "NA", 
                null, 
                null, 
                null, 
                null, 
                TertiaryBool.GetFromEnum(TertiaryBoolEnumType.False), 
                null)
        {
            this._lastUpdated = new SdmxDateCore(DateTime.Now, TimeFormatEnumType.DateTime);
            this.indexTimeseries = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._indexDataset = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._indexAttribtues = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._indexReportingPeriod = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            Log.Debug("Building IRegistrationObject from 2.1 SDMX");
            if (registrationType.validFrom != null)
            {
                this._validFrom = new SdmxDateCore(registrationType.validFrom, TimeFormatEnumType.DateTime);
            }

            if (registrationType.lastUpdated != null)
            {
                this._lastUpdated = new SdmxDateCore(registrationType.lastUpdated, TimeFormatEnumType.DateTime);
            }

            if (registrationType.validTo != null)
            {
                this._validTo = new SdmxDateCore(registrationType.validTo, TimeFormatEnumType.DateTime);
            }

            if (registrationType.Datasource != null)
            {
                if (ObjectUtil.ValidCollection(registrationType.Datasource.QueryableDataSource))
                {
                    this._dataSource = new DataSourceCore(registrationType.Datasource.QueryableDataSource[0], this);
                }
                else if (ObjectUtil.ValidCollection(registrationType.Datasource.SimpleDataSource))
                {
                    this._dataSource = new DataSourceCore(
                        registrationType.Datasource.SimpleDataSource[0].ToString(), this);
                }
            }

            if (registrationType.ProvisionAgreement != null)
            {
                this._provisionAgreementRef = RefUtil.CreateReference(this, registrationType.ProvisionAgreement);
            }

            if (registrationType.indexAttributes)
            {
                this._indexAttribtues = TertiaryBool.ParseBoolean(registrationType.indexAttributes);
            }

            if (registrationType.indexDataSet)
            {
                this._indexDataset = TertiaryBool.ParseBoolean(registrationType.indexDataSet);
            }

            if (registrationType.indexReportingPeriod)
            {
                this._indexReportingPeriod = TertiaryBool.ParseBoolean(registrationType.indexReportingPeriod);
            }

            if (registrationType.indexTimeSeries)
            {
                this.indexTimeseries = TertiaryBool.ParseBoolean(registrationType.indexTimeSeries);
            }

            this.Validate();
            if (Log.IsDebugEnabled)
            {
                Log.Debug("IRegistrationObject Built " + this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationObjectCore"/> class.
        /// </summary>
        /// <param name="registrationType">
        /// The registration type. 
        /// </param>
        /// ///
        /// <exception cref="SdmxNotImplementedException">
        /// Throws Unsupported Exception.
        /// </exception>
        public RegistrationObjectCore(Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry.RegistrationType registrationType)
            : base(
                null, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Registration), 
                null, 
                null, 
                null, 
                TertiaryBool.GetFromEnum(TertiaryBoolEnumType.False), 
                "NA", 
                null, 
                null, 
                null, 
                null, 
                TertiaryBool.GetFromEnum(TertiaryBoolEnumType.False), 
                null)
        {
            this._lastUpdated = new SdmxDateCore(DateTime.Now, TimeFormatEnumType.DateTime);
            this.indexTimeseries = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._indexDataset = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._indexAttribtues = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._indexReportingPeriod = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            Log.Debug("Building IRegistrationObject from 2.0 SDMX");
            if (registrationType.ValidFrom != null)
            {
                this._validFrom = new SdmxDateCore(registrationType.ValidFrom.Value, TimeFormatEnumType.DateTime);
            }

            if (registrationType.LastUpdated != null)
            {
                this._lastUpdated = new SdmxDateCore(registrationType.LastUpdated.Value, TimeFormatEnumType.DateTime);
            }

            if (registrationType.ValidTo != null)
            {
                this._validTo = new SdmxDateCore(registrationType.ValidTo, TimeFormatEnumType.DateTime);
            }

            if (registrationType.Datasource != null)
            {
                this._dataSource = new DataSourceCore(registrationType.Datasource, this);
            }

            if (registrationType.ProvisionAgreementRef != null)
            {
                ProvisionAgreementRefType provRef = registrationType.ProvisionAgreementRef;
                if (provRef.URN != null)
                {
                    this._provisionAgreementRef = new CrossReferenceImpl(
                        this, registrationType.ProvisionAgreementRef.URN);
                }
                else if (ObjectUtil.ValidOneString(provRef.DataflowID, provRef.OrganisationSchemeID))
                {
                    throw new SdmxNotImplementedException(
                        "Registrations submitted in version 2.0 of SDMX must reference a "
                        +
                        "Provision agreement by URN.  Note : The 2.1 URN syntax for provision agreements must be used; the 2.0 URN syntax for provision agreements is not longer supported");
                }
            }

            this.Validate();
            if (Log.IsDebugEnabled)
            {
                Log.Debug("IRegistrationObject Built " + this.Urn);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationObjectCore"/> class.
        /// </summary>
        /// <param name="provRef">
        /// The prov ref. 
        /// </param>
        /// <param name="datasourceType">
        /// The datasource type. 
        /// </param>
        public RegistrationObjectCore(ProvisionAgreementRefType provRef, DatasourceType datasourceType)
            : base(
                null, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Registration), 
                null, 
                null, 
                null, 
                TertiaryBool.GetFromEnum(TertiaryBoolEnumType.False), 
                "NA", 
                null, 
                null, 
                null, 
                null, 
                TertiaryBool.GetFromEnum(TertiaryBoolEnumType.False), 
                null)
        {
            this._lastUpdated = new SdmxDateCore(DateTime.Now, TimeFormatEnumType.DateTime);
            this.indexTimeseries = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._indexDataset = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._indexAttribtues = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._indexReportingPeriod = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            Log.Debug("Building IRegistrationObject from Provision and Datasource");
            if (datasourceType != null)
            {
                this._dataSource = new DataSourceCore(datasourceType, this);
            }

            if (provRef != null)
            {
                this._provisionAgreementRef = new CrossReferenceImpl(this, provRef.URN);
            }

            this.Validate();
            if (Log.IsDebugEnabled)
            {
                Log.Debug("IRegistrationObject Built " + this.Urn);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the cross referenced constrainables.
        /// </summary>
        public virtual IList<ICrossReference> CrossReferencedConstrainables
        {
            get
            {
                IList<ICrossReference> returnList = new List<ICrossReference>();
                returnList.Add(this.ProvisionAgreementRef);
                return returnList;
            }
        }

        /// <summary>
        ///   Gets the data source.
        /// </summary>
        public virtual IDataSource DataSource
        {
            get
            {
                return this._dataSource;
            }
        }

        /// <summary>
        /// Gets the Urn
        /// </summary>
        public override sealed Uri Urn
        {
            get
            {
                return base.Urn;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATE                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///   Gets the index attribtues.
        /// </summary>
        public virtual TertiaryBool IndexAttribtues
        {
            get
            {
                return this._indexAttribtues;
            }
        }

        /// <summary>
        ///   Gets the index dataset.
        /// </summary>
        public virtual TertiaryBool IndexDataset
        {
            get
            {
                return this._indexDataset;
            }
        }

        /// <summary>
        ///   Gets the index reporting period.
        /// </summary>
        public virtual TertiaryBool IndexReportingPeriod
        {
            get
            {
                return this._indexReportingPeriod;
            }
        }

        /// <summary>
        ///   Gets the index time series.
        /// </summary>
        public virtual TertiaryBool IndexTimeseries
        {
            get
            {
                return this.indexTimeseries;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether indexed.
        /// </summary>
        public virtual bool Indexed
        {
            get
            {
                return this._indexAttribtues.IsTrue || this._indexDataset.IsTrue || this._indexReportingPeriod.IsTrue
                       || this.indexTimeseries.IsTrue;
            }
        }

        /// <summary>
        ///   Gets the last updated.
        /// </summary>
        public virtual ISdmxDate LastUpdated
        {
            get
            {
                return this._lastUpdated;
            }
        }

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public override IRegistrationMutableObject MutableInstance
        {
            get
            {
                return new RegistrationMutableCore(this);
            }
        }

        /// <summary>
        ///   Gets the provision agreement ref.
        /// </summary>
        public virtual ICrossReference ProvisionAgreementRef
        {
            get
            {
                return this._provisionAgreementRef;
            }
        }

        /// <summary>
        ///   Gets the valid from.
        /// </summary>
        public virtual ISdmxDate ValidFrom
        {
            get
            {
                return this._validFrom;
            }
        }

        /// <summary>
        ///   Gets the valid to.
        /// </summary>
        public virtual ISdmxDate ValidTo
        {
            get
            {
                return this._validTo;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The dataStructureObject. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        /// ///
        /// <exception cref="SdmxNotImplementedException">
        /// Throws Unsupported Exception.
        /// </exception>
        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            throw new SdmxNotImplementedException("deepEquals on registration");
        }

        /// <summary>
        /// The get stub.
        /// </summary>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableObject"/> . 
        /// </returns>
        public override IMaintainableObject GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return this;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The validate agency id.
        /// </summary>
        protected internal override void ValidateAgencyId()
        {
            // Do nothing yet, not yet fully built
        }

        /// <summary>
        /// The validate id.
        /// </summary>
        /// <param name="startWithIntAllowed">
        /// The start with int allowed. 
        /// </param>
        protected internal override void ValidateId(bool startWithIntAllowed)
        {
            // Do nothing yet, not yet fully built
        }

        /// <summary>
        ///   The generate id.
        /// </summary>
        /// <returns> The <see cref="string" /> . </returns>
        private string GenerateId()
        {
            return RandomUtil.GenerateRandomString();
        }

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (this._provisionAgreementRef == null)
            {
                throw new SdmxSemmanticException("Registration must reference a provision agreement");
            }

            if (this._provisionAgreementRef.TargetReference
                != SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProvisionAgreement))
            {
                throw new SdmxSemmanticException(
                    "Registration must reference a provision agreement, actual referenced structre supplied was "
                    + this._provisionAgreementRef.TargetReference.GetType());
            }

            if (this._dataSource == null)
            {
                throw new SdmxSemmanticException("Registration must have a datasource");
            }

            this.AgencyId = this._provisionAgreementRef.MaintainableReference.AgencyId;
            if (string.IsNullOrWhiteSpace(this.Id))
            {
                // If an Id was not provided then generate one
                this.Id = this.GenerateId();
            }

            if(this._lastUpdated == null) 
            {
			  this._lastUpdated = new SdmxDateCore(new DateTime(), TimeFormatEnumType.DateTime);	
		    }

            base.ValidateId(true);
            base.ValidateAgencyId();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////COMPOSITES		                     //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The get composites internal.
        /// </summary>
        protected override ISet<ISdmxObject> GetCompositesInternal() 
        {
    	    ISet<ISdmxObject> composites = base.GetCompositesInternal();
            base.AddToCompositeSet(this._dataSource, composites);
            return composites;
        }

        #endregion
    }
}