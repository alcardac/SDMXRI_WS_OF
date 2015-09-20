// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataConsumerSchemeBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data consumer scheme object core.
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
    ///   The data consumer scheme object core.
    /// </summary>
    [Serializable]
    public sealed class DataConsumerSchemeCore :
        OrganisationSchemeCore<IDataConsumer, IDataConsumerScheme, IDataConsumerSchemeMutableObject, IDataConsumerMutableObject>, 
        IDataConsumerScheme
    {
        #region Static Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private static readonly ILog LOG = LogManager.GetLogger(typeof(DataConsumerSchemeCore));

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConsumerSchemeCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The agencyScheme. 
        /// </param>
        public DataConsumerSchemeCore(IDataConsumerSchemeMutableObject itemMutableObject)
            : base(itemMutableObject)
        {
            LOG.Debug("Building IDataConsumerScheme from Mutable Object");
            if (itemMutableObject.Items != null)
            {
                foreach (IDataConsumerMutableObject item in itemMutableObject.Items)
                {
                    this.AddInternalItem(new DataConsumerCore(item, this));
                }
            }

            if (LOG.IsDebugEnabled)
            {
                LOG.Debug("IDataConsumerScheme Built " + this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConsumerSchemeCore"/> class.
        /// </summary>
        /// <param name="type">
        /// The type. 
        /// </param>
        public DataConsumerSchemeCore(DataConsumerSchemeType type)
            : base(type, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumerScheme))
        {
            LOG.Debug("Building IDataConsumerScheme from 2.1");
            if (ObjectUtil.ValidCollection(type.Organisation))
            {
                foreach (DataConsumer currentDataConsumer in type.Organisation)
                {
                    this.AddInternalItem(new DataConsumerCore(currentDataConsumer.Content, this));
                }
            }

            if (LOG.IsDebugEnabled)
            {
                LOG.Debug("IDataConsumerScheme Built " + this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConsumerSchemeCore"/> class.
        /// </summary>
        /// <param name="organisationScheme">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxException"> Throws SdmxException.
        /// </exception>
        public DataConsumerSchemeCore(OrganisationSchemeType organisationScheme)
            : base(
                organisationScheme, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataConsumerScheme), 
                organisationScheme.validTo, 
                organisationScheme.validFrom, 
                DataConsumerScheme.FixedVersion, 
                TertiaryBool.GetFromEnum(TertiaryBoolEnumType.False), 
                organisationScheme.agencyID, 
                DataConsumerScheme.FixedId, 
                organisationScheme.uri, 
                organisationScheme.Name, 
                organisationScheme.Description, 
                CreateTertiary(organisationScheme.isExternalReference), 
                organisationScheme.Annotations)
        {
            LOG.Debug("Building IDataConsumerScheme from 2.0");
            try
            {
                if (organisationScheme.DataProviders != null)
                {
                    foreach (DataConsumersType dataConsumersType in organisationScheme.DataConsumers)
                    {
                        foreach (OrganisationType dataConsumer in dataConsumersType.DataConsumer)
                        {
                            this.AddInternalItem(new DataConsumerCore(dataConsumer, this));
                        }
                    }
                }
            }
            catch (SdmxException ex)
            {
                throw new SdmxException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            if (LOG.IsDebugEnabled)
            {
                LOG.Debug("IDataConsumerScheme Built " + this.Urn);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConsumerSchemeCore"/> class.
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
        private DataConsumerSchemeCore(IDataConsumerScheme agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
            LOG.Debug("Stub IDataConsumerScheme Built");
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
                return DataConsumerScheme.FixedId;
            }
        }

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public override IDataConsumerSchemeMutableObject MutableInstance
        {
            get
            {
                return new DataConsumerSchemeMutableCore(this);
            }
        }

        /// <summary>
        ///   Gets the version.
        /// </summary>
        public override string Version
        {
            get
            {
                return DataConsumerScheme.FixedVersion;
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
            if (sdmxObject.StructureType == this.StructureType)
            {
                return this.DeepEqualsInternal((IDataConsumerScheme)sdmxObject, includeFinalProperties);
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
        /// The <see cref="IDataConsumerScheme"/> . 
        /// </returns>
        public override IDataConsumerScheme GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return new DataConsumerSchemeCore(this, actualLocation, isServiceUrl);
        }

        #endregion
    }
}