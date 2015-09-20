// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportingCategoryBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The reporting category core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.CategoryScheme
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    using CategoryType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.CategoryType;
    using DataflowRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.DataflowRefType;
    using MetadataflowRefType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.MetadataflowRefType;
    using ReportingCategory = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure.ReportingCategory;

    /// <summary>
    ///   The reporting category core.
    /// </summary>
    [Serializable]
    public class ReportingCategoryCore : ItemCore, IReportingCategoryObject
    {
        #region Fields

        /// <summary>
        ///   The provisioning metadata.
        /// </summary>
        private readonly IList<ICrossReference> provisioningMetadata;

        /// <summary>
        ///   The reporting categories.
        /// </summary>
        private readonly IList<IReportingCategoryObject> reportingCategories;

        /// <summary>
        ///   The structural metadata.
        /// </summary>
        private readonly IList<ICrossReference> structuralMetadata;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingCategoryCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="reportingCategory">
        /// The reporting category. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ReportingCategoryCore(IIdentifiableObject parent, IReportingCategoryMutableObject reportingCategory)
            : base(reportingCategory, parent)
        {
            this.structuralMetadata = new List<ICrossReference>();
            this.provisioningMetadata = new List<ICrossReference>();
            this.reportingCategories = new List<IReportingCategoryObject>();

            if (reportingCategory.ProvisioningMetadata != null)
            {
                foreach (IStructureReference structureReference in reportingCategory.ProvisioningMetadata)
                {
                    this.provisioningMetadata.Add(new CrossReferenceImpl(this, structureReference));
                }
            }

            if (reportingCategory.StructuralMetadata != null)
            {
                foreach (IStructureReference structureReference in reportingCategory.StructuralMetadata)
                {
                    this.structuralMetadata.Add(new CrossReferenceImpl(this, structureReference));
                }
            }

            if (reportingCategory.Items != null)
            {
                foreach (IReportingCategoryMutableObject reportingCategoryMutableObject in reportingCategory.Items)
                {
                    this.reportingCategories.Add(new ReportingCategoryCore(this, reportingCategoryMutableObject));
                }
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingCategoryCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="category">
        /// The category. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ReportingCategoryCore(IIdentifiableObject parent, ReportingCategoryType category)
            : base(category, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingCategory), parent)
        {
            this.structuralMetadata = new List<ICrossReference>();
            this.provisioningMetadata = new List<ICrossReference>();
            this.reportingCategories = new List<IReportingCategoryObject>();

            if (category.ProvisioningMetadata != null)
            {
                foreach (StructureUsageReferenceType structureUsageReferenceType in category.ProvisioningMetadata)
                {
                    this.provisioningMetadata.Add(RefUtil.CreateReference(this, structureUsageReferenceType));
                }
            }

            if (category.StructuralMetadata != null)
            {
                foreach (StructureReferenceType structureReferenceType in category.StructuralMetadata)
                {
                    this.structuralMetadata.Add(RefUtil.CreateReference(this, structureReferenceType));
                }
            }

            if (category.Item != null)
            {
                foreach (ReportingCategory childCategory in category.Item)
                {
                    this.reportingCategories.Add(new ReportingCategoryCore(this, childCategory.Content));
                }
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingCategoryCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="category">
        /// The category. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ReportingCategoryCore(IIdentifiableObject parent, CategoryType category)
            : base(
                category, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingCategory), 
                category.id, 
                category.uri, 
                category.Name, 
                category.Description, 
                category.Annotations, 
                parent)
        {
            this.structuralMetadata = new List<ICrossReference>();
            this.provisioningMetadata = new List<ICrossReference>();
            this.reportingCategories = new List<IReportingCategoryObject>();

            if (category.DataflowRef != null)
            {
                foreach (DataflowRefType dataflowRefType in category.DataflowRef)
                {
                    if (dataflowRefType.URN != null)
                    {
                        this.provisioningMetadata.Add(new CrossReferenceImpl(this, dataflowRefType.URN));
                    }
                    else
                    {
                        this.provisioningMetadata.Add(
                            new CrossReferenceImpl(
                                this, 
                                dataflowRefType.AgencyID, 
                                dataflowRefType.DataflowID, 
                                dataflowRefType.Version, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow)));
                    }
                }
            }

            if (category.MetadataflowRef != null)
            {
                foreach (MetadataflowRefType mdfRef in category.MetadataflowRef)
                {
                    if (mdfRef.URN != null)
                    {
                        this.provisioningMetadata.Add(new CrossReferenceImpl(this, mdfRef.URN));
                    }
                    else
                    {
                        this.provisioningMetadata.Add(
                            new CrossReferenceImpl(
                                this, 
                                mdfRef.AgencyID, 
                                mdfRef.MetadataflowID, 
                                mdfRef.Version, 
                                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow)));
                    }
                }
            }

            if (category.Category != null)
            {
                foreach (CategoryType childCategory in category.Category)
                {
                    this.reportingCategories.Add(new ReportingCategoryCore(this, childCategory));
                }
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

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

        /// <summary>
        ///   Gets the items.
        /// </summary>
        public virtual IList<IReportingCategoryObject> Items
        {
            get
            {
                return new List<IReportingCategoryObject>(this.reportingCategories);
            }
        }

        /// <summary>
        ///   Gets the provisioning metadata.
        /// </summary>
        public virtual IList<ICrossReference> ProvisioningMetadata
        {
            get
            {
                return new List<ICrossReference>(this.provisioningMetadata);
            }
        }

        /// <summary>
        ///   Gets the structural metadata.
        /// </summary>
        public virtual IList<ICrossReference> StructuralMetadata
        {
            get
            {
                return new List<ICrossReference>(this.structuralMetadata);
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
            if (sdmxObject == null) return false;

            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IReportingCategoryObject)sdmxObject;
                if (!ObjectUtil.Equivalent(this.structuralMetadata, that.StructuralMetadata))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.provisioningMetadata, that.ProvisioningMetadata))
                {
                    return false;
                }

                if (!this.Equivalent(this.reportingCategories, that.Items, includeFinalProperties))
                {
                    return false;
                }

                return this.DeepEqualsNameable(that, includeFinalProperties);
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
            if (ObjectUtil.ValidCollection(this.provisioningMetadata)
                && ObjectUtil.ValidCollection(this.structuralMetadata))
            {
                throw new SdmxSemmanticException(
                    "Reporting Category can not have both structural metadata and provisioning metadata");
            }

            // Validate StructuralMetadata is either pointing to a DSD or MSD, and only contains the same type
            foreach (ICrossReference crossReference in this.structuralMetadata)
            {
                if (crossReference.TargetReference.EnumType != SdmxStructureEnumType.Dsd
                    && crossReference.TargetReference.EnumType != SdmxStructureEnumType.Dsd)
                {
                    throw new SdmxSemmanticException(
                        "Reporting Category 'Structural Metadata' must either reference DSDs or MSDs, not "
                        + crossReference.TargetReference.GetType());
                }
            }

            foreach (ICrossReference crossReference in this.provisioningMetadata)
            {
                if (crossReference.TargetReference.EnumType != SdmxStructureEnumType.Dataflow
                    && crossReference.TargetReference.EnumType != SdmxStructureEnumType.MetadataFlow)
                {
                    throw new SdmxSemmanticException(
                        "Reporting Category 'Provisioning Metadata' must either reference a Data Flow or Metadata Flow, not "
                        + crossReference.TargetReference.GetType());
                }
            }
        }
       ///////////////////////////////////////////////////////////////////////////////////////////////////
       ////////////COMPOSITES                           //////////////////////////////////////////////////
       ///////////////////////////////////////////////////////////////////////////////////////////////////	

        /// <summary>
        /// The get composites internal.
        /// </summary>
        protected override ISet<ISdmxObject> GetCompositesInternal() 
        {
    	    ISet<ISdmxObject> composites = base.GetCompositesInternal();
    	    base.AddToCompositeSet(reportingCategories, composites);
    	    return composites;
        }
        #endregion
    }
}