// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AgencySchemeMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The agency scheme mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    /// <summary>
    ///   The agency scheme mutable core.
    /// </summary>
    [Serializable]
    public class AgencySchemeMutableCore : ItemSchemeMutableCore<IAgencyMutableObject, IAgency, IAgencyScheme>, 
                                           IAgencySchemeMutableObject
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AgencySchemeMutableCore"/> class.
        /// </summary>
        /// <param name="agencyScheme">
        /// The agencyScheme. 
        /// </param>
        public AgencySchemeMutableCore(IAgencyScheme agencyScheme)
            : base(agencyScheme)
        {
            foreach (IAgency acy in agencyScheme.Items)
            {
                this.AddItem(new AgencyMutableCore(acy));
            }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AgencySchemeMutableCore" /> class.
        /// </summary>
        public AgencySchemeMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AgencyScheme))
        {
        }

        #endregion

        /*
         public override IAgencyScheme ImmutableInstance {
          get {
                return new AgencySchemeCore(this);
            }
        }


         IMaintainableObject IMaintainableObject.ImmutableInstance
        {
            get { return ImmutableInstance; }
        } ^^^ */
        #region Public Properties

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override IAgencyScheme ImmutableInstance
        {
            get
            {
                return new AgencySchemeCore(this);
            }
        }

     
        #endregion

        public override IAgencyMutableObject CreateItem(string id, string name)
        {
            IAgencyMutableObject acy = new AgencyMutableCore();
            acy.Id = id;
            acy.AddName("en", name);
            this.AddItem(acy);
            return acy;
        }
    }
}