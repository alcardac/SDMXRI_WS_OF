// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReferenceValueBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The reference value object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Metadata
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.MetaData.Generic;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Metadata;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Util;

    using TargetType = Org.Sdmxsource.Sdmx.Api.Constants.TargetType;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///   The reference value object core.
    /// </summary>
    [Serializable]
    public class ReferenceValueObjectCore : SdmxObjectCore, IReferenceValue
    {
        #region Fields

        private readonly ISdmxDate reportPeriod;

        /// <summary>
        ///   The constraint reference.
        /// </summary>
        private readonly ICrossReference constraintReference;

        /// <summary>
        ///   The data keys.
        /// </summary>
        private readonly IList<IDataKey> dataKeys;

        /// <summary>
        ///   The dataset id.
        /// </summary>
        private readonly string datasetId;

        /// <summary>
        ///   The id.
        /// </summary>
        private readonly string id;

        /// <summary>
        ///   The identifiable reference.
        /// </summary>
        private readonly ICrossReference identifiableReference;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ReferenceValueObjectCore" /> class.
        /// </summary>
        /// <param name="parent"> The parent. </param>
        /// <param name="type"> The type. </param>
        public ReferenceValueObjectCore(ITarget parent, ReferenceValueType type)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataReferenceValue), parent)
        {
            this.dataKeys = new List<IDataKey>();
            this.id = type.id;
            if (type.ConstraintContentReference != null)
            {
                this.constraintReference = RefUtil.CreateReference(this, type.ConstraintContentReference);
            }

            if (type.ObjectReference != null)
            {
                this.identifiableReference = RefUtil.CreateReference(this, type.ObjectReference);
            }

            if (type.DataSetReference != null)
            {
                this.datasetId = type.DataSetReference.ID;
                this.identifiableReference = RefUtil.CreateReference(this, type.DataSetReference.DataProvider);
            }

            if (type.DataKey != null)
            {
                foreach (var cvst in type.DataKey.GetTypedKeyValue<DataKeyValueType>())
                {
                    this.dataKeys.Add(new DataKeyObjectCore(this, cvst));
                }
            }

            if (type.ReportPeriod != null)
            {
                reportPeriod = new SdmxDateCore(type.ReportPeriod.ToString());
            }

            this.Validate();
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATE                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Properties

        /// <summary>
        ///   Gets the content constraint reference.
        /// </summary>
        public ICrossReference ContentConstraintReference
        {
            get
            {
                return this.constraintReference;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether content constriant reference.
        /// </summary>
        public bool IsContentConstriantReference
        {
            get
            {
                return this.constraintReference != null;
            }
        }

        /// <summary>
        ///   Gets the data keys.
        /// </summary>
        public IList<IDataKey> DataKeys
        {
            get
            {
                return new List<IDataKey>(this.dataKeys);
            }
        }

        /// <summary>
        ///   Gets a value indicating whether datakey reference.
        /// </summary>
        public bool DatakeyReference
        {
            get
            {
                return this.dataKeys.Count > 0;
            }
        }

        /// <summary>
        ///   Gets the dataset id.
        /// </summary>
        public string DatasetId
        {
            get
            {
                return this.datasetId;
            }
        }

        public ISdmxDate ReportPeriod
        {
            get
            {
                return reportPeriod;
            }
        }

        public TargetType TargetType
        {
            get
            {
                if (constraintReference != null)
                {
                    return TargetType.Constraint;
                }
                if (ObjectUtil.ValidString(datasetId))
                {
                    return TargetType.Dataset;
                }
                if (dataKeys.Count > 0)
                {
                    return TargetType.DataKey;
                }
                if (reportPeriod != null)
                {
                    return TargetType.ReportPeriod;
                }
                if(identifiableReference != null) 
                {
		            return TargetType.Identifiable;
		        }
                //THIS POINT SHOULD NEVER BE REACHED AND PICKED UP BY THE VALIDATE METHOD
                throw new Exception("Reference value is not referencing anything");
            }
        }

        /// <summary>
        ///   Gets a value indicating whether dataset reference.
        /// </summary>
        public bool DatasetReference
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.datasetId);
            }
        }

        /// <summary>
        ///   Gets the id.
        /// </summary>
        public string Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        ///   Gets the identifiable reference.
        /// </summary>
        public ICrossReference IdentifiableReference
        {
            get
            {
                return this.identifiableReference;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether is identifiable reference.
        /// </summary>
        public bool IsIdentifiableReference
        {
            get
            {
                return !this.DatasetReference && this.identifiableReference != null;
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
                IReferenceValue that = (IReferenceValue)sdmxObject;
			    if(!string.Equals(this.id, that.Id)) 
                {
				    return false;
		        }
		        if(!string.Equals(this.datasetId, that.DatasetId)) 
                {
				    return false;
			    }
			    if(!string.Equals(this.constraintReference, that.ContentConstraintReference)) 
                {
				    return false;
			    }
		        if(!string.Equals(this.identifiableReference, that.IdentifiableReference)) 
                {
				    return false;
			    }
			    if(!string.Equals(this.reportPeriod, that.ReportPeriod))
                {
				    return false;
			    }
		        if(!base.Equivalent(dataKeys, that.DataKeys, includeFinalProperties))
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

        /// <summary>
        ///   Gets the internal composites.
        /// </summary>
        protected override ISet<ISdmxObject> GetCompositesInternal()
        {
            ISet<ISdmxObject> composites = new HashSet<ISdmxObject>();
            base.AddToCompositeSet(dataKeys, composites);
            return composites;
        }

        
	    public override string ToString()
        {
		    return "Metadata Target Reference " + Id;
	    }

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (!ObjectUtil.ValidString(id))
            {
                throw new SdmxSemmanticException("Metadata Report must have an Id");
            }
            if (ObjectUtil.ValidString(datasetId))
            {
              
                if (dataKeys.Count > 0)
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both a dataset, and a DataKey");
                }
                if (reportPeriod != null)
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both a dataset, and a Report Period");
                }
                if (constraintReference != null)
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both a dataset, and a Constraint");
                }
            }
            else if (dataKeys.Count > 0)
            {
                if (identifiableReference != null)
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both a DataKey, and an Identifiable");
                }
                if (ObjectUtil.ValidString(datasetId))
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both a DataKey, and a dataset");
                }
                if (reportPeriod != null)
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both a DataKey, and a Report Period");
                }
                if (constraintReference != null)
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both a DataKey, and a Constraint");
                }
            }
            else if (identifiableReference != null)
            {
                if (dataKeys.Count > 0)
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both an Identifiable, and a DataKey");
                }
                if (ObjectUtil.ValidString(datasetId))
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both an Identifiable, and a dataset");
                }
                if (reportPeriod != null)
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both an Identifiable, and a Report Period");
                }
                if (constraintReference != null)
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both an Identifiable, and a Constraint");
                }
            }
            else if (reportPeriod != null)
            {
                if (identifiableReference != null)
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both a Report Period, and an Identifiable");
                }
                if (dataKeys.Count > 0)
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both a Report Period, and a DataKey");
                }
                if (ObjectUtil.ValidString(datasetId))
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both a Report Period, and a dataset");
                }
                if (constraintReference != null)
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both a Report Period, and a Constraint");
                }
            }
            else if (constraintReference != null)
            {
                if (identifiableReference != null)
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both a Constraint, and an Identifiable");
                }
                if (ObjectUtil.ValidString(datasetId))
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both a Constraint, and a dataset");
                }
                if (dataKeys.Count > 0)
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both a Constraint, and a DataKey");
                }
                if (reportPeriod != null)
                {
                    throw new SdmxSemmanticException(
                        "Reference Value can only contain one target, a datakey, dataset, report period, or an identifiable.  '"
                        + id + "' references both a Constraint, and a Report Period");
                }
            }
            else
            {
                throw new SdmxSemmanticException(
                    "Metadata Reference Value must referenece either a datakey, dataset, report period, or an identifiable");
            }
        }

        #endregion
    }
}