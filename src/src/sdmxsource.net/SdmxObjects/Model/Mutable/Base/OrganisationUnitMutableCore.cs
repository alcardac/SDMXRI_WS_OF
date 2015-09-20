// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationUnitMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The organisation unit mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///   The organisation unit mutable core.
    /// </summary>
    [Serializable]
    public class OrganisationUnitMutableCore : OrganisationMutableObjectCore, IOrganisationUnitMutableObject
    {
        /// <summary>
        /// The _parent unit
        /// </summary>
        private string _parentUnit;

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationUnitMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public OrganisationUnitMutableCore(IOrganisationUnit objTarget)
            : base(objTarget)
        {
            this._parentUnit = objTarget.ParentUnit;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="OrganisationUnitMutableCore" /> class.
        /// </summary>
        public OrganisationUnitMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnit))
        {
        }

        #endregion

        #region Implementation of IOrganisationUnitMutableObject

        /// <summary>
        /// Gets or sets the id of the parent organisation unit, or null if there is none
        /// </summary>
        public string ParentUnit
        {
            get
            {
                return _parentUnit;
            }

            set
            {
                this._parentUnit = value;
            }
        }

        #endregion
    }
}