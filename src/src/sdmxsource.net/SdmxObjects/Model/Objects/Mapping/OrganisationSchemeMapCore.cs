// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationSchemeMapBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The organisation scheme map core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Mapping
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    using OrganisationMap = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.OrganisationMap;
    using OrganisationMapType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.OrganisationMapType;

    /// <summary>
    ///   The organisation scheme map core.
    /// </summary>
    [Serializable]
    public class OrganisationSchemeMapCore : ItemSchemeMapCore, IOrganisationSchemeMapObject
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationSchemeMapCore"/> class.
        /// </summary>
        /// <param name="organisationSchemeMapMutableObject">
        /// The iorg. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public OrganisationSchemeMapCore(IOrganisationSchemeMapMutableObject organisationSchemeMapMutableObject, IStructureSetObject parent)
            : base(organisationSchemeMapMutableObject, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationSchemeMap), parent)
        {
            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationSchemeMapCore"/> class.
        /// </summary>
        /// <param name="organisation">
        /// The organisation. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public OrganisationSchemeMapCore(OrganisationSchemeMapType organisation, IStructureSetObject parent)
            : base(organisation, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationSchemeMap), parent)
        {
            this.SourceRef = RefUtil.CreateReference(this, organisation.Source);
            this.TargetRef = RefUtil.CreateReference(this, organisation.Target);

            // get list of code maps
            if (organisation.ItemAssociation != null)
            {
                foreach (OrganisationMap orgMap in organisation.ItemAssociation)
                {
                    IItemMap item = new ItemMapCore(orgMap.Source.GetTypedRef<LocalOrganisationRefType>().id, organisation.Target.GetTypedRef<LocalOrganisationRefType>().id, this);
                    this.AddInternalItem(item);
                }
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationSchemeMapCore"/> class.
        /// </summary>
        /// <param name="orgObject">
        /// The org attachmentConstraint. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public OrganisationSchemeMapCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.OrganisationSchemeMapType orgObject, 
            IStructureSetObject parent)
            : base(
                orgObject, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationSchemeMap), 
                orgObject.id, 
                null, 
                orgObject.Name, 
                orgObject.Description, 
                orgObject.Annotations, 
                parent)
        {
            if (orgObject.OrganisationSchemeRef != null)
            {
                if (orgObject.OrganisationSchemeRef.URN != null)
                {
                    this.SourceRef = new CrossReferenceImpl(this, orgObject.OrganisationSchemeRef.URN);
                }
                else
                {
                    this.SourceRef = new CrossReferenceImpl(
                        this, 
                        orgObject.OrganisationSchemeRef.AgencyID, 
                        orgObject.OrganisationSchemeRef.OrganisationSchemeID, 
                        orgObject.OrganisationSchemeRef.Version, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnitScheme));
                }
            }

            if (orgObject.TargetOrganisationSchemeRef != null)
            {
                if (orgObject.TargetOrganisationSchemeRef.URN != null)
                {
                    this.SourceRef = new CrossReferenceImpl(this, orgObject.TargetOrganisationSchemeRef.URN);
                }
                else
                {
                    this.TargetRef = new CrossReferenceImpl(
                        this, 
                        orgObject.TargetOrganisationSchemeRef.AgencyID, 
                        orgObject.TargetOrganisationSchemeRef.OrganisationSchemeID, 
                        orgObject.TargetOrganisationSchemeRef.Version, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnitScheme));
                }
            }

            // get list of code maps
            if (orgObject.OrganisationMap != null)
            {
                foreach (OrganisationMapType orgMap in orgObject.OrganisationMap)
                {
                    IItemMap item = new ItemMapCore(
                        orgMap.organisationAlias, orgMap.OrganisationID, orgMap.TargetOrganisationID, this);
                    this.AddInternalItem(item);
                }
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        protected internal void Validate()
        {
            if (this.SourceRef == null)
            {
                throw new SdmxSemmanticException(
                    ExceptionCode.ObjectMissingRequiredElement, this.StructureType, "OrganisationSchemeRef");
            }

            if (this.TargetRef == null)
            {
                throw new SdmxSemmanticException(
                    ExceptionCode.ObjectMissingRequiredElement, this.StructureType, "TargetOrganisationSchemeRef");
            }

            if (this.Items == null)
            {
                throw new SdmxSemmanticException(
                    ExceptionCode.ObjectMissingRequiredElement, this.StructureType, "OrganisationMap");
            }
        }

        #endregion
    }
}