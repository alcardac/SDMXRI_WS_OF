// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepresentationMapRefBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The representation map ref core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Mapping
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Util.Collections;

    using TextFormatType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.TextFormatType;
    using ToValueTypeTypeConstants = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.ToValueTypeTypeConstants;

    /// <summary>
    ///   The representation map ref core.
    /// </summary>
    [Serializable]
    public class RepresentationMapRefCore : SdmxStructureCore, IRepresentationMapRef
    {
        #region Fields

        /// <summary>
        ///   The codelist map.
        /// </summary>
        private readonly ICrossReference codelistMap;

        /// <summary>
        ///   The to text format.
        /// </summary>
        private readonly ITextFormat toTextFormat;

        /// <summary>
        ///   The to value type.
        /// </summary>
        private readonly ToValue toValueType;

        /// <summary>
        ///   The value mappings.
        /// </summary>
        private readonly IDictionaryOfSets<string, string> valueMappings;
      

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V21 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RepresentationMapRefCore"/> class.
        /// </summary>
        /// <param name="representationMapType">
        /// The iref. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public RepresentationMapRefCore(RepresentationMapType representationMapType, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.RepresentationMap), parent)
        {
            this.valueMappings = new DictionaryOfSets<string, string>();

            if (representationMapType.CodelistMap != null)
            {
                // Local Reference
                string agencyId = this.MaintainableParent.AgencyId;
                string maintainableId = this.MaintainableParent.Id;
                string version = this.MaintainableParent.Version;
                this.codelistMap = new CrossReferenceImpl(
                    this, agencyId, maintainableId, version, SdmxStructureEnumType.CodeListMap, representationMapType.CodelistMap.GetTypedRef<LocalCodelistMapRefType>().id);
            }

            if (representationMapType.ToTextFormat != null)
            {
                this.toTextFormat = new TextFormatObjectCore(representationMapType.ToTextFormat, this);
            }

            if (representationMapType.ToValueType != null)
            {
                switch (representationMapType.ToValueType)
                {
                    case ToValueTypeTypeConstants.Description:
                        this.toValueType = ToValue.Description;
                        break;
                    case ToValueTypeTypeConstants.Name:
                        this.toValueType = ToValue.Name;
                        break;
                    case ToValueTypeTypeConstants.Value:
                        this.toValueType = ToValue.Value;
                        break;
                }
            }

            if (representationMapType.ValueMap != null)
            {
                foreach (ValueMappingType vmt in representationMapType.ValueMap.ValueMapping)
                {
                    ISet<string> mappings = valueMappings[vmt.source];
                    if (mappings==null)
                    {
                        mappings = new HashSet<string>();
                        valueMappings.Add(vmt.source, mappings);
                    }
                    mappings.Add(vmt.target);
                }
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="RepresentationMapRefCore"/> class.
        /// </summary>
        /// <param name="textFormatType">
        /// The text format type. 
        /// </param>
        /// <param name="toValueType">
        /// The to value type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public RepresentationMapRefCore(TextFormatType textFormatType, string toValueType, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.RepresentationMap), parent)
        {
            this.valueMappings = new DictionaryOfSets<string, string>();
            this.toTextFormat = new TextFormatObjectCore(textFormatType, this);
            if (toValueType != null)
            {
                switch (toValueType)
                {
                    case Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ToValueTypeTypeConstants.Description:
                        this.toValueType = ToValue.Description;
                        break;
                    case Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ToValueTypeTypeConstants.Name:
                        this.toValueType = ToValue.Name;
                        break;
                    case Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ToValueTypeTypeConstants.Value:
                        this.toValueType = ToValue.Value;
                        break;
                }
            }

            this.Validate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepresentationMapRefCore"/> class.
        /// </summary>
        /// <param name="representationMapRefType">
        /// The iref. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public RepresentationMapRefCore(RepresentationMapRefType representationMapRefType, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.RepresentationMap), parent)
        {
            this.valueMappings = new DictionaryOfSets<string, string>();

            this.codelistMap = new CrossReferenceImpl(
                this, 
                representationMapRefType.RepresentationMapAgencyID, 
                representationMapRefType.RepresentationMapID, 
                MaintainableObject.DefaultVersion, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList));
            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepresentationMapRefCore"/> class.
        /// </summary>
        /// <param name="xref">
        /// The xref. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        protected internal RepresentationMapRefCore(IRepresentationMapRefMutableObject xref, ISdmxStructure parent)
            : base(xref, parent)
        {
            this.valueMappings = new DictionaryOfSets<string, string>();
            if (xref.CodelistMap != null)
            {
                this.codelistMap = new CrossReferenceImpl(this, xref.CodelistMap);
            }

            if (xref.ToTextFormat != null)
            {
                this.toTextFormat = new TextFormatObjectCore(xref.ToTextFormat, this);
            }

            if (xref.ValueMappings != null)
            {
                this.valueMappings = new DictionaryOfSets<string, string>(xref.ValueMappings);
            }

            this.toValueType = xref.ToValueType;
            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this);
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the codelist map.
        /// </summary>
        public virtual ICrossReference CodelistMap
        {
            get
            {
                return this.codelistMap;
            }
        }

        /// <summary>
        ///   Gets the to text format.
        /// </summary>
        public virtual ITextFormat ToTextFormat
        {
            get
            {
                return this.toTextFormat;
            }
        }

        /// <summary>
        ///   Gets the to value type.
        /// </summary>
        public virtual ToValue ToValueType
        {
            get
            {
                return this.toValueType;
            }
        }

        /// <summary>
        ///   Gets the value mappings.
        /// </summary>
        public virtual IDictionaryOfSets<string, string> ValueMappings
        {
            get
            {
                return new DictionaryOfSets<string, string>(this.valueMappings);
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
                var that = (IRepresentationMapRef)sdmxObject;
                if (!this.Equivalent(this.codelistMap, that.CodelistMap))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.toValueType, that.ToValueType))
                {
                    return false;
                }

                if (!ObjectUtil.EquivalentCollection(this.valueMappings.Keys, that.ValueMappings.Keys))
                {
                    return false;
                }

                if (!ObjectUtil.EquivalentCollection(this.valueMappings.Values, that.ValueMappings.Values))
                {
                    return false;
                }

                return true;
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
            if (this.codelistMap == null && this.valueMappings.Count == 0 && this.toTextFormat == null)
            {
                throw new SdmxSemmanticException(
                    "RepresentationMapping requires either a codelistMap, ToTextFormat or ValueMap");
            }

            if (this.toTextFormat != null)
            {
                if (this.toValueType == ToValue.Null)
                {
                    throw new SdmxSemmanticException(
                        "For RepresentationMapping, if using ToTextFormat ToValueType is also required");
                }
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
        base.AddToCompositeSet(this.toTextFormat, composites);
        return composites;
     }

        #endregion
    }
}