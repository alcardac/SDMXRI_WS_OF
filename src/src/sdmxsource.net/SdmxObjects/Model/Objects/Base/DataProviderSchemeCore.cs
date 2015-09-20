// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataProviderSchemeBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data provider scheme object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Util;

    using OrganisationSchemeType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.OrganisationSchemeType;
    using OrganisationType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.OrganisationType;

    /// <summary>
    ///   The data provider scheme object core.
    /// </summary>
    [Serializable]
    public class DataProviderSchemeCore :
        OrganisationSchemeCore<IDataProvider, IDataProviderScheme, IDataProviderSchemeMutableObject, IDataProviderMutableObject>, 
        IDataProviderScheme
    {
        #region Static Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private static readonly ILog LOG = LogManager.GetLogger(typeof(DataProviderSchemeCore));

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderSchemeCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        public DataProviderSchemeCore(IDataProviderSchemeMutableObject itemMutableObject)
            : base(itemMutableObject)
        {
            LOG.Debug("Building IDataProviderScheme from Mutable Object");
            if (itemMutableObject.Items != null)
            {
                foreach (IDataProviderMutableObject dataProviderMutableObject in itemMutableObject.Items)
                {
                    this.AddInternalItem(new DataProviderCore(dataProviderMutableObject, this));
                }
            }

            if (LOG.IsDebugEnabled)
            {
                LOG.Debug("IDataProviderScheme Built " + base.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderSchemeCore"/> class.
        /// </summary>
        /// <param name="type">
        /// The type. 
        /// </param>
        public DataProviderSchemeCore(DataProviderSchemeType type)
            : base(type, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProviderScheme))
        {
            LOG.Debug("Building IDataProviderScheme from 2.1 SDMX");
            if (ObjectUtil.ValidCollection(type.Organisation))
            {
                foreach (DataProvider currentDataProvider in type.Organisation)
                {
                    this.AddInternalItem(new DataProviderCore(currentDataProvider.Content, this));
                }
            }

            if (LOG.IsDebugEnabled)
            {
                LOG.Debug("IDataProviderScheme Built " + this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderSchemeCore"/> class.
        /// </summary>
        /// <param name="organisationScheme">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        public DataProviderSchemeCore(OrganisationSchemeType organisationScheme)
            : base(
                organisationScheme, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataProviderScheme), 
                organisationScheme.validTo, 
                organisationScheme.validFrom, 
                DataProviderScheme.FixedVersion, 
                TertiaryBool.GetFromEnum(TertiaryBoolEnumType.False), 
                organisationScheme.agencyID, 
                DataProviderScheme.FixedId, 
                organisationScheme.uri, 
                organisationScheme.Name, 
                organisationScheme.Description, 
                CreateTertiary(organisationScheme.isExternalReference), 
                organisationScheme.Annotations)
        {
            LOG.Debug("Building IDataProviderScheme from 2.0 SDMX");
            try
            {
                if (organisationScheme.DataProviders != null)
                {
                    foreach (DataProvidersType dataProvidersType in organisationScheme.DataProviders)
                    {
                        foreach (OrganisationType dataProvider in dataProvidersType.DataProvider)
                        {
                            this.AddInternalItem(new DataProviderCore(dataProvider, this));
                        }
                    }
                }
            }
            catch(SdmxSemmanticException ex) {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
		    }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
		    }

            if (LOG.IsDebugEnabled)
            {
                LOG.Debug("IDataProviderScheme Built " + this.Urn);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderSchemeCore"/> class.
        /// </summary>
        /// <param name="agencyScheme">
        /// The agencyScheme. 
        /// </param>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        private DataProviderSchemeCore(IDataProviderScheme agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
            LOG.Debug("Stub IDataProviderScheme Built");
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP VALIDATION                         //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the id.
        /// </summary>
        public override string Id
        {
            get
            {
                return DataProviderScheme.FixedId;
            }
        }

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public override IDataProviderSchemeMutableObject MutableInstance
        {
            get
            {
                return new DataProviderSchemeMutableCore(this);
            }
        }

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
        ///   Gets the version.
        /// </summary>
        public override string Version
        {
            get
            {
                return DataProviderScheme.FixedVersion;
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
            if(sdmxObject == null) return false;

            if (sdmxObject.StructureType == this.StructureType)
            {
                return this.DeepEqualsInternal((IDataProviderScheme)sdmxObject, includeFinalProperties);
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
        /// The <see cref="IDataProviderScheme"/> . 
        /// </returns>
        public override IDataProviderScheme GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return new DataProviderSchemeCore(this, actualLocation, isServiceUrl);
        }

        #endregion
    }
}