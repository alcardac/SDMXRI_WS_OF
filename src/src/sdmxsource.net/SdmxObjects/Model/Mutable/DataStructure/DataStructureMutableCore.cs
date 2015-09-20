// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataStructureMutableCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data structure mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure;

    /// <summary>
    ///   The data structure mutable core.
    /// </summary>
    [Serializable]
    public class DataStructureMutableCore : MaintainableMutableCore<IDataStructureObject>, IDataStructureMutableObject
    {
        #region Fields

        /// <summary>
        ///   The _groups.
        /// </summary>
        private readonly IList<IGroupMutableObject> _groups;

        /// <summary>
        ///   The _attribtue list.
        /// </summary>
        private IAttributeListMutableObject _attributeList;

        /// <summary>
        ///   The _dimension list.
        /// </summary>
        private IDimensionListMutableObject _dimensionList;

        /// <summary>
        ///   The _measure list.
        /// </summary>
        private IMeasureListMutableObject _measureList;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="DataStructureMutableCore" /> class.
        /// </summary>
        public DataStructureMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd))
        {
            this._groups = new List<IGroupMutableObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStructureMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The source immutable object
        /// </param>
        public DataStructureMutableCore(IDataStructureObject objTarget)
            : base(objTarget)
        {
            this._groups = new List<IGroupMutableObject>();
            if (objTarget.Groups != null)
            {
                foreach (IGroup group in objTarget.Groups)
                {
                    this._groups.Add(new GroupMutableCore(group));
                }
            }

            if (objTarget.DimensionList != null)
            {
                this._dimensionList = new DimensionListMutableCore(objTarget.DimensionList);
            }

            if (objTarget.AttributeList != null)
            {
                this._attributeList = new AttributeListMutableCore(objTarget.AttributeList);
            }

            if (objTarget.MeasureList != null)
            {
                this._measureList = new MeasureListMutableCore(objTarget.MeasureList);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the attribute list.
        /// </summary>
        public virtual IAttributeListMutableObject AttributeList
        {
            get
            {
                return this._attributeList;
            }

            set
            {
                this._attributeList = value;
            }
        }

        /// <summary>
        ///   Gets or sets the dimension list.
        /// </summary>
        public virtual IDimensionListMutableObject DimensionList
        {
            get
            {
                return this._dimensionList;
            }

            set
            {
                this._dimensionList = value;
            }
        }

        /// <summary>
        ///   Gets the dimensions.
        /// </summary>
        public virtual IList<IDimensionMutableObject> Dimensions
        {
            get
            {
                if (this._dimensionList != null)
                {
                    return this._dimensionList.Dimensions;
                }

                return null;
            }
        }

        /// <summary>
        ///   Gets the groups.
        /// </summary>
        public virtual IList<IGroupMutableObject> Groups
        {
            get
            {
                return this._groups;
            }
        }

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override IDataStructureObject ImmutableInstance
        {
            get
            {
                return new DataStructureObjectCore(this);
            }
        }

        /// <summary>
        ///   Gets or sets the measure list.
        /// </summary>
        public virtual IMeasureListMutableObject MeasureList
        {
            get
            {
                return this._measureList;
            }

            set
            {
                this._measureList = value;
            }
        }

        public virtual IList<IAttributeMutableObject> Attributes
        {
            get
            {
                if (this._attributeList != null)
                {
                    return this._attributeList.Attributes;
                }
                return null;
            }
        }

        /// <summary>
        ///  Gets or sets the primary measure.
        /// </summary>
        public virtual IPrimaryMeasureMutableObject PrimaryMeasure
        {
            get
            {
                var measureListMutableObject = this._measureList;
                if (measureListMutableObject != null)
                {
                    return measureListMutableObject.PrimaryMeasure;
                }

                return null;
            }

            set
            {
                if (this._measureList == null)
                {
                    this._measureList = new MeasureListMutableCore();
                }

                this._measureList.PrimaryMeasure = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add attribute.
        /// </summary>
        /// <param name="attribute">
        /// The attribute. 
        /// </param>
        public virtual void AddAttribute(IAttributeMutableObject attribute)
        {
            if (this._attributeList == null)
            {
                this._attributeList = new AttributeListMutableCore();
            }

            this._attributeList.AddAttribute(attribute);
        }

        public virtual IDataStructureMutableObject RemoveComponent(string id)
        {
            RemoveComponent (this.Dimensions, id);
            RemoveComponent (this.Attributes, id);
		     return this;
	    }
	
	
	    public virtual IDimensionMutableObject GetDimension(string id) 
        {
		     return GetComponentById(this.Dimensions, id);
	    }

	    public  virtual IAttributeMutableObject GetAttribute(string id) 
        {
		     return GetComponentById(this.Attributes, id);
	    }
	
	    private void RemoveComponent<T>(IList<T> comps, string removeId) where T :IComponentMutableObject
        {
		    if(comps == null) 
            {
			   return;
		    }
		    T toRemove = GetComponentById(comps, removeId);
            if (!EqualityComparer<T>.Default.Equals(toRemove, default(T)))
            {
			   comps.Remove(toRemove);
		    }
	    }
	
	    private T GetComponentById<T>(IList<T> comps, string removeId) where T: IComponentMutableObject
        {
		     if(comps == null)
             {
			    return default(T);
		     }
		     foreach(T currentComponent in comps)
             {
			     if(currentComponent.Id != null && currentComponent.Id.Equals(removeId))
                 {
				   return currentComponent;
			     }
		     }

		     return default(T);
	    }
	


        /// <summary>
        /// The add dimension.
        /// </summary>
        /// <param name="dimension">
        /// The dimension. 
        /// </param>
        public virtual void AddDimension(IDimensionMutableObject dimension)
        {
            if (this._dimensionList == null)
            {
                this._dimensionList = new DimensionListMutableCore();
            }

            this._dimensionList.AddDimension(dimension);
        }

        public IDimensionMutableObject AddDimension(IStructureReference conceptRef, IStructureReference codelistRef)
        {
            IDimensionMutableObject newDimension = new DimensionMutableCore();
            newDimension.ConceptRef = conceptRef;
            if (codelistRef != null)
            {
                IRepresentationMutableObject representation = new RepresentationMutableCore();
                representation.Representation = codelistRef;
                newDimension.Representation = representation;
            }
            this.AddDimension(newDimension);
            return newDimension;
        }

        public IAttributeMutableObject AddAttribute(IStructureReference conceptRef, IStructureReference codelistRef)
        {
            IAttributeMutableObject newAttribute = new AttributeMutableCore();
            newAttribute.ConceptRef = conceptRef;
            if (codelistRef != null)
            {
                IRepresentationMutableObject representation = new RepresentationMutableCore();
                representation.Representation = codelistRef;
                newAttribute.Representation = representation;
            }
            this.AddAttribute(newAttribute);
            return newAttribute;
        }

        public IPrimaryMeasureMutableObject AddPrimaryMeasure(IStructureReference conceptRef)
        {
            IPrimaryMeasureMutableObject primaryMeasure = new PrimaryMeasureMutableCore();
            primaryMeasure.ConceptRef = conceptRef;
            PrimaryMeasure = primaryMeasure;
            return primaryMeasure;
        }

        /// <summary>
        /// The add group.
        /// </summary>
        /// <param name="group">
        /// The group. 
        /// </param>
        public virtual void AddGroup(IGroupMutableObject group)
        {
            this._groups.Add(group);
        }

        #endregion
    }
}