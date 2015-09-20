// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AgencySchemeBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The agency scheme object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Util;

    using Agency = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.Agency;
    using OrganisationSchemeType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.OrganisationSchemeType;
    using OrganisationType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.OrganisationType;

    /// <summary>
    ///   The agency scheme object core.
    /// </summary>
    [Serializable]
    public class AgencySchemeCore :
        OrganisationSchemeCore<IAgency, IAgencyScheme, IAgencySchemeMutableObject, IAgencyMutableObject>, 
        IAgencyScheme
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AgencySchemeCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The agencyScheme. 
        /// </param>
        public AgencySchemeCore(IAgencySchemeMutableObject itemMutableObject)
            : base(itemMutableObject)
        {
            if (itemMutableObject.Items != null)
            {
                foreach (IAgencyMutableObject item in itemMutableObject.Items)
                {
                    this.AddInternalItem(new AgencyCore(item, this));
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="AgencySchemeCore"/> class.
        /// </summary>
        /// <param name="type">
        /// The type. 
        /// </param>
        public AgencySchemeCore(AgencySchemeType type)
            : base(type, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AgencyScheme))
        {
            if (ObjectUtil.ValidCollection(type.Organisation))
            {
                foreach (Agency currentAgency in type.Organisation)
                {
                    this.AddInternalItem(new AgencyCore(currentAgency.Content, this));
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="AgencySchemeCore"/> class.
        /// </summary>
        /// <param name="organisationSchemeType">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        public AgencySchemeCore(OrganisationSchemeType organisationSchemeType)
            : base(
                organisationSchemeType, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AgencyScheme), 
                organisationSchemeType.validTo, 
                organisationSchemeType.validFrom, 
                AgencyScheme.FixedVersion, 
                TertiaryBool.GetFromEnum(TertiaryBoolEnumType.False), 
                AgencyScheme.DefaultScheme, 
                AgencyScheme.FixedId, 
                organisationSchemeType.uri, 
                organisationSchemeType.Name, 
                organisationSchemeType.Description, 
                CreateTertiary(organisationSchemeType.isExternalReference), 
                organisationSchemeType.Annotations)
        {
            try
            {
                if (organisationSchemeType.Agencies != null)
                {
                    foreach (AgenciesType agenciesType in organisationSchemeType.Agencies)
                    {
                        foreach (OrganisationType agencyType in agenciesType.Agency)
                        {
                            this.AddInternalItem(new AgencyCore(agencyType, this));
                        }
                    }
                }
            }
            catch(SdmxSemmanticException ex) {
			    throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
		    } 
            catch(Exception ex) {
			    throw new SdmxException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
		    }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgencySchemeCore"/> class.
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
        private AgencySchemeCore(IAgencyScheme agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
        }

        #endregion

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
        ///   Gets a value indicating whether default scheme.
        /// </summary>
        public bool DefaultScheme
        {
            get
            {
                return this.AgencyId.Equals(AgencyScheme.DefaultScheme);
            }
        }

        /// <summary>
        ///   Gets the id.
        /// </summary>
        public override string Id
        {
            get
            {
                return AgencyScheme.FixedId;
            }
        }

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public override IAgencySchemeMutableObject MutableInstance
        {
            get
            {
                return new AgencySchemeMutableCore(this);
            }
        }

        /// <summary>
        ///   Gets the version.
        /// </summary>
        public override string Version
        {
            get
            {
                return AgencyScheme.FixedVersion;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The create default scheme.
        /// </summary>
        /// <returns> The <see cref="IAgencyScheme" /> . </returns>
        public static IAgencyScheme CreateDefaultScheme()
        {
            IAgencySchemeMutableObject mutable = new AgencySchemeMutableCore();

            mutable.AgencyId = AgencyScheme.DefaultScheme;
            mutable.Id = AgencyScheme.FixedId;
            mutable.Version = AgencyScheme.FixedVersion;
            mutable.AddName("en", "SDMX Agency Scheme");
            IAgencyMutableObject agencyMutableCore = new AgencyMutableCore();
            agencyMutableCore.AddName("en", Api.Constants.InterfaceConstant.Agency.DefaultAgency);
            agencyMutableCore.Id = Api.Constants.InterfaceConstant.Agency.DefaultAgency;
            mutable.AddItem(agencyMutableCore);
            return mutable.ImmutableInstance;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP VALIDATION                         //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

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
            if (sdmxObject !=null &&  sdmxObject.StructureType == this.StructureType)
            {
                return this.DeepEqualsInternal((IAgencyScheme)sdmxObject, includeFinalProperties);
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
        /// The <see cref="IAgencyScheme"/> . 
        /// </returns>
        public override IAgencyScheme GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return new AgencySchemeCore(this, actualLocation, isServiceUrl);
        }

        #endregion

     
    }
}