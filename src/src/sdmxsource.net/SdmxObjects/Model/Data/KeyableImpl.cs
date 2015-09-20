// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyableImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The keyable impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Util.Date;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///   The keyable impl.
    /// </summary>
    public class KeyableImpl : IKeyable
    {
        #region Fields

        private IDataStructureObject _dataStructure;
        private IDataflowObject _dataflow;

        /// <summary>
        ///   The _attribute map.
        /// </summary>
        private readonly IDictionary<string, IKeyValue> _attributeMap;

        /// <summary>
        ///   The _attributes.
        /// </summary>
        private readonly IList<IKeyValue> _attributes;

        /// <summary>
        ///   The _cross section concept.
        /// </summary>
        private readonly string _crossSectionConcept;

        /// <summary>
        ///   The _group name.
        /// </summary>
        private readonly string _groupName;

        /// <summary>
        ///   The _is time series.
        /// </summary>
        private readonly bool _isTimeSeries;

        /// <summary>
        ///   The _key.
        /// </summary>
        private readonly IList<IKeyValue> _key;

        /// <summary>
        ///   The _key map.
        /// </summary>
        private readonly IDictionary<string, string> _keyMap;

        /// <summary>
        ///   The _annotations.
        /// </summary>
        private  readonly IList<IAnnotation> _annotations ;

        /// <summary>
        ///   The _obs time.
        /// </summary>
        private readonly string _obsTime;

        /// <summary>
        ///   The _series.
        /// </summary>
        private readonly bool _series;

        /// <summary>
        ///   The _time format.
        /// </summary>
        private readonly TimeFormat _timeFormat;

        /// <summary>
        ///   The _date.
        /// </summary>
        private DateTime? _date;

        /// <summary>
        ///   The _shot code.
        /// </summary>
        private string _shotCode;

        /// <summary>
        ///   The _unique id.
        /// </summary>
        private string _uniqueId; // Generated on equals/hashcode

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyableImpl"/> class.
        /// </summary>
        /// <param name="key">
        /// The key 0. 
        /// </param>
        /// <param name="attributes">
        /// The attributes 1. 
        /// </param>
        /// <param name="timeFormat">
        /// The time format 2. 
        /// </param>
        /// <param name="crossSectionConcept">
        /// The cross section concept 3. 
        /// </param>
        /// <param name="obsTime">
        /// The obs time 4. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public KeyableImpl(
            IDataflowObject dataflowObject,
            IDataStructureObject dataStructureObject,
            IList<IKeyValue> key, IList<IKeyValue> attributes,
            TimeFormat timeFormat,
            string crossSectionConcept,
            string obsTime, params IAnnotation[] annotations)
            : this(dataflowObject, dataStructureObject, key, attributes, null, timeFormat, annotations)
        {
            this._isTimeSeries = false;
            this._crossSectionConcept = crossSectionConcept;
            this._obsTime = obsTime;
            if (obsTime == null)
            {
                throw new SdmxSemmanticException("Cross sectional dataset missing time value for key : " + this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyableImpl"/> class.
        /// </summary>
        /// <param name="key">
        /// The key 0. 
        /// </param>
        /// <param name="attributes">
        /// The attributes 1. 
        /// </param>
        /// <param name="timeFormat">
        /// The time format 2. 
        /// </param>
        public KeyableImpl(IDataflowObject dataflowObject,
            IDataStructureObject dataStructureObject, IList<IKeyValue> key, IList<IKeyValue> attributes, TimeFormat timeFormat
            ,params IAnnotation[] annotations)
            : this(dataflowObject, dataStructureObject, key, attributes, null, timeFormat, annotations)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyableImpl"/> class.
        /// </summary>
        /// <param name="key">
        /// The key 0. 
        /// </param>
        /// <param name="attributes">
        /// The attributes 1. 
        /// </param>
        /// <param name="groupName">
        /// The group name 2. 
        /// </param>
        public KeyableImpl(IDataflowObject dataflowObject,
            IDataStructureObject dataStructureObject, IList<IKeyValue> key, IList<IKeyValue> attributes, string groupName
            ,params IAnnotation[] annotations)
            : this(dataflowObject, dataStructureObject, key, attributes, groupName, default(TimeFormat), annotations)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyableImpl" /> class.
        /// </summary>
        /// <param name="dataflowObject">The dataflow object.</param>
        /// <param name="dataStructureObject">The data structure object.</param>
        /// <param name="key">The key .</param>
        /// <param name="attributes">The attributes.</param>
        /// <exception cref="System.ArgumentException">Data Structure can not be null</exception>
        public KeyableImpl(IDataflowObject dataflowObject, IDataStructureObject dataStructureObject, IList<IKeyValue> key, IList<IKeyValue> attributes)
            : this(dataflowObject, dataStructureObject, key, attributes: attributes, groupName: null, timeFormat: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyableImpl" /> class.
        /// </summary>
        /// <param name="dataflowObject">The dataflow object.</param>
        /// <param name="dataStructureObject">The data structure object.</param>
        /// <param name="key">The key .</param>
        /// <param name="attributes">The attributes .</param>
        /// <param name="groupName">The group name .</param>
        /// <param name="timeFormat">The time format .</param>
        /// <param name="annotations">The annotations.</param>
        /// <exception cref="System.ArgumentException">Data Structure can not be null</exception>
        public KeyableImpl(IDataflowObject dataflowObject,
            IDataStructureObject dataStructureObject,
            IList<IKeyValue> key, IList<IKeyValue> attributes, string groupName, TimeFormat timeFormat
            ,params IAnnotation[] annotations)
        {
            this._dataStructure = dataStructureObject;
            this._dataflow = dataflowObject;
            this._attributes = new List<IKeyValue>();
            this._key = new List<IKeyValue>();
            this._attributeMap = new Dictionary<string, IKeyValue>();
            this._keyMap = new Dictionary<string, string>();
            this._annotations = new List<IAnnotation>();
            this._isTimeSeries = true;

            if (_dataStructure == null)
            {
                throw new ArgumentException("Data Structure can not be null");
            }

            this._series = string.IsNullOrWhiteSpace(groupName);

            if (attributes != null)
            {
                this._attributes = new List<IKeyValue>(attributes);

                foreach (IKeyValue currentKv in attributes)
                {
                    this._attributeMap.Add(currentKv.Concept, currentKv);
                }
            }

            if (key != null)
            {
                this._key = new List<IKeyValue>(key);
                foreach (IKeyValue currentKv4 in key)
                {
                    this._keyMap.Add(currentKv4.Concept, currentKv4.Code);
                }
            }

            if(annotations != null)
            {
			   foreach(IAnnotation currentAnnotation in annotations) 
               {
				  this._annotations.Add(currentAnnotation);
			   }
		    }

            this._groupName = groupName;
            this._timeFormat = timeFormat;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the attributes.
        /// </summary>
        public virtual IList<IKeyValue> Attributes
        {
            get
            {
                return new List<IKeyValue>(this._attributes);
            }
        }

        /// <summary>
        ///   Gets a value indicating annotations.
        /// </summary>
        public virtual IList<IAnnotation> Annotations
        {
            get
            {
                return new List<IAnnotation>(_annotations);
            }
        }

        public virtual IDataStructureObject DataStructure
        {
            get
            {
                return _dataStructure;
            }
        }

        public IDataflowObject Dataflow
        {
            get
            {
                return _dataflow;
            }
        }

        /// <summary>
        ///   Gets the cross section concept.
        /// </summary>
        public virtual string CrossSectionConcept
        {
            get
            {
                return this._crossSectionConcept;
            }
        }

        /// <summary>
        ///   Gets the group name.
        /// </summary>
        public virtual string GroupName
        {
            get
            {
                return this._groupName;
            }
        }

        /// <summary>
        ///   Gets the key.
        /// </summary>
        public virtual IList<IKeyValue> Key
        {
            get
            {
                return new List<IKeyValue>(this._key);
            }
        }

        /// <summary>
        ///   Gets the obs as time date.
        /// </summary>
        public virtual DateTime? ObsAsTimeDate
        {
            get
            {
                if (this._isTimeSeries)
                {
                    return null;
                }

                return this._date ?? (this._date = DateUtil.FormatDate(this._obsTime));
            }
        }

        /// <summary>
        ///   Gets the obs time.
        /// </summary>
        public virtual string ObsTime
        {
            get
            {
                return this._obsTime;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether series.
        /// </summary>
        public virtual bool Series
        {
            get
            {
                return this._series;
            }
        }

        /// <summary>
        ///   Gets the short code.
        /// </summary>
        public virtual string ShortCode
        {
            get
            {
                if (this._shotCode == null)
                {
                    this.GenerateUniqueId();
                }

                return this._shotCode;
            }
        }

        /// <summary>
        ///   Gets the time format.
        /// </summary>
        public virtual TimeFormat TimeFormat
        {
            get
            {
                return this._timeFormat;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether time series.
        /// </summary>
        public virtual bool TimeSeries
        {
            get
            {
                return this._isTimeSeries;
            }
        }

      
        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }

            if (obj is KeyableImpl)
            {
                if (this._uniqueId == null)
                {
                    this.GenerateUniqueId();
                }

                var that = (KeyableImpl)obj;
                if (that._uniqueId == null)
                {
                    that.GenerateUniqueId();
                }

                return this._uniqueId.Equals(that._uniqueId);
            }

            return false;
        }

        /// <summary>
        /// The get attribute.
        /// </summary>
        /// <param name="concept">
        /// The concept. 
        /// </param>
        /// <returns>
        /// The <see cref="IKeyValue"/> . 
        /// </returns>
        public virtual IKeyValue GetAttribute(string concept)
        {
            return this._attributeMap[concept];
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The <see cref="int" /> . </returns>
        public override int GetHashCode()
        {
            if (this._uniqueId == null)
            {
                this.GenerateUniqueId();
            }

            return this._uniqueId.GetHashCode();
        }

        /// <summary>
        /// The get key value.
        /// </summary>
        /// <param name="dimensionId">
        /// The dimension id. 
        /// </param>
        /// <returns>
        /// The <see cref="string"/> . 
        /// </returns>
        public virtual string GetKeyValue(string dimensionId)
        {
            string value;
            if (this._keyMap.TryGetValue(dimensionId, out value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        ///   The to string.
        /// </summary>
        /// <returns> The <see cref="string" /> . </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (this._series)
            {
                sb.Append("series  ");
            }
            else
            {
                sb.Append("Group  " + this._groupName + "  ");
            }

            string concat = string.Empty;

            foreach (IKeyValue kv in this._key)
            {
                sb.Append(concat);
                sb.Append(kv.Concept);
                sb.Append(":");
                sb.Append(kv.Code);
                concat = ",";
            }

            if (!this._isTimeSeries)
            {
                sb.Append(concat);
                sb.Append(this.ObsTime);
            }

            return sb.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The generate unique id.
        /// </summary>
        private void GenerateUniqueId()
        {
            this._shotCode = string.Empty;
            string concat = string.Empty;
            var sb = new StringBuilder();

            foreach (IKeyValue kv in this.Key)
            {
                this._shotCode += concat + kv.Code;
                concat = ":";
                sb.Append(kv.Concept + concat + kv.Code);
            }

            foreach (IKeyValue kv0 in this.Attributes)
            {
                sb.Append(kv0.Concept + concat + kv0.Code);
            }

            sb.Append(this._series);
            if (this._groupName != null)
            {
                sb.Append(this._groupName);
            }

            this._uniqueId = sb.ToString();
        }

        #endregion
    }
}