// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProvisionAgreementBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The provision agreement object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    using DataflowRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry.DataflowRefType;
    using DataProviderRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry.DataProviderRefType;
    using DataProviderScheme = Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant.DataProviderScheme;
    using MetadataflowRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry.MetadataflowRefType;

    /// <summary>
    ///   The provision agreement object core.
    /// </summary>
    [Serializable]
    public class ProvisionAgreementObjectCore :
        MaintainableObjectCore<IProvisionAgreementObject, IProvisionAgreementMutableObject>, 
        IProvisionAgreementObject
    {
        #region Static Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private static readonly ILog LOG = LogManager.GetLogger(typeof(ProvisionAgreementObjectCore));

        #endregion

        #region Fields

        /// <summary>
        ///   The _dataprovider ref.
        /// </summary>
        private readonly ICrossReference _dataproviderRef;

        /// <summary>
        ///   The _structure useage.
        /// </summary>
        private readonly ICrossReference _structureUseage;

        #endregion

      
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvisionAgreementObjectCore"/> class.
        /// </summary>
        /// <param name="provisionAgreementMutable">
        /// The provision agreement mutable. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws CategorisationStructure exception.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ProvisionAgreementObjectCore(IProvisionAgreementMutableObject provisionAgreementMutable)
            : base(provisionAgreementMutable)
        {
            LOG.Debug("Building IProvisionAgreementObject from Mutable Object");
            try
            {
                if (provisionAgreementMutable.StructureUsage != null)
                {
                    this._structureUseage = new CrossReferenceImpl(this, provisionAgreementMutable.StructureUsage);
                }

                if (provisionAgreementMutable.DataproviderRef != null)
                {
                    this._dataproviderRef = new CrossReferenceImpl(this, provisionAgreementMutable.DataproviderRef);
                }
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException(th, ExceptionCode.ObjectStructureConstructionError, this);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }

            if (LOG.IsDebugEnabled)
            {
                LOG.Debug("IProvisionAgreementObject Built " + this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvisionAgreementObjectCore"/> class.
        /// </summary>
        /// <param name="provisionAgreementType">
        /// The provision agreement type. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ProvisionAgreementObjectCore(ProvisionAgreementType provisionAgreementType)
            : base(provisionAgreementType, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProvisionAgreement))
        {
            LOG.Debug("Building IProvisionAgreementObject from 2.1 SDMX");
            try
            {
                if (provisionAgreementType.StructureUsage != null)
                {
                    StructureUsageReferenceType structureUsage = provisionAgreementType.StructureUsage;
                    this._structureUseage = RefUtil.CreateReference(this, structureUsage);
                }

                if (provisionAgreementType.DataProvider != null)
                {
                    DataProviderReferenceType dataProvider = provisionAgreementType.DataProvider;
                    this._dataproviderRef = RefUtil.CreateReference(this, dataProvider);
                }
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException(th, ExceptionCode.ObjectStructureConstructionError, this);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }

            if (LOG.IsDebugEnabled)
            {
                LOG.Debug("IProvisionAgreementObject Built " + this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvisionAgreementObjectCore"/> class.
        /// </summary>
        /// <param name="provisionAgreementType">
        /// The provision agreement type. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ProvisionAgreementObjectCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry.ProvisionAgreementType provisionAgreementType)
            : base(
                provisionAgreementType, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProvisionAgreement), 
                provisionAgreementType.validTo, 
                provisionAgreementType.validFrom, 
                null, 
                null, 
                null, 
                provisionAgreementType.id, 
                provisionAgreementType.uri, 
                provisionAgreementType.Name, 
                provisionAgreementType.Description, 
                null, 
                provisionAgreementType.Annotations)
        {
            LOG.Debug("Building IProvisionAgreementObject from 2.0 SDMX");
            try
            {
                if (provisionAgreementType.DataflowRef != null)
                {
                    DataflowRefType dataflowRef = provisionAgreementType.DataflowRef;
                    if (dataflowRef.URN != null)
                    {
                        this._structureUseage = new CrossReferenceImpl(this, dataflowRef.URN);
                    }
                    else
                    {
                        this._structureUseage = new CrossReferenceImpl(
                            this, 
                            dataflowRef.AgencyID, 
                            dataflowRef.DataflowID, 
                            dataflowRef.Version, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow));
                    }

                }
                else if (provisionAgreementType.MetadataflowRef != null)
                {
                    MetadataflowRefType mdfRef = provisionAgreementType.MetadataflowRef;
                    if (mdfRef.URN != null)
                    {
                        this._structureUseage = new CrossReferenceImpl(this, mdfRef.URN);
                    }
                    else
                    {
                        this._structureUseage = new CrossReferenceImpl(
                            this, 
                            mdfRef.AgencyID, 
                            mdfRef.MetadataflowID, 
                            mdfRef.Version, 
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow));
                    }

                }

                if (provisionAgreementType.DataProviderRef != null)
                {
                    DataProviderRefType dataProviderRef = provisionAgreementType.DataProviderRef;
                    if (dataProviderRef.URN != null)
                    {
                        this._dataproviderRef = new CrossReferenceImpl(this, dataProviderRef.URN);
                    }
                    else
                    {
                        this._dataproviderRef = new CrossReferenceImpl(
                            this, 
                            dataProviderRef.OrganisationSchemeAgencyID, 
                            dataProviderRef.OrganisationSchemeID, 
                            dataProviderRef.Version, 
                            SdmxStructureEnumType.DataProvider, 
                            dataProviderRef.DataProviderID);
                    }
                }
                //Set Agency ID, Id and Name
                base.AgencyId = _structureUseage.MaintainableReference.AgencyId;

                if (this.OriginalId== null)
                {
                    this.OriginalId = Guid.NewGuid().ToString();
                }
                if (base.name == null)
                {
                    base.name = new List<ITextTypeWrapper>(); 
                    base.name.Add(new TextTypeWrapperImpl("en", _dataproviderRef.ChildReference.Id + " provising for " + _structureUseage.MaintainableId, this));
                }
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException(th, ExceptionCode.ObjectStructureConstructionError, this);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }

            if (LOG.IsDebugEnabled)
            {
                LOG.Debug("IProvisionAgreementObject Built " + this.Urn);
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        /// Gets the Urn
        /// </summary>
        public sealed override Uri Urn
        {
            get
            {
                return base.Urn;
            }
        }

        /// <summary>
        ///   Gets the cross referenced constrainables.
        /// </summary>
        public virtual IList<ICrossReference> CrossReferencedConstrainables
        {
            get
            {
                IList<ICrossReference> returnList = new List<ICrossReference>();
                returnList.Add(this.StructureUseage);
                returnList.Add(this.DataproviderRef);
                return returnList;
            }
        }

        /// <summary>
        ///   Gets the dataprovider ref.
        /// </summary>
        public virtual ICrossReference DataproviderRef
        {
            get
            {
                return this._dataproviderRef;
            }
        }

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public override IProvisionAgreementMutableObject MutableInstance
        {
            get
            {
                return new ProvisionAgreementMutableCore(this);
            }
        }

        /// <summary>
        ///   Gets the structure useage.
        /// </summary>
        public virtual ICrossReference StructureUseage
        {
            get
            {
                return this._structureUseage;
            }
        }

        /// <summary>
        /// Gets or sets the agency Id
        /// </summary>
        public sealed override string AgencyId
        {
            get
            {
                return base.AgencyId;
            }

            set
            {
                base.AgencyId = value;
            }
        }

        #endregion

        #region Public Methods and Operators

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
                var that = (IProvisionAgreementObject)sdmxObject;
                if (!this.Equivalent(this._structureUseage, that.StructureUseage))
                {
                    return false;
                }

                if (!this.Equivalent(this._dataproviderRef, that.DataproviderRef))
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
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
        /// The <see cref="IProvisionAgreementObject"/> . 
        /// </returns>
        public override IProvisionAgreementObject GetStub(Uri actualLocation, bool isServiceUrl)
        {
           if(actualLocation == null)
           {
	         return this;
	       }
		   IProvisionAgreementMutableObject mutable = this.MutableInstance;
           mutable.ExternalReference = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.True);
		   if (isServiceUrl)
           {
			  mutable.ServiceURL= actualLocation;
		   } 
           else
           {
		      mutable.StructureURL = actualLocation;
		   }
		   return mutable.ImmutableInstance; 
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
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (this._dataproviderRef == null)
            {
                throw new SdmxSemmanticException("Provision Agreement missing reference to a data provider");
            }

            if (
                !this._dataproviderRef.MaintainableReference.Version.Equals(
                    DataProviderScheme.FixedVersion))
            {
                throw new SdmxSemmanticException(
                    "Version 2.0 Data Provider Scheme is no longer supported.  Data Provider Scheme has a fixed version of 1.0 in SDMX 2.1");
            }

            if (this._structureUseage == null)
            {
                throw new SdmxSemmanticException("Provision Agreement missing reference to a data/metadata flows");
            }

            base.ValidateAgencyId();
            base.ValidateIdentifiableAttributes();
            base.ValidateNameableAttributes();
        }

        protected  override void ValidateNameableAttributes()
        {
            //Do nothing yet, not yet fully built
        }

        protected internal override  void ValidateIdentifiableAttributes()
        {
            //Do nothing yet, not yet fully built
        }
        #endregion
    }
}