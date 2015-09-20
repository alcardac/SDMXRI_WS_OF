// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AgencyCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The agency core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;

    using V21 = Org.Sdmx.Resources.SdmxMl.Schemas.V21;
    using V20 = Org.Sdmx.Resources.SdmxMl.Schemas.V20;
    using V10 = Org.Sdmx.Resources.SdmxMl.Schemas.V10;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    using OrganisationType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.OrganisationType;

    /// <summary>
    ///   The agency core.
    /// </summary>
    [Serializable]
    public class AgencyCore : OrganisationCore<IAgency>, IAgency
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AgencyCore"/> class.
        /// </summary>
        /// <param name="agencyMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public AgencyCore(IAgencyMutableObject agencyMutableObject, IAgencyScheme parent)
            : base(agencyMutableObject, parent)
        {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="AgencyCore"/> class.
        /// </summary>
        /// <param name="agency">
        /// The agency. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public AgencyCore(V21.Structure.AgencyType agency, IAgencyScheme parent)
            : base(agency, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Agency), parent)
        {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="AgencyCore"/> class.
        /// </summary>
        /// <param name="organisationType">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public AgencyCore(V20.structure.OrganisationType organisationType, IAgencyScheme parent)
            : base(
                organisationType, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Agency), 
                organisationType.CollectorContact, 
                organisationType.id, 
                organisationType.uri, 
                organisationType.Name, 
                organisationType.Description, 
                organisationType.Annotations,
                parent)
        {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="AgencyCore"/> class.
        /// </summary>
        /// <param name="agencyType">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public AgencyCore(V10.structure.AgencyType agencyType, IAgencyScheme parent)
            : base(
                agencyType, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Agency), 
                agencyType.CollectorContact, 
                agencyType.id, 
                agencyType.uri, 
                agencyType.Name,
                null, 
                null, 
                parent)
        {
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the full id.
        /// </summary>
        public virtual string FullId
        {
            get
            {
                var parent = (IAgencyScheme)this.MaintainableParent;
                if (parent.DefaultScheme)
                {
                    return this.Id;
                }

                return parent.AgencyId + "." + this.Id;
            }
        }

        public IAgencyScheme GetMaintainableParent
        {
            get
            {
                return (IAgencyScheme)base.MaintainableParent;
            }
        }

        /// <summary>
        ///   Gets the urn.
        /// </summary>
        public override Uri Urn
        {
            get
            {
                return new Uri(this.StructureType.UrnPrefix + this.FullId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        new public IAgencyScheme MaintainableParent
        { 
            get
            {
                return (IAgencyScheme)base.MaintainableParent;
            } 
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if (sdmxObject != null && sdmxObject.StructureType.EnumType == SdmxStructureEnumType.Agency)
            {
                var that = (IAgency)sdmxObject;
                if (!ObjectUtil.Equivalent(this.FullId, that.FullId))
                {
                    return false;
                }

                return base.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        #region Methods

        /// <summary>
        /// The validate id.
        /// </summary>
        /// <param name="startWithIntAllowed">
        /// The start with int allowed. 
        /// </param>
        protected internal override void ValidateId(bool startWithIntAllowed)
        {
            // Not allowed to start with an integer
            base.ValidateId(false);
        }


        #endregion
    }
}