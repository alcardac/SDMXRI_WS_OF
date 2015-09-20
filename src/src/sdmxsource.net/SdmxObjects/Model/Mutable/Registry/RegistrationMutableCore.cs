// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The registration mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry;

    /// <summary>
    ///   The registration mutable core.
    /// </summary>
    [Serializable]
    public class RegistrationMutableCore : MaintainableMutableCore<IRegistrationObject>, IRegistrationMutableObject
    {
        #region Fields

        /// <summary>
        ///   The _datasource.
        /// </summary>
        private IDataSourceMutableObject _datasource;

        /// <summary>
        ///   The _index attribtues.
        /// </summary>
        private TertiaryBool _indexAttributes;

        /// <summary>
        ///   The _index dataset.
        /// </summary>
        private TertiaryBool _indexDataset;

        /// <summary>
        ///   The _index reporting period.
        /// </summary>
        private TertiaryBool _indexReportingPeriod;

        /// <summary>
        ///   The _index time series.
        /// </summary>
        private TertiaryBool indexTimeseries;

        /// <summary>
        ///   The _last updated.
        /// </summary>
        private DateTime? _lastUpdated;

        /// <summary>
        ///   The _provision agreement ref.
        /// </summary>
        private IStructureReference _provisionAgreementRef;

        /// <summary>
        ///   The _valid from.
        /// </summary>
        private DateTime? _validFrom;

        /// <summary>
        ///   The _valid to.
        /// </summary>
        private DateTime? _validTo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="RegistrationMutableCore" /> class.
        /// </summary>
        public RegistrationMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Registration))
        {
            this.indexTimeseries = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._indexDataset = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._indexAttributes = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._indexReportingPeriod = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationMutableCore"/> class.
        /// </summary>
        /// <param name="registration">
        /// The registration. 
        /// </param>
        public RegistrationMutableCore(IRegistrationObject registration)
            : base(registration)
        {
            this.indexTimeseries = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._indexDataset = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._indexAttributes = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this._indexReportingPeriod = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            if (registration.DataSource != null)
            {
                this._datasource = new DataSourceMutableCore(registration.DataSource);
            }

            if (registration.LastUpdated != null)
            {
                this._lastUpdated = registration.LastUpdated.Date;
            }

            if (registration.ValidFrom != null)
            {
                this._validFrom = registration.ValidFrom.Date;
            }

            if (registration.ValidTo != null)
            {
                this._validTo = registration.ValidTo.Date;
            }

            if (registration.ProvisionAgreementRef != null)
            {
                this._provisionAgreementRef = registration.ProvisionAgreementRef.CreateMutableInstance();
            }

            this.indexTimeseries = registration.IndexTimeseries;
            this._indexDataset = registration.IndexDataset;
            this._indexAttributes = registration.IndexAttribtues;
            this._indexReportingPeriod = registration.IndexReportingPeriod;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the data source.
        /// </summary>
        public virtual IDataSourceMutableObject DataSource
        {
            get
            {
                return this._datasource;
            }

            set
            {
                this._datasource = value;
            }
        }

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override IRegistrationObject ImmutableInstance
        {
            get
            {
                return new RegistrationObjectCore(this);
            }
        }

        /// <summary>
        ///   Gets or sets the index attribtues.
        /// </summary>
        public virtual TertiaryBool IndexAttributes
        {
            get
            {
                return this._indexAttributes;
            }

            set
            {
                this._indexAttributes = value;
            }
        }

        /// <summary>
        ///   Gets or sets the index dataset.
        /// </summary>
        public virtual TertiaryBool IndexDataset
        {
            get
            {
                return this._indexDataset;
            }

            set
            {
                this._indexDataset = value;
            }
        }

        /// <summary>
        ///   Gets or sets the index reporting period.
        /// </summary>
        public virtual TertiaryBool IndexReportingPeriod
        {
            get
            {
                return this._indexReportingPeriod;
            }

            set
            {
                this._indexReportingPeriod = value;
            }
        }

        /// <summary>
        ///   Gets or sets the index time series.
        /// </summary>
        public virtual TertiaryBool IndexTimeseries
        {
            get
            {
                return this.indexTimeseries;
            }

            set
            {
                this.indexTimeseries = value;
            }
        }

        /// <summary>
        ///   Gets or sets the last updated.
        /// </summary>
        public virtual DateTime? LastUpdated
        {
            get
            {
                return this._lastUpdated;
            }

            set
            {
                this._lastUpdated = value;
            }
        }

        /// <summary>
        ///   Gets or sets the provision agreement ref.
        /// </summary>
        public virtual IStructureReference ProvisionAgreementRef
        {
            get
            {
                return this._provisionAgreementRef;
            }

            set
            {
                this._provisionAgreementRef = value;
            }
        }

        /// <summary>
        ///   Gets or sets the valid from.
        /// </summary>
        public virtual DateTime? ValidFrom
        {
            get
            {
                return this._validFrom;
            }

            set
            {
                this._validFrom = value;
            }
        }

        /// <summary>
        ///   Gets or sets the valid to.
        /// </summary>
        public virtual DateTime? ValidTo
        {
            get
            {
                return this._validTo;
            }

            set
            {
                this._validTo = value;
            }
        }

        #endregion
    }
}