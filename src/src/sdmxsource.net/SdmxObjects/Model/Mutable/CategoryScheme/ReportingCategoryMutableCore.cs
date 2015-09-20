// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportingCategoryMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The reporting category mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.CategoryScheme
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The reporting category mutable core.
    /// </summary>
    [Serializable]
    public class ReportingCategoryMutableCore : ItemMutableCore, IReportingCategoryMutableObject
    {
        #region Fields

        /// <summary>
        ///   The provisioning metadata.
        /// </summary>
        private IList<IStructureReference> _provisioningMetadata;

        /// <summary>
        ///   The reporting categories.
        /// </summary>
        private readonly IList<IReportingCategoryMutableObject> _reportingCategories;

        /// <summary>
        ///   The structural metadata.
        /// </summary>
        private IList<IStructureReference> _structuralMetadata;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ReportingCategoryMutableCore" /> class. 
        ///   Default Constructor
        /// </summary>
        public ReportingCategoryMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingCategory))
        {
            this._structuralMetadata = new List<IStructureReference>();
            this._provisioningMetadata = new List<IStructureReference>();
            this._reportingCategories = new List<IReportingCategoryMutableObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingCategoryMutableCore"/> class. 
        ///   Construct from IReportingCategoryObject Bean
        /// </summary>
        /// <param name="objTarget">The Reporting CategoryObject
        /// </param>
        public ReportingCategoryMutableCore(IReportingCategoryObject objTarget)
            : base(objTarget)
        {
            this._structuralMetadata = new List<IStructureReference>();
            this._provisioningMetadata = new List<IStructureReference>();
            this._reportingCategories = new List<IReportingCategoryMutableObject>();

            if (objTarget.StructuralMetadata != null)
            {
                foreach (ICrossReference crossReference in objTarget.StructuralMetadata)
                {
                    this._structuralMetadata.Add(crossReference.CreateMutableInstance());
                }
            }

            if (objTarget.ProvisioningMetadata != null)
            {
                foreach (ICrossReference p in objTarget.ProvisioningMetadata)
                {
                    this._provisioningMetadata.Add(p.CreateMutableInstance());
                }
            }

            if (objTarget.Items != null)
            {
                foreach (IReportingCategoryObject reportingCategoryObject in objTarget.Items)
                {
                    this._reportingCategories.Add(new ReportingCategoryMutableCore(reportingCategoryObject));
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the items.
        /// </summary>
        public IList<IReportingCategoryMutableObject> Items
        {
            get
            {
                return this._reportingCategories;
            }
       }

        /// <summary>
        ///   Gets the provisioning metadata.
        /// </summary>
        public IList<IStructureReference> ProvisioningMetadata
        {
            get
            {
                return this._provisioningMetadata;
            }

            set
            {
                this._provisioningMetadata = value;
            }
        }

        /// <summary>
        ///   Gets the structural metadata.
        /// </summary>
        public IList<IStructureReference> StructuralMetadata
        {
            get
            {
                return this._structuralMetadata;
            }

            set
            {
                this._structuralMetadata = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add item.
        /// </summary>
        /// <param name="item">
        /// The reporting category. 
        /// </param>
        public void AddItem(IReportingCategoryMutableObject item)
        {
            this._reportingCategories.Add(item);
        }

        #endregion
    }
}