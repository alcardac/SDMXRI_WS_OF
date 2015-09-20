// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentMapBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The component map core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Mapping
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The component map core.
    /// </summary>
    [Serializable]
    public class ComponentMapCore : AnnotableCore, IComponentMapObject
    {
        #region Fields

        /// <summary>
        ///   The map concept ref.
        /// </summary>
        private readonly string mapConceptRef;

        /// <summary>
        ///   The map target concept ref.
        /// </summary>
        private readonly string mapTargetConceptRef;

        /// <summary>
        ///   The rep map ref.
        /// </summary>
        private readonly IRepresentationMapRef repMapRef;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentMapCore"/> class.
        /// </summary>
        /// <param name="compMap">
        /// The comp map. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        protected internal ComponentMapCore(IComponentMapMutableObject compMap, ISdmxStructure parent)
            : base(compMap, parent)
        {
            if (compMap.MapConceptRef != null)
            {
                this.mapConceptRef = compMap.MapConceptRef;
            }

            if (compMap.MapTargetConceptRef != null)
            {
                this.mapTargetConceptRef = compMap.MapTargetConceptRef;
            }

            if (compMap.RepMapRef != null)
            {
                this.repMapRef = new RepresentationMapRefCore(compMap.RepMapRef, this);
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
        /// Initializes a new instance of the <see cref="ComponentMapCore"/> class.
        /// </summary>
        /// <param name="compMap">
        /// The comp map. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        protected internal ComponentMapCore(ComponentMapType compMap, IStructureMapObject parent)
            : base(null, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryMap), parent)
        {
            this.mapConceptRef = compMap.Source.GetTypedRef<LocalComponentListComponentRefType>().id;
            this.mapTargetConceptRef = compMap.Target.GetTypedRef<LocalComponentListComponentRefType>().id;
            if (compMap.RepresentationMapping != null)
            {
                this.repMapRef = new RepresentationMapRefCore(compMap.RepresentationMapping, this);
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

        // private ICrossReference getCrossReferenceFromRef(ICrossReference dsdRef, RefBaseType ref) {
        // SdmxStructureType structureType = SdmxStructureType.parseClass(dsdRef.get().toString());
        // string agencyId = dsdRef.getMaintainableReference().getAgencyId();
        // string id = dsdRef.getMaintainableReference().getMaintainableId();
        // string version = dsdRef.getMaintainableReference().getVersion();
        // string componentId = ref.getId();
        // return  new CrossReferenceCore(this, agencyId, id, version, structureType, componentId);
        // }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentMapCore"/> class.
        /// </summary>
        /// <param name="compMap">
        /// The comp map. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// ///
        /// <exception cref="SdmxNotImplementedException">
        /// Throws Unsupported Exception.
        /// </exception>
        protected internal ComponentMapCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ComponentMapType compMap, IStructureMapObject parent)
            : base(null, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryMap), parent)
        {
            throw new SdmxNotImplementedException("ComponentMapCore at version 2.0");

            // super(null, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CategoryMap), parent);
            // this.mapConceptRef = getCrossReferenceFromV2Ref(parent.getSourceRef(), compMap.getMapConceptRef());
            // this.mapTargetConceptRef = getCrossReferenceFromV2Ref(parent.getTargetRef(), compMap.getMapTargetConceptRef());
            // if (compMap.getRepresentationMapRef() != null) {
            // this.repMapRef =  new RepresentationMapRefCore(compMap.getRepresentationMapRef(), this);
            // }
            // if(compMap.getToTextFormat() != null) {
            // this.repMapRef = new RepresentationMapRefCore(compMap.getToTextFormat(), compMap.getToValueType(), this);
            // }
            // try {
            // validate();
            // } catch(ValidationException e) {
            // throw new SdmxSemmanticException(e, ExceptionCode.FAIL_VALIDATION, this);
            // }
        }

        #endregion

        // //Dimensions@MT@DsdIn@10@A
        // private ICrossReference getCrossReferenceFromV2Ref(ICrossReference dsdRef, string ref) {
        // string[] split = ref.split("@");
        // if(split.length != 2) {
        // throw new SdmxSemmanticException("Version 2.0 ComponentMap expecting conceptRef to be a reference to the component in the format 'Class@ComponentId'   Exmaple:Dimensions@FREQ");
        // }
        // SdmxStructureType structureType = SdmxStructureType.parseClass(split[0]);
        // string agencyId = dsdRef.getMaintainableReference().getAgencyId();
        // string id = dsdRef.getMaintainableReference().getMaintainableId();
        // string version = dsdRef.getMaintainableReference().getVersion();
        // string componentId = split[1];
        // return  new CrossReferenceCore(this, agencyId, id, version, structureType, componentId);
        // }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        // private void verifyTypes(Set<SdmxStructureType> allowedTypes, SdmxStructureType actualType) {
        // if(!allowedTypes.contains(actualType)) {
        // string allowedTypesStr = "";
        // for(SdmxStructureType currentType : allowedTypes) {
        // allowedTypesStr+=currentType +",";
        // }
        // allowedTypesStr = allowedTypesStr.substring(0, allowedTypesStr.length()-2);
        // throw new SdmxSemmanticException("Disallowed concept map type '"+actualType+"' allowed types are '"+allowedTypesStr+"'");
        // }
        // }
        #region Public Properties

        /// <summary>
        ///   Gets the map concept ref.
        /// </summary>
        public virtual string MapConceptRef
        {
            get
            {
                return this.mapConceptRef;
            }
        }

        /// <summary>
        ///   Gets the map target concept ref.
        /// </summary>
        public virtual string MapTargetConceptRef
        {
            get
            {
                return this.mapTargetConceptRef;
            }
        }

        /// <summary>
        ///   Gets the rep map ref.
        /// </summary>
        public virtual IRepresentationMapRef RepMapRef
        {
            get
            {
                return this.repMapRef;
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
            if (sdmxObject == null) return false;
            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IComponentMapObject)sdmxObject;
                if (!ObjectUtil.Equivalent(this.mapConceptRef, that.MapConceptRef))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.mapTargetConceptRef, that.MapTargetConceptRef))
                {
                    return false;
                }

                if (!this.Equivalent(this.repMapRef, that.RepMapRef, includeFinalProperties))
                {
                    return false;
                }

                return this.DeepEqualsInternalAnnotable(that, includeFinalProperties);
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
            if (!ObjectUtil.ValidObject(this.mapConceptRef))
            {
                throw new SdmxSemmanticException("Component Map missing source component");
            }

            if (!ObjectUtil.ValidObject(this.mapTargetConceptRef))
            {
                throw new SdmxSemmanticException("Component Map missing target component");
            }

            ISet<SdmxStructureType> allowedTypes = new HashSet<SdmxStructureType>();
            allowedTypes.Add(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.AttributeDescriptor));
            allowedTypes.Add(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConstraintContentTarget));
            allowedTypes.Add(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DatasetTarget));
            allowedTypes.Add(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dimension));
            allowedTypes.Add(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DimensionDescriptorValuesTarget));
            allowedTypes.Add(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.IdentifiableObjectTarget));
            allowedTypes.Add(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MeasureDimension));
            allowedTypes.Add(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataAttribute));
            allowedTypes.Add(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.PrimaryMeasure));
            allowedTypes.Add(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.TimeDimension));
            allowedTypes.Add(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportPeriodTarget));

            // verifyTypes(allowedTypes, mapConceptRef.getTargetReference());
            // verifyTypes(allowedTypes, mapTargetConceptRef.getTargetReference());
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
           base.AddToCompositeSet(this.repMapRef, composites);
           return composites;
        }

        #endregion
    }
}