// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The target object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Metadata
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.MetaData.Generic;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Metadata;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    using TargetType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.MetaData.Generic.TargetType;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///   The target object core.
    /// </summary>
    [Serializable]
    public class TargetObjectCore : SdmxObjectCore, ITarget
    {
        #region Fields

        /// <summary>
        ///   The id.
        /// </summary>
        private readonly string id;

        /// <summary>
        ///   The reference values.
        /// </summary>
        private readonly IList<IReferenceValue> referenceValues;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetObjectCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="type">
        /// The type. 
        /// </param>
        public TargetObjectCore(IMetadataReport parent, TargetType type)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataReportTarget), parent)
        {
            this.referenceValues = new List<IReferenceValue>();
            this.id = type.id;
            if (ObjectUtil.ValidCollection(type.ReferenceValue))
            {
                foreach (ReferenceValueType refValue in type.ReferenceValue)
                {
                    this.referenceValues.Add(new ReferenceValueObjectCore(this, refValue));
                }
            }

            this.Validate();
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATE                             //////////////////////////////////////////////////
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
        ///   Gets the reference values.
        /// </summary>
        public virtual IList<IReferenceValue> ReferenceValues
        {
            get
            {
                return new List<IReferenceValue>(this.referenceValues);
            }
        }

       
        #endregion

        #region Methods

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS							 //////////////////////////////////////////////////
 	    ///////////////////////////////////////////////////////////////////////////////////////////////////

        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties) 
        {
		   
             if(sdmxObject == null) 
             {
		   	     return false;
		     }
             if (sdmxObject.StructureType == this.StructureType) 
             {
			     ITarget that = (ITarget) sdmxObject;
			     if(!string.Equals(this.id, that.Id))
                 {
				     return false;
			     }
			     if(!base.Equivalent(referenceValues, that.ReferenceValues, includeFinalProperties))
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
            base.AddToCompositeSet(referenceValues, composites);
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
        }

        #endregion
    }
}