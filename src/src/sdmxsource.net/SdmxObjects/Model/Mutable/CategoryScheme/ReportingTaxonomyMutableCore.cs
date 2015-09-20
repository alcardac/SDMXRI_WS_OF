// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportingTaxonomyMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The reporting taxonomy mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.CategoryScheme
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.CategoryScheme;

    /// <summary>
    ///   The reporting taxonomy mutable core.
    /// </summary>
    [Serializable]
    public class ReportingTaxonomyMutableCore :
        ItemSchemeMutableCore<IReportingCategoryMutableObject, IReportingCategoryObject, IReportingTaxonomyObject>, 
        IReportingTaxonomyMutableObject
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ReportingTaxonomyMutableCore" /> class.
        /// </summary>
        public ReportingTaxonomyMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingTaxonomy))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingTaxonomyMutableCore"/> class.
        /// </summary>
        /// <param name="reportingTaxonomy">
        /// The reportingTaxonomy. 
        /// </param>
        public ReportingTaxonomyMutableCore(IReportingTaxonomyObject reportingTaxonomy)
            : base(reportingTaxonomy)
        {
            // make into a Category mutable list
            if (reportingTaxonomy.Items != null)
            {
                foreach (IReportingCategoryObject categoryObject in reportingTaxonomy.Items)
                {
                    this.AddReportingCategory(new ReportingCategoryMutableCore(categoryObject));
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override IReportingTaxonomyObject ImmutableInstance
        {
            get
            {
                return new ReportingTaxonomyObjectCore(this);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add reporting category.
        /// </summary>
        /// <param name="category">
        /// The category. 
        /// </param>
        public void AddReportingCategory(IReportingCategoryMutableObject category)
        {
            this.AddItem(category);
        }

        #endregion

        #region Overrides of ItemSchemeMutableCore<IReportingCategoryMutableObject,IReportingCategoryObject,IReportingTaxonomyObject>

        public override IReportingCategoryMutableObject CreateItem(string id, string name)
        {
            IReportingCategoryMutableObject rcMut = new ReportingCategoryMutableCore();
            rcMut.Id = id;
            rcMut.AddName("en", name);
            AddItem(rcMut);
            return rcMut;
        }

        #endregion
    }
}