// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservationImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The observation impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Util.Date;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///   The observation impl.
    /// </summary>
    public class ObservationImpl : IObservation
    {
        #region Fields

        private IKeyable _seriesKey;

        /// <summary>
        ///   The attribute map.
        /// </summary>
        private readonly IDictionary<string, IKeyValue> attributeMap = new Dictionary<string, IKeyValue>(StringComparer.Ordinal);

        /// <summary>
        ///   The attributes.
        /// </summary>
        private readonly IList<IKeyValue> attributes = new List<IKeyValue>();

        /// <summary>
        ///   The cross section value.
        /// </summary>
        private readonly IKeyValue crossSectionValue;

        /// <summary>
        ///   The is cross section.
        /// </summary>
        private readonly bool isCrossSection;

        /// <summary>
        ///   The obs time.
        /// </summary>
        private readonly string obsTime;

        /// <summary>
        ///   The obs value.
        /// </summary>
        private readonly string obsValue;

        /// <summary>
        ///   The date.
        /// </summary>
        private DateTime? date;

        /// <summary>
        ///   The time format.
        /// </summary>
        private TimeFormat timeFormat;

        /// <summary>
        ///   The _annotations.
        /// </summary>
        private readonly IList<IAnnotation> annotations = new List<IAnnotation>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservationImpl"/> class.
        /// </summary>
        /// <param name="obsTime">
        /// The obs time 0. 
        /// </param>
        /// <param name="obsValue">
        /// The obs value 1. 
        /// </param>
        /// <param name="attributes">
        /// The attributes 2. 
        /// </param>
        /// <param name="crossSectionValue">
        /// The cross section value 3. 
        /// </param>
        /// <param name="annotations">
        /// The cross section value 3. 
        /// </param>
        public ObservationImpl(IKeyable seriesKey,
            string obsTime, string obsValue, IList<IKeyValue> attributes, IKeyValue crossSectionValue, 
            params IAnnotation[] annotations)
        {
            this._seriesKey = seriesKey;
            this.obsValue = obsValue;
            this.obsTime = obsTime;

            if (seriesKey == null)
                throw new ArgumentException("Series Key can not be null");

            if (attributes != null)
            {
                this.attributes = new List<IKeyValue>(attributes);

                foreach (IKeyValue currentKv in attributes)
                {
                    this.attributeMap.Add(currentKv.Concept, currentKv);
                }
            }

            if(annotations != null) 
            {
			   foreach(IAnnotation currentAnnotation in annotations)
               {
				  this.annotations.Add(currentAnnotation);
			   }
		    }

            this.crossSectionValue = crossSectionValue;
            this.isCrossSection = crossSectionValue != null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservationImpl"/> class.
        /// </summary>
        /// <param name="obsTime">
        /// The obs time 0. 
        /// </param>
        /// <param name="obsValue">
        /// The obs value 1. 
        /// </param>
        /// <param name="attributes">
        /// The attributes 2. 
        /// </param>
        public ObservationImpl(IKeyable seriesKey, string obsTime, string obsValue, IList<IKeyValue> attributes
            ,params IAnnotation[] annotations)
        {
            this.obsValue = obsValue;
            this._seriesKey = seriesKey;

            if (seriesKey == null)
            {
                throw new ArgumentException("Series Key can not be null");
            }
            if (!ObjectUtil.ValidString(obsTime))
            {
                if (seriesKey.TimeSeries)
                {
                    throw new ArgumentException(
                        "Observation for Key '" + seriesKey + "' does not specify the observation time");
                }
                throw new ArgumentException(
                    "Observation for Key '" + seriesKey + "' does not specify the observation concept: "
                    + seriesKey.CrossSectionConcept);
            }
       
            this.obsTime = obsTime;
            if (attributes != null)
            {
                this.attributes = new List<IKeyValue>(attributes);
                foreach (IKeyValue currentKv in attributes)
                {
                    this.attributeMap.Add(currentKv.Concept, currentKv);
                }
            }

            if(annotations != null)
            {
			  foreach(IAnnotation currentAnnotation in annotations) 
              {
				 this.annotations.Add(currentAnnotation);
			  }
	 	    }
            this.isCrossSection = false;
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
                return new List<IKeyValue>(this.attributes);
            }
        }

        /// <summary>
        ///   Gets a value indicating annotations.
        /// </summary>
        public virtual IList<IAnnotation> Annotations
        {
            get
            {
                return new List<IAnnotation>(annotations);
            }
        }

        public virtual IKeyable SeriesKey
        {
            get
            {
                return _seriesKey;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether cross section.
        /// </summary>
        public virtual bool CrossSection
        {
            get
            {
                return this.isCrossSection;
            }
        }

        /// <summary>
        ///   Gets the cross sectional value.
        /// </summary>
        public virtual IKeyValue CrossSectionalValue
        {
            get
            {
                return this.crossSectionValue;
            }
        }

        /// <summary>
        ///   Gets the obs as time date.
        /// </summary>
        public virtual DateTime? ObsAsTimeDate
        {
            get
            {
                if (this.obsTime == null)
                {
                    return null;
                }

                return this.date ?? (this.date = DateUtil.FormatDate(this.obsTime, true)); //TODO: Copy paste
            }
        }

        /// <summary>
        ///   Gets the obs time.
        /// </summary>
        public virtual string ObsTime
        {
            get
            {
                return this.obsTime;
            }
        }

        /// <summary>
        ///   Gets the obs time format.
        /// </summary>
        public virtual TimeFormat ObsTimeFormat
        {
            get
            {
                if (this.obsTime == null)
                {
                    return default(TimeFormat) /* was: null */;
                }

                return this.timeFormat ?? (this.timeFormat = DateUtil.GetTimeFormatOfDate(this.obsTime));
            }
        }

        /// <summary>
        ///   Gets the observation value.
        /// </summary>
        public virtual string ObservationValue
        {
            get
            {
                return this.obsValue;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The compare to.
        /// </summary>
        /// <param name="other">
        /// The other. 
        /// </param>
        /// <returns>
        /// The <see cref="int"/> . 
        /// </returns>
        public virtual int CompareTo(IObservation other)
        {
            if (this.obsTime == null)
            {
                if (other.ObsTime == null)
                {
                    return 0;
                }

                return -1;
            }

            if (other.ObsTime == null)
            {
                return 1;
            }

            if (this.obsTime.Length == other.ObsTime.Length)
            {
                return string.CompareOrdinal(this.obsTime, other.ObsTime);
            }

            DateTime? obsAsTimeDate = this.ObsAsTimeDate;
            if (obsAsTimeDate != null)
            {
                return obsAsTimeDate.Value.CompareTo(other.ObsAsTimeDate);
            }

            return -1;
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
            return this.attributeMap[concept];
        }

        /// <summary>
        ///   The to string.
        /// </summary>
        /// <returns> The <see cref="string" /> . </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            string concat = string.Empty;

            foreach (IKeyValue kv in this.attributes)
            {
                sb.Append(concat + kv.Concept + ":" + kv.Code);
                concat = ",";
            }

            if (this.isCrossSection)
            {
                return "Obs " + this.crossSectionValue.Concept + ":" + this.crossSectionValue.Code + " = "
                       + this.obsValue + " - Attributes : " + sb;
            }

            return "Obs " + this.obsTime + " = " + this.obsValue + " - Attributes : " + sb;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return obj.ToString().Equals(this.ToString());
        }

        #endregion
    }
}