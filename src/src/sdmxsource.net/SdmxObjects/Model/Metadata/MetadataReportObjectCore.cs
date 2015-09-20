// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataReportBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The metadata report dataStructureObject core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Metadata
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.MetaData.Generic;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Metadata;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    using TargetType = Org.Sdmxsource.Sdmx.Api.Constants.TargetType;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///   The metadata report dataStructureObject core.
    /// </summary>
    [Serializable]
    public class MetadataReportObjectCore : SdmxObjectCore, IMetadataReport
    {
        #region Fields

        /// <summary>
        ///   The id.
        /// </summary>
        private readonly string id;

        /// <summary>
        ///   The reported attributes.
        /// </summary>
        private readonly IList<IReportedAttributeObject> _reportedAttributes;

        /// <summary>
        ///   The _target.
        /// </summary>
        private readonly ITarget _target;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="MetadataReportObjectCore" /> class.
        /// </summary>
        /// <param name="parent"> The parent. </param>
        /// <param name="report"> The dataStructureObject. </param>
        public MetadataReportObjectCore(IMetadataSet parent, ReportType report)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataReport), parent)
        {
            this._reportedAttributes = new List<IReportedAttributeObject>();
            this.id = report.id;
            if (report.Target != null)
            {
                this._target = new TargetObjectCore(this, report.Target);
            }

            if (report.AttributeSet != null)
            {
                if (ObjectUtil.ValidCollection(report.AttributeSet.ReportedAttribute))
                {
                    this._reportedAttributes.Clear();

                    foreach (ReportedAttributeType each in report.AttributeSet.ReportedAttribute)
                    {
                        this._reportedAttributes.Add(new ReportedAttributeObjectObjectCore(this, each));
                    }
                }
            }

            this.Validate();
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
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
        ///   Gets the reported attributes.
        /// </summary>
        public virtual IList<IReportedAttributeObject> ReportedAttributes
        {
            get
            {
                return new List<IReportedAttributeObject>(this._reportedAttributes);
            }
        }

        /// <summary>
        ///   Gets the _target.
        /// </summary>
        public virtual ITarget Target
        {
            get
            {
                return this._target;
            }
        }

        public ISet<TargetType> Targets
        {
            get
            {
                ISet<TargetType> targets = new HashSet<TargetType>();
                foreach (IReferenceValue rv in _target.ReferenceValues)
                {
                    targets.Add(rv.TargetType);
                }
                return targets;
            }
        }

        public string TargetDatasetId
        {
            get
            {
                foreach (IReferenceValue rv in _target.ReferenceValues)
                {
                    if (rv.TargetType == TargetType.Dataset)
                    {
                        return rv.DatasetId;
                    }
                }
                return null;
            }
        }

        public ISdmxDate TargetReportPeriod
        {
            get
            {
                foreach (IReferenceValue rv in _target.ReferenceValues)
                {
                    if (rv.TargetType == TargetType.ReportPeriod)
                    {
                        return rv.ReportPeriod;
                    }
                }
                return null;
            }
        }

        public ICrossReference TargetIdentifiableReference
        {
            get
            {
                foreach (IReferenceValue rv in _target.ReferenceValues)
                {
                    if (rv.TargetType == TargetType.Identifiable)
                    {
                        return rv.IdentifiableReference;
                    }
                }
                return null;
            }
        }

        public ICrossReference TargetContentConstraintReference
        {
            get
            {
                foreach (IReferenceValue rv in _target.ReferenceValues)
                {
                    if (rv.TargetType == TargetType.Constraint)
                    {
                        return rv.ContentConstraintReference;
                    }
                }
                return null;
            }
        }

        public IList<IDataKey> TargetDataKeys
        {
            get
            {
                foreach (IReferenceValue rv in _target.ReferenceValues)
                {
                    if (rv.TargetType == TargetType.DataKey)
                    {
                        return rv.DataKeys;
                    }
                }
                return null;
            }
        }

        #endregion

        #region Methods

       ///////////////////////////////////////////////////////////////////////////////////////////////////
	   ////////////DEEP EQUALS							 //////////////////////////////////////////////////
	   ///////////////////////////////////////////////////////////////////////////////////////////////////
	
	    public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties) 
        {
            if (sdmxObject == null) 
            {
			  return false;
		    }
            if (sdmxObject.StructureType == this.StructureType)
            {
               IMetadataReport that = (IMetadataReport)sdmxObject;
			   if(!string.Equals(this.id, that.Id))
               {
				  return false;
			   }
			   if(!base.Equivalent(_target, that.Target, includeFinalProperties)) 
               {
				  return false;
			   }
		 	   if(!base.Equivalent(_reportedAttributes, that.ReportedAttributes, includeFinalProperties)) 
               {
				  return false;
			   }
		       return base.DeepEqualsInternal(that, includeFinalProperties);
	      }

		  return false;
  	    }

        /// <summary>
        ///   Gets the internal composites.
        /// </summary>
        protected override ISet<ISdmxObject> GetCompositesInternal()
        {
            ISet<ISdmxObject> composites = new HashSet<ISdmxObject>();
            base.AddToCompositeSet(_target, composites);
            base.AddToCompositeSet(_reportedAttributes, composites);
            return composites;
        }

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.id))
            {
                throw new SdmxSemmanticException("Metadata Report must have an Id");
            }

            if (this._target == null)
            {
                throw new SdmxSemmanticException("Metadata Report must have a Target");
            }

            if (!ObjectUtil.ValidCollection(this._reportedAttributes))
            {
                throw new SdmxSemmanticException("Metadata Report must have at least one Reported Attribute");
            }
        }

        #endregion
    }
}