// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataSetBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The metadata set object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Metadata
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.MetaData.Generic;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Metadata;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The metadata set object core.
    /// </summary>
    [Serializable]
    public class MetadataSetObjectCore : SdmxObjectCore, IMetadataSet
    {
                #region Fields

        private readonly string setId;

        /// <summary>
        ///   The data provider reference.
        /// </summary>
        private readonly ICrossReference dataProviderReference;

        /// <summary>
        ///   The publication period.
        /// </summary>
        private readonly object publicationPeriod;

        /// <summary>
        ///   The publication year.
        /// </summary>
        private readonly ISdmxDate publicationYear;

        /// <summary>
        ///   The reporting begin date.
        /// </summary>
        private readonly ISdmxDate reportingBeginDate;

        /// <summary>
        ///   The reporting end date.
        /// </summary>
        private readonly ISdmxDate reportingEndDate;

        /// <summary>
        ///   The reports.
        /// </summary>
        private readonly IList<IMetadataReport> reports = new List<IMetadataReport>();

        /// <summary>
        ///   The structure ref.
        /// </summary>
        private readonly ICrossReference structureRef;

        /// <summary>
        ///   The valid from date.
        /// </summary>
        private readonly ISdmxDate validFromDate;

        /// <summary>
        ///   The valid to date.
        /// </summary>
        private readonly ISdmxDate validToDate;

        private readonly IList<ITextTypeWrapper> names = new List<ITextTypeWrapper>();

        #endregion

        private MetadataSetObjectCore(MetadataSetObjectCore metadataSet, IMetadataReport report) :
            base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataSet), null)
        {
            this.setId = metadataSet.setId;
            this.structureRef = metadataSet.structureRef;
            this.reportingBeginDate = metadataSet.reportingBeginDate;
            this.reportingEndDate = metadataSet.reportingEndDate;
            this.publicationYear = metadataSet.publicationYear;
            this.validFromDate = metadataSet.validFromDate;
            this.validToDate = metadataSet.validToDate;
            this.publicationPeriod = metadataSet.publicationPeriod;
            this.dataProviderReference = metadataSet.dataProviderReference;
            this.names = metadataSet.names;
            this.reports.Add(report);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataSetObjectCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="createdFrom">
        /// The created from. 
        /// </param>
        /// ///
        /// <exception cref="ArgumentNullException">
        /// Throws ArgumentNullException.
        /// </exception>
        public MetadataSetObjectCore(IMetadata parent, MetadataSetType createdFrom)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataSet), null)
        {
            if (createdFrom == null)
            {
                throw new ArgumentNullException("createdFrom");
            }
            this.setId = createdFrom.setID;

            foreach (IDatasetStructureReference structurereference in parent.Header.Structures)
            {
                if (structurereference.Id.Equals(createdFrom.structureRef))
                {
                    this.structureRef = new CrossReferenceImpl(this, structurereference.StructureReference);
                    break;
                }
            }
            if (createdFrom.Name != null)
            {
                this.names = TextTypeUtil.WrapTextTypeV21(createdFrom.Name, this);
            }

            var reportingDate = createdFrom.reportingBeginDate as DateTime?;

            if (reportingDate != null)
            {
                this.reportingBeginDate = new SdmxDateCore(reportingDate, TimeFormatEnumType.DateTime);
            }

            reportingDate = createdFrom.reportingEndDate as DateTime?;

            if (reportingDate != null)
            {
                this.reportingEndDate = new SdmxDateCore(reportingDate, TimeFormatEnumType.DateTime);
            }

            if (createdFrom.publicationYear != null)
            {
                this.publicationYear = new SdmxDateCore(createdFrom.publicationYear, TimeFormatEnumType.DateTime);
            }

            if (createdFrom.validFromDate != null)
            {
                this.validFromDate = new SdmxDateCore(createdFrom.validFromDate, TimeFormatEnumType.DateTime);
            }

            if (createdFrom.validToDate != null)
            {
                this.validToDate = new SdmxDateCore(createdFrom.validToDate, TimeFormatEnumType.DateTime);
            }

            // FUNC Publication Period
            this.publicationPeriod = createdFrom.publicationPeriod;

            if (createdFrom.DataProvider != null)
            {
                this.dataProviderReference = RefUtil.CreateReference(this, createdFrom.DataProvider);
            }

            if (ObjectUtil.ValidCollection(createdFrom.Report))
            {
                foreach (ReportType currentReport in createdFrom.Report)
                {
                    this.reports.Add(new MetadataReportObjectCore(this, currentReport));
                }
            }

            this.Validate();
        }

        #endregion

        #region Public Properties

        public IList<IMetadataSet> SplitReports
        {
            get
            {
                var returnList = new List<IMetadataSet>();
                foreach (IMetadataReport report in reports)
                {
                    returnList.Add(new MetadataSetObjectCore(this, report));
                }
                return returnList;
            }
        }

        public string SetId
        {
            get
            {
                return setId;
            }
        }

        public IList<ITextTypeWrapper> Names
        {
            get
            {
                return names;
            }
        }

        /// <summary>
        ///   Gets the data provider reference.
        /// </summary>
        public virtual ICrossReference DataProviderReference
        {
            get
            {
                return this.dataProviderReference;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///   Gets the msd reference.
        /// </summary>
        public virtual ICrossReference MsdReference
        {
            get
            {
                return this.structureRef;
            }
        }

        /// <summary>
        ///   Gets the publication period.
        /// </summary>
        public virtual object PublicationPeriod
        {
            get
            {
                return this.publicationPeriod;
            }
        }

        /// <summary>
        ///   Gets the publication year.
        /// </summary>
        public virtual ISdmxDate PublicationYear
        {
            get
            {
                return this.publicationYear;
            }
        }

        /// <summary>
        ///   Gets the reporting begin date.
        /// </summary>
        public virtual ISdmxDate ReportingBeginDate
        {
            get
            {
                return this.reportingBeginDate;
            }
        }

        /// <summary>
        ///   Gets the reporting end date.
        /// </summary>
        public virtual ISdmxDate ReportingEndDate
        {
            get
            {
                return this.reportingEndDate;
            }
        }

        /// <summary>
        ///   Gets the reports.
        /// </summary>
        public virtual IList<IMetadataReport> Reports
        {
            get
            {
                return new List<IMetadataReport>(this.reports);
            }
        }

        /// <summary>
        ///   Gets the valid from date.
        /// </summary>
        public virtual ISdmxDate ValidFromDate
        {
            get
            {
                return this.validFromDate;
            }
        }

        /// <summary>
        ///   Gets the valid to date.
        /// </summary>
        public virtual ISdmxDate ValidToDate
        {
            get
            {
                return this.validToDate;
            }
        }

        #endregion

        #region Methods

       ///////////////////////////////////////////////////////////////////////////////////////////////////
	   ////////////DEEP EQUALS							 //////////////////////////////////////////////////
	   ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.setId != null ? this.setId.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (this.dataProviderReference != null ? this.dataProviderReference.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.publicationPeriod != null ? this.publicationPeriod.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.publicationYear != null ? this.publicationYear.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.reports != null ? this.reports.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.structureRef != null ? this.structureRef.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.reportingBeginDate != null ? this.reportingBeginDate.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.reportingEndDate != null ? this.reportingEndDate.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.validFromDate != null ? this.validFromDate.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.validToDate != null ? this.validToDate.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.names != null ? this.names.GetHashCode() : 0);
                return hashCode;
            }
        }


	   public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties) 
       {
		   if(sdmxObject == null)
           {
			   return false;
		   }
           if (sdmxObject.StructureType == this.StructureType) 
           {
	
               IMetadataSet that = (IMetadataSet) sdmxObject;
		       if(!string.Equals(setId, that.SetId)) 
               {
				   return false;
		       }
		       if(!base.Equivalent(structureRef, that.MsdReference))
               {
			       return false;
		       }
               if (!string.Equals(reportingBeginDate, that.ReportingBeginDate))
               {
				   return false;
			   }
			   if(!string.Equals(reportingEndDate, that.ReportingEndDate)) 
               {
				   return false;
			   }
			   if(!string.Equals(publicationYear, that.PublicationYear))
               {
				   return false;
			   }
			   if(!string.Equals(validFromDate, that.ValidFromDate))
               {
				  return false;
			   }
			   if(!string.Equals(validToDate, that.ValidToDate)) 
               {
				  return false;
			   }
			   if(!string.Equals(publicationPeriod, (string) that.PublicationPeriod))
               {
			      return false;
		       }
		       if(!string.Equals(dataProviderReference, that.DataProviderReference))
               {
			      return false;
			   }
		       if(!base.Equivalent(names, that.Names, includeFinalProperties))
               {
			      return false;
		       }
		       if(!base.Equivalent(reports, that.Reports, includeFinalProperties))
               {
				  return false;
			   }
			
               return base.DeepEqualsInternal(that, includeFinalProperties);
		   }
		   return false;
	    }


        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////COMPOSITES		                     //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        public override bool Equals(object obj)
        {
		   if(obj is IMetadataSet) 
           {
		    
               IMetadataSet that = (IMetadataSet)obj;
		       return that.DeepEquals(this, true);
	       }
	       return false;
       }

	   public override string ToString() 
       {
		return setId;
	   }

       /// <summary>
       /// Equals the specified other.
       /// </summary>
       /// <param name="other">The other.</param>
       /// <returns></returns>
       protected bool Equals(MetadataSetObjectCore other)
       {
           return string.Equals(this.setId, other.setId) && Equals(this.dataProviderReference, other.dataProviderReference) && Equals(this.publicationPeriod, other.publicationPeriod) && Equals(this.publicationYear, other.publicationYear) && Equals(this.reports, other.reports) && Equals(this.structureRef, other.structureRef) && Equals(this.reportingBeginDate, other.reportingBeginDate) && Equals(this.reportingEndDate, other.reportingEndDate) && Equals(this.validFromDate, other.validFromDate) && Equals(this.validToDate, other.validToDate) && Equals(this.names, other.names);
       }

        /// <summary>
        ///   Gets the internal composites.
        /// </summary>
        protected override ISet<ISdmxObject> GetCompositesInternal()
        {
            ISet<ISdmxObject> composites = new HashSet<ISdmxObject>();
            base.AddToCompositeSet(names, composites);
            base.AddToCompositeSet(reports, composites);
            return composites;
        }

        
        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            string setId = this.setId ?? string.Empty;

            if (!ObjectUtil.ValidCollection(this.reports))
            {
                throw new SdmxSemmanticException("Metadata Set " + setId + "requires at least one Report");
            }

            if (this.structureRef == null)
            {
                throw new SdmxSemmanticException("Metadata Set " + setId + "requires a reference to an MSD");
            }

            if (this.structureRef.TargetReference != SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd))
            {
                throw new SdmxSemmanticException("Metadata Set " + setId + "reference must be a reference to an MSD");
            }
        }

        #endregion

    }
}