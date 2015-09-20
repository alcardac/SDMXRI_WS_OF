// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyValuesMutableImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The key values mutable impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    ///   The key values mutable impl.
    /// </summary>
    [Serializable]
    public class KeyValuesMutableImpl : MutableCore, IKeyValuesMutable
    {
        #region Fields

        /// <summary>
        ///   The itime range.
        /// </summary>
        private ITimeRangeMutableObject timeRange;

        /// <summary>
        ///   The cascade list.
        /// </summary>
        private IList<string> cascadeList;

        /// <summary>
        ///   The id.
        /// </summary>
        private string id;

        /// <summary>
        ///   The values.
        /// </summary>
        private IList<string> values;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="KeyValuesMutableImpl" /> class.
        /// </summary>
        public KeyValuesMutableImpl()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.KeyValues))
        {
            this.values = new List<string>();
            this.cascadeList = new List<string>();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM IMMUTABLE OBJECT                 //////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValuesMutableImpl"/> class.
        /// </summary>
        /// <param name="immutable">
        /// The immutable. 
        /// </param>
        public KeyValuesMutableImpl(IKeyValues immutable)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.KeyValues))
        {
            this.values = new List<string>(immutable.Values);
            this.cascadeList = new List<string>();
            this.id = immutable.Id;
            
            foreach (string each in this.values)
            {
                if (immutable.IsCascadeValue(each))
                {
                    this.cascadeList.Add(each);
                }
            }

            if (immutable.TimeRange != null)
            {
                this.timeRange = new TimeRangeMutableCore(immutable.TimeRange);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValuesMutableImpl"/> class.
        /// </summary>
        /// <param name="keyValueType">
        /// The key value type. 
        /// </param>
        public KeyValuesMutableImpl(ComponentValueSetType keyValueType)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.KeyValues))
        {
            this.values = new List<string>();
            this.cascadeList = new List<string>();

            this.id = keyValueType.id;

            if (keyValueType.Value != null)
            {
                foreach (SimpleValueType dataKeyType in keyValueType.Value)
                {
                    this.values.Add(dataKeyType.TypedValue);
                    if (dataKeyType.cascadeValues)
                    {
                        this.cascadeList.Add(dataKeyType.TypedValue);
                    }
                }
            }

            if (keyValueType.TimeRange != null)
            {
                this.timeRange = new TimeRangeMutableCore(keyValueType.TimeRange);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the cascade.
        /// </summary>
        public virtual IList<string> Cascade
        {
            get
            {
                return this.cascadeList;
            }
        }

        /// <summary>
        ///   Gets or sets the id.
        /// </summary>
        public virtual string Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        /// <summary>
        ///   Gets the key values.
        /// </summary>
        public virtual IList<string> KeyValues
        {
            get
            {
                return this.values;
            }
        }

        /// <summary>
        ///   Gets or sets the time range.
        /// </summary>
        public virtual ITimeRangeMutableObject TimeRange
        {
            get
            {
                return this.timeRange;
            }

            set
            {
                this.timeRange = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add cascade.
        /// </summary>
        /// <param name="value">
        /// The value. 
        /// </param>
        public virtual void AddCascade(string value)
        {
            if (this.cascadeList == null)
            {
                this.cascadeList = new List<string>();
            }

            this.cascadeList.Add(value);
        }

        /// <summary>
        /// The add value.
        /// </summary>
        /// <param name="value">
        /// The value. 
        /// </param>
        public virtual void AddValue(string value)
        {
            if (this.values == null)
            {
                this.values = new List<string>();
            }

            if (value != null)
            {
                this.values.Add(value);
            }
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
            return this.cascadeList.Contains(value);
        }

        #endregion
    }
}