// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeRangeImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The time range core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference.Complex
{
    #region Using directives

    using System;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;

    #endregion

    /// <summary>
    ///   The time range core.
    /// </summary>
    [Serializable]
    public class TimeRangeCore : ITimeRange
    {
        #region Fields

        /// <summary>
        ///   The range.
        /// </summary>
        private bool _range;

        /// <summary>
        ///   The start date.
        /// </summary>
        private ISdmxDate _startDate;

        /// <summary>
        ///   The end date.
        /// </summary>
	   private ISdmxDate _endDate;

       /// <summary>
       ///   The start inclusive.
       /// </summary>
	   private bool _startInclusive = true;

       /// <summary>
       ///   The end inclusive.
       /// </summary>
	   private bool _endInclusive = true;

       #endregion

       #region Constructors and Destructors


       /// <summary>
       /// Initializes a new instance of the <see cref="ComplexAnnotationReferenceCore"/> class.
       /// </summary>
       /// <param name="range">
       /// The range. 
       /// </param>
       /// <param name="startDate">
       /// The start date. 
       /// </param>
       /// <param name="endDate">
       /// The end date. 
       /// </param>
       /// <param name="startInclusive">
       /// The start inclusive. 
       /// </param>
       /// <param name="endInclusive">
       /// The end inclusive. 
       /// </param>
        public TimeRangeCore(bool range, ISdmxDate startDate, ISdmxDate endDate, bool startInclusive, bool endInclusive) 
        {
	  	    this._range = range;
		    this._startDate = startDate;
		    this._endDate = endDate;
		    this._startInclusive = startInclusive;
		    this._endInclusive = endInclusive;
		 
		    if (startDate == null && endDate == null)
            {
			   throw new SdmxSemmanticException("When setting range, cannot have both start/end periods null.");
		    }

		   if (this._range)
           {
			 if (startDate == null || endDate == null)
             {
				throw new SdmxSemmanticException("When range is defined then both start/end periods should be set.");
			 }
		   } 
           else
           {
			  if (startDate != null && endDate != null) 
              {
				throw new SdmxSemmanticException("When it is not a range then not both start/end periods can be set.");
			  }
		   }
  	    }

       #endregion

       #region Public Properties

        /// <summary>
        ///   Gets the range.
        /// </summary>
       public virtual bool IsRange
       {
		  get
		  {
		    return _range;
		  }
       }

       /// <summary>
       ///   Gets the start date.
       /// </summary>
      public virtual ISdmxDate StartDate
      {
		 get
		 {
		    return _startDate;
		 }
      }

      /// <summary>
      ///   Gets the end date.
      /// </summary>
	  public virtual ISdmxDate EndDate
      {
		 get
		 {
		    return _endDate;
		 }
      }

      /// <summary>
      ///   Gets the is start inclusive.
      /// </summary>
	  public virtual bool IsStartInclusive
      {
         get
         {
            return _startInclusive;
         }
      }

      /// <summary>
      ///   Gets the is end inclusive.
      /// </summary>
       public virtual bool IsEndInclusive
       {
		  get
		  {
		    return _endInclusive;
		  }
       }

        #endregion

    }
}
