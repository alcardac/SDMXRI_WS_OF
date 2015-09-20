// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationUnitBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The organisation unit core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The organisation unit core.
    /// </summary>
    [Serializable]
    public class OrganisationUnitCore : OrganisationCore<IOrganisationUnit>, IOrganisationUnit
    {
        /// <summary>
        /// The _parent identifier
        /// </summary>
        private readonly string _parentId;

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationUnitCore"/> class.
        /// </summary>
        /// <param name="organisationUnitMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public OrganisationUnitCore(IOrganisationUnitMutableObject organisationUnitMutableObject, IOrganisationUnitSchemeObject parent)
            : base(organisationUnitMutableObject, parent)
        {
            this._parentId = !string.IsNullOrWhiteSpace(organisationUnitMutableObject.ParentUnit) ? organisationUnitMutableObject.ParentUnit : null;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationUnitCore"/> class.
        /// </summary>
        /// <param name="type">
        /// The type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public OrganisationUnitCore(OrganisationUnitType type, IOrganisationUnitSchemeObject parent)
            : base(type, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnit), parent)
        {
            var parentItem = type.GetTypedParent<LocalOrganisationUnitReferenceType>();
            this._parentId = (parentItem != null) ? parentItem.GetTypedRef<LocalOrganisationUnitRefType>().id : null;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP VALIDATION                         //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
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
            if (sdmxObject == null)
            {
                return false;
            }
            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IOrganisationUnit)sdmxObject;
                if (!ObjectUtil.Equivalent(_parentId, that.ParentUnit))
                {
                    return false;
                }
                return this.DeepEqualsInternal((IOrganisationUnit)sdmxObject, includeFinalProperties);
            }

            return false;
        }

        #endregion


        /// <summary>
        /// Gets true if getParentUnit returns a not null object
        /// </summary>
        public bool HasParentUnit
        {
            get
            {
                return ParentUnit != null;
            }
        }

        /// <summary>
        /// Gets the parent organisation unit
        /// </summary>
        public string ParentUnit
        {
            get
            {
                return this._parentId;
            }
        }
    }
}