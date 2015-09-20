// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorisationBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The categorisation object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.CategoryScheme
{
    using System;
    using System.Globalization;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///   The categorisation object core.
    /// </summary>
    [Serializable]
    public class CategorisationObjectCore : MaintainableObjectCore<ICategorisationObject, ICategorisationMutableObject>, 
                                            ICategorisationObject
    {
        #region Static Fields

        /// <summary>
        ///   The _sdmx structure.
        /// </summary>
        private static readonly SdmxStructureType _sdmxStructure =
            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Categorisation);

        #endregion

        #region Fields

        /// <summary>
        ///   The category reference.
        /// </summary>
        private readonly ICrossReference categoryReference;

        /// <summary>
        ///   The structure reference.
        /// </summary>
        private readonly ICrossReference structureReference;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT              //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorisationObjectCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public CategorisationObjectCore(ICategorisationMutableObject itemMutableObject)
            : base(itemMutableObject)
        {
            try
            {
                if (itemMutableObject.CategoryReference != null)
                {
                    this.categoryReference = new CrossReferenceImpl(this, itemMutableObject.CategoryReference);
                }

                if (itemMutableObject.StructureReference != null)
                {
                    this.structureReference = new CrossReferenceImpl(this, itemMutableObject.StructureReference);
                }
            }
            catch(SdmxSemmanticException ex) {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
		    }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex1)
            {
                throw new SdmxSemmanticException(ex1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th1)
            {
                throw new SdmxException(th1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorisationObjectCore"/> class.
        /// </summary>
        /// <param name="cs">
        /// The cs. 
        /// </param>
        public CategorisationObjectCore(CategorisationType cs)
            : base(cs, _sdmxStructure)
        {
            this.structureReference = RefUtil.CreateReference(this, cs.Source);
            this.categoryReference = RefUtil.CreateReference(this, cs.Target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <param name="mdf"></param>
        public CategorisationObjectCore(ICategoryObject category, MetadataflowRefType mdf)
            : base(null, _sdmxStructure, null, null, 
				null,
                null, category.MaintainableParent.AgencyId, null, null,
                null,
                null,
                null,
                null)
        {
            try
            {
                this.name = category.Names;
                if (mdf.URN != null)
                {
                    this.structureReference = new CrossReferenceImpl(this, mdf.URN);
                }
                else
                {
                    this.structureReference = new CrossReferenceImpl(this, mdf.AgencyID, mdf.MetadataflowID, mdf.Version, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.MetadataFlow));
                }
                this.categoryReference = new CrossReferenceImpl(this, category.Urn);
                this.GenerateId();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
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

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorisationObjectCore"/> class. 
        ///   Constructs a categorisation from a category that contains a dataflow ref
        /// </summary>
        /// <param name="category">Category object
        /// </param>
        /// <param name="df">DataFlow object
        /// </param>
        public CategorisationObjectCore(ICategoryObject category, DataflowRefType df)
            : base(
                null, 
                _sdmxStructure, 
                null, 
                null, 
                null, 
                null, 
                category.MaintainableParent.AgencyId, 
                null, 
                null, 
                null, 
                null, 
                null, 
                null)
        {
            try
            {
                this.Names = category.Names;
                if (df.URN != null)
                {
                    this.structureReference = new CrossReferenceImpl(this, df.URN);
                }
                else
                {
                    this.structureReference = new CrossReferenceImpl(
                        this, 
                        df.AgencyID, 
                        df.DataflowID, 
                        df.Version, 
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dataflow));
                }

                this.categoryReference = new CrossReferenceImpl(this, category.Urn);
                this.GenerateId();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
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

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorisationObjectCore"/> class. 
        ///   Constructs a cateogrisation from a dataflow that contains a category ref
        /// </summary>
        /// <param name="referencedFrom">Maintainable object
        /// </param>
        /// <param name="currentRef">
        /// The current Ref. 
        /// </param>
        public CategorisationObjectCore(IMaintainableObject referencedFrom, CategoryRefType currentRef)
            : base(
                null, 
                _sdmxStructure, 
                null, 
                null, 
                null, 
                null, 
                referencedFrom.AgencyId, 
                null, 
                null, 
                null, 
                null, 
                null, 
                null)
        {
            try
            {
                this.Names = referencedFrom.Names;
                this.structureReference = new CrossReferenceImpl(this, referencedFrom.Urn);
                this.categoryReference = RefUtil.CreateCategoryRef(this, currentRef);
                this.GenerateId();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
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

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorisationObjectCore"/> class.
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
        private CategorisationObjectCore(ICategorisationObject agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
        }

        #endregion

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
        ///   Gets the category reference.
        /// </summary>
        public virtual ICrossReference CategoryReference
        {
            get
            {
                return this.categoryReference;
            }
        }

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public override ICategorisationMutableObject MutableInstance
        {
            get
            {
                return new CategorisationMutableCore(this);
            }
        }

        /// <summary>
        ///   Gets the structure reference.
        /// </summary>
        public virtual ICrossReference StructureReference
        {
            get
            {
                return this.structureReference;
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
            if (sdmxObject == null) return false;

            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (ICategorisationObject)sdmxObject;
                if (!this.Equivalent(this.categoryReference, that.CategoryReference))
                {
                    return false;
                }

                if (!this.Equivalent(this.structureReference, that.StructureReference))
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

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
        /// The <see cref="ICategorisationObject"/> . 
        /// </returns>
        public override ICategorisationObject GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return new CategorisationObjectCore(this, actualLocation, isServiceUrl);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The validate id.
        /// </summary>
        /// <param name="startWithIntAllowed">
        /// The start with int allowed. 
        /// </param>
        protected internal override void ValidateId(bool startWithIntAllowed)
        {
            // Do nothing yet, not yet fully built
        }

        /// <summary>
        ///   The generate id.
        /// </summary>
        private void GenerateId()
        {
            this.Id = string.Format(
                CultureInfo.InvariantCulture, 
                "{0}_{1}", 
                this.categoryReference.GetHashCode(), 
                this.structureReference.GetHashCode());
        }

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (!IsExternalReference.IsTrue)
            {
                if (this.structureReference == null)
                {
                    throw new SdmxSemmanticException(
                        ExceptionCode.ObjectMissingRequiredElement, this.StructureType, "StructureReference");
                }

                if (this.categoryReference == null)
                {
                    throw new SdmxSemmanticException(
                        ExceptionCode.ObjectMissingRequiredElement, this.StructureType, "CategoryReference");
                }
            }

            base.ValidateId(true);
            base.ValidateNameableAttributes();
        }

        protected override void ValidateNameableAttributes()
        {
            //Do nothing yet, not yet fully built
        }

        #endregion
    }
}