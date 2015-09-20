// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataAttributeBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The metadata attribute core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.MetadataStructure
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    using UsageStatusTypeConstants = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.UsageStatusTypeConstants;

    /// <summary>
    ///   The metadata attributeObject core.
    /// </summary>
    [Serializable]
    public class MetadataAttributeObjectCore : ComponentCore, IMetadataAttributeObject
    {
        #region Fields

        /// <summary>
        ///   The metadata attributes.
        /// </summary>
        private readonly IList<IMetadataAttributeObject> metadataAttributes;

        /// <summary>
        ///   The presentational.
        /// </summary>
        private readonly TertiaryBool presentational;

        /// <summary>
        ///   The max occurs.
        /// </summary>
        private int? maxOccurs;

        /// <summary>
        ///   The min occurs.
        /// </summary>
        private int? minOccurs;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataAttributeObjectCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public MetadataAttributeObjectCore(IIdentifiableObject parent, IMetadataAttributeMutableObject itemMutableObject)
            : base(itemMutableObject, parent)
        {
            this.metadataAttributes = new List<IMetadataAttributeObject>();
            this.presentational = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            try
            {
                if (itemMutableObject.MetadataAttributes != null)
                {
                    foreach (IMetadataAttributeMutableObject currentMa in itemMutableObject.MetadataAttributes)
                    {
                        this.metadataAttributes.Add(new MetadataAttributeObjectCore(this, currentMa));
                    }
                }

                if (itemMutableObject.MinOccurs != null)
                {
                    this.minOccurs = itemMutableObject.MinOccurs;
                }

                if (itemMutableObject.MaxOccurs != null)
                {
                    this.maxOccurs = itemMutableObject.MaxOccurs;
                }

                this.presentational = itemMutableObject.Presentational;
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException("IsError creating structure: " + this, th);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataAttributeObjectCore"/> class.
        /// </summary>
        /// <param name="metadataAttribute">
        /// The metadata attributeObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public MetadataAttributeObjectCore(MetadataAttributeType metadataAttribute, IIdentifiableObject parent)
            : base(metadataAttribute, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataAttribute), parent)
        {
            this.metadataAttributes = new List<IMetadataAttributeObject>();
            this.presentational = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            this.minOccurs = (metadataAttribute.minOccurs < int.MaxValue) ? decimal.ToInt32(metadataAttribute.minOccurs) : (int?)null;
           
            if (metadataAttribute.maxOccurs != null)
            {
                long res;
                if (long.TryParse(metadataAttribute.maxOccurs.ToString(), out res))
                {
                    this.maxOccurs = Convert.ToInt32(res);
                }
                else
                {
                    this.maxOccurs = null; // unbounded 
                }
            }

            if (metadataAttribute.isPresentational)
            {
                this.presentational = TertiaryBool.ParseBoolean(metadataAttribute.isPresentational);
            }

            if (metadataAttribute.MetadataAttribute != null)
            {
                foreach (MetadataAttribute currentMaType in metadataAttribute.MetadataAttribute)
                {
                    this.metadataAttributes.Add(new MetadataAttributeObjectCore(currentMaType.Content, this));
                }
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataAttributeObjectCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="metadataAttribute">
        /// The metadata attributeObject. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public MetadataAttributeObjectCore(
            IIdentifiableObject parent, 
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.MetadataAttributeType metadataAttribute)
            : base(
                metadataAttribute, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataAttribute), 
                metadataAttribute.Annotations, 
                metadataAttribute.TextFormat, 
                metadataAttribute.representationSchemeAgency, 
                metadataAttribute.representationScheme,
                metadataAttribute.RepresentationSchemeVersionEstat, 
                metadataAttribute.conceptSchemeAgency, 
                metadataAttribute.conceptSchemeRef, 
                GetConceptSchemeVersion(metadataAttribute), 
                metadataAttribute.conceptAgency, 
                metadataAttribute.conceptRef, 
                parent)
        {
            this.metadataAttributes = new List<IMetadataAttributeObject>();
            this.presentational = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);

            if (metadataAttribute.usageStatus != null)
            {
                if (metadataAttribute.usageStatus == UsageStatusTypeConstants.Mandatory)
                {
                    this.minOccurs = 1;
                    this.maxOccurs = 1;
                }
                else
                {
                    this.minOccurs = 0;
                }
            }

            if (metadataAttribute.MetadataAttribute != null)
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.MetadataAttributeType currentMa in
                    metadataAttribute.MetadataAttribute)
                {
                    this.metadataAttributes.Add(new MetadataAttributeObjectCore(this, currentMa));
                }
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the max occurs.
        /// </summary>
        public virtual int? MaxOccurs
        {
            get
            {
                return this.maxOccurs;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///   Gets the metadata attributes.
        /// </summary>
        public virtual IList<IMetadataAttributeObject> MetadataAttributes
        {
            get
            {
                return new List<IMetadataAttributeObject>(this.metadataAttributes);
            }
        }

        /// <summary>
        ///   Gets the min occurs.
        /// </summary>
        public virtual int? MinOccurs
        {
            get
            {
                if (this.minOccurs == null)
                {
                    return new int() /* was: null */;
                }

                return this.minOccurs;
            }
        }

        /// <summary>
        ///   Gets the presentational.
        /// </summary>
        public virtual TertiaryBool Presentational
        {
            get
            {
                return this.presentational;
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
            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IMetadataAttributeObject)sdmxObject;
                if (!this.Equivalent(this.metadataAttributes, that.MetadataAttributes, includeFinalProperties))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.minOccurs, that.MinOccurs))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.maxOccurs, that.MaxOccurs))
                {
                    return false;
                }

                if (this.presentational != that.Presentational)
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        protected internal void Validate()
        {
            if (this.metadataAttributes != null)
            {
                ISet<string> conceptIds = new HashSet<string>();

                foreach (IMetadataAttributeObject metadataAttribute in this.metadataAttributes)
                {
                    if (metadataAttribute.ConceptRef == null)
                    {
                        throw new SdmxSemmanticException("Metadata Attribute must reference a concept");
                    }

                    string ids = metadataAttribute.Id;
                    if (conceptIds.Contains(ids))
                    {
                        throw new SdmxSemmanticException(ExceptionCode.DuplicateConcept, metadataAttribute.ToString());
                    }

                    conceptIds.Add(ids);
                }
            }

            if (this.minOccurs != null && this.maxOccurs != null)
            {
                if (this.minOccurs.Value.CompareTo(this.maxOccurs.Value) > 0)
                {
                    throw new SdmxSemmanticException(
                        "Max Occurs '" + this.maxOccurs + "' can not be a lower value then Min Occurs '"
                        + this.minOccurs
                        +
                        "'.  Please note the abscence of Max Occurs defaults to value of 1 - specify the value as 'unbounded' is this is not the case.");
                }
            }
        }

      ///////////////////////////////////////////////////////////////////////////////////////////////////
      ////////////COMPOSITES		                     //////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////////////////////////////////

      /// <summary>
      ///   The get composites internal.
      /// </summary>
      protected override ISet<ISdmxObject> GetCompositesInternal()
      {
    	  ISet<ISdmxObject> composites = base.GetCompositesInternal();
          base.AddToCompositeSet(this.metadataAttributes, composites);
          return composites;
      }

      /// <summary>
      /// Returns concept scheme version. It tries to detect various conventions
      /// </summary>
      /// <param name="attributeType">
      /// The attribute Type.
      /// </param>
      /// <returns>
      /// The concept scheme version; otherwise null
      /// </returns>
      private static string GetConceptSchemeVersion(Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.MetadataAttributeType attributeType)
      {
          if (!string.IsNullOrWhiteSpace(attributeType.conceptVersion))
          {
              return attributeType.conceptVersion;
          }

          if (!string.IsNullOrWhiteSpace(attributeType.ConceptSchemeVersionEstat))
          {
              return attributeType.ConceptSchemeVersionEstat;
          }

          var extDimension = attributeType as Org.Sdmx.Resources.SdmxMl.Schemas.V20.extension.structure.MetadataAttributeType;
          if (extDimension != null && !string.IsNullOrWhiteSpace(extDimension.conceptSchemeVersion))
          {
              return extDimension.conceptSchemeVersion;
          }

          return null;
      }
        #endregion
    }
}