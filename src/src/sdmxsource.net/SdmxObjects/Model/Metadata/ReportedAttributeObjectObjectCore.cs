// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportedAttributeBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The reported attributeObject object core.
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
    using Org.Sdmxsource.Sdmx.Api.Model.Metadata;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The reported attributeObject object core.
    /// </summary>
    [Serializable]
    public class ReportedAttributeObjectObjectCore : SdmxObjectCore, IReportedAttributeObject
    {
        #region Fields

        /// <summary>
        ///   The id.
        /// </summary>
        private readonly string id;

        /// <summary>
        ///   The metadata text.
        /// </summary>
        private readonly IList<ITextTypeWrapper> metadataText;

        /// <summary>
        ///   The reported attributeObject.
        /// </summary>
        private readonly IList<IReportedAttributeObject> reportedAttribute;

        /// <summary>
        ///   The simple value.
        /// </summary>
        private readonly string simpleValue;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportedAttributeObjectObjectCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="type">
        /// The type. 
        /// </param>
        public ReportedAttributeObjectObjectCore(ISdmxObject parent, ReportedAttributeType type)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataReportAttribute), parent)
        {
            this.metadataText = new List<ITextTypeWrapper>();
            this.reportedAttribute = new List<IReportedAttributeObject>();
            this.id = type.id;
            if (ObjectUtil.ValidCollection(type.StructuredText))
            {
                foreach (StructuredText text in type.StructuredText)
                {
                    this.metadataText.Add(new TextTypeWrapperImpl(text.Content, this));
                }
            }

            if (ObjectUtil.ValidCollection(type.Text))
            {
                foreach (Text text0 in type.Text)
                {
                    this.metadataText.Add(new TextTypeWrapperImpl(text0.Content, this));
                }
            }

            if (type.AttributeSet != null)
            {
                foreach (ReportedAttributeType currentType in type.AttributeSet.ReportedAttribute)
                {
                    this.reportedAttribute.Add(new ReportedAttributeObjectObjectCore(this, currentType));
                }
            }

            this.simpleValue = type.value;
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
        ///   Gets the metadata text.
        /// </summary>
        public virtual IList<ITextTypeWrapper> MetadataText
        {
            get
            {
                return new List<ITextTypeWrapper>(this.metadataText);
            }
        }

        /// <summary>
        ///   Gets a value indicating whether presentational.
        /// </summary>
        public virtual bool Presentational
        {
            get
            {
                return this.metadataText.Count == 0;
            }
        }

        /// <summary>
        ///   Gets the reported attributes.
        /// </summary>
        public virtual IList<IReportedAttributeObject> ReportedAttributes
        {
            get
            {
                return new List<IReportedAttributeObject>(this.reportedAttribute);
            }
        }

        /// <summary>
        ///   Gets the simple value.
        /// </summary>
        public virtual string SimpleValue
        {
            get
            {
                return this.simpleValue;
            }
        }

        #endregion

        #region Public Methods and Operators

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
			    IReportedAttributeObject that = (IReportedAttributeObject) sdmxObject;
			    if(!string.Equals(this.id, that.Id)) 
                {
				    return false;
		        }		
                if(!string.Equals(this.simpleValue, that.SimpleValue))
                {
				    return false;
			    }
			    if(!base.Equivalent(metadataText, that.MetadataText, includeFinalProperties))
                {
			        return false;
		        }
		        if(!base.Equivalent(reportedAttribute, that.ReportedAttributes, includeFinalProperties))
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
            base.AddToCompositeSet(metadataText, composites);
            base.AddToCompositeSet(reportedAttribute, composites);
            return composites;
        }

        /// <summary>
        ///   The has simple value.
        /// </summary>
        /// <returns> The <see cref="bool" /> . </returns>
        public virtual bool HasSimpleValue()
        {
            return this.simpleValue != null;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.id))
            {
                throw new SdmxSemmanticException("Metadata Reported Attribute must have an Id");
            }
        }

        #endregion
    }
}