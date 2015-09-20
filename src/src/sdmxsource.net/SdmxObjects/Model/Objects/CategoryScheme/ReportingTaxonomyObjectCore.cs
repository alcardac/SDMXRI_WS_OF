// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportingTaxonomyBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The reporting taxonomy object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.CategoryScheme
{
    using System;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    using CategoryType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.CategoryType;

    /// <summary>
    ///   The reporting taxonomy object core.
    /// </summary>
    [Serializable]
    public class ReportingTaxonomyObjectCore :
        ItemSchemeObjectCore<IReportingCategoryObject, IReportingTaxonomyObject, IReportingTaxonomyMutableObject, 
            IReportingCategoryMutableObject>, 
        IReportingTaxonomyObject
    {
        #region Static Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private static readonly ILog LOG = LogManager.GetLogger(typeof(ReportingTaxonomyObjectCore));

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingTaxonomyObjectCore"/> class.
        /// </summary>
        /// <param name="reportingTaxonomy">
        /// The reporting taxonomy. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ReportingTaxonomyObjectCore(IReportingTaxonomyMutableObject reportingTaxonomy)
            : base(reportingTaxonomy)
        {
            LOG.Debug("Building IReportingTaxonomyObject from Mutable Object");
            try
            {
                if (reportingTaxonomy.Items != null)
                {
                    foreach (IReportingCategoryMutableObject currentcategory in reportingTaxonomy.Items)
                    {
                        this.AddInternalItem(new ReportingCategoryCore(this, currentcategory));
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

            if (LOG.IsDebugEnabled)
            {
                LOG.Debug("IReportingTaxonomyObject Built " + this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingTaxonomyObjectCore"/> class.
        /// </summary>
        /// <param name="reportingTaxonomy">
        /// The reporting taxonomy. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ReportingTaxonomyObjectCore(ReportingTaxonomyType reportingTaxonomy)
            : base(reportingTaxonomy, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingTaxonomy))
        {
            LOG.Debug("Building IReportingTaxonomyObject from 2.1 SDMX");
            try
            {
                if (reportingTaxonomy.Item != null)
                {
                    foreach (ReportingCategory currentcategory in reportingTaxonomy.Item)
                    {
                        this.AddInternalItem(new ReportingCategoryCore(this, currentcategory.Content));
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

            if (LOG.IsDebugEnabled)
            {
                LOG.Debug("IReportingTaxonomyObject Built " + this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingTaxonomyObjectCore"/> class.
        /// </summary>
        /// <param name="reportingTaxonomy">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ReportingTaxonomyObjectCore(Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ReportingTaxonomyType reportingTaxonomy)
            : base(
                reportingTaxonomy, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ReportingTaxonomy), 
                reportingTaxonomy.validTo, 
                reportingTaxonomy.validFrom, 
                reportingTaxonomy.version, 
                CreateTertiary(reportingTaxonomy.isFinal), 
                reportingTaxonomy.agencyID, 
                reportingTaxonomy.id, 
                reportingTaxonomy.uri, 
                reportingTaxonomy.Name, 
                reportingTaxonomy.Description, 
                CreateTertiary(reportingTaxonomy.isExternalReference), 
                reportingTaxonomy.Annotations)
        {
            LOG.Debug("Building IReportingTaxonomyObject from 2.0 SDMX");
            try
            {
                if (reportingTaxonomy.Category != null)
                {
                    foreach (CategoryType currentcategory in reportingTaxonomy.Category)
                    {
                        this.AddInternalItem(new ReportingCategoryCore(this, currentcategory));
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

            if (LOG.IsDebugEnabled)
            {
                LOG.Debug("IReportingTaxonomyObject Built " + this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingTaxonomyObjectCore"/> class.
        /// </summary>
        /// <param name="agencyScheme">
        /// The agencyScheme. 
        /// </param>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        private ReportingTaxonomyObjectCore(IReportingTaxonomyObject agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
            LOG.Debug("Stub IReportingTaxonomyObject Built");
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public override IReportingTaxonomyMutableObject MutableInstance
        {
            get
            {
                return new ReportingTaxonomyMutableCore(this);
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

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP VALIDATION                         //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
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
            if (sdmxObject == null) return false;

            if (sdmxObject.StructureType == this.StructureType)
            {
                return this.DeepEqualsInternal((IReportingTaxonomyObject)sdmxObject, includeFinalProperties);
            }

            return false;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////GETTERS                                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The get stub.
        /// </summary>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        /// <returns>
        /// The <see cref="IReportingTaxonomyObject"/> . 
        /// </returns>
        public override IReportingTaxonomyObject GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return new ReportingTaxonomyObjectCore(this, actualLocation, isServiceUrl);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        private void Validate()
        {
        }

        #endregion
    }
}