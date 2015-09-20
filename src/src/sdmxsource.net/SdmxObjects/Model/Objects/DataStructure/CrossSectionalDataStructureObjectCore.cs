// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossSectionalDataStructureObjectCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The cross sectional data structure object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Extensions;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;

    /// <summary>
    ///     The cross sectional data structure object core.
    /// </summary>
    [Serializable]
    public class CrossSectionalDataStructureObjectCore : DataStructureObjectCore, ICrossSectionalDataStructureObject
    {
        #region Fields

        /// <summary>
        ///     The _attribute to measures map.
        /// </summary>
        private readonly IDictionary<string, IList<ICrossSectionalMeasure>> _attributeToMeasuresMap;

        /// <summary>
        ///     The codelist map.
        /// </summary>
        private readonly IDictionary<string, ICrossReference> _codelistMap;

        /// <summary>
        ///     The _cross sectional attach data set.
        /// </summary>
        private readonly IList<IComponent> _crossSectionalAttachDataSet;

        /// <summary>
        ///     The _cross sectional attach group.
        /// </summary>
        private readonly IList<IComponent> _crossSectionalAttachGroup;

        /// <summary>
        ///     The _cross sectional attach observation.
        /// </summary>
        private readonly IList<IComponent> _crossSectionalAttachObservation;

        /// <summary>
        ///     The _cross sectional attach section.
        /// </summary>
        private readonly IList<IComponent> _crossSectionalAttachSection;

        /// <summary>
        ///     The _cross sectional measures.
        /// </summary>
        private readonly IList<ICrossSectionalMeasure> _crossSectionalMeasures;

        /// <summary>
        ///     The measure dimensions.
        /// </summary>
        private readonly IList<string> _measureDimensions;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossSectionalDataStructureObjectCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The itemMutableObject.
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public CrossSectionalDataStructureObjectCore(ICrossSectionalDataStructureMutableObject itemMutableObject)
            : base(itemMutableObject)
        {
            this._crossSectionalMeasures = new List<ICrossSectionalMeasure>();
            this._crossSectionalAttachDataSet = new List<IComponent>();
            this._crossSectionalAttachGroup = new List<IComponent>();
            this._crossSectionalAttachSection = new List<IComponent>();
            this._crossSectionalAttachObservation = new List<IComponent>();
            this._attributeToMeasuresMap = new Dictionary<string, IList<ICrossSectionalMeasure>>();
            this._measureDimensions = new List<string>();
            this._codelistMap = new Dictionary<string, ICrossReference>();

            if (itemMutableObject.CrossSectionalMeasures != null)
            {
                foreach (ICrossSectionalMeasureMutableObject currentMeasure in itemMutableObject.CrossSectionalMeasures)
                {
                    this._crossSectionalMeasures.Add(new CrossSectionalMeasureCore(currentMeasure, this));
                }
            }

            if (itemMutableObject.MeasureDimensionCodelistMapping != null)
            {
                foreach (KeyValuePair<string, IStructureReference> pair in itemMutableObject.MeasureDimensionCodelistMapping)
                {
                    this._codelistMap.Add(pair.Key, new CrossReferenceImpl(this, pair.Value));
                }
            }

            foreach (IDimensionMutableObject dim in itemMutableObject.Dimensions)
            {
                if (dim.MeasureDimension)
                {
                    this._measureDimensions.Add(dim.ConceptRef.IdentifiableIds[0]);
                }
            }

            if (itemMutableObject.CrossSectionalAttachDataSet != null)
            {
                foreach (string componentId in itemMutableObject.CrossSectionalAttachDataSet)
                {
                    this.AddComponentToList(componentId, this._crossSectionalAttachDataSet);
                }
            }

            if (itemMutableObject.CrossSectionalAttachGroup != null)
            {
                foreach (string componentId0 in itemMutableObject.CrossSectionalAttachGroup)
                {
                    this.AddComponentToList(componentId0, this._crossSectionalAttachGroup);
                }
            }

            if (itemMutableObject.CrossSectionalAttachSection != null)
            {
                foreach (string componentId1 in itemMutableObject.CrossSectionalAttachSection)
                {
                    this.AddComponentToList(componentId1, this._crossSectionalAttachSection);
                }
            }

            if (itemMutableObject.CrossSectionalAttachObservation != null)
            {
                foreach (string componentId2 in itemMutableObject.CrossSectionalAttachObservation)
                {
                    this.AddComponentToList(componentId2, this._crossSectionalAttachObservation);
                }
            }

            if (itemMutableObject.AttributeToMeasureMap != null)
            {
                foreach (KeyValuePair<string, IList<string>> pair in itemMutableObject.AttributeToMeasureMap)
                {
                    this.SetAttributeMeasures(pair.Key, pair.Value);
                }
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
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossSectionalDataStructureObjectCore"/> class.
        /// </summary>
        /// <param name="keyFamilyType">
        /// The itemMutableObject.
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public CrossSectionalDataStructureObjectCore(KeyFamilyType keyFamilyType)
            : base(keyFamilyType)
        {
            this._crossSectionalMeasures = new List<ICrossSectionalMeasure>();
            this._crossSectionalAttachDataSet = new List<IComponent>();
            this._crossSectionalAttachGroup = new List<IComponent>();
            this._crossSectionalAttachSection = new List<IComponent>();
            this._crossSectionalAttachObservation = new List<IComponent>();
            this._attributeToMeasuresMap = new Dictionary<string, IList<ICrossSectionalMeasure>>();
            this._measureDimensions = new List<string>();
            this._codelistMap = new Dictionary<string, ICrossReference>();

            //// TODO java 0.9.9 bug. XS DSD may not have any XS Measures
            //// Reverted changes that were made during sync
            //// Please do not change unless you are sure.
            if (keyFamilyType.Components == null || !CrossSectionalUtil.IsCrossSectional(keyFamilyType))
            {
                throw new SdmxSemmanticException("Can not create ICrossSectionalDataStructureObject as there are no CrossSectional Measures defined");
            }

            foreach (CrossSectionalMeasureType xsMeasureType in keyFamilyType.Components.CrossSectionalMeasure)
            {
                this._crossSectionalMeasures.Add(new CrossSectionalMeasureCore(xsMeasureType, this));
            }

            foreach (DimensionType currentComponent in keyFamilyType.Components.Dimension)
            {
                string componentId = currentComponent.conceptRef;
                if (currentComponent.isMeasureDimension)
                {
                    this._measureDimensions.Add(componentId);
                    string codelistAgency = currentComponent.codelistAgency;
                    if (string.IsNullOrWhiteSpace(codelistAgency))
                    {
                        codelistAgency = this.BaseAgencyId;
                    }

                    ICrossReference codelistRef = new CrossReferenceImpl(
                        this, codelistAgency, currentComponent.codelist, currentComponent.codelistVersion, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList));
                    this._codelistMap.Add(componentId, codelistRef);
                }

                if (currentComponent.crossSectionalAttachDataSet != null && currentComponent.crossSectionalAttachDataSet.Value)
                {
                    this.AddComponentToList(componentId, this._crossSectionalAttachDataSet);
                }

                if (currentComponent.crossSectionalAttachGroup != null && currentComponent.crossSectionalAttachGroup.Value)
                {
                    this.AddComponentToList(componentId, this._crossSectionalAttachGroup);
                }

                if (currentComponent.crossSectionalAttachObservation != null && currentComponent.crossSectionalAttachObservation.Value)
                {
                    this.AddComponentToList(componentId, this._crossSectionalAttachObservation);
                }

                if (currentComponent.crossSectionalAttachSection != null && currentComponent.crossSectionalAttachSection.Value)
                {
                    this.AddComponentToList(componentId, this._crossSectionalAttachSection);
                }
            }

            foreach (AttributeType currentComponent0 in keyFamilyType.Components.Attribute)
            {
                string componentId1 = currentComponent0.conceptRef;

                if (currentComponent0.crossSectionalAttachDataSet != null && currentComponent0.crossSectionalAttachDataSet.Value)
                {
                    this.AddComponentToList(componentId1, this._crossSectionalAttachDataSet);
                }

                if (currentComponent0.crossSectionalAttachGroup != null && currentComponent0.crossSectionalAttachGroup.Value)
                {
                    this.AddComponentToList(componentId1, this._crossSectionalAttachGroup);
                }

                if (currentComponent0.crossSectionalAttachObservation != null && currentComponent0.crossSectionalAttachObservation.Value)
                {
                    this.AddComponentToList(componentId1, this._crossSectionalAttachObservation);
                }

                if (currentComponent0.crossSectionalAttachSection != null && currentComponent0.crossSectionalAttachSection.Value)
                {
                    this.AddComponentToList(componentId1, this._crossSectionalAttachSection);
                }

                this.SetAttributeMeasures(componentId1, currentComponent0.AttachmentMeasure);
            }

            if (keyFamilyType.Components.TimeDimension != null)
            {
                TimeDimensionType timeDimensionType = keyFamilyType.Components.TimeDimension;
                if (timeDimensionType.crossSectionalAttachDataSet != null && timeDimensionType.crossSectionalAttachDataSet.Value)
                {
                    this.AddComponentToList(DimensionObject.TimeDimensionFixedId, this._crossSectionalAttachDataSet);
                }

                if (timeDimensionType.crossSectionalAttachGroup != null && timeDimensionType.crossSectionalAttachGroup.Value)
                {
                    this.AddComponentToList(DimensionObject.TimeDimensionFixedId, this._crossSectionalAttachGroup);
                }

                if (timeDimensionType.crossSectionalAttachObservation != null && timeDimensionType.crossSectionalAttachObservation.Value)
                {
                    this.AddComponentToList(DimensionObject.TimeDimensionFixedId, this._crossSectionalAttachObservation);
                }

                if (timeDimensionType.crossSectionalAttachSection != null && timeDimensionType.crossSectionalAttachSection.Value)
                {
                    this.AddComponentToList(DimensionObject.TimeDimensionFixedId, this._crossSectionalAttachSection);
                }
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
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the cross sectional measures.
        /// </summary>
        public virtual IList<ICrossSectionalMeasure> CrossSectionalMeasures
        {
            get
            {
                return new List<ICrossSectionalMeasure>(this._crossSectionalMeasures);
            }
        }

        // public List<IDimension> getDimensions(SdmxStructureType... includeTypes) {
        // List<IDimension> reutrnList = super.getDimensions(includeTypes);
        // for(IDimension dimension : dimensionList.getDimensions()) {
        // if(includeTypes != null && includeTypes.length > 0) {
        // for(SdmxStructureType currentType : includeTypes) {
        // if(currentType == dimension.getStructureType()) {
        // returnList.add(dimension);
        // }
        // }
        // } else {
        // returnList.add(dimension);
        // }
        // }
        // return returnList;
        // return new ArrayList<IDimension>();
        // }

        // private void addDimensionToList(string componentId, List<IDimension> listToAddTo) {
        // IComponent component = getComponent(componentId);
        // if(component == null) {
        // throw new SdmxSemmanticException("Can not find referenced component with id " + componentId);
        // }
        // if(component instanceof IDimension) {
        // listToAddTo.add((IDimension)component);
        // } else {
        // throw new SdmxSemmanticException("Referneced component is not a dimension " + componentId);
        // }
        // }

        /// <summary>
        ///     Gets the mutable instance.
        /// </summary>
        public new ICrossSectionalDataStructureMutableObject MutableInstance
        {
            get
            {
                return new CrossSectionalDataStructureMutableCore(this);
            }
        }

        /// <summary>
        ///     Gets the components.
        /// </summary>
        public override IList<IComponent> Components
        {
            get
            {
                IList<IComponent> components = base.Components;
                components.AddAll(this.CrossSectionalMeasures);
                return components;
            }
        }


        #endregion

        #region Explicit Interface Properties

        /// <summary>
        ///     Gets the mutable instance.
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
        /// Get the cross sectional measures that the attribute is linked to, returns an empty list if there is no cross sectional measures
        ///     defined by the attribute.
        /// </summary>
        /// <param name="attributeObject">
        /// The attribute.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> .
        /// </returns>
        public virtual IList<ICrossSectionalMeasure> GetAttachmentMeasures(IAttributeObject attributeObject)
        {
            if (this._attributeToMeasuresMap.ContainsKey(attributeObject.Id))
            {
                return this._attributeToMeasuresMap[attributeObject.Id];
            }

            return new List<ICrossSectionalMeasure>();
        }

        /// <summary>
        /// Gets the codelist reference for the dimension with the given id
        /// </summary>
        /// <param name="dimensionId">
        /// The dimension Id.
        /// </param>
        /// <returns>
        /// The <see cref="ICrossReference"/> .
        /// </returns>
        public virtual ICrossReference GetCodelistForMeasureDimension(string dimensionId)
        {
            ICrossReference codelistRef;
            if (this._codelistMap.TryGetValue(dimensionId, out codelistRef))
            {
                return codelistRef;
            }

            return null;
        }

        /// <summary>
        /// The get cross sectional attach data set.
        /// </summary>
        /// <param name="returnOnlyIfLowestLevel">
        /// The return only if lowest level.
        /// </param>
        /// <param name="returnTypes">
        /// The return enum types.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> .
        /// </returns>
        public virtual IList<IComponent> GetCrossSectionalAttachDataSet(bool returnOnlyIfLowestLevel, params SdmxStructureType[] returnTypes)
        {
            IList<IComponent> returnList = GetComponets(this._crossSectionalAttachDataSet, returnTypes);
            if (returnOnlyIfLowestLevel)
            {
                returnList.RemoveItemList(this._crossSectionalAttachGroup);
                returnList.RemoveItemList(this._crossSectionalAttachSection);
                returnList.RemoveItemList(this._crossSectionalAttachObservation);
            }

            return returnList;
        }

        /// <summary>
        /// Returns the cross sectional attach group.
        /// </summary>
        /// <param name="returnOnlyIfLowestLevel">
        /// The return only if lowest level.
        /// </param>
        /// <param name="returnTypes">
        /// The return enum types.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> .
        /// </returns>
        public virtual IList<IComponent> GetCrossSectionalAttachGroup(bool returnOnlyIfLowestLevel, params SdmxStructureType[] returnTypes)
        {
            IList<IComponent> returnList = GetComponets(this._crossSectionalAttachGroup, returnTypes);
            if (returnOnlyIfLowestLevel)
            {
                returnList.RemoveItemList(this._crossSectionalAttachSection);
                returnList.RemoveItemList(this._crossSectionalAttachObservation);
            }

            return returnList;
        }

        /// <summary>
        /// The get cross sectional attach observation.
        /// </summary>
        /// <param name="returnTypes">
        /// The return types.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> .
        /// </returns>
        public virtual IList<IComponent> GetCrossSectionalAttachObservation(params SdmxStructureType[] returnTypes)
        {
            return GetComponets(this._crossSectionalAttachObservation, returnTypes);
        }

        /// <summary>
        /// The get cross sectional attach section.
        /// </summary>
        /// <param name="returnOnlyIfLowestLevel">
        /// The return only if lowest level.
        /// </param>
        /// <param name="returnTypes">
        /// The return types.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> .
        /// </returns>
        public virtual IList<IComponent> GetCrossSectionalAttachSection(bool returnOnlyIfLowestLevel, params SdmxStructureType[] returnTypes)
        {
            IList<IComponent> returnList = GetComponets(this._crossSectionalAttachSection, returnTypes);
            if (returnOnlyIfLowestLevel)
            {
                returnList.RemoveItemList(this._crossSectionalAttachObservation);
            }

            return returnList;
        }

        /// <summary>
        /// The get cross sectional measure.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ICrossSectionalMeasure"/> .
        /// </returns>
        public virtual ICrossSectionalMeasure GetCrossSectionalMeasure(string id)
        {
            foreach (ICrossSectionalMeasure currentMeasure in this._crossSectionalMeasures)
            {
                if (currentMeasure.Id.Equals(id) || string.Equals(currentMeasure.Code, id))
                {
                    return currentMeasure;
                }
            }

            return null;
        }

        /// <summary>
        /// The is measure dimension.
        /// </summary>
        /// <param name="dimension">
        /// The dimension.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        public virtual bool IsMeasureDimension(IDimension dimension)
        {
            return this._measureDimensions.Contains(dimension.Id);
        }

        #endregion

        #region Explicit Interface Methods

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
        /// The <see cref="IMaintainableObject"/> .
        /// </returns>
        IMaintainableObject IMaintainableObject.GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return this.GetStub(actualLocation, isServiceUrl);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get componets.
        /// </summary>
        /// <param name="listToGetFrom">
        /// The list to get from.
        /// </param>
        /// <param name="returnTypes">
        /// The return types.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> .
        /// </returns>
        private static IList<IComponent> GetComponets(IEnumerable<IComponent> listToGetFrom, params SdmxStructureType[] returnTypes)
        {
            IList<IComponent> returnList = new List<IComponent>();

            foreach (IComponent currentComponent in listToGetFrom)
            {
                if (IsValidReturnType(currentComponent, returnTypes))
                {
                    returnList.Add(currentComponent);
                }
            }

            return returnList;
        }

        /// <summary>
        /// The is valid return type.
        /// </summary>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <param name="returnTypes">
        /// The return types.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        private static bool IsValidReturnType(ISdmxObject component, params SdmxStructureType[] returnTypes)
        {
            if (returnTypes == null || returnTypes.Length == 0)
            {
                return true;
            }

            foreach (SdmxStructureType currentRetrunType in returnTypes)
            {
                if (component.StructureType == currentRetrunType)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The add component to list.
        /// </summary>
        /// <param name="componentId">
        /// The component id.
        /// </param>
        /// <param name="listToAddTo">
        /// The list to add to.
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        private void AddComponentToList(string componentId, ICollection<IComponent> listToAddTo)
        {
            // TODO BUG in Java 0.9.4. It assumes that component id == concept ref always
            IComponent component = this.GetComponent(componentId);
            if (component == null)
            {
                throw new SdmxSemmanticException("Can not find referenced component with id " + componentId);
            }

            listToAddTo.Add(component);
        }

        /// <summary>
        /// The set attributeObject measures.
        /// </summary>
        /// <param name="attributeId">
        /// The attributeObject id.
        /// </param>
        /// <param name="measureIds">
        /// The measure ids.
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        private void SetAttributeMeasures(string attributeId, ICollection<string> measureIds)
        {
            if (!ObjectUtil.ValidCollection(measureIds))
            {
                return;
            }

            try
            {
                // TODO BUG in Java 0.9.4. It assumes that component id == concept ref always
                IComponent component = this.GetComponent(attributeId);
                if (component == null)
                {
                    throw new SdmxSemmanticException("Could not resolve reference to attributeObject with id '" + attributeId + "' " + "referenced from cross sectional data structure");
                }

                var attribute = component as IAttributeObject;
                if (attribute != null)
                {
                    IAttributeObject att = attribute;
                    if (att.AttachmentLevel != AttributeAttachmentLevel.Observation)
                    {
                        var sb = new StringBuilder();
                        sb.Append("Attribute '");
                        sb.Append(attributeId);
                        sb.Append("' is referencing cross sectional measure, the attributeObject ");
                        sb.Append("must have an attachment level of Observation, it is currently set to '");
                        sb.Append(att.AttachmentLevel);
                        sb.Append("'");

                        throw new SdmxSemmanticException(sb.ToString());
                    }
                }
                else
                {
                    throw new SdmxSemmanticException(
                        "Cross Sectional Measure attributeObject reference id '" + attributeId + "' " + "is referencing structure of type '" + component.StructureType.StructureType + "'");
                }

                IList<ICrossSectionalMeasure> measureList = new List<ICrossSectionalMeasure>();

                foreach (string measureId in measureIds)
                {
                    if (measureId == null)
                    {
                        continue;
                    }

                    ICrossSectionalMeasure crossSectionalMeasure = this.GetCrossSectionalMeasure(measureId);
                    if (crossSectionalMeasure == null)
                    {
                        throw new SdmxSemmanticException("Could not resolve reference to cross sectional measure with id '" + measureId + "' " + "referenced from attributeObject '" + attributeId + "'");
                    }

                    measureList.Add(crossSectionalMeasure);
                }

                this._attributeToMeasuresMap.Add(attributeId, measureList);
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATE                              //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///     The validate.
        /// </summary>
        private void Validate()
        {
            //// Reverted changes that were made during sync
            //// Please do not change unless you are sure.
             ISet<IComponent> crossSectionalAttached = new HashSet<IComponent>(this._crossSectionalAttachDataSet);
            crossSectionalAttached.UnionWith(this._crossSectionalAttachGroup);
            crossSectionalAttached.UnionWith(this._crossSectionalAttachSection);
            crossSectionalAttached.UnionWith(this._crossSectionalAttachObservation);

            var dimensions = this.GetDimensions(SdmxStructureEnumType.Dimension);
            foreach (IDimension dimension in dimensions)
            {
                if (!dimension.FrequencyDimension && !crossSectionalAttached.Contains(dimension))
                {
                    throw new SdmxSemmanticException(string.Format(CultureInfo.InvariantCulture, "Can not create Cross Sectional Data Structure, dimension '{0}' doesn't have cross sectional attachment level.", dimension.Id));
                }
            }

            var attributes = this.Attributes;
            foreach (IAttributeObject attribute in attributes)
            {
                if (!crossSectionalAttached.Contains(attribute))
                {
                    throw new SdmxSemmanticException(string.Format(CultureInfo.InvariantCulture, "Can not create Cross Sectional Data Structure, attribute '{0}' doesn't have cross sectional attachment level.", attribute.Id));
                }
            }
        }

       	///////////////////////////////////////////////////////////////////////////////////////////////////
	    ////////////COMPOSITES				 //////////////////////////////////////////////////
	    ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///   The get composites internal.
        /// </summary>
        protected override ISet<ISdmxObject> GetCompositesInternal()
        {
            ISet<ISdmxObject> composites = base.GetCompositesInternal();
            base.AddToCompositeSet(this._crossSectionalAttachDataSet, composites);
            base.AddToCompositeSet(this._crossSectionalAttachGroup, composites);
            base.AddToCompositeSet(this._crossSectionalAttachSection, composites);
            base.AddToCompositeSet(this._crossSectionalAttachObservation, composites);
            base.AddToCompositeSet(this._crossSectionalMeasures, composites);
            return composites;
        }

        #endregion
    }
}