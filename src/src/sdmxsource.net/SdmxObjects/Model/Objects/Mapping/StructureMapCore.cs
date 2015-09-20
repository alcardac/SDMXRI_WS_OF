// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureMapBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure map core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Mapping
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    using ComponentMapType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.ComponentMapType;
    using StructureMapType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.StructureMapType;

    /// <summary>
    ///   The structure map core.
    /// </summary>
    [Serializable]
    public class StructureMapCore : SchemeMapCore, IStructureMapObject
    {
        #region Fields

        /// <summary>
        ///   The components.
        /// </summary>
        private readonly IList<IComponentMapObject> components;

        /// <summary>
        ///   The extension.
        /// </summary>
        private readonly bool extension;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapCore"/> class.
        /// </summary>
        /// <param name="structMapType">
        /// The struct map type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public StructureMapCore(IStructureMapMutableObject structMapType, IStructureSetObject parent)
            : base(structMapType, parent)
        {
            this.components = new List<IComponentMapObject>();
            this.extension = structMapType.Extension;
            if (structMapType.Components != null)
            {
                foreach (IComponentMapMutableObject mutable in structMapType.Components)
                {
                    this.components.Add(new ComponentMapCore(mutable, this));
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
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapCore"/> class.
        /// </summary>
        /// <param name="structMapType">
        /// The struct map type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public StructureMapCore(StructureMapType structMapType, IStructureSetObject parent)
            : base(structMapType, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureMap), parent)
        {
            this.components = new List<IComponentMapObject>();
            this.SourceRef = RefUtil.CreateReference(this, structMapType.Source);
            this.TargetRef = RefUtil.CreateReference(this, structMapType.Target);
            IList<ComponentMapType> componentMapType = structMapType.ComponentMapTypes;
            if (ObjectUtil.ValidCollection(componentMapType))
            {
                foreach (ComponentMapType compMap in componentMapType)
                {
                    this.components.Add(new ComponentMapCore(compMap, this));
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
        /// Initializes a new instance of the <see cref="StructureMapCore"/> class.
        /// </summary>
        /// <param name="structMapType">
        /// The struct map type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public StructureMapCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.StructureMapType structMapType, IStructureSetObject parent)
            : base(
                structMapType, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureMap), 
                structMapType.id, 
                null, 
                structMapType.Name, 
                structMapType.Description, 
                structMapType.Annotations, 
                parent)
        {
            this.components = new List<IComponentMapObject>();

            this.extension = structMapType.isExtension.Value;
            KeyFamilyRefType keyFamilyRef = structMapType.KeyFamilyRef;

            if (keyFamilyRef != null)
            {
                if (keyFamilyRef.URN != null)
                {
                    this.SourceRef = new CrossReferenceImpl(this, keyFamilyRef.URN);
                }
                else
                {
                    string agencyId = keyFamilyRef.KeyFamilyAgencyID;
                    if (string.IsNullOrWhiteSpace(agencyId))
                    {
                        agencyId = this.MaintainableParent.AgencyId;
                    }

                    this.SourceRef = new CrossReferenceImpl(
                        this, 
                        agencyId, 
                        keyFamilyRef.KeyFamilyID, 
                        keyFamilyRef.Version, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd));
                }
            }
            else if (structMapType.MetadataStructureRef != null)
            {
                if (structMapType.MetadataStructureRef.URN != null)
                {
                    this.SourceRef = new CrossReferenceImpl(this, structMapType.MetadataStructureRef.URN);
                }
                else
                {
                    string agencyId0 = structMapType.KeyFamilyRef.KeyFamilyAgencyID;
                    if (string.IsNullOrWhiteSpace(agencyId0))
                    {
                        agencyId0 = this.MaintainableParent.AgencyId;
                    }

                    this.SourceRef = new CrossReferenceImpl(
                        this, 
                        agencyId0, 
                        structMapType.MetadataStructureRef.MetadataStructureID, 
                        structMapType.MetadataStructureRef.Version, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd));
                }
            }

            // target ref can be one of two types so get one which isn't null
            if (structMapType.TargetKeyFamilyRef != null)
            {
                if (structMapType.TargetKeyFamilyRef.URN != null)
                {
                    this.TargetRef = new CrossReferenceImpl(this, structMapType.TargetKeyFamilyRef.URN);
                }
                else
                {
                    string agencyId1 = structMapType.TargetKeyFamilyRef.KeyFamilyAgencyID;
                    if (string.IsNullOrWhiteSpace(agencyId1))
                    {
                        agencyId1 = this.MaintainableParent.AgencyId;
                    }

                    this.TargetRef = new CrossReferenceImpl(
                        this, 
                        agencyId1, 
                        structMapType.TargetKeyFamilyRef.KeyFamilyID, 
                        structMapType.TargetKeyFamilyRef.Version, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd));
                }
            }
            else if (structMapType.TargetMetadataStructureRef != null)
            {
                if (structMapType.TargetMetadataStructureRef.URN != null)
                {
                    this.TargetRef = new CrossReferenceImpl(
                        this, structMapType.TargetMetadataStructureRef.URN);
                }
                else
                {
                    string agencyId2 = structMapType.TargetMetadataStructureRef.MetadataStructureAgencyID;
                    if (string.IsNullOrWhiteSpace(agencyId2))
                    {
                        agencyId2 = this.MaintainableParent.AgencyId;
                    }

                    this.TargetRef = new CrossReferenceImpl(
                        this, 
                        agencyId2, 
                        structMapType.TargetMetadataStructureRef.MetadataStructureID, 
                        structMapType.TargetMetadataStructureRef.Version, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd));
                }
            }

            // get list of component maps
            if (structMapType.ComponentMap != null)
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ComponentMapType compMap in
                    structMapType.ComponentMap)
                {
                    var componentMapCore = new ComponentMapCore(compMap, this);
                    this.components.Add(componentMapCore);
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

        #region Public Properties

        /// <summary>
        ///   Gets the components.
        /// </summary>
        public virtual IList<IComponentMapObject> Components
        {
            get
            {
                return new List<IComponentMapObject>(this.components);
            }
        }

        /// <summary>
        ///   Gets a value indicating whether extension.
        /// </summary>
        public virtual bool Extension
        {
            get
            {
                return this.extension;
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
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
            if (sdmxObject == null) return false;
            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IStructureMapObject)sdmxObject;
                if (!this.Equivalent(this.components, that.Components, includeFinalProperties))
                {
                    return false;
                }

                if (this.Extension != that.Extension)
                {
                    return false;
                }

                return this.DeepEqualsNameable(that, includeFinalProperties);
            }

            return false;
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
        private void Validate()
        {
            if (this.SourceRef == null)
            {
                throw new SdmxSemmanticException("CategorisationStructure Map missing source component");
            }

            if (this.TargetRef == null)
            {
                throw new SdmxSemmanticException("CategorisationStructure Map missing target component");
            }

            ISet<SdmxStructureType> allowedTypes = new HashSet<SdmxStructureType>();
            allowedTypes.Add(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow));
            allowedTypes.Add(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd));
            allowedTypes.Add(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow));
            allowedTypes.Add(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd));

            this.VerifyTypes(allowedTypes, this.SourceRef.TargetReference);
            this.VerifyTypes(allowedTypes, this.TargetRef.TargetReference);
        }

        /// <summary>
        /// The verify types.
        /// </summary>
        /// <param name="allowedTypes">
        /// The allowed types. 
        /// </param>
        /// <param name="actualType">
        /// The actual type. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        private void VerifyTypes(ISet<SdmxStructureType> allowedTypes, SdmxStructureType actualType)
        {
            if (!allowedTypes.Contains(actualType))
            {
                string allowedTypesStr = string.Empty;

                foreach (SdmxStructureType currentType in allowedTypes)
                {
                    allowedTypesStr += currentType + ",";
                }

                allowedTypesStr = allowedTypesStr.Substring(0, (allowedTypesStr.Length - 2) - 0);

                throw new SdmxSemmanticException(
                    "Disallowed concept map type '" + actualType + "' allowed types are '" + allowedTypesStr + "'");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////COMPOSITES		                     //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        ///   Get composites internal.
        /// </summary>
        protected override ISet<ISdmxObject> GetCompositesInternal()
        {
        	ISet<ISdmxObject> composites = base.GetCompositesInternal();
            base.AddToCompositeSet(this.components, composites);
            return composites;
        }

        #endregion
    }
}