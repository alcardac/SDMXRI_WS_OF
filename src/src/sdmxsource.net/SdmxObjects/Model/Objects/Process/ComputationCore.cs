// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComputationBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The computation core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Process
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Util;

    using ProcessStepType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ProcessStepType;

    /// <summary>
    ///   The computation core.
    /// </summary>
    [Serializable]
    public class ComputationCore : AnnotableCore, IComputationObject
    {
        #region Fields

        /// <summary>
        ///   The description.
        /// </summary>
        private readonly IList<ITextTypeWrapper> description;

        /// <summary>
        ///   The local id.
        /// </summary>
        private readonly string localId;

        /// <summary>
        ///   The software language.
        /// </summary>
        private readonly string softwareLanguage;

        /// <summary>
        ///   The software package.
        /// </summary>
        private readonly string softwarePackage;

        /// <summary>
        ///   The software version.
        /// </summary>
        private readonly string softwareVersion;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputationCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="mutableObject">
        /// The mutable object. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ComputationCore(IIdentifiableObject parent, IComputationMutableObject mutableObject)
            : base(mutableObject, parent)
        {
            this.description = new List<ITextTypeWrapper>();
            this.localId = mutableObject.LocalId;
            this.softwareLanguage = mutableObject.SoftwareLanguage;
            this.softwarePackage = mutableObject.SoftwarePackage;
            this.softwareVersion = mutableObject.SoftwareVersion;
            if (mutableObject.Descriptions != null)
            {
                foreach (ITextTypeWrapperMutableObject currentTT in mutableObject.Descriptions)
                {
                    if (!string.IsNullOrWhiteSpace(currentTT.Value))
                    {
                        this.description.Add(new TextTypeWrapperImpl(currentTT, this));
                    }
                }
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputationCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="xmlType">
        /// The xml type. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ComputationCore(IIdentifiableObject parent, ComputationType xmlType)
            : base(xmlType, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Computation), parent)
        {
            this.description = new List<ITextTypeWrapper>();
            this.localId = xmlType.localID;
            this.softwareLanguage = xmlType.softwareLanguage;
            this.softwarePackage = xmlType.softwarePackage;
            this.softwareVersion = xmlType.softwareVersion;
            this.description = TextTypeUtil.WrapTextTypeV21(xmlType.Description, this);
            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.0 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputationCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="process">
        /// The process. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ComputationCore(IIdentifiableObject parent, ProcessStepType process)
            : base(null, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Computation), parent)
        {
            this.description = new List<ITextTypeWrapper>();
            if (process.Computation != null)
            {
                this.description = TextTypeUtil.WrapTextTypeV2(process.Computation, this);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the description.
        /// </summary>
        public virtual IList<ITextTypeWrapper> Description
        {
            get
            {
                return new List<ITextTypeWrapper>(this.description);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///   Gets the local id.
        /// </summary>
        public virtual string LocalId
        {
            get
            {
                return this.localId;
            }
        }

        /// <summary>
        ///   Gets the software language.
        /// </summary>
        public virtual string SoftwareLanguage
        {
            get
            {
                return this.softwareLanguage;
            }
        }

        /// <summary>
        ///   Gets the software package.
        /// </summary>
        public virtual string SoftwarePackage
        {
            get
            {
                return this.softwarePackage;
            }
        }

        /// <summary>
        ///   Gets the software version.
        /// </summary>
        public virtual string SoftwareVersion
        {
            get
            {
                return this.softwareVersion;
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
                var that = (IComputationObject)sdmxObject;
                if (!this.Equivalent(this.description, that.Description, includeFinalProperties))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.localId, that.LocalId))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.softwarePackage, that.SoftwarePackage))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.softwareLanguage, that.SoftwareLanguage))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.softwareVersion, that.SoftwareVersion))
                {
                    return false;
                }

                return this.DeepEqualsInternalAnnotable(that, includeFinalProperties);
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
            if (this.description == null)
            {
                throw new SdmxSemmanticException("Computation missing mandatory field 'description'");
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
            base.AddToCompositeSet(this.description, composites);
            return composites;
       }

        #endregion
    }
}