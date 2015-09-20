// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyValuesCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The key values impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    ///   The key values impl.
    /// </summary>
    [Serializable]
    public class KeyValuesCore : SdmxStructureCore, IKeyValues
    {
        #region Fields

        /// <summary>
        ///   The itime range.
        /// </summary>
        private readonly ITimeRange timeRange;

        /// <summary>
        ///   The case cade list.
        /// </summary>
        private readonly IList<string> caseCadeList;

        /// <summary>
        ///   The id.
        /// </summary>
        private readonly string id;

        /// <summary>
        ///   The values.
        /// </summary>
        private readonly List<string> values;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValuesCore"/> class.
        /// </summary>
        /// <param name="mutable">
        /// The mutable. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public KeyValuesCore(IKeyValuesMutable mutable, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.KeyValues), parent)
        {
            this.values = new List<string>();
            this.caseCadeList = new List<string>();
            this.id = mutable.Id;
            this.values.AddRange(mutable.KeyValues);

            foreach (string value in this.values)
            {
                if (mutable.IsCascadeValue(value))
                {
                    this.caseCadeList.Add(value);
                }
            }

            if (mutable.TimeRange != null)
            {
                this.timeRange = new TimeRangeCore(mutable.TimeRange, this);
            }
            Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValuesCore"/> class.
        /// </summary>
        /// <param name="keyValueType">
        /// The key value type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public KeyValuesCore(ComponentValueSetType keyValueType, ISdmxStructure parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.KeyValues), parent)
        {
            this.values = new List<string>();
            this.caseCadeList = new List<string>();

            this.id = keyValueType.id;

            if (keyValueType.Value != null)
            {
                foreach (SimpleValueType dataKeyType in keyValueType.Value)
                {
                    this.values.Add(dataKeyType.TypedValue);
                    if (dataKeyType.cascadeValues)
                    {
                        this.caseCadeList.Add(dataKeyType.TypedValue);
                    }
                }
            }

            if (keyValueType.TimeRange != null)
            {
                this.timeRange = new TimeRangeCore(keyValueType.TimeRange, this);
            }
            Validate();
        }

        private void Validate()
        {
            if(!ObjectUtil.ValidString(id))
                throw new SdmxSemmanticException("KeyValues requires an id");
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the id.
        /// </summary>
        public virtual string Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        ///   Gets the time range.
        /// </summary>
        public virtual ITimeRange TimeRange
        {
            get
            {
                return this.timeRange;
            }
        }

        /// <summary>
        ///   Gets the values.
        /// </summary>
        public virtual IList<string> Values
        {
            get
            {
                return new List<string>(this.values);
            }
        }

        /// <summary>
        /// Get cascade values.
        /// </summary>
        public virtual IList<string> CascadeValues
        {
            get
            {
                return new List<string>(caseCadeList);
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
                var that = (IKeyValues)sdmxObject;
                if (!ObjectUtil.EquivalentCollection(this.values, that.Values))
                {
                    return false;
                }

                foreach (string currentValue in this.values)
                {
                    if (that.IsCascadeValue(currentValue) != this.IsCascadeValue(currentValue))
                    {
                        return false;
                    }
                }

                if (!ObjectUtil.Equivalent(this.id, that.Id))
                {
                    return false;
                }

                if (!this.Equivalent(this.timeRange, that.TimeRange, includeFinalProperties))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// The is cascade value.
        /// </summary>
        /// <param name="value">
        /// The value. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public virtual bool IsCascadeValue(string value)
        {
            return this.caseCadeList.Contains(value);
        }

        #endregion

        #region Methods

       ///////////////////////////////////////////////////////////////////////////////////////////////////
       ////////////COMPOSITES		                     //////////////////////////////////////////////////
       ///////////////////////////////////////////////////////////////////////////////////////////////////

       /// <summary>
       /// The get composites internal.
       /// </summary>
       protected override ISet<ISdmxObject> GetCompositesInternal() 
       {
    	   ISet<ISdmxObject> composites = base.GetCompositesInternal();
           base.AddToCompositeSet(this.timeRange, composites);
           return composites;
       }

        #endregion
    }
}