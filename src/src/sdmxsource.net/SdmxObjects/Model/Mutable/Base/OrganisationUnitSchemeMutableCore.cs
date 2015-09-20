// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationUnitSchemeMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The organisation unit scheme mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    /// <summary>
    ///   The organisation unit scheme mutable core.
    /// </summary>
    [Serializable]
    public class OrganisationUnitSchemeMutableCore :
        ItemSchemeMutableCore<IOrganisationUnitMutableObject, IOrganisationUnit, IOrganisationUnitSchemeObject>, 
        IOrganisationUnitSchemeMutableObject
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationUnitSchemeMutableCore"/> class.
        /// </summary>
        /// <param name="organisationUnitSchemeObject">
        /// The organisationUnitSchemeObject. 
        /// </param>
        public OrganisationUnitSchemeMutableCore(IOrganisationUnitSchemeObject organisationUnitSchemeObject)
            : base(organisationUnitSchemeObject)
        {
            foreach (IOrganisationUnit organisationUnit in organisationUnitSchemeObject.Items)
            {
                this.AddItem(new OrganisationUnitMutableCore(organisationUnit));

                // organisationUnits.add(new OrganisationUnitMutableCore(organisationUnit));
            }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="OrganisationUnitSchemeMutableCore" /> class.
        /// </summary>
        public OrganisationUnitSchemeMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnitScheme))
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override IOrganisationUnitSchemeObject ImmutableInstance
        {
            get
            {
                return new OrganisationUnitSchemeObjectCore(this);
            }
        }

        #endregion

        #region Overrides of ItemSchemeMutableCore<IOrganisationUnitMutableObject,IOrganisationUnit,IOrganisationUnitSchemeObject>

        public override IOrganisationUnitMutableObject CreateItem(string id, string name)
        {
            IOrganisationUnitMutableObject orgUnit = new OrganisationUnitMutableCore();
            orgUnit.Id = id;
            orgUnit.AddName("en", name);
            AddItem(orgUnit);
            return orgUnit;
        }

        #endregion

    }
}