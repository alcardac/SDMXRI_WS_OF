// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportStructureBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The report structure core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.MetadataStructure
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Util;

    using MetadataAttribute = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.MetadataAttribute;
    using MetadataAttributeType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.MetadataAttributeType;

    /// <summary>
    ///   The report structure core.
    /// </summary>
    [Serializable]
    public class ReportStructureCore : IdentifiableCore, IReportStructure
    {
        #region Fields

        /// <summary>
        ///   The metadata attributes.
        /// </summary>
        private readonly IList<IMetadataAttributeObject> metadataAttributes;

        /// <summary>
        ///   The target metadatas.
        /// </summary>
        private readonly IList<string> targetMetadatas;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportStructureCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="rs">
        /// The rs. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ReportStructureCore(IMetadataStructureDefinitionObject parent, IReportStructureMutableObject rs)
            : base(rs, parent)
        {
            this.metadataAttributes = new List<IMetadataAttributeObject>();
            this.targetMetadatas = new List<string>();
            try
            {
                if (rs.MetadataAttributes != null)
                {
                    foreach (IMetadataAttributeMutableObject currentMa in rs.MetadataAttributes)
                    {
                        this.metadataAttributes.Add(new MetadataAttributeObjectCore(this, currentMa));
                    }
                }

                if (rs.TargetMetadatas != null)
                {
                    this.targetMetadatas = new List<string>(rs.TargetMetadatas);
                }
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException(th, ExceptionCode.ObjectStructureConstructionError, this);
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
        /// Initializes a new instance of the <see cref="ReportStructureCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="rs">
        /// The rs. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ReportStructureCore(IMetadataStructureDefinitionObject parent, ReportStructureType rs)
            : base(rs, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportStructure), parent)
        {
            this.metadataAttributes = new List<IMetadataAttributeObject>();
            this.targetMetadatas = new List<string>();
            try
            {
                foreach (MetadataAttribute currentMa in rs.Component)
                {
                    this.metadataAttributes.Add(new MetadataAttributeObjectCore(currentMa.Content, this));
                }

                if (rs.MetadataTarget != null)
                {
                    foreach (LocalMetadataTargetReferenceType localReference in rs.MetadataTarget)
                    {
                        this.targetMetadatas.Add(RefUtil.CreateLocalIdReference(localReference));
                    }
                }
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException(th, ExceptionCode.ObjectStructureConstructionError, this);
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
        /// Initializes a new instance of the <see cref="ReportStructureCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="rs">
        /// The rs. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ReportStructureCore(
            IMetadataStructureDefinitionObject parent, 
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ReportStructureType rs)
            : base(
                rs, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportStructure), 
                rs.id, 
                rs.uri, 
                rs.Annotations, 
                parent)
        {
            this.metadataAttributes = new List<IMetadataAttributeObject>();
            this.targetMetadatas = new List<string>();
            try
            {
                foreach (MetadataAttributeType currentMa in rs.MetadataAttribute)
                {
                    this.metadataAttributes.Add(new MetadataAttributeObjectCore(this, currentMa));
                }

                if (!string.IsNullOrWhiteSpace(rs.target))
                {
                    this.targetMetadatas.Add(rs.target);
                }
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException(th, ExceptionCode.ObjectStructureConstructionError, this);
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

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

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
        ///   Gets the target metadatas.
        /// </summary>
        public virtual IList<string> TargetMetadatas
        {
            get
            {
                return new List<string>(this.targetMetadatas);
            }
        }
        
        /// <summary>
        /// Gets the Urn
        /// </summary>
        public sealed override Uri Urn
        {
            get
            {
                return base.Urn;
            }
        }
        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The agencyScheme. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IReportStructure)sdmxObject;
                if (!this.Equivalent(this.metadataAttributes, that.MetadataAttributes, includeFinalProperties))
                {
                    return false;
                }

                if (!ObjectUtil.EquivalentCollection(this.targetMetadatas, that.TargetMetadatas))
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
        private void Validate()
        {
            if (!ObjectUtil.ValidCollection(this.metadataAttributes))
            {
                throw new SdmxSemmanticException("Report CategorisationStructure requires at least one Metadata Attribute");
            }

            if (!ObjectUtil.ValidCollection(this.targetMetadatas))
            {
                throw new SdmxSemmanticException("Report CategorisationStructure requires at least one Metadata Target");
            }
        }

       ///////////////////////////////////////////////////////////////////////////////////////////////////
      ////////////COMPOSITES		                     //////////////////////////////////////////////////
      ///////////////////////////////////////////////////////////////////////////////////////////////////
     
      /// <summary>
      ///   Get composites internal.
      /// </summary>
     protected override ISet<ISdmxObject> GetCompositesInternal() 
     {
    	ISet<ISdmxObject> composites = base.GetCompositesInternal();
        base.AddToCompositeSet(this.metadataAttributes, composites);
        return composites;
     }

        #endregion
    }
}