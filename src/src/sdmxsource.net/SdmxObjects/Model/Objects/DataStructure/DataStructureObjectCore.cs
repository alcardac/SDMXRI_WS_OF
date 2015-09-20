// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataStructureObjectCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data structure object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure
{
    using System;
    using System.Collections.Generic;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Extensions;

    using GroupType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.GroupType;

    // using System.ComponentModel;

    /// <summary>
    ///   The data structure object core.
    /// </summary>
    [Serializable]
    public class DataStructureObjectCore : MaintainableObjectCore<IDataStructureObject, IDataStructureMutableObject>, 
                                           IDataStructureObject
    {
        #region Static Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataStructureObjectCore));

        #endregion

        #region Fields

        /// <summary>
        ///   The _attribtue list.
        /// </summary>
        private readonly IAttributeList attributeList;

        /// <summary>
        ///   The _component id to component.
        /// </summary>
        private readonly Dictionary<string, IComponent> _componentIdToComponent =
            new Dictionary<string, IComponent>(StringComparer.Ordinal);

        /// <summary>
        ///   The _concept id to component.
        /// </summary>
        private readonly Dictionary<string, IComponent> _conceptIdToComponent =
            new Dictionary<string, IComponent>(StringComparer.Ordinal);

        /// <summary>
        ///   The _dimension list.
        /// </summary>
        private readonly IDimensionList _dimensionList;

        /// <summary>
        ///   The _groups.
        /// </summary>
        private readonly IList<IGroup> _groups;

        /// <summary>
        ///   The _measure list.
        /// </summary>
        private readonly IMeasureList _measureList;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStructureObjectCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public DataStructureObjectCore(IDataStructureMutableObject itemMutableObject)
            : base(itemMutableObject)
        {
            this._groups = new List<IGroup>();
            Log.Debug("Building DataStructureObject from Mutable Object");
            try
            {
                if (itemMutableObject.Groups != null)
                {
                    foreach (IGroupMutableObject mutable in itemMutableObject.Groups)
                    {
                        this._groups.Add(new GroupCore(mutable, this));
                    }
                }

                if (itemMutableObject.MeasureList != null)
                {
                    this._measureList = new MeasureListCore(itemMutableObject.MeasureList, this);
                }

                if (itemMutableObject.DimensionList != null)
                {
                    this._dimensionList = new DimensionListCore(itemMutableObject.DimensionList, this);
                }

                if (itemMutableObject.AttributeList != null)
                {
                    this.attributeList = new AttributeListCore(itemMutableObject.AttributeList, this);
                }

                this.PopulateComponentDic();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("DataStructureObject Built " + this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStructureObjectCore"/> class.
        /// </summary>
        /// <param name="dataStructure">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public DataStructureObjectCore(DataStructureType dataStructure)
            : base(dataStructure, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd))
        {
            this._groups = new List<IGroup>();
            Log.Debug("Building DataStructureObject from 2.1 SDMX");
            DataStructureComponentsType components = null;
           
            if (dataStructure.DataStructureComponents != null)
            {
                components = dataStructure.DataStructureComponents.Content;    
            }
            
            try
            {
                if (components != null)
                {
                    foreach (Group currentGroup in components.Group)
                    {
                        this._groups.Add(new GroupCore(currentGroup.Content, this));
                    }

                    if (components.DimensionList != null)
                    {
                        this._dimensionList = new DimensionListCore(components.DimensionList.Content, this);
                    }

                    if (components.AttributeList != null)
                    {
                        this.attributeList = new AttributeListCore(components.AttributeList.Content, this);
                    }

                    if (components.MeasureList != null)
                    {
                        this._measureList = new MeasureListCore(components.MeasureList.Content, this);
                    }

                    this.PopulateComponentDic();
                }
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException(th, ExceptionCode.ObjectStructureConstructionError, this);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("DataStructureObject Built " + this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStructureObjectCore"/> class.
        /// </summary>
        /// <param name="keyFamilyType">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public DataStructureObjectCore(KeyFamilyType keyFamilyType)
            : base(
                keyFamilyType, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd), 
                keyFamilyType.validTo, 
                keyFamilyType.validFrom, 
                keyFamilyType.version, 
                CreateTertiary(keyFamilyType.isFinal), 
                keyFamilyType.agencyID, 
                keyFamilyType.id, 
                keyFamilyType.uri, 
                keyFamilyType.Name, 
                keyFamilyType.Description, 
                CreateTertiary(keyFamilyType.isExternalReference), 
                keyFamilyType.Annotations)
        {
            this._groups = new List<IGroup>();
            Log.Debug("Building DataStructureObject from 2.0 SDMX");
            ComponentsType components = keyFamilyType.Components;
            try
            {
                if (components != null)
                {
                    foreach (GroupType currentGroup in components.Group)
                    {
                        this._groups.Add(new GroupCore(currentGroup, this));
                    }

                    if (components.Dimension != null)
                    {
                        this._dimensionList = new DimensionListCore(keyFamilyType, this);
                    }

                    if (components.PrimaryMeasure != null)
                    {
                        this._measureList = new MeasureListCore(components.PrimaryMeasure, this);
                    }

                    if (ObjectUtil.ValidCollection(components.Attribute))
                    {
                        this.attributeList = new AttributeListCore(keyFamilyType, this);
                    }

                    this.PopulateComponentDic();
                }
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("DataStructureObject Built " + this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStructureObjectCore"/> class.
        /// </summary>
        /// <param name="keyFamilyType">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public DataStructureObjectCore(Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.KeyFamilyType keyFamilyType)
            : base(
                keyFamilyType, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd), 
                keyFamilyType.version, 
                keyFamilyType.agency, 
                keyFamilyType.id, 
                keyFamilyType.uri, 
                keyFamilyType.Name, 
                null, 
                keyFamilyType.Annotations)
        {
            this._groups = new List<IGroup>();
            Log.Debug("Building DataStructureObject from 1.0 SDMX");
            Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.ComponentsType components = keyFamilyType.Components;
            if (components != null)
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.GroupType currentGroup in components.Group)
                {
                    this._groups.Add(new GroupCore(currentGroup, this));
                }

                if (components.Dimension != null)
                {
                    this._dimensionList = new DimensionListCore(keyFamilyType, this);
                }

                if (components.PrimaryMeasure != null)
                {
                    this._measureList = new MeasureListCore(components.PrimaryMeasure, this);
                }

                if (components.Attribute != null)
                {
                    this.attributeList = new AttributeListCore(keyFamilyType, this);
                }

                this.PopulateComponentDic();
            }
         
            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("DataStructureObject Built " + this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStructureObjectCore"/> class.
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
        private DataStructureObjectCore(IDataStructureObject agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
            this._groups = new List<IGroup>();
            Log.Debug("Stub DataStructureObject Built");
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
        ///   Gets the attribtue list.
        /// </summary>
        public virtual IAttributeList AttributeList
        {
            get
            {
                return this.attributeList;
            }
        }

        /// <summary>
        ///   Gets the attributes.
        /// </summary>
        public virtual IList<IAttributeObject> Attributes
        {
            get
            {
                if (this.attributeList != null)
                {
                    return this.attributeList.Attributes;
                }

                return new List<IAttributeObject>();
            }
        }

        /// <summary>
        /// Gets the components
        /// </summary>
        public virtual IList<IComponent> Components
        {
            get
            {
                IList<IComponent> returnList = new List<IComponent>();
                returnList.AddAll(this._dimensionList.Dimensions);
                returnList.AddAll(this.Attributes);
                returnList.Add(this.PrimaryMeasure);

                return returnList;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///   Gets the cross referenced constrainables.
        /// </summary>
        public virtual IList<ICrossReference> CrossReferencedConstrainables
        {
            get
            {
                return new List<ICrossReference>();
            }
        }

        /// <summary>
        ///   Gets the dataset attributes.
        /// </summary>
        public virtual IList<IAttributeObject> DatasetAttributes
        {
            get
            {
                return this.GetAttribute(AttributeAttachmentLevel.DataSet);
            }
        }

        /// <summary>
        ///   Gets the dimension group attributes.
        /// </summary>
        public virtual IList<IAttributeObject> DimensionGroupAttributes
        {
            get
            {
                return this.GetAttribute(AttributeAttachmentLevel.DimensionGroup);
            }
        }

        /// <summary>
        ///   Gets the dimension list.
        /// </summary>
        public virtual IDimensionList DimensionList
        {
            get
            {
                return this._dimensionList;
            }
        }

        /// <summary>
        ///   Gets the frequency dimension.
        /// </summary>
        public virtual IDimension FrequencyDimension
        {
            get
            {
                foreach (IDimension currentDimension in this.GetDimensions())
                {
                    if (currentDimension.FrequencyDimension)
                    {
                        return currentDimension;
                    }
                }

                return null;
            }
        }

        /// <summary>
        ///   Gets the group attributes.
        /// </summary>
        public virtual IList<IAttributeObject> GroupAttributes
        {
            get
            {
                return this.GetAttribute(AttributeAttachmentLevel.Group);
            }
        }

        /// <summary>
        ///   Gets the groups.
        /// </summary>
        public virtual IList<IGroup> Groups
        {
            get
            {
                return new List<IGroup>(this._groups);
            }
        }

        /// <summary>
        ///   Gets the measure list.
        /// </summary>
        public virtual IMeasureList MeasureList
        {
            get
            {
                return this._measureList;
            }
        }

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public override IDataStructureMutableObject MutableInstance
        {
            get
            {
                return new DataStructureMutableCore(this);
            }
        }

        /// <summary>
        ///   Gets the observation attributes.
        /// </summary>
        public virtual IList<IAttributeObject> ObservationAttributes
        {
            get
            {
                return this.GetAttribute(AttributeAttachmentLevel.Observation);
            }
        }

        /// <summary>
        ///   Gets the primary measure.
        /// </summary>
        public virtual IPrimaryMeasure PrimaryMeasure
        {
            get
            {
                if (this._measureList != null)
                {
                    return this._measureList.PrimaryMeasure;
                }

                return null;
            }
        }

        /// <summary>
        ///   Gets the time dimension.
        /// </summary>
        public virtual IDimension TimeDimension
        {
            get
            {
                foreach (IDimension currentDimension in this.GetDimensions())
                {
                    if (currentDimension.TimeDimension)
                    {
                        return currentDimension;
                    }
                }

                return null;
            }
        }

        #endregion

        #region Explicit Interface Properties

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        IMaintainableMutableObject IMaintainableObject.MutableInstance
        {
            get
            {
                return this.MutableInstance;
            }
        }

        #endregion

        #region Public Methods and Operators

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
            if (sdmxObject == null) return false;
            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IDataStructureObject)sdmxObject;
                if (!this.Equivalent(this._groups, that.Groups, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this._dimensionList, that.DimensionList, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this.attributeList, that.AttributeList, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this._measureList, that.MeasureList, includeFinalProperties))
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

        /// <summary>
        /// The get attribute.
        /// </summary>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <returns>
        /// The <see cref="IAttributeObject"/> . 
        /// </returns>
        public virtual IAttributeObject GetAttribute(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                foreach (IAttributeObject currentAttribtue in this.Attributes)
                {
                    if (currentAttribtue.Id.Equals(id))
                    {
                        return currentAttribtue;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// The get component.
        /// </summary>
        /// <param name="id">
        /// The concept id. 
        /// </param>
        /// <returns>
        /// The <see cref="IComponent"/> . 
        /// </returns>
        public IComponent GetComponent(string id)
        {
            IComponent component = null;
            if (this._componentIdToComponent.TryGetValue(id, out component))
            {
                return component;
            }

            return null;
        }

        /// <summary>
        /// The get dimension.
        /// </summary>
        /// <param name="id">
        /// The concept id. 
        /// </param>
        /// <returns>
        /// The <see cref="IDimension"/> . 
        /// </returns>
        public IDimension GetDimension(string id)
        {
            IComponent component;
            if (this._componentIdToComponent.TryGetValue(id, out component))
            {
                var dimension = component as IDimension;
                return dimension;
            }

            return null;
        }

        /// <summary>
        /// The get dimension group attribute.
        /// </summary>
        /// <param name="id">
        /// The concept id. 
        /// </param>
        /// <returns>
        /// The <see cref="IAttributeObject"/> . 
        /// </returns>
        public virtual IAttributeObject GetDimensionGroupAttribute(string id)
        {
            return this.GetAttribute(AttributeAttachmentLevel.DimensionGroup, id);
        }

        /// <summary>
        /// The get dimensions.
        /// </summary>
        /// <param name="includeTypes">
        /// The include types. 
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> . 
        /// </returns>
       /* public virtual IList<IDimension> GetDimensions(params SdmxStructureType[] includeTypes)
        {
            if (this._dimensionList != null)
            {
                if (includeTypes == null || includeTypes.Length == 0)
                {
                    return this._dimensionList.Dimensions;
                }

                IList<IDimension> returnList = new List<IDimension>();
                foreach (IDimension dim in this._dimensionList.Dimensions)
                {
                    foreach (SdmxStructureType currentType in includeTypes)
                    {
                        if (currentType == dim.StructureType)
                        {
                            returnList.Add(dim);
                        }
                    }
                }

                return returnList;
            }

            return new List<IDimension>();
        }*/

        /// <summary>
        /// The get dimensions.
        /// </summary>
        /// <param name="include">
        /// The include. 
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> . 
        /// </returns>
        public IList<IDimension> GetDimensions(params SdmxStructureEnumType[] include)
        {
            var returnList = new List<IDimension>();
            if (this._dimensionList != null)
            {
                if (include == null || include.Length == 0)
                {
                    return this._dimensionList.Dimensions;
                }

                foreach (IDimension dimensionCore in this._dimensionList.Dimensions)
                {
                    foreach (SdmxStructureEnumType currentType in include)
                    {
                        if (currentType == dimensionCore.StructureType.EnumType)
                        {
                            returnList.Add(dimensionCore);
                        }
                    }
                }
            }

            return returnList;
        }

        /// <summary>
        /// The get group.
        /// </summary>
        /// <param name="groupId">
        /// The group id. 
        /// </param>
        /// <returns>
        /// The <see cref="IGroup"/> . 
        /// </returns>
        public virtual IGroup GetGroup(string groupId)
        {
            foreach (IGroup currentGroup in this._groups)
            {
                if (currentGroup.Id.Equals(groupId))
                {
                    return currentGroup;
                }
            }

            return null;
        }

        /// <summary>
        /// The get group attribute.
        /// </summary>
        /// <param name="id">
        /// The concept id. 
        /// </param>
        /// <returns>
        /// The <see cref="IAttributeObject"/> . 
        /// </returns>
        public virtual IAttributeObject GetGroupAttribute(string id)
        {
            return this.GetAttribute(AttributeAttachmentLevel.Group, id);
        }

        /// <summary>
        /// The get group attributes.
        /// </summary>
        /// <param name="groupId">
        /// The group id. 
        /// </param>
        /// <param name="includeDimensionGroups">
        /// The include dimension groups. 
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> . 
        /// </returns>
        public virtual IList<IAttributeObject> GetGroupAttributes(string groupId, bool includeDimensionGroups)
        {
            IGroup group = GetGroup(groupId);
            if (group == null)
            {
                throw new ArgumentNullException("Group not found on Data Structure '" + this.Urn + "' with id: " + groupId);
            }
            
            IList<IAttributeObject> allGroupAttributes = this.GroupAttributes;
            IList<IAttributeObject> returnList = new List<IAttributeObject>();

            foreach (IAttributeObject currentAttribtue in allGroupAttributes)
            {
                if (currentAttribtue.AttachmentGroup != null)
                {
                    if (currentAttribtue.AttachmentGroup.Equals(groupId))
                    {
                        returnList.Add(currentAttribtue);
                    }
                }
            }

          	foreach(IAttributeObject cuAttributeBean in DimensionGroupAttributes)
            {
		     	IList<string> attrDimRefs = cuAttributeBean.DimensionReferences;
			    IList<string> grpDimRefs = group.DimensionRefs;

			    if(attrDimRefs.ContainsAll(grpDimRefs) && grpDimRefs.ContainsAll(attrDimRefs))
                {
				   returnList.Add(cuAttributeBean);
			    }
	    	}

            return returnList;
        }

        /// <summary>
        /// The get observation attribute.
        /// </summary>
        /// <param name="id">
        /// The concept id. 
        /// </param>
        /// <returns>
        /// The <see cref="IAttributeObject"/> . 
        /// </returns>
        public virtual IAttributeObject GetObservationAttribute(string id)
        {
            return this.GetAttribute(AttributeAttachmentLevel.Observation, id);
        }

        /// <summary>
        /// The get observation attributes.
        /// </summary>
        /// <param name="crossSectionalConcept">
        /// The cross sectional concept. 
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> . 
        /// </returns>
        public virtual IList<IAttributeObject> GetObservationAttributes(string crossSectionalConcept)
        {
            if (crossSectionalConcept == null)
            {
                return this.ObservationAttributes;
            }

            IList<IAttributeObject> returnList = new List<IAttributeObject>();

            foreach (IAttributeObject att in this.DimensionGroupAttributes)
            {
                if (att.DimensionReferences.Contains(crossSectionalConcept))
                {
                    returnList.Add(att);
                }
            }

            this.ObservationAttributes.AddAll(returnList);
            return returnList;
        }

        /// <summary>
        /// The get series attributes.
        /// </summary>
        /// <param name="crossSectionalConcept">
        /// The cross sectional concept. 
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> . 
        /// </returns>
        public virtual IList<IAttributeObject> GetSeriesAttributes(string crossSectionalConcept)
        {
            if (crossSectionalConcept == null)
            {
                return this.DimensionGroupAttributes;
            }

            IList<IAttributeObject> returnList = new List<IAttributeObject>();

            foreach (IAttributeObject att in this.DimensionGroupAttributes)
            {
                if (!att.DimensionReferences.Contains(crossSectionalConcept))
                {
                    returnList.Add(att);
                }
            }

            return returnList;
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
        /// The <see cref="IDataStructureObject"/> . 
        /// </returns>
        public override IDataStructureObject GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return new DataStructureObjectCore(this, actualLocation, isServiceUrl);
        }

        /// <summary>
        ///   The has frequency dimension.
        /// </summary>
        /// <returns> The <see cref="bool" /> . </returns>
        public virtual bool HasFrequencyDimension()
        {
            
            foreach (IDimension currentDimension in this.GetDimensions(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dimension).EnumType))
            {
                if (currentDimension.FrequencyDimension)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The is compatible.
        /// </summary>
        /// <param name="schemaVersion">
        /// </param>
        /// <returns>
        /// </returns>
        public virtual bool IsCompatible(SdmxSchema schemaVersion)
        {
            switch (schemaVersion.EnumType)
            {
                case SdmxSchemaEnumType.VersionOne:
                case SdmxSchemaEnumType.VersionTwo:
                    if (schemaVersion.EnumType == SdmxSchemaEnumType.VersionOne)
                    {
                        foreach (IComponent currentComponent in GetDimensions(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dimension).EnumType))
                        {
                            if (currentComponent.Representation == null || currentComponent.Representation.Representation == null)
                            {
                                return false;
                            }
                        }
                    }
                    //Intensionally no break, as version 2.0 compatability checks also apply to version 1.0

                    ISet<String> componentIds = new HashSet<String>();
                    foreach (IComponent currentComponent in Components)
                    {
                        if (currentComponent.ConceptRef.TargetReference == SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Concept))
                        {
                            string conceptId = currentComponent.ConceptRef.FullId;
                            if (componentIds.Contains(conceptId))
                            {
                                return false;
                            }
                            componentIds.Add(conceptId);
                        }
                    }
                    break;
                case SdmxSchemaEnumType.Edi:
                    break;
            }
            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the component with <paramref name="conceptId"/>
        /// </summary>
        /// <param name="components">
        /// The components. 
        /// </param>
        /// <param name="conceptId">
        /// The concept id. 
        /// </param>
        /// <typeparam name="TComponent">
        /// The component type 
        /// </typeparam>
        /// <returns>
        /// The <see cref="IComponent"/> . 
        /// </returns>
        private static IComponent GetComponent<TComponent>(IList<TComponent> components, string conceptId)
            where TComponent : IComponent
        {
            // TODO this method and it's overloads are being called a lot. Maybe use a Dictionary between concept id and components.
            foreach (TComponent currentcomponent in components)
            {
                // TODO java 0.9.4 checks the concept id against the component id which might not be the same.
                if (currentcomponent.ConceptRef.ChildReference.Id.Equals(conceptId))
                {
                    return currentcomponent;
                }
            }

            return null;
        }

        /// <summary>
        /// The get attribute.
        /// </summary>
        /// <param name="type">
        /// The type. 
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> . 
        /// </returns>
        private IList<IAttributeObject> GetAttribute(AttributeAttachmentLevel type)
        {
            IList<IAttributeObject> returnList = new List<IAttributeObject>();

            foreach (IAttributeObject currentAttribtue in this.Attributes)
            {
                if (currentAttribtue.AttachmentLevel == type)
                {
                    returnList.Add(currentAttribtue);
                }
            }

            return returnList;
        }

        /// <summary>
        /// The get attribute.
        /// </summary>
        /// <param name="type">
        /// The type. 
        /// </param>
        /// <param name="conceptId">
        /// The concept id. 
        /// </param>
        /// <returns>
        /// The <see cref="IAttributeObject"/> . 
        /// </returns>
        private IAttributeObject GetAttribute(AttributeAttachmentLevel type, string conceptId)
        {
            foreach (IAttributeObject currentAttribtue in GetAttribute(type))
            {
                if (currentAttribtue.Id.Equals(conceptId))
                {
                    return currentAttribtue;
                }
            }

            return null;
        }

        /// <summary>
        /// The i validate unique component.
        /// </summary>
        /// <param name="conceptIds">
        /// The concept ids. 
        /// </param>
        /// <param name="component">
        /// The component. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        private void IValidateUniqueComponent(ISet<string> conceptIds, IComponent component)
        {
            string conceptId = component.Id;
            if (conceptIds.Contains(conceptId))
            {
                throw new SdmxSemmanticException("Duplicate Data CategorisationStructure Component Id : " + conceptId);
            }

            conceptIds.Add(conceptId);
        }

        /// <summary>
        ///   Populate the <see cref="_componentIdToComponent" /> from <see cref="_dimensionList" />, <see cref="attributeList" /> and <see
        ///    cref="_measureList" />
        /// </summary>
        private void PopulateComponentDic()
        {
            var dimensionList = this._dimensionList;
            if (dimensionList != null)
            {
                this.PopulateComponentDic(dimensionList.Dimensions);
            }

            var attribtueList = this.attributeList;
            if (attribtueList != null)
            {
                this.PopulateComponentDic(attribtueList.Attributes);
            }

            if (this._measureList != null)
            {
                this.PopulateComponentDic(new[] { this._measureList.PrimaryMeasure });
            }
        }

        /// <summary>
        /// Populate <see cref="_componentIdToComponent"/> from <paramref name="components"/>
        /// </summary>
        /// <param name="components">
        /// The components. 
        /// </param>
        /// <typeparam name="TComponent">
        /// The <paramref name="components"/> type 
        /// </typeparam>
        private void PopulateComponentDic<TComponent>(IEnumerable<TComponent> components) where TComponent : IComponent
        {
            foreach (TComponent component in components)
            {
                string id = component.Id;
                this._componentIdToComponent.Add(id, component);
            }
        }

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (!this.IsExternalReference.IsTrue)
            {
                IDictionary<string, IDimension> dimensionMap = new Dictionary<string, IDimension>(StringComparer.Ordinal);

                ISet<string> conceptIds = new HashSet<string>(StringComparer.Ordinal);
                ISet<int> dimPos = new HashSet<int>();

                // VALIDATE DIMENSIONS
                if (!ObjectUtil.ValidCollection(this.GetDimensions()))
                {
                    throw new SdmxSemmanticException("DSD must have at least one dimension");
                }

                foreach (IDimension dimension in this.GetDimensions())
                {
                    if (dimPos.Contains(dimension.Position))
                    {
                        throw new SdmxSemmanticException(
                            "Two dimensions can not share the same dimension position : " + dimension.Position);
                    }

                    dimPos.Add(dimension.Position);
                    string conceptId = dimension.Id;
                    this.IValidateUniqueComponent(conceptIds, dimension);
                    dimensionMap.Add(conceptId, dimension);
                }

                // VALIDATE ONLY ONE FREQUENCY DIMENSION
                bool foundFreq = false;

                foreach (IDimension dimension0 in this.GetDimensions())
                {
                    if (dimension0.FrequencyDimension)
                    {
                        if (foundFreq)
                        {
                            throw new SdmxSemmanticException(
                                "DataStructure can not have more then one frequency dimension");
                        }

                        foundFreq = true;
                    }
                }

                // VALIDATE GROUPS
                ISet<string> groupIds = new HashSet<string>();
                if (this._groups != null)
                {
                    foreach (IGroup group in this._groups)
                    {
                        foreach (string dimensionRef in group.DimensionRefs)
                        {
                            if (!dimensionMap.ContainsKey(dimensionRef))
                            {
                                IDimension timeDimension = this.TimeDimension;
                                if (timeDimension != null)
                                {
                                    if (timeDimension.Id.Equals(dimensionRef))
                                    {
                                        throw new SdmxSemmanticException(
                                            ExceptionCode.GroupCannotReferenceTimeDimension, group.Id);
                                    }
                                }

                                throw new SdmxSemmanticException(
                                    ExceptionCode.ReferenceError, 
                                    SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dimension) + " " + dimensionRef, 
                                    SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Group), 
                                    group.ToString());
                            }
                        }

                        if (groupIds.Contains(group.Id))
                        {
                            throw new SdmxSemmanticException(ExceptionCode.KeyFamilyDuplicateGroupId, group.Id);
                        }

                        groupIds.Add(group.Id);
                    }
                }

                // VALIDATE PRIMARY MEASURE
                if (this.PrimaryMeasure != null)
                {
                    this.IValidateUniqueComponent(conceptIds, this.PrimaryMeasure);
                }
                else if (!IsExternalReference.IsTrue)
                {
                    throw new SdmxSemmanticException(
                        ExceptionCode.ObjectMissingRequiredElement, this.StructureType, "PrimaryMeasure");
                }

                // VALIDATE ATTRIBUTES
                foreach (IAttributeObject current in this.Attributes)
                {
                    this.IValidateUniqueComponent(conceptIds, current);
                    if (current.AttachmentLevel == AttributeAttachmentLevel.Group)
                    {
                        if (current.AttachmentGroup == null)
                        {
                            throw new SdmxSemmanticException(
                                ExceptionCode.KeyFamilyGroupAttributeMissingGroupId, current.ToString());
                        }

                        if (!groupIds.Contains(current.AttachmentGroup))
                        {
                            throw new SdmxSemmanticException(
                                ExceptionCode.ReferenceError, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Group), 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataAttribute), 
                                current.ToString());
                        }
                    }
                }
            }
        }

      

       ///////////////////////////////////////////////////////////////////////////////////////////////////
       ////////////COMPOSITES				             //////////////////////////////////////////////////
       ///////////////////////////////////////////////////////////////////////////////////////////////////

       /// <summary>
       /// Gets composites internal.
       /// </summary>
       protected override ISet<ISdmxObject> GetCompositesInternal() 
       {
        	ISet<ISdmxObject> composites = base.GetCompositesInternal();
            base.AddToCompositeSet(this._groups, composites);
            base.AddToCompositeSet(this._dimensionList, composites);
            base.AddToCompositeSet(this.attributeList, composites);
            base.AddToCompositeSet(this._measureList, composites);
            return composites;
       }

        #endregion
    }
}